using Sonny.Application.Domain.Exceptions ;

namespace Sonny.Application.Domain.Services ;

/// <summary>
///     Interface for managing Revit transactions
/// </summary>
public interface ITransactionManager : IDisposable
{
    /// <summary>
    ///     Starts the transaction
    /// </summary>
    void Start() ;

    /// <summary>
    ///     Commits the transaction
    /// </summary>
    /// <returns>True if commit successful</returns>
    /// <exception cref="TransactionCommitFailedException">Thrown when commit fails</exception>
    bool Commit() ;

    /// <summary>
    ///     Rolls the transaction back, discarding every change made since <see cref="Start" />.
    ///     Disposing without committing rolls back too — call this only to roll back explicitly
    ///     before the manager goes out of scope (e.g. user cancellation)
    /// </summary>
    void RollBack() ;
}
