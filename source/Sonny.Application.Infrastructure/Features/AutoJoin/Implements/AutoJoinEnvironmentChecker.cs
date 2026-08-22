using Sonny.Application.Infrastructure.Features.AutoJoin.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;

namespace Sonny.Application.Infrastructure.Features.AutoJoin.Implements ;

public class AutoJoinEnvironmentChecker(IRevitDocument revitDocument) : IAutoJoinEnvironmentChecker
{
    public AutoJoinEnvironmentCheck Check()
    {
        if (revitDocument.Document.IsFamilyDocument) {
            return AutoJoinEnvironmentCheck.FamilyDocument ;
        }

        var viewType = revitDocument.ActiveView.ViewType ;
        if (viewType is ViewType.Schedule or ViewType.ColumnSchedule or ViewType.DrawingSheet) {
            return AutoJoinEnvironmentCheck.UnsupportedViewType ;
        }

        return AutoJoinEnvironmentCheck.Ok ;
    }
}
