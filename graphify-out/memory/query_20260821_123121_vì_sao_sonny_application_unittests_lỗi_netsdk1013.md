---
type: "debugging"
date: "2026-08-21T12:31:21.717320+00:00"
question: "Vì sao Sonny.Application.UnitTests lỗi NETSDK1013 TargetFramework rỗng khi build qua solution nhưng dotnet test csproj trực tiếp lại xanh?"
contributor: "graphify"
outcome: "corrected"
correction: "Đổi <Configurations> trong csproj mà không cập nhật mapping trong Sonny.sln: sln vẫn trỏ mọi solution configuration về 'Debug|Any CPU' không còn tồn tại, project bị build với Configuration=Debug nên không nhánh điều kiện nào gán TargetFramework. Luật: task nào chạm <Configurations> hoặc thêm project thì gate phải gồm dotnet build Sonny.sln -c 'Debug R25', không chỉ build per-project."
source_nodes: ["Sonny.Application.UnitTests"]
---

# Q: Vì sao Sonny.Application.UnitTests lỗi NETSDK1013 TargetFramework rỗng khi build qua solution nhưng dotnet test csproj trực tiếp lại xanh?

## Answer

Solution mapping lỗi thời, không phải csproj sai.

## Outcome

- Signal: corrected
- Correction: Đổi <Configurations> trong csproj mà không cập nhật mapping trong Sonny.sln: sln vẫn trỏ mọi solution configuration về 'Debug|Any CPU' không còn tồn tại, project bị build với Configuration=Debug nên không nhánh điều kiện nào gán TargetFramework. Luật: task nào chạm <Configurations> hoặc thêm project thì gate phải gồm dotnet build Sonny.sln -c 'Debug R25', không chỉ build per-project.

## Source Nodes

- Sonny.Application.UnitTests