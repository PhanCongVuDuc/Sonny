using System.Windows ;
using Sonny.Application.Presentation.AutoJoin.ViewModels ;
using Sonny.Application.Presentation.Extensions ;

namespace Sonny.Application.Presentation.AutoJoin.Views ;

/// <summary>
///     Interaction logic for AutoJoinView.xaml
/// </summary>
public partial class AutoJoinView : Window
{
    /// <summary>
    ///     Initializes a new instance of AutoJoinView
    /// </summary>
    /// <param name="viewModel">The view model</param>
    public AutoJoinView(AutoJoinViewModel viewModel)
    {
        InitializeComponent() ;
        this.SetOwnerByRevit() ;
        DataContext = viewModel ;

        // Set close window action
        viewModel.Window = this ;
    }
}
