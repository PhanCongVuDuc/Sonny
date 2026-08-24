# AJ-001 — Join bị Revit gỡ ngầm khi hai kẻ cắt chồng vùng cắt, người dùng không được báo

| | |
|---|---|
| **Status** | open |
| **Feature** | [AutoJoin](../features/AutoJoin.md) |
| **Affects** | AlphaBIM gốc **và** Sonny (cùng logic port) |
| **Phát hiện** | 2026-08-22, khi dựng integration test (station S15 trước khi tách vùng cắt) |

## Hiện tượng

Chạy Auto Join với hai kẻ cắt cùng nhắm một element mà vùng cắt của chúng chồng lên nhau: lệnh chạy
xong không lỗi, dialog thành công vẫn hiện, nhưng **cặp join sau đã bị Revit gỡ** — người dùng tưởng
cả hai dính, thực tế chỉ một.

## Tái hiện (Revit UI, ~2 phút)

1. Cột KC 400×400 cao 3 m.
2. Dầm bề rộng ≥ 400 xuyên qua cột, đáy dầm ở ~0.5 m (dầm nuốt trọn tiết diện cột từ 0.5 m lên).
3. Khối Generic Model giao cột từ chân đến ~1 m (khúc 0.5–1 m nằm trong vùng dầm).
4. Chạy Auto Join với 2 rule `Beam → StructuralColumn` và `GenericModel → StructuralColumn`.
5. Lệnh xong không lỗi, nhưng select khối GM → **không** joined với cột (dầm thì có). Dời khối GM để
   vùng cắt tách khỏi vùng dầm rồi chạy lại → cả hai dính.

## Bản chất

Trong transaction cả hai `JoinGeometry` đều thành công; lúc commit Revit tính lại, phán cặp sau
"joined but do not intersect" và tự unjoin qua failure-resolution mức **warning** — không đi qua F9
(`TryExecuteJoin` đã trả true nên không có log), không có failing id (không phải error nên tracker
rỗng), dialog thành công vẫn hiện. Đây là hành vi Revit; bản gốc AlphaBIM cùng logic nên bị y hệt.

## Hướng fix đề xuất (chưa làm)

Sau commit, re-check `AreElementsJoined` cho mọi cặp đã join trong lượt chạy; cặp nào bị gỡ thì select
và báo người dùng (cùng cơ chế F10). Fixture đã có sẵn hình để dựng test tái hiện — xem lịch sử
`AutoJoinFixtureBuilder` (station S15 trước commit tách vùng cắt).

Fix = chạy lại feature flow trên AutoJoin (`/sonny-flow:feature AutoJoin`): spec chuyển hành vi đúng
thành failure mode trong `## Contract` · implement viết test tái hiện **RED trước** · xong thì đổi
Status ở đây thành `fixed` và gỡ dòng link khỏi `## Related` của feature doc.
