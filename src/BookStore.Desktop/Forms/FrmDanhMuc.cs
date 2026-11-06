using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>D04 – Quản lý danh mục sách (cây 2 cấp). Chỉ Quản trị viên.</summary>
public partial class FrmDanhMuc : Form
{
    private readonly IDanhMucService _svc;
    private readonly PhienDangNhap _phien;
    private int _maDangSua;      // 0 = đang thêm mới
    private bool _dangNap;       // chặn SelectionChanged trong lúc nạp lưới

    public FrmDanhMuc(IDanhMucService svc, PhienDangNhap phien)
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
        var ds = await _svc.LayDanhSachDanhMucAsync(txtTimKiem.Text);
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaDanhMuc", 30), ("SoSach", 35), ("TrangThaiText", 45));
        GiaoDien.GanCombo(cboDanhMucCha, await _svc.LayDanhMucGocAsync(), "(Không có – danh mục gốc)");
        lblTongSo.Text = $"Có {ds.Count} danh mục";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<DanhMucQtDto>(dgvDanhSach, d => d.MaDanhMuc == ma);
        HienThiDongDangChon();
    }

    private void HienThiDongDangChon()
    {
        if (GiaoDien.DongDangChon<DanhMucQtDto>(dgvDanhSach) is not { } d) { XoaTrang(); return; }
        _maDangSua = d.MaDanhMuc;
        txtTenDanhMuc.Text = d.TenDanhMuc;
        GiaoDien.ChonTheoMa(cboDanhMucCha, d.MaDanhMucCha);
        txtMoTa.Text = d.MoTa;
        chkDangDung.Checked = d.TrangThai;
        grpThongTin.Text = $"Sửa danh mục #{d.MaDanhMuc}";
    }

    private void XoaTrang()
    {
        _maDangSua = 0;
        txtTenDanhMuc.Clear();
        GiaoDien.ChonTheoMa(cboDanhMucCha, null);
        txtMoTa.Clear();
        chkDangDung.Checked = true;
        grpThongTin.Text = "Thêm danh mục mới";
        txtTenDanhMuc.Focus();
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
        var dto = new DanhMucLuuDto
        {
            MaDanhMuc = _maDangSua, TenDanhMuc = txtTenDanhMuc.Text, MaDanhMucCha = GiaoDien.MaDangChon(cboDanhMucCha),
            MoTa = txtMoTa.Text, TrangThai = chkDangDung.Checked
        };
        int ma = await _svc.LuuDanhMucAsync(_phien.MaTaiKhoan, dto);
        ThongBao.ThanhCong(_maDangSua == 0 ? "Đã thêm danh mục." : "Đã cập nhật danh mục.");
        await TaiDanhSachAsync(ma);
    });

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        if (_maDangSua == 0) { ThongBao.CanhBao("Hãy chọn danh mục cần xóa."); return; }
        if (!ThongBao.XacNhan($"Xóa danh mục “{txtTenDanhMuc.Text}”?\nNếu danh mục đang có sách hoặc danh mục con, hệ thống chỉ chuyển sang “Ngừng dùng”.")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            bool daXoa = await _svc.XoaDanhMucAsync(_phien.MaTaiKhoan, _maDangSua);
            ThongBao.ThanhCong(daXoa ? "Đã xóa danh mục." : "Danh mục đang được sử dụng nên đã chuyển sang “Ngừng dùng”.");
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
