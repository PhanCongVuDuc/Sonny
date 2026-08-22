# Revit test environment — máy này, host ricaun, và các bẫy đã trả giá

Đọc file này **trước khi chạy hoặc viết bất kỳ test nào trong `Sonny.Application.Tests`**. Mọi điều ở
đây đều được trả giá bằng nhiều vòng chạy Revit thật khi dựng bộ integration test AutoJoin (2026-08-22);
không có điều nào suy ra được từ code.

## Chạy test thế nào

**Mọi lần chạy `Sonny.Application.Tests` đi qua một cửa duy nhất: `scripts/loop.ps1`** (chính là
`loopCommand` khai trong `CLAUDE.md`). Nó build, lo dialog trust, chạy test, và trả verdict theo luật
`rules/revit-loop.md` của sonny-flow: exit **0** = GREEN · **1** = RED/build hỏng · **2** = không thấy
test nào (không phải pass!).

```powershell
powershell -ExecutionPolicy Bypass -File scripts\loop.ps1                     # vòng lặp dev: giữ Revit mở
powershell -ExecutionPolicy Bypass -File scripts\loop.ps1 -Filter AutoJoin    # lọc theo tên
powershell -ExecutionPolicy Bypass -File scripts\loop.ps1 -Final              # chạy chốt: mở Revit mới, đóng khi xong
```

Chế độ mặc định (dev) build với `-p:RevitTestKeepOpen=true` → DLL test mang metadata
`NUnit.Open=false / NUnit.Close=false` → ricaun **tái dùng Revit đang mở** và nạp DLL test mới nhờ
shadow-copy; target `RepackForRevitReload` merge các assembly repo (+ Nice3point) vào DLL test nên
**code sản phẩm cũng reload theo** — đã xác nhận 5 vòng sửa-code-chạy-lại trong một Revit (2026-08-22),
mỗi vòng ~30 giây. Không ai sửa tay csproj; hết cờ là trở về mặc định mở-đóng sạch.

Bốn điều kiện bắt buộc của chế độ attach (vỡ cái nào là đỏ cái đó — chi tiết trong
`rules/revit-loop.md` của sonny-flow): build dev phải kèm `-p:DeployRevitAddin=false` (Revit khoá
Addins) · merge cả `Nice3point.Revit.*` (add-in khác ship bản cũ → MissingMethodException) · attach
KHÔNG copy `Resources\` nên helper fixture có fallback `[CallerFilePath]` về source · **cấm NSubstitute
trong test chạy Revit** — Castle proxy trúng bản assembly nạp đầu → InvalidCastException; dùng fake
tay trong `TestDoubles.cs`.

Một hook PreToolUse (`.claude/hooks/revit-test-guard.ps1`) chặn lệnh `dotnet test` gõ thẳng vào
`Sonny.Application.Tests` và chỉ về `loop.ps1` — thoát hiểm bằng biến môi trường `SONNY_DIRECT_TEST=1`
khi thật sự cần chạy trần (CI).

## Trust "Always Load" — theo HASH của DLL

Revit trust add-in unsigned theo hash: **mỗi lần rebuild add-in là dialog quay lại** ở cold start, và
một run headless sẽ treo tới timeout 10 phút. `scripts/Watch-AlwaysLoad.ps1` tự click nút (PostMessage
vào HWND của nút — UIA InvokePattern không có, click chuột vật lý fail khi khoá màn hình) rồi **thoát
ngay** — để nó poll UIA trong lúc test chạy chỉ tổ nhiễu. `loop.ps1` tự khởi động watcher khi cold start.

## Bốn quy tắc vàng khi viết test/builder chạy trong Revit

1. **Cấm đưa cho Revit callback định nghĩa trong test assembly.** `IFailuresPreprocessor` gắn vào
   `Transaction.Commit(options)` làm MỌI commit trả `RolledBack` không một failure message;
   `IFamilyLoadOptions` làm `LoadFamily` trả false. Nguyên nhân: DLL test bị ricaun shadow-copy nên
   callback native→managed resolve fail, lỗi bị nuốt. Thay thế: `Commit()` trần; load family
   **in-memory** (`familyDocument.LoadFamily(document, options)` — riêng đường này chạy được);
   preprocessor chỉ dùng loại đã có trong assembly Sonny thật (qua `ITransactionManagerFactory`).
2. **Document phải được mở ở `OnSetup`** (một sự kiện API riêng), không mở-rồi-commit trong cùng test
   method — Revit chưa dọn xong trạng thái post-open thì mọi commit bị hủy ngầm ("attempt to modify
   wrong element during regeneration"). Khuôn `SonnyDocumentTestBase` là điều kiện đúng đắn, không phải
   tiện nghi.
3. **`DocumentFilePath` bị base class đọc HAI lần** — getter có side effect (vd copy file) phải cache
   (`??=`), không thì lần đọc thứ hai rẽ nhánh khác và mở nhầm file.
4. **File nền sinh từ template kết cấu ẩn category kiến trúc ở view mới** (Columns, Ceilings, Roofs)
   → mọi collector view-scoped lặng lẽ trả rỗng. Builder fixture phải `SetCategoryHidden(false)`
   tường minh cho từng category dùng đến.

Và một hành vi Revit cần biết khi dựng case join: **hai kẻ cắt chồng vùng cắt trên cùng một element →
Revit lặng lẽ gỡ join sau tại commit** ("joined but do not intersect"), không log, không failing id —
xem [AJ-001](../bugs/AJ-001-overlapping-cut-regions-silently-unjoined.md).

## Fixture tự sinh

`Test_V2023_AutoJoin.rvt` là fixture **generated**: xóa file rồi chạy
`AutoJoinFixtureBuilder` (Debug R23) để vẽ lại. Pattern đầy đủ (station cách ly, tag Comments,
self-verify hình học trước khi save, fixture ở version Revit thấp nhất) nằm trong rule
`revit-fixture.md` của skill sonny-flow. **Luật cứng: mọi việc tạo/load/sửa family phải hỏi chủ dự án
trước** — kể cả family tối giản cho fixture.

## Máy này có gì / thiếu gì

- Revit 2021–2026 đã cài; test Revit thật chạy trên **2023** (`Debug R23`).
- **Không có thư viện family chuẩn** (`C:\ProgramData\Autodesk\RVT 2023\Libraries` chỉ có bộ Precast —
  không có M_Concrete-Rectangular*). Family templates (`.rft`) thì đầy đủ.
- Journal của Revit (`%LOCALAPPDATA%\Autodesk\Revit\Autodesk Revit 2023\Journals`) là nơi chẩn đoán khi
  test treo/timeout — dialog đang chặn được ghi ở đó (`TaskDialog "..."`).
- Log Serilog của add-in: `%LOCALAPPDATA%\Sonny\Logs\sonny-*.log`.

## `scripts/` — vô hình với knowledge graph

Cả graphify lẫn codegraph **không index file `.ps1`**, nên đừng trông chờ hai graph dẫn tới thư mục này:

| Script | Việc |
|---|---|
| `loop.ps1` | Cửa duy nhất chạy test Revit — build + trust + test + verdict 0/1/2 |
| `Watch-AlwaysLoad.ps1` | Tự click dialog trust, thoát sau cú click đầu |
| `Deploy-SonnyAddin.ps1` | Build + deploy add-in cho (các) năm Revit — từ chối khi Revit đang mở |
| `Install-SonnyAddinManifests.ps1` | Đảm bảo manifest .addin cho các bản đã deploy |
