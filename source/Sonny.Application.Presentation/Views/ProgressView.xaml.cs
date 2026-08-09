using System.Windows ;
using System.Windows.Threading ;
using Sonny.Application.Presentation.Extensions ;

namespace Sonny.Application.Presentation.Views ;

public partial class ProgressView : Window
{
    private readonly string _title ;

    public ProgressView(string title)
    {
        _title = title ;
        InitializeComponent() ;
        this.SetOwnerByRevit() ;

        // Initialize progress bar
        ProgressBar.Minimum = 0 ;
        ProgressBar.Maximum = 100 ;
        ProgressBar.Value = 0 ;
    }

    public void UpdateProgress(int current,
        int total) =>
        Dispatcher.Invoke(() =>
            {
                ProgressBar.Maximum = total ;
                ProgressBar.Value = current ;
                Title = $"{_title} ({current} / {total})" ;
            },
            DispatcherPriority.Background) ;
}
