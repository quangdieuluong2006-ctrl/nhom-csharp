using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;

namespace BookStore.Desktop.Forms;

/// <summary>
/// Form chính (MDI): menu chức năng hiển thị theo vai trò, các form nghiệp vụ mở bên trong.
///
/// CÁCH THÊM CHỨC NĂNG (xem thêm docs/HUONG_DAN_DESKTOP.md):
///   1. Tạo form mới trong thư mục Forms, ví dụ FrmQuanLySach (nhận service qua constructor).
///   2. Đăng ký trong Program.cs:  builder.Services.AddTransient&lt;FrmQuanLySach&gt;();
///   3. Trong handler của mục menu tương ứng bên dưới, thay ChuaHienThuc(...) bằng  MoForm&lt;FrmQuanLySach&gt;();
/// </summary>
public partial class FrmChinh : Form
{
    private readonly IFormFactory _forms;
    private readonly PhienDangNhap _phien;

    /// <summary>true nếu người dùng chọn Đăng xuất (Program sẽ hiện lại màn hình đăng nhập).</summary>
    public bool DaDangXuat { get; private set; }

    public FrmChinh(IFormFactory forms, PhienDangNhap phien)
    {
        InitializeComponent();
        _forms = forms;
        _phien = phien;
    }

    private void FrmChinh_Load(object sender, EventArgs e)
    {
        var nd = _phien.NguoiDung!;
        lblNguoiDung.Text = $"Người dùng: {nd.HoTen} ({nd.TenDangNhap})";
        lblVaiTro.Text = $"Vai trò: {PhienDangNhap.TenVaiTro(nd.VaiTro)}";
        lblChao.Text = $"Xin chào {nd.HoTen}!";
        PhanQuyenMenu();
    }

    /// <summary>
    /// Ẩn menu không thuộc quyền của vai trò hiện tại (theo bảng phân quyền trong báo cáo).
    /// Lưu ý: ẩn menu chỉ là lớp bảo vệ giao diện; service vẫn nên kiểm tra lại quyền khi cần.
    /// </summary>
    private void PhanQuyenMenu()
    {
        mnuQuanTri.Visible = _phien.CoQuyen(VaiTro.QuanTriVien);
        mnuDanhMuc.Visible = _phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienKho);
        mnuKho.Visible = _phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienKho);
        mnuDonHang.Visible = _phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienBanHang);
        mnuBaoCao.Visible = _phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienBanHang);
    }

    /// <summary>Mở form con trong vùng MDI; nếu form đó đang mở thì chỉ đưa lên trước, không mở trùng.</summary>
    private void MoForm<T>() where T : Form
    {
        var dangMo = MdiChildren.OfType<T>().FirstOrDefault();
        if (dangMo != null)
        {
            dangMo.Activate();
            return;
        }
        var f = _forms.Tao<T>();
        f.MdiParent = this;
        f.WindowState = FormWindowState.Maximized;
        f.Show();
    }

    private static void ChuaHienThuc(string chucNang) =>
        ThongBao.ThanhCong($"Chức năng “{chucNang}” đang được phát triển.");

    // ---------- Hệ thống ----------
    private void mnuDoiMatKhau_Click(object sender, EventArgs e) => ChuaHienThuc("Đổi mật khẩu");

    private void mnuDangXuat_Click(object sender, EventArgs e)
    {
        if (!ThongBao.XacNhan("Bạn muốn đăng xuất?")) return;
        DaDangXuat = true;
        Close();
    }

    private void mnuThoat_Click(object sender, EventArgs e) => Close();

    // ---------- Quản trị (Quản trị viên) ----------
    private void mnuTaiKhoan_Click(object sender, EventArgs e) => ChuaHienThuc("Quản lý tài khoản nhân viên");

    // ---------- Danh mục ----------
    private void mnuDanhMucSach_Click(object sender, EventArgs e) => ChuaHienThuc("Danh mục sách");
    private void mnuNhaXuatBan_Click(object sender, EventArgs e) => ChuaHienThuc("Nhà xuất bản");
    private void mnuTacGia_Click(object sender, EventArgs e) => ChuaHienThuc("Tác giả");
    private void mnuSach_Click(object sender, EventArgs e) => ChuaHienThuc("Quản lý sách");

    // ---------- Kho ----------
    private void mnuPhieuKho_Click(object sender, EventArgs e) => ChuaHienThuc("Lập phiếu nhập/xuất kho");
    private void mnuTonKho_Click(object sender, EventArgs e) => ChuaHienThuc("Tồn kho");

    // ---------- Đơn hàng ----------
    private void mnuXuLyDon_Click(object sender, EventArgs e) => ChuaHienThuc("Xử lý đơn hàng");

    // ---------- Báo cáo ----------
    private void mnuDoanhThu_Click(object sender, EventArgs e) => ChuaHienThuc("Báo cáo doanh thu");
    private void mnuSachBanChay_Click(object sender, EventArgs e) => ChuaHienThuc("Sách bán chạy");

    // ---------- Cửa sổ ----------
    private void mnuXepCuaSo_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);

    private void mnuDongTatCa_Click(object sender, EventArgs e)
    {
        foreach (var f in MdiChildren) f.Close();
    }
}
