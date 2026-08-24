using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.AutoJoin.Models ;
using Sonny.Application.UseCases.AutoJoin.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Elements ;
using Sonny.RevitExtensions.Extensions.GeometryObjects.Solids ;

namespace Sonny.Application.Infrastructure.Features.AutoJoin.Implements ;

/// <summary>
///     Revit adapter behind IAutoJoinPairExecutor. Holds the anchor's solid SNAPSHOT for the
///     current outer-loop element — joins made while its pairs execute must not change what the
///     checks compare against (ported behaviour). Transient: the snapshot is per-run state.
/// </summary>
public class AutoJoinPairExecutor(IRevitDocument revitDocument) : IAutoJoinPairExecutor
{
    private Solid? _anchorSolid ;

    public void BeginAnchor(long anchorElementId)
    {
        var element = revitDocument.Document.GetElement(anchorElementId.ToElementId()) ;
        _anchorSolid = element?.GetSolidMax() ;
    }

    public SolidIntersectCheck CheckIntersectWithAnchor(long otherElementId)
    {
        if (_anchorSolid == null) {
            return SolidIntersectCheck.NoAnchorSolid ;
        }

        try {
            var otherElement = revitDocument.Document.GetElement(otherElementId.ToElementId()) ;
            var otherSolid = otherElement?.GetSolidMax() ;
            if (otherSolid == null) {
                return SolidIntersectCheck.NotIntersecting ;
            }

            return otherSolid.IntersectsSolidByUnion(_anchorSolid)
                ? SolidIntersectCheck.Intersecting
                : SolidIntersectCheck.NotIntersecting ;
        }
        catch (Exception) {
            // F8 (D2): an unverifiable pair is treated as intersecting by the interactor
            return SolidIntersectCheck.CheckFailed ;
        }
    }

    public bool TryExecuteJoin(long priorityElementId,
        long targetElementId,
        bool isJoin,
        bool isReverse)
    {
        try {
            var document = revitDocument.Document ;
            var priorityElement = document.GetElement(priorityElementId.ToElementId()) ;
            var targetElement = document.GetElement(targetElementId.ToElementId()) ;

            if (isJoin) {
                priorityElement.JoinAsCutting(targetElement,
                    isReverse) ;
            }
            else {
                priorityElement.UnjoinIfJoined(targetElement) ;
            }

            return true ;
        }
        catch (Exception) {
            // F9 (D3): the interactor logs the pair; nothing surfaces to the user
            return false ;
        }
    }
}
