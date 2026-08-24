using Sonny.Application.Domain.Entities ;
using Sonny.Application.UseCases.AutoColumnDimension.Implements ;
using Sonny.Application.UseCases.AutoColumnDimension.Models ;

namespace Sonny.Application.UnitTests.UseCases.AutoColumnDimension ;

/// <summary>
///     Characterization tests for <see cref="ColumnDimensionPolicy" /> — pin the geometric
///     decisions exactly as ColumnDimensionContext.Create made them (ADR 0001), most importantly
///     the crossed grid lookup that looks like a typo and is not.
/// </summary>
public class ColumnDimensionPolicyTests
{
    private static readonly Point3D s_handOrientation = new(1,
        0,
        0) ;

    private static readonly Point3D s_facingOrientation = new(0,
        1,
        0) ;

    [Test]
    public void CreatePlan_NoBoundingBox_ReturnsNull()
    {
        var column = new ColumnGeometryData("column-1",
            null,
            null,
            s_handOrientation,
            s_facingOrientation) ;

        var plan = ColumnDimensionPolicy.CreatePlan(column,
            PlanView(),
            []) ;

        Assert.That(plan,
            Is.Null) ;
    }

    [Test]
    public void CreatePlan_PlanView_GridLookupIsCrossed()
    {
        // THE deliberate oddity: the FIRST axis (measuring along hand X) references the grid
        // running along FACING (Y), and vice versa
        var gridAlongX = new GridCandidate("grid-x",
            new Point3D(1,
                0,
                0),
            new Point3D(0,
                5,
                0)) ;
        var gridAlongY = new GridCandidate("grid-y",
            new Point3D(0,
                1,
                0),
            new Point3D(5,
                0,
                0)) ;

        var plan = ColumnDimensionPolicy.CreatePlan(Column(),
            PlanView(),
            [gridAlongX, gridAlongY]) ;

        Assert.That(plan,
            Is.Not.Null) ;
        Assert.Multiple(() =>
        {
            Assert.That(plan!.FirstAxis.GridUniqueId,
                Is.EqualTo("grid-y")) ;
            Assert.That(plan.SecondAxis.GridUniqueId,
                Is.EqualTo("grid-x")) ;
            Assert.That(plan.FirstAxis.Direction.X,
                Is.EqualTo(1)) ;
            Assert.That(plan.FirstAxis.OffsetDirection.Y,
                Is.EqualTo(1)) ;
        }) ;
    }

    [Test]
    public void CreatePlan_TwoParallelGrids_PicksTheNearestAlongTheProductDirection()
    {
        // Both grids run along Y; the first axis measures along X (hand), so proximity is the
        // X distance from the column midpoint (0,0,0)
        var farGrid = new GridCandidate("far",
            new Point3D(0,
                1,
                0),
            new Point3D(5,
                0,
                0)) ;
        var nearGrid = new GridCandidate("near",
            new Point3D(0,
                1,
                0),
            new Point3D(2,
                0,
                0)) ;

        var plan = ColumnDimensionPolicy.CreatePlan(Column(),
            PlanView(),
            [farGrid, nearGrid]) ;

        Assert.That(plan!.FirstAxis.GridUniqueId,
            Is.EqualTo("near")) ;
    }

    [Test]
    public void CreatePlan_AntiParallelGridCountsAsParallel()
    {
        var reversedGrid = new GridCandidate("reversed",
            new Point3D(0,
                -1,
                0),
            new Point3D(2,
                0,
                0)) ;

        var plan = ColumnDimensionPolicy.CreatePlan(Column(),
            PlanView(),
            [reversedGrid]) ;

        Assert.That(plan!.FirstAxis.GridUniqueId,
            Is.EqualTo("reversed")) ;
    }

    [Test]
    public void CreatePlan_NoParallelGrid_LeavesGridNullButStillPlans()
    {
        var plan = ColumnDimensionPolicy.CreatePlan(Column(),
            PlanView(),
            []) ;

        Assert.That(plan,
            Is.Not.Null) ;
        Assert.Multiple(() =>
        {
            Assert.That(plan!.FirstAxis.GridUniqueId,
                Is.Null) ;
            Assert.That(plan.SecondAxis.GridUniqueId,
                Is.Null) ;
        }) ;
    }

    [Test]
    public void CreatePlan_NonPlanView_VerticalAxisGetsNoGridLookup()
    {
        // Elevation-like view: up is vertical (parallel to BasisZ) so the first axis gets no
        // grid even though a matching candidate exists; the horizontal axis looks up its grid
        // by the VIEW direction
        var view = new ViewGeometryData("Section A",
            false,
            new Point3D(0,
                0,
                1),
            new Point3D(1,
                0,
                0),
            new Point3D(0,
                1,
                0)) ;
        var gridAlongViewDirection = new GridCandidate("grid-view",
            new Point3D(0,
                1,
                0),
            new Point3D(2,
                0,
                0)) ;

        var plan = ColumnDimensionPolicy.CreatePlan(Column(),
            view,
            [gridAlongViewDirection]) ;

        Assert.Multiple(() =>
        {
            Assert.That(plan!.FirstAxis.GridUniqueId,
                Is.Null) ;
            Assert.That(plan.SecondAxis.GridUniqueId,
                Is.EqualTo("grid-view")) ;
            Assert.That(plan.FirstAxis.Direction.Z,
                Is.EqualTo(1)) ;
        }) ;
    }

    [Test]
    public void CreatePlan_MaxPointIsTheBoundingBoxMaximum()
    {
        var plan = ColumnDimensionPolicy.CreatePlan(Column(),
            PlanView(),
            []) ;

        Assert.Multiple(() =>
        {
            Assert.That(plan!.MaxPoint.X,
                Is.EqualTo(1)) ;
            Assert.That(plan.MaxPoint.Y,
                Is.EqualTo(1)) ;
            Assert.That(plan.MaxPoint.Z,
                Is.EqualTo(0)) ;
        }) ;
    }

    private static ColumnGeometryData Column() =>
        new("column-1",
            new Point3D(-1,
                -1,
                0),
            new Point3D(1,
                1,
                0),
            s_handOrientation,
            s_facingOrientation) ;

    private static ViewGeometryData PlanView() =>
        new("Level 1",
            true,
            new Point3D(0,
                1,
                0),
            new Point3D(1,
                0,
                0),
            new Point3D(0,
                0,
                -1)) ;
}
