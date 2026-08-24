using System.Windows.Threading ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Views ;

namespace Sonny.Application.Presentation.Implements ;

/// <summary>
///     Implementation of IProgressReporter using ProgressView window
/// </summary>
public class ProgressReporter : IProgressReporter
{
    private ProgressView? _progressView ;

    /// <summary>
    ///     Shows the progress window with the specified title
    /// </summary>
    /// <param name="title">Title to display on progress window</param>
    /// <param name="allowCancel">Shows a Cancel button that raises <see cref="IsCancelRequested" /></param>
    public void Show(string title,
        bool allowCancel = false)
    {
        if (Dispatcher.CurrentDispatcher.CheckAccess()) {
            _progressView = new ProgressView(title,
                allowCancel) ;
            _progressView.Show() ;
        }
        else {
            Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    _progressView = new ProgressView(title,
                        allowCancel) ;
                    _progressView.Show() ;
                },
                DispatcherPriority.Normal) ;
        }
    }

    /// <summary>
    ///     Whether the user pressed Cancel on the progress window
    /// </summary>
    public bool IsCancelRequested => _progressView?.IsCancelRequested ?? false ;

    /// <summary>
    ///     Updates the progress indicator
    /// </summary>
    /// <param name="current">Current progress value</param>
    /// <param name="total">Total progress value</param>
    public void Update(int current,
        int total) =>
        _progressView?.UpdateProgress(current,
            total) ;

    /// <summary>
    ///     Closes the progress window
    /// </summary>
    public void Close()
    {
        if (_progressView == null) {
            return ;
        }

        if (_progressView.Dispatcher.CheckAccess()) {
            _progressView.Close() ;
            _progressView = null ;
        }
        else {
            _progressView.Dispatcher.Invoke(() =>
                {
                    _progressView?.Close() ;
                    _progressView = null ;
                },
                DispatcherPriority.Normal) ;
        }
    }
}
