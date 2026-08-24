using System.Collections.ObjectModel ;
using Sonny.Application.Domain.Entities.FramingFromCad ;
using Sonny.Application.Domain.Entities.FramingFromCad.Contexts ;
using Sonny.Application.Domain.Entities.FramingFromCad.Models ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Entities.Settings.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Bases ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.UseCases.FramingFromCad.Services ;

namespace Sonny.Application.Presentation.FramingFromCad.ViewModels ;

/// <summary>
///     Dialog state for FramingFromCad. This is the unit boundary: everything the user types is in display
///     units, and this class is the only place that converts to feet before handing a context to the
///     interactor
/// </summary>
public partial class FramingFromCadViewModel : BaseViewModelWithSettings<FramingFromCadSettings>
{
    #region Services

    private readonly IFramingFromCadInteractor _framingFromCadInteractor ;

    private readonly IFramingFromCadContext _context ;

    #endregion

    #region Constructor

    public FramingFromCadViewModel(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider,
        IFramingFromCadInteractor framingFromCadInteractor,
        IFramingFromCadContext context,
        IViewModelSettingsService<FramingFromCadSettings> settingsService) : base(commonServices,
        displayUnitProvider,
        settingsService)
    {
        _framingFromCadInteractor = framingFromCadInteractor ;
        _context = context ;

        InitializeWithSettings() ;
    }

    #endregion

    #region Properties for UI Binding

    /// <summary>
    ///     All available layers from the CAD link
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _allLayerNames = [] ;

    /// <summary>
    ///     Layer holding the beam strokes
    /// </summary>
    [ObservableProperty]
    private string? _selectedLayer ;

    /// <summary>
    ///     All structural framing families in the project
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<FamilyModel> _allFramingFamilies = [] ;

    /// <summary>
    ///     The single family every created beam is an instance of
    /// </summary>
    [ObservableProperty]
    private FamilyModel? _selectedFramingFamily ;

    /// <summary>
    ///     Type parameters of the selected family that could carry a dimension
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _allFramingTypeParameters = [] ;

    /// <summary>
    ///     Type parameter carrying beam width, "b" by convention
    /// </summary>
    [ObservableProperty]
    private string? _widthParameter ;

    /// <summary>
    ///     Type parameter carrying beam height, "h" by convention
    /// </summary>
    [ObservableProperty]
    private string? _heightParameter ;

    /// <summary>
    ///     All levels in the project, ordered by elevation
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<LevelModel> _allLevels = [] ;

    /// <summary>
    ///     Level the beams are hosted on
    /// </summary>
    [ObservableProperty]
    private LevelModel? _referenceLevel ;

    /// <summary>
    ///     Vertical offset in display units
    /// </summary>
    [ObservableProperty]
    private double _zOffsetDisplay ;

    /// <summary>
    ///     Free-text section list, e.g. "200x300; 400x600", in display units. Entries that are not two
    ///     positive numbers are dropped silently when the run starts
    /// </summary>
    [ObservableProperty]
    private string? _allSectionsText ;

    /// <summary>
    ///     Whether strokes left over after pairing also become beams
    /// </summary>
    [ObservableProperty]
    private bool _isCreateForSingleLine ;

    /// <summary>
    ///     Whether both ends of every created beam are set to disallow join
    /// </summary>
    [ObservableProperty]
    private bool _isDisallowJoin ;

    #endregion

    #region Commands

    /// <summary>
    ///     Runs the feature
    /// </summary>
    [RelayCommand]
    public async Task Execute()
    {
        SaveSettings() ;

        CloseWindow() ;

        if (! ValidateInput()) {
            return ;
        }

        await _framingFromCadInteractor.Execute(CreateContext()) ;
    }

    /// <summary>
    ///     Closes the dialog without running
    /// </summary>
    [RelayCommand]
    private void Cancel() => CloseWindow() ;

    #endregion

    #region Event Handlers

    partial void OnSelectedFramingFamilyChanged(FamilyModel? value)
    {
        if (value == null) {
            return ;
        }

        LoadFramingTypeParameters(value) ;
    }

    #endregion

    #region Unit Conversion

    /// <summary>
    ///     Vertical offset in internal units (feet)
    /// </summary>
    public double ZOffsetInternal =>
        UnitConverter.ToInternalUnit(ZOffsetDisplay,
            DisplayUnit) ;

    /// <summary>
    ///     Converts the offset when the user switches the project's display unit
    /// </summary>
    protected override void OnDisplayUnitChanged(AppDisplayUnit oldUnit,
        AppDisplayUnit newUnit)
    {
        var zOffsetInternal = UnitConverter.ToInternalUnit(ZOffsetDisplay,
            oldUnit) ;
        ZOffsetDisplay = UnitConverter.FromInternalUnit(zOffsetInternal,
            newUnit) ;
    }

    /// <summary>
    ///     Builds the run context, converting every measurement to feet on the way through.
    ///     One millimetre expressed in the current display unit becomes the smallest side a generated type
    ///     name will accept
    /// </summary>
    private FramingCreationContext CreateContext()
    {
        var oneMillimetreInternal = UnitConverter.ToInternalUnit(1,
            AppDisplayUnit.Millimeters) ;

        return new FramingCreationContext {
            Settings = CreateSettings(),
            Sections = BeamSectionListParser.Parse(AllSectionsText,
                displayValue => UnitConverter.ToInternalUnit(displayValue,
                    DisplayUnit)),
            ZOffset = ZOffsetInternal,
            MinimumSizeDisplay = UnitConverter.FromInternalUnit(oneMillimetreInternal,
                DisplayUnit),
        } ;
    }

    #endregion

    #region Settings Management

    protected override void ApplySettings(FramingFromCadSettings settings)
    {
        if (! string.IsNullOrEmpty(settings.SelectedLayer)
            && AllLayerNames.Contains(settings.SelectedLayer!)) {
            SelectedLayer = settings.SelectedLayer ;
        }

        if (! string.IsNullOrEmpty(settings.SelectedFamilyId)
            && AllFramingFamilies.FirstOrDefault(family => family.UniqueId == settings.SelectedFamilyId) is
                { } family) {
            SelectedFramingFamily = family ;
            LoadFramingTypeParameters(family) ;
        }

        // Only accept a remembered parameter the selected family actually has
        if (! string.IsNullOrEmpty(settings.WidthParameter)
            && AllFramingTypeParameters.Contains(settings.WidthParameter!)) {
            WidthParameter = settings.WidthParameter ;
        }

        if (! string.IsNullOrEmpty(settings.HeightParameter)
            && AllFramingTypeParameters.Contains(settings.HeightParameter!)) {
            HeightParameter = settings.HeightParameter ;
        }

        if (! string.IsNullOrEmpty(settings.ReferenceLevelId)
            && AllLevels.FirstOrDefault(level => level.UniqueId == settings.ReferenceLevelId) is { } level) {
            ReferenceLevel = level ;
        }

        ZOffsetDisplay = settings.ZOffsetDisplay ;
        AllSectionsText = settings.AllSectionsText ;
        IsCreateForSingleLine = settings.IsCreateForSingleLine ;

        // Restored too, unlike the original implementation which left this out of its settings class
        IsDisallowJoin = settings.IsDisallowJoin ;
    }

    protected override FramingFromCadSettings CreateSettings() =>
        new() {
            SelectedCadLinkId = _context.SelectedCadLinkId,
            SelectedLayer = SelectedLayer,
            SelectedFamilyId = SelectedFramingFamily?.UniqueId,
            WidthParameter = WidthParameter,
            HeightParameter = HeightParameter,
            ReferenceLevelId = ReferenceLevel?.UniqueId,
            ZOffsetDisplay = ZOffsetDisplay,
            AllSectionsText = AllSectionsText,
            IsCreateForSingleLine = IsCreateForSingleLine,
            IsDisallowJoin = IsDisallowJoin,
        } ;

    #endregion

    #region Private Methods - Initialization

    protected override void OnDataInitialized()
    {
        AllLayerNames = new ObservableCollection<string>(_context.LayerNames) ;

        // Same guess as the original — the first layer whose name mentions a beam, else the first layer —
        // but matched case-insensitively. The original's Contains("Beam") was case-sensitive, so a real
        // layer called "S-BEAM" missed it and the dialog opened on the wrong layer. This is only the
        // default selection and the user can change it, so the friendlier match wins
        SelectedLayer = AllLayerNames.FirstOrDefault(layerName =>
                            layerName.IndexOf("beam",
                                StringComparison.OrdinalIgnoreCase)
                            >= 0)
                        ?? AllLayerNames.FirstOrDefault() ;

        LoadFramingFamilies() ;

        LoadLevels() ;
    }

    private void LoadFramingFamilies()
    {
        AllFramingFamilies = new ObservableCollection<FamilyModel>(_context.FramingFamilies) ;
        SelectedFramingFamily = AllFramingFamilies.FirstOrDefault() ;

        if (SelectedFramingFamily is { } family) {
            LoadFramingTypeParameters(family) ;
        }
    }

    /// <summary>
    ///     Picks "b" and "h" when the family has them, which is what the concrete beam families in use
    ///     call their dimensions; otherwise falls back to the first parameter
    /// </summary>
    private void LoadFramingTypeParameters(FamilyModel family)
    {
        if (! _context.FamilyNumericParameters.TryGetValue(family.UniqueId,
                out var parameterNames)) {
            AllFramingTypeParameters = [] ;
            WidthParameter = null ;
            HeightParameter = null ;

            return ;
        }

        AllFramingTypeParameters = new ObservableCollection<string>(parameterNames) ;

        WidthParameter = AllFramingTypeParameters.FirstOrDefault(name => name == "b")
                         ?? AllFramingTypeParameters.FirstOrDefault() ;
        HeightParameter = AllFramingTypeParameters.FirstOrDefault(name => name == "h")
                          ?? AllFramingTypeParameters.FirstOrDefault() ;
    }

    /// <summary>
    ///     Defaults to the level of the active view, as the original did
    /// </summary>
    private void LoadLevels()
    {
        AllLevels = new ObservableCollection<LevelModel>(_context.Levels) ;
        ReferenceLevel = AllLevels.FirstOrDefault() ;
    }

    #endregion

    #region Validation

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(SelectedLayer)) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectLayer")) ;

            return false ;
        }

        if (SelectedFramingFamily == null) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectFramingFamily")) ;

            return false ;
        }

        if (string.IsNullOrEmpty(WidthParameter)) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectWidthParameter")) ;

            return false ;
        }

        if (string.IsNullOrEmpty(HeightParameter)) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectHeightParameter")) ;

            return false ;
        }

        if (ReferenceLevel == null) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectReferenceLevel")) ;

            return false ;
        }

        return true ;
    }

    #endregion
}
