namespace Sonny.Application.Domain.Entities.FramingFromCad.Models ;

/// <summary>
///     A paired beam that has been created, paired with the direction its justification must be decided
///     from. Collected during the main transaction and consumed by the second one
/// </summary>
public class CreatedPairedBeam
{
    /// <summary>
    ///     UniqueId of the created beam instance
    /// </summary>
    public string UniqueId { get ; set ; } = string.Empty ;

    /// <summary>
    ///     Direction from the beam axis across to the stroke it was paired with
    /// </summary>
    public Vector3D Normal { get ; set ; } = null! ;
}
