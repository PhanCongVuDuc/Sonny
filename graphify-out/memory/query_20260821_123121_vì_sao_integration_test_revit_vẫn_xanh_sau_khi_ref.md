---
type: "verification"
date: "2026-08-21T12:31:21.503640+00:00"
question: "Vì sao integration test Revit vẫn xanh sau khi refactor dù có thể đang chạy code cũ?"
contributor: "graphify"
outcome: "corrected"
correction: "ricaun.RevitTest giữ Revit mở giữa các lần chạy; add-in trong %AppData%/Autodesk/Revit/Addins/<year> load DLL vào AppDomain TRƯỚC test assembly, nên khi skip deploy (-p:DeployRevitAddin=false) gate chạy trên binary CŨ và pass vô nghĩa. Quy trình đúng: đóng Revit test host (CloseMainWindow, chờ ~20s), build có deploy, rồi mới chạy test. Triệu chứng khi lệch version: TypeLoadException cho type mới."
source_nodes: ["AutoColumnDimensionIntegrationTest", "ColumnFromCadIntegrationTest"]
---

# Q: Vì sao integration test Revit vẫn xanh sau khi refactor dù có thể đang chạy code cũ?

## Answer

Gate tưởng đã xác nhận code mới nhưng Revit đang chạy binary cũ từ thư mục Addins.

## Outcome

- Signal: corrected
- Correction: ricaun.RevitTest giữ Revit mở giữa các lần chạy; add-in trong %AppData%/Autodesk/Revit/Addins/<year> load DLL vào AppDomain TRƯỚC test assembly, nên khi skip deploy (-p:DeployRevitAddin=false) gate chạy trên binary CŨ và pass vô nghĩa. Quy trình đúng: đóng Revit test host (CloseMainWindow, chờ ~20s), build có deploy, rồi mới chạy test. Triệu chứng khi lệch version: TypeLoadException cho type mới.

## Source Nodes

- AutoColumnDimensionIntegrationTest
- ColumnFromCadIntegrationTest