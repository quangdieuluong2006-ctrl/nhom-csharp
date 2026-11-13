using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>
/// D05 – Quản lý khách hàng (US08). QTV, NV bán hàng: tìm, xem đơn của khách, thêm khách vãng lai, sửa,
/// xóa (khách đã có đơn thì chỉ khóa tài khoản Web), khóa/mở khóa tài khoản Web.
/// </summary>
public partial class FrmKhachHang : Form
{
    private readonly IKhachHangService _svc;
    private readonly IDonHangService _donHang;
    private readonly PhienDangNhap _phien;
    private int _maDangSua;                  // 0 = đang nhập khách mới
    private KhachHangQtDto? _dangChon;
    private bool _dangNap;
    private readonly KiemTraNhap _kiemTra;   // báo lỗi ngay tại ô nhập (ErrorProvider)

    public FrmKhachHang(IKhachHangService svc, IDonHangService donHang, PhienDangNhap phien)
    {
        InitializeComponent();
        _kiemTra = new KiemTraNhap(this)
            .BatBuoc(txtHoTen, "Họ tên", 100)
            .SoDienThoai(txtSoDienThoai, batBuoc: true)
            .Email(txtEmail)
            .ToiDa(txtDiaChi, "Địa chỉ", 255);
        _svc = svc;
        _donHang = donHang;
        _phien = phien;
        GiaoDien.KieuBang(dgvDanhSach);
        GiaoDien.KieuBang(dgvDonCuaKhach);
        Load += async (_, _) => await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private async Task TaiDanhSachAsync(int? chonMa = null)
    {
        _dangNap = true;
        var ds = await _svc.TimKiemAsync(_phien.MaTaiKhoan, txtTimKiem.Text);
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaKhachHang", 25), ("TrangThaiText", 45), ("SoDonHang", 30), ("DiaChi", 140));
        lblTongSo.Text = $"Có {ds.Count} khách hàng";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<KhachHangQtDto>(dgvDanhSach, k => k.MaKhachHang == ma);
        await HienThiDongDangChonAsync();
    }

    private async Task HienThiDongDangChonAsync()
    {
        _kiemTra.XoaLoi();
        if (GiaoDien.DongDangChon<KhachHangQtDto>(dgvDanhSach) is not { } k)
        {
            XoaTrang();
            return;
        }
        _dangChon = k;
        _maDangSua = k.MaKhachHang;
        btnXoa.Enabled = true;
        btnKhoa.Enabled = k.TaiKhoanHoatDong is not null;
        btnKhoa.Text = k.TaiKhoanHoatDong == false ? "Mở khóa tài khoản" : "Khóa tài khoản";
        txtHoTen.Text = k.HoTen;
        txtSoDienThoai.Text = k.SoDienThoai;
        txtEmail.Text = k.Email;
        txtDiaChi.Text = k.DiaChi;
        lblTaiKhoan.Text = k.TaiKhoanHoatDong switch
        {
            null => "Khách vãng lai (không có tài khoản)",
            true => k.TaiKhoan,
            false => $"{k.TaiKhoan} (đã khóa)"
        };
        grpThongTin.Text = $"Khách hàng #{k.MaKhachHang}";

        var dons = await _donHang.TimKiemQuanTriAsync(new LocDonHang { MaKhachHang = k.MaKhachHang }, _phien.MaTaiKhoan);
        GiaoDien.GanDanhSach(dgvDonCuaKhach, dons);
        foreach (var cot in new[] { "KhachHang", "HoTenNhan", "SoDienThoaiNhan", "ThanhToanText" })
            if (dgvDonCuaKhach.Columns[cot] is { } c) c.Visible = false;
        lblDonCuaKhach.Text = $"Đơn hàng của khách ({dons.Count})";
    }

    private async void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
    {
        if (!_dangNap) await GiaoDien.ChayTuanTuAsync(this, HienThiDongDangChonAsync);
    }

    /// <summary>Đưa vùng thông tin về trạng thái nhập khách mới.</summary>
    private void XoaTrang()
    {
        _dangChon = null;
        _maDangSua = 0;
        foreach (var t in new[] { txtHoTen, txtSoDienThoai, txtEmail, txtDiaChi }) t.Clear();
        lblTaiKhoan.Text = "Khách vãng lai (không có tài khoản)";
        grpThongTin.Text = "Thêm khách hàng mới";
        dgvDonCuaKhach.DataSource = null;
        lblDonCuaKhach.Text = "Đơn hàng của khách";
        btnXoa.Enabled = btnKhoa.Enabled = false;
        btnKhoa.Text = "Khóa tài khoản";
        _kiemTra.XoaLoi();
    }

    private void btnThemMoi_Click(object sender, EventArgs e)
    {
        _dangNap = true;
        dgvDanhSach.ClearSelection();
        _dangNap = false;
        XoaTrang();
        txtHoTen.Focus();
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (!_kiemTra.HopLe()) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            var dto = new KhachHangLuuDto
            {
                MaKhachHang = _maDangSua, HoTen = txtHoTen.Text, SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text, DiaChi = txtDiaChi.Text
            };
            int ma;
            if (_maDangSua == 0)
            {
                ma = await _svc.ThemAsync(_phien.MaTaiKhoan, dto);
                ThongBao.ThanhCong("Đã thêm khách hàng (khách vãng lai, chưa có tài khoản Web).");
            }
            else
            {
                await _svc.CapNhatAsync(_phien.MaTaiKhoan, dto);
                ma = _maDangSua;
                ThongBao.ThanhCong("Đã cập nhật thông tin khách hàng. Các đơn đã đặt vẫn giữ thông tin người nhận lúc đặt.");
            }
            await TaiDanhSachAsync(ma);
        });
    }

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        if (_dangChon is not { } k) { ThongBao.CanhBao("Hãy chọn khách hàng cần xóa."); return; }
        if (!ThongBao.XacNhan($"Xóa khách hàng “{k.HoTen}”?\nKhách đã có đơn hàng sẽ không bị xóa để giữ lịch sử đơn; " +
                              "nếu khách có tài khoản Web thì hệ thống khóa tài khoản đó.")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            bool daXoa = await _svc.XoaAsync(_phien.MaTaiKhoan, k.MaKhachHang);
            ThongBao.ThanhCong(daXoa ? "Đã xóa khách hàng."
                : "Khách đã có đơn hàng nên không xóa; tài khoản Web của khách đã bị khóa.");
            await TaiDanhSachAsync(daXoa ? null : k.MaKhachHang);
        });
    }

    private async void btnKhoa_Click(object sender, EventArgs e)
    {
        if (_dangChon is not { TaiKhoanHoatDong: bool dangHoatDong } k) return;
        if (!ThongBao.XacNhan(dangHoatDong
                ? $"Khóa tài khoản Web “{k.TaiKhoan}” của khách {k.HoTen}?\nKhách sẽ bị đăng xuất và không đăng nhập website được nữa."
                : $"Mở khóa tài khoản Web “{k.TaiKhoan}” của khách {k.HoTen}?")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.DatTrangThaiTaiKhoanAsync(_phien.MaTaiKhoan, k.MaKhachHang, !dangHoatDong);
            await TaiDanhSachAsync(k.MaKhachHang);
        });
    }

    private async void btnTim_Click(object sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());

    private async void btnLamMoi_Click(object sender, EventArgs e)
    {
        txtTimKiem.Clear();
        await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        btnTim_Click(sender, e);
    }
}
