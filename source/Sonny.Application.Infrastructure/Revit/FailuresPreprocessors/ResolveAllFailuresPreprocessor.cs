using Sonny.Application.Domain.Services ;
using Sonny.RevitExtensions.Extensions ;

namespace Sonny.Application.Infrastructure.Revit.FailuresPreprocessors ;

/// <summary>
///     Resolves every failure (warnings included) and records the element ids behind
///     error-severity failures into the tracker, so the caller can select them after commit.
///     Ported from AlphaBIM's WarningResolveFailure — used by AutoJoin's default path (F10)
/// </summary>
public class ResolveAllFailuresPreprocessor(IFailingElementIdsTracker failingElementIdsTracker) : IFailuresPreprocessor
{
    public FailureProcessingResult PreprocessFailures(FailuresAccessor failuresAccessor)
    {
        var failures = failuresAccessor.GetFailureMessages() ;
        if (failures.Count == 0) {
            return FailureProcessingResult.Continue ;
        }

        foreach (var failure in failures) {
            if (failure.GetSeverity() == FailureSeverity.Error) {
                failingElementIdsTracker.AddRange(failure.GetFailingElementIds()
                    .Select(id => id.GetValue())) ;
            }

            failuresAccessor.ResolveFailure(failure) ;
        }

        return FailureProcessingResult.ProceedWithCommit ;
    }
}
