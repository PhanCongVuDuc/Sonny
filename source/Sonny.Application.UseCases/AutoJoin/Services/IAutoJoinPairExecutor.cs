using Sonny.Application.UseCases.AutoJoin.Models ;

namespace Sonny.Application.UseCases.AutoJoin.Services ;

/// <summary>
///     Output port: geometry checks and the actual join/unjoin calls for one pair of elements
///     (ADR 0001 — plain ids in, results out). Implemented in Infrastructure; must be called
///     inside an open transaction.
/// </summary>
public interface IAutoJoinPairExecutor
{
    /// <summary>
    ///     Takes a snapshot of the anchor element's solid. Called once per outer-loop element,
    ///     BEFORE its pairs execute — joins made while processing the pairs must not change the
    ///     solid the checks compare against (ported behaviour)
    /// </summary>
    /// <param name="anchorElementId">The element whose solid anchors the following checks</param>
    void BeginAnchor(long anchorElementId) ;

    /// <summary>
    ///     Checks whether the element's solid intersects the current anchor's solid snapshot
    /// </summary>
    /// <param name="otherElementId">The candidate element</param>
    /// <returns>The outcome the interactor maps to join/skip decisions</returns>
    SolidIntersectCheck CheckIntersectWithAnchor(long otherElementId) ;

    /// <summary>
    ///     Joins (or unjoins) one pair. Never throws — a Revit failure returns <c>false</c>
    /// </summary>
    /// <param name="priorityElementId">The element that should cut</param>
    /// <param name="targetElementId">The element that should be cut</param>
    /// <param name="isJoin"><c>false</c> unjoins the pair instead</param>
    /// <param name="isReverse">Reverses the cut order after joining</param>
    /// <returns><c>true</c> when the pair was processed without a Revit failure</returns>
    bool TryExecuteJoin(long priorityElementId,
        long targetElementId,
        bool isJoin,
        bool isReverse) ;
}
