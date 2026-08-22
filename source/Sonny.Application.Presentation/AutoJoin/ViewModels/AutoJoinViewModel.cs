using System.Collections ;
using System.Collections.ObjectModel ;
using System.IO ;
using Microsoft.Win32 ;
using Sonny.Application.Domain.Entities.AutoJoin.Models ;
using Sonny.Application.Domain.Services ;
using Sonny.Application.Presentation.AutoJoin.Models ;
using Sonny.Application.Presentation.Bases ;
using Sonny.Application.Presentation.Services ;
using Sonny.Application.UseCases.AutoJoin.Implements ;
using Sonny.Application.UseCases.AutoJoin.Services ;
#if NETCOREAPP
using System.Text.Json ;
#else
using Newtonsoft.Json ;
#endif

namespace Sonny.Application.Presentation.AutoJoin.ViewModels ;

public partial class AutoJoinViewModel : BaseViewModelWithSettings<AutoJoinSettings>
{
    #region Services

    private readonly IAutoJoinInteractor _interactor ;

    private readonly IAutoJoinScopeReader _scopeReader ;

    private readonly IRevitTaskRunner _revitTaskRunner ;

    #endregion

    private bool _hasSelection ;

    #region Constructor

    public AutoJoinViewModel(ICommonServices commonServices,
        IDisplayUnitProvider displayUnitProvider,
        IAutoJoinInteractor interactor,
        IAutoJoinScopeReader scopeReader,
        IRevitTaskRunner revitTaskRunner,
        IViewModelSettingsService<AutoJoinSettings> settingsService) : base(commonServices,
        displayUnitProvider,
        settingsService)
    {
        _interactor = interactor ;
        _scopeReader = scopeReader ;
        _revitTaskRunner = revitTaskRunner ;

        InitializeWithSettings() ;

        // No selection, no saved settings: the original seeds one (Beam, <All>) rule
        if (! _hasSelection
            && AllRules.Count == 0) {
            AllRules.Add(new AutoJoinRuleItem()) ;
        }
    }

    #endregion

    #region Properties for UI Binding

    public ObservableCollection<AutoJoinRuleItem> AllRules { get ; } = [] ;

    public IReadOnlyList<string> AllPriorityCategories { get ; } = JoinCategoryDisplay.PriorityNames ;

    public IReadOnlyList<string> AllCutCategories { get ; } = JoinCategoryDisplay.CutNames ;

    [ObservableProperty]
    private bool isUnjoin ;

    [ObservableProperty]
    private bool isAcceptWarning ;

    [ObservableProperty]
    private bool isCutSelectedElements ;

    [ObservableProperty]
    private bool isCutOtherElements ;

    /// <summary>
    ///     Whether the rule grid and the UnJoin checkbox are usable — the cut modes disable both
    /// </summary>
    [ObservableProperty]
    private bool isEnabledScope = true ;

    #endregion

    #region Event Handlers

    partial void OnIsCutSelectedElementsChanged(bool value)
    {
        if (value) {
            IsCutOtherElements = false ;
        }

        UpdateScopeEnabled() ;
    }

    partial void OnIsCutOtherElementsChanged(bool value)
    {
        if (value) {
            IsCutSelectedElements = false ;
        }

        UpdateScopeEnabled() ;
    }

    private void UpdateScopeEnabled() => IsEnabledScope = ! IsCutSelectedElements && ! IsCutOtherElements ;

    #endregion

    #region Commands

    [RelayCommand]
    private async Task Run()
    {
        // The rule table is saved even when the run fails afterwards (ported behaviour)
        SaveSettings() ;
        CloseWindow() ;

        try {
            var input = new AutoJoinInput
            {
                Mode = IsCutSelectedElements
                    ? AutoJoinMode.CutSelectedElements
                    : IsCutOtherElements
                        ? AutoJoinMode.CutOtherElements
                        : AutoJoinMode.RuleBased,
                Rules = AllRules.Select(item => item.ToRule())
                    .ToList(),
                IsUnjoin = IsUnjoin,
                IsAcceptWarnings = IsAcceptWarning
            } ;

            await _revitTaskRunner.RunAsync(() => _interactor.Execute(input)) ;
        }
        catch (Exception ex) {
            LogError("Error occurred during auto join",
                ex) ;
            ShowError(ResourceHelper.GetString("MessageAutoJoinError",
                ex.Message)) ;
        }
    }

    [RelayCommand]
    private void NewRule()
    {
        if (AllRules.Count > 0) {
            var last = AllRules[AllRules.Count - 1] ;
            AllRules.Add(new AutoJoinRuleItem
            {
                PriorityCategory = last.PriorityCategory,
                JoinWithCategory = last.JoinWithCategory,
                IsReverse = last.IsReverse
            }) ;
            return ;
        }

        // Ported quirk: the empty-table fallback is (Beam, Architectural Column), not (Beam, <All>)
        AllRules.Add(new AutoJoinRuleItem
        {
            JoinWithCategory = JoinCategoryDisplay.ToName(JoinCategory.ArchitecturalColumn)
        }) ;
    }

    [RelayCommand]
    private void DeleteRules(IList? selectedItems)
    {
        if (selectedItems == null) {
            return ;
        }

        // Copy first: removing while iterating the live selection throws
        var rulesToRemove = selectedItems.OfType<AutoJoinRuleItem>()
            .ToList() ;
        foreach (var rule in rulesToRemove) {
            AllRules.Remove(rule) ;
        }
    }

    [RelayCommand]
    private void SaveRulesToFile()
    {
        var dialog = new SaveFileDialog
        {
            Title = "Save Auto Join Rule file", Filter = "Json file(*.json)|*.json", FileName = "Auto_Join_Rules"
        } ;
        if (dialog.ShowDialog() != true) {
            return ;
        }

        try {
            File.WriteAllText(dialog.FileName,
                SerializeRules()) ;
        }
        catch (Exception ex) {
            // F13: silent for the user, logged for us
            LogWarning($"Failed to save join rules: {ex.Message}") ;
        }
    }

    [RelayCommand]
    private void LoadRulesFromFile()
    {
        var dialog = new OpenFileDialog
        {
            Title = "Select Auto Join Rule file", Filter = "Json file(*.json)|*.json"
        } ;
        if (dialog.ShowDialog() != true) {
            // Ported behaviour: cancelling the dialog still tells the user what was expected
            ShowInfo(ResourceHelper.GetString("MessageSelectRuleFile")) ;
            return ;
        }

        try {
            var rules = DeserializeRules(File.ReadAllText(dialog.FileName)) ;
            AllRules.Clear() ;
            foreach (var rule in rules) {
                AllRules.Add(AutoJoinRuleItem.FromRule(rule)) ;
            }
        }
        catch (Exception ex) {
            // F13: a broken file leaves the current table untouched, silently
            LogWarning($"Failed to load join rules: {ex.Message}") ;
        }
    }

    #endregion

    #region Settings

    protected override void OnDataInitialized()
    {
        _hasSelection = _scopeReader.HasSelectedElements() ;
        if (! _hasSelection) {
            return ;
        }

        // Seed one rule from the selection's category (D4 mapping, quirks included). An unmapped
        // or model-category-less selection leaves the table empty — see docs/features/AutoJoin.md
        if (_scopeReader.GetFirstSelectedModelCategoryId() is { } categoryId
            && AutoJoinSeedMapper.CreateSeedRule(categoryId) is { } seedRule) {
            AllRules.Add(AutoJoinRuleItem.FromRule(seedRule)) ;
        }
    }

    protected override void ApplySettings(AutoJoinSettings settings)
    {
        // A selection-seeded table wins over the saved one (ported behaviour)
        if (_hasSelection) {
            return ;
        }

        AllRules.Clear() ;
        foreach (var rule in settings.Rules) {
            AllRules.Add(AutoJoinRuleItem.FromRule(rule)) ;
        }
    }

    protected override AutoJoinSettings CreateSettings() =>
        new()
        {
            Rules = AllRules.Select(item => item.ToRule())
                .ToList()
        } ;

    #endregion

    #region Private Methods

    private string SerializeRules()
    {
        var rules = AllRules.Select(item => item.ToRule())
            .ToList() ;
#if NETCOREAPP
        return JsonSerializer.Serialize(rules,
            new JsonSerializerOptions { WriteIndented = true }) ;
#else
        return JsonConvert.SerializeObject(rules,
            Formatting.Indented) ;
#endif
    }

    private static List<AutoJoinRule> DeserializeRules(string json)
    {
#if NETCOREAPP
        return JsonSerializer.Deserialize<List<AutoJoinRule>>(json) ?? [] ;
#else
        return JsonConvert.DeserializeObject<List<AutoJoinRule>>(json) ?? [] ;
#endif
    }

    #endregion
}
