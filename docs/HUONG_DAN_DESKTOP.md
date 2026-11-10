# Phần mềm quản trị (BookStore.Desktop)

Ứng dụng Windows Forms (.NET 8) cho nhân viên cửa hàng. Dùng chung tầng nghiệp vụ `BookStore.Application`
và CSDL `BookStoreDB` với website, nên mọi thay đổi ở Desktop được Web thấy ngay và ngược lại.

## Các màn hình (theo báo cáo mục 3.9)

| Mã | Form | Chức năng | Vai trò |
|---|---|---|---|
| D01 | `FrmDangNhap` | Đăng nhập, từ chối tài khoản khách/khóa; sai 5 lần liên tiếp thì tạm khóa 30 giây | Mọi nhân viên |
| D02 | `FrmChinh`, `FrmTongQuan` | Menu theo vai trò, tổng quan số liệu, báo đơn mới từ website, đổi mật khẩu, đăng xuất | Mọi nhân viên |
| D03 | `FrmSach` | Tìm/lọc sách; thêm, sửa, ngừng kinh doanh, ảnh bìa, tác giả; xuất Excel | QTV (sửa), NVBH/NVK (xem) |
| D04 | `FrmDanhMuc`, `FrmNhaXuatBan`, `FrmTacGia` | Thêm/sửa/xóa/tìm; đang dùng thì chỉ ngừng dùng | QTV |
| D05 | `FrmKhachHang` | Tra cứu, cập nhật khách hàng, xem đơn của khách | QTV, NVBH |
| D06 | `FrmDonHang` | Lọc đơn, xác nhận → đang giao → hoàn thành, hủy (hoàn kho), tự làm mới 30 giây | QTV, NVBH |
| D07 | `FrmPhieuKho`, `FrmKho` | Lập phiếu nhập/xuất/điều chỉnh nhiều dòng; tồn kho, sách sắp hết, lịch sử phiếu | QTV, NVK |
| D08 | `FrmTaiKhoan` | Tạo tài khoản nhân viên, phân vai trò, khóa/mở khóa, đặt lại mật khẩu | QTV |
| D09 | `FrmBaoCao` | Doanh thu theo ngày (biểu đồ), sách bán chạy, xuất Excel | QTV, NVBH |

Tài khoản thử: `admin`, `nvbanhang`, `nvkho` – mật khẩu `123456`.

## Quy trình liên thông Web ↔ Desktop (báo cáo mục 4.3.4)

1. Khách đặt hàng trên Web → đơn **Chờ xác nhận**, tồn kho bị trừ.
2. Desktop: thanh trạng thái báo "Có đơn MỚI từ website" (kiểm tra mỗi 30 giây); màn hình Tổng quan liệt kê đơn chờ.
3. Nhân viên bán hàng mở **Bán hàng → Xử lý đơn hàng** (F3), xác nhận → đang giao → hoàn thành (tự ghi "Đã thanh toán").
4. Khách mở **Đơn hàng của tôi** trên Web thấy trạng thái mới; nút Hủy đơn biến mất khi đơn đã được xác nhận.

Nếu khách vừa hủy đơn trên Web đúng lúc nhân viên bấm xác nhận, lệnh xác nhận bị từ chối (UPDATE có điều kiện)
và nhân viên được yêu cầu tải lại – dữ liệu không bị ghi đè.

## Cấu trúc mã

```
src/BookStore.Desktop
├── Program.cs              DI (Generic Host), đăng ký service và form, vòng đăng nhập/đăng xuất
├── appsettings.json        Chuỗi kết nối; "ThuMucAnhWeb" (tùy chọn) – thư mục wwwroot của Web để lưu ảnh bìa
├── HaTang/                 Hạ tầng giao diện dùng chung
│   ├── PhienDangNhap.cs    Người đang đăng nhập, kiểm tra quyền: _phien.CoQuyen(VaiTro.QuanTriVien)
│   ├── FormFactory.cs      Tạo form qua DI, mỗi form một DbContext riêng
│   ├── GiaoDien.cs         Định dạng lưới, ThucHienAsync (khóa form + bắt BusinessException), hàng đợi CSDL theo form
│   ├── ThongBao.cs, HopThoai.cs, XuLyLoi.cs (bắt lỗi toàn cục + ghi log, hiện mã lỗi thay vì chi tiết kỹ thuật)
│   ├── KiemTraNhap.cs      Báo lỗi ngay tại ô nhập (ErrorProvider), dùng lại quy tắc KiemTra của tầng nghiệp vụ
│   ├── AnhSach.cs          Chép ảnh bìa vào wwwroot/images/sach của Web
│   ├── XuatExcel.cs        Xuất danh sách ra .xlsx (ClosedXML)
│   └── BieuDoCot.cs        Biểu đồ cột tự vẽ bằng GDI+
└── Forms/                  Các form (mỗi form: .cs xử lý + .Designer.cs giao diện)
```

Mỗi form chỉ gọi service của `BookStore.Application`; không viết truy vấn CSDL trong form.
Service kiểm tra lại quyền (`QuyenHan.KiemTraAsync`) nên dù gọi trực tiếp, chức năng ngoài quyền vẫn bị từ chối (ca ST03).
Kiểm tra quyền áp dụng cho mọi thao tác ghi và cả thao tác đọc dữ liệu nhạy cảm (doanh thu, khách hàng, đơn hàng,
lịch sử kho); danh sách sách và tồn kho mọi nhân viên đều xem được (Bảng 3.13, dòng 6).

## Thêm một chức năng mới

1. Viết nghiệp vụ trong `BookStore.Application/Services` (lỗi nghiệp vụ: `throw new BusinessException("...")`),
   đăng ký trong `BookStore.Application/DependencyInjection.cs`.
2. Tạo form trong `Forms` (Add → Form (Windows Forms)), nhận service qua constructor.
   Kiểm tra dữ liệu nhập: tạo `new KiemTraNhap(this).BatBuoc(txtTen, "Tên", 100)...` trong constructor,
   gọi `if (!_kiemTra.HopLe()) return;` đầu nút Lưu và `_kiemTra.XoaLoi()` khi chọn dòng khác.
3. Đăng ký form trong `Program.cs`: `builder.Services.AddTransient<FrmMoi>();`
4. Thêm mục menu trong `FrmChinh` (Designer) và gọi `MoForm<FrmMoi>()`; ẩn/hiện theo vai trò trong `PhanQuyenMenu()`.
5. Viết kiểm thử cho service trong `tests/BookStore.Tests`.
