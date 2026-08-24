# Bugs — sổ nợ defect đã biết, chưa fix

Mỗi bug một file `<mã>-<slug>.md`, mã theo feature (`AJ-` = AutoJoin…). Luật nằm trong sonny-flow
`rules/gates.md`: bug phát hiện ngoài scope → ghi vào đây + một dòng link trong `## Related` của
feature doc bị ảnh hưởng; **cấm fix im lặng**; fix = chạy lại feature flow, xong đổi Status thành
`fixed` (giữ file làm lịch sử).

| Mã | Mô tả một dòng | Feature | Status |
|---|---|---|---|
| [AJ-001](AJ-001-overlapping-cut-regions-silently-unjoined.md) | Hai kẻ cắt chồng vùng cắt: Revit lặng lẽ gỡ join sau tại commit, tool vẫn báo thành công | AutoJoin | open |
| [FFC-001](FFC-001-single-line-pass-crashes-when-no-symbol-resolved.md) | Tick "Create for Single Line" mà pass dầm cặp không dựng được gì: lệnh văng exception, mất sạch | FramingFromCad | open — giữ có chủ ý |
| [FFC-002](FFC-002-duplicate-type-name-loses-a-whole-section-silently.md) | Type trùng tên `WxH`: `Duplicate` throw, bị catch trần ăn mất, cả tiết diện đó không có dầm nào, không báo | FramingFromCad | open — giữ có chủ ý |
| [FFC-003](FFC-003-single-line-mode-lets-one-section-eat-another.md) | Bật "Create for Single Line": tiết diện trước ăn hết nét của tiết diện sau, mất trọn một tiết diện, không báo — và thứ tự gõ tiết diện đổi kết quả | FramingFromCad | open |
