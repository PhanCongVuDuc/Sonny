using Sonny.Application.Domain.Services ;

namespace Sonny.Application.Infrastructure.Revit.Implements ;

/// <summary>
///     Singleton store for the element ids behind error-severity failures of the current run.
///     Written by the failure preprocessors during commit, read by the interactor afterwards.
///     Holds only numeric ids, never Revit objects — safe to keep for the whole session.
/// </summary>
public class FailingElementIdsTracker : IFailingElementIdsTracker
{
    private readonly List<long> _failingElementIds = [] ;
    private readonly object _lock = new() ;

    public void Clear()
    {
        lock (_lock) {
            _failingElementIds.Clear() ;
        }
    }

    public void AddRange(IEnumerable<long> elementIds)
    {
        lock (_lock) {
            _failingElementIds.AddRange(elementIds) ;
        }
    }

    public IReadOnlyList<long> GetFailingElementIds()
    {
        lock (_lock) {
            return _failingElementIds.ToList() ;
        }
    }
}
