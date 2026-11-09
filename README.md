# Hệ thống quản lý và bán sách trực tuyến – Nhóm 01

Đồ án môn **Lập trình C#** – GV: TS. Đỗ Như Tài – Trường Đại học Sài Gòn.

Hệ thống gồm hai ứng dụng dùng chung một CSDL SQL Server `BookStoreDB`:

| Thành phần | Công nghệ | Người dùng |
|---|---|---|
| `BookStore.Web` – website bán sách | ASP.NET Core MVC (.NET 8) | Khách hàng |
| `BookStore.Desktop` – phần mềm quản trị | WinForms (.NET 8) | Quản trị viên, nhân viên bán hàng, nhân viên kho |

## Cấu trúc solution

```
BookStore.sln
└── src
    ├── BookStore.Domain          Thực thể (Sach, DonHang, ...) và enum – không phụ thuộc thư viện nào
    ├── BookStore.Application     Nghiệp vụ: service, DTO, IAppDbContext (dùng chung cho Web và Desktop)
    ├── BookStore.Infrastructure  EF Core: BookStoreDbContext, migration, dữ liệu mẫu
    ├── BookStore.Web             Controller, View, View Component
    └── BookStore.Desktop         WinForms: đăng nhập, phân quyền, các form quản trị
tests
└── BookStore.Tests               Kiểm thử tích hợp (xUnit) chạy trên SQL Server thật
```

## Chạy thử

Yêu cầu: **.NET 8 SDK**, **SQL Server** (Express/Developer/LocalDB), Visual Studio 2022 trở lên (khuyên dùng, có Designer WinForms).

1. **Chuỗi kết nối:** mặc định dùng **SQL Server LocalDB** (`(localdb)\\MSSQLLocalDB`, cài kèm Visual Studio).
   Nếu máy bạn dùng SQL Server khác, **không sửa `appsettings.json`** mà tạo file `appsettings.Local.json`
   (đã được .gitignore, không lên GitHub) đặt cạnh `appsettings.json` trong `src/BookStore.Web` và `src/BookStore.Desktop`:
   ```json
   { "ConnectionStrings": { "BookStoreDB": "Server=.\\SQLEXPRESS;Database=BookStoreDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True" } }
   ```
2. Chạy project `BookStore.Web` (F5 trong Visual Studio, hoặc `dotnet run --project src/BookStore.Web`).
   Lần chạy đầu ứng dụng **tự tạo CSDL theo migration và nạp dữ liệu mẫu** (24 sách, 12 danh mục, 8 NXB).
3. Tài khoản thử nghiệm (mật khẩu đều là `123456`):

| Tên đăng nhập | Vai trò | Dùng ở |
|---|---|---|
| `khachhang` | Khách hàng | Web |
| `admin` | Quản trị viên | Desktop |
| `nvbanhang` | Nhân viên bán hàng | Desktop |
| `nvkho` | Nhân viên kho | Desktop |

### Chạy phần mềm quản trị (Desktop)

Chuỗi kết nối giống Web (LocalDB hoặc `appsettings.Local.json`), chọn `BookStore.Desktop` làm
Startup Project rồi F5. Đăng nhập bằng `admin` / `123456`. Hướng dẫn thêm chức năng: [docs/HUONG_DAN_DESKTOP.md](docs/HUONG_DAN_DESKTOP.md).

### Dùng VS Code thay cho Visual Studio

Mở **thư mục** `nhom-csharp` bằng VS Code (File → Open Folder), cài các tiện ích được gợi ý (C# Dev Kit, SQL Server).
Vào **Run and Debug** (Ctrl+Shift+D), chọn `Web (khách hàng)`, `Desktop (quản trị)` hoặc `Web + Desktop` rồi bấm **F5**.
Muốn web tự tải lại khi sửa code: Terminal → Run Task → `web: chạy và tự tải lại khi sửa code (dotnet watch)`.
Giao diện kéo-thả (Designer) của WinForms chỉ có trong Visual Studio.

Khi thay đổi thực thể, tạo migration mới:

```bash
dotnet tool restore
dotnet ef migrations add TenMigration -p src/BookStore.Infrastructure -s src/BookStore.Web -o Data/Migrations
```

Log ghi tại `src/BookStore.Web/logs/web-<ngày>.log` (Serilog).

## Chức năng Web đã có (đối chiếu Lab)

| Lab | Nội dung trong Lab (MVC 5) | Hiện thực trong đồ án (ASP.NET Core) |
|---|---|---|
| Lab01 | Trang chủ hiển thị sách mới | `HomeController.Index` – 8 sách mới nhất |
| Lab02 | Partial View chủ đề, NXB | View Component `MenuDanhMuc` (cây 2 cấp), `MenuNhaXuatBan` |
| Lab03 | Sách theo chủ đề/NXB, chi tiết | `/Sach?danhmuc=`, `/Sach?nxb=`, `/Sach/ChiTiet/{id}` |
| Lab04 | Đăng ký, đăng nhập | `TaiKhoanController` – BCrypt + Cookie Authentication |
| Lab05 | Giỏ hàng bằng Session | `GioHangController` – thêm, cập nhật, xóa, xóa tất cả |
| Lab06 | Đặt hàng, xác nhận đơn | `DatHangController` – bắt buộc đăng nhập, giao tác + trừ kho có điều kiện |
| Lab07 | Phân trang, tìm kiếm | X.PagedList, lọc theo từ khóa, giá, sắp xếp |
| – | Theo dõi đơn hàng | `DonHangController` – danh sách, chi tiết, khách tự hủy đơn chờ xác nhận |

Lab08–09 (trang quản trị) được hiện thực bằng **ứng dụng Desktop WinForms** theo yêu cầu đề tài
(phần quản trị: Windows Forms; phần người dùng: Web).

## Chức năng Desktop (phần mềm quản trị)

Đăng nhập và phân quyền 3 vai trò; tổng quan; quản lý sách, danh mục, NXB, tác giả; khách hàng; xử lý đơn hàng;
lập phiếu kho, tồn kho; báo cáo doanh thu, sách bán chạy, xuất Excel; tài khoản và phân quyền.
Chi tiết: [docs/HUONG_DAN_DESKTOP.md](docs/HUONG_DAN_DESKTOP.md).

## Kiểm thử

```bash
dotnet test
```

31 ca kiểm thử tích hợp (UT01–UT10 trong báo cáo, quy trình liên thông, phân quyền, tranh chấp dữ liệu).
Mặc định tạo CSDL tạm `BookStoreDB_KiemThu` trên LocalDB rồi xóa khi xong; đổi máy chủ bằng biến môi trường
`BOOKSTORE_TEST_DB`. Trong Visual Studio: menu **Test → Run All Tests**.

## Quy trình nghiệp vụ liên thông Web ↔ Desktop

Khách đặt hàng trên Web (đơn ở trạng thái *Chờ xác nhận*, tồn kho bị trừ) → nhân viên bán hàng xác nhận /
giao / hoàn thành đơn trên Desktop → khách xem trạng thái đơn cập nhật trên trang *Đơn hàng của tôi*.

## Phân công

| Thành viên | MSSV | Phần phụ trách |
|---|---|---|
| Lương Diệu Quang (nhóm trưởng) | 3124411246 | *(cập nhật)* |
