using System.Windows ;
using System.Windows.Threading ;
using Sonny.Application.Presentation.Extensions ;

namespace Sonny.Application.Presentation.Views ;

public partial class ProgressView : Window
{
    private readonly string _title ;

    public ProgressView(string title,
        bool allowCancel = false)
    {
        _title = title ;
        InitializeComponent() ;
        this.SetOwnerByRevit() ;

        // Initialize progress bar
        ProgressBar.Minimum = 0 ;
        ProgressBar.Maximum = 100 ;
        ProgressBar.Value = 0 ;

        if (allowCancel) {
            CancelButton.Visibility = Visibility.Visible ;
        }
    }

    /// <summary>
    ///     Whether the user pressed Cancel. The window only raises the flag — whoever polls it
    ///     owns the rollback policy
    /// </summary>
    public bool IsCancelRequested { get ; private set ; }

    /// <summary>
    ///     Updates progress bar
    /// </summary>
    public void UpdateProgress(int current,
        int total) =>
        Dispatcher.Invoke(() =>
            {
                ProgressBar.Maximum = total ;
                ProgressBar.Value = current ;
                Title = $"{_title} ({current} / {total})" ;
            },
            DispatcherPriority.Background) ;

    private void CancelButton_Click(object sender,
        RoutedEventArgs e)
    {
        IsCancelRequested = true ;
        CancelButton.IsEnabled = false ;
    }
}
