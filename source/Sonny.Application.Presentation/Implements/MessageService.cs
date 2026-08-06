using System.Windows ;
using Sonny.Application.Domain.Services ;

namespace Sonny.Application.Presentation.Implements ;

/// <summary>
///     Service for displaying messages to users
/// </summary>
public class MessageService : IMessageService
{
    public const string ErrorTitle = "Error" ;
    public const string InfoTitle = "Information" ;
    public const string WarningTitle = "Warning" ;
    public const string QuestionTitle = "Question" ;

    /// <summary>
    ///     Shows an error message with title
    /// </summary>
    /// <param name="title">The title of the error message</param>
    /// <param name="message">The error message content</param>
    public void ShowError(string title,
        string message) =>
        MessageBox.Show(message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Error) ;

    /// <summary>
    ///     Shows an error message with default title
    /// </summary>
    /// <param name="message">The error message content</param>
    public void ShowError(string message) =>
        ShowError(ErrorTitle,
            message) ;

    /// <summary>
    ///     Shows an information message with title
    /// </summary>
    /// <param name="title">The title of the information message</param>
    /// <param name="message">The information message content</param>
    public void ShowInfo(string title,
        string message) =>
        MessageBox.Show(message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Information) ;

    /// <summary>
    ///     Shows an information message with default title
    /// </summary>
    /// <param name="message">The information message content</param>
    public void ShowInfo(string message) =>
        ShowInfo(InfoTitle,
            message) ;

    /// <summary>
    ///     Shows a warning message with title
    /// </summary>
    /// <param name="title">The title of the warning message</param>
    /// <param name="message">The warning message content</param>
    public void ShowWarning(string title,
        string message) =>
        MessageBox.Show(message,
            title,
            MessageBoxButton.OK,
            MessageBoxImage.Warning) ;

    /// <summary>
    ///     Shows a warning message with default title
    /// </summary>
    /// <param name="message">The warning message content</param>
    public void ShowWarning(string message) =>
        ShowWarning(WarningTitle,
            message) ;

    /// <summary>
    ///     Shows a question message with title
    /// </summary>
    /// <param name="title">The title of the question message</param>
    /// <param name="message">The question message content</param>
    /// <returns>True if user clicks Yes, False if user clicks No</returns>
    public bool ShowQuestion(string title,
        string message)
    {
        var result = MessageBox.Show(message,
            title,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question) ;
        return result == MessageBoxResult.Yes ;
    }

    /// <summary>
    ///     Shows a question message with default title
    /// </summary>
    /// <param name="message">The question message content</param>
    /// <returns>True if user clicks Yes, False if user clicks No</returns>
    public bool ShowQuestion(string message) =>
        ShowQuestion(QuestionTitle,
            message) ;
}
