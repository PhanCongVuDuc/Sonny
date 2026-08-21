---
type: "debugging"
date: "2026-08-21T13:18:54.019782+00:00"
question: "Vì sao NUnit.Close=true trong Sonny.Application.Tests.csproj mà Revit không tự đóng sau khi test xong?"
contributor: "graphify"
outcome: "corrected"
correction: "Csproj đặt GenerateAssemblyInfo=false (vì có AssemblyInfo.cs tay), mà các item <AssemblyAttribute> NUnit.Version/Open/Verbosity/Close do chính target GenerateAssemblyInfo sinh ra — tắt nó là DLL không có AssemblyMetadataAttribute nào, ricaun TestAdapter chạy default Open=false/Close=false (Revit bị tái sử dụng và không bao giờ đóng; version 2023 là adapter tự dò từ RevitAPI.dll chứ không phải từ setting). Fix: bật GenerateAssemblyInfo=true và xoá AssemblyInfo.cs tay. Kiểm chứng bằng reflection: Assembly.Load rồi GetCustomAttributes(AssemblyMetadataAttribute) phải ra đủ 4 key NUnit.*."
source_nodes: ["Sonny.Application.Tests"]
---

# Q: Vì sao NUnit.Close=true trong Sonny.Application.Tests.csproj mà Revit không tự đóng sau khi test xong?

## Answer

Setting có trong csproj nhưng chưa bao giờ vào được DLL.

## Outcome

- Signal: corrected
- Correction: Csproj đặt GenerateAssemblyInfo=false (vì có AssemblyInfo.cs tay), mà các item <AssemblyAttribute> NUnit.Version/Open/Verbosity/Close do chính target GenerateAssemblyInfo sinh ra — tắt nó là DLL không có AssemblyMetadataAttribute nào, ricaun TestAdapter chạy default Open=false/Close=false (Revit bị tái sử dụng và không bao giờ đóng; version 2023 là adapter tự dò từ RevitAPI.dll chứ không phải từ setting). Fix: bật GenerateAssemblyInfo=true và xoá AssemblyInfo.cs tay. Kiểm chứng bằng reflection: Assembly.Load rồi GetCustomAttributes(AssemblyMetadataAttribute) phải ra đủ 4 key NUnit.*.

## Source Nodes

- Sonny.Application.Tests