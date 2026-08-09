using System.Collections.ObjectModel ;
using Sonny.Application.Domain.Entities.Settings ;
using Sonny.Application.Domain.Entities.Settings.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.Bases ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.UseCases.AutoColumnDimension.Services ;

namespace Sonny.Application.Presentation.AutoColumnDimension.ViewModels ;

public partial class AutoColumnDimensionViewModel : BaseViewModel
{
    #region Services

    private readonly IAutoColumnDimensionInteractor _interactor ;

    private readonly IDimensionTypeProvider _dimensionTypeProvider ;

    private readonly IViewScaleProvider _viewScaleProvider ;

    private readonly IRevitTaskRunner _revitTaskRunner ;

    #endregion

    #region Constructor

    public AutoColumnDimensionViewModel(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider,
        IAutoColumnDimensionInteractor interactor,
        IDimensionTypeProvider dimensionTypeProvider,
        IViewScaleProvider viewScaleProvider,
        IRevitTaskRunner revitTaskRunner) : base(commonServices,
        displayUnitProvider)
    {
        _interactor = interactor ;
        _dimensionTypeProvider = dimensionTypeProvider ;
        _viewScaleProvider = viewScaleProvider ;
        _revitTaskRunner = revitTaskRunner ;

        // Initialize data synchronously
        InitializeData() ;
    }

    #endregion

    #region Properties for UI Binding

    [ObservableProperty]
    private DimensionTypeModel? selectedDimensionType ;

    public ObservableCollection<DimensionTypeModel> DimensionTypes { get ; set ; } = [] ;

    [ObservableProperty]
    private double snapDistanceDisplay ;

    /// <summary>
    ///     Snap distance in internal unit (feet) for calculation
    /// </summary>
    private double SnapDistanceInternal =>
        UnitConverter.ToInternalUnit(snapDistanceDisplay,
            DisplayUnit) ;

    #endregion

    #region Commands

    [RelayCommand]
    private async Task Run()
    {
        try {
            // Calculate snap distance: convert to internal unit and multiply by view scale
            var snapDistance = SnapDistanceInternal * _viewScaleProvider.GetActiveViewScale() ;
            var dimensionTypeUniqueId = SelectedDimensionType?.UniqueId ;

            await _revitTaskRunner.RunAsync(() => _interactor.Execute(snapDistance,
                dimensionTypeUniqueId)) ;

            // Close window after successful execution
            CloseWindow() ;
        }
        catch (Exception ex) {
            LogError("Error occurred during dimension creation",
                ex) ;
            ShowError(ResourceHelper.GetString("MessageErrorOccurred",
                ex.Message)) ;
        }
    }

    #endregion

    #region Event Handlers

    partial void OnSelectedDimensionTypeChanged(DimensionTypeModel? value) => UpdateSnapDistanceFromDimensionType() ;

    protected override void OnDisplayUnitChanged(AppDisplayUnit oldUnit,
        AppDisplayUnit newUnit)
    {
        base.OnDisplayUnitChanged(oldUnit,
            newUnit) ;

        // Convert snap distance from old unit to new unit
        if (SnapDistanceDisplay != 0) {
            try {
                // Convert current value to internal unit (feet), then to new display unit
                var valueInFeet = UnitConverter.ToInternalUnit(SnapDistanceDisplay,
                    oldUnit) ;
                SnapDistanceDisplay = UnitConverter.FromInternalUnit(valueInFeet,
                    newUnit) ;
            }
            catch (Exception ex) {
                LogWarning($"Failed to convert snap distance when unit changed: {ex.Message}") ;
            }
        }
    }

    #endregion

    #region Private Methods - Initialization

    private void InitializeData()
    {
        try {
            // Get dimension types from provider (Domain models)
            var dimensionTypes = _dimensionTypeProvider.GetDimensionTypes() ;

            DimensionTypes = new ObservableCollection<DimensionTypeModel>(dimensionTypes) ;
            SelectedDimensionType = DimensionTypes.LastOrDefault() ;

            // Update snap distance
            UpdateSnapDistanceFromDimensionType() ;
        }
        catch (Exception ex) {
            LogError("Failed to initialize dimension types",
                ex) ;
            ShowError(ResourceHelper.GetString("MessageFailedToInitialize",
                ex.Message)) ;
        }
    }

    #endregion

    #region Private Methods - UI Updates

    private void UpdateSnapDistanceFromDimensionType()
    {
        if (SelectedDimensionType is not { } dimensionType) {
            return ;
        }

        try {
            // Get snap distance from Domain model (already in feet)
            var snapDistanceFeet = dimensionType.SnapDistance ;

            // Convert from internal unit (feet) to display unit (mm, cm, etc.)
            SnapDistanceDisplay = UnitConverter.FromInternalUnit(snapDistanceFeet,
                DisplayUnit) ;
        }
        catch (Exception ex) {
            LogWarning($"Failed to update snap distance: {ex.Message}") ;
        }
    }

    #endregion
}
