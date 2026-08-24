# Business decisions live in UseCases/Domain; Infrastructure keeps only Revit mechanism behind interfaces

`AutoColumnDimensionInteractor` played a use-case role from inside Infrastructure, and several
Infrastructure classes mixed business decisions (geometry policy, shape recognition, rotation math,
symbol sizing) with Revit mechanism. We classified all 131 product classes and moved the decision halves
down: interactors and coordination policies to `UseCases`, single-model calculations to `Domain`, so every
decision is unit-testable in `Sonny.Application.UnitTests` (seconds, no Revit). Mechanism stays in
`Infrastructure` behind ports.

The boundary is **plain DTOs, data crossing once** — Infrastructure reads Revit into Revit-free data
(`Point3D`, doubles, `UniqueId` strings, e.g. `ColumnGeometryData`, `GridCandidate`), the decision core
returns a *plan* (e.g. `ColumnDimensionPlan`), and Infrastructure maps ids back to elements and executes.
`Reference`/`PlanarFace`/`XYZ` never leave Infrastructure; tests of decision code need no mocks.
`IColumnGeometryReader` / `IDimensionPlanExecutor` are the reference port pair. Placement rule: a
calculation owned by one model sits on the Domain model (`RectangularColumnModel.FromCorners`,
`ColumnShapeDetector`, `ColumnSymbolSizingPolicy`); a decision coordinating several inputs is a UseCases
service (`ColumnDimensionPolicy`, the interactors).

## Phương án đã loại

- **Interface-hoá wrapper** (`IColumnGeometry`, `IViewInfo`… cho decision core gọi ngược): bề mặt
  interface phình, test phải mock từng property, và lifetime Revit rò vào Domain. DTO một chiều rẻ hơn.
- **Giữ interactor ở Infrastructure, chỉ tách helper thuần**: interactor test được nhưng phần quyết định
  khó nhất (policy hình học) vẫn ngoài lưới — không đạt mục tiêu.
- **Big-bang toàn bộ 131 class trong một đợt**: diff không đọc được, không bisect được. Thay bằng chuỗi
  move nhỏ, mỗi move một commit, gate integration sau mỗi cụm.
- **Move cụm license**: loại theo yêu cầu — chỉ phân loại, không đụng code.

## Consequences

- Phân loại chốt: 79 class đã đúng chỗ (toàn bộ Domain/UseCases/Presentation/App/ResourceManager),
  0 class "chuyển thẳng được", 8 class phải tách (đã tách xong), 44 class là cơ chế đúng chỗ ở
  Infrastructure. `GridFinder`, `ColumnDimensionContext`, `AutoColumnDimension` (executor cũ) và
  Infrastructure `AutoColumnDimensionInteractor` bị xoá sau khi vai của chúng dời chỗ.
- `UseCases` tham chiếu thêm `Serilog` (4.2.0) để giữ nguyên hành vi logging của interactor.
- Toán thuần (normalize/atan2) tái tạo `XYZ.AngleTo` đủ chính xác để integration test so khớp
  rotation angle bằng exact-equality vẫn xanh — được xác nhận trên Revit 2023 thật.
- Hai vi-lệch có chủ đích trên input suy biến, chỉ đổi *kiểu* exception, không đổi kết cục người dùng
  thấy (extraction vẫn fail to tiếng): fit đường tròn qua 3 điểm thẳng hàng ném
  `InvalidOperationException` thay vì `ArgumentsInconsistentException` của Revit; guard "ít hơn 4 curve"
  ném từ factory trước khi chạm Domain.
- `AutoColumnDimensionIntegrationTest` phải retarget phần wiring trong `OnSetup` (nó construct interactor
  bằng tay để tiêm mock `IMessageService`) — mọi assertion giữ nguyên. Đây là ca "test dựa vào chi tiết
  implementation" được flow dự báo.
- Nhánh non-plan view được move **không có lưới integration** (chấp nhận tường minh); policy của nhánh đó
  giờ có unit test pin theo code cũ.
- Ghi nhận ngoài phạm vi (chưa xử lý): `AutoColumnDimensionViewModel.Run` nhân `snapDistance` với view
  scale, điều Contract của feature doc không nhắc — cần một feature run phân xử doc thiếu hay code thừa;
  và `ColumnShapeDetector` kế thừa quirk cũ: loop tessellate ra đúng 3 điểm sẽ ném
  `ArgumentOutOfRangeException` (points[3]).
