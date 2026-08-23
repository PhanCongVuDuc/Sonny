using Sonny.Application.Presentation.Extensions ;
using Sonny.Application.Presentation.FramingFromCad.ViewModels ;

namespace Sonny.Application.Presentation.FramingFromCad.Views ;

public partial class FramingFromCadView
{
    public FramingFromCadView(FramingFromCadViewModel viewModel)
    {
        InitializeComponent() ;
        this.SetOwnerByRevit() ;
        DataContext = viewModel ;

        // Set close window action
        viewModel.Window = this ;
    }
}
