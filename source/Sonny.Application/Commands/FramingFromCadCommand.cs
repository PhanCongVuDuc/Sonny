using Autodesk.Revit.Attributes ;
using Autodesk.Revit.UI ;
using Sonny.Application.Bases ;
using Sonny.Application.Presentation.FramingFromCad.Views ;

namespace Sonny.Application.Commands ;

[Transaction(TransactionMode.Manual)]
public class FramingFromCadCommand : BaseExternalCommand
{
    /// <summary>
    ///     Executes the command logic. Resolving the view builds the context, which is what asks the user
    ///     to pick a CAD link — so an ESC on that pick fails here and the dialog never opens
    /// </summary>
    protected override Result ExecuteInternal(ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        var view = Host.GetService<FramingFromCadView>() ;
        view.Show() ;

        return Result.Succeeded ;
    }
}
