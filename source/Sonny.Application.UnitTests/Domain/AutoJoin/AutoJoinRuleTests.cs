using Sonny.Application.Domain.Entities.AutoJoin.Models ;

namespace Sonny.Application.UnitTests.Domain.AutoJoin ;

/// <summary>
///     Pins the default rule the UI seeds when nothing is selected and no settings exist:
///     (Beam, All, no reverse) — the same default the original AlphaBIM tool used
/// </summary>
public class AutoJoinRuleTests
{
    [Test]
    public void NewRule_Defaults_AreBeamCutsAllWithoutReverse()
    {
        var rule = new AutoJoinRule() ;

        Assert.That(rule.PriorityCategory,
            Is.EqualTo(JoinCategory.Beam)) ;
        Assert.That(rule.JoinWithCategory,
            Is.EqualTo(JoinCategory.All)) ;
        Assert.That(rule.IsReverse,
            Is.False) ;
    }
}
