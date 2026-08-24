# .sonnyflow — mọi cơ khí sonny-flow lắp vào repo này, một chỗ duy nhất

Do `/sonny-flow:setup` tạo và sửa. **Tài liệu sản phẩm KHÔNG nằm đây** — feature doc / ADR / bugs vẫn ở
`docs/` vì chúng là sản phẩm cho người đọc, không phải máy móc của flow.

**Hai tầng kinh nghiệm, đừng lẫn:** `retro/` là **hàng đợi** thứ agent phát hiện (chờ chủ dự án xét);
`lessons/` là **luật phải theo** — chỉ vào đây khi chủ dự án confirm. Vòng đời + ba status:
[`retro/README.md`](retro/README.md). Luật đúng cho mọi project thì không nằm ở đây mà ở `rules/` của
plugin sonny-flow (xem ADR 0001 của plugin).

| File | Việc |
|---|---|
| `loop.ps1` | Cửa duy nhất chạy `Sonny.Application.Tests` — build + trust + test + verdict 0/1/2. Chính là `loopCommand` trong `CLAUDE.md` |
| `watch-always-load.ps1` | Tự click dialog trust "Always Load", thoát sau cú click đầu |
| `hooks/revit-test-guard.ps1` | Hook PreToolUse chặn `dotnet test` trần vào project Revit |
| `lessons/test-environment.md` | **Đọc trước khi chạy/viết test Revit** — bẫy môi trường của project này + luật verdict |
| `lessons/test-safety-net.md` | **Đọc trước khi move/rename/xoá type** — test nào bám chi tiết implementation, gãy kiểu wiring hay assertion; nguyên liệu bước baseline của flow refactor |
| `retro/README.md` | **Hàng đợi bài học** — bảng chờ xét / đã nhận / không nhận, cùng luật ba status |
| `retro/<Feature>-retro.md` | Bài học thô của một lượt flow, mỗi mục một status |
| `retro/LESSONS.md` | Index theo lượt flow: mỗi lượt một dòng — có lỗ là thấy ngay lượt nào bỏ retro |

Ba thứ của flow bắt buộc nằm ngoài (chỗ hệ thống quy định, chỉ là con trỏ về đây):
`loopCommand:` + pointer trong `CLAUDE.md` (context nạp mỗi phiên) · entry hook + `claudeMdExcludes`
trong `.claude/settings.json` (Claude Code chỉ đọc ở đó).
