# Graph Report - Sonny  (2026-08-20)

## Corpus Check
- 365 files · ~223,077 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 2357 nodes · 3586 edges · 208 communities (173 shown, 35 thin omitted)
- Extraction: 96% EXTRACTED · 4% INFERRED · 0% AMBIGUOUS · INFERRED: 146 edges (avg confidence: 0.8)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `ebf7f100`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- Sonny.Application.Domain.Services
- Sonny.RevitExtensions.Extensions
- AutoColumnDimensionInteractor (documented)
- ColumnFromCadViewModel
- DimensionProcessor
- Clean Architecture
- Sonny.Application.Domain
- CurveExtensions
- ProgressReporter
- CircularColumnModel
- TransactionGroupManager
- LicenseFileService
- LoginViewModel
- Sonny.Application.Infrastructure/ServiceRegistration.cs
- Panel: General
- ColumnCreationContext
- ViewWrapperBase
- Point3D
- XYZVectorExtensions
- .Create
- KeygenAuthService
- ElementParameterExtensions
- IApplicationModule
- SonnyRevitTestBase
- Sonny.EasyRibbon.Extensions
- Sonny.Keygen
- .CreatePushButtonData
- .CollectStackedItems
- Sonny.EasyRibbon.csproj
- Window
- Sonny.EasyRibbon.Example
- UnitConverterTests
- Sonny.Keygen.UI.Views
- BaseViewModel
- Sonny.EasyRibbon
- LicenseStatus
- Sonny.Keygen.Services
- LicenseService
- Window
- AutoColumnDimensionViewModel
- ICompositeFailurePreprocessor
- Installer
- LanguageCode
- SettingsViewModel
- What You Must Do When Invoked
- SettingsService
- LoginOrchestrator
- Quantity Survey Panel
- AuthService
- Sonny.ResourceManager.csproj
- ElementCurveExtensions
- Sonny.Application.Infrastructure.Revit.Services
- Window
- IMessageService
- .GetAllElements
- FaceCurveExtensions
- Sonny.Application.Domain.Entities.ColumnFromCad.Models
- AppLanguageCode
- Sonny.Application.csproj
- CircularColumnExtractor
- LanguageCodeExtensionsTests
- ColumnModel
- FaceExtensions
- .GetIntersectingElements
- XYZGeometryExtensions
- IRectangularColumnExtractor
- .CreateDimensions
- .DimensionByDirection
- Sonny.Application.Infrastructure.csproj
- Sonny.Application.Presentation/ServiceRegistration.cs
- ElementQuery
- Sonny.RevitExtensions
- .GetPlanarFaces
- Sonny.sln
- IRevitDocument
- Sonny.Application.Presentation.csproj
- .TryAutoLoginAsync
- .GetCylindricalFaces
- SolidCurveExtensions
- Window
- KeygenConfigBuilder
- SonnyPannel2
- TypeSelectionFilter
- MachineFingerprintService
- Sonny.Application.Tests.csproj
- AppDisplayUnit
- UIDocumentProvider
- KeygenModels.cs
- Sonny.RevitExtensions
- PlanarFaceExtensions
- BaseExternalCommand.cs
- Window
- LineExtensions
- Sonny.Application.UnitTests.csproj
- AutoColumnDimensionInteractor
- CadLinkSelector
- SonnyModule.cs
- Sonny Tab (Revit ribbon tab)
- ExampleModule
- XYZDistanceExtensions
- ElementWrapperBase
- .InvokeAsync
- LoggerConfiguration
- Sonny.Application.Commands
- UIStyleManager
- ServiceRegistration
- graphify reference: extra exports and benchmark
- SonnyPulldownButton4
- .GetSolids
- Develop branch
- ParallelXyzEqualityComparer
- IElementSelector
- .RunAsync
- BaseExternalCommand
- SonnyPannel1
- IColumnCreationStrategy
- .AddStackedItemsMixed
- .IsBuiltInCategory
- FamilyInstanceWrapperBase
- PulldownButtonData1
- Revit R Gear Processor
- Sonny.Application.UIStyle.csproj
- TypeExtension
- UIControlledApplicationExtension
- ITransactionGroupManager
- .GetCurves
- .GetFamilySymbols
- Sonny.RevitExtensions.Extensions.GeometryObjects.Faces
- LinqExtensions
- Application
- Resources
- CylindricalFaceExtensions.cs
- install/Installer.csproj
- TransactionManager
- SetUp
- Sonny.RevitExtensions/install/Installer.csproj
- ElementIdExtensions
- AppConstants.cs
- .nuke/parameters.json
- ColumnDimension Feature Icon
- Sonny.RevitExtensions.Extensions.XYZs
- Border
- Sonny.RevitExtensions/.nuke/parameters.json
- ReferenceExtensions.cs
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
- Host
- XYZUtilityExtensions
- LoginCommand
- Sonny.Application.Domain.Exceptions
- .GetIntersectingElements
- .ExecuteInternal
- SonnyModule
- .GetXYZPoints
- .GetOrCreateCircularFamilySymbol
- ColumnCreationStrategy
- RectangularColumnCreationStrategy
- CompositeFailurePreprocessor
- FailurePreprocessorType
- Architecture decision records
- .CreateUri
- graphify reference: query, path, explain
- PanelAttribute
- .GetXyzs
- .IsOnLayer
- Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao
- Q: Trace call flow cua AutoColumnDimensionCommand tu command den interactor
- .ExecuteInternal
- .GetAllTypeParameters
- LicenseDataset
- .GetXyzes
- XYZTransformExtensions
- string
- graphify reference: add a URL and watch a folder
- graphify reference: commit hook and native CLAUDE.md integration
- graphify reference: incremental update and cluster-only
- Test
- .IsBuiltInCategory
- Test
- graphify reference: GitHub clone and cross-repo merge
- graphify reference: transcribe video and audio
- extraction-spec.md

## God Nodes (most connected - your core abstractions)
1. `Sonny.Application.Domain.Services` - 55 edges
2. `CurveExtensions` - 33 edges
3. `Sonny.Application.Infrastructure.Revit.Services` - 30 edges
4. `Sonny.RevitExtensions.Extensions` - 27 edges
5. `KeygenConfigBuilder` - 26 edges
6. `ColumnFromCadViewModel` - 24 edges
7. `AppDisplayUnit` - 23 edges
8. `Sonny.Keygen.Services` - 23 edges
9. `Sonny.Keygen.Models` - 23 edges
10. `Sonny.Application.Domain.Entities.ColumnFromCad.Models` - 22 edges

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

## Communities (208 total, 35 thin omitted)

### Community 0 - "Sonny.Application.Domain.Services"
Cohesion: 0.13
Nodes (8): Sonny.Application.Domain.Entities.Settings, Sonny.Application.Domain.Services, Sonny.Application.Domain.Entities.Settings.Models, Sonny.Application.Presentation.Implements, Sonny.Application.Tests.Core.UnitTests.Services, Sonny.Application.Infrastructure.Settings.Implements, Sonny.Application.Presentation.Bases, Sonny.Application.Presentation.Services

### Community 1 - "Sonny.RevitExtensions.Extensions"
Cohesion: 0.22
Nodes (8): Sonny.RevitExtensions.Extensions, Sonny.Application.Features.ColumnFromCad.Services, Sonny.RevitExtensions.Extensions.GeometryObjects.Solids, Sonny.RevitExtensions.Extensions.GeometryObjects.Curves, Sonny.RevitExtensions.Extensions.GeometryObjects, Sonny.Application.Infrastructure.Features.ColumnFromCad.Implements, Sonny.RevitExtensions.Extensions.CurveLoops, Sonny.Application.Features.ColumnFromCad.Interfaces

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

### Community 6 - "Sonny.Application.Domain"
Cohesion: 0.50
Nodes (3): Sonny.Application.Domain, double, Constraint

### Community 7 - "CurveExtensions"
Cohesion: 0.13
Nodes (9): ModelLine, Curve, CurveLoop, Document, IEnumerable, IList, Line, XYZ (+1 more)

### Community 8 - "ProgressReporter"
Cohesion: 0.12
Nodes (7): IProgressReporter, ProgressReporter, ProgressBar, Window, string, ProgressView, ProgressBar

### Community 9 - "CircularColumnModel"
Cohesion: 0.08
Nodes (23): CircularColumnModel, RectangularColumnModel, double, ImportInstance, List, CircularColumnExtractor, Arc, Curve (+15 more)

### Community 10 - "TransactionGroupManager"
Cohesion: 0.25
Nodes (3): bool, TransactionGroupManager, TransactionGroup

### Community 11 - "LicenseFileService"
Cohesion: 0.11
Nodes (12): Certificate, JsonElement, LicenseDataset, int, string, Certificate, LicenseData, LicenseFileService (+4 more)

### Community 12 - "LoginViewModel"
Cohesion: 0.10
Nodes (12): ObservableObject, RoutedEventArgs, UserInfoResult, IMessageBoxService, MessageBoxService, Task, UserInfoService, RelayCommand (+4 more)

### Community 13 - "Sonny.Application.Infrastructure/ServiceRegistration.cs"
Cohesion: 0.12
Nodes (11): Sonny.Application, Sonny.Application.Infrastructure, Sonny.Application.Infrastructure.Resource.Implements, Sonny.Application.UseCases.ColumnFromCad.Implements, Sonny.Application.UseCases, Sonny.Application.UseCases.AutoColumnDimension.Services, Sonny.Application.UseCases.ColumnFromCad.Services, Sonny.Application.Tests.Features.ColumnFromCad.IntegrationTests (+3 more)

### Community 14 - "Panel: General"
Cohesion: 0.06
Nodes (32): Button: ALPHA BIM, Button: Create 3D Box, Button: Create Elements from Room, Button: Create Pipe from Line CAD Link, Button: Create Schedule, Button: Create Sewer, Button: Crop View, Button: Element from AutoCAD (+24 more)

### Community 15 - "ColumnCreationContext"
Cohesion: 0.21
Nodes (9): ColumnCreationContext, HashSet, List, Task, ColumnFromCadInteractor, HashSet, List, Task (+1 more)

### Community 16 - "ViewWrapperBase"
Cohesion: 0.14
Nodes (16): Grid, List, PlanarFace, XYZ, ColumnDimensionContext, XYZ, GridFinder, XYZ (+8 more)

### Community 17 - "Point3D"
Cohesion: 0.24
Nodes (6): Point3D, Element, XYZ, Point3DConverter, XYZ, IPoint3DConverter

### Community 18 - "XYZVectorExtensions"
Cohesion: 0.13
Nodes (8): Sonny.RevitExtensions, IEnumerable, XYZ, XYZComparisonExtensions, XYZ, XYZVectorExtensions, double, ToleranceConstants

### Community 19 - ".Create"
Cohesion: 0.25
Nodes (4): Application, Application, App, StartupEventArgs

### Community 20 - "KeygenAuthService"
Cohesion: 0.18
Nodes (7): KeygenRegisterResult, KeygenUserResult, List, UserLicensesResult, RestClient, Task, KeygenAuthService

### Community 21 - "ElementParameterExtensions"
Cohesion: 0.22
Nodes (10): BuiltInParameter, Definition, DefinitionBindingMap, Guid, Element, ElementId, IEnumerable, Parameter (+2 more)

### Community 22 - "IApplicationModule"
Cohesion: 0.13
Nodes (9): Sonny.EasyRibbon.MasterExample, Sonny.EasyRibbon.Modules, IReadOnlyList, Application, UIControlledApplication, IApplicationModule, List, UIControlledApplication (+1 more)

### Community 23 - "SonnyRevitTestBase"
Cohesion: 0.06
Nodes (27): ControlledApplication, Sonny.Application.Tests.Features.AutoColumnDimension.IntegrationTests, Sonny.Application.Tests, Sonny.Application.UnitTests, IAutoColumnDimensionInteractor, IRevitDocument, OneTimeSetUp, OneTimeTearDown (+19 more)

### Community 24 - "Sonny.EasyRibbon.Extensions"
Cohesion: 0.14
Nodes (11): Sonny.EasyRibbon.UIAttributeBase, Sonny.EasyRibbon.UIAttributeBase.Base, Sonny.EasyRibbon.Extensions, StringExtension, IRibbonItem, Type, ButtonAttribute, PulldownButtonDataAttribute (+3 more)

### Community 25 - "Sonny.Keygen"
Cohesion: 0.10
Nodes (23): Auth0.OidcClient.WPF (4.4.0), Hardware.Info (10.0.0), NSec.Cryptography (22.4.0), Portable.BouncyCastle (1.9.0), RestSharp (108.0.1), System.Management (10.0.1), Sonny.Keygen, net48 (+15 more)

### Community 26 - ".CreatePushButtonData"
Cohesion: 0.17
Nodes (7): Attribute, BitmapImage, ImageExtension, ResourceExtension, UIAttributeBase, PushButtonData, PulldownButtonData

### Community 27 - ".CollectStackedItems"
Cohesion: 0.20
Nodes (13): buttons, pulldownConfigs, data, IList, List, PulldownButtonData, PushButtonData, RibbonItem (+5 more)

### Community 28 - "Sonny.EasyRibbon.csproj"
Cohesion: 0.07
Nodes (32): Sonny.EasyRibbon.Example, net48, net8.0-windows, ILRepack (2.0.41), Microsoft.Extensions.DependencyInjection (9.0.3), Nice3point.Revit.Api.RevitAPI ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPIUI ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1) (+24 more)

### Community 29 - "Window"
Cohesion: 0.10
Nodes (20): AllCircularColumnTypeParameters, AllColumnFamilies, AllLayerNames, AllLevels, AllRectangularColumnTypeParameters, BaseLevel, BaseOffsetDisplay, DiameterParameter (+12 more)

### Community 30 - "Sonny.EasyRibbon.Example"
Cohesion: 0.15
Nodes (9): Sonny.EasyRibbon.Example, Sonny.EasyRibbon.Example.Commands, ExternalCommand, StartupCommand, SonnyTab2, SonnyPannel1, SonnyTab3, SonnyPannel4 (+1 more)

### Community 31 - "UnitConverterTests"
Cohesion: 0.21
Nodes (5): ForgeTypeId, UnitConverter, SetUp, Test, UnitConverterTests

### Community 32 - "Sonny.Keygen.UI.Views"
Cohesion: 0.15
Nodes (7): Sonny.Keygen.Test.Net48, Sonny.Keygen.UI.Views, Sonny.Keygen.Test, Application, App, Window, MainWindow

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
Cohesion: 0.10
Nodes (13): Sonny.Keygen.Services, Sonny.Application.Infrastructure.License, Sonny.Keygen.Models, Sonny.Keygen.UI.ViewModels, ActivateMachineResult, DateTime, LicenseValidationResult, KeygenConfig (+5 more)

### Community 37 - "LicenseService"
Cohesion: 0.11
Nodes (14): ApiError, CheckoutAttributes, CheckoutData, CreateLicenseResult, List, RestClient, Task, ApiError (+6 more)

### Community 38 - "Window"
Cohesion: 0.25
Nodes (7): Email, LicenseExpiryDate, LicenseStartDate, LicenseType, LoginCommand, LogoutCommand, Window

### Community 39 - "AutoColumnDimensionViewModel"
Cohesion: 0.09
Nodes (15): DimensionTypeModel, List, IDimensionTypeProvider, IViewScaleProvider, DimensionType, List, DimensionTypeProvider, ViewScaleProvider (+7 more)

### Community 40 - "ICompositeFailurePreprocessor"
Cohesion: 0.22
Nodes (6): Sonny.Application.Infrastructure.Revit.FailuresPreprocessors, IFailuresPreprocessor, FailureProcessingResult, FailuresAccessor, SuppressWarningsPreprocessor, ICompositeFailurePreprocessor

### Community 41 - "Installer"
Cohesion: 0.13
Nodes (7): Installer, IEnumerable, WixEntity, Generator, IEnumerable, WixEntity, Generator

### Community 42 - "LanguageCode"
Cohesion: 0.06
Nodes (23): Sonny.ResourceManager, EventArgs, GetValueOrDefault, bool, SonnyResourcesInitializer, CultureInfo, CultureChangedEventArgs, CultureInfo (+15 more)

### Community 43 - "SettingsViewModel"
Cohesion: 0.20
Nodes (5): AppDisplayUnit, UnitOption, ObservableCollection, RelayCommand, SettingsViewModel

### Community 44 - "What You Must Do When Invoked"
Cohesion: 0.07
Nodes (26): For /graphify add and --watch, For /graphify query, For the commit hook and native CLAUDE.md integration, For --update and --cluster-only, /graphify, Honesty Rules, Interpreter guard for subcommands, Part A - Structural extraction for code files (+18 more)

### Community 45 - "SettingsService"
Cohesion: 0.15
Nodes (10): SettingsData, Func, string, SettingsData, SettingsService, SetUp, string, Test (+2 more)

### Community 46 - "LoginOrchestrator"
Cohesion: 0.42
Nodes (3): LoginOrchestrationResult, Task, LoginOrchestrator

### Community 47 - "Quantity Survey Panel"
Cohesion: 0.13
Nodes (16): Auto DIM Button, AutoCAD to Revit Button, Beam Rebar Button, Column/Wall Rebar Button, Concrete Column/Wall Button, Export Quantity Button, Formwork Area Button, Load Family Button (+8 more)

### Community 48 - "AuthService"
Cohesion: 0.22
Nodes (7): Auth0Client, ClaimsPrincipal, LoginResult, LogoutResult, Task, AuthService, TokenData

### Community 49 - "Sonny.ResourceManager.csproj"
Cohesion: 0.33
Nodes (5): WPFLocalizeExtension (3.9.0), net48, net8.0-windows, Microsoft.Extensions.DependencyInjection (9.0.3), Microsoft.NET.Sdk

### Community 50 - "ElementCurveExtensions"
Cohesion: 0.10
Nodes (19): Sonny.RevitExtensions.Extensions.GeometryObjects.GeometryElements, GeometryElement, PolyLine, Arc, Curve, Edge, Element, IEnumerable (+11 more)

### Community 51 - "Sonny.Application.Infrastructure.Revit.Services"
Cohesion: 0.17
Nodes (4): Sonny.Application.Infrastructure.Revit.Services, Sonny.Application.Infrastructure.Revit.Implements, Sonny.Application.Infrastructure.Revit.Managers.Transactions, Sonny.Application.Domain.Entities

### Community 52 - "Window"
Cohesion: 0.25
Nodes (6): Sonny.Application.Presentation.Settings.ViewModels, AutoColumnDimensionView, SettingsView, Window, MainWindow, Window

### Community 53 - "IMessageService"
Cohesion: 0.15
Nodes (7): IMessageService, IResourceHelper, ResourceHelper, ILogger, CommonServices, ILogger, ICommonServices

### Community 54 - ".GetAllElements"
Cohesion: 0.24
Nodes (9): BuiltInCategory, Document, Element, ElementId, IEnumerable, Reference, Type, View (+1 more)

### Community 55 - "FaceCurveExtensions"
Cohesion: 0.23
Nodes (8): Arc, Curve, Edge, Face, IEnumerable, Line, XYZ, FaceCurveExtensions

### Community 56 - "Sonny.Application.Domain.Entities.ColumnFromCad.Models"
Cohesion: 0.23
Nodes (6): Sonny.Application.Domain.Entities.ColumnFromCad.Services, Sonny.Application.Domain.Entities.ColumnFromCad.Models, Sonny.Application.Infrastructure.Features.ColumnFromCad.Services, Sonny.Application.Domain.Entities.ColumnFromCad.Contexts, Sonny.Application.Infrastructure.Features.ColumnFromCad.Strategies, Sonny.RevitExtensions.Extensions.Families

### Community 57 - "AppLanguageCode"
Cohesion: 0.22
Nodes (4): AppLanguageCode, ResourcesInitializer, LanguageChangeHandler, LanguageCodeConverter

### Community 58 - "Sonny.Application.csproj"
Cohesion: 0.15
Nodes (13): Serilog.Sinks.Console (6.0.0), Serilog.Sinks.Debug (3.0.0), Serilog.Sinks.File (6.0.0), Sonny.Application, net48, net8.0-windows, ILRepack (2.0.41), Microsoft.Extensions.DependencyInjection (9.0.3) (+5 more)

### Community 59 - "CircularColumnExtractor"
Cohesion: 0.23
Nodes (9): CircularColumnModel, ImportInstance, List, ICircularColumnExtractor, CircularColumnModel, double, ImportInstance, List (+1 more)

### Community 60 - "LanguageCodeExtensionsTests"
Cohesion: 0.08
Nodes (11): Sonny.Application.Tests.Utils, Sonny.Application.UseCases.Settings.Models, Sonny.Application.Tests.ResourceManager.UnitTests, Test, CultureChangedEventArgsTests, Test, LanguageCodeExtensionsTests, Test (+3 more)

### Community 61 - "ColumnModel"
Cohesion: 0.25
Nodes (6): Point3D, ColumnModel, List, IColumnDataExtractor, List, ColumnDataExtractor

### Community 62 - "FaceExtensions"
Cohesion: 0.32
Nodes (6): Curve, Face, IEnumerable, Line, XYZ, FaceExtensions

### Community 63 - ".GetIntersectingElements"
Cohesion: 0.16
Nodes (10): Document, Element, ElementId, ICollection, IEnumerable, List, Solid, View (+2 more)

### Community 64 - "XYZGeometryExtensions"
Cohesion: 0.26
Nodes (3): IEnumerable, XYZ, XYZGeometryExtensions

### Community 65 - "IRectangularColumnExtractor"
Cohesion: 0.26
Nodes (8): ImportInstance, List, RectangularColumnModel, IRectangularColumnExtractor, ImportInstance, List, RectangularColumnModel, RectangularColumnExtractor

### Community 66 - ".CreateDimensions"
Cohesion: 0.27
Nodes (7): DimensionType, List, AutoColumnDimension, DimensionType, List, IAutoColumnDimension, ColumnWrapperBase

### Community 67 - ".DimensionByDirection"
Cohesion: 0.17
Nodes (10): DimensionType, List, PlanarFace, XYZ, DimensionCreator, DimensionType, List, PlanarFace (+2 more)

### Community 68 - "Sonny.Application.Infrastructure.csproj"
Cohesion: 0.17
Nodes (12): Sonny.Application.Infrastructure, net48, net8.0-windows, Microsoft.Extensions.DependencyInjection (9.0.3), Newtonsoft.Json (13.0.3), Nice3point.Revit.Api.RevitAPI ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPIUI ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1) (+4 more)

### Community 69 - "Sonny.Application.Presentation/ServiceRegistration.cs"
Cohesion: 0.14
Nodes (11): Sonny.Application.Presentation.AutoColumnDimension.Views, Sonny.Application.Presentation.Extensions, Sonny.Application.Presentation.AutoColumnDimension.ViewModels, Sonny.Application.Presentation.Views, Sonny.Application.Presentation.ColumnFromCad.ViewModels, Sonny.Application.Presentation.ColumnFromCad.Views, ColumnFromCadView, Window (+3 more)

### Community 70 - "ElementQuery"
Cohesion: 0.20
Nodes (8): FilteredElementCollector, IEnumerable, IEnumerator, Document, ElementId, Type, View, ElementQuery

### Community 71 - "Sonny.RevitExtensions"
Cohesion: 0.17
Nodes (11): MoreLinq (3.4.2), Sonny.RevitExtensions, net48, net8.0-windows, Nice3point.Revit.Api.AdWindows ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPI ($(RevitVersion).*), Nice3point.Revit.Api.RevitAPIUI ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1) (+3 more)

### Community 72 - ".GetPlanarFaces"
Cohesion: 0.25
Nodes (7): CylindricalFace, Face, IEnumerable, PlanarFace, Solid, XYZ, SolidFaceExtensions

### Community 73 - "Sonny.sln"
Cohesion: 0.18
Nodes (8): Installer, net48, net8.0-windows, Microsoft.NET.Sdk, net48, net8.0-windows, Microsoft.Extensions.DependencyInjection (9.0.3), Microsoft.NET.Sdk

### Community 74 - "IRevitDocument"
Cohesion: 0.18
Nodes (10): Document, UIApplication, UIDocument, View, RevitDocument, Document, UIApplication, UIDocument (+2 more)

### Community 75 - "Sonny.Application.Presentation.csproj"
Cohesion: 0.18
Nodes (11): Sonny.Application.Presentation, net48, net8.0-windows, CommunityToolkit.Mvvm (8.4.0), Microsoft.Extensions.DependencyInjection (9.0.3), Newtonsoft.Json (13.0.3), Nice3point.Revit.Api.AdWindows ($(RevitVersion).*), Nice3point.Revit.Build.Tasks (3.0.1) (+3 more)

### Community 76 - ".TryAutoLoginAsync"
Cohesion: 0.31
Nodes (5): AutoLoginResult, DateTime, LicenseInfo, Task, AutoLoginService

### Community 77 - ".GetCylindricalFaces"
Cohesion: 0.26
Nodes (8): CylindricalFace, Element, Face, IEnumerable, Options, PlanarFace, Solid, ElementFaceExtensions

### Community 78 - "SolidCurveExtensions"
Cohesion: 0.29
Nodes (7): Arc, Curve, Edge, IEnumerable, Line, Solid, SolidCurveExtensions

### Community 79 - "Window"
Cohesion: 0.20
Nodes (9): LanguageOptions, SaveCommand, SelectedLanguageOption, SelectedUnitOption, UnitOptions, ThisWindow, Window, CancelCommand (+1 more)

### Community 81 - "SonnyPannel2"
Cohesion: 0.33
Nodes (6): SonnyButton1, SonnyButton2, SonnyButton3, SonnyButton4, SonnyPannel2, SonnySpitButton2

### Community 82 - "TypeSelectionFilter"
Cohesion: 0.20
Nodes (7): Sonny.Application.Infrastructure.Revit.SelectionFilters, ISelectionFilter, Element, List, Reference, XYZ, TypeSelectionFilter

### Community 83 - "MachineFingerprintService"
Cohesion: 0.22
Nodes (5): HardwareInfo, object, string, MachineFingerprintService, SerialNumber

### Community 84 - "Sonny.Application.Tests.csproj"
Cohesion: 0.20
Nodes (10): NSubstitute (5.3.0), Revit_All_Main_Versions_API_x64 ($(RevitVersion).*), ricaun.Revit.UI.Tasks (*), ricaun.RevitTest.TestAdapter (*), Sonny.Application.Tests, net48, net8.0-windows, Microsoft.NET.Test.Sdk (*) (+2 more)

### Community 85 - "AppDisplayUnit"
Cohesion: 0.17
Nodes (6): AppDisplayUnit, IDisplayUnitProvider, Func, ISettingsService, IUnitConverter, DisplayUnitProvider

### Community 86 - "UIDocumentProvider"
Cohesion: 0.27
Nodes (5): object, UIDocument, UIDocumentProvider, UIDocument, IUIDocumentProvider

### Community 87 - "KeygenModels.cs"
Cohesion: 0.40
Nodes (9): List, Attributes, Data, Document, DocumentArray, Error, PolicyData, PolicyRelationship (+1 more)

### Community 88 - "Sonny.RevitExtensions"
Cohesion: 0.22
Nodes (10): Sonny.RevitExtensions Compile workflow, Nuke build (RevitExtensions), Sonny.RevitExtensions PublishRelease workflow, Sonny.RevitExtensions Changelog, DimensionProcessor, DocumentExtension, ElementQuery, Sonny.RevitExtensions (+2 more)

### Community 89 - "PlanarFaceExtensions"
Cohesion: 0.42
Nodes (4): IEnumerable, PlanarFace, XYZ, PlanarFaceExtensions

### Community 90 - "BaseExternalCommand.cs"
Cohesion: 0.29
Nodes (3): Sonny.Application.UseCases.Services, ILicenseCheckService, LicenseCheckService

### Community 91 - "Window"
Cohesion: 0.22
Nodes (8): DimensionTypes, DisplayUnitName, RunCommand, SelectedDimensionType, SnapDistanceDisplay, ThisWindow, Window, Window

### Community 92 - "LineExtensions"
Cohesion: 0.39
Nodes (4): Sonny.RevitExtensions.Extensions.GeometryObjects.Curves.Lines, Line, XYZ, LineExtensions

### Community 93 - "Sonny.Application.UnitTests.csproj"
Cohesion: 0.25
Nodes (7): net9.0, coverlet.collector (6.0.2), Microsoft.NET.Test.Sdk (17.12.0), NUnit (4.2.2), NUnit3TestAdapter (4.6.0), NUnit.Analyzers (4.4.0), Microsoft.NET.Sdk

### Community 94 - "AutoColumnDimensionInteractor"
Cohesion: 0.36
Nodes (5): DimensionType, int, List, string, AutoColumnDimensionInteractor

### Community 95 - "CadLinkSelector"
Cohesion: 0.22
Nodes (6): ImportInstance, UIDocument, CadLinkSelector, ImportInstance, UIDocument, ICadLinkSelector

### Community 96 - "SonnyModule.cs"
Cohesion: 0.15
Nodes (11): Sonny.Application.Ribbon, Sonny.EasyRibbon, Sonny.Application.Modules, AutoColumnDimensionButton, ColumnFromCadButton, ColumnFromCadPanel, DimensionPanel, LicenseButton (+3 more)

### Community 97 - "Sonny Tab (Revit ribbon tab)"
Cohesion: 0.33
Nodes (9): Declarative Tab/Panel/Button attribute layout, Sonny Panel 1, Sonny Panel 2, Sonny Panel 4, PulldownButton (Sonny Pulldown Button Data 1 / 4), PushButton element (Sonny Button 1-1..2-4, 4-1, 4-2), Sonny.EasyRibbon Ribbon Demo Screenshot, Sonny Tab (Revit ribbon tab) (+1 more)

### Community 98 - "ExampleModule"
Cohesion: 0.33
Nodes (4): ExternalApplication, Application, UIControlledApplication, ExampleModule

### Community 99 - "XYZDistanceExtensions"
Cohesion: 0.31
Nodes (4): IEnumerable, Line, XYZ, XYZDistanceExtensions

### Community 100 - "ElementWrapperBase"
Cohesion: 0.15
Nodes (9): BoundingBoxXYZ, Dimension, DimensionWrapperBase, Dictionary, Element, XYZ, ElementWrapperBase, WallWrapperBase (+1 more)

### Community 101 - ".InvokeAsync"
Cohesion: 0.31
Nodes (7): BrowserOptions, BrowserResult, CancellationToken, IBrowser, string, Task, CustomWebViewBrowser

### Community 102 - "LoggerConfiguration"
Cohesion: 0.29
Nodes (5): Sonny.Application.Config.Logging, Logger, IServiceCollection, string, LoggerConfiguration

### Community 103 - "Sonny.Application.Commands"
Cohesion: 0.21
Nodes (7): Sonny.Application.Commands, Sonny.Application.Presentation.Settings.Views, Sonny.Application.Bases, ElementSet, ExternalCommandData, Result, SettingsCommand

### Community 104 - "UIStyleManager"
Cohesion: 0.43
Nodes (3): Sonny.Application.UIStyle, Application, UIStyleManager

### Community 106 - "graphify reference: extra exports and benchmark"
Cohesion: 0.22
Nodes (8): graphify reference: extra exports and benchmark, Step 6b - Wiki (only if --wiki flag), Step 7 - Neo4j export (only if --neo4j or --neo4j-push flag), Step 7a - FalkorDB export (only if --falkordb or --falkordb-push flag), Step 7b - SVG export (only if --svg flag), Step 7c - GraphML export (only if --graphml flag), Step 7d - MCP server (only if --mcp flag), Step 8 - Token reduction benchmark (only if total_words > 5000)

### Community 107 - "SonnyPulldownButton4"
Cohesion: 0.29
Nodes (7): SonnyButton4_1, SonnyButton4_2, SonnyButton4_3, SonnyButton4_4, SonnyButton4_5, SonnyPulldownButton4, SonnyStackedButton4

### Community 108 - ".GetSolids"
Cohesion: 0.33
Nodes (5): HashSet, IEnumerable, ImportInstance, Solid, ImportInstanceExtensions

### Community 109 - "Develop branch"
Cohesion: 0.76
Nodes (7): Develop branch, Git Flow Branching Model Diagram, Feature branch, Hotfix branch, Master branch, Release branch, Version tags (v0.1, v0.2, v1.0)

### Community 110 - "ParallelXyzEqualityComparer"
Cohesion: 0.38
Nodes (4): Sonny.RevitExtensions.EqualityComparers, IEqualityComparer, XYZ, ParallelXyzEqualityComparer

### Community 111 - "IElementSelector"
Cohesion: 0.29
Nodes (4): ICollection, IElementSelector, ICollection, ElementSelector

### Community 112 - ".RunAsync"
Cohesion: 0.18
Nodes (8): Action, Func, Task, IRevitTaskRunner, Action, Func, Task, RevitTaskRunner

### Community 113 - "BaseExternalCommand"
Cohesion: 0.43
Nodes (5): IExternalCommand, ElementSet, ExternalCommandData, Result, BaseExternalCommand

### Community 114 - "SonnyPannel1"
Cohesion: 0.29
Nodes (7): SonnyButton1, SonnyButton12, SonnyButton2, SonnyButton3, SonnyPannel1, SonnySpitButton1, SonnyTab1

### Community 115 - "IColumnCreationStrategy"
Cohesion: 0.33
Nodes (3): IColumnCreationStrategy, IColumnCreationStrategyFactory, ColumnCreationStrategyFactory

### Community 116 - ".AddStackedItemsMixed"
Cohesion: 0.29
Nodes (5): IList, List, RibbonItem, RibbonPanel, RibbonPanelExtension

### Community 117 - ".IsBuiltInCategory"
Cohesion: 0.33
Nodes (4): Category, BuiltInCategory, Document, CategoryExtensions

### Community 118 - "FamilyInstanceWrapperBase"
Cohesion: 0.33
Nodes (6): FamilyInstance, double, List, XYZ, FamilyInstanceWrapperBase, Transform

### Community 119 - "PulldownButtonData1"
Cohesion: 0.29
Nodes (7): PulldownButtonData1, SonnyButton1, SonnyButton2, SonnyButton3, SonnyButton4, SonnyButton5, SonnyButton6

### Community 120 - "Revit R Gear Processor"
Cohesion: 0.40
Nodes (6): Transformation Arrow, CAD Drawing Blueprint (input), ColumnFromCad Feature (CAD to Revit columns), ColumnFromCadCommand Icon Illustration, Revit R Gear Processor, Structural Column (output)

### Community 121 - "Sonny.Application.UIStyle.csproj"
Cohesion: 0.33
Nodes (6): Sonny.Application.UIStyle, net48, net8.0-windows, Nice3point.Revit.Build.Tasks (3.0.1), Nice3point.Revit.Toolkit ($(RevitVersion).*), Microsoft.NET.Sdk

### Community 122 - "TypeExtension"
Cohesion: 0.47
Nodes (3): List, Type, TypeExtension

### Community 123 - "UIControlledApplicationExtension"
Cohesion: 0.40
Nodes (3): RibbonPanel, UIControlledApplication, UIControlledApplicationExtension

### Community 124 - "ITransactionGroupManager"
Cohesion: 0.14
Nodes (7): IDisposable, ITransactionGroupManager, ITransactionManager, IEnumerable, ITransactionManagerFactory, IEnumerable, TransactionManagerFactory

### Community 125 - ".GetCurves"
Cohesion: 0.33
Nodes (4): Curve, CurveLoop, IEnumerable, CurveLoopExtensions

### Community 126 - ".GetFamilySymbols"
Cohesion: 0.33
Nodes (4): Family, FamilySymbol, IEnumerable, FamilyExtensions

### Community 127 - "Sonny.RevitExtensions.Extensions.GeometryObjects.Faces"
Cohesion: 0.22
Nodes (5): Sonny.RevitExtensions.Extensions.GeometryObjects.Faces, Face, IEnumerable, XYZ, FacePointExtensions

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

### Community 133 - "TransactionManager"
Cohesion: 0.16
Nodes (7): DomainTransactionStatus, bool, IFailuresPreprocessor, TransactionManager, TransactionStatusConverter, Transaction, TransactionStatus

### Community 135 - "Sonny.RevitExtensions/install/Installer.csproj"
Cohesion: 0.40
Nodes (4): net48, WixSharp.bin (1.26.0), WixSharp.wix.bin (3.14.1), Microsoft.NET.Sdk

### Community 137 - "AppConstants.cs"
Cohesion: 0.50
Nodes (3): Sonny.Keygen, string, AppConstants

### Community 138 - ".nuke/parameters.json"
Cohesion: 0.50
Nodes (3): $schema, Solution, Verbosity

### Community 139 - "ColumnDimension Feature Icon"
Cohesion: 0.67
Nodes (4): Column Glyph (top/bottom caps + shaft), AutoColumnDimension Revit Feature, ColumnDimension Feature Icon, Vertical Double-Headed Dimension Arrow

### Community 140 - "Sonny.RevitExtensions.Extensions.XYZs"
Cohesion: 0.20
Nodes (7): Sonny.Application.Infrastructure.Features.AutoColumnDimension.Implements, Sonny.Application.Infrastructure.Features.AutoColumnDimension.Services, Sonny.RevitExtensions.Extensions.GeometryObjects.Faces.PlanarFaces, Sonny.RevitExtensions.RevitWrapper, Sonny.RevitExtensions.Extensions.Elements, Sonny.RevitExtensions.Extensions.XYZs, Sonny.Application.Infrastructure.Features.AutoColumnDimension.Contexts

### Community 141 - "Border"
Cohesion: 0.50
Nodes (3): Border, ResourceDictionary, Border

### Community 142 - "Sonny.RevitExtensions/.nuke/parameters.json"
Cohesion: 0.50
Nodes (3): $schema, Solution, Verbosity

### Community 171 - "Host"
Cohesion: 0.32
Nodes (4): IServiceProvider, object, Host, UnhandledExceptionEventArgs

### Community 173 - "LoginCommand"
Cohesion: 0.33
Nodes (4): ElementSet, ExternalCommandData, Result, LoginCommand

### Community 174 - "Sonny.Application.Domain.Exceptions"
Cohesion: 0.40
Nodes (3): Sonny.Application.Domain.Exceptions, Exception, TransactionCommitFailedException

### Community 175 - ".GetIntersectingElements"
Cohesion: 0.22
Nodes (7): Document, Element, ElementId, ICollection, List, View, ElementIntersectExtensions

### Community 176 - ".ExecuteInternal"
Cohesion: 0.40
Nodes (4): ElementSet, ExternalCommandData, Result, AutoColumnDimensionCommand

### Community 177 - "SonnyModule"
Cohesion: 0.43
Nodes (3): UIControlledApplication, SonnyModule, SonnyApp

### Community 178 - ".GetXYZPoints"
Cohesion: 0.33
Nodes (4): Curve, IEnumerable, XYZ, CurvePointExtensions

### Community 179 - ".GetOrCreateCircularFamilySymbol"
Cohesion: 0.38
Nodes (4): Family, FamilySymbol, CircularColumnCreationStrategy, Parameter

### Community 180 - "ColumnCreationStrategy"
Cohesion: 0.38
Nodes (4): double, Element, FamilySymbol, ColumnCreationStrategy

### Community 181 - "RectangularColumnCreationStrategy"
Cohesion: 0.60
Nodes (3): Family, FamilySymbol, RectangularColumnCreationStrategy

### Community 182 - "CompositeFailurePreprocessor"
Cohesion: 0.29
Nodes (5): FailureProcessingResult, FailuresAccessor, IFailuresPreprocessor, List, CompositeFailurePreprocessor

### Community 183 - "FailurePreprocessorType"
Cohesion: 0.24
Nodes (7): FailurePreprocessorType, IEnumerable, IFailuresPreprocessor, FailurePreprocessorFactory, IEnumerable, IFailuresPreprocessor, IFailurePreprocessorFactory

### Community 184 - "Architecture decision records"
Cohesion: 0.33
Nodes (5): Architecture decision records, Format, What does not belong, What qualifies here, When to write one

### Community 186 - "graphify reference: query, path, explain"
Cohesion: 0.33
Nodes (5): For /graphify explain, For /graphify path, graphify reference: query, path, explain, Step 0 — Constrained query expansion (REQUIRED before traversal), Step 1 — Traversal

### Community 187 - "PanelAttribute"
Cohesion: 0.33
Nodes (4): RibbonPanel, string, UIControlledApplication, PanelAttribute

### Community 188 - ".GetXyzs"
Cohesion: 0.29
Nodes (5): Element, IEnumerable, Options, XYZ, ElementPointExtensions

### Community 189 - ".IsOnLayer"
Cohesion: 0.40
Nodes (3): GeometryObject, Document, GeometryObjectExtensions

### Community 190 - "Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao"
Cohesion: 0.40
Nodes (4): Answer, Outcome, Q: UIDocument lifetime va IUIDocumentProvider hoat dong the nao, Source Nodes

### Community 191 - "Q: Trace call flow cua AutoColumnDimensionCommand tu command den interactor"
Cohesion: 0.40
Nodes (4): Answer, Outcome, Q: Trace call flow cua AutoColumnDimensionCommand tu command den interactor, Source Nodes

### Community 192 - ".ExecuteInternal"
Cohesion: 0.40
Nodes (4): ElementSet, ExternalCommandData, Result, ColumnFromCadCommand

### Community 193 - ".GetAllTypeParameters"
Cohesion: 0.40
Nodes (3): Element, IEnumerable, ElementParameterExtensions

### Community 194 - "LicenseDataset"
Cohesion: 0.50
Nodes (4): LicenseData, LicenseMeta, DateTime, LicenseDataset

### Community 195 - ".GetXyzes"
Cohesion: 0.40
Nodes (4): IEnumerable, Solid, XYZ, SolidPointExtensions

### Community 198 - "graphify reference: add a URL and watch a folder"
Cohesion: 0.50
Nodes (3): For /graphify add, For --watch, graphify reference: add a URL and watch a folder

### Community 199 - "graphify reference: commit hook and native CLAUDE.md integration"
Cohesion: 0.50
Nodes (3): For git commit hook, For native CLAUDE.md integration, graphify reference: commit hook and native CLAUDE.md integration

### Community 200 - "graphify reference: incremental update and cluster-only"
Cohesion: 0.50
Nodes (3): For --cluster-only, For --update (incremental re-extraction), graphify reference: incremental update and cluster-only

### Community 202 - ".IsBuiltInCategory"
Cohesion: 0.40
Nodes (3): BuiltInCategory, Element, ElementExtensions

## Ambiguous Edges - Review These
- `Sonny.Application.Presentation` → `Sonny.Application.Infrastructure`  [AMBIGUOUS]
  CLAUDE.md · relation: references

## Knowledge Gaps
- **368 isolated node(s):** `net9.0`, `coverlet.collector (6.0.2)`, `Microsoft.NET.Test.Sdk (17.12.0)`, `NUnit (4.2.2)`, `NUnit.Analyzers (4.4.0)` (+363 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **35 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What is the exact relationship between `Sonny.Application.Presentation` and `Sonny.Application.Infrastructure`?**
  _Edge tagged AMBIGUOUS (relation: references) - confidence is low._
- **Why does `BaseViewModel` connect `BaseViewModel` to `AutoColumnDimensionViewModel`, `SettingsViewModel`, `LoginViewModel`, `IMessageService`, `AppDisplayUnit`?**
  _High betweenness centrality (0.000) - this node is a cross-community bridge._
- **Why does `BaseViewModelWithSettings` connect `ColumnFromCadViewModel` to `BaseViewModel`?**
  _High betweenness centrality (0.000) - this node is a cross-community bridge._
- **Why does `KeygenConfigBuilder` connect `KeygenConfigBuilder` to `.Create`?**
  _High betweenness centrality (0.000) - this node is a cross-community bridge._
- **What connects `net9.0`, `coverlet.collector (6.0.2)`, `Microsoft.NET.Test.Sdk (17.12.0)` to the rest of the system?**
  _368 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Sonny.Application.Domain.Services` be split into smaller, more focused modules?**
  _Cohesion score 0.13333333333333333 - nodes in this community are weakly interconnected._
- **Should `AutoColumnDimensionInteractor (documented)` be split into smaller, more focused modules?**
  _Cohesion score 0.05297532656023222 - nodes in this community are weakly interconnected._