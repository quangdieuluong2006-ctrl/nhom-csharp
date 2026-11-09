# Hệ thống quản lý và bán sách trực tuyến – Nhóm 01

Đồ án môn **Lập trình C#** – GV: TS. Đỗ Như Tài – Trường Đại học Sài Gòn.

Hệ thống gồm hai ứng dụng dùng chung một CSDL SQL Server `BookStoreDB`:

| Thành phần | Công nghệ | Người dùng |
|---|---|---|
| `BookStore.Web` – website bán sách | ASP.NET Core MVC (.NET 8) | Khách hàng |
| `BookStore.Web` – trang quản trị `/Admin` (Area) | ASP.NET Core MVC (.NET 8) | Quản trị viên, nhân viên bán hàng, nhân viên kho |
| `BookStore.Desktop` – phần mềm quản trị | WinForms (.NET 8) | Quản trị viên, nhân viên bán hàng, nhân viên kho |

## Cấu trúc solution

```
BookStore.sln
└── src
    ├── BookStore.Domain          Thực thể (Sach, DonHang, ...) và enum – không phụ thuộc thư viện nào
    ├── BookStore.Application     Nghiệp vụ: service, DTO, IAppDbContext (dùng chung cho Web và Desktop)
    ├── BookStore.Infrastructure  EF Core: BookStoreDbContext, migration, dữ liệu mẫu
    ├── BookStore.Web             Controller, View, View Component
    └── BookStore.Desktop         WinForms: đăng nhập, form chính (MDI), các form quản trị
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
| `admin` | Quản trị viên | Web `/Admin`, Desktop |
| `nvbanhang` | Nhân viên bán hàng | Web `/Admin`, Desktop |
| `nvkho` | Nhân viên kho | Web `/Admin`, Desktop |

### Chạy bằng VS Code (thay cho Visual Studio)

1. Cài **.NET 8 SDK**, **SQL Server Express** (hoặc Developer) và VS Code + extension **C# Dev Kit** (VS Code sẽ gợi ý khi mở thư mục).
   VS Code không kèm LocalDB, nên tạo `appsettings.Local.json` (nội dung ở bước 1 phía trên, `Server=.\\SQLEXPRESS`)
   trong **cả** `src/BookStore.Web` và `src/BookStore.Desktop`.
2. Một lần duy nhất, tin cậy chứng chỉ HTTPS phát triển: `dotnet dev-certs https --trust`.
3. `File → Open Folder` chọn thư mục chứa `BookStore.sln`. Mở tab **Run and Debug** (Ctrl+Shift+D), chọn
   `Web (BookStore.Web)`, `Desktop (BookStore.Desktop)` hoặc `Web + Desktop (chạy cả hai)` rồi nhấn F5.
4. Hoặc chạy bằng terminal: `dotnet run --project src/BookStore.Web` và (terminal khác) `dotnet run --project src/BookStore.Desktop`.

Các form Desktop được dựng bằng mã C# (không phụ thuộc Designer), nên sửa và chạy trên VS Code bình thường.

### Chạy phần mềm quản trị (Desktop)

Chuỗi kết nối giống Web (LocalDB hoặc `appsettings.Local.json`), chọn `BookStore.Desktop` làm
Startup Project rồi F5. Đăng nhập bằng `admin` / `123456`. Hướng dẫn thêm chức năng: [docs/HUONG_DAN_DESKTOP.md](docs/HUONG_DAN_DESKTOP.md).

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

Lab08–09 (trang quản trị) có ở cả **trang Web `/Admin`** và ứng dụng **Desktop WinForms** (xem các mục dưới).

## Trang quản trị Web (`/Admin`)

Mở `https://localhost:<port>/Admin`, đăng nhập bằng tài khoản nhân viên (ví dụ `admin` / `123456`).

| Chức năng | Controller (Areas/Admin) | Quyền |
|---|---|---|
| Tổng quan: doanh thu tháng, biểu đồ 30 ngày, đơn chờ xử lý, sách sắp hết | `HomeController` | Mọi nhân viên |
| Đơn hàng: lọc theo trạng thái/ngày/từ khóa, xác nhận → giao → hoàn thành, hủy (hoàn kho), in phiếu giao | `DonHangController` | Quản trị, Bán hàng |
| Báo cáo doanh thu theo ngày, top bán chạy, xuất CSV (Excel) | `BaoCaoController` | Quản trị, Bán hàng |
| Sách: CRUD, tải ảnh bìa, ngừng/mở bán, lọc + phân trang | `SachController` | Quản trị, Kho |
| Danh mục (2 cấp), Nhà xuất bản, Tác giả | `DanhMucController`, `NhaXuatBanController`, `TacGiaController` | Quản trị, Kho |
| Kho: lập phiếu nhập/xuất/điều chỉnh (nhiều dòng), lịch sử phiếu, tồn kho | `KhoController` | Quản trị, Kho |
| Tài khoản: thêm/sửa nhân viên, khóa/mở, đặt lại mật khẩu (cả khách hàng) | `NhanVienController` | Quản trị |

Thiết kế:

- **Nghiệp vụ nằm ở `BookStore.Application/Services/QuanTri`** (`QuanLySachService`, `QuanLyDonHangService`, `KhoService`,
  `ThongKeService`...). Desktop đã đăng ký sẵn các service này nên form WinForms dùng lại được, không viết lại truy vấn.
- **Cookie đăng nhập riêng** (scheme `Admin`, `Path=/Admin`): khách hàng và nhân viên đăng nhập song song trên cùng trình duyệt
  không lẫn phiên. Mỗi request kiểm tra lại tài khoản – bị khóa hoặc đổi vai trò thì mất phiên ngay.
- **Tồn kho chỉ thay đổi qua phiếu kho và đơn hàng** (sửa sách không sửa được tồn). Giảm tồn dùng `UPDATE ... WHERE SoLuongTon >= n`
  nên không bao giờ âm; chuyển trạng thái đơn là cập nhật có điều kiện nên không ghi đè khi khách vừa tự hủy trên Web.
- Ràng buộc: không xóa sách/danh mục/NXB/tác giả đã có dữ liệu liên quan (chuyển sang ẩn/ngừng bán); không tự khóa/tự hạ quyền;
  luôn còn ít nhất một quản trị viên hoạt động; ảnh bìa chỉ nhận JPG/PNG/WEBP/GIF ≤ 2 MB (kiểm tra chữ ký tệp, không nhận SVG).
- Doanh thu chỉ tính đơn **Hoàn thành**, theo **ngày đặt** (CSDL chưa có cột ngày hoàn thành).

## Phần mềm quản trị Desktop (WinForms)

Đăng nhập bằng tài khoản nhân viên; menu hiện theo vai trò (`FrmChinh.PhanQuyenMenu`). Sau khi đăng nhập mở ngay màn hình **Tổng quan**.

| Màn hình (Forms/) | Chức năng | Vai trò |
|---|---|---|
| `FrmTongQuan` (F2) | Doanh thu tháng (so với tháng trước), đơn chờ xác nhận/đang xử lý, sách sắp hết, biểu đồ 30 ngày, đơn mới nhất; **tự làm mới mỗi 60 giây** (đơn khách vừa đặt trên Web hiện ra ngay); bấm ô số liệu để mở màn hình xử lý | Mọi nhân viên |
| `FrmDoiMatKhau` | Đổi mật khẩu của chính mình | Mọi nhân viên |
| `FrmTaiKhoan`, `FrmTaiKhoanNhanVien` | Tìm kiếm + lọc vai trò + phân trang; thêm/sửa nhân viên, phân vai trò; khóa/mở; đặt lại mật khẩu (cả khách hàng) | Quản trị |
| `FrmSach`, `FrmSachChiTiet` | Tìm theo tên/ISBN/tác giả/mã, lọc danh mục/NXB/trạng thái, phân trang, xem nhanh ảnh bìa; thêm/sửa (chọn nhiều tác giả, chọn ảnh bìa – ảnh được chép vào `wwwroot` của Web), ngừng/mở bán, xóa | Quản trị, Kho |
| `FrmDanhMuc` | Danh mục 2 cấp hiển thị dạng cây; thêm/sửa/xóa/ẩn | Quản trị, Kho |
| `FrmNhaXuatBan`, `FrmTacGia` | Thêm/sửa/xóa, tìm kiếm | Quản trị, Kho |
| `FrmPhieuKho`, `FrmLapPhieuKho` | Lịch sử phiếu (lọc loại/ngày), xem chi tiết; lập phiếu nhập/xuất/điều chỉnh nhiều dòng trong 1 giao tác | Quản trị, Kho |
| `FrmTonKho` | Tồn kho tăng dần, lọc ngưỡng, tô màu hết/sắp hết, xuất CSV | Quản trị, Kho |
| `FrmDonHang` | Lọc theo từ khóa/trạng thái/ngày, chip đếm theo trạng thái, chi tiết đơn; nút chuyển trạng thái chỉ hiện bước hợp lệ; hủy đơn hoàn kho; **in phiếu giao hàng** (Print Preview) | Quản trị, Bán hàng |
| `FrmDoanhThu`, `FrmSachBanChay` | Báo cáo theo khoảng ngày (nút nhanh 7/30/90 ngày, tháng này), ô tổng hợp, biểu đồ tự vẽ GDI+, xuất CSV | Quản trị, Bán hàng |

Kỹ thuật chính (thư mục `HaTang/`):

- **`FrmCoBan`** – lớp cha của mọi form. `GoiAsync((IQuanLySachService s) => s.TimKiemAsync(loc))` tạo một DI scope
  (một `DbContext`) **cho mỗi thao tác** rồi giải phóng ngay – tránh dữ liệu cũ bị EF lưu đệm khi form mở lâu
  ([khuyến nghị của Microsoft cho ứng dụng desktop](https://learn.microsoft.com/ef/core/dbcontext-configuration/#dbcontext-in-a-desktop-application)).
- **Dùng lại 100% nghiệp vụ** của `BookStore.Application` (cùng service với trang Web `/Admin`); form không chứa truy vấn SQL.
  Dữ liệu nhập được kiểm tra bằng chính các DataAnnotation của DTO (`KiemTraHopLe`) – Web và Desktop kiểm tra giống nhau.
- `GiaoDien` (màu, font, hàm tạo control/lưới dùng chung), `ThanhPhanTrang`, `BieuDoCot` + `OSoLieu` (biểu đồ, ô KPI tự vẽ),
  `AnhBiaDesktop` (kiểm tra chữ ký tệp ảnh, chép vào `wwwroot/images/sach` của Web).
- Lỗi nghiệp vụ (`BusinessException`) hiện hộp cảnh báo; lỗi khác ghi `logs/desktop-*.log` (`XuLyLoi`).

**Ảnh bìa:** khi chạy từ Visual Studio, Desktop tự tìm `src/BookStore.Web/wwwroot`. Nếu chạy bản đã publish ở nơi khác,
khai báo trong `appsettings.Local.json` của Desktop: `{ "ThuMucWebRoot": "D:\\...\\BookStore.Web\\wwwroot" }`.

## Web API

Controller `Controllers/Api/SachApiController.cs` (REST, JSON, chỉ đọc – dùng lại `ISachService`):

| Endpoint | Mô tả |
|---|---|
| `GET /api/sach?tuKhoa=&maDanhMuc=&maNhaXuatBan=&trang=1&kichThuoc=12` | Tìm kiếm sách đang bán, có phân trang |
| `GET /api/sach/{id}` | Chi tiết sách (404 nếu không có) |
| `GET /api/sach/goi-y?q=` | Tối đa 6 gợi ý – dùng cho **ô tìm kiếm gợi ý tức thời** trên thanh menu Web (`wwwroot/js/site.js`) |
| `GET /api/danh-muc` | Cây danh mục 2 cấp kèm số sách |

## Kịch bản kiểm thử liên thông (demo)

1. Chạy Web, đăng nhập `khachhang`, thêm sách vào giỏ và đặt hàng → đơn ở trạng thái *Chờ xác nhận*, tồn kho giảm.
2. Chạy Desktop, đăng nhập `nvbanhang` → màn hình Tổng quan (≤ 60 giây) thấy số đơn chờ xác nhận tăng; bấm ô đó → `FrmDonHang`.
3. Xác nhận → Bắt đầu giao → Hoàn thành. Trên Web, trang *Đơn hàng của tôi* hiện trạng thái mới; báo cáo doanh thu tăng.
4. Thử hủy đơn ở Desktop đúng lúc khách đã tự hủy trên Web → Desktop báo “Đơn hàng vừa được cập nhật bởi người khác”, không hoàn kho 2 lần.
5. Đăng nhập Desktop bằng `nvkho` → thêm sách mới (kèm ảnh bìa), lập phiếu nhập → sách hiện trên Web với đúng tồn kho.

Dữ liệu mẫu (tạo khi CSDL trống) gồm 40 đơn hàng trong 45 ngày gần nhất ở đủ các trạng thái để báo cáo có số liệu.
Nếu đã có CSDL cũ, xóa CSDL `BookStoreDB` rồi chạy lại để nạp dữ liệu mẫu mới.

## Quy trình nghiệp vụ liên thông Web ↔ Desktop

Khách đặt hàng trên Web (đơn ở trạng thái *Chờ xác nhận*, tồn kho bị trừ) → nhân viên bán hàng xác nhận /
giao / hoàn thành đơn trên Desktop → khách xem trạng thái đơn cập nhật trên trang *Đơn hàng của tôi*.

## Phân công

| Thành viên | MSSV | Phần phụ trách |
|---|---|---|
| Lương Diệu Quang (nhóm trưởng) | 3124411246 | *(cập nhật)* |
