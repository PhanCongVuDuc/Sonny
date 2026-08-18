using Autodesk.Revit.UI ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.RevitExtensions.Extensions ;

namespace Sonny.Application.Infrastructure.Revit.Implements ;

public class RevitDocument(IUIDocumentProvider uiDocumentProvider) : IRevitDocument
{
    public Document Document => UIDocument.Document ;

    public UIDocument UIDocument => uiDocumentProvider.GetUIDocument() ;

    public View ActiveView => UIDocument.ActiveView ;

    public UIApplication Application => UIDocument.Application ;
}
