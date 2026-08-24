using Sonny.Application.Domain.Entities.Settings.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Infrastructure.Revit.Services ;
using Sonny.Application.UseCases.FramingFromCad.Services ;
using Sonny.RevitExtensions.Extensions ;
using Sonny.RevitExtensions.Extensions.Families ;

namespace Sonny.Application.Infrastructure.Features.FramingFromCad.Implements ;

/// <summary>
///     Gathers what the FramingFromCad dialog needs, and picks the CAD link while doing so. Construction
///     is therefore the cancel point: an ESC on the pick throws, the command reports and the dialog never
///     opens — same order as the original implementation, which picked before showing its window
/// </summary>
public class FramingFromCadContext : IFramingFromCadContext
{
    public string SelectedCadLinkId { get ; }
    public HashSet<string> LayerNames { get ; }
    public List<FamilyModel> FramingFamilies { get ; }
    public List<LevelModel> Levels { get ; }
    public Dictionary<string, HashSet<string>> FamilyNumericParameters { get ; }

    public FramingFromCadContext(ICadLinkSelector cadLinkSelector,
        IRevitDocument revitDocument,
        IResourceHelper resourceHelper)
    {
        var uiDocument = revitDocument.UIDocument ;

        if (cadLinkSelector.SelectCadLink(uiDocument) is not { } selectedCadLink) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageFailedToSelectCadLink")) ;
        }

        var layerNames = selectedCadLink.GetAllLayerNames(true) ;
        if (layerNames.Count == 0) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageNoLayersFoundInCadLink")) ;
        }

        var document = uiDocument.Document ;
        var structuralFraming = Category.GetCategory(document,
            BuiltInCategory.OST_StructuralFraming) ;

        var families = document.GetAllElements<Family>()
            .Where(family => family.FamilyCategory != null
                             && family.FamilyCategory.Id.Equals(structuralFraming.Id))
            .Where(family => family.GetFamilySymbolIds()
                .Any())
            .OrderBy(family => family.Name)
            .ToList() ;

        if (families.Count == 0) {
            throw new InvalidOperationException(resourceHelper.GetString("MessageNoFramingFamiliesFound")) ;
        }

        var familyModels = families.Select(family => new FamilyModel(family.UniqueId,
                family.Name))
            .ToList() ;

        // Same filter list as the original: what is left is what a user might pick as width or height
        var familyParameters = new Dictionary<string, HashSet<string>>() ;
        foreach (var family in families) {
            if (family.GetFamilySymbols()
                    .FirstOrDefault() is not { } familySymbol) {
                continue ;
            }

            var parameterNames = familySymbol.Parameters
                .Cast<Parameter>()
                .Where(parameter => parameter.StorageType is StorageType.Double or StorageType.Integer)
                .Where(parameter => ! parameter.Definition.Name.Contains("Assembly"))
                .Where(parameter => ! parameter.Definition.Name.Contains("OmniClass"))
                .Where(parameter => ! parameter.Definition.Name.Contains("Material"))
                .Where(parameter => ! parameter.Definition.Name.Contains("Category"))
                .Where(parameter => ! parameter.Definition.Name.Contains("Type"))
                .Select(parameter => parameter.Definition.Name)
                .ToHashSet() ;
            if (parameterNames.Count == 0) {
                continue ;
            }

            familyParameters[family.UniqueId] = parameterNames ;
        }

        var levels = document.GetAllElements<Level>()
            .OrderBy(level => level.Elevation)
            .Select(level => new LevelModel(level.UniqueId,
                level.Name))
            .ToList() ;

        SelectedCadLinkId = selectedCadLink.UniqueId ;
        LayerNames = layerNames ;
        FramingFamilies = familyModels ;
        Levels = levels ;
        FamilyNumericParameters = familyParameters ;
    }
}
