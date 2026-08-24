using Sonny.Application.Domain.Entities.AutoJoin.Models ;
using Sonny.Application.UseCases.AutoJoin.Implements ;

namespace Sonny.Application.UnitTests.UseCases.AutoJoin ;

/// <summary>
///     Pins the seed-from-selection mapping, ported verbatim from AlphaBIM including its quirks
///     (D4 in docs/features/AutoJoin.md). If one of these "wrong-looking" cases fails, read the
///     doc before fixing the mapping — the quirk is the requirement.
/// </summary>
public class AutoJoinSeedMapperTests
{
    [TestCase(-2001320,
        JoinCategory.Beam)]
    [TestCase(-2001300,
        JoinCategory.Foundation)]
    [TestCase(-2001640,
        JoinCategory.GenericModel)]
    [TestCase(-2000038,
        JoinCategory.Ceiling)]
    [TestCase(-2000035,
        JoinCategory.Roof)]
    public void CreateSeedRule_StraightMappings_SeedThePriorityCategory(long categoryId,
        JoinCategory expected)
    {
        var rule = AutoJoinSeedMapper.CreateSeedRule(categoryId) ;

        Assert.That(rule,
            Is.Not.Null) ;
        Assert.That(rule!.PriorityCategory,
            Is.EqualTo(expected)) ;
        Assert.That(rule.JoinWithCategory,
            Is.EqualTo(JoinCategory.All)) ;
    }

    [Test]
    public void CreateSeedRule_StructuralColumns_SeedsArchitecturalColumn_PortedQuirk() =>
        Assert.That(AutoJoinSeedMapper.CreateSeedRule(-2001330)!.PriorityCategory,
            Is.EqualTo(JoinCategory.ArchitecturalColumn)) ;

    [Test]
    public void CreateSeedRule_Walls_SeedsStructuralWall_PortedQuirk() =>
        Assert.That(AutoJoinSeedMapper.CreateSeedRule(-2000011)!.PriorityCategory,
            Is.EqualTo(JoinCategory.StructuralWall)) ;

    [Test]
    public void CreateSeedRule_Floors_SeedsArchitecturalFloor_PortedQuirk() =>
        Assert.That(AutoJoinSeedMapper.CreateSeedRule(-2000032)!.PriorityCategory,
            Is.EqualTo(JoinCategory.ArchitecturalFloor)) ;

    [Test]
    public void CreateSeedRule_UnmappedCategory_ReturnsNull() =>
        Assert.That(AutoJoinSeedMapper.CreateSeedRule(-2000023),
            Is.Null) ;
}
