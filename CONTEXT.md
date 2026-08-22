# CONTEXT — ubiquitous language

Thuật ngữ dùng thống nhất trong docs, code và commit message. Tên code giữ tiếng Anh.

| Term | Meaning |
|---|---|
| **decision core** | Phần logic quyết định nghiệp vụ thuần, Revit-free, nằm ở Domain/UseCases, unit-test được không cần Revit. |
| **mechanism** | Phần cơ chế gọi Revit API (đọc document, transaction, tạo element), nằm ở Infrastructure sau interface. |
| **DTO boundary** | Ranh giới giữa mechanism và decision core: Infrastructure dựng DTO thuần (Point3D/double/string) từ wrapper Revit, decision core trả về *plan*; kiểu Revit không bao giờ rời Infrastructure. Xem [ADR 0001](docs/adr/0001-decision-logic-lives-in-usecases.md). |
| **plan** | Kết quả của decision core: mô tả việc cần làm (ví dụ `DimensionAxisPlan`) bằng dữ liệu thuần + id (`UniqueId` string), để Infrastructure map về element Revit và thi hành. |
| **grid candidate** | DTO thuần đại diện một grid trong view (`UniqueId`, direction, origin) để decision core chọn grid mà không thấy `GridWrapperBase`. |
| **characterization test** | Test khoá hành vi *hiện tại* trước khi refactor, kể cả hành vi trông như bug (silent skip, failed-count arithmetic). |
| **pure form** | Kiểu interactor chuẩn: nằm ở UseCases, không Revit type, mock được end-to-end — `ColumnFromCadInteractor` là mẫu. |
