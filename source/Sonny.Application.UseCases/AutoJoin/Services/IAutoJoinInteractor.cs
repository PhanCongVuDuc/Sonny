using Sonny.Application.Domain.Entities.AutoJoin.Models ;

namespace Sonny.Application.UseCases.AutoJoin.Services ;

/// <summary>
///     Runs one auto-join pass over the document. Must be called in the Revit API context
///     (the ViewModel dispatches through IRevitTaskRunner)
/// </summary>
public interface IAutoJoinInteractor
{
    /// <summary>
    ///     Executes the run described by the input inside a single transaction (one undo step)
    /// </summary>
    /// <param name="input">Mode, rules and options captured from the UI</param>
    void Execute(AutoJoinInput input) ;
}
