using Serilog ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Models ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;

namespace Sonny.Application.UseCases.AutoColumnDimension.Implements ;

/// <summary>
///     Interactor for executing auto column dimension creation process. Pure form (ADR 0001):
///     geometry comes in through IColumnGeometryReader, decisions are made by
///     ColumnDimensionPolicy, and Revit work goes out through IDimensionPlanExecutor.
/// </summary>
public class AutoColumnDimensionInteractor(
    IColumnGeometryReader columnGeometryReader,
    IDimensionPlanExecutor dimensionPlanExecutor,
    IMessageService messageService,
    ILogger logger,
    IResourceHelper resourceHelper,
    ITransactionManagerFactory transactionManagerFactory) : IAutoColumnDimensionInteractor
{
    private const string TransactionName = "Auto Column Dimension" ;
    private const int ExpectedDimensionsPerColumn = 2 ;

    public void Execute(double snapDistance,
        string? dimensionTypeUniqueId = null)
    {
        logger.Information("Starting dimension creation process") ;

        var geometry = columnGeometryReader.ReadActiveView() ;

        logger.Debug("Processing view: {ViewName}",
            geometry.View.Name) ;

        logger.Information("Found {Count} column wrappers",
            geometry.Columns.Count) ;

        if (! ValidateColumns(geometry.Columns)) {
            return ;
        }

        var createdCount = CreateDimensions(geometry,
            snapDistance,
            dimensionTypeUniqueId) ;

        logger.Information("Created {Count} dimensions successfully",
            createdCount) ;

        ShowResult(createdCount,
            geometry.Columns.Count) ;
    }

    private bool ValidateColumns(List<ColumnGeometryData> columns)
    {
        if (columns.Count == 0) {
            logger.Warning("No valid columns found for dimensioning") ;
            messageService.ShowInfo(resourceHelper.GetString("MessageNoColumnsFound")) ;
            return false ;
        }

        return true ;
    }

    private int CreateDimensions(ActiveViewGeometry geometry,
        double snapDistance,
        string? dimensionTypeUniqueId)
    {
        // One transaction around the whole loop: one undo step, nothing visible until commit
        using var transaction = transactionManagerFactory.Create(TransactionName) ;
        transaction.Start() ;

        var createdCount = 0 ;

        foreach (var column in geometry.Columns) {
            try {
                if (ColumnDimensionPolicy.CreatePlan(column,
                        geometry.View,
                        geometry.Grids) is not { } plan) {
                    continue ;
                }

                createdCount += dimensionPlanExecutor.Execute(column.UniqueId,
                    plan,
                    snapDistance,
                    dimensionTypeUniqueId) ;
            }
            catch (Exception ex) {
                // Load-bearing: one bad column must not abort the run
                logger.Warning(ex,
                    "Failed to create dimensions for column") ;
            }
        }

        transaction.Commit() ;

        return createdCount ;
    }

    private void ShowResult(int successCount,
        int columnCount)
    {
        var expectedDimensionCount = columnCount * ExpectedDimensionsPerColumn ;
        var failureCount = Math.Max(0,
            expectedDimensionCount - successCount) ;

        var total = successCount + failureCount ;
        var completedTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") ;
        var logMessage = resourceHelper.GetString("ExecutionResultCompleted",
                             completedTime)
                         + "\n\n"
                         + resourceHelper.GetString("ExecutionResultTotalColumns",
                             total)
                         + "\n"
                         + resourceHelper.GetString("ExecutionResultSuccess",
                             successCount)
                         + "\n"
                         + resourceHelper.GetString("ExecutionResultFailed",
                             failureCount) ;

        messageService.ShowInfo(logMessage) ;
    }
}
