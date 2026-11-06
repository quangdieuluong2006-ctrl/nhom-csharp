using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;

namespace BookStore.Desktop.Forms;

/// <summary>D08 – Quản lý tài khoản và phân quyền (UC-D08). Chỉ Quản trị viên.</summary>
public partial class FrmTaiKhoan : Form
{
    private readonly ITaiKhoanService _svc;
    private readonly PhienDangNhap _phien;
    private TaiKhoanQtDto? _dangSua;   // null = đang tạo mới
    private bool _dangNap;

    private static readonly VaiTro[] VaiTroNhanVien = { VaiTro.QuanTriVien, VaiTro.NhanVienBanHang, VaiTro.NhanVienKho };

    public FrmTaiKhoan(ITaiKhoanService svc, PhienDangNhap phien)
    {
        InitializeComponent();
        _svc = svc;
        _phien = phien;
        GiaoDien.KieuBang(dgvDanhSach);

        _dangNap = true;
        GiaoDien.GanCombo(cboLocVaiTro, Enum.GetValues<VaiTro>().Select(v => new MucChonDto((int)v + 1, v.Ten())), "(Tất cả vai trò)");
        GiaoDien.GanCombo(cboVaiTro, VaiTroNhanVien.Select(v => new MucChonDto((int)v + 1, v.Ten())));
        _dangNap = false;
        Load += async (_, _) => await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private async Task TaiDanhSachAsync(int? chonMa = null)
    {
        _dangNap = true;
        VaiTro? loc = GiaoDien.MaDangChon(cboLocVaiTro) is int m ? (VaiTro)(m - 1) : null;
        var ds = await _svc.LayDanhSachAsync(_phien.MaTaiKhoan, txtTimKiem.Text, loc);
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaTaiKhoan", 25));
        lblTongSo.Text = $"Có {ds.Count} tài khoản";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<TaiKhoanQtDto>(dgvDanhSach, t => t.MaTaiKhoan == ma);
        HienThiDongDangChon();
    }

    private void HienThiDongDangChon()
    {
        if (GiaoDien.DongDangChon<TaiKhoanQtDto>(dgvDanhSach) is not { } t) { XoaTrang(); return; }
        _dangSua = t;
        bool laKhach = t.VaiTro == VaiTro.KhachHang;
        txtTenDangNhap.Text = t.TenDangNhap;
        txtTenDangNhap.ReadOnly = true;           // tên đăng nhập không đổi sau khi tạo
        txtHoTen.Text = t.HoTen;
        txtEmail.Text = t.Email;
        txtSoDienThoai.Text = t.SoDienThoai;
        txtMatKhau.Clear();
        txtMatKhau.Enabled = false;
        if (laKhach)
        {
            GiaoDien.GanCombo(cboVaiTro, new[] { new MucChonDto((int)VaiTro.KhachHang + 1, VaiTro.KhachHang.Ten()) });
        }
        else
        {
            GiaoDien.GanCombo(cboVaiTro, VaiTroNhanVien.Select(v => new MucChonDto((int)v + 1, v.Ten())));
            GiaoDien.ChonTheoMa(cboVaiTro, (int)t.VaiTro + 1);
        }
        // Tài khoản khách: thông tin liên hệ sửa ở màn hình Khách hàng; ở đây chỉ khóa/đặt lại mật khẩu
        foreach (Control c in new Control[] { txtHoTen, txtEmail, txtSoDienThoai, cboVaiTro }) c.Enabled = !laKhach;
        btnLuu.Enabled = !laKhach;
        lblTrangThai.Text = t.TrangThaiText;
        lblTrangThai.ForeColor = t.TrangThai ? Color.SeaGreen : Color.Firebrick;
        btnKhoa.Text = t.TrangThai ? "Khóa" : "Mở khóa";
        btnKhoa.Enabled = btnDatLaiMatKhau.Enabled = true;
        grpThongTin.Text = laKhach ? $"Tài khoản khách hàng #{t.MaTaiKhoan}" : $"Sửa tài khoản #{t.MaTaiKhoan}";
    }

    private void XoaTrang()
    {
        _dangSua = null;
        GiaoDien.GanCombo(cboVaiTro, VaiTroNhanVien.Select(v => new MucChonDto((int)v + 1, v.Ten())));
        GiaoDien.ChonTheoMa(cboVaiTro, (int)VaiTro.NhanVienBanHang + 1);
        foreach (var t in new[] { txtTenDangNhap, txtHoTen, txtEmail, txtSoDienThoai, txtMatKhau }) { t.Clear(); t.Enabled = true; }
        txtTenDangNhap.ReadOnly = false;
        cboVaiTro.Enabled = btnLuu.Enabled = true;
        lblTrangThai.Text = "Tài khoản mới";
        lblTrangThai.ForeColor = Color.DimGray;
        btnKhoa.Enabled = btnDatLaiMatKhau.Enabled = false;
        grpThongTin.Text = "Tạo tài khoản nhân viên";
        txtTenDangNhap.Focus();
    }

    private void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
    {
        if (!_dangNap) HienThiDongDangChon();
    }

    private void btnThemMoi_Click(object sender, EventArgs e)
    {
        dgvDanhSach.ClearSelection();
        XoaTrang();
    }

    private async void btnLuu_Click(object sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, async () =>
    {
        var dto = new TaiKhoanLuuDto
        {
            MaTaiKhoan = _dangSua?.MaTaiKhoan ?? 0, TenDangNhap = txtTenDangNhap.Text, HoTen = txtHoTen.Text,
            Email = txtEmail.Text, SoDienThoai = txtSoDienThoai.Text,
            VaiTro = (VaiTro)((GiaoDien.MaDangChon(cboVaiTro) ?? 1) - 1), MatKhau = txtMatKhau.Text
        };
        int ma;
        if (_dangSua is null)
        {
            ma = await _svc.TaoTaiKhoanNhanVienAsync(_phien.MaTaiKhoan, dto);
            ThongBao.ThanhCong($"Đã tạo tài khoản “{dto.TenDangNhap}”.");
        }
        else
        {
            await _svc.CapNhatTaiKhoanAsync(_phien.MaTaiKhoan, dto);
            ma = dto.MaTaiKhoan;
            ThongBao.ThanhCong("Đã cập nhật tài khoản.");
        }
        await TaiDanhSachAsync(ma);
    });

    private async void btnKhoa_Click(object sender, EventArgs e)
    {
        if (_dangSua is not { } t) return;
        bool moKhoa = !t.TrangThai;
        if (!ThongBao.XacNhan(moKhoa ? $"Mở khóa tài khoản “{t.TenDangNhap}”?"
                : $"Khóa tài khoản “{t.TenDangNhap}”?\nTài khoản bị khóa sẽ không đăng nhập được Web và Desktop.")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.DatTrangThaiAsync(_phien.MaTaiKhoan, t.MaTaiKhoan, moKhoa);
            await TaiDanhSachAsync(t.MaTaiKhoan);
        });
    }

    private async void btnDatLaiMatKhau_Click(object sender, EventArgs e)
    {
        if (_dangSua is not { } t) return;
        var mk = HopThoai.NhapChuoi(this, "Đặt lại mật khẩu", $"Mật khẩu mới cho “{t.TenDangNhap}” (ít nhất 6 ký tự):", laMatKhau: true, 100);
        if (mk is null) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.DatLaiMatKhauAsync(_phien.MaTaiKhoan, t.MaTaiKhoan, mk);
            ThongBao.ThanhCong("Đã đặt lại mật khẩu. Hãy báo mật khẩu mới cho người dùng.");
        });
    }

    private async void btnTim_Click(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private async void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        _dangNap = true;
        cboLocVaiTro.SelectedIndex = 0;
        _dangNap = false;
        await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        btnTim_Click(sender, e);
    }
}
