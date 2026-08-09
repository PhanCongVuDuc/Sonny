using System.Collections.ObjectModel ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Contexts ;
using Sonny.Application.Domain.Entities.ColumnFromCad.Models ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Entities.Settings.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Bases ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.UseCases.ColumnFromCad.Services ;

namespace Sonny.Application.Presentation.ColumnFromCad.ViewModels ;

public partial class ColumnFromCadViewModel : BaseViewModelWithSettings<ColumnFromCadSettings>
{
    #region Services

    private readonly IColumnFromCadInteractor _columnFromCadInteractor ;

    private readonly IColumnFromCadContext _context ;

    #endregion

    #region Constructor

    public ColumnFromCadViewModel(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider,
        IColumnFromCadInteractor columnFromCadInteractor,
        IColumnFromCadContext context,
        IViewModelSettingsService<ColumnFromCadSettings> settingsService) : base(commonServices,
        displayUnitProvider,
        settingsService)
    {
        _columnFromCadInteractor = columnFromCadInteractor ;
        _context = context ;

        InitializeWithSettings() ;
    }

    #endregion

    #region Properties for UI Binding

    [ObservableProperty]
    private ObservableCollection<string> _allLayerNames = [] ;

    [ObservableProperty]
    private string? _selectedLayer ;

    /// <summary>
    ///     Whether to model by hatch (true) or boundary (false)
    /// </summary>
    [ObservableProperty]
    private bool _isModelByHatch = true ;

    [ObservableProperty]
    private bool _isModelByBoundary ;

    [ObservableProperty]
    private ObservableCollection<FamilyModel> _allColumnFamilies = [] ;

    [ObservableProperty]
    private FamilyModel? _selectedRectangularColumnFamily ;

    [ObservableProperty]
    private FamilyModel? _selectedCircularColumnFamily ;

    /// <summary>
    ///     All available type parameters for rectangular columns (Width, Height)
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _allRectangularColumnTypeParameters = [] ;

    /// <summary>
    ///     All available type parameters for circular columns (Diameter)
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<string> _allCircularColumnTypeParameters = [] ;

    [ObservableProperty]
    private string? _widthParameter ;

    [ObservableProperty]
    private string? _heightParameter ;

    [ObservableProperty]
    private string? _diameterParameter ;

    [ObservableProperty]
    private ObservableCollection<LevelModel> _allLevels = [] ;

    [ObservableProperty]
    private LevelModel? _baseLevel ;

    [ObservableProperty]
    private LevelModel? _topLevel ;

    [ObservableProperty]
    private double _baseOffsetDisplay ;

    [ObservableProperty]
    private double _topOffsetDisplay ;

    #endregion

    #region Commands

    [RelayCommand]
    public async Task Execute()
    {
        // Save settings before executing
        SaveSettings() ;

        // Close settings window first
        CloseWindow() ;

        if (! ValidateInput()) {
            return ;
        }

        var context = new ColumnCreationContext
        {
            Settings = CreateSettings(), BaseOffset = BaseOffsetInternal, TopOffset = TopOffsetInternal
        } ;

        // Extract column data
        await _columnFromCadInteractor.Execute(context) ;
    }

    [RelayCommand]
    private void Cancel() => CloseWindow() ;

    #endregion

    #region Event Handlers

    partial void OnSelectedRectangularColumnFamilyChanged(FamilyModel? value)
    {
        if (value == null) {
            return ;
        }

        LoadRectangularColumnParameters(value) ;
    }

    partial void OnSelectedCircularColumnFamilyChanged(FamilyModel? value)
    {
        if (value == null) {
            return ;
        }

        LoadCircularColumnParameters(value) ;
    }

    #endregion

    #region Unit Conversion

    /// <summary>
    ///     Gets base offset in internal unit (feet)
    /// </summary>
    public double BaseOffsetInternal =>
        UnitConverter.ToInternalUnit(BaseOffsetDisplay,
            DisplayUnit) ;

    /// <summary>
    ///     Gets top offset in internal unit (feet)
    /// </summary>
    public double TopOffsetInternal =>
        UnitConverter.ToInternalUnit(TopOffsetDisplay,
            DisplayUnit) ;

    /// <summary>
    ///     Called when display unit changes, converts offset values to new unit
    /// </summary>
    protected override void OnDisplayUnitChanged(AppDisplayUnit oldUnit,
        AppDisplayUnit newUnit)
    {
        // Convert BaseOffsetDisplay from old unit to new unit
        var baseOffsetInternal = UnitConverter.ToInternalUnit(BaseOffsetDisplay,
            oldUnit) ;
        BaseOffsetDisplay = UnitConverter.FromInternalUnit(baseOffsetInternal,
            newUnit) ;

        // Convert TopOffsetDisplay from old unit to new unit
        var topOffsetInternal = UnitConverter.ToInternalUnit(TopOffsetDisplay,
            oldUnit) ;
        TopOffsetDisplay = UnitConverter.FromInternalUnit(topOffsetInternal,
            newUnit) ;
    }

    #endregion

    #region Settings Management

    protected override void ApplySettings(ColumnFromCadSettings settings)
    {
        // Load layer selection
        if (! string.IsNullOrEmpty(settings.SelectedLayer)
            && AllLayerNames.Contains(settings.SelectedLayer)) {
            SelectedLayer = settings.SelectedLayer ;
        }

        // Load modeling method
        IsModelByHatch = settings.IsModelByHatch ;
        IsModelByBoundary = ! settings.IsModelByHatch ;

        // Load column families (must be after LoadColumnFamilies is called)
        if (! string.IsNullOrEmpty(settings.RectangularColumnFamilyId)) {
            var rectangularFamily =
                AllColumnFamilies.FirstOrDefault(f => f.UniqueId == settings.RectangularColumnFamilyId) ;
            if (rectangularFamily != null) {
                SelectedRectangularColumnFamily = rectangularFamily ;
                LoadRectangularColumnParameters(rectangularFamily) ;
            }
        }

        if (! string.IsNullOrEmpty(settings.CircularColumnFamilyId)) {
            var circularFamily = AllColumnFamilies.FirstOrDefault(f => f.UniqueId == settings.CircularColumnFamilyId) ;
            if (circularFamily != null) {
                SelectedCircularColumnFamily = circularFamily ;
                LoadCircularColumnParameters(circularFamily) ;
            }
        }

        // Load parameters
        if (! string.IsNullOrEmpty(settings.WidthParameter)) {
            WidthParameter = settings.WidthParameter ;
        }

        if (! string.IsNullOrEmpty(settings.HeightParameter)) {
            HeightParameter = settings.HeightParameter ;
        }

        if (! string.IsNullOrEmpty(settings.DiameterParameter)) {
            DiameterParameter = settings.DiameterParameter ;
        }

        // Load levels (must be after LoadLevels is called)
        if (! string.IsNullOrEmpty(settings.BaseLevelId)) {
            var baseLevel = AllLevels.FirstOrDefault(l => l.UniqueId == settings.BaseLevelId) ;
            if (baseLevel != null) {
                BaseLevel = baseLevel ;
            }
        }

        if (! string.IsNullOrEmpty(settings.TopLevelId)) {
            var topLevel = AllLevels.FirstOrDefault(l => l.UniqueId == settings.TopLevelId) ;
            if (topLevel != null) {
                TopLevel = topLevel ;
            }
        }

        // Load offsets
        BaseOffsetDisplay = settings.BaseOffsetDisplay ;
        TopOffsetDisplay = settings.TopOffsetDisplay ;
    }

    protected override ColumnFromCadSettings CreateSettings() =>
        new()
        {
            SelectedCadLinkId = _context.SelectedCadLinkId,
            SelectedLayer = SelectedLayer,
            IsModelByHatch = IsModelByHatch,
            RectangularColumnFamilyId = SelectedRectangularColumnFamily?.UniqueId,
            CircularColumnFamilyId = SelectedCircularColumnFamily?.UniqueId,
            WidthParameter = WidthParameter,
            HeightParameter = HeightParameter,
            DiameterParameter = DiameterParameter,
            BaseLevelId = BaseLevel?.UniqueId,
            TopLevelId = TopLevel?.UniqueId,
            BaseOffsetDisplay = BaseOffsetDisplay,
            TopOffsetDisplay = TopOffsetDisplay
        } ;

    #endregion

    #region Private Methods - Initialization

    protected override void OnDataInitialized()
    {
        AllLayerNames = new ObservableCollection<string>(_context.LayerNames) ;
        SelectedLayer = AllLayerNames[0] ;

        LoadColumnFamilies() ;

        LoadLevels() ;
    }

    /// <summary>
    ///     Loads column families from context (business data already extracted)
    /// </summary>
    private void LoadColumnFamilies()
    {
        AllColumnFamilies = new ObservableCollection<FamilyModel>(_context.ColumnFamilies) ;
        SelectedRectangularColumnFamily = AllColumnFamilies.First() ;
        SelectedCircularColumnFamily = AllColumnFamilies.First() ;

        LoadRectangularColumnParameters(SelectedRectangularColumnFamily) ;
        LoadCircularColumnParameters(SelectedCircularColumnFamily) ;
    }

    /// <summary>
    ///     Loads rectangular column type parameters from context (business data already extracted)
    /// </summary>
    private void LoadRectangularColumnParameters(FamilyModel family)
    {
        // Get parameters from context (business data already extracted)
        var allParameters = _context.FamilyNumericParameters[family.UniqueId] ;
        AllRectangularColumnTypeParameters = new ObservableCollection<string>(allParameters) ;

        WidthParameter = AllRectangularColumnTypeParameters[0] ;
        HeightParameter = AllRectangularColumnTypeParameters.Count > 1
            ? AllRectangularColumnTypeParameters[1]
            : AllRectangularColumnTypeParameters[0] ;
    }

    /// <summary>
    ///     Loads circular column type parameters from context (business data already extracted)
    /// </summary>
    private void LoadCircularColumnParameters(FamilyModel family)
    {
        // Get parameters from context (business data already extracted)
        var allParameters = _context.FamilyNumericParameters[family.UniqueId] ;
        AllCircularColumnTypeParameters = new ObservableCollection<string>(allParameters) ;

        DiameterParameter = AllCircularColumnTypeParameters[0] ;
    }

    /// <summary>
    ///     Loads levels from context (business data already extracted)
    /// </summary>
    private void LoadLevels()
    {
        AllLevels = new ObservableCollection<LevelModel>(_context.Levels) ;

        if (AllLevels.Count <= 0) {
            return ;
        }

        BaseLevel = AllLevels[0] ;
        TopLevel = AllLevels.Count > 1 ? AllLevels[1] : AllLevels[0] ;
    }

    #endregion

    #region Validation

    private bool ValidateInput()
    {
        if (string.IsNullOrEmpty(SelectedLayer)) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectLayer")) ;
            return false ;
        }

        if (SelectedRectangularColumnFamily == null) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectRectangularColumnFamily")) ;
            return false ;
        }

        if (SelectedCircularColumnFamily == null) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectCircularColumnFamily")) ;
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

        if (string.IsNullOrEmpty(DiameterParameter)) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectDiameterParameter")) ;
            return false ;
        }

        if (BaseLevel == null) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectBaseLevel")) ;
            return false ;
        }

        if (TopLevel == null) {
            ShowError(ResourceHelper.GetString("ValidationPleaseSelectTopLevel")) ;
            return false ;
        }

        return true ;
    }

    #endregion
}
