# retro — hàng đợi bài học, chờ chủ dự án xét

Đây là **inbox**, không phải kho luật. Agent ghi vào đây thứ nó **phát hiện** trong một lượt chạy flow;
thứ được chủ dự án **confirm** mới chuyển sang [`../lessons/`](../lessons/) và trở thành luật phải theo.

```
lượt flow phát hiện  →  retro/ (chờ xét)  →  CHỦ DỰ ÁN CONFIRM  →  lessons/ (luật phải theo)
                        ↑ tồn dư ở đây là BÌNH THƯỜNG
```

**Tồn dư không phải lỗi.** Một dự án sinh ra nhiều bài học hơn số bài đáng thành luật; hàng đợi còn hàng
là trạng thái đúng. Cái sai duy nhất là bài học **không được ghi**, hoặc **đã confirm mà không ai chép**.

## Ba status — mỗi bài học đúng một cái

| Status | Nghĩa | Ai đặt |
|---|---|---|
| `chờ xét` | mới ghi, chưa ai xem | agent, lúc chạy bước 6d |
| `đã nhận → <file>` | chủ dự án confirm, đã chép sang `lessons/` | agent, sau khi chủ dự án gật |
| `không nhận — <lý do>` | chủ dự án đã xem và gạt | agent, sau khi chủ dự án gạt |

Bài `đã nhận` và `không nhận` **gạch ngang** trong file retro thay vì xoá trắng — hàng đợi vẫn sạch
(gạch ngang = không còn chờ), nhưng **bối cảnh phát hiện** thì giữ lại. Bối cảnh là thứ bản chép sang
`lessons/` luôn mất: `lessons/` ghi *luật gọn*, retro ghi *chuyện gì đã xảy ra để ra luật đó*.

Vì sao không xoá trắng: nó là cách duy nhất để lần sau biết bài này **đã có nhà**. Ngày 2026-08-23 cùng
một bài học (`async` làm mất Revit API context) bị thêm vào hai chỗ trong một ngày, vì retro không nói
được rằng nó đã được chép đi rồi.

## Hàng đợi hiện tại

Cập nhật ở ô 6d mỗi lượt flow. Con số phải khớp với số dòng `chờ xét` trong các file retro.

| Lượt flow | Chờ xét | Đã nhận | Không nhận | File |
|---|---|---|---|---|
| AutoJoin (2026-08-22) | 10 | 0 | 0 | [AutoJoin-retro.md](AutoJoin-retro.md) |
| FramingFromCad (2026-08-23) | 5 | 8 | 0 | [FramingFromCad-retro.md](FramingFromCad-retro.md) |

**Tổng chờ xét: 15.** Bước 0 (orient) quét bảng này để không phát hiện lại thứ đã có trong hàng đợi.

## Còn `LESSONS.md`?

[`LESSONS.md`](LESSONS.md) là index **theo lượt flow** — mỗi lượt một dòng, bài học đắt nhất. Nó trả lời
*"lượt nào đã chạy và học được gì"*; bảng trên trả lời *"còn gì chưa xử"*. Hai câu hỏi khác nhau, và
LESSONS.md có lỗ nghĩa là một lượt flow đã bỏ bước retro.
