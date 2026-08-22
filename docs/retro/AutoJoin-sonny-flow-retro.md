# Retro — chạy sonny-flow cho feature AutoJoin (2026-08-22)

Ghi chú trong lúc chạy trọn flow `/sonny-flow:feature AutoJoin` (port từ AlphaBIM), để anh và tôi review
lại xem cái gì đáng bổ sung vào skill. Mỗi mục: chuyện gì xảy ra → đề xuất.

## Những chỗ flow chạy tốt

- **Feature doc là state** hoạt động đúng như thiết kế: resume sau grill bằng cách đọc bảng Decisions,
  không hỏi lại câu nào.
- **Grill trước spec** bắt được 3 quirk thật của code gốc (nhánh chết `is Floor`, mapping seed lạ,
  fallback NewRule = Architectural Column) mà nếu đi thẳng vào code sẽ "sửa nhầm thành bug".
- **Bảng Named failure modes → test 1:1** rất hiệu quả: 16 test interactor viết thẳng từ F3–F11 không
  cần đọc lại code vừa viết.

## Những chỗ nên cải thiện skill

### 1. Orient nên có mục "API surface của service dùng chung"

Chuyện xảy ra: đến bước implement mới phát hiện `ITransactionManager` **không có `RollBack()`** (class có,
interface không) và `IProgressReporter` **không có Cancel** — cả hai đều phải mở rộng abstraction giữa
chừng. Plan đã ghi chú trước ở task 7 nhưng chỉ vì may mắn để ý sớm.

Đề xuất: checklist G0 của `orient` thêm câu 5: *"Feature cần thao tác gì trên các service dùng chung
(transaction, progress, message…), và interface hiện tại có đủ chưa?"* — thiếu thì thành một dòng
Decisions ngay từ grill.

### 2. Quy tắc D8 (đưa method vào Sonny.RevitExtensions) cần một bước "check Nice3point trước"

Chuyện xảy ra: tôi thêm `ToElementId(long)` vào `Sonny.RevitExtensions` → **đụng ambiguous** với
`Nice3point.Revit.Extensions.ElementIdExtensions.ToElementId` (package đã reference sẵn, global using).
Phải xoá đi.

Đề xuất: ghi vào CLAUDE.md của submodule (hoặc mục Knowledge graphs): *trước khi thêm extension mới vào
Sonny.RevitExtensions, grep Nice3point.Revit.Extensions xem đã có chưa* — nó là dependency nền của
submodule này.

### 3. Verify với Sonny.Application.Tests cần ghi rõ kỳ vọng thời gian + điều kiện

Chuyện xảy ra: gate Revit launch cả một process Revit 2023, timeout adapter 10 phút, và pass chỉ có nghĩa
khi Revit load binary mới. `verify.md` (0.4.0) đã nói kỹ phần deploy/stale-binding — tốt. Nhưng CLAUDE.md
project chưa nói **feature không có integration test thì gate Revit chỉ chứng minh "không hồi quy feature
cũ"**, dễ hiểu nhầm là AutoJoin đã được kiểm trong Revit.

Đề xuất: verify.md thêm một câu: khi feature mới không có test trong Sonny.Application.Tests, báo cáo G5
phải tách hai ý — *bộ cũ pass (không hồi quy)* và *hành vi mới trong Revit: chưa kiểm bằng máy*.

### 4. Presentation ViewModel là vùng trắng test có hệ thống

`Sonny.Application.UnitTests` chỉ reference Domain + UseCases + ResourceManager, còn Presentation kéo
`Autodesk.Windows` (qua `DialogExtension`) nên không nhét vào được. Logic thuần trong AutoJoinViewModel
(mutual exclusion 2 chế độ cut, thứ tự seed > settings, quirk NewRule fallback) đều không có autotest.

Đề xuất dài hạn: tách phần quyết định của VM thành class thuần (như ADR 0001 đã làm với interactor) hoặc
tách `DialogExtension` ra để Presentation test được. Đáng một ADR riêng nếu làm.

### 5. Hook graphify nhắc "MANDATORY" hơi ồn khi đang implement

Sau khi đã orient bằng graphify + codegraph, mọi Read/Grep/Bash vẫn bị nhắc "MUST run graphify first".
Trong bước implement (đọc file để sửa từng dòng) nhắc này không còn giá trị.

Đề xuất: hook nên im trong N phút sau lần `graphify query`/`codegraph` gần nhất, hoặc chỉ nhắc với file
chưa từng được đọc trong session.

### 6. Grill round cuối: `grill.md` vs `feature.md` mâu thuẫn nhẹ

`grill.md` bảo *"chờ người xác nhận đã hiểu nhau"* sau khi frontier rỗng; `feature.md` bảo *"frontier
rỗng thì không dừng — đi tiếp luôn"*. Tôi theo feature.md (đúng cho chế độ chạy cả flow), nhưng hai file
nên nói cùng một câu để agent không phải tự phân xử.

### 7. Ý mới phát sinh đáng ghi lại (không phải lỗi flow)

- Seed từ selection có **một deviation nhỏ so với bản gốc**: category không map được → bản gốc tạo rule
  với tên rỗng (và vì id mặc định = 0 nên hành xử như Architectural Floor — gần chắc là bug); bản Sonny
  để bảng rỗng. Đã ghi trong Behaviour của feature doc.
- Icon ribbon đang là placeholder copy từ AutoColumnDimension — cần icon thật.
- `OST_TopographySurface` nằm trong list category port từ AlphaBIM — compile pass R21–R26 hiện tại,
  nhưng nếu Autodesk bỏ enum này ở version mới thì `ElementCategoryFilters` là chỗ vỡ đầu tiên.

### 8. Bài học đắt giá từ việc tự dựng fixture Revit bằng test (phần mở rộng sau flow)

Việc "điều khiển Revit vẽ fixture rồi autotest trên đó" thành công (`AutoJoinFixtureBuilder` +
`AutoJoinIntegrationTest`, 16/16 pass) nhưng tốn ~12 vòng chạy Revit để dò ra các bẫy môi trường.
Đáng đưa vào skill/doc để lần sau không trả giá lại:

- **Callback managed định nghĩa trong test assembly là thuốc độc dưới host ricaun**: `IFailuresPreprocessor`
  gắn vào `Transaction.Commit(options)` làm MỌI commit trả `RolledBack` không một failure message;
  `IFamilyLoadOptions` làm `LoadFamily` trả false. Nguyên nhân khả dĩ: assembly bị shadow-copy nên
  callback native→managed resolve fail. Quy tắc: trong `Sonny.Application.Tests`, commit trần
  (`Commit()`), load family in-memory không options; preprocessor chỉ dùng loại đã đăng ký trong
  assembly Sonny thật (qua `ITransactionManagerFactory`).
- **Document phải được mở ở `OnSetup` (event API riêng)** — mở bằng `OpenAndActivateDocument` rồi commit
  ngay trong cùng test method → commit rollback ("attempt to modify wrong element during regeneration").
  Khuôn `SonnyDocumentTestBase` không chỉ là tiện nghi, nó là điều kiện đúng đắn.
- **`SonnyDocumentTestBase.OnSetup` đọc property `DocumentFilePath` hai lần** — getter có side effect
  (copy file) phải idempotent/cached, không thì lần đọc thứ hai đổi quyết định.
- **Trust "Always Load" cho add-in unsigned theo hash DLL** — mỗi lần rebuild test project là dialog
  quay lại. Watcher UIA click được, nhưng phải thoát ngay sau click; để nó poll trong lúc test chạy
  từng bị nghi oan là nguồn rollback.
- **Fixture sinh từ template kết cấu ẩn category kiến trúc ở view mới** (arch columns, ceilings, roofs)
  → mọi collector view-scoped lặng lẽ trả rỗng. Builder phải `SetCategoryHidden(false)` tường minh.
- **Hai kẻ cắt chồng vùng cắt trên cùng element**: Revit gỡ join sau tại commit ("joined but do not
  intersect") — không log, không failing id. Khi dựng case, mỗi kẻ cắt một vùng tách biệt.
- Pattern tổng quát đáng vào skill: builder-test tự vẽ fixture + tự verify hình học từng case trước khi
  save + tag element bằng Comments; fixture 2023 để version cao hơn mở được; builder tự `Ignore` khi
  fixture tồn tại nên nằm chung suite vô hại.

### 9. Đề xuất đưa vào chính skill: retro là output bắt buộc của mỗi lần chạy flow

Mỗi lần chạy xong `/sonny-flow:*` cho một feature, bước cuối (doc) nên **bắt buộc tạo/cập nhật một file
retro** tại vị trí cố định `docs/retro/<Feature>-sonny-flow-retro.md`, nội dung tối thiểu: (a) chỗ nào
flow chạy mượt, (b) chỗ nào phải tự xoay ngoài kịch bản — kèm đề xuất sửa skill cụ thể (sửa file nào,
thêm câu gì), (c) bài học kỹ thuật trả giá bằng nhiều vòng chạy. File này chính là backlog để nâng cấp
skill; không có nó thì bài học chết theo phiên chat. (File này của AutoJoin là ví dụ mẫu.)

### 10. Quy tắc làm việc mới (feedback từ chủ dự án, 2026-08-22)

- **Mọi việc đụng đến family (tạo family, load family, sửa family) phải hỏi và được xác nhận trước**,
  kể cả trong fixture test. Lần này builder tự chế family hộp từ template vì máy không có thư viện
  family và placeholder trống — chấp nhận giữ, nhưng là ngoại lệ, không phải tiền lệ.
