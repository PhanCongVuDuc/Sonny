namespace Sonny.Application.UnitTests.Utils ;

/// <summary>
///     Helper class for enum operations with .NET Framework 4.8 compatibility
/// </summary>
public static class EnumHelper
{
    /// <summary>
    ///     Gets all values of the specified enum type
    ///     Compatible with both .NET Framework 4.8 and .NET 8.0
    /// </summary>
    /// <typeparam name="T">The enum type</typeparam>
    /// <returns>An array of all enum values</returns>
    public static T[] GetValues<T>() where T : struct, Enum
    {
#if NETCOREAPP
        return Enum.GetValues<T>() ;
#else
        return (T[])Enum.GetValues(typeof( T )) ;
#endif
    }
}
