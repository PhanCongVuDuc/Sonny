# Revit-free unit tests live in `Sonny.Application.UnitTests`, not in `Sonny.Application.Tests`

**BẢN NHÁP — refactor đang chạy.** `## Decisions

Chốt ở vòng grill 1 (người dùng quyết định):

| # | Quyết định | Lý do |
|---|---|---|
| D1 | **Chỉ 3 file `ResourceManager/UnitTests` được move.** Không thêm `ProjectReference` nào vào `Sonny.Application.UnitTests` | Giữ nguyên tính chất Revit-free mà comment trong `Sonny.Application.UnitTests.csproj` đang khẳng định. Kéo `Infrastructure` vào là kéo `Nice3point.Revit.Api.RevitAPI` vào |
| D2 | **`SettingsServiceLanguageTests` bị xoá**, không move | Nó ghi và xoá state toàn cục của máy (`%APPDATA%\Sonny\SonnySettings.json`) vì `SettingsService` hard-code đường dẫn, không có seam để cô lập. Phần lớn assert chỉ kiểm tra getter/setter. Ghi lại thành gap trong `test-safety-net.md` |
| D3 | **`UnitConverterTests` ở lại `Sonny.Application.Tests`**, chuyển từ `Core/UnitTests/Services/` sang `Core/RevitApiTests/` | `UnitConverter` gọi `ForgeTypeId`/`UnitTypeId`/`UnitUtils` — nó thật sự cần Revit API. Tên thư mục cũ nói sai sự thật. Không đụng vào code sản phẩm |
| D4 | **Assert viết lại theo constraint model `Assert.That(x, Is.EqualTo(y))`** | `Sonny.Application.UnitTests` dùng NUnit 4.2.2 + `NUnit.Analyzers`; classic assert đã dời sang `NUnit.Framework.Legacy.ClassicAssert`. Constraint model khớp phong cách 6 file test đã có ở đó, không để lại nợ legacy |
| D5 | **`Tests/Utils/EnumHelper` được copy sang `UnitTests/Utils/`, bản cũ bị xoá** *(quyết định thường lệ, không hỏi)* | Sau D1 + D2 không còn ai trong `Sonny.Application.Tests` dùng nó. `Sonny.Application.UnitTests` build cả net48 (R21–R24) nên vẫn cần `#if NETCOREAPP` của helper, không thay bằng `Enum.GetValues<T>()` được |

D4 là **retarget wiring, không phải đổi assertion**: mỗi `Assert.AreEqual(a, b)` thành `Assert.That(b, Is.EqualTo(a))`, `Assert.IsTrue(x)` thành `Assert.That(x, Is.True)`, `Assert.IsNull/IsNotNull` thành `Is.Null`/`Is.Not.Null`. Giá trị kỳ vọng không đổi một chữ. Đây đúng loại "test không còn compile vì bám vào chi tiết implementation" mà `test-safety-net.md` cho phép, với điều kiện ghi vào ADR — dòng này là bản ghi đó.

## Lưới an toàn (baseline — bước 2)

Refactor này **không chạm code sản phẩm**. Thứ phải giữ nguyên là: cùng một tập assert, cùng kết quả.

| Cái gì | Lưới an toàn | Trạng thái baseline |
|---|---|---|
| 6 file test sẵn có trong `Sonny.Application.UnitTests` | Chính chúng | ✅ 46 passed / 0 failed, 530 ms (`dotnet test … -c "Debug R25"`) |
| 3 file sắp move | Chính chúng, chạy ở nhà cũ trước khi move | ❌ **KHÔNG KIỂM CHỨNG ĐƯỢC** — xem bên dưới |
| `UnitConverterTests` (chỉ đổi namespace) | Chính nó | ❌ **KHÔNG KIỂM CHỨNG ĐƯỢC** — xem bên dưới |
| `SettingsServiceLanguageTests` (sắp xoá) | — | Cố tình đi **không có lưới**: đây là xoá, không phải move. Hành vi bị mất là chủ đích của D2 |
| `AutoColumnDimensionIntegrationTest`, `ColumnFromCadIntegrationTest` | Không bị chạm — refactor không đụng file nào chúng bind vào | Không cần chạy lại |

Không có characterization test nào phải viết mới: mọi thứ bị chạm **đã là** test.

### Baseline trong Revit host: thất bại

```
dotnet test source/Sonny.Application.Tests/... -c "Debug R25"   --filter "FullyQualifiedName~Sonny.Application.Tests.ResourceManager.UnitTests|FullyQualifiedName~Sonny.Application.Tests.Core.UnitTests"

Failed! - Failed: 40, Passed: 0, Skipped: 0, Total: 40
Mỗi test: "RevitTest: Timeout 10 minutes."
```

Cả 40 test đỏ với **cùng một** lý do: `ricaun.RevitTest.TestAdapter` chờ Revit 2025 khởi động 10 phút rồi
bỏ cuộc. Không còn process `Revit.exe` nào sau khi chạy. Đây **không phải** hành vi của code — không một
assert nào được thực thi.

Hệ quả cho refactor:

- Baseline cho 3 file sắp move: **không kiểm chứng được**. Chúng được move dựa trên việc đọc code — cả ba
  chỉ chạm `Sonny.ResourceManager`, `Domain.Entities.Settings`, `UseCases.Settings.Models`, không có một
  API Revit nào.
- Bù lại, đúng nhà mới thì chúng chạy được: `Sonny.Application.UnitTests` xanh trong 530 ms mà không cần
  Revit. **Kết quả baseline này chính là bằng chứng mạnh nhất cho D1** — ở nhà cũ, ba test Revit-free đang
  bị bắt làm con tin bởi một lần khởi động Revit, và trên máy này chúng thực tế **không chạy được**.
- `UnitConverterTests` (task 7, chỉ đổi namespace): cũng không kiểm chứng được ở đây. Đổi namespace là
  thay đổi mà compiler kiểm hộ; nhưng gate ở bước 8 phải ghi rõ là **không kiểm chứng được**, không được
  ghi là pass.

## Plan

Một move một commit. Mỗi bước tự đứng được: sau bước nào cả hai project cũng phải build.

- [x] **1 — Thêm `EnumHelper` vào project Revit-free.** Tạo `source/Sonny.Application.UnitTests/Utils/EnumHelper.cs`, namespace `Sonny.Application.UnitTests.Utils`, nội dung y hệt bản trong `Sonny.Application.Tests/Utils/EnumHelper.cs` (giữ nguyên `#if NETCOREAPP`). Chưa xoá bản cũ. Không test mới — helper được 2 test ở bước 3 và 4 dùng. *(D5)*
- [x] **2 — Move `CultureChangedEventArgsTests`.** `Sonny.Application.Tests/ResourceManager/UnitTests/` → `Sonny.Application.UnitTests/ResourceManager/`, namespace `Sonny.Application.UnitTests.ResourceManager`. Đổi 9 assert sang `Assert.That`. Xoá file cũ trong cùng commit. *(D1, D4)*
- [x] **3 — Move `LanguageCodeExtensionsTests`.** Cùng thư mục đích, namespace `Sonny.Application.UnitTests.ResourceManager`, `using Sonny.Application.UnitTests.Utils` cho `EnumHelper`. Đổi ~30 assert sang `Assert.That`. Xoá file cũ. *(D1, D4, D5)*
- [x] **4 — Move `LanguageOptionTests`.** → `Sonny.Application.UnitTests/UseCases/Settings/`, namespace `Sonny.Application.UnitTests.UseCases.Settings` (khớp quy ước thư mục theo layer đang có: `Domain/ColumnFromCad`, `UseCases/AutoColumnDimension`). Đổi ~14 assert. Xoá file cũ. *(D1, D4, D5)*
- [ ] **5 — Xoá `Core/UnitTests/Services/SettingsServiceLanguageTests.cs`.** Không thay thế. *(D2)*
- [ ] **6 — Xoá `Sonny.Application.Tests/Utils/EnumHelper.cs`.** Sau bước 3–5 không còn ai trong project đó dùng — kiểm chứng bằng `grep -rn EnumHelper source/Sonny.Application.Tests`. *(D5)*
- [ ] **7 — Đổi chỗ `UnitConverterTests`.** `Core/UnitTests/Services/` → `Core/RevitApiTests/`, namespace `Sonny.Application.Tests.Core.RevitApiTests`. **Không đổi một assert nào** — file này ở lại NUnit 3, `Assert.AreEqual` vẫn hợp lệ. Thư mục `Core/UnitTests` trống và biến mất. *(D3)*
- [ ] **8 — Gate (bước 5).** `dotnet test source/Sonny.Application.UnitTests/... -c "Debug R25"` phải ra **46 + 3 file mới**; `dotnet test source/Sonny.Application.Tests/... -c "Debug R25" --filter "FullyQualifiedName~Core.RevitApiTests"` phải xanh. So sánh từng tên test với baseline: tập test chỉ được **mất đúng những test của `SettingsServiceLanguageTests`**, không mất gì khác.
- [ ] **9 — sync-docs (bước 6).** `CLAUDE.md:47` (câu khẳng định `Core/UnitTests` và `ResourceManager/UnitTests` không cần Revit — sau refactor sai hai lần); `docs/architecture/test-safety-net.md` (thêm dòng inventory cho `UnitConverterTests`, ghi gap do D2 để lại); kiểm identifier bị xoá còn sót trong `docs/**`, `CLAUDE.md`, `CONTEXT.md`; xoá `## Decisions` và `## Plan` khỏi ADR này.
