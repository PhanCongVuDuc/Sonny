using System.Collections.Generic ;
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
