using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.Forms;

/// <summary>
/// D02 – Form chính (MDI): menu hiển thị theo vai trò (ma trận chức năng – Bảng 3.13), các form nghiệp vụ mở bên trong.
/// Thanh trạng thái hiển thị số đơn chờ xác nhận và báo khi có đơn mới từ website (kiểm tra mỗi 30 giây).
/// </summary>
public partial class FrmChinh : Form
{
    private readonly IFormFactory _forms;
    private readonly PhienDangNhap _phien;
    private readonly IServiceScopeFactory _scopes;
    private readonly ILogger<FrmChinh> _log;
    private int? _soDonChoTruoc;

    /// <summary>true nếu người dùng chọn Đăng xuất (Program sẽ hiện lại màn hình đăng nhập).</summary>
    public bool DaDangXuat { get; private set; }

    public FrmChinh(IFormFactory forms, PhienDangNhap phien, IServiceScopeFactory scopes, ILogger<FrmChinh> log)
    {
        InitializeComponent();
        _forms = forms;
        _phien = phien;
        _scopes = scopes;
        _log = log;
    }

    private bool LaQuanTri => _phien.CoQuyen(VaiTro.QuanTriVien);
    private bool LaBanHang => _phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienBanHang);
    private bool LaKho => _phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienKho);

    private async void FrmChinh_Load(object sender, EventArgs e)
    {
        var nd = _phien.NguoiDung!;
        lblNguoiDung.Text = $"Người dùng: {nd.HoTen} ({nd.TenDangNhap})";
        lblVaiTro.Text = $"Vai trò: {nd.VaiTro.Ten()}";
        Text = $"BookStore – Phần mềm quản trị – {nd.HoTen}";
        PhanQuyenMenu();
        MoTongQuan();
        if (LaBanHang)
        {
            await KiemTraDonMoiAsync();
            tmrKiemTraDon.Start();
        }
    }

    /// <summary>
    /// Ẩn chức năng không thuộc quyền của vai trò hiện tại (Bảng 3.13). Đây là lớp bảo vệ ở giao diện;
    /// các service ở tầng nghiệp vụ vẫn kiểm tra lại quyền (QuyenHan.KiemTraAsync) – ca kiểm thử ST03.
    /// </summary>
    private void PhanQuyenMenu()
    {
        // Danh mục: mọi nhân viên xem được danh sách sách; chỉ quản trị viên quản lý danh mục, NXB, tác giả
        mnuDanhMucSach.Visible = mnuNhaXuatBan.Visible = mnuTacGia.Visible = sep2.Visible = LaQuanTri;
        mnuBanHang.Visible = LaBanHang;
        mnuKho.Visible = LaKho;
        mnuBaoCao.Visible = LaBanHang;
        mnuQuanTri.Visible = LaQuanTri;
    }

    /// <summary>Mở form con trong vùng MDI; nếu form đó đang mở thì chỉ đưa lên trước, không mở trùng.</summary>
    private T MoForm<T>() where T : Form
    {
        var dangMo = MdiChildren.OfType<T>().FirstOrDefault();
        if (dangMo != null)
        {
            dangMo.Activate();
            return dangMo;
        }
        var f = _forms.Tao<T>();
        f.MdiParent = this;
        f.WindowState = FormWindowState.Maximized;
        f.Show();
        return f;
    }

    private void MoTongQuan()
    {
        bool moiTao = !MdiChildren.OfType<FrmTongQuan>().Any();
        var f = MoForm<FrmTongQuan>();
        if (moiTao) f.YeuCauMoDon += MoDonHangAsync;
    }

    private async Task MoDonHangAsync(long maDon)
    {
        if (!LaBanHang) return;
        await MoForm<FrmDonHang>().ChonDonAsync(maDon);
    }

    // ------------------------- Báo đơn mới từ website -------------------------

    private async Task KiemTraDonMoiAsync()
    {
        using var scope = _scopes.CreateScope();
        var tq = await scope.ServiceProvider.GetRequiredService<IBaoCaoService>().TongQuanAsync();
        int soDon = tq.DonChoXacNhan;
        lblDonCho.Text = soDon > 0 ? $"🔔 {soDon} đơn chờ xác nhận – nhấp để xử lý" : "Không có đơn chờ xác nhận";
        lblDonCho.ForeColor = soDon > 0 ? Color.Firebrick : Color.DimGray;
        if (_soDonChoTruoc is int truoc && soDon > truoc)
        {
            System.Media.SystemSounds.Asterisk.Play();
            lblDonCho.Text = $"🔔 Có {soDon - truoc} đơn MỚI từ website · {soDon} đơn chờ xác nhận – nhấp để xử lý";
        }
        _soDonChoTruoc = soDon;
    }

    private async void tmrKiemTraDon_Tick(object? sender, EventArgs e)
    {
        try { await KiemTraDonMoiAsync(); }
        catch (Exception ex) { _log.LogWarning(ex, "Kiểm tra đơn mới thất bại"); }
    }

    private void lblDonCho_Click(object? sender, EventArgs e)
    {
        if (LaBanHang) MoForm<FrmDonHang>();
    }

    // ---------------------------------- Hệ thống ----------------------------------
    private void mnuTongQuan_Click(object sender, EventArgs e) => MoTongQuan();

    private void mnuDoiMatKhau_Click(object sender, EventArgs e)
    {
        using var f = _forms.Tao<FrmDoiMatKhau>();
        f.ShowDialog(this);
    }

    private void mnuDangXuat_Click(object sender, EventArgs e)
    {
        if (!ThongBao.XacNhan("Bạn muốn đăng xuất?")) return;
        DaDangXuat = true;
        Close();
    }

    private void mnuThoat_Click(object sender, EventArgs e) => Close();

    // ---------------------------------- Danh mục ----------------------------------
    private void mnuSach_Click(object sender, EventArgs e) => MoForm<FrmSach>();
    private void mnuDanhMucSach_Click(object sender, EventArgs e) => MoForm<FrmDanhMuc>();
    private void mnuNhaXuatBan_Click(object sender, EventArgs e) => MoForm<FrmNhaXuatBan>();
    private void mnuTacGia_Click(object sender, EventArgs e) => MoForm<FrmTacGia>();

    // ---------------------------------- Bán hàng ----------------------------------
    private void mnuDonHang_Click(object sender, EventArgs e) => MoForm<FrmDonHang>();
    private void mnuKhachHang_Click(object sender, EventArgs e) => MoForm<FrmKhachHang>();

    // ------------------------------------ Kho ------------------------------------
    private void mnuPhieuKho_Click(object sender, EventArgs e) => MoForm<FrmPhieuKho>();
    private void mnuTonKho_Click(object sender, EventArgs e) => MoForm<FrmKho>();

    // ----------------------------- Báo cáo, Quản trị -----------------------------
    private void mnuDoanhThu_Click(object sender, EventArgs e) => MoForm<FrmBaoCao>();
    private void mnuTaiKhoan_Click(object sender, EventArgs e) => MoForm<FrmTaiKhoan>();

    // ---------------------------------- Cửa sổ ----------------------------------
    private void mnuXepCuaSo_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);

    private void mnuDongTatCa_Click(object sender, EventArgs e)
    {
        foreach (var f in MdiChildren) f.Close();
    }
}
