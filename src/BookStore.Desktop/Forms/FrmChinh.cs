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
        ChuDe.KieuMenu(menuStrip, statusStrip);
        TaoThanhDieuHuong();
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

    // ------------------------------ Thanh điều hướng bên trái ------------------------------

    private Button? _nutDangChon;

    /// <summary>
    /// Thanh điều hướng dựng từ chính menu: chỉ có nút cho mục menu mà vai trò hiện tại được thấy,
    /// nên phân quyền luôn khớp với menu (PhanQuyenMenu) mà không phải viết lặp.
    /// </summary>
    private void TaoThanhDieuHuong()
    {
        var thanh = new Panel { Dock = DockStyle.Left, Width = 230, BackColor = ChuDe.XanhRatDam, Name = "pnlDieuHuong" };
        var ds = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoScroll = true,
            Padding = new Padding(10, 6, 10, 10), BackColor = ChuDe.XanhRatDam
        };
        var logo = new Label
        {
            Dock = DockStyle.Top, Height = 64, Text = "BookStore", ForeColor = ChuDe.Vang,
            Font = new Font("Segoe UI Semibold", 15F), TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(16, 0, 0, 0)
        };
        var duoi = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, FlowDirection = FlowDirection.TopDown, WrapContents = false, AutoSize = true,
            Padding = new Padding(10, 6, 10, 10), BackColor = ChuDe.XanhRatDam
        };

        ds.Controls.Add(TaoNut("Tổng quan", mnuTongQuan));
        foreach (var nhom in new[] { mnuDanhMuc, mnuBanHang, mnuKho, mnuBaoCao, mnuQuanTri })
        {
            var cacMuc = nhom.DropDownItems.OfType<ToolStripMenuItem>().Where(m => m.Available).ToList();
            if (!nhom.Available || cacMuc.Count == 0) continue;
            ds.Controls.Add(new Label
            {
                Text = (nhom.Text ?? "").Replace("&", "").ToUpper(), AutoSize = false, Width = 200, Height = 30,
                ForeColor = Color.FromArgb(150, 170, 190), Font = new Font("Segoe UI Semibold", 8.25F),
                TextAlign = ContentAlignment.BottomLeft, Margin = new Padding(6, 10, 0, 2)
            });
            foreach (var muc in cacMuc) ds.Controls.Add(TaoNut(muc.Text ?? "", muc));
        }
        duoi.Controls.Add(TaoNut("Đổi mật khẩu", mnuDoiMatKhau, chonDuoc: false));
        duoi.Controls.Add(TaoNut("Đăng xuất", mnuDangXuat, chonDuoc: false));

        thanh.Controls.Add(ds);
        thanh.Controls.Add(duoi);
        thanh.Controls.Add(logo);
        Controls.Add(thanh);
        // Thứ tự neo: menu trên cùng → thanh trạng thái dưới cùng → thanh điều hướng bên trái → vùng MDI
        thanh.SendToBack();
        statusStrip.SendToBack();
        menuStrip.SendToBack();
    }

    private Button TaoNut(string chu, ToolStripMenuItem muc, bool chonDuoc = true)
    {
        var nut = new Button
        {
            Text = chu, Width = 200, Height = 38, FlatStyle = FlatStyle.Flat, TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.White, BackColor = ChuDe.XanhRatDam, Cursor = Cursors.Hand, Font = new Font("Segoe UI", 10F),
            Padding = new Padding(12, 0, 0, 0), Margin = new Padding(0, 1, 0, 1), UseVisualStyleBackColor = false
        };
        nut.FlatAppearance.BorderSize = 0;
        nut.FlatAppearance.MouseOverBackColor = ChuDe.XanhNhat;
        nut.FlatAppearance.MouseDownBackColor = ChuDe.XanhDam;
        if (muc.ShortcutKeys != Keys.None) nut.Text += $"   ({muc.ShortcutKeys})";
        nut.Click += (_, _) =>
        {
            if (chonDuoc) DanhDauNut(nut);
            muc.PerformClick();
        };
        return nut;
    }

    private void DanhDauNut(Button nut)
    {
        if (_nutDangChon != null)
        {
            _nutDangChon.BackColor = ChuDe.XanhRatDam;
            _nutDangChon.ForeColor = Color.White;
        }
        nut.BackColor = ChuDe.XanhDam;
        nut.ForeColor = ChuDe.Vang;
        _nutDangChon = nut;
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
        var tq = await scope.ServiceProvider.GetRequiredService<IBaoCaoService>().TongQuanAsync(_phien.MaTaiKhoan);
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
