using System.Windows.Threading ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Views ;

namespace Sonny.Application.Presentation.Implements ;

public class ProgressReporter : IProgressReporter
{
    private ProgressView? _progressView ;

    public void Show(string title)
    {
        if (Dispatcher.CurrentDispatcher.CheckAccess()) {
            _progressView = new ProgressView(title) ;
            _progressView.Show() ;
        }
        else {
            Dispatcher.CurrentDispatcher.Invoke(() =>
                {
                    _progressView = new ProgressView(title) ;
                    _progressView.Show() ;
                },
                DispatcherPriority.Normal) ;
        }
    }

    public void Update(int current,
        int total) =>
        _progressView?.UpdateProgress(current,
            total) ;

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
