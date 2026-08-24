using Sonny.Application.Domain.Entities.ColumnFromCad ;

namespace Sonny.Application.UnitTests.Domain.ColumnFromCad ;

/// <summary>
///     Characterization tests for <see cref="ColumnSymbolSizingPolicy" /> — pin the symbol
///     match/round/minimum-size/naming rules exactly as the creation strategies applied them
///     before the move (ADR 0001).
/// </summary>
public class ColumnSymbolSizingPolicyTests
{
    [TestCase(1.0,
        1.0005,
        ExpectedResult = true)]
    [TestCase(1.0,
        1.0015,
        ExpectedResult = false)]
    [TestCase(1.0,
        0.9995,
        ExpectedResult = true)]
    public bool Matches_UsesStrictTolerance(double candidate,
        double target) =>
        ColumnSymbolSizingPolicy.Matches(candidate,
            target) ;

    [TestCase(0.0005,
        ExpectedResult = true)]
    [TestCase(0.001,
        ExpectedResult = false)]
    [TestCase(-0.0005,
        ExpectedResult = true)]
    public bool IsNegligibleSize_UsesStrictTolerance(double size) =>
        ColumnSymbolSizingPolicy.IsNegligibleSize(size) ;

    [Test]
    public void TryBuildRectangularSymbolName_RoundsAndAppendsUnit()
    {
        var name = ColumnSymbolSizingPolicy.TryBuildRectangularSymbolName(219.6,
            500.4,
            1.0,
            "mm") ;

        Assert.That(name,
            Is.EqualTo("220 x 500mm")) ;
    }

    [Test]
    public void TryBuildRectangularSymbolName_SideBelowMinimumAfterRounding_ReturnsNull()
    {
        // 0.4 rounds to 0, below the 1mm minimum — refuse instead of creating a "0 x 500mm" type
        var name = ColumnSymbolSizingPolicy.TryBuildRectangularSymbolName(0.4,
            500,
            1.0,
            "mm") ;

        Assert.That(name,
            Is.Null) ;
    }

    [Test]
    public void TryBuildCircularSymbolName_RoundsAndAppendsUnit()
    {
        var name = ColumnSymbolSizingPolicy.TryBuildCircularSymbolName(349.5,
            1.0,
            "mm") ;

        Assert.That(name,
            Is.EqualTo("350mm")) ;
    }

    [Test]
    public void TryBuildCircularSymbolName_BelowMinimum_ReturnsNull()
    {
        var name = ColumnSymbolSizingPolicy.TryBuildCircularSymbolName(0.3,
            1.0,
            "mm") ;

        Assert.That(name,
            Is.Null) ;
    }
}
