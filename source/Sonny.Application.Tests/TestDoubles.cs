using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using Sonny.Application.Domain.Services ;

namespace Sonny.Application.Tests ;

// Hand-written fakes on purpose: NSubstitute/Castle proxies resolve the faked interface by
// ASSEMBLY NAME, and the ricaun dev loop (Revit kept open) loads a new copy of this test assembly
// per run — the proxy then implements the interface from the FIRST loaded copy and the cast to
// the current copy's interface fails (InvalidCastException: ObjectProxy_2). A plain class newed
// by the current copy has no such indirection. Keep integration tests NSubstitute-free.

/// <summary>
///     IProgressReporter that does nothing and never cancels
/// </summary>
public class FakeProgressReporter : IProgressReporter
{
    public bool IsCancelRequested => false ;

    public void Show(string title,
        bool allowCancel = false)
    {
    }

    public void Update(int current,
        int total)
    {
    }

    public void Close()
    {
    }
}

/// <summary>
///     IRevitTaskRunner that runs the work inline instead of marshalling it through Revit.Async.
///     An integration test is already ON the Revit API thread, and the real runner needs an external
///     event that never fires here — it would deadlock
/// </summary>
public class ImmediateRevitTaskRunner : IRevitTaskRunner
{
    public Task<TResult> RunAsync<TResult>(Func<TResult> function) => Task.FromResult(function()) ;

    public Task RunAsync(Action action)
    {
        action() ;

        return Task.CompletedTask ;
    }
}

/// <summary>
///     IMessageService that swallows every dialog (a real one would block the headless run)
///     and records the messages for optional assertions
/// </summary>
public class FakeMessageService : IMessageService
{
    public List<string> Messages { get ; } = [] ;

    public void ShowError(string title,
        string message) =>
        Messages.Add(message) ;

    public void ShowError(string message) => Messages.Add(message) ;

    public void ShowInfo(string title,
        string message) =>
        Messages.Add(message) ;

    public void ShowInfo(string message) => Messages.Add(message) ;

    public void ShowWarning(string title,
        string message) =>
        Messages.Add(message) ;

    public void ShowWarning(string message) => Messages.Add(message) ;

    public bool ShowQuestion(string title,
        string message)
    {
        Messages.Add(message) ;

        return true ;
    }

    public bool ShowQuestion(string message)
    {
        Messages.Add(message) ;

        return true ;
    }
}
