# FFC-003 — Bật "Create for Single Line" làm một tiết diện ăn mất dầm của tiết diện sau, im lặng

| | |
|---|---|
| **Status** | open |
| **Feature** | [FramingFromCad](../features/FramingFromCad.md) |
| **Affects** | Sonny |
| **Phát hiện** | 2026-08-23, khi dựng integration test trên DWG thật của chủ dự án |

## Hiện tượng

Tick **Create for Single Line** rồi chạy với nhiều tiết diện: một tiết diện đứng trước có thể **ăn hết nét
CAD mà tiết diện đứng sau cần**, làm tiết diện sau không dựng được dầm nào. Không có cảnh báo, không có log;
message cuối vẫn báo thành công với số lượng của những tiết diện còn lại.

Hệ quả nặng hơn: **thứ tự gõ trong ô AllSections là một phần của input** — cùng bộ tiết diện, đổi thứ tự cho
ra số dầm khác. UI không hề nói điều đó.

## Đo được trên fixture

`Test_V2023_FramingFromCad.rvt` (DWG thật, layer `S-BEAM`, 168 nét), tiết diện
`200x300; 300x600; 350x700; 400x800`:

| Chế độ | 200 | 300 | 350 | 400 | Tổng |
|---|---|---|---|---|---|
| `IsCreateForSingleLine` **off** | 4 | 30 | **3** | 6 | **43** |
| `IsCreateForSingleLine` **on** | 4 | 30 | **0** | 6 | **40** |

Toàn bộ chênh lệch là tiết diện `350x700`: tiết diện `300x600` chạy trước đã tiêu thụ những nét mà 350 cần.
Gõ `350x700` trước `300x600` sẽ cho kết quả khác.

Pin bằng test `FramingFromCadIntegrationTest.FramingFromCad_ExtractsPairsFromTheRealDrawing`, có một assertion
riêng khẳng định *toàn bộ* chênh lệch 43 → 40 phải đúng bằng tiết diện 350 — nên nếu ai sửa hành vi này, test
đỏ ở đúng chỗ có ý nghĩa.

## Bản chất

`FramingDataExtractor.Extract` xoá các nét đã ghép cặp khỏi pool **ngay trong vòng lặp tiết diện**, và chỉ khi
`IsCreateForSingleLine` bật:

```csharp
if (settings.IsCreateForSingleLine) {
    foreach (var stroke in groups.SelectMany(group => group)) {
        strokes.Remove(stroke) ;   // pool teo lại cho MỌI tiết diện sau
    }
}
```

Bản gốc y hệt (`_allLinesFraming = _allLinesFraming.Except(...)` trong vòng lặp section, cũng chỉ khi
`IsCreateForSingleLine`).

**Chỗ đáng chú ý: mục đích của việc tiêu thụ là chính đáng, nhưng phạm vi thì quá rộng.** Nó tồn tại để một
nét đã dùng làm dầm cặp không bị dựng lại lần nữa thành dầm nét lẻ — tức nó chỉ cần loại nét đó khỏi **phép
tính nét lẻ**, không cần loại khỏi **việc ghép cặp của các tiết diện sau**. Việc thu hẹp pool cho các tiết
diện sau là tác dụng phụ, không phải ý định.

## Hướng fix đề xuất (chưa làm)

Tách "đã tiêu thụ" ra khỏi "pool để ghép cặp":

```csharp
var consumed = new HashSet<Curve>() ;        // chỉ dùng cho phép tính nét lẻ
foreach (var section in context.Sections) {
    var groups = strokes.GetParallelCurveGroups(section.Width) ;   // LUÔN ghép trên pool đầy đủ
    if (settings.IsCreateForSingleLine) {
        foreach (var stroke in groups.SelectMany(g => g)) consumed.Add(stroke) ;
    }
    ...
}
// nét lẻ = pool đầy đủ − consumed − nét dài bằng một bề rộng tiết diện
```

Vì sao đường này đáng làm: phần **ghép cặp không đổi một dầm nào** — bật hay tắt checkbox đều ra 43 dầm, giống
hệt chế độ off hiện tại, và thứ tự tiết diện thôi ảnh hưởng. Chỉ số **nét lẻ** đổi (sẽ ≤ 41 vì tiết diện 350
giờ cũng tiêu thụ nét), nên phải **đo lại** rồi cập nhật `FramingFromCadFixtureFacts`, đừng đoán.

Đánh đổi: **lệch bản gốc**. Chủ dự án đã chốt nguyên tắc "y hệt" cho lượt port, nên đây là quyết định phải
được duyệt tường minh, không phải sửa lặng lẽ.

Phương án tối thiểu nếu không muốn đổi hành vi: **báo cho người dùng** — cuối lượt chạy, tiết diện nào khai mà
ra 0 dầm thì liệt kê ra. Rẻ, không đổi một dầm nào, và giết đúng phần đắt nhất của bug là việc mất *im lặng*.

Fix = chạy lại feature flow (`/sonny-flow:feature FramingFromCad`): spec chuyển hành vi mới vào `## Contract`
· implement viết test **RED trước** · đo lại số nét lẻ trên fixture · xong đổi Status ở đây thành `fixed` và
gỡ dòng link khỏi `## Related` của feature doc.
