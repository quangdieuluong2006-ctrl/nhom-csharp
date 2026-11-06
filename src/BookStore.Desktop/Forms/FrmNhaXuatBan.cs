using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>D04 – Quản lý nhà xuất bản. Chỉ Quản trị viên.</summary>
public partial class FrmNhaXuatBan : Form
{
    private readonly IDanhMucService _svc;
    private readonly PhienDangNhap _phien;
    private int _maDangSua;
    private bool _dangNap;

    public FrmNhaXuatBan(IDanhMucService svc, PhienDangNhap phien)
    {
        InitializeComponent();
        _svc = svc;
        _phien = phien;
        GiaoDien.KieuBang(dgvDanhSach);
        Load += async (_, _) => await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private async Task TaiDanhSachAsync(int? chonMa = null)
    {
        _dangNap = true;
        var ds = await _svc.LayDanhSachNhaXuatBanAsync(txtTimKiem.Text);
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaNhaXuatBan", 30), ("SoSach", 35));
        lblTongSo.Text = $"Có {ds.Count} nhà xuất bản";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<NhaXuatBanQtDto>(dgvDanhSach, n => n.MaNhaXuatBan == ma);
        HienThiDongDangChon();
    }

    private void HienThiDongDangChon()
    {
        if (GiaoDien.DongDangChon<NhaXuatBanQtDto>(dgvDanhSach) is not { } n) { XoaTrang(); return; }
        _maDangSua = n.MaNhaXuatBan;
        txtTenNhaXuatBan.Text = n.TenNhaXuatBan;
        txtDiaChi.Text = n.DiaChi;
        txtSoDienThoai.Text = n.SoDienThoai;
        txtEmail.Text = n.Email;
        grpThongTin.Text = $"Sửa nhà xuất bản #{n.MaNhaXuatBan}";
    }

    private void XoaTrang()
    {
        _maDangSua = 0;
        txtTenNhaXuatBan.Clear();
        txtDiaChi.Clear();
        txtSoDienThoai.Clear();
        txtEmail.Clear();
        grpThongTin.Text = "Thêm nhà xuất bản mới";
        txtTenNhaXuatBan.Focus();
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
        int ma = await _svc.LuuNhaXuatBanAsync(_phien.MaTaiKhoan, new NhaXuatBanLuuDto
        {
            MaNhaXuatBan = _maDangSua, TenNhaXuatBan = txtTenNhaXuatBan.Text, DiaChi = txtDiaChi.Text,
            SoDienThoai = txtSoDienThoai.Text, Email = txtEmail.Text
        });
        ThongBao.ThanhCong(_maDangSua == 0 ? "Đã thêm nhà xuất bản." : "Đã cập nhật nhà xuất bản.");
        await TaiDanhSachAsync(ma);
    });

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        if (_maDangSua == 0) { ThongBao.CanhBao("Hãy chọn nhà xuất bản cần xóa."); return; }
        if (!ThongBao.XacNhan($"Xóa nhà xuất bản “{txtTenNhaXuatBan.Text}”?")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.XoaNhaXuatBanAsync(_phien.MaTaiKhoan, _maDangSua);
            ThongBao.ThanhCong("Đã xóa nhà xuất bản.");
            await TaiDanhSachAsync();
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
