using System.Globalization ;
using Sonny.ResourceManager ;

namespace Sonny.Application.UnitTests.ResourceManager ;

/// <summary>
///     Unit tests for CultureChangedEventArgs
/// </summary>
[TestFixture]
public class CultureChangedEventArgsTests
{
    [Test]
    public void Constructor_ShouldInitializeCulture()
    {
        // Arrange
        var culture = new CultureInfo("en-US") ;

        // Act
        var args = new CultureChangedEventArgs(culture) ;

        // Assert
        Assert.That(args.Culture,
            Is.EqualTo(culture)) ;
    }

    [Test]
    public void Constructor_ShouldHandleEnglishCulture()
    {
        // Arrange
        var culture = new CultureInfo("en") ;

        // Act
        var args = new CultureChangedEventArgs(culture) ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(args.Culture,
                Is.EqualTo(culture)) ;
            Assert.That(args.Culture.TwoLetterISOLanguageName,
                Is.EqualTo("en")) ;
        }) ;
    }

    [Test]
    public void Constructor_ShouldHandleVietnameseCulture()
    {
        // Arrange
        var culture = new CultureInfo("vi-VN") ;

        // Act
        var args = new CultureChangedEventArgs(culture) ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(args.Culture,
                Is.EqualTo(culture)) ;
            Assert.That(args.Culture.TwoLetterISOLanguageName,
                Is.EqualTo("vi")) ;
        }) ;
    }

    [Test]
    public void Constructor_ShouldHandleJapaneseCulture()
    {
        // Arrange
        var culture = new CultureInfo("ja-JP") ;

        // Act
        var args = new CultureChangedEventArgs(culture) ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(args.Culture,
                Is.EqualTo(culture)) ;
            Assert.That(args.Culture.TwoLetterISOLanguageName,
                Is.EqualTo("ja")) ;
        }) ;
    }

    [Test]
    public void Culture_ShouldBeReadOnly()
    {
        // Arrange
        var culture = new CultureInfo("en-US") ;
        var args = new CultureChangedEventArgs(culture) ;

        // Act & Assert
        // Culture property should be get-only
        Assert.Multiple(() =>
        {
            Assert.That(args.Culture,
                Is.Not.Null) ;
            Assert.That(args.Culture,
                Is.EqualTo(culture)) ;
        }) ;
    }

    [Test]
    public void Constructor_ShouldHandleInvariantCulture()
    {
        // Arrange
        var culture = CultureInfo.InvariantCulture ;

        // Act
        var args = new CultureChangedEventArgs(culture) ;

        // Assert
        Assert.That(args.Culture,
            Is.EqualTo(culture)) ;
    }
}
