using NUnit.Framework ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Infrastructure.Revit.Implements ;

namespace Sonny.Application.Tests.Core.RevitApiTests ;

/// <summary>
///     Unit tests for UnitConverter.
///     Not Revit-free despite testing a single class: UnitConverter maps AppDisplayUnit through
///     ForgeTypeId/UnitTypeId and converts with UnitUtils, so these need the Revit API and belong
///     in this assembly rather than in Sonny.Application.UnitTests. See ADR 0002.
/// </summary>
[TestFixture]
public class UnitConverterTests
{
    private UnitConverter _converter ;

    [SetUp]
    public void Setup() => _converter = new UnitConverter() ;

    [Test]
    public void ToInternalUnit_ShouldConvertMillimetersToFeet()
    {
        // Arrange
        var value = 3048.0 ; // 3048 mm = 10 feet
        var displayUnit = AppDisplayUnit.Millimeters ;

        // Act
        var result = _converter.ToInternalUnit(value,
            displayUnit) ;

        // Assert
        // 3048 mm = 10 feet (approximately)
        Assert.AreEqual(10.0,
            result,
            0.01) ;
    }

    [Test]
    public void ToInternalUnit_ShouldReturnSameValue_WhenAlreadyInFeet()
    {
        // Arrange
        var value = 10.0 ;
        var displayUnit = AppDisplayUnit.Feet ;

        // Act
        var result = _converter.ToInternalUnit(value,
            displayUnit) ;

        // Assert
        Assert.AreEqual(10.0,
            result) ;
    }

    [Test]
    public void FromInternalUnit_ShouldConvertFeetToMillimeters()
    {
        // Arrange
        var value = 10.0 ; // 10 feet
        var displayUnit = AppDisplayUnit.Millimeters ;

        // Act
        var result = _converter.FromInternalUnit(value,
            displayUnit) ;

        // Assert
        // 10 feet = 3048 mm (approximately)
        Assert.AreEqual(3048.0,
            result,
            0.01) ;
    }

    [Test]
    public void FromInternalUnit_ShouldReturnSameValue_WhenAlreadyInFeet()
    {
        // Arrange
        var value = 10.0 ;
        var displayUnit = AppDisplayUnit.Feet ;

        // Act
        var result = _converter.FromInternalUnit(value,
            displayUnit) ;

        // Assert
        Assert.AreEqual(10.0,
            result) ;
    }

    [Test]
    public void GetUnitDisplayName_ShouldReturnCorrectUnitNames()
    {
        // Act & Assert
        Assert.AreEqual("mm",
            _converter.GetUnitDisplayName(AppDisplayUnit.Millimeters)) ;
        Assert.AreEqual("cm",
            _converter.GetUnitDisplayName(AppDisplayUnit.Centimeters)) ;
        Assert.AreEqual("m",
            _converter.GetUnitDisplayName(AppDisplayUnit.Meters)) ;
        Assert.AreEqual("ft",
            _converter.GetUnitDisplayName(AppDisplayUnit.Feet)) ;
        Assert.AreEqual("in",
            _converter.GetUnitDisplayName(AppDisplayUnit.Inches)) ;
    }

    [Test]
    public void FormatWithUnit_ShouldFormatValueWithUnitSuffix()
    {
        // Arrange
        var value = 10.5 ;
        var displayUnit = AppDisplayUnit.Millimeters ;

        // Act
        var result = _converter.FormatWithUnit(value,
            displayUnit) ;

        // Assert
        Assert.IsTrue(result.Contains("10.50")) ;
        Assert.IsTrue(result.Contains("mm")) ;
    }

    [Test]
    public void ToInternalUnit_ShouldConvertInchesToFeet()
    {
        // Arrange
        var value = 120.0 ; // 120 inches = 10 feet
        var displayUnit = AppDisplayUnit.Inches ;

        // Act
        var result = _converter.ToInternalUnit(value,
            displayUnit) ;

        // Assert
        Assert.AreEqual(10.0,
            result,
            0.01) ;
    }

    [Test]
    public void FromInternalUnit_ShouldConvertFeetToInches()
    {
        // Arrange
        var value = 10.0 ; // 10 feet
        var displayUnit = AppDisplayUnit.Inches ;

        // Act
        var result = _converter.FromInternalUnit(value,
            displayUnit) ;

        // Assert
        Assert.AreEqual(120.0,
            result,
            0.01) ;
    }
}
