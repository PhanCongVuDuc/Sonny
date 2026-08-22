using Autodesk.Revit.Attributes ;
using Autodesk.Revit.UI ;
using Sonny.Application.Bases ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Features.AutoJoin.Services ;
using Sonny.Application.Presentation.AutoJoin.Views ;

namespace Sonny.Application.Commands ;

[Transaction(TransactionMode.Manual)]
public class AutoJoinCommand : BaseExternalCommand
{
    protected override Result ExecuteInternal(ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        // F1/F2: reject family documents and schedule/sheet views BEFORE the window opens —
        // asking the user to fill in rules first would violate the contract invariant
        var environmentCheck = Host.GetService<IAutoJoinEnvironmentChecker>()
            .Check() ;
        if (environmentCheck != AutoJoinEnvironmentCheck.Ok) {
            var messageKey = environmentCheck == AutoJoinEnvironmentCheck.FamilyDocument
                ? "MessageAutoJoinFamilyDocument"
                : "MessageAutoJoinUnsupportedView" ;
            Host.GetService<IMessageService>()
                .ShowInfo(Host.GetService<IResourceHelper>()
                    .GetString(messageKey)) ;

            return Result.Cancelled ;
        }

        var view = Host.GetService<AutoJoinView>() ;
        view.Show() ;

        return Result.Succeeded ;
    }
}
