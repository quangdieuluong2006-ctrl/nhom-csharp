# Hệ thống quản lý và bán sách trực tuyến – Nhóm 01

Đồ án môn **Ngôn ngữ lập trình C#** – GVHD: **TS. Đỗ Như Tài** – Trường Đại học Sài Gòn – Lớp DCT124C4.

## Liên kết nộp bài

| Nội dung | Liên kết |
|---|---|
| Báo cáo (Google Doc) | https://docs.google.com/document/d/16X33uLi0ysc6rPKGMBnTGU5o9F5M-hY7mMxZA49yDto/edit |
| Slide thuyết trình | *(dán link Google Slides tại đây)* |
| Mã nguồn (GitHub nhóm) | https://github.com/khangdamgia-ux/C-SHARP |

## Thành viên

| STT | Họ tên | MSSV |
|---|---|---|
| 1 | Nguyễn Đạt Mạnh | 3124411162 |
| 2 | Đàm Gia Khang | 3124411121 |
| 3 | Lương Diệu Quang | 3124411246 |
| 4 | Phạm Quốc Trung | 3124411329 |

Phân công chi tiết: `docs/PhanCong_ThanhVien_Nhom01.xlsx`.

## Danh mục tài liệu nộp

| Tài liệu | Tệp |
|---|---|
| Cuốn báo cáo | `docs/Baocao_Nhom1.docx` (bản Google Doc: liên kết ở trên) |
| Slide thuyết trình | liên kết ở trên |
| Tài liệu Use Case (mẫu Fsoft) | `docs/BookStore_Nhom01_UseCase.xlsx` |
| Tài liệu thiết kế màn hình – Screen Design (mẫu Fsoft) | `docs/BookStore_Nhom01_ScreenDesign.xlsx` |
| Bản vẽ thiết kế (use case, ERD, workflow, C4, lớp, tuần tự, triển khai, mockup) | `docs/BanVe_ThietKe_Nhom01.xlsx` |
| Kế hoạch và các bước thực hiện đồ án (có biểu đồ Gantt) | `docs/KeHoach_CacBuocThucHien_Nhom01.xlsx` |
| Bảng phân công thành viên | `docs/PhanCong_ThanhVien_Nhom01.xlsx` |
| Hướng dẫn sử dụng phần mềm quản trị | `docs/HUONG_DAN_DESKTOP.md` |
| Mã nguồn | `src/`, `tests/`, `BookStore.sln` |

## Giới thiệu

Hệ thống gồm hai ứng dụng dùng chung một cơ sở dữ liệu SQL Server `BookStoreDB` và một thư viện nghiệp vụ:

| Thành phần | Công nghệ | Người dùng |
|---|---|---|
| `BookStore.Web` – website bán sách | ASP.NET Core MVC (.NET 8), Bootstrap 5 | Khách hàng |
| `BookStore.Desktop` – phần mềm quản trị | Windows Forms (.NET 8) | Quản trị viên, nhân viên bán hàng, nhân viên kho |
| `BookStoreDB` | Microsoft SQL Server, Entity Framework Core 8 | Dùng chung cho cả hai ứng dụng |

**Quy trình liên thông Web ↔ Desktop:** khách đặt hàng trên Web (đơn *Chờ xác nhận*, tồn kho bị trừ) → nhân viên
xác nhận đơn, xác nhận đã nhận tiền (chuyển khoản/MoMo), giao và hoàn thành trên Desktop → khách xem trạng thái mới
ở trang *Đơn hàng của tôi*.

## Chức năng

**Web (khách hàng):** trang chủ (sách bán chạy, sách mới), menu danh mục 2 cấp và nhà xuất bản, tìm kiếm theo tên,
tác giả, ISBN, lọc giá, sắp xếp, phân trang; chi tiết sách; đăng ký, đăng nhập; giỏ hàng; đặt hàng với 3 phương thức
thanh toán (COD, chuyển khoản ngân hàng, ví MoMo bằng mã QR); đơn hàng của tôi, hủy đơn chờ xác nhận; hồ sơ, đổi mật khẩu;
giao diện responsive.

**Desktop (quản trị):** đăng nhập, phân quyền 3 vai trò (khóa 30 giây sau 5 lần sai); tổng quan; quản lý sách, danh mục,
tác giả, nhà xuất bản, khách hàng (thêm, xóa, sửa, xem, tìm kiếm); xử lý đơn hàng, xác nhận đã nhận tiền; lập phiếu
nhập/xuất/điều chỉnh kho, tồn kho và lịch sử phiếu; báo cáo doanh thu, biểu đồ, sách bán chạy, xuất Excel;
tài khoản và phân quyền; đổi mật khẩu.

## Cấu trúc mã nguồn

```
BookStore.sln
├── src
│   ├── BookStore.Domain          Thực thể (Sach, DonHang, ...) và enum – không phụ thuộc thư viện nào
│   ├── BookStore.Application     Nghiệp vụ dùng chung: service, DTO, kiểm tra dữ liệu, phân quyền, IAppDbContext
│   ├── BookStore.Infrastructure  EF Core: BookStoreDbContext (Fluent API), migration, dữ liệu mẫu
│   ├── BookStore.Web             ASP.NET Core MVC: controller, view, view component
│   └── BookStore.Desktop         WinForms: đăng nhập, cửa sổ chính, các màn hình quản trị
├── tests
│   └── BookStore.Tests           Kiểm thử tích hợp xUnit chạy trên SQL Server thật
└── docs                          Tài liệu phân tích, thiết kế, báo cáo
```

## Cài đặt và chạy

**Yêu cầu:** .NET 8 SDK; SQL Server (LocalDB cài kèm Visual Studio, hoặc Express/Developer);
Visual Studio 2022 (khuyên dùng, có Designer WinForms) hoặc VS Code với C# Dev Kit.

1. Tải mã nguồn: `git clone https://github.com/khangdamgia-ux/C-SHARP.git`
2. **Chuỗi kết nối:** mặc định dùng `(localdb)\MSSQLLocalDB`. Nếu dùng SQL Server khác, không sửa `appsettings.json`
   mà tạo tệp `appsettings.Local.json` (đã nằm trong `.gitignore`) cạnh `appsettings.json` của `src/BookStore.Web`
   và `src/BookStore.Desktop`:
   ```json
   { "ConnectionStrings": { "BookStoreDB": "Server=.\\SQLEXPRESS;Database=BookStoreDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True" } }
   ```
3. **Chạy Web:** `dotnet run --project src/BookStore.Web` (hoặc chọn `BookStore.Web` làm Startup Project rồi F5).
   Lần chạy đầu tự tạo CSDL theo migration và nạp dữ liệu mẫu (24 sách, 12 danh mục, 8 NXB, 5 tài khoản).
4. **Chạy Desktop:** `dotnet run --project src/BookStore.Desktop` (hoặc chọn `BookStore.Desktop` rồi F5).
5. **VS Code:** mở thư mục gốc, vào Run and Debug (Ctrl+Shift+D), chọn `Web + Desktop (demo quy trình liên thông)`, bấm F5.

**Tài khoản thử nghiệm** (mật khẩu đều là `123456`):

| Tên đăng nhập | Vai trò | Dùng ở |
|---|---|---|
| `khachhang`, `khachhang2` | Khách hàng | Web |
| `admin` | Quản trị viên | Desktop |
| `nvbanhang` | Nhân viên bán hàng | Desktop |
| `nvkho` | Nhân viên kho | Desktop |

**Thanh toán QR:** bản trên GitHub dùng mã QR mẫu (`wwwroot/images/thanhtoan/qr-mau.svg`). Mã QR và số tài khoản thật
của cửa hàng khai báo trong `appsettings.Local.json`, không đưa lên GitHub.

## Kiểm thử

```bash
dotnet test
```

41 ca kiểm thử tích hợp: 36 ca trình bày trong báo cáo (Bảng 5.4) và 5 ca cho chức năng bổ sung (quản lý khách hàng,
sách bán chạy). Mặc định tạo CSDL tạm `BookStoreDB_KiemThu` trên LocalDB rồi xóa khi xong; đổi máy chủ bằng biến môi
trường `BOOKSTORE_TEST_DB`. Trong Visual Studio: **Test → Run All Tests**.

## Tài liệu tham khảo, mã nguồn bên ngoài

- Microsoft Learn: C#, ASP.NET Core, Windows Forms, Entity Framework Core, SQL Server.
- Bộ Lab "Web bán sách MVC5" (Lab00 – Lab10) của môn học: phần Web tham khảo trình tự Lab01 – Lab07.
- Thư viện: Bootstrap 5.3.3, Bootstrap Icons, BCrypt.Net-Next, X.PagedList, Serilog, ClosedXML, xUnit.
