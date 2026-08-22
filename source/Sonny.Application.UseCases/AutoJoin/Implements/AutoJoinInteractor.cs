using Serilog ;
using Sonny.Application.Domain.Entities.AutoJoin.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.AutoJoin.Models ;
using Sonny.Application.UseCases.AutoJoin.Services ;

namespace Sonny.Application.UseCases.AutoJoin.Implements ;

/// <summary>
///     Interactor for the auto-join run. Pure form (ADR 0001): elements come in through
///     IAutoJoinScopeReader, geometry checks and joins go out through IAutoJoinPairExecutor.
///     Behaviour is a faithful port of the original AlphaBIM tool — the silent branches are
///     deliberate and documented as F4/F5/F7 in docs/features/AutoJoin.md.
/// </summary>
public class AutoJoinInteractor(
    IAutoJoinScopeReader scopeReader,
    IAutoJoinPairExecutor pairExecutor,
    IFailingElementIdsTracker failingElementIdsTracker,
    ITransactionManagerFactory transactionManagerFactory,
    IProgressReporter progressReporter,
    IMessageService messageService,
    IResourceHelper resourceHelper,
    ILogger logger) : IAutoJoinInteractor
{
    private const string TransactionName = "Auto Join" ;

    private enum RunOutcome
    {
        Completed,
        Cancelled,
        NoSelection,
        EmptyScope
    }

    public void Execute(AutoJoinInput input)
    {
        logger.Information("Starting Auto Join, mode {Mode}",
            input.Mode) ;

        failingElementIdsTracker.Clear() ;

        // Unjoin and accept-warnings keep warnings visible as warnings; the default resolves
        // everything and tracks the elements behind error-severity failures (F10)
        var preprocessorType = input.IsUnjoin || input.IsAcceptWarnings
            ? FailurePreprocessorType.DeleteWarningsResolveErrors
            : FailurePreprocessorType.ResolveAllFailures ;

        // One transaction around the whole run: one undo step, Cancel rolls back everything (D7)
        using var transaction = transactionManagerFactory.Create(TransactionName,
            [preprocessorType]) ;
        transaction.Start() ;

        RunOutcome outcome ;
        progressReporter.Show(resourceHelper.GetString("AutoJoinProgressTitle"),
            true) ;
        try {
            outcome = input.Mode switch
            {
                AutoJoinMode.CutSelectedElements => RunCutSelectedElements(),
                AutoJoinMode.CutOtherElements => RunCutOtherElements(),
                _ => RunRuleBased(input)
            } ;
        }
        finally {
            progressReporter.Close() ;
        }

        switch (outcome) {
            case RunOutcome.Completed :
                transaction.Commit() ;
                ReportResult(input) ;
                break ;
            case RunOutcome.Cancelled :
                // F11: all-or-nothing — everything joined so far is rolled back
                transaction.RollBack() ;
                messageService.ShowInfo(resourceHelper.GetString("MessageAutoJoinCancelled")) ;
                break ;
            case RunOutcome.NoSelection :
                // F3: dialog first, then roll back — mirrors the original order
                messageService.ShowInfo(resourceHelper.GetString("MessageNoElementSelected")) ;
                transaction.RollBack() ;
                break ;
            case RunOutcome.EmptyScope :
                // F4: deliberately silent — the original says nothing when the view is empty
                transaction.RollBack() ;
                break ;
        }
    }

    private RunOutcome RunCutSelectedElements()
    {
        var selectedIds = scopeReader.GetSelectedSolidElementIds() ;
        if (selectedIds.Count == 0) {
            return RunOutcome.NoSelection ;
        }

        var current = 0 ;
        foreach (var selectedId in selectedIds) {
            if (progressReporter.IsCancelRequested) {
                return RunOutcome.Cancelled ;
            }

            progressReporter.Update(++current,
                selectedIds.Count) ;

            // The selected element is being cut, so its pre-join solid anchors the checks
            pairExecutor.BeginAnchor(selectedId) ;
            foreach (var candidateId in scopeReader.GetBoundingBoxIntersectingInView(selectedId)) {
                ExecutePair(candidateId,
                    selectedId,
                    candidateId,
                    true,
                    false) ;
            }
        }

        return RunOutcome.Completed ;
    }

    private RunOutcome RunCutOtherElements()
    {
        var selectedIds = scopeReader.GetSelectedSolidElementIds() ;
        if (selectedIds.Count == 0) {
            return RunOutcome.NoSelection ;
        }

        var current = 0 ;
        foreach (var selectedId in selectedIds) {
            if (progressReporter.IsCancelRequested) {
                return RunOutcome.Cancelled ;
            }

            progressReporter.Update(++current,
                selectedIds.Count) ;

            pairExecutor.BeginAnchor(selectedId) ;
            foreach (var candidateId in scopeReader.GetBoundingBoxIntersectingInView(selectedId)) {
                ExecutePair(selectedId,
                    candidateId,
                    candidateId,
                    true,
                    false) ;
            }
        }

        return RunOutcome.Completed ;
    }

    private RunOutcome RunRuleBased(AutoJoinInput input)
    {
        // Priority side: the selection when it has solid elements, the whole view otherwise
        var scopeIds = scopeReader.GetSelectedSolidElementIds() ;
        if (scopeIds.Count == 0) {
            scopeIds = scopeReader.GetActiveViewSolidElementIds() ;
        }

        if (scopeIds.Count == 0) {
            return RunOutcome.EmptyScope ;
        }

        foreach (var rule in input.Rules) {
            var priorityIds = scopeReader.GetPriorityElementIds(scopeIds,
                rule.PriorityCategory) ;
            var joinWithIds = scopeReader.GetJoinWithElementIds(rule.JoinWithCategory) ;

            // F5: a rule with nothing on either side is skipped without a word
            if (priorityIds.Count == 0
                || joinWithIds.Count == 0) {
                continue ;
            }

            var current = 0 ;
            foreach (var priorityId in priorityIds) {
                if (progressReporter.IsCancelRequested) {
                    return RunOutcome.Cancelled ;
                }

                progressReporter.Update(++current,
                    priorityIds.Count) ;

                pairExecutor.BeginAnchor(priorityId) ;
                foreach (var targetId in scopeReader.GetBoundingBoxIntersecting(priorityId,
                             joinWithIds)) {
                    ExecutePair(priorityId,
                        targetId,
                        targetId,
                        ! input.IsUnjoin,
                        rule.IsReverse) ;
                }
            }
        }

        return RunOutcome.Completed ;
    }

    private void ExecutePair(long priorityElementId,
        long targetElementId,
        long checkOtherElementId,
        bool isJoin,
        bool isReverse)
    {
        var check = pairExecutor.CheckIntersectWithAnchor(checkOtherElementId) ;
        if (check == SolidIntersectCheck.NotIntersecting) {
            return ; // F7: silent skip
        }

        // NoAnchorSolid (F6), Intersecting, and CheckFailed (F8, D2) all join
        if (! pairExecutor.TryExecuteJoin(priorityElementId,
                targetElementId,
                isJoin,
                isReverse)) {
            // F9 (D3): silent for the user, but logged so it can surface in the UI later
            logger.Warning("Auto Join failed for pair {PriorityElementId} -> {TargetElementId}",
                priorityElementId,
                targetElementId) ;
        }
    }

    private void ReportResult(AutoJoinInput input)
    {
        var failingIds = failingElementIdsTracker.GetFailingElementIds() ;
        if (failingIds.Count > 0) {
            // F10: hand the user the elements Revit complained about
            scopeReader.SelectElements(failingIds) ;
            messageService.ShowInfo(resourceHelper.GetString("MessageCheckFailingElements")) ;
        }
        else if (input.Rules.Count > 0) {
            messageService.ShowInfo(resourceHelper.GetString("MessageAutoJoinSuccess")) ;
        }
    }
}
