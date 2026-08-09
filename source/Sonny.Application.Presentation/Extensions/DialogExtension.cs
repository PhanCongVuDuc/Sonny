using System.Windows ;
using System.Windows.Interop ;
using Autodesk.Windows ;

namespace Sonny.Application.Presentation.Extensions ;

public static class DialogExtension
{
    public static void SetOwnerByRevit(this Window window)
    {
        if (HwndSource.FromHwnd(ComponentManager.ApplicationWindow)
                ?.RootVisual is Window mainWindow) {
            window.Owner = mainWindow ;
        }
    }
}
