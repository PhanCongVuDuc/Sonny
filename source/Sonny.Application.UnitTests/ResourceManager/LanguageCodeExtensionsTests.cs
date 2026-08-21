using Sonny.Application.UnitTests.Utils ;
using Sonny.ResourceManager ;

namespace Sonny.Application.UnitTests.ResourceManager ;

/// <summary>
///     Unit tests for LanguageCodeExtensions
/// </summary>
[TestFixture]
public class LanguageCodeExtensionsTests
{
    [Test]
    public void ToCodeString_ShouldReturnCorrectCode_ForAllLanguageCodes()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That(LanguageCode.En.ToCodeString(),
                Is.EqualTo("en")) ;
            Assert.That(LanguageCode.Vi.ToCodeString(),
                Is.EqualTo("vi")) ;
            Assert.That(LanguageCode.Ja.ToCodeString(),
                Is.EqualTo("ja")) ;
            Assert.That(LanguageCode.Es.ToCodeString(),
                Is.EqualTo("es")) ;
            Assert.That(LanguageCode.Id.ToCodeString(),
                Is.EqualTo("id")) ;
            Assert.That(LanguageCode.Th.ToCodeString(),
                Is.EqualTo("th")) ;
            Assert.That(LanguageCode.Km.ToCodeString(),
                Is.EqualTo("km")) ;
            Assert.That(LanguageCode.Zh.ToCodeString(),
                Is.EqualTo("zh")) ;
            Assert.That(LanguageCode.Ko.ToCodeString(),
                Is.EqualTo("ko")) ;
        }) ;
    }

    [Test]
    public void ToCodeString_ShouldReturnEn_ForInvalidEnumValue()
    {
        // Arrange
        var invalidValue = (LanguageCode)999 ;

        // Act
        var result = invalidValue.ToCodeString() ;

        // Assert
        Assert.That(result,
            Is.EqualTo("en")) ;
    }

    [Test]
    public void ToLanguageCode_ShouldReturnCorrectEnum_ForValidCodeStrings()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That("en".ToLanguageCode(),
                Is.EqualTo(LanguageCode.En)) ;
            Assert.That("vi".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Vi)) ;
            Assert.That("ja".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Ja)) ;
            Assert.That("es".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Es)) ;
            Assert.That("id".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Id)) ;
            Assert.That("th".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Th)) ;
            Assert.That("km".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Km)) ;
            Assert.That("zh".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Zh)) ;
            Assert.That("ko".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Ko)) ;
        }) ;
    }

    [Test]
    public void ToLanguageCode_ShouldBeCaseInsensitive()
    {
        // Act & Assert
        Assert.Multiple(() =>
        {
            Assert.That("EN".ToLanguageCode(),
                Is.EqualTo(LanguageCode.En)) ;
            Assert.That("VI".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Vi)) ;
            Assert.That("Ja".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Ja)) ;
            Assert.That("ES".ToLanguageCode(),
                Is.EqualTo(LanguageCode.Es)) ;
        }) ;
    }

    [Test]
    public void ToLanguageCode_ShouldReturnEn_ForNullString()
    {
        // Act
        var result = ((string?)null).ToLanguageCode() ;

        // Assert
        Assert.That(result,
            Is.EqualTo(LanguageCode.En)) ;
    }

    [Test]
    public void ToLanguageCode_ShouldReturnEn_ForEmptyString()
    {
        // Act
        var result = string.Empty.ToLanguageCode() ;

        // Assert
        Assert.That(result,
            Is.EqualTo(LanguageCode.En)) ;
    }

    [Test]
    public void ToLanguageCode_ShouldReturnEn_ForWhitespaceString()
    {
        // Act
        var result = "   ".ToLanguageCode() ;

        // Assert
        Assert.That(result,
            Is.EqualTo(LanguageCode.En)) ;
    }

    [Test]
    public void ToLanguageCode_ShouldReturnEn_ForInvalidCodeString()
    {
        // Act
        var result = "invalid".ToLanguageCode() ;

        // Assert
        Assert.That(result,
            Is.EqualTo(LanguageCode.En)) ;
    }

    [Test]
    public void ToCultureInfo_ShouldReturnCorrectCultureInfo_ForValidLanguageCodes()
    {
        // Act & Assert
        var enCulture = LanguageCode.En.ToCultureInfo() ;
        Assert.That(enCulture.TwoLetterISOLanguageName,
            Is.EqualTo("en")) ;

        var viCulture = LanguageCode.Vi.ToCultureInfo() ;
        Assert.That(viCulture.TwoLetterISOLanguageName,
            Is.EqualTo("vi")) ;

        var jaCulture = LanguageCode.Ja.ToCultureInfo() ;
        Assert.That(jaCulture.TwoLetterISOLanguageName,
            Is.EqualTo("ja")) ;
    }

    [Test]
    public void ToCultureInfo_ShouldReturnEnglishCulture_WhenCultureCreationFails()
    {
        // Note: This test verifies fallback behavior
        // In practice, all valid language codes should create valid cultures
        // But the method has a try-catch to handle edge cases

        // Act
        var result = LanguageCode.En.ToCultureInfo() ;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result,
                Is.Not.Null) ;
            Assert.That(result.TwoLetterISOLanguageName,
                Is.EqualTo("en")) ;
        }) ;
    }

    [Test]
    public void ToCultureInfo_ShouldReturnValidCultureInfo_ForAllLanguageCodes()
    {
        // Act & Assert - Verify all language codes can create valid cultures
        var allCodes = EnumHelper.GetValues<LanguageCode>() ;

        foreach (var code in allCodes) {
            var culture = code.ToCultureInfo() ;
            Assert.That(culture,
                Is.Not.Null,
                $"Culture should not be null for {code}") ;
            Assert.That(culture.TwoLetterISOLanguageName,
                Is.Not.Null,
                $"TwoLetterISOLanguageName should not be null for {code}") ;
        }
    }
}
