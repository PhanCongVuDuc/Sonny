# Bugs — sổ nợ defect đã biết, chưa fix

Mỗi bug một file `<mã>-<slug>.md`, mã theo feature (`AJ-` = AutoJoin…). Luật nằm trong sonny-flow
`rules/gates.md`: bug phát hiện ngoài scope → ghi vào đây + một dòng link trong `## Related` của
feature doc bị ảnh hưởng; **cấm fix im lặng**; fix = chạy lại feature flow, xong đổi Status thành
`fixed` (giữ file làm lịch sử).

| Mã | Mô tả một dòng | Feature | Status |
|---|---|---|---|
| [AJ-001](AJ-001-overlapping-cut-regions-silently-unjoined.md) | Hai kẻ cắt chồng vùng cắt: Revit lặng lẽ gỡ join sau tại commit, tool vẫn báo thành công | AutoJoin | open |
