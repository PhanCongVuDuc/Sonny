using Autodesk.Revit.Attributes ;
using Autodesk.Revit.UI ;
using Sonny.Application.Bases ;
using Sonny.Application.Presentation.ColumnFromCad.Views ;

namespace Sonny.Application.Commands ;

[Transaction(TransactionMode.Manual)]
public class ColumnFromCadCommand : BaseExternalCommand
{
    protected override Result ExecuteInternal(ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        var view = Host.GetService<ColumnFromCadView>() ;
        view.Show() ;

        return Result.Succeeded ;
    }
}
