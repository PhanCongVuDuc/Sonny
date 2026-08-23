# .sonnyflow — mọi cơ khí sonny-flow lắp vào repo này, một chỗ duy nhất

Do `/sonny-flow:setup` tạo và sửa. **Tài liệu sản phẩm KHÔNG nằm đây** — feature doc / ADR / bugs vẫn ở
`docs/` vì chúng là sản phẩm cho người đọc, không phải máy móc của flow.

| File | Việc |
|---|---|
| `loop.ps1` | Cửa duy nhất chạy `Sonny.Application.Tests` — build + trust + test + verdict 0/1/2. Chính là `loopCommand` trong `CLAUDE.md` |
| `watch-always-load.ps1` | Tự click dialog trust "Always Load", thoát sau cú click đầu |
| `hooks/revit-test-guard.ps1` | Hook PreToolUse chặn `dotnet test` trần vào project Revit |
| `revit-test-environment.md` | **Đọc trước khi chạy/viết test Revit** — các bẫy đã trả giá + luật verdict |
| `retro/<Feature>-retro.md` | Kinh nghiệm sau mỗi lần chạy flow — backlog để nâng cấp skill sonny-flow |

Ba thứ của flow bắt buộc nằm ngoài (chỗ hệ thống quy định, chỉ là con trỏ về đây):
`loopCommand:` + pointer trong `CLAUDE.md` (context nạp mỗi phiên) · entry hook + `claudeMdExcludes`
trong `.claude/settings.json` (Claude Code chỉ đọc ở đó).
