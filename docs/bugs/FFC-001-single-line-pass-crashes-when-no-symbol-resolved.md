# FFC-001 — Bật "Create for Single Line" mà không có dầm cặp nào: lệnh crash và mất sạch

| | |
|---|---|
| **Status** | open — **giữ nguyên có chủ ý** (quyết định #6 khi port, 2026-08-23) |
| **Feature** | [FramingFromCad](../features/FramingFromCad.md) |
| **Affects** | Sonny |
| **Phát hiện** | 2026-08-23, khi đọc `FramingFromCadProcess.xaml.cs` để port |

## Hiện tượng

Tick **Create for Single Line** rồi chạy trên một bản vẽ mà pass dầm cặp không dựng được dầm nào —
ví dụ CAD chỉ có nét đơn, hoặc ô tiết diện gõ sai hết, hoặc không cặp nét nào cách nhau đúng bề rộng.
Lệnh **văng exception**, Revit hiện dialog lỗi, và **không dầm nào được tạo** kể cả những nét lẻ mà
lẽ ra tool phải dựng được.

## Tái hiện (~1 phút)

1. Một bản vẽ CAD chỉ có 2 nét thẳng rời, không cặp nào cách nhau đúng bề rộng dầm sẽ khai.
2. Chạy **Framing from CAD**, chọn layer đó, gõ `200x300`, tick **Create for Single Line**, Run.
3. Pass dầm cặp không tạo được gì → pass nét lẻ không có symbol nào để dùng → exception.

## Bản chất

Nét lẻ không mang tiết diện: chúng dùng **symbol đầu tiên mà pass dầm cặp đã resolve**. Bản gốc giữ
symbol đó trong `_firstFamilySymbol`; nếu pass dầm cặp không chạy đến đó thì biến vẫn `null`, và block
dựng nét lẻ dereference nó ở chỗ đặt tiêu đề progress:

```csharp
MainWindow.Title = string.Concat("Model Beams from CAD for single line with section ",
    _firstFamilySymbol.Name, ...)   // NullReferenceException
```

Block này **không có try/catch**, khác block dầm cặp ngay trên nó. Exception thoát khỏi `Process()`,
lên `FramingFromCadCmd` — nơi `catch (Exception e) { Console.WriteLine(e); throw; }` ném lại — nên
`TransactionGroup` bị dispose mà không `Assimilate()`, rollback toàn bộ.

Bản Sonny giữ đúng hành vi này, chỉ đổi exception thành một `InvalidOperationException` có thông điệp
(`MessageNoSymbolResolvedForSingleLines`) thay cho một NRE trần — vẫn thoát ra, vẫn rollback. Xem
`BeamCreator.CreateSingleLineBeam` và vòng lặp nét lẻ **không catch** trong
`FramingFromCadInteractor.CreateFramings`.

## Vì sao chưa fix

Quyết định lúc port: bản gốc đang chạy tốt trong sản xuất và yêu cầu là "y hệt, đừng để mất
logic". Sửa ở đây là đổi hành vi trong đúng trường hợp bản gốc đang crash, nên nó là *đổi yêu cầu*,
không phải sửa implementation. Được pin bằng test:
`FramingFromCadInteractorTests.Execute_SingleLinesButNoSymbolEverResolved_LetsTheExceptionEscape`.

## Hướng fix đề xuất (chưa làm)

Hai phương án đã trình và bị gạt, ghi lại để lần sau không phải nghĩ lại:

1. **Fallback**: khi chưa có symbol nào, tự resolve symbol cho tiết diện đầu tiên trong ô AllSections
   (duplicate nếu cần) — đúng ý định của cái checkbox, và CAD toàn nét đơn vẫn chạy được.
2. **Bỏ qua có báo**: giữ dầm cặp đã dựng, bỏ phần nét lẻ, báo một message.

Fix = chạy lại feature flow (`/sonny-flow:feature FramingFromCad`): spec chuyển hành vi mới vào
`## Contract` · implement viết test **RED trước** · xong đổi Status ở đây thành `fixed` và gỡ dòng link
khỏi `## Related` của feature doc.
