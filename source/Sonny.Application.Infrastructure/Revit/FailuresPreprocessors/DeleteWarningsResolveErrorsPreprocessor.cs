namespace Sonny.Application.Infrastructure.Revit.FailuresPreprocessors ;

/// <summary>
///     Deletes warnings from the failure dialog, resolves errors, and rolls back on document
///     corruption. Ported from AlphaBIM's WarningDeleteWarning — used by AutoJoin when the user
///     unjoins or accepts warnings. Deliberately does NOT track failing element ids: the original
///     never read them on this path (docs/features/AutoJoin.md)
/// </summary>
public class DeleteWarningsResolveErrorsPreprocessor : IFailuresPreprocessor
{
    public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
    {
        var failures = failuresAccessor.GetFailureMessages() ;
        if (failures.Count == 0) {
            return FailureProcessingResult.Continue ;
        }

        foreach (var failure in failures) {
            var severity = failure.GetSeverity() ;
            if (severity == FailureSeverity.Warning) {
                failuresAccessor.DeleteWarning(failure) ;
            }
            else if (severity == FailureSeverity.Error) {
                failuresAccessor.ResolveFailure(failure) ;
            }
            else if (severity == FailureSeverity.DocumentCorruption) {
                return FailureProcessingResult.ProceedWithRollBack ;
            }
        }

        return FailureProcessingResult.ProceedWithCommit ;
    }
}
