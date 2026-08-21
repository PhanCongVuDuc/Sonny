# Graph Report - Sonny  (2026-08-21)

## Corpus Check
- 378 files · ~229,160 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 2450 nodes · 3885 edges · 204 communities (163 shown, 41 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 177 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `0368695d`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- .FromCorners
- .TryDetectCircle
- AutoColumnDimensionInteractor (documented)
- ColumnFromCadViewModel
- DimensionProcessor
- Clean Architecture
- Sonny.Application.Domain.Services
- CurveExtensions
- Sonny.EasyRibbon.Example.Commands
- ColumnModel
- ColumnFromCadInteractorTests
- LicenseFileService
- LoginViewModel
- .CreateRectangular
- Panel: General
- ColumnCreationContext
- ElementCurveExtensions
- Sonny.Application.Domain.Exceptions
- XYZVectorExtensions
- RectangularColumnModel
- Sonny.Keygen.Models
- ElementParameterExtensions
- IApplicationModule
- SonnyRevitTestBase
- Sonny.EasyRibbon.Extensions
- Sonny.Keygen
- .CreatePushButtonData
- .CollectStackedItems
- Sonny.EasyRibbon
- Window
- IElementSelector
- UnitConverterTests
- Sonny.ResourceManager
- BaseViewModel
- Sonny.EasyRibbon
- LicenseStatus
- Sonny.Keygen.Services
- LicenseService
- Window
- AutoColumnDimensionViewModel
- .GetIntersectingElements
- Installer
- LanguageCode
- SettingsViewModel
- What You Must Do When Invoked
- SettingsService
- LoginOrchestrator
- Quantity Survey Panel
- AuthService
- Sonny.ResourceManager.csproj
- .GetSolids
- .InvokeAsync
- LanguageCodeExtensionsTests
- IMessageService
- .GetAllElements
- FaceExtensions
- Sonny.Application.Domain.Entities.ColumnFromCad.Models
- ResourceRegistry
- Sonny.Application.csproj
- AppDisplayUnit
- LanguageOptionTests
- Host.cs
- SonnyApp.cs
- IViewScaleProvider
- XYZGeometryExtensions
- ElementIdExtensions
- PlanarFaceExtensions
- .IsBuiltInCategory
- Sonny.Application.Infrastructure.csproj
- Point3D
- ElementQuery
- Sonny.RevitExtensions.csproj
- .GetPlanarFaces
- Sonny.sln
- .GetAllTypeParameters
- Sonny.Application.Presentation.csproj
- IRevitDocument
- .GetCylindricalFaces
- ViewWrapperBase
- .DimensionByDirection
- KeygenConfigBuilder
- SonnyTab2.cs
- TypeSelectionFilter
- .TryAutoLoginAsync
- Sonny.Application.Tests.csproj
- DialogExtension.cs
- UIDocumentProvider
- KeygenModels.cs
- Sonny.RevitExtensions
- .RunAsync
- Sonny.Application.Domain.Entities
- Window
- LineExtensions
- CircularColumnModel
- ReferenceExtensions.cs
- CadLinkSelector
- .LoadResource
- Sonny Tab (Revit ribbon tab)
- ExampleModule
- XYZDistanceExtensions
- ElementWrapperBase
- Host
- LoggerConfiguration
- AppLanguageCode
- .Log
- ServiceRegistration
- graphify reference: extra exports and benchmark
- CultureChangedEventArgsTests
- .GetSolids
- Develop branch
- ParallelXyzEqualityComparer
- ResourceDictionaryManager
- IRevitTaskRunner
- UIStyleManager
- AutoColumnDimensionIntegrationTest
- .CreateUri
- .AddStackedItemsMixed
- .IsBuiltInCategory
- StringExtension.cs
- SonnyTab4.cs
- Revit R Gear Processor
- Sonny.Application.UIStyle.csproj
- TypeExtension
- UIControlledApplicationExtension
- ITransactionManager
- .GetCurves
- .DimensionByDirection
- .AutoColumnDimension_IntegrationTest_2F
- LinqExtensions
- Application
- Resources
- CylindricalFaceExtensions.cs
- install/Installer.csproj
- TransactionGroupManager
- ActivateMachineResult
- Sonny.RevitExtensions/install/Installer.csproj
- .GetFamilySymbols
- AppConstants.cs
- .nuke/parameters.json
- ColumnDimension Feature Icon
- Sonny.RevitExtensions.Extensions
- Border
- Sonny.RevitExtensions/.nuke/parameters.json
- XYZComparisonExtensions
- Bug Report issue template
- Security Policy
- resize_icons.py
- AutoColumnDimension.en.xaml
- AutoColumnDimension.vi.xaml
- ColumnFromCad.en.xaml
- ColumnFromCad.vi.xaml
- Common.en.xaml
- Common.vi.xaml
- Colors.xaml
- ComboBoxStyles.xaml
- GroupBoxStyles.xaml
- LabelStyles.xaml
- LayoutStyles.xaml
- TextBlockStyles.xaml
- TextBoxStyles.xaml
- Themes.xaml
- Strings.xaml
- graphify skill
- Feature Request issue template
- Question issue template
- App
- SonnyDocumentTestBase
- FamilyInstanceWrapperBase
- CultureManager
- .GetIntersectingElements
- .GetXyzes
- Sonny.Application.Infrastructure.Resource.Implements
- .CreateColumnData
- XYZUtilityExtensions
- ColumnCreationStrategy
- Sonny.EasyRibbon.Example
- FailurePreprocessorType
- Architecture decision records
- graphify reference: query, path, explain
- PanelAttribute
- .IsOnLayer
- Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao
- Q: Trace call flow cua AutoColumnDimensionCommand tu command den interactor
- Window
- graphify reference: add a URL and watch a folder
- graphify reference: commit hook and native CLAUDE.md integration
- graphify reference: incremental update and cluster-only
- Sonny.Application.Presentation/ServiceRegistration.cs
- graphify reference: GitHub clone and cross-repo merge
- graphify reference: transcribe video and audio
- extraction-spec.md
- XYZTransformExtensions
- Sonny.RevitExtensions.Extensions.XYZs
- PulldownButtonData1
- BaseExternalCommand

## God Nodes (most connected - your core abstractions)
1. `Sonny.Application.Domain.Services` - 58 edges
2. `CurveExtensions` - 33 edges
3. `Sonny.Application.Infrastructure.Revit.Services` - 33 edges
4. `KeygenConfigBuilder` - 26 edges
5. `Sonny.RevitExtensions.Extensions` - 26 edges
6. `Sonny.Application.Domain.Entities.ColumnFromCad.Models` - 25 edges
7. `ColumnFromCadViewModel` - 24 edges
8. `AppDisplayUnit` - 23 edges
9. `Sonny.Keygen.Services` - 23 edges
10. `Sonny.Keygen.Models` - 23 edges

## Surprising Connections (you probably didn't know these)
- `Dependency Injection (Microsoft.Extensions.DI)` --conceptually_related_to--> `Host composition root`  [INFERRED]
  Readme.md → CLAUDE.md
- `MVVM (CommunityToolkit.Mvvm)` --conceptually_related_to--> `Sonny.Application.Presentation`  [INFERRED]
  Readme.md → CLAUDE.md
- `ExtractColumnData phase` --semantically_similar_to--> `Empty case — ValidateColumns short-circuit`  [INFERRED] [semantically similar]
  docs/features/ColumnFromCad.md → docs/features/AutoColumnDimension.md
- `revit-architecture-fixer agent` --references--> `Code style conventions`  [EXTRACTED]
  .claude/agents/revit-architecture-fixer.md → CLAUDE.md
- `revit-architecture-fixer agent` --references--> `ITransactionManagerFactory`  [EXTRACTED]
  .claude/agents/revit-architecture-fixer.md → CLAUDE.md

## Import Cycles
- None detected.

## Hyperedges (group relationships)
- **EasyRibbon module lifecycle** — source_sonny_easyribbon_sonny_easyribbon_modules_readme_iapplicationmodule, source_sonny_easyribbon_sonny_easyribbon_modules_readme_moduleregistry, source_sonny_easyribbon_readme_createuiapp [EXTRACTED 0.75]
- **Command execution pipeline** — claude_ribbon_ui, claude_base_external_command, claude_host_composition_root, claude_iuidocumentprovider, claude_revit_task_runner [EXTRACTED 0.85]
- **Attribute-based ribbon UI system** — source_sonny_easyribbon_readme_tab_attribute, source_sonny_easyribbon_readme_panel_attribute, source_sonny_easyribbon_readme_button_attribute, source_sonny_easyribbon_readme_createuiapp [EXTRACTED 0.85]
- **Clean Architecture layered dependency flow** — claude_domain_layer, claude_usecases_layer, claude_infrastructure_layer, claude_presentation_layer, claude_sonny_application_entry [EXTRACTED 0.90]
- **Git submodule dependency libraries** — claude_revit_async, claude_easyribbon, claude_revitextensions, claude_keygen [EXTRACTED 0.90]
- **Ribbon-to-interactor command execution pipeline** — docs_architecture_command_flow_baseexternalcommand, docs_architecture_command_flow_license_check, docs_architecture_command_flow_iuidocumentprovider, docs_architecture_command_flow_irevittaskrunner, docs_architecture_command_flow_host_getservice_blind_spot, docs_architecture_command_flow_thin_command_rule [EXTRACTED 1.00]
- **UIDocument lifetime rule participants** — docs_architecture_command_flow_uidocument_lifetime, docs_architecture_command_flow_iuidocumentprovider, docs_architecture_command_flow_irevitdocument, docs_architecture_command_flow_singleton_consumer_hazard, docs_features_autocolumndimension_autocolumndimensioninteractor [EXTRACTED 1.00]
- **Documented silent-failure behaviours across features** — docs_readme_feature_document_shape, docs_features_autocolumndimension_column_selection_rule, docs_features_autocolumndimension_per_column_error_isolation, docs_features_autocolumndimension_result_reporting, docs_features_columnfromcad_silent_skip, docs_features_columnfromcad_unit_conversion_boundary [INFERRED 0.85]

## Communities (204 total, 41 thin omitted)

### Community 0 - ".FromCorners"
Cohesion: 0.32
Nodes (5): IReadOnlyList, double, List, Test, RectangularColumnModelFromCornersTests

### Community 1 - ".TryDetectCircle"
Cohesion: 0.17
Nodes (9): double, int, IReadOnlyList, Point3D, ColumnShapeDetector, List, Test, TestCase (+1 more)

### Community 2 - "AutoColumnDimensionInteractor (documented)"
Cohesion: 0.05
Nodes (53): Adding a new tool — five edits, BaseExternalCommand (documented), Command flow — ribbon button to interactor, Fixed command execution pipeline, Host.GetService<T>() DI hop invisible to AST, IRevitDocument (transient, expression-bodied passthrough), IRevitTaskRunner (documented), ITransactionManagerFactory (documented) (+45 more)

### Community 3 - "ColumnFromCadViewModel"
Cohesion: 0.06
Nodes (23): ColumnFromCadSettings, FamilyModel, LevelModel, Dictionary, HashSet, List, ColumnFromCadContext, bool (+15 more)

### Community 4 - "DimensionProcessor"
Cohesion: 0.05
Nodes (33): Sonny.RevitExtensions.Processors, Sonny.RevitExtensions.Extensions.Views, Sonny.RevitExtensions.Utilities, DimensionSegmentArray, Dimension, DimensionType, Line, View (+25 more)

### Community 5 - "Clean Architecture"
Cohesion: 0.06
Nodes (44): revit-architecture-fixer agent, revit-architecture-reviewer agent, Pull Request template, Compile workflow, Publish Release workflow, AutoColumnDimension feature, ColumnFromCad feature, License Management (Keygen + Auth0) (+36 more)

### Community 6 - "Sonny.Application.Domain.Services"
Cohesion: 0.12
Nodes (10): Sonny.Application.Infrastructure.Revit.Services, Sonny.Application.Infrastructure.Revit.Implements, Sonny.Application.Domain.Entities.Settings, Sonny.Application.Infrastructure.Revit.Managers.Transactions, Sonny.Application.Domain.Services, Sonny.Application.Tests.Features.AutoColumnDimension.IntegrationTests, Sonny.Application.Infrastructure.Features.ColumnFromCad.Strategies, Sonny.RevitExtensions.Extensions.Families (+2 more)

### Community 7 - "CurveExtensions"
Cohesion: 0.13
Nodes (9): ModelLine, Curve, CurveLoop, Document, IEnumerable, IList, Line, XYZ (+1 more)

### Community 8 - "Sonny.EasyRibbon.Example.Commands"
Cohesion: 0.15
Nodes (10): Sonny.EasyRibbon.Example.Commands, ExternalCommand, StartupCommand, SonnyButton1, SonnyButton12, SonnyButton2, SonnyButton3, SonnyPannel1 (+2 more)

### Community 9 - "ColumnModel"
Cohesion: 0.18
Nodes (7): Point3D, ColumnModel, IColumnCreationStrategy, IColumnCreationStrategyFactory, List, ColumnDataExtractor, ColumnCreationStrategyFactory

### Community 10 - "ColumnFromCadInteractorTests"
Cohesion: 0.27
Nodes (7): List, IColumnDataExtractor, List, Task, TearDown, Test, ColumnFromCadInteractorTests

### Community 11 - "LicenseFileService"
Cohesion: 0.07
Nodes (20): Certificate, JsonElement, LicenseData, LicenseDataset, LicenseMeta, DateTime, LicenseValidationResult, DateTime (+12 more)

### Community 12 - "LoginViewModel"
Cohesion: 0.14
Nodes (9): ObservableObject, RoutedEventArgs, UserInfoResult, IMessageBoxService, RelayCommand, string, Task, LoginViewModel (+1 more)

### Community 13 - ".CreateRectangular"
Cohesion: 0.17
Nodes (8): Arc, Curve, List, ColumnModelFactory, Arc, Curve, List, IColumnModelFactory

### Community 14 - "Panel: General"
Cohesion: 0.06
Nodes (32): Button: ALPHA BIM, Button: Create 3D Box, Button: Create Elements from Room, Button: Create Pipe from Line CAD Link, Button: Create Schedule, Button: Create Sewer, Button: Crop View, Button: Element from AutoCAD (+24 more)

### Community 15 - "ColumnCreationContext"
Cohesion: 0.21
Nodes (9): ColumnCreationContext, HashSet, List, Task, ColumnFromCadInteractor, HashSet, List, Task (+1 more)

### Community 16 - "ElementCurveExtensions"
Cohesion: 0.29
Nodes (9): PolyLine, Arc, Curve, Edge, Element, IEnumerable, Line, Options (+1 more)

### Community 17 - "Sonny.Application.Domain.Exceptions"
Cohesion: 0.40
Nodes (3): Sonny.Application.Domain.Exceptions, Exception, TransactionCommitFailedException

### Community 18 - "XYZVectorExtensions"
Cohesion: 0.20
Nodes (5): Sonny.RevitExtensions, XYZ, XYZVectorExtensions, double, ToleranceConstants

### Community 19 - "RectangularColumnModel"
Cohesion: 0.21
Nodes (8): Point3D, RectangularColumnModel, ImportInstance, List, RectangularColumnExtractor, ImportInstance, List, IRectangularColumnExtractor

### Community 20 - "Sonny.Keygen.Models"
Cohesion: 0.12
Nodes (11): Sonny.Keygen.Models, KeygenRegisterResult, KeygenUserResult, DateTime, LicenseInfo, List, UserLicensesResult, RestClient (+3 more)

### Community 21 - "ElementParameterExtensions"
Cohesion: 0.22
Nodes (10): BuiltInParameter, Definition, DefinitionBindingMap, Guid, Element, ElementId, IEnumerable, Parameter (+2 more)

### Community 22 - "IApplicationModule"
Cohesion: 0.17
Nodes (7): Application, UIControlledApplication, IApplicationModule, IReadOnlyList, List, UIControlledApplication, ModuleRegistry

### Community 23 - "SonnyRevitTestBase"
Cohesion: 0.27
Nodes (7): ControlledApplication, OneTimeSetUp, RevitApplication, UIApplication, UIControlledApplication, UIDocument, SonnyRevitTestBase

### Community 24 - "Sonny.EasyRibbon.Extensions"
Cohesion: 0.18
Nodes (10): Sonny.EasyRibbon.UIAttributeBase, Sonny.EasyRibbon.UIAttributeBase.Base, Sonny.EasyRibbon.Extensions, IRibbonItem, Type, ButtonAttribute, PulldownButtonDataAttribute, StackedButtonAttribute (+2 more)

### Community 25 - "Sonny.Keygen"
Cohesion: 0.10
Nodes (22): Auth0.OidcClient.WPF (4.4.0), Hardware.Info (10.0.0), NSec.Cryptography (22.4.0), Portable.BouncyCastle (1.9.0), RestSharp (108.0.1), System.Management (10.0.1), net48, net8.0-windows (+14 more)

### Community 26 - ".CreatePushButtonData"
Cohesion: 0.17
Nodes (7): Attribute, BitmapImage, ImageExtension, ResourceExtension, UIAttributeBase, PushButtonData, PulldownButtonData

### Community 27 - ".CollectStackedItems"
Cohesion: 0.20
Nodes (13): buttons, pulldownConfigs, data, IList, List, PulldownButtonData, PushButtonData, RibbonItem (+5 more)

### Community 28 - "Sonny.EasyRibbon"
Cohesion: 0.07
Nodes (31): Sonny.EasyRibbon.Example, net48, net8.0-windows, ILRepack (2.0.41), Microsoft.Extensions.DependencyInjection (9.0.3), Nice3point.Revit.Api.RevitAPI ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPIUI ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1) (+23 more)

### Community 29 - "Window"
Cohesion: 0.10
Nodes (20): AllCircularColumnTypeParameters, AllColumnFamilies, AllLayerNames, AllLevels, AllRectangularColumnTypeParameters, BaseLevel, BaseOffsetDisplay, DiameterParameter (+12 more)

### Community 30 - "IElementSelector"
Cohesion: 0.29
Nodes (4): ICollection, IElementSelector, ICollection, ElementSelector

### Community 31 - "UnitConverterTests"
Cohesion: 0.21
Nodes (5): ForgeTypeId, UnitConverter, SetUp, Test, UnitConverterTests

### Community 32 - "Sonny.ResourceManager"
Cohesion: 0.17
Nodes (6): Sonny.ResourceManager, Sonny.Application.Tests.ResourceManager.UnitTests, EventArgs, CultureInfo, CultureChangedEventArgs, ResourceHelper

### Community 33 - "BaseViewModel"
Cohesion: 0.15
Nodes (5): ILogger, Window, BaseViewModel, string, MessageService

### Community 34 - "Sonny.EasyRibbon"
Cohesion: 0.11
Nodes (20): EasyRibbon Changelog, EasyRibbon Contributing Guide, Button Attribute, CreateUIApp, Sonny.EasyRibbon, Nice3point.Revit.Toolkit, Panel Attribute, PulldownButtonData Attribute (+12 more)

### Community 35 - "LicenseStatus"
Cohesion: 0.19
Nodes (8): Sonny.Application.Domain.Entities.License, DateTime, LicenseStatus, Task, ILicenseValidator, DateTime, Task, KeygenLicenseValidator

### Community 36 - "Sonny.Keygen.Services"
Cohesion: 0.07
Nodes (14): Sonny.Keygen.Services, Sonny.Application.Infrastructure.License, Sonny.Keygen.Test.Net48, Sonny.Keygen.UI.Views, Sonny.Keygen.UI.ViewModels, Sonny.Keygen.Test, KeygenConfig, string (+6 more)

### Community 37 - "LicenseService"
Cohesion: 0.13
Nodes (13): ApiError, CheckoutAttributes, CheckoutData, CreateLicenseResult, List, RestClient, Task, ApiError (+5 more)

### Community 38 - "Window"
Cohesion: 0.25
Nodes (7): Email, LicenseExpiryDate, LicenseStartDate, LicenseType, LoginCommand, LogoutCommand, Window

### Community 39 - "AutoColumnDimensionViewModel"
Cohesion: 0.17
Nodes (9): DimensionTypeModel, List, IDimensionTypeProvider, DimensionType, List, DimensionTypeProvider, double, ObservableCollection (+1 more)

### Community 40 - ".GetIntersectingElements"
Cohesion: 0.18
Nodes (10): Document, Element, ElementId, ICollection, IEnumerable, List, Solid, View (+2 more)

### Community 41 - "Installer"
Cohesion: 0.13
Nodes (7): Installer, IEnumerable, WixEntity, Generator, IEnumerable, WixEntity, Generator

### Community 42 - "LanguageCode"
Cohesion: 0.22
Nodes (5): bool, SonnyResourcesInitializer, LanguageCode, CultureInfo, LanguageCodeExtensions

### Community 43 - "SettingsViewModel"
Cohesion: 0.12
Nodes (9): AppDisplayUnit, UnitOption, RelayCommand, Task, Exception, ObservableCollection, RelayCommand, SettingsViewModel (+1 more)

### Community 44 - "What You Must Do When Invoked"
Cohesion: 0.07
Nodes (26): For /graphify add and --watch, For /graphify query, For the commit hook and native CLAUDE.md integration, For --update and --cluster-only, /graphify, Honesty Rules, Interpreter guard for subcommands, Part A - Structural extraction for code files (+18 more)

### Community 45 - "SettingsService"
Cohesion: 0.15
Nodes (10): SettingsData, Func, string, SettingsData, SettingsService, SetUp, string, TearDown (+2 more)

### Community 46 - "LoginOrchestrator"
Cohesion: 0.42
Nodes (3): LoginOrchestrationResult, Task, LoginOrchestrator

### Community 47 - "Quantity Survey Panel"
Cohesion: 0.13
Nodes (16): Auto DIM Button, AutoCAD to Revit Button, Beam Rebar Button, Column/Wall Rebar Button, Concrete Column/Wall Button, Export Quantity Button, Formwork Area Button, Load Family Button (+8 more)

### Community 48 - "AuthService"
Cohesion: 0.20
Nodes (7): Auth0Client, ClaimsPrincipal, LoginResult, LogoutResult, Task, AuthService, TokenData

### Community 49 - "Sonny.ResourceManager.csproj"
Cohesion: 0.33
Nodes (5): WPFLocalizeExtension (3.9.0), net48, net8.0-windows, Microsoft.Extensions.DependencyInjection (9.0.3), Microsoft.NET.Sdk

### Community 50 - ".GetSolids"
Cohesion: 0.14
Nodes (10): Sonny.RevitExtensions.Extensions.GeometryObjects.GeometryElements, GeometryElement, Element, IEnumerable, Options, Solid, ElementSolidExtensions, Element (+2 more)

### Community 51 - ".InvokeAsync"
Cohesion: 0.31
Nodes (7): BrowserOptions, BrowserResult, CancellationToken, IBrowser, string, Task, CustomWebViewBrowser

### Community 53 - "IMessageService"
Cohesion: 0.15
Nodes (7): IMessageService, IResourceHelper, ResourceHelper, ILogger, CommonServices, ILogger, ICommonServices

### Community 54 - ".GetAllElements"
Cohesion: 0.24
Nodes (9): BuiltInCategory, Document, Element, ElementId, IEnumerable, Reference, Type, View (+1 more)

### Community 55 - "FaceExtensions"
Cohesion: 0.07
Nodes (26): Sonny.RevitExtensions.Extensions.GeometryObjects.Faces, Arc, Curve, Edge, Face, IEnumerable, Line, XYZ (+18 more)

### Community 56 - "Sonny.Application.Domain.Entities.ColumnFromCad.Models"
Cohesion: 0.12
Nodes (13): Sonny.Application.UnitTests.UseCases.ColumnFromCad, Sonny.Application.Domain.Entities.ColumnFromCad.Services, Sonny.Application.UnitTests.Domain.ColumnFromCad, Sonny.Application.UseCases.ColumnFromCad.Implements, Sonny.Application.Domain.Entities.ColumnFromCad.Models, Sonny.Application.Infrastructure.Features.ColumnFromCad.Services, Sonny.Application.Domain.Entities.ColumnFromCad.Contexts, Sonny.Application.Domain.Entities.ColumnFromCad (+5 more)

### Community 57 - "ResourceRegistry"
Cohesion: 0.20
Nodes (6): GetValueOrDefault, ResourceConfig, Dictionary, IEnumerable, ResourceRegistry, TryGetValue

### Community 58 - "Sonny.Application.csproj"
Cohesion: 0.15
Nodes (12): Serilog.Sinks.Console (6.0.0), Serilog.Sinks.Debug (3.0.0), Serilog.Sinks.File (6.0.0), net48, net8.0-windows, ILRepack (2.0.41), Microsoft.Extensions.DependencyInjection (9.0.3), Nice3point.Revit.Api.AdWindows ($(RevitVersion).*) (+4 more)

### Community 59 - "AppDisplayUnit"
Cohesion: 0.25
Nodes (4): AppDisplayUnit, IDisplayUnitProvider, IUnitConverter, DisplayUnitProvider

### Community 60 - "LanguageOptionTests"
Cohesion: 0.16
Nodes (6): Sonny.Application.Tests.Utils, Sonny.Application.UseCases.Settings.Models, Test, LanguageOptionTests, EnumHelper, LanguageOption

### Community 61 - "Host.cs"
Cohesion: 0.14
Nodes (8): Sonny.Application.Infrastructure, Sonny.Application.UseCases, Sonny.Application.UseCases.Services, Sonny.Application.Presentation, IServiceCollection, ServiceRegistration, ILicenseCheckService, LicenseCheckService

### Community 62 - "SonnyApp.cs"
Cohesion: 0.19
Nodes (7): Sonny.Application, Sonny.EasyRibbon, Sonny.Application.Modules, ExternalApplication, UIControlledApplication, SonnyModule, SonnyApp

### Community 64 - "XYZGeometryExtensions"
Cohesion: 0.26
Nodes (3): IEnumerable, XYZ, XYZGeometryExtensions

### Community 66 - "PlanarFaceExtensions"
Cohesion: 0.32
Nodes (5): Sonny.RevitExtensions.Extensions.GeometryObjects.Faces.PlanarFaces, IEnumerable, PlanarFace, XYZ, PlanarFaceExtensions

### Community 67 - ".IsBuiltInCategory"
Cohesion: 0.40
Nodes (3): BuiltInCategory, Element, ElementExtensions

### Community 68 - "Sonny.Application.Infrastructure.csproj"
Cohesion: 0.17
Nodes (11): net48, net8.0-windows, Microsoft.Extensions.DependencyInjection (9.0.3), Newtonsoft.Json (13.0.3), Nice3point.Revit.Api.RevitAPI ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPIUI ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1), Nice3point.Revit.Extensions ($(RevitVersion).*) (+3 more)

### Community 69 - "Point3D"
Cohesion: 0.06
Nodes (32): Point3D, DimensionType, List, PlanarFace, XYZ, DimensionPlanExecutor, XYZ, Point3DConverter (+24 more)

### Community 70 - "ElementQuery"
Cohesion: 0.20
Nodes (8): FilteredElementCollector, IEnumerable, IEnumerator, Document, ElementId, Type, View, ElementQuery

### Community 71 - "Sonny.RevitExtensions.csproj"
Cohesion: 0.17
Nodes (10): MoreLinq (3.4.2), net48, net8.0-windows, Nice3point.Revit.Api.AdWindows ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPI ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPIUI ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1), Nice3point.Revit.Extensions ($(RevitVersion).*) (+2 more)

### Community 72 - ".GetPlanarFaces"
Cohesion: 0.25
Nodes (7): CylindricalFace, Face, IEnumerable, PlanarFace, Solid, XYZ, SolidFaceExtensions

### Community 73 - "Sonny.sln"
Cohesion: 0.11
Nodes (16): coverlet.collector (6.0.2), NUnit3TestAdapter (4.6.0), NUnit.Analyzers (4.4.0), net48, net8.0-windows, Microsoft.NET.Sdk, net8.0-windows, Microsoft.NET.Test.Sdk (17.12.0) (+8 more)

### Community 74 - ".GetAllTypeParameters"
Cohesion: 0.40
Nodes (3): Element, IEnumerable, ElementParameterExtensions

### Community 75 - "Sonny.Application.Presentation.csproj"
Cohesion: 0.18
Nodes (10): net48, net8.0-windows, CommunityToolkit.Mvvm (8.4.0), Microsoft.Extensions.DependencyInjection (9.0.3), Newtonsoft.Json (13.0.3), Nice3point.Revit.Api.AdWindows ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1), Nice3point.Revit.Toolkit ($(RevitVersion).*) (+2 more)

### Community 76 - "IRevitDocument"
Cohesion: 0.18
Nodes (10): Document, UIApplication, UIDocument, View, RevitDocument, Document, UIApplication, UIDocument (+2 more)

### Community 77 - ".GetCylindricalFaces"
Cohesion: 0.29
Nodes (8): CylindricalFace, Element, Face, IEnumerable, Options, PlanarFace, Solid, ElementFaceExtensions

### Community 78 - "ViewWrapperBase"
Cohesion: 0.27
Nodes (7): BoundingBoxXYZ, XYZ, bool, Element, List, View, ViewWrapperBase

### Community 79 - ".DimensionByDirection"
Cohesion: 0.22
Nodes (7): Grid, DimensionType, List, PlanarFace, XYZ, Line, GridWrapperBase

### Community 81 - "SonnyTab2.cs"
Cohesion: 0.25
Nodes (7): SonnyButton1, SonnyButton2, SonnyButton3, SonnyButton4, SonnyPannel2, SonnySpitButton2, SonnyTab2

### Community 82 - "TypeSelectionFilter"
Cohesion: 0.20
Nodes (7): Sonny.Application.Infrastructure.Revit.SelectionFilters, ISelectionFilter, Element, List, Reference, XYZ, TypeSelectionFilter

### Community 83 - ".TryAutoLoginAsync"
Cohesion: 0.17
Nodes (8): HardwareInfo, AutoLoginResult, Task, AutoLoginService, object, string, MachineFingerprintService, SerialNumber

### Community 84 - "Sonny.Application.Tests.csproj"
Cohesion: 0.20
Nodes (9): Revit_All_Main_Versions_API_x64 ($(RevitVersion).*), ricaun.Revit.UI.Tasks (*), ricaun.RevitTest.TestAdapter (*), net48, net8.0-windows, Microsoft.NET.Test.Sdk (*), NSubstitute (5.3.0), NUnit (3.13.3) (+1 more)

### Community 86 - "UIDocumentProvider"
Cohesion: 0.27
Nodes (5): object, UIDocument, UIDocumentProvider, UIDocument, IUIDocumentProvider

### Community 87 - "KeygenModels.cs"
Cohesion: 0.40
Nodes (9): List, Attributes, Data, Document, DocumentArray, Error, PolicyData, PolicyRelationship (+1 more)

### Community 88 - "Sonny.RevitExtensions"
Cohesion: 0.22
Nodes (10): Sonny.RevitExtensions Compile workflow, Nuke build (RevitExtensions), Sonny.RevitExtensions PublishRelease workflow, Sonny.RevitExtensions Changelog, DimensionProcessor, DocumentExtension, ElementQuery, Sonny.RevitExtensions (+2 more)

### Community 89 - ".RunAsync"
Cohesion: 0.50
Nodes (3): Action, Func, ImmediateRevitTaskRunner

### Community 90 - "Sonny.Application.Domain.Entities"
Cohesion: 0.23
Nodes (5): Sonny.Application.UnitTests.UseCases.AutoColumnDimension, Sonny.Application.UseCases.AutoColumnDimension.Models, Sonny.Application.Domain.Entities, Sonny.Application.UseCases.AutoColumnDimension.Services, Sonny.Application.UseCases.AutoColumnDimension.Implements

### Community 91 - "Window"
Cohesion: 0.22
Nodes (8): DimensionTypes, DisplayUnitName, RunCommand, SelectedDimensionType, SnapDistanceDisplay, ThisWindow, Window, Window

### Community 92 - "LineExtensions"
Cohesion: 0.39
Nodes (4): Sonny.RevitExtensions.Extensions.GeometryObjects.Curves.Lines, Line, XYZ, LineExtensions

### Community 93 - "CircularColumnModel"
Cohesion: 0.27
Nodes (7): CircularColumnModel, ImportInstance, List, CircularColumnExtractor, ImportInstance, List, ICircularColumnExtractor

### Community 95 - "CadLinkSelector"
Cohesion: 0.22
Nodes (6): ImportInstance, UIDocument, CadLinkSelector, ImportInstance, UIDocument, ICadLinkSelector

### Community 96 - ".LoadResource"
Cohesion: 0.28
Nodes (3): Dictionary, ResourceLoader, ResourcePathBuilder

### Community 97 - "Sonny Tab (Revit ribbon tab)"
Cohesion: 0.33
Nodes (9): Declarative Tab/Panel/Button attribute layout, Sonny Panel 1, Sonny Panel 2, Sonny Panel 4, PulldownButton (Sonny Pulldown Button Data 1 / 4), PushButton element (Sonny Button 1-1..2-4, 4-1, 4-2), Sonny.EasyRibbon Ribbon Demo Screenshot, Sonny Tab (Revit ribbon tab) (+1 more)

### Community 98 - "ExampleModule"
Cohesion: 0.33
Nodes (3): Application, UIControlledApplication, ExampleModule

### Community 99 - "XYZDistanceExtensions"
Cohesion: 0.36
Nodes (4): IEnumerable, Line, XYZ, XYZDistanceExtensions

### Community 100 - "ElementWrapperBase"
Cohesion: 0.22
Nodes (7): Dimension, DimensionWrapperBase, Dictionary, Element, ElementWrapperBase, WallWrapperBase, Wall

### Community 101 - "Host"
Cohesion: 0.32
Nodes (4): IServiceProvider, object, Host, UnhandledExceptionEventArgs

### Community 102 - "LoggerConfiguration"
Cohesion: 0.29
Nodes (5): Sonny.Application.Config.Logging, Logger, IServiceCollection, string, LoggerConfiguration

### Community 103 - "AppLanguageCode"
Cohesion: 0.16
Nodes (5): AppLanguageCode, Func, ISettingsService, LanguageChangeHandler, LanguageCodeConverter

### Community 106 - "graphify reference: extra exports and benchmark"
Cohesion: 0.22
Nodes (8): graphify reference: extra exports and benchmark, Step 6b - Wiki (only if --wiki flag), Step 7 - Neo4j export (only if --neo4j or --neo4j-push flag), Step 7a - FalkorDB export (only if --falkordb or --falkordb-push flag), Step 7b - SVG export (only if --svg flag), Step 7c - GraphML export (only if --graphml flag), Step 7d - MCP server (only if --mcp flag), Step 8 - Token reduction benchmark (only if total_words > 5000)

### Community 108 - ".GetSolids"
Cohesion: 0.33
Nodes (5): HashSet, IEnumerable, ImportInstance, Solid, ImportInstanceExtensions

### Community 109 - "Develop branch"
Cohesion: 0.76
Nodes (7): Develop branch, Git Flow Branching Model Diagram, Feature branch, Hotfix branch, Master branch, Release branch, Version tags (v0.1, v0.2, v1.0)

### Community 110 - "ParallelXyzEqualityComparer"
Cohesion: 0.38
Nodes (4): Sonny.RevitExtensions.EqualityComparers, IEqualityComparer, XYZ, ParallelXyzEqualityComparer

### Community 112 - "IRevitTaskRunner"
Cohesion: 0.17
Nodes (8): Action, Func, Task, IRevitTaskRunner, Action, Func, Task, RevitTaskRunner

### Community 113 - "UIStyleManager"
Cohesion: 0.43
Nodes (3): Sonny.Application.UIStyle, Application, UIStyleManager

### Community 114 - "AutoColumnDimensionIntegrationTest"
Cohesion: 0.29
Nodes (5): SonnyDocumentTestBase, string, AutoColumnDimensionIntegrationTest, string, ColumnFromCadIntegrationTest

### Community 116 - ".AddStackedItemsMixed"
Cohesion: 0.29
Nodes (5): IList, List, RibbonItem, RibbonPanel, RibbonPanelExtension

### Community 117 - ".IsBuiltInCategory"
Cohesion: 0.33
Nodes (4): Category, BuiltInCategory, Document, CategoryExtensions

### Community 119 - "SonnyTab4.cs"
Cohesion: 0.20
Nodes (9): SonnyButton4_1, SonnyButton4_2, SonnyButton4_3, SonnyButton4_4, SonnyButton4_5, SonnyPannel4, SonnyPulldownButton4, SonnyStackedButton4 (+1 more)

### Community 120 - "Revit R Gear Processor"
Cohesion: 0.40
Nodes (6): Transformation Arrow, CAD Drawing Blueprint (input), ColumnFromCad Feature (CAD to Revit columns), ColumnFromCadCommand Icon Illustration, Revit R Gear Processor, Structural Column (output)

### Community 121 - "Sonny.Application.UIStyle.csproj"
Cohesion: 0.33
Nodes (5): net48, net8.0-windows, Nice3point.Revit.Build.Tasks (3.0.1), Nice3point.Revit.Toolkit ($(RevitVersion).*), Microsoft.NET.Sdk

### Community 122 - "TypeExtension"
Cohesion: 0.47
Nodes (3): List, Type, TypeExtension

### Community 123 - "UIControlledApplicationExtension"
Cohesion: 0.40
Nodes (3): RibbonPanel, UIControlledApplication, UIControlledApplicationExtension

### Community 124 - "ITransactionManager"
Cohesion: 0.11
Nodes (9): IDisposable, ITransactionGroupManager, ITransactionManager, IEnumerable, ITransactionManagerFactory, IEnumerable, TransactionManagerFactory, SetUp (+1 more)

### Community 125 - ".GetCurves"
Cohesion: 0.33
Nodes (4): Curve, CurveLoop, IEnumerable, CurveLoopExtensions

### Community 126 - ".DimensionByDirection"
Cohesion: 0.29
Nodes (6): DimensionCreator, DimensionType, List, PlanarFace, XYZ, IDimensionCreator

### Community 127 - ".AutoColumnDimension_IntegrationTest_2F"
Cohesion: 0.33
Nodes (3): Test, Document, View

### Community 128 - "LinqExtensions"
Cohesion: 0.40
Nodes (3): Func, IEnumerable, LinqExtensions

### Community 129 - "Application"
Cohesion: 0.90
Nodes (5): Application, Clean Architecture Layer Diagram, Domain, Infrastructure, Presentation

### Community 130 - "Resources"
Cohesion: 0.40
Nodes (4): Sonny.Keygen.Test.Net48.Properties, ResourceManager, CultureInfo, Resources

### Community 131 - "CylindricalFaceExtensions.cs"
Cohesion: 0.40
Nodes (3): Sonny.RevitExtensions.Extensions.GeometryObjects.Faces.CylindricalFaces, CylindricalFace, CylindricalFaceExtensions

### Community 132 - "install/Installer.csproj"
Cohesion: 0.40
Nodes (4): net48, WixSharp.bin (1.26.0), WixSharp.wix.bin (3.14.1), Microsoft.NET.Sdk

### Community 133 - "TransactionGroupManager"
Cohesion: 0.10
Nodes (10): DomainTransactionStatus, bool, TransactionGroupManager, bool, IFailuresPreprocessor, TransactionManager, TransactionStatusConverter, Transaction (+2 more)

### Community 134 - "ActivateMachineResult"
Cohesion: 0.29
Nodes (4): ActivateMachineResult, RestClient, Task, MachineService

### Community 135 - "Sonny.RevitExtensions/install/Installer.csproj"
Cohesion: 0.40
Nodes (4): net48, WixSharp.bin (1.26.0), WixSharp.wix.bin (3.14.1), Microsoft.NET.Sdk

### Community 136 - ".GetFamilySymbols"
Cohesion: 0.33
Nodes (4): Family, FamilySymbol, IEnumerable, FamilyExtensions

### Community 137 - "AppConstants.cs"
Cohesion: 0.50
Nodes (3): Sonny.Keygen, string, AppConstants

### Community 138 - ".nuke/parameters.json"
Cohesion: 0.50
Nodes (3): $schema, Solution, Verbosity

### Community 139 - "ColumnDimension Feature Icon"
Cohesion: 0.67
Nodes (4): Column Glyph (top/bottom caps + shaft), AutoColumnDimension Revit Feature, ColumnDimension Feature Icon, Vertical Double-Headed Dimension Arrow

### Community 140 - "Sonny.RevitExtensions.Extensions"
Cohesion: 0.11
Nodes (14): Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements, Sonny.Application.Infrastructure.Features.AutoColumnDimension.Services, Sonny.RevitExtensions.Extensions, Sonny.Application.Domain, Sonny.RevitExtensions.RevitWrapper, Sonny.RevitExtensions.Extensions.Elements, Sonny.RevitExtensions.Extensions.GeometryObjects.Solids, double (+6 more)

### Community 141 - "Border"
Cohesion: 0.50
Nodes (3): Border, ResourceDictionary, Border

### Community 142 - "Sonny.RevitExtensions/.nuke/parameters.json"
Cohesion: 0.50
Nodes (3): $schema, Solution, Verbosity

### Community 143 - "XYZComparisonExtensions"
Cohesion: 0.48
Nodes (3): IEnumerable, XYZ, XYZComparisonExtensions

### Community 171 - "App"
Cohesion: 0.33
Nodes (4): Application, Application, App, StartupEventArgs

### Community 172 - "SonnyDocumentTestBase"
Cohesion: 0.33
Nodes (4): Sonny.Application.Tests, Document, UIDocument, SonnyDocumentTestBase

### Community 173 - "FamilyInstanceWrapperBase"
Cohesion: 0.33
Nodes (6): FamilyInstance, double, List, XYZ, FamilyInstanceWrapperBase, Transform

### Community 175 - ".GetIntersectingElements"
Cohesion: 0.25
Nodes (7): Document, Element, ElementId, ICollection, List, View, ElementIntersectExtensions

### Community 176 - ".GetXyzes"
Cohesion: 0.33
Nodes (4): IEnumerable, Solid, XYZ, SolidPointExtensions

### Community 181 - "ColumnCreationStrategy"
Cohesion: 0.09
Nodes (16): double, ColumnSymbolSizingPolicy, Family, FamilySymbol, CircularColumnCreationStrategy, Element, FamilySymbol, Parameter (+8 more)

### Community 182 - "Sonny.EasyRibbon.Example"
Cohesion: 0.33
Nodes (3): Sonny.EasyRibbon.Example, Sonny.EasyRibbon.MasterExample, Sonny.EasyRibbon.Modules

### Community 183 - "FailurePreprocessorType"
Cohesion: 0.08
Nodes (18): Sonny.Application.Infrastructure.Revit.FailuresPreprocessors, IFailuresPreprocessor, FailurePreprocessorType, FailureProcessingResult, FailuresAccessor, SuppressWarningsPreprocessor, FailureProcessingResult, FailuresAccessor (+10 more)

### Community 184 - "Architecture decision records"
Cohesion: 0.17
Nodes (9): CONTEXT — ubiquitous language, Business decisions live in UseCases/Domain; Infrastructure keeps only Revit mechanism behind interfaces, Consequences, Phương án đã loại, Architecture decision records, Format, What does not belong, What qualifies here (+1 more)

### Community 186 - "graphify reference: query, path, explain"
Cohesion: 0.33
Nodes (5): For /graphify explain, For /graphify path, graphify reference: query, path, explain, Step 0 — Constrained query expansion (REQUIRED before traversal), Step 1 — Traversal

### Community 187 - "PanelAttribute"
Cohesion: 0.33
Nodes (4): RibbonPanel, string, UIControlledApplication, PanelAttribute

### Community 189 - ".IsOnLayer"
Cohesion: 0.40
Nodes (3): GeometryObject, Document, GeometryObjectExtensions

### Community 190 - "Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao"
Cohesion: 0.40
Nodes (4): Answer, Outcome, Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao, Source Nodes

### Community 191 - "Q: Trace call flow cua AutoColumnDimensionCommand tu command den interactor"
Cohesion: 0.40
Nodes (4): Answer, Outcome, Q: Trace call flow cua AutoColumnDimensionCommand tu command den interactor, Source Nodes

### Community 192 - "Window"
Cohesion: 0.06
Nodes (20): LanguageOptions, SaveCommand, SelectedLanguageOption, SelectedUnitOption, UnitOptions, IProgressReporter, ProgressReporter, ThisWindow (+12 more)

### Community 198 - "graphify reference: add a URL and watch a folder"
Cohesion: 0.50
Nodes (3): For /graphify add, For --watch, graphify reference: add a URL and watch a folder

### Community 199 - "graphify reference: commit hook and native CLAUDE.md integration"
Cohesion: 0.50
Nodes (3): For git commit hook, For native CLAUDE.md integration, graphify reference: commit hook and native CLAUDE.md integration

### Community 200 - "graphify reference: incremental update and cluster-only"
Cohesion: 0.50
Nodes (3): For --cluster-only, For --update (incremental re-extraction), graphify reference: incremental update and cluster-only

### Community 202 - "Sonny.Application.Presentation/ServiceRegistration.cs"
Cohesion: 0.10
Nodes (15): Sonny.Application.Presentation.AutoColumnDimension.Views, Sonny.Application.Presentation.Extensions, Sonny.Application.Presentation.AutoColumnDimension.ViewModels, Sonny.Application.Presentation.Views, Sonny.Application.Domain.Entities.Settings.Models, Sonny.Application.Presentation.Implements, Sonny.Application.Presentation.ColumnFromCad.ViewModels, Sonny.Application.Presentation.ColumnFromCad.Views (+7 more)

### Community 214 - "Sonny.RevitExtensions.Extensions.XYZs"
Cohesion: 0.17
Nodes (6): Sonny.RevitExtensions.Extensions.GeometryObjects.Curves, Sonny.RevitExtensions.Extensions.XYZs, Curve, IEnumerable, XYZ, CurvePointExtensions

### Community 217 - "PulldownButtonData1"
Cohesion: 0.20
Nodes (9): PulldownButtonData1, SonnyButton1, SonnyButton2, SonnyButton3, SonnyButton4, SonnyButton5, SonnyButton6, SonnyPannel1 (+1 more)

### Community 218 - "BaseExternalCommand"
Cohesion: 0.05
Nodes (33): Sonny.Application.Commands, Sonny.Application.Ribbon, Sonny.Application.Presentation.Settings.Views, Sonny.Application.Bases, IExternalCommand, ElementSet, ExternalCommandData, Result (+25 more)

## Ambiguous Edges - Review These
- `Sonny.Application.Presentation` → `Sonny.Application.Infrastructure`  [AMBIGUOUS]
  CLAUDE.md · relation: references

## Knowledge Gaps
- **373 isolated node(s):** `Phương án đã loại`, `Consequences`, `When to write one`, `What qualifies here`, `What does not belong` (+368 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **41 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What is the exact relationship between `Sonny.Application.Presentation` and `Sonny.Application.Infrastructure`?**
  _Edge tagged AMBIGUOUS (relation: references) - confidence is low._
- **What connects `Phương án đã loại`, `Consequences`, `When to write one` to the rest of the system?**
  _373 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `AutoColumnDimensionInteractor (documented)` be split into smaller, more focused modules?**
  _Cohesion score 0.05297532656023222 - nodes in this community are weakly interconnected._
- **Should `ColumnFromCadViewModel` be split into smaller, more focused modules?**
  _Cohesion score 0.058823529411764705 - nodes in this community are weakly interconnected._
- **Should `DimensionProcessor` be split into smaller, more focused modules?**
  _Cohesion score 0.05019607843137255 - nodes in this community are weakly interconnected._
- **Should `Clean Architecture` be split into smaller, more focused modules?**
  _Cohesion score 0.06025369978858351 - nodes in this community are weakly interconnected._
- **Should `Sonny.Application.Domain.Services` be split into smaller, more focused modules?**
  _Cohesion score 0.12162162162162163 - nodes in this community are weakly interconnected._