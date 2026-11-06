using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>D04 – Quản lý tác giả. Chỉ Quản trị viên.</summary>
public partial class FrmTacGia : Form
{
    private readonly IDanhMucService _svc;
    private readonly PhienDangNhap _phien;
    private int _maDangSua;
    private bool _dangNap;

    public FrmTacGia(IDanhMucService svc, PhienDangNhap phien)
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
        var ds = await _svc.LayDanhSachTacGiaAsync(txtTimKiem.Text);
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaTacGia", 25), ("TieuSu", 160), ("SoSach", 30), ("TrangThaiText", 40));
        lblTongSo.Text = $"Có {ds.Count} tác giả";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<TacGiaQtDto>(dgvDanhSach, t => t.MaTacGia == ma);
        HienThiDongDangChon();
    }

    private void HienThiDongDangChon()
    {
        if (GiaoDien.DongDangChon<TacGiaQtDto>(dgvDanhSach) is not { } t) { XoaTrang(); return; }
        _maDangSua = t.MaTacGia;
        txtTenTacGia.Text = t.TenTacGia;
        txtTieuSu.Text = t.TieuSu;
        chkDangDung.Checked = t.TrangThai;
        grpThongTin.Text = $"Sửa tác giả #{t.MaTacGia}";
    }

    private void XoaTrang()
    {
        _maDangSua = 0;
        txtTenTacGia.Clear();
        txtTieuSu.Clear();
        chkDangDung.Checked = true;
        grpThongTin.Text = "Thêm tác giả mới";
        txtTenTacGia.Focus();
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
        int ma = await _svc.LuuTacGiaAsync(_phien.MaTaiKhoan, new TacGiaLuuDto
        {
            MaTacGia = _maDangSua, TenTacGia = txtTenTacGia.Text, TieuSu = txtTieuSu.Text, TrangThai = chkDangDung.Checked
        });
        ThongBao.ThanhCong(_maDangSua == 0 ? "Đã thêm tác giả." : "Đã cập nhật tác giả.");
        await TaiDanhSachAsync(ma);
    });

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        if (_maDangSua == 0) { ThongBao.CanhBao("Hãy chọn tác giả cần xóa."); return; }
        if (!ThongBao.XacNhan($"Xóa tác giả “{txtTenTacGia.Text}”?\nNếu tác giả đã có sách, hệ thống chỉ chuyển sang “Ngừng dùng”.")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            bool daXoa = await _svc.XoaTacGiaAsync(_phien.MaTaiKhoan, _maDangSua);
            ThongBao.ThanhCong(daXoa ? "Đã xóa tác giả." : "Tác giả đã có sách nên đã chuyển sang “Ngừng dùng”.");
            await TaiDanhSachAsync(daXoa ? null : _maDangSua);
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
