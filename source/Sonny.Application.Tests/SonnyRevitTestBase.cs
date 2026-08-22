using System ;
using System.IO ;
using System.Linq ;
using System.Reflection ;
using Autodesk.Revit.ApplicationServices ;
using Autodesk.Revit.DB ;
using Autodesk.Revit.UI ;
using NUnit.Framework ;
using Sonny.RevitExtensions.Extensions ;
using RevitApplication = Autodesk.Revit.ApplicationServices.Application ;

namespace Sonny.Application.Tests ;

/// <summary>
///     Base class for Sonny Revit tests with helper methods
/// </summary>
public abstract class SonnyRevitTestBase
{
    protected UIApplication? UIApp { get ; private set ; }
    protected RevitApplication? Application { get ; private set ; }
    protected UIControlledApplication? UIControlledApp { get ; private set ; }
    protected ControlledApplication? ControlledApp { get ; private set ; }

    [OneTimeSetUp]
    public void Setup(UIApplication uiapp,
        RevitApplication application,
        UIControlledApplication uiControlledApplication,
        ControlledApplication controlledApplication)
    {
        UIApp = uiapp ;
        Application = application ;
        UIControlledApp = uiControlledApplication ;
        ControlledApp = controlledApplication ;
        OnSetup() ;
    }

    /// <summary>
    ///     Override this method to add custom setup logic
    /// </summary>
    protected virtual void OnSetup()
    {
        // Override in derived classes
    }

    [OneTimeTearDown]
    public void TearDown() => OnTearDown() ;

    /// <summary>
    ///     Override this method to add custom teardown logic
    /// </summary>
    protected virtual void OnTearDown()
    {
        // Override in derived classes
    }

    #region Helper Methods

    /// <summary>
    ///     Get active document
    /// </summary>
    protected Document? GetActiveDocument() => UIApp?.ActiveUIDocument?.Document ;

    protected UIDocument? OpenDocument(string filePath) => UIApp!.OpenAndActivateDocument(filePath) ;

    protected View? OpenView(Document doc,
        string viewName)
    {
        // Find view - try ViewPlan first, then all View types
        var view = doc.GetAllElements<ViewPlan>()
            .FirstOrDefault(v => v.Name == viewName) ;

        if (view == null) {
            return null ;
        }

        var uiDoc = new UIDocument(doc) ;
        uiDoc.RequestViewChange(view) ;

        return view ;
    }

    /// <summary>
    ///     Log message to test output
    ///     Uses TestContext.WriteLine for better integration with test runners
    /// </summary>
    protected void Log(string message)
    {
        var logMessage = $"[{DateTime.Now:HH:mm:ss}] {message}" ;
        TestContext.WriteLine(logMessage) ;
        // Also write to console for compatibility with Revit test adapter
        Console.WriteLine(logMessage) ;
    }

    protected void AssertDocument(Document doc,
        string message = "Document should not be null") =>
        Assert.IsNotNull(doc,
            message) ;

    /// <summary>
    ///     Get count of dimensions in document
    /// </summary>
    protected int GetDimensionCount(Document doc) =>
        new FilteredElementCollector(doc).OfClass(typeof( Dimension ))
            .GetElementCount() ;

    /// <summary>
    ///     Get path to test Revit file in Resources/RevitFiles/ directory
    /// </summary>
    /// <param name="fileName">Name of the Revit file (e.g., "Test_V2023.rvt")</param>
    /// <param name="subDirectory">Optional subdirectory path (default: "Resources/RevitFiles")</param>
    /// <returns>Full path to the file if exists, null otherwise</returns>
    protected string? GetTestRevitFilePath(string? fileName,
        string subDirectory = "Resources/RevitFiles")
    {
        if (string.IsNullOrEmpty(fileName)) {
            return null ;
        }

        // Get the directory where the test assembly DLL is located
        // This is more reliable than AppDomain.BaseDirectory in Revit environment
        var assembly = Assembly.GetExecutingAssembly() ;
        var assemblyLocation = assembly.Location ;

        if (string.IsNullOrEmpty(assemblyLocation)) {
            // Fallback: try CodeBase if Location is empty (only for .NET Framework)
#if NETFRAMEWORK
            var codeBase = assembly.CodeBase ;
            if (! string.IsNullOrEmpty(codeBase)) {
                var uri = new Uri(codeBase) ;
                assemblyLocation = uri.LocalPath ;
            }
#endif
        }

        if (string.IsNullOrEmpty(assemblyLocation)) {
            Log("Warning: Could not determine assembly location, will create new document") ;
            return null ;
        }

        var assemblyDirectory = Path.GetDirectoryName(assemblyLocation) ;
        if (string.IsNullOrEmpty(assemblyDirectory)) {
            Log("Warning: Could not determine assembly directory, will create new document") ;
            return null ;
        }

        // Construct path to test Revit file
        var testFilePath = Path.Combine(assemblyDirectory,
            subDirectory,
            fileName) ;

        // Log the path being used
        Log($"Assembly location: {assemblyLocation}") ;
        Log($"Assembly directory: {assemblyDirectory}") ;
        Log($"Looking for test file at: {testFilePath}") ;

        // Return path if file exists, otherwise return null to create new document
        if (File.Exists(testFilePath)) {
            Log($"✓ Found test file: {testFilePath}") ;
            return testFilePath ;
        }

        // The ricaun adapter's ATTACH mode (reusing an open Revit) copies only the assemblies,
        // not the Resources subfolder — fall back to the source project's copy in that case
        var sourceFilePath = GetSourceRevitFilePath(fileName!,
            subDirectory) ;
        if (sourceFilePath != null) {
            Log($"✓ Found test file in source project: {sourceFilePath}") ;
            return sourceFilePath ;
        }

        Log($"⚠ Test file not found at: {testFilePath}") ;
        Log("  Will create new document instead.") ;
        return null ;
    }

    /// <summary>
    ///     Resolves a test file inside the source project folder. The compile-time path of this
    ///     file is the anchor because the assembly location is a shadow copy
    /// </summary>
    private static string? GetSourceRevitFilePath(string fileName,
        string subDirectory,
        [System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "")
    {
        var directory = new DirectoryInfo(Path.GetDirectoryName(callerFilePath)!) ;
        while (directory != null
               && ! File.Exists(Path.Combine(directory.FullName,
                   "Sonny.Application.Tests.csproj"))) {
            directory = directory.Parent ;
        }

        if (directory == null) {
            return null ;
        }

        var candidate = Path.Combine(directory.FullName,
            subDirectory.Replace('/',
                Path.DirectorySeparatorChar),
            fileName) ;

        return File.Exists(candidate) ? candidate : null ;
    }

    #endregion
}
