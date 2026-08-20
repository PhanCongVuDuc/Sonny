# Business decisions live in UseCases/Domain; Infrastructure keeps only Revit mechanism behind interfaces

Several Infrastructure classes mix business decisions with Revit mechanism — `AutoColumnDimensionInteractor`
is the canonical case (use-case role, Infrastructure placement), while `ColumnFromCadInteractor` already
shows the pure form. This refactor classifies all 131 product classes into four groups (already in place /
straight move / must split / correct in Infrastructure) and moves the decision halves down to
UseCases/Domain so they are unit-testable without Revit, in `Sonny.Application.UnitTests`.

> **DRAFT** — sections `## Decisions` and (later) `## Plan` are working state for the refactor flow and
> will be deleted when the refactor completes, leaving only the decision record.

## Decisions

- **Q1 — Phạm vi move (ĐÃ CHỐT)**: phân loại phủ cả 131 class; move thực hiện ở mọi chỗ phân loại ra
  "chuyển thẳng / phải tách", **trừ cụm license** (KeygenLicenseValidator + Sonny.Keygen consumers) —
  cụm đó chỉ phân loại và ghi backlog, không đụng code.
- **Q2 — Hình dạng tách AutoColumnDimension (ĐÃ CHỐT)**: phương án (b) — interactor xuống UseCases,
  mọi bước Revit đứng sau interface Revit-free, **và** policy hình học (chọn cặp direction plan/non-plan,
  điều kiện skip BasisZ, quy tắc crossed grid lookup) tách thành logic thuần ở UseCases/Domain.
  Infrastructure chỉ còn: đọc dữ liệu từ Revit → hỏi policy → thi hành.
- **Q4 — ColumnFromCad (ĐÃ CHỐT)**: tách cả quadrant/rotation math (ColumnModelFactory) và
  shape-detection (rect = polyline 5 toạ độ / 4 curve; tròn = điểm cách đều tâm ±1e-4) xuống logic thuần
  Revit-free. Phần đọc CAD (GetPolyLines/GetSolids/IsOnLayer) ở lại Infrastructure.
- **Q5 — Không đáng tách (ĐÃ CHỐT)**: DimensionCreator, GridFinder, transaction managers, UnitConverter,
  Point3DConverter, ElementSelector, CadLinkSelector, UIDocumentProvider, RevitDocument, các Provider đọc
  document, SettingsService, license cluster, Presentation services (MessageService, ProgressReporter) —
  đúng chỗ ở Infrastructure/Presentation, là cơ chế.
- **Q6 — Project mồ côi (ĐÃ CHỐT)**: xoá hẳn `source/Sonny.Application.Features/` — commit riêng, đầu
  chuỗi (không được build nên hành vi không đổi; giảm nhiễu index).
- **Q7 — Lưới an toàn (ĐÃ CHỐT)**: (1) gate = 2 integration test `Debug R23` chạy trước chuỗi move để có
  baseline xanh và sau mỗi cụm move; (2) characterization unit test viết trước move ở chỗ dựng được input
  không cần Revit process, không thì lưới là integration test; (3) unit test mới trong
  Sonny.Application.UnitTests (R25) viết cùng lúc tách, khoá hành vi hiện tại kể cả chỗ trông như bug
  (failed = arithmetic, silent skip); (4) **chấp nhận đi không lưới cho nhánh non-plan view** của
  AutoColumnDimension (không có test nào chạm, không dựng fixture mới trong đợt này) — ghi nhận rủi ro
  tường minh tại đây.
- **Q3 — Hình thức ranh giới (ĐÃ CHỐT)**: phương án (a) — **DTO thuần, dữ liệu đi qua ranh giới một
  lần**. Infrastructure đọc wrapper Revit → dựng DTO Revit-free (Point3D/double/string, ví dụ
  ColumnGeometryData, GridCandidate) → decision core trả về "plan" (ví dụ DimensionAxisPlan với
  GridUniqueId-or-null) → Infrastructure map id về wrapper và thi hành. Không trừu tượng hoá wrapper
  bằng interface; Reference/PlanarFace/XYZ không bao giờ rời Infrastructure. Test decision core không
  cần mock.
- **Q8 — Thứ tự move (ĐÃ CHỐT)**: (1) xoá project mồ côi → (2) chạy 2 integration test R23 chốt baseline
  xanh → (3) ColumnFromCad (shape-detection, quadrant math) → (4) AutoColumnDimension (DTO + decision
  core + interactor xuống UseCases) → (5) quét ngang "chuyển thẳng được" còn lại, trừ license →
  (6) unit tests viết song song từng bước, không dồn cuối. Gate integration sau mỗi cụm.
- **Q9 — Chỗ đặt logic thuần (ĐÃ CHỐT)**: phương án (c) — tính toán gắn với một model thì nằm trên
  Domain model (quadrant math → RectangularColumnModel factory; shape-detection → Domain
  ColumnFromCad); quyết định phối hợp nhiều dữ liệu của use case thì thành service trong UseCases
  (policy chọn direction/grid của AutoColumnDimension).

## Baseline — lưới an toàn trước khi move

Trạng thái test khoá hành vi, chốt tại thời điểm bắt đầu chuỗi move:

| Hành vi bị chạm | Lưới | Trạng thái |
|---|---|---|
| ColumnFromCadInteractor — orchestration, order dependency, silent skip, message routing | 5 characterization unit tests mới (`Sonny.Application.UnitTests/UseCases/ColumnFromCad/ColumnFromCadInteractorTests.cs`), viết trước move | ✅ 5/5 xanh, 351ms, Debug R25 |
| ColumnFromCad — extraction + creation end-to-end (37 rect + 8 tròn, 45 tạo, 6 rotation angle exact-equality) | `ColumnFromCadIntegrationTest` (Debug R23, Revit 2023, fixture `Test_V2023_ModelColumnFromAutoCad.rvt`) | ✅ baseline xanh 2026-08-21 |
| AutoColumnDimension — plan view (80 dimensions, view `Level 2`, `Test_V2023.rvt`) | `AutoColumnDimensionIntegrationTest` (Debug R23) | ✅ baseline xanh 2026-08-21 |

Baseline chạy 2026-08-21: `dotnet test source/Sonny.Application.Tests/... -c "Debug R23"` → **42/42 passed, 19s**
(Revit 2023 thật); `dotnet test source/Sonny.Application.UnitTests/... -c "Debug R25"` → **5/5 passed, 351ms**.
| AutoColumnDimension — quadrant/rotation math, shape-detection chi tiết | chỉ gián tiếp qua 2 integration test trên (rotation angles khoá quadrant math rất chặt) | gián tiếp |
| **AutoColumnDimension — nhánh non-plan view (elevation/section, BasisZ-skip)** | **KHÔNG CÓ LƯỚI** — không test nào chạm; quyết định tường minh (Q7): move không lưới, không dựng fixture mới | ⚠️ chấp nhận |
| MessageNoColumnsFound của AutoColumnDimension (0 cột trong view) | không có lưới tự động; hành vi được pin lại bằng unit test *sau khi* decision core tách ra | ⚠️ chấp nhận tạm |
| Settings/Resource/Presentation services (không move, chỉ có thể bị đổi using) | `Core/UnitTests` + `ResourceManager/UnitTests` trong project R23 | ✅ chạy cùng baseline |

## Phân loại 131 class

Đã đọc cả 6 project (Domain 34 · UseCases 8 · Infrastructure 52 · Presentation 17 · App 10 ·
ResourceManager 10). Kết quả 4 nhóm:

### Nhóm 1 — Đã đúng chỗ: 79 class

- **Domain (34/34)**: toàn bộ — entities, models, exceptions và 15 service interface đều Revit-free.
- **UseCases (8/8)**: `ColumnFromCadInteractor` (pure form mẫu), `LicenseCheckService`, các interface
  interactor, `LanguageOption`, `ServiceRegistration`.
- **Presentation (17/17)**: ViewModels/Views/Bases/`MessageService`/`ProgressReporter`/
  `ViewModelSettingsService`/`CommonServices` — logic UI đúng tầng, không tham chiếu Infrastructure.
- **Sonny.Application (10/10)**: entry point, commands mỏng, ribbon, Host, module, logging config.
- **ResourceManager (10/10)**: engine localization độc lập.

### Nhóm 2 — Chuyển thẳng được (không cần tách): 0 class

Không có class nào ở Infrastructure thuần quyết định đến mức nhấc nguyên sang UseCases/Domain mà không
phải tách phần cơ chế trước. Đây là kết quả khảo sát, không phải bỏ sót.

### Nhóm 3 — Phải tách (quyết định ⇢ UseCases/Domain, cơ chế ở lại): 8 class

| Class (Infrastructure) | Quyết định bị trộn | Đi đâu |
|---|---|---|
| `Features/AutoColumnDimension/Implements/AutoColumnDimensionInteractor` | validate, số học expected/failed, compose message | UseCases (interactor mới, giữ nguyên `IAutoColumnDimensionInteractor`) |
| `Features/AutoColumnDimension/Implements/AutoColumnDimension` | vòng lặp per-column + try/catch load-bearing + gọi policy | vòng lặp + catch → interactor UseCases; thi hành 1 plan → executor Infrastructure |
| `Features/AutoColumnDimension/Contexts/ColumnDimensionContext` | plan/non-plan direction, BasisZ-skip, crossed grid lookup, chọn nearest grid | policy thuần ở UseCases trên DTO (Q3); đọc bbox/solids/faces ở lại |
| `Features/ColumnFromCad/Implements/RectangularColumnExtractor` | nhận diện rect: polyline 5 toạ độ / curve-loop 4 cạnh | Domain shape-detection trên `Point3D`; đọc CAD ở lại |
| `Features/ColumnFromCad/Implements/CircularColumnExtractor` | nhận diện tròn: điểm cách đều tâm ±1e-4, skip 4 cạnh | Domain shape-detection; đọc CAD ở lại |
| `Features/ColumnFromCad/Implements/ColumnModelFactory` | quadrant/rotation math, chọn short/long side, midpoint | `RectangularColumnModel` factory method trên `Point3D` (Q9); convert XYZ ở lại |
| `Features/ColumnFromCad/Strategies/RectangularColumnCreationStrategy` | match symbol theo tolerance, round, min-size 1mm, naming `"{w} x {h}{unit}"` | sizing/naming policy thuần; LookupParameter/Duplicate ở lại |
| `Features/ColumnFromCad/Strategies/CircularColumnCreationStrategy` | như trên cho đường kính | như trên |

### Nhóm 4 — Đúng ở Infrastructure (cơ chế, không tách): 44 class

`ColumnDataExtractor`, `ColumnFromCadContext`¹, `ColumnCreationStrategy` (base — NewFamilyInstance/set
param), `ColumnCreationStrategyFactory`, `DimensionCreator`, `GridFinder`², `DimensionTypeProvider`,
3 interface AutoColumnDimension (Revit-typed), 3 interface ColumnFromCad extractor/factory,
`KeygenLicenseValidator` (Q1 — không đụng), Resource ×3, toàn bộ `Revit/*` (25: adapters, converters,
transaction managers, providers, preprocessors, selection filter), `ServiceRegistration`,
`SettingsService`.

¹ Chứa policy nhỏ (lọc tên parameter "Assembly/OmniClass/Material/Category/Type") — ghi nhận, không
tách: bề mặt interface sẽ to hơn phần logic.
² Sau refactor, quyết định "nearest parallel grid" chuyển vào policy qua grid candidates; `GridFinder`
còn lại là mechanism thu thập/map wrapper.

## Plan

Luật thi hành: **một move một commit** · không sửa hành vi kèm theo (bug thấy được thì ghi lại đây,
sửa sau bằng feature run) · test cũ phải sửa mới xanh ⇒ dừng lại hỏi người.

### Cụm 0 — dọn dẹp & baseline

- [x] T0.1 Characterization tests cho `ColumnFromCadInteractor` — 5 test, xanh
      (`UnitTests/UseCases/ColumnFromCad/ColumnFromCadInteractorTests.cs`)
- [x] T0.2 Baseline integration R23: 42/42 xanh
- [ ] T0.3 Xoá `source/Sonny.Application.Features/` (mồ côi, không build). Gate: build
      `Debug R25` + unit tests xanh. Commit: `Refactor: remove orphaned Sonny.Application.Features sources`

### Cụm 1 — ColumnFromCad

- [ ] T1.1 Domain: `RectangularColumnModel.FromCorners(IReadOnlyList<Point3D> corners)` — chuyển quadrant
      math từ `ColumnModelFactory` (giữ nguyên từng nhánh quadrant). Test:
      `UnitTests/Domain/ColumnFromCad/RectangularColumnModelFromCornersTests.cs` — pin 4 quadrant + cạnh
      bằng nhau + đầu vào <4 điểm (ArgumentException).
- [ ] T1.2 Domain: `ColumnShapeDetector` (Entities/ColumnFromCad) — `IsRectangleLoop(curveCount)`,
      `TryDetectCircle(IReadOnlyList<Point3D> points, tolerance)` — chuyển detection từ 2 extractor.
      Test: `UnitTests/Domain/ColumnFromCad/ColumnShapeDetectorTests.cs` — pin ±1e-4, skip-4-cạnh,
      points<3.
- [ ] T1.3 Infrastructure: `ColumnModelFactory` + 2 extractor gọi Domain logic, xoá logic trùng tại chỗ.
      Gate: `ColumnFromCadIntegrationTest` R23 xanh (45 cột, 6 angle exact).
- [ ] T1.4 Domain: `ColumnSymbolSizingPolicy` — match-tolerance/round/min-1mm/naming từ 2 strategy.
      Test: `UnitTests/Domain/ColumnFromCad/ColumnSymbolSizingPolicyTests.cs`. Infrastructure strategies
      gọi policy. Gate: integration R23 xanh.

### Cụm 2 — AutoColumnDimension

- [ ] T2.1 UseCases: DTOs + `ColumnDimensionPolicy` (AutoColumnDimension/) — `ColumnGeometryData`,
      `ViewGeometryData`, `GridCandidate`, `DimensionAxisPlan`; policy giữ nguyên crossed lookup +
      BasisZ-skip + nearest-parallel-grid. Test:
      `UnitTests/UseCases/AutoColumnDimension/ColumnDimensionPolicyTests.cs` — pin chiều crossed (dir1
      lookup bằng dir2), plan/non-plan, BasisZ-skip, không grid song song → null. ⚠️ nhánh non-plan là
      đoạn đi không lưới cũ — test mới pin theo code hiện tại, đọc kỹ trước khi chép.
- [ ] T2.2 UseCases: `AutoColumnDimensionInteractor` mới (implement `IAutoColumnDimensionInteractor`
      sẵn có) — validate/MessageNoColumnsFound, transaction 1 lần quanh vòng lặp, vòng lặp per-column
      **kèm try/catch load-bearing**, số học ShowResult. Ports mới (UseCases/AutoColumnDimension/Services):
      `IColumnGeometryReader` (active view → DTOs), `IDimensionPlanExecutor` (1 plan → tạo dimension,
      trả số dimension tạo được). Test:
      `UnitTests/UseCases/AutoColumnDimension/AutoColumnDimensionInteractorTests.cs` — pin
      MessageNoColumnsFound (không mở transaction), failed=arithmetic, throw-1-cột-vẫn-chạy-tiếp,
      commit sau vòng lặp.
- [ ] T2.3 Infrastructure: implement 2 port (reader từ ViewWrapper/ColumnWrapper/GridWrappers; executor
      map GridUniqueId→wrapper, gọi `DimensionCreator`); xoá `AutoColumnDimensionInteractor` +
      `AutoColumnDimension` + `ColumnDimensionContext` cũ; cập nhật `ServiceRegistration` 2 layer
      (lifetime: interactor mới singleton như cũ — chỉ đọc qua ports). Gate:
      `AutoColumnDimensionIntegrationTest` R23 xanh (80 dims) + toàn bộ unit tests.

### Cụm 3 — chốt

- [ ] T3.1 Chạy verify đầy đủ: unit R25 + full R23 (42+ test).
- [ ] T3.2 sync-docs: quét identifier đã xoá/di chuyển trong `docs/**`, `CLAUDE.md`, `CONTEXT.md`
      (AutoColumnDimension.md, ColumnFromCad.md, command-flow.md — danh sách singleton + deviation note,
      CLAUDE.md — đoạn "AutoColumnDimensionInteractor lives in Infrastructure", adr/README.md ví dụ);
      cập nhật diagram theo code mới; `graphify update .`. Xoá `## Decisions`/`## Baseline`/`## Plan`
      khỏi ADR này, để lại bản ghi quyết định.

Ghi chú bug thấy trong lúc đọc (KHÔNG sửa trong refactor này):
- `AutoColumnDimensionViewModel.Run` nhân snapDistance với view scale — chưa được Contract của
  feature doc nhắc tới (doc thiếu hoặc code thừa; cần người phân xử, xử lý ở một feature run).
