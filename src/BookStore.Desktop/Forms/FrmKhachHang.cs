using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>D05 – Tra cứu, cập nhật thông tin khách hàng và xem các đơn của khách (US08). QTV, NV bán hàng.</summary>
public partial class FrmKhachHang : Form
{
    private readonly IKhachHangService _svc;
    private readonly IDonHangService _donHang;
    private readonly PhienDangNhap _phien;
    private int _maDangSua;
    private bool _dangNap;

    public FrmKhachHang(IKhachHangService svc, IDonHangService donHang, PhienDangNhap phien)
    {
        InitializeComponent();
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
        var ds = await _svc.TimKiemAsync(txtTimKiem.Text);
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaKhachHang", 25), ("SoDonHang", 30), ("DiaChi", 140));
        lblTongSo.Text = $"Có {ds.Count} khách hàng";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<KhachHangQtDto>(dgvDanhSach, k => k.MaKhachHang == ma);
        await HienThiDongDangChonAsync();
    }

    private async Task HienThiDongDangChonAsync()
    {
        if (GiaoDien.DongDangChon<KhachHangQtDto>(dgvDanhSach) is not { } k)
        {
            _maDangSua = 0;
            foreach (var t in new[] { txtHoTen, txtSoDienThoai, txtEmail, txtDiaChi }) t.Clear();
            lblTaiKhoan.Text = "";
            dgvDonCuaKhach.DataSource = null;
            return;
        }
        _maDangSua = k.MaKhachHang;
        txtHoTen.Text = k.HoTen;
        txtSoDienThoai.Text = k.SoDienThoai;
        txtEmail.Text = k.Email;
        txtDiaChi.Text = k.DiaChi;
        lblTaiKhoan.Text = k.TaiKhoan == "—" ? "Khách vãng lai (không có tài khoản)" : k.TaiKhoan;
        grpThongTin.Text = $"Khách hàng #{k.MaKhachHang}";

        var dons = await _donHang.TimKiemQuanTriAsync(new LocDonHang { MaKhachHang = k.MaKhachHang });
        GiaoDien.GanDanhSach(dgvDonCuaKhach, dons);
        foreach (var cot in new[] { "KhachHang", "HoTenNhan", "SoDienThoaiNhan", "ThanhToanText" })
            if (dgvDonCuaKhach.Columns[cot] is { } c) c.Visible = false;
        lblDonCuaKhach.Text = $"Đơn hàng của khách ({dons.Count})";
    }

    private async void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
    {
        if (!_dangNap) await GiaoDien.ChayTuanTuAsync(this, HienThiDongDangChonAsync);
    }

    private async void btnLuu_Click(object sender, EventArgs e)
    {
        if (_maDangSua == 0) { ThongBao.CanhBao("Hãy chọn khách hàng."); return; }
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.CapNhatAsync(_phien.MaTaiKhoan, new KhachHangLuuDto
            {
                MaKhachHang = _maDangSua, HoTen = txtHoTen.Text, SoDienThoai = txtSoDienThoai.Text,
                Email = txtEmail.Text, DiaChi = txtDiaChi.Text
            });
            ThongBao.ThanhCong("Đã cập nhật thông tin khách hàng. Các đơn đã đặt vẫn giữ thông tin người nhận lúc đặt.");
            await TaiDanhSachAsync(_maDangSua);
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
