namespace Sonny.Application.Domain.Entities.AutoJoin.Models ;

/// <summary>
///     Persisted AutoJoin state: the last rule table the user ran with
/// </summary>
public class AutoJoinSettings
{
    /// <summary>
    ///     The saved rule table
    /// </summary>
    public List<AutoJoinRule> Rules { get ; set ; } = [] ;
}
