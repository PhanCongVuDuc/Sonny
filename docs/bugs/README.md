# Bugs — sổ nợ defect, gồm cả cái kết luận là không phải bug

Mỗi báo cáo một file `<mã>-<slug>.md`. Luật nằm trong sonny-flow `rules/gates.md`: bug phát hiện ngoài
scope → ghi vào đây + một dòng link trong `## Related` của feature doc bị ảnh hưởng; **cấm fix im lặng**;
**cấm xoá file bug**; fix = chạy lại feature flow, xong `/sonny-flow:doc` đổi Status thành `fixed` (giữ
file làm lịch sử).

Bug do **người dùng báo từ ngoài** (video, file `.rvt`, lời kể) vào bằng `/sonny-flow:bug <đường dẫn gói>`
— nó đọc cả video, chẩn đoán tới `file:line`, ghi file ở đây rồi dừng. Gói gốc của người dùng **không**
copy vào repo; file bug chỉ trỏ đường dẫn trong mục `## Nguồn`.

## Status

| Status | Nghĩa |
|---|---|
| `triaging` | Đang chẩn đoán, chưa có kết luận. File đã sinh để giữ tiến độ |
| `open` | Bug thật, chưa fix |
| `open — giữ có chủ ý` | Bug thật, quyết định **không** fix; lý do ghi trong file |
| `fixed` | Đã fix; file giữ lại làm lịch sử |
| `not-a-bug` | Đã chẩn đoán: hành vi Revit, đúng thiết kế, hoặc người dùng hiểu sai tool |
| `duplicate` | Trùng một file cũ; trỏ về mã đó |

`not-a-bug` **vẫn nằm đây**, không tách folder: chi phí thật của một báo cáo là *lần thứ hai có người báo
lại nó*, và lúc đó cần đúng một chỗ để tra. AJ-001 là ví dụ — "hành vi Revit, không phải lỗi ta".

## Prefix

Chữ đầu tên feature; không thuộc feature nào thì `APP-`. Đăng ký ở đây để không sinh trùng.

| Prefix | Feature |
|---|---|
| `AJ-` | AutoJoin |
| `FFC-` | FramingFromCad |
| `ACD-` | AutoColumnDimension |
| `CFC-` | ColumnFromCad |
| `APP-` | Không thuộc feature nào — startup, license, ribbon, localization |

## Sổ

| Mã | Mô tả một dòng | Feature | Status |
|---|---|---|---|
| [AJ-001](AJ-001-overlapping-cut-regions-silently-unjoined.md) | Hai kẻ cắt chồng vùng cắt: Revit lặng lẽ gỡ join sau tại commit, tool vẫn báo thành công | AutoJoin | open |
| [FFC-001](FFC-001-single-line-pass-crashes-when-no-symbol-resolved.md) | Tick "Create for Single Line" mà pass dầm cặp không dựng được gì: lệnh văng exception, mất sạch | FramingFromCad | open — giữ có chủ ý |
| [FFC-002](FFC-002-duplicate-type-name-loses-a-whole-section-silently.md) | Type trùng tên `WxH`: `Duplicate` throw, bị catch trần ăn mất, cả tiết diện đó không có dầm nào, không báo | FramingFromCad | open — giữ có chủ ý |
| [FFC-003](FFC-003-single-line-mode-lets-one-section-eat-another.md) | Bật "Create for Single Line": tiết diện trước ăn hết nét của tiết diện sau, mất trọn một tiết diện, không báo — và thứ tự gõ tiết diện đổi kết quả | FramingFromCad | open |
