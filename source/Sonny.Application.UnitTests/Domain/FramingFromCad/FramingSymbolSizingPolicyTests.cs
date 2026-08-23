using Sonny.Application.Domain.Entities.FramingFromCad ;

namespace Sonny.Application.UnitTests.Domain.FramingFromCad ;

/// <summary>
///     Tests for <see cref="FramingSymbolSizingPolicy" />, derived from the Output and Invariants
///     sections of docs/features/FramingFromCad.md: symbol matching is a strict 0.001 ft comparison, and
///     a generated type is named from the <b>display</b> values rounded to whole units — "200x300", with
///     no spaces and no unit suffix, unlike the column feature's "200 x 300mm"
/// </summary>
public class FramingSymbolSizingPolicyTests
{
    [TestCase(1.0,
        1.0005,
        ExpectedResult = true,
        TestName = "Matches_JustInsideTolerance")]
    [TestCase(1.0,
        0.9995,
        ExpectedResult = true,
        TestName = "Matches_JustInsideToleranceBelow")]
    [TestCase(1.0,
        1.0015,
        ExpectedResult = false,
        TestName = "Matches_OutsideTolerance")]
    // Boundary case with a representable gap: |0 - 0.001| is exactly the tolerance, and the comparison
    // is strict, so it is not a match. Probing the boundary as 1.0 vs 1.001 instead would assert
    // nothing, because that subtraction yields 0.00099999999999988987 — below the tolerance in double
    [TestCase(0.0,
        0.001,
        ExpectedResult = false,
        TestName = "Matches_ExactlyAtToleranceIsNotAMatch")]
    public bool Matches_UsesStrictThousandthOfAFoot(double candidate,
        double target) =>
        FramingSymbolSizingPolicy.Matches(candidate,
            target) ;

    [Test]
    public void TryBuildSymbolName_WholeValues_JoinsWithLowercaseXAndNoUnit()
    {
        var name = FramingSymbolSizingPolicy.TryBuildSymbolName(200,
            300,
            1.0) ;

        Assert.That(name,
            Is.EqualTo("200x300")) ;
    }

    [Test]
    public void TryBuildSymbolName_FractionalValues_RoundsToWholeUnits()
    {
        var name = FramingSymbolSizingPolicy.TryBuildSymbolName(199.6,
            300.4,
            1.0) ;

        Assert.That(name,
            Is.EqualTo("200x300")) ;
    }

    /// <summary>
    ///     The rounding example the original code carried as a comment: 2995.5 -> 2996
    /// </summary>
    [Test]
    public void TryBuildSymbolName_RoundsHalfAwayFromZeroLikeTheOriginalComment()
    {
        var name = FramingSymbolSizingPolicy.TryBuildSymbolName(2995.5,
            600,
            1.0) ;

        Assert.That(name,
            Is.EqualTo("2996x600")) ;
    }

    [TestCase(0.3,
        600.0,
        TestName = "TryBuildSymbolName_WidthRoundsToZero_Refused")]
    [TestCase(600.0,
        0.4,
        TestName = "TryBuildSymbolName_HeightRoundsToZero_Refused")]
    [TestCase(0.2,
        0.2,
        TestName = "TryBuildSymbolName_BothRoundToZero_Refused")]
    public void TryBuildSymbolName_SideRoundsBelowMinimum_ReturnsNull(double widthDisplay,
        double heightDisplay)
    {
        var name = FramingSymbolSizingPolicy.TryBuildSymbolName(widthDisplay,
            heightDisplay,
            1.0) ;

        Assert.That(name,
            Is.Null) ;
    }

    /// <summary>
    ///     A side that rounds to exactly the minimum is still buildable — the refusal is "below", not
    ///     "at or below"
    /// </summary>
    [Test]
    public void TryBuildSymbolName_SideRoundsToExactlyTheMinimum_IsAccepted()
    {
        var name = FramingSymbolSizingPolicy.TryBuildSymbolName(1.0,
            600,
            1.0) ;

        Assert.That(name,
            Is.EqualTo("1x600")) ;
    }
}
