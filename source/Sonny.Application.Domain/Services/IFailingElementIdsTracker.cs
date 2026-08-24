namespace Sonny.Application.Domain.Services ;

/// <summary>
///     Collects the element ids that caused error-severity failures during a transaction commit,
///     so the caller can report them after the commit. Cleared explicitly at the start of each
///     run — the tracker itself is a singleton shared with the failure preprocessors.
/// </summary>
public interface IFailingElementIdsTracker
{
    /// <summary>
    ///     Clears the ids collected by the previous run
    /// </summary>
    void Clear() ;

    /// <summary>
    ///     Records element ids that caused an error-severity failure
    /// </summary>
    /// <param name="elementIds">Numeric element ids</param>
    void AddRange(IEnumerable<long> elementIds) ;

    /// <summary>
    ///     Gets the ids collected since the last <see cref="Clear" />
    /// </summary>
    /// <returns>The failing element ids, in the order they were recorded</returns>
    IReadOnlyList<long> GetFailingElementIds() ;
}
