using Autodesk.Revit.UI ;
using Revit.Async ;
using Serilog ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.Services ;

namespace Sonny.Application.Bases ;

/// <summary>
///     Base class for all external commands with automatic RevitTask initialization
/// </summary>
public abstract class BaseExternalCommand : IExternalCommand
{
    /// <summary>
    ///     Determines whether license check should be performed for this command
    /// </summary>
    /// <returns>True if license should be checked, false otherwise</returns>
    protected virtual bool ShouldCheckLicense() => false ;

    /// <summary>
    ///     Executes the command with automatic RevitTask initialization
    /// </summary>
    /// <param name="commandData">The external command data</param>
    /// <param name="message">The message</param>
    /// <param name="elements">The element set</param>
    /// <returns>The result of the command execution</returns>
    public Result Execute(ExternalCommandData commandData,
        ref string message,
        ElementSet elements)
    {
        try {
            // Initialize Host if not already initialized
            Host.Start() ;

            if (ShouldCheckLicense()) {
                var licenseCheckService = Host.GetService<ILicenseCheckService>() ;
                if (! licenseCheckService.CheckLicense()) {
                    return Result.Cancelled ;
                }
            }

            // Set UIDocument in provider for DI container
            var uiDocumentProvider = Host.GetService<IUIDocumentProvider>() ;
            uiDocumentProvider.SetUIDocument(commandData.Application.ActiveUIDocument) ;

            // Initialize RevitTask in Command context (valid Revit API context)
            RevitTask.Initialize(commandData.Application) ;

            // Call the derived class implementation
            return ExecuteInternal(commandData,
                ref message,
                elements) ;
        }
        catch (Exception ex) {
            // Log error
            try {
                var logger = Host.GetService<ILogger>() ;
                logger.Error(ex,
                    "Error executing command: {CommandName}",
                    GetType()
                        .Name) ;
            }
            catch {
                // Ignore logging errors
            }

            // Show error message to user
            try {
                var messageService = Host.GetService<IMessageService>() ;
                messageService?.ShowError("Command Error",
                    $"An error occurred while executing the command.\n\n{ex.Message}") ;
            }
            catch {
                // If message service fails, set message parameter
                message = $"Error: {ex.Message}" ;
            }

            return Result.Failed ;
        }
    }

    /// <summary>
    ///     Executes the command logic (to be implemented by derived classes)
    /// </summary>
    /// <param name="commandData">The external command data</param>
    /// <param name="message">The message</param>
    /// <param name="elements">The element set</param>
    /// <returns>The result of the command execution</returns>
    protected abstract Result ExecuteInternal(ExternalCommandData commandData,
        ref string message,
        ElementSet elements) ;
}
