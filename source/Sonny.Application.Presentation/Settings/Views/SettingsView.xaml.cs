using System.Windows ;
using Sonny.Application.Presentation.Settings.ViewModels ;
using Sonny.Application.Presentation.Extensions ;

namespace Sonny.Application.Presentation.Settings.Views ;

public partial class SettingsView : Window
{
    public SettingsView(SettingsViewModel viewModel)
    {
        InitializeComponent() ;
        this.SetOwnerByRevit() ;
        DataContext = viewModel ;

        // Set close window action
        viewModel.Window = this ;
    }
}
