namespace Sonny.Application.Domain.Services ;

/// <summary>
///     Interface for reporting progress of long-running operations
/// </summary>
public interface IProgressReporter
{
    /// <summary>
    ///     Shows the progress window with the specified title
    /// </summary>
    /// <param name="title">Title to display on progress window</param>
    /// <param name="allowCancel">Shows a Cancel button that raises <see cref="IsCancelRequested" /></param>
    void Show(string title,
        bool allowCancel = false) ;

    /// <summary>
    ///     Whether the user pressed Cancel on the progress window. Callers poll this between work
    ///     items and decide their own rollback policy — the reporter never aborts anything itself
    /// </summary>
    bool IsCancelRequested { get ; }

    /// <summary>
    ///     Updates the progress indicator
    /// </summary>
    /// <param name="current">Current progress value</param>
    /// <param name="total">Total progress value</param>
    void Update(int current,
        int total) ;

    /// <summary>
    ///     Closes the progress window
    /// </summary>
    void Close() ;
}
