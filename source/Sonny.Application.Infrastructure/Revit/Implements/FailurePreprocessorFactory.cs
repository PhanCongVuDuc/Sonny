using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.FailuresPreprocessors ;
using Sonny.Application.Infrastructure.Revit.Services ;

namespace Sonny.Application.Infrastructure.Revit.Implements ;

public class FailurePreprocessorFactory(IFailingElementIdsTracker failingElementIdsTracker) : IFailurePreprocessorFactory
{
    public IFailuresPreprocessor? CreateComposite(IEnumerable<FailurePreprocessorType> types)
    {
        var validTypes = types.Where(t => t != FailurePreprocessorType.None)
            .ToList() ;
        if (validTypes.Count == 0) {
            return null ;
        }

        if (validTypes.Count == 1) {
            return Create(validTypes[0]) ;
        }

        var composite = CreateCompositeFailurePreprocessor() ;
        foreach (var type in validTypes) {
            var preprocessor = Create(type) ;
            if (preprocessor != null) {
                composite.AddPreprocessor(preprocessor) ;
            }
        }

        return composite ;
    }

    public IFailuresPreprocessor? Create(FailurePreprocessorType type) =>
        type switch
        {
            FailurePreprocessorType.None => null,
            FailurePreprocessorType.SuppressWarnings => CreateSuppressWarningsPreprocessor(),
            FailurePreprocessorType.ResolveAllFailures => new ResolveAllFailuresPreprocessor(failingElementIdsTracker),
            FailurePreprocessorType.DeleteWarningsResolveErrors => new DeleteWarningsResolveErrorsPreprocessor(),
            _ => null
        } ;

    public IFailuresPreprocessor CreateSuppressWarningsPreprocessor() => new SuppressWarningsPreprocessor() ;

    public ICompositeFailurePreprocessor CreateCompositeFailurePreprocessor() => new CompositeFailurePreprocessor() ;
}
