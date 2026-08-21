using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.UnitTests.Utils ;
using Sonny.Application.UseCases.Settings.Models ;

namespace Sonny.Application.UnitTests.UseCases.Settings ;

/// <summary>
///     Unit tests for LanguageOption
/// </summary>
[TestFixture]
public class LanguageOptionTests
{
    [Test]
    public void Constructor_ShouldInitializeProperties()
    {
        // Arrange
        var displayName = "English" ;
        var languageCode = AppLanguageCode.En ;

        // Act
        var option = new LanguageOption(displayName,
            languageCode) ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(option.DisplayName,
                Is.EqualTo(displayName)) ;
            Assert.That(option.LanguageCode,
                Is.EqualTo(languageCode)) ;
        }) ;
    }

    [Test]
    public void Constructor_ShouldHandleEmptyDisplayName()
    {
        // Arrange
        var displayName = string.Empty ;
        var languageCode = AppLanguageCode.En ;

        // Act
        var option = new LanguageOption(displayName,
            languageCode) ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(option.DisplayName,
                Is.EqualTo(string.Empty)) ;
            Assert.That(option.LanguageCode,
                Is.EqualTo(AppLanguageCode.En)) ;
        }) ;
    }

    [Test]
    public void Constructor_ShouldHandleNullDisplayName()
    {
        // Arrange
        string? displayName = null ;
        var languageCode = AppLanguageCode.Vi ;

        // Act
        var option = new LanguageOption(displayName!,
            languageCode) ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(option.DisplayName,
                Is.Null) ;
            Assert.That(option.LanguageCode,
                Is.EqualTo(AppLanguageCode.Vi)) ;
        }) ;
    }

    [Test]
    public void ToString_ShouldReturnDisplayName()
    {
        // Arrange
        var displayName = "Vietnamese" ;
        var languageCode = AppLanguageCode.Vi ;
        var option = new LanguageOption(displayName,
            languageCode) ;

        // Act
        var result = option.ToString() ;

        // Assert
        Assert.That(result,
            Is.EqualTo(displayName)) ;
    }

    [Test]
    public void ToString_ShouldReturnEmptyString_WhenDisplayNameIsEmpty()
    {
        // Arrange
        var option = new LanguageOption(string.Empty,
            AppLanguageCode.En) ;

        // Act
        var result = option.ToString() ;

        // Assert
        Assert.That(result,
            Is.EqualTo(string.Empty)) ;
    }

    [Test]
    public void Properties_ShouldBeReadOnly()
    {
        // Arrange
        var option = new LanguageOption("English",
            AppLanguageCode.En) ;

        // Act & Assert
        // Properties should be get-only, so we can't set them
        // This test verifies the properties exist and are accessible.
        // The original also asserted LanguageCode is not null; AppLanguageCode is an enum, so that
        // assert could never fail and NUnit 4's analyzer rejects it (NUnit2023). Dropped, not replaced.
        Assert.That(option.DisplayName,
            Is.Not.Null) ;
    }

    [Test]
    public void Constructor_ShouldWorkWithAllLanguageCodes()
    {
        // Act & Assert - Verify constructor works with all language codes
        var allCodes = EnumHelper.GetValues<AppLanguageCode>() ;

        foreach (var code in allCodes) {
            var displayName = $"Language {code}" ;
            var option = new LanguageOption(displayName,
                code) ;

            Assert.That(option.DisplayName,
                Is.EqualTo(displayName)) ;
            Assert.That(option.LanguageCode,
                Is.EqualTo(code)) ;
        }
    }
}
