using System.Globalization ;
using Sonny.Application.Domain.Entities.FramingFromCad ;

namespace Sonny.Application.UnitTests.Domain.FramingFromCad ;

/// <summary>
///     Tests for <see cref="BeamSectionListParser" />, derived from the "Luật parse tiết diện" rule and
///     the <c>silent section skip</c> failure mode in docs/features/FramingFromCad.md — not from the
///     parser's own code. Every malformed entry must vanish without a word; every well-formed one must
///     survive in the order it was typed.
/// </summary>
public class BeamSectionListParserTests
{
    /// <summary>
    ///     Identity converter: these tests are about parsing, so display units and feet coincide
    /// </summary>
    private static double Identity(double value) => value ;

    [Test]
    public void Parse_TwoWellFormedEntries_KeepsBothInTypedOrder()
    {
        var sections = BeamSectionListParser.Parse("200x300; 400x600",
            Identity) ;

        Assert.That(sections,
            Has.Count.EqualTo(2)) ;
        Assert.Multiple(() => {
            Assert.That(sections[0].WidthDisplay,
                Is.EqualTo(200)) ;
            Assert.That(sections[0].HeightDisplay,
                Is.EqualTo(300)) ;
            Assert.That(sections[1].WidthDisplay,
                Is.EqualTo(400)) ;
            Assert.That(sections[1].HeightDisplay,
                Is.EqualTo(600)) ;
        }) ;
    }

    [Test]
    public void Parse_UppercaseSeparatorAndSurroundingSpaces_StillParses()
    {
        var sections = BeamSectionListParser.Parse(" 200X300 ",
            Identity) ;

        Assert.That(sections,
            Has.Count.EqualTo(1)) ;
        Assert.Multiple(() => {
            Assert.That(sections[0].WidthDisplay,
                Is.EqualTo(200)) ;
            Assert.That(sections[0].HeightDisplay,
                Is.EqualTo(300)) ;
        }) ;
    }

    [TestCase("abc",
        TestName = "Parse_NotANumber_Dropped")]
    [TestCase("200",
        TestName = "Parse_MissingSeparator_Dropped")]
    [TestCase("0x300",
        TestName = "Parse_ZeroWidth_Dropped")]
    [TestCase("200x0",
        TestName = "Parse_ZeroHeight_Dropped")]
    [TestCase("-200x300",
        TestName = "Parse_NegativeWidth_Dropped")]
    [TestCase("200x-300",
        TestName = "Parse_NegativeHeight_Dropped")]
    [TestCase("x300",
        TestName = "Parse_EmptyWidth_Dropped")]
    public void Parse_MalformedEntry_IsDroppedSilently(string text)
    {
        var sections = BeamSectionListParser.Parse(text,
            Identity) ;

        Assert.That(sections,
            Is.Empty) ;
    }

    [Test]
    public void Parse_MalformedEntryBetweenGoodOnes_DropsOnlyTheBadEntry()
    {
        var sections = BeamSectionListParser.Parse("200x300; abc; 400x600",
            Identity) ;

        Assert.That(sections,
            Has.Count.EqualTo(2)) ;
        Assert.Multiple(() => {
            Assert.That(sections[0].WidthDisplay,
                Is.EqualTo(200)) ;
            Assert.That(sections[1].WidthDisplay,
                Is.EqualTo(400)) ;
        }) ;
    }

    [Test]
    public void Parse_EmptyEntriesFromRepeatedSeparator_AreSkipped()
    {
        var sections = BeamSectionListParser.Parse("200x300;;400x600",
            Identity) ;

        Assert.That(sections,
            Has.Count.EqualTo(2)) ;
    }

    [TestCase(null,
        TestName = "Parse_Null_ReturnsEmpty")]
    [TestCase("",
        TestName = "Parse_EmptyString_ReturnsEmpty")]
    [TestCase("   ",
        TestName = "Parse_Whitespace_ReturnsEmpty")]
    [TestCase(";;;",
        TestName = "Parse_OnlySeparators_ReturnsEmpty")]
    public void Parse_NothingUsable_ReturnsEmpty(string? text)
    {
        var sections = BeamSectionListParser.Parse(text,
            Identity) ;

        Assert.That(sections,
            Is.Empty) ;
    }

    [Test]
    public void Parse_MoreThanTwoDimensions_UsesTheFirstTwo()
    {
        var sections = BeamSectionListParser.Parse("200x300x400",
            Identity) ;

        Assert.That(sections,
            Has.Count.EqualTo(1)) ;
        Assert.Multiple(() => {
            Assert.That(sections[0].WidthDisplay,
                Is.EqualTo(200)) ;
            Assert.That(sections[0].HeightDisplay,
                Is.EqualTo(300)) ;
        }) ;
    }

    [Test]
    public void Parse_ConvertsToInternalUnitsButKeepsDisplayValues()
    {
        var sections = BeamSectionListParser.Parse("200x300",
            displayValue => displayValue / 304.8) ;

        Assert.That(sections,
            Has.Count.EqualTo(1)) ;
        Assert.Multiple(() => {
            Assert.That(sections[0].WidthDisplay,
                Is.EqualTo(200)) ;
            Assert.That(sections[0].HeightDisplay,
                Is.EqualTo(300)) ;
            Assert.That(sections[0].Width,
                Is.EqualTo(200 / 304.8).Within(1e-9)) ;
            Assert.That(sections[0].Height,
                Is.EqualTo(300 / 304.8).Within(1e-9)) ;
        }) ;
    }

    /// <summary>
    ///     The original used <c>Convert.ToDouble</c>, which reads the current culture. Pinned explicitly
    ///     so the behaviour is a decision on record rather than an accident of the build machine
    /// </summary>
    [Test]
    public void Parse_DecimalSeparator_FollowsCurrentCulture()
    {
        var originalCulture = CultureInfo.CurrentCulture ;
        try {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture ;
            var sections = BeamSectionListParser.Parse("200.5x300",
                Identity) ;

            Assert.That(sections,
                Has.Count.EqualTo(1)) ;
            Assert.That(sections[0].WidthDisplay,
                Is.EqualTo(200.5)) ;
        }
        finally {
            CultureInfo.CurrentCulture = originalCulture ;
        }
    }

    [Test]
    public void Parse_KeepsSourceTextForProgressReporting()
    {
        var sections = BeamSectionListParser.Parse(" 200x300 ",
            Identity) ;

        Assert.That(sections[0].SourceText,
            Is.EqualTo("200x300")) ;
    }
}
