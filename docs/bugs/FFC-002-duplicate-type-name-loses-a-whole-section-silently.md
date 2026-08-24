# FFC-002 — Trùng tên type: mất sạch dầm của một tiết diện, không một lời báo

| | |
|---|---|
| **Status** | open — **giữ nguyên có chủ ý** (quyết định #7 khi port, 2026-08-23) |
| **Feature** | [FramingFromCad](../features/FramingFromCad.md) |
| **Affects** | Sonny |
| **Phát hiện** | 2026-08-23, khi đọc `FramingFromCadProcess.xaml.cs` để port |

## Hiện tượng

Chạy tool với một tiết diện mà project **đã có type trùng tên** — ví dụ gõ `200x300` khi trong family
đã có type tên `200x300` nhưng `b`/`h` của nó không đúng 200/300 (ai đó đổi tên tay), hoặc gõ trùng một
tiết diện hai lần (`200x300; 200x300`). **Toàn bộ dầm của tiết diện đó không được tạo**, message cuối
vẫn báo thành công với số lượng của các tiết diện còn lại. Không có cảnh báo, không có log.

## Tái hiện (~2 phút)

1. Trong family dầm, đổi tên một type thành `200x300` nhưng để `b`=250, `h`=500.
2. Chạy **Framing from CAD** trên bản vẽ có các cặp nét cách nhau 200 mm, gõ `200x300`, Run.
3. Message báo "đã tạo 0 dầm" (hoặc chỉ số của tiết diện khác) — không nói vì sao.

Biến thể nhanh hơn: gõ `200x300; 200x300`. Tiết diện thứ hai mất sạch.

## Bản chất

Danh sách symbol để dò khớp được lấy **một lần, trước khi duplicate bất cứ gì** (`allFamilySymbol` ở đầu
`Process()`). Nên:

- Dò `b`/`h` trong danh sách cũ → không khớp (type kia có b/h khác, hoặc type mới vừa tạo không nằm
  trong danh sách đã chụp).
- Rơi vào nhánh `allFamilySymbol[0].Duplicate(name)` với `name = "200x300"`.
- Revit **throw** vì tên đã tồn tại.
- Exception bị `catch (Exception e) { }` trần của vòng lặp dầm cặp ăn mất → dầm bị bỏ, chạy tiếp; và vì
  `familySymbol` vẫn `null` nên **mọi cặp còn lại của tiết diện đó** cũng đi đúng đường đó.

Bản Sonny giữ nguyên: `BeamCreator.ResolveSymbol` chụp `_symbolsAtRunStart` một lần và để `Duplicate`
throw; `FramingFromCadInteractor.CreateFramings` có đúng cái `catch (Exception)` im lặng đó. Pin bằng
test `FramingFromCadInteractorTests.Execute_OnePairThrows_SkipsItSilentlyAndKeepsTheRest`.

## Vì sao chưa fix

Quyết định lúc port: giữ y hệt tool đang chạy tốt. Đây cũng là *hai* silent skip lồng nhau (throw của
`Duplicate` + `catch` trần), nên sửa nó là đổi Contract chứ không phải sửa lỗi implementation.

## Hướng fix đề xuất (chưa làm)

Hai phương án đã trình và bị gạt:

1. **Dùng lại type trùng tên**: tìm type đúng tên trước khi Duplicate, có thì set `b`/`h` cho nó rồi
   dùng. Đánh đổi: ghi đè kích thước một type người dùng đã có, đổi cả những dầm cũ đang dùng type đó.
2. **Đệm hậu tố**: `"200x300 (2)"`. Đánh đổi: project sinh type tên lạ, hai type cùng tiết diện.

Rẻ hơn cả hai và không đổi hành vi dựng dầm: **báo cho người dùng** — đếm số tiết diện bị mất vì lý do
tên và hiện một warning ở cuối, giữ nguyên phần còn lại. Cái làm bug này đắt không phải việc mất dầm mà
là việc mất im lặng.

Fix = chạy lại feature flow (`/sonny-flow:feature FramingFromCad`): spec chuyển hành vi mới vào
`## Contract` · implement viết test **RED trước** · xong đổi Status ở đây thành `fixed` và gỡ dòng link
khỏi `## Related` của feature doc.
