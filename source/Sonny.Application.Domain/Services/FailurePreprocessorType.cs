namespace Sonny.Application.Domain.Services ;

/// <summary>
///     Type of failure preprocessor to use
/// </summary>
public enum FailurePreprocessorType
{
    /// <summary>
    ///     No preprocessor
    /// </summary>
    None,

    /// <summary>
    ///     Suppress warnings preprocessor
    /// </summary>
    SuppressWarnings,

    /// <summary>
    ///     Resolves every failure (warnings included) and records the element ids of
    ///     error-severity failures into <see cref="IFailingElementIdsTracker" />
    /// </summary>
    ResolveAllFailures,

    /// <summary>
    ///     Deletes warnings from the failure dialog, resolves errors, and rolls back on
    ///     document corruption. Does not record failing element ids (ported behaviour)
    /// </summary>
    DeleteWarningsResolveErrors
}
