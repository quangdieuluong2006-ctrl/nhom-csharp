# Hướng dẫn thêm chức năng vào phần mềm quản trị (BookStore.Desktop)

Khung Desktop đã có sẵn: đăng nhập, form chính (MDI) với menu theo vai trò, kết nối CSDL, ghi log.
Mỗi thành viên chỉ cần **thêm form của mình** và **gắn vào menu**.

## Cấu trúc

```
src/BookStore.Desktop
├── Program.cs              Cấu hình DI, đăng ký service và form, vòng đăng nhập/đăng xuất
├── appsettings.json        Chuỗi kết nối CSDL (sửa cho đúng máy của bạn)
├── HaTang/
│   ├── PhienDangNhap.cs    Người đang đăng nhập, kiểm tra quyền: _phien.CoQuyen(VaiTro.QuanTriVien)
│   ├── FormFactory.cs      Tạo form qua DI, mỗi form một DbContext riêng
│   ├── ThongBao.cs         ThongBao.ThanhCong / Loi / CanhBao / XacNhan
│   └── XuLyLoi.cs          Bắt lỗi toàn cục, ghi log logs/desktop-*.log
└── Forms/
    ├── FrmDangNhap         Đăng nhập (chỉ nhân viên, quản trị viên)
    └── FrmChinh            Form chính: menu + vùng MDI
```

## Các bước thêm một chức năng (ví dụ: Quản lý sách)

**1. Viết nghiệp vụ trong `BookStore.Application`** (không viết truy vấn CSDL trong form):

```csharp
// src/BookStore.Application/Services/QuanLySachService.cs
public interface IQuanLySachService
{
    Task<List<Sach>> LayDanhSachAsync(string? tuKhoa);
    Task ThemAsync(Sach sach);
}
public class QuanLySachService : IQuanLySachService
{
    private readonly IAppDbContext _db;
    public QuanLySachService(IAppDbContext db) => _db = db;
    // ... dùng _db.Sachs, _db.SaveChangesAsync(); lỗi nghiệp vụ thì throw new BusinessException("...")
}
```

**2. Tạo form** trong Visual Studio: chuột phải thư mục `Forms` → Add → Form (Windows Forms) → `FrmQuanLySach`.
Nhận service qua constructor:

```csharp
public partial class FrmQuanLySach : Form
{
    private readonly IQuanLySachService _svc;
    public FrmQuanLySach(IQuanLySachService svc)
    {
        InitializeComponent();
        _svc = svc;
    }

    private async void FrmQuanLySach_Load(object sender, EventArgs e)
    {
        dgvSach.DataSource = await _svc.LayDanhSachAsync(null);
    }
}
```

**3. Đăng ký trong `Program.cs`:**

```csharp
builder.Services.AddScoped<IQuanLySachService, QuanLySachService>();
builder.Services.AddTransient<FrmQuanLySach>();
```

**4. Gắn vào menu** trong `Forms/FrmChinh.cs`: thay dòng `ChuaHienThuc(...)` của mục menu tương ứng:

```csharp
private void mnuSach_Click(object sender, EventArgs e) => MoForm<FrmQuanLySach>();
```

## Quy ước

- Tên form: `Frm` + tên chức năng (FrmQuanLySach, FrmPhieuKho, FrmXuLyDonHang...).
- Tên control: `txt`, `btn`, `dgv`, `cbo`, `lbl`, `nud`, `dtp` + tên (txtTenSach, btnLuu, dgvSach).
- Lỗi do người dùng nhập sai / vi phạm nghiệp vụ: `throw new BusinessException("thông báo")` trong service.
  Lỗi này được hiện thành hộp thoại cảnh báo; lỗi khác được ghi log tự động.
- Thao tác nhiều bảng (phiếu kho, chuyển trạng thái đơn + hoàn kho): dùng `_db.ThucThiTrongGiaoTacAsync(...)`,
  xem mẫu trong `DonHangService.DatHangAsync` / `HuyDonAsync`.
- Cần biết ai đang thao tác (ví dụ người lập phiếu kho): nhận `PhienDangNhap` qua constructor, dùng `_phien.MaTaiKhoan`.

## Menu và vai trò hiện tại

| Menu | Mục | Vai trò thấy menu |
|---|---|---|
| Quản trị | Tài khoản nhân viên | Quản trị viên |
| Danh mục | Sách, Danh mục sách, Nhà xuất bản, Tác giả | Quản trị viên, Nhân viên kho |
| Kho | Lập phiếu kho, Tồn kho | Quản trị viên, Nhân viên kho |
| Đơn hàng | Xử lý đơn hàng | Quản trị viên, Nhân viên bán hàng |
| Báo cáo | Doanh thu, Sách bán chạy | Quản trị viên, Nhân viên bán hàng |

Nếu phân quyền trong báo cáo khác bảng này, sửa trong `FrmChinh.PhanQuyenMenu()`.
