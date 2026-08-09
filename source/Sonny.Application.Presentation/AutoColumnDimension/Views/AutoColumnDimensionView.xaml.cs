using System.Windows ;
using Sonny.Application.Presentation.AutoColumnDimension.ViewModels ;
using Sonny.Application.Presentation.Extensions ;

namespace Sonny.Application.Presentation.AutoColumnDimension.Views ;

public partial class AutoColumnDimensionView : Window
{
    public AutoColumnDimensionView(AutoColumnDimensionViewModel viewModel)
    {
        InitializeComponent() ;
        this.SetOwnerByRevit() ;
        DataContext = viewModel ;

        // Set close window action
        viewModel.Window = this ;
    }
}
