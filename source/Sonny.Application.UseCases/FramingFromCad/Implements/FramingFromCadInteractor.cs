using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.FramingFromCad.Services ;
using Sonny.Application.Domain.Exceptions ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.FramingFromCad.Services ;

namespace Sonny.Application.UseCases.FramingFromCad.Implements ;

/// <summary>
///     Ported from the original implementation. Two things about the shape are deliberate and
///     differ from <c>ColumnFromCadInteractor</c>, so do not "align" them without changing the contract:
///     the whole beam pass sits in <b>one</b> transaction (Cancel must leave the model untouched), and
///     justification runs in a <b>second</b> transaction after that one commits (FacingOrientation only
///     tells the truth once Revit has regenerated the instance)
/// </summary>
public class FramingFromCadInteractor(
    IFramingDataExtractor framingDataExtractor,
    IBeamCreator beamCreator,
    IBeamJustificationAdjuster beamJustificationAdjuster,
    IResourceHelper resourceHelper,
    ITransactionManagerFactory transactionManagerFactory,
    IProgressReporter progressReporter,
    IMessageService messageService,
    IElementSelector elementSelector,
    IRevitTaskRunner revitTaskRunner) : IFramingFromCadInteractor
{
    public async Task Execute(FramingCreationContext input)
    {
        // The creator remembers the first symbol it resolves; clear it or the previous run's size is
        // what single-stroke beams get built at
        beamCreator.Reset() ;

        var extraction = await revitTaskRunner.RunAsync(() => framingDataExtractor.Extract(input)) ;

        var outcome = await revitTaskRunner.RunAsync(() => CreateFramings(input,
            extraction)) ;

        if (outcome.WasCancelled) {
            messageService.ShowInfo(resourceHelper.GetString("MessageCancelled")) ;
            return ;
        }

        if (outcome.ParameterMissing is { } parameterMissing) {
            messageService.ShowError(resourceHelper.GetString("MessageParameterNotFound",
                parameterMissing.WidthParameter ?? string.Empty,
                parameterMissing.HeightParameter ?? string.Empty)) ;
            return ;
        }

        // Second transaction: FacingOrientation is only meaningful after the beams above committed
        if (outcome.PairedBeams.Count > 0) {
            await revitTaskRunner.RunAsync(() => AdjustJustification(outcome.PairedBeams)) ;
        }

        if (outcome.CreatedIds.Count == 0) {
            messageService.ShowWarning(resourceHelper.GetString("MessageNoFramingsCreated")) ;
            return ;
        }

        await revitTaskRunner.RunAsync(() => elementSelector.SelectElements(outcome.CreatedIds)) ;

        messageService.ShowInfo(resourceHelper.GetString("MessageSuccessfullyCreated",
            outcome.CreatedIds.Count)) ;
    }

    /// <summary>
    ///     Builds every beam inside one transaction. Returns rather than throws for the two outcomes the
    ///     user gets a message about; lets everything else escape
    /// </summary>
    private FramingRunOutcome CreateFramings(FramingCreationContext context,
        FramingExtractionResult extraction)
    {
        var createdIds = new List<string>() ;
        var pairedBeams = new List<CreatedPairedBeam>() ;
        var total = extraction.Pairs.Count + extraction.SingleLines.Count ;
        var current = 0 ;

        progressReporter.Show(resourceHelper.GetString("MessageCreatingFramings"),
            true) ;

        try {
            using var transactionManager = transactionManagerFactory.Create(
                resourceHelper.GetString("TransactionCreateFramings"),
                [FailurePreprocessorType.SuppressWarnings]) ;
            transactionManager.Start() ;

            foreach (var pair in extraction.Pairs) {
                if (progressReporter.IsCancelRequested) {
                    transactionManager.RollBack() ;

                    return FramingRunOutcome.Cancelled() ;
                }

                progressReporter.Update(++current,
                    total) ;

                try {
                    if (beamCreator.CreatePairedBeam(pair,
                            context) is not { } uniqueId) {
                        continue ;
                    }

                    createdIds.Add(uniqueId) ;
                    pairedBeams.Add(new CreatedPairedBeam {
                        UniqueId = uniqueId,
                        Normal = pair.Normal,
                    }) ;
                }
                catch (FramingParameterMissingException parameterMissing) {
                    // Not a per-beam problem: without those parameters every remaining beam would be
                    // wrong the same way, so the run stops and the transaction rolls back on dispose
                    return FramingRunOutcome.MissingParameter(parameterMissing) ;
                }
                catch (Exception) {
                    // Silent per-pair skip, kept from the original: the beam is dropped and the run
                    // reports a count that says nothing about what went missing
                }
            }

            foreach (var singleLine in extraction.SingleLines) {
                if (progressReporter.IsCancelRequested) {
                    transactionManager.RollBack() ;

                    return FramingRunOutcome.Cancelled() ;
                }

                progressReporter.Update(++current,
                    total) ;

                // No catch here on purpose. The original crashed when the paired pass had resolved no
                // symbol, and that behaviour is the contract — the exception escapes and the
                // transaction rolls back undisposed. See
                // docs/bugs/FFC-001-single-line-pass-crashes-when-no-symbol-resolved.md
                createdIds.Add(beamCreator.CreateSingleLineBeam(singleLine,
                    context)) ;
            }

            transactionManager.Commit() ;

            return FramingRunOutcome.Completed(createdIds,
                pairedBeams) ;
        }
        finally {
            progressReporter.Close() ;
        }
    }

    /// <summary>
    ///     The justification pass, in its own transaction
    /// </summary>
    private void AdjustJustification(IReadOnlyList<CreatedPairedBeam> pairedBeams)
    {
        using var transactionManager = transactionManagerFactory.Create(
            resourceHelper.GetString("TransactionAdjustFramingJustification"),
            [FailurePreprocessorType.SuppressWarnings]) ;
        transactionManager.Start() ;

        beamJustificationAdjuster.Adjust(pairedBeams) ;

        transactionManager.Commit() ;
    }

    /// <summary>
    ///     What the beam pass ended up doing — exactly one of cancelled, parameter-missing or completed
    /// </summary>
    private sealed class FramingRunOutcome
    {
        public List<string> CreatedIds { get ; private set ; } = [] ;
        public List<CreatedPairedBeam> PairedBeams { get ; private set ; } = [] ;
        public bool WasCancelled { get ; private set ; }
        public FramingParameterMissingException? ParameterMissing { get ; private set ; }

        public static FramingRunOutcome Cancelled() => new() { WasCancelled = true } ;

        public static FramingRunOutcome MissingParameter(FramingParameterMissingException exception) =>
            new() { ParameterMissing = exception } ;

        public static FramingRunOutcome Completed(List<string> createdIds,
            List<CreatedPairedBeam> pairedBeams) =>
            new() { CreatedIds = createdIds, PairedBeams = pairedBeams } ;
    }
}
