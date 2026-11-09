using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;

namespace BookStore.Desktop.Forms;

/// <summary>
/// D03 – Quản lý sách. Quản trị viên: thêm, sửa, ngừng kinh doanh (US06). NV bán hàng, NV kho: chỉ xem (Bảng 3.13).
/// Giá, tên, ảnh sửa ở đây được Web hiển thị ngay ở lần tải trang kế tiếp (dùng chung CSDL – ca kiểm thử ST10).
/// </summary>
public partial class FrmSach : Form
{
    private readonly ISachService _sach;
    private readonly IDanhMucService _danhMuc;
    private readonly PhienDangNhap _phien;
    private readonly AnhSach _anh;
    private readonly bool _duocSua;
    private int _maDangSua;
    private string? _hinhAnhHienTai;   // URL ảnh đang lưu trong CSDL
    private string? _fileAnhMoi;       // file ảnh vừa chọn, chưa lưu
    private bool _dangNap;
    private List<SachQtDto> _ds = new();

    public FrmSach(ISachService sach, IDanhMucService danhMuc, PhienDangNhap phien, AnhSach anh)
    {
        InitializeComponent();
        _sach = sach;
        _danhMuc = danhMuc;
        _phien = phien;
        _anh = anh;
        _duocSua = phien.CoQuyen(VaiTro.QuanTriVien);
        GiaoDien.KieuBang(dgvDanhSach);
        Load += FrmSach_Load;
    }

    private async void FrmSach_Load(object? sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, NapLanDauAsync);

    private async Task NapLanDauAsync()
    {
        _dangNap = true;
        var danhMucs = await _danhMuc.LayDanhMucChonAsync();
        GiaoDien.GanCombo(cboLocDanhMuc, danhMucs, "(Tất cả danh mục)");
        GiaoDien.GanCombo(cboLocTrangThai, new[] { new MucChonDto(1, "Đang bán"), new MucChonDto(2, "Ngừng bán") }, "(Tất cả)");
        GiaoDien.GanCombo(cboDanhMuc, danhMucs);
        GiaoDien.GanCombo(cboNhaXuatBan, await _danhMuc.LayNhaXuatBanChonAsync(), "(Không rõ)");
        var tacGias = await _danhMuc.LayDanhSachTacGiaAsync(null);
        clbTacGia.DisplayMember = "Ten";
        foreach (var t in tacGias)
            clbTacGia.Items.Add(new MucChonDto(t.MaTacGia, t.TrangThai ? t.TenTacGia : t.TenTacGia + " (ngừng dùng)"));
        _dangNap = false;

        if (!_duocSua)
        {
            // Chế độ chỉ xem: khóa vùng nhập, ẩn các nút ghi dữ liệu
            flowNut.Visible = btnChonAnh.Visible = false;
            foreach (Control c in tlpThongTin.Controls)
                if (c is TextBox t) t.ReadOnly = true;
                else if (c is not Label and not PictureBox) c.Enabled = false;
            grpThongTin.Text = "Thông tin sách (chỉ xem)";
        }
        await TaiDanhSachAsync();
    }

    private async Task TaiDanhSachAsync(int? chonMa = null)
    {
        _dangNap = true;
        bool? dangBan = GiaoDien.MaDangChon(cboLocTrangThai) switch { 1 => true, 2 => false, _ => null };
        _ds = await _sach.TimKiemQuanTriAsync(txtTimKiem.Text, GiaoDien.MaDangChon(cboLocDanhMuc), dangBan);
        GiaoDien.GanDanhSach(dgvDanhSach, _ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaSach", 30), ("ISBN", 70), ("TenSach", 160), ("NamXuatBan", 40), ("SoLuongTon", 45), ("TrangThaiText", 55));
        lblTongSo.Text = $"Có {_ds.Count} đầu sách · tồn kho chỉ thay đổi qua phiếu kho (menu Kho) hoặc đơn hàng";
        _dangNap = false;
        if (chonMa is int ma) GiaoDien.ChonDong<SachQtDto>(dgvDanhSach, s => s.MaSach == ma);
        await HienThiDongDangChonAsync();
    }

    private async Task HienThiDongDangChonAsync()
    {
        if (GiaoDien.DongDangChon<SachQtDto>(dgvDanhSach) is not { } dong) { XoaTrang(); return; }
        var s = await _sach.LayDeSuaAsync(dong.MaSach);
        if (s is null) { XoaTrang(); return; }
        _maDangSua = s.MaSach;
        txtISBN.Text = s.ISBN;
        txtTenSach.Text = s.TenSach;
        GiaoDien.ChonTheoMa(cboDanhMuc, s.MaDanhMuc);
        GiaoDien.ChonTheoMa(cboNhaXuatBan, s.MaNhaXuatBan);
        nudNamXuatBan.Value = s.NamXuatBan ?? 0;
        nudDonGia.Value = Math.Min(s.DonGia, nudDonGia.Maximum);
        lblTonKho.Text = $"{s.SoLuongTon} cuốn (cập nhật qua phiếu kho)";
        for (int i = 0; i < clbTacGia.Items.Count; i++)
            clbTacGia.SetItemChecked(i, s.MaTacGias.Contains(((MucChonDto)clbTacGia.Items[i]).Ma));
        txtMoTa.Text = s.MoTa;
        chkDangBan.Checked = s.TrangThai;
        _hinhAnhHienTai = s.HinhAnh;
        _fileAnhMoi = null;
        HienAnh(_anh.DuongDanFile(s.HinhAnh), s.HinhAnh);
        grpThongTin.Text = _duocSua ? $"Sửa sách #{s.MaSach}" : $"Sách #{s.MaSach} (chỉ xem)";
    }

    private void HienAnh(string? file, string? url)
    {
        picAnhBia.Image?.Dispose();
        picAnhBia.Image = AnhSach.MoAnh(file) ?? AnhSach.AnhThayThe(
            url is null ? "Chưa có ảnh bìa"
            : url.EndsWith(".svg", StringComparison.OrdinalIgnoreCase) ? "Ảnh bìa dạng SVG\n(xem trên website)"
            : "Không tìm thấy file ảnh\n" + url);
    }

    private void XoaTrang()
    {
        _maDangSua = 0;
        txtISBN.Clear();
        txtTenSach.Clear();
        if (cboDanhMuc.Items.Count > 0) cboDanhMuc.SelectedIndex = 0;
        GiaoDien.ChonTheoMa(cboNhaXuatBan, null);
        nudNamXuatBan.Value = DateTime.Today.Year;
        nudDonGia.Value = 0;
        lblTonKho.Text = "0 (sách mới – nhập kho bằng phiếu nhập)";
        for (int i = 0; i < clbTacGia.Items.Count; i++) clbTacGia.SetItemChecked(i, false);
        txtMoTa.Clear();
        chkDangBan.Checked = true;
        _hinhAnhHienTai = _fileAnhMoi = null;
        HienAnh(null, null);
        grpThongTin.Text = "Thêm sách mới";
        txtTenSach.Focus();
    }

    private async void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
    {
        if (!_dangNap) await GiaoDien.ChayTuanTuAsync(this, HienThiDongDangChonAsync);
    }

    private void btnThemMoi_Click(object sender, EventArgs e)
    {
        dgvDanhSach.ClearSelection();
        XoaTrang();
    }

    private void btnChonAnh_Click(object sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog { Filter = "Ảnh (*.jpg;*.jpeg;*.png;*.webp;*.gif)|*.jpg;*.jpeg;*.png;*.webp;*.gif" };
        if (dlg.ShowDialog(this) != DialogResult.OK) return;
        if (new FileInfo(dlg.FileName).Length > 2 * 1024 * 1024) { ThongBao.CanhBao("Ảnh tối đa 2 MB."); return; }
        _fileAnhMoi = dlg.FileName;
        HienAnh(dlg.FileName, dlg.FileName);
    }

    private async void btnLuu_Click(object sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, async () =>
    {
        var dto = new SachLuuDto
        {
            MaSach = _maDangSua, ISBN = txtISBN.Text, TenSach = txtTenSach.Text,
            MaDanhMuc = GiaoDien.MaDangChon(cboDanhMuc) ?? 0, MaNhaXuatBan = GiaoDien.MaDangChon(cboNhaXuatBan),
            NamXuatBan = nudNamXuatBan.Value == 0 ? null : (short)nudNamXuatBan.Value, DonGia = nudDonGia.Value,
            MoTa = txtMoTa.Text, TrangThai = chkDangBan.Checked, HinhAnh = _hinhAnhHienTai,
            MaTacGias = clbTacGia.CheckedItems.Cast<MucChonDto>().Select(t => t.Ma).ToList()
        };
        if (_fileAnhMoi != null) dto.HinhAnh = _anh.LuuAnh(_fileAnhMoi, dto.TenSach);
        int ma = await _sach.LuuAsync(_phien.MaTaiKhoan, dto);
        ThongBao.ThanhCong(_maDangSua == 0 ? "Đã thêm sách. Hãy lập phiếu nhập kho để có hàng bán." : "Đã cập nhật sách.");
        await TaiDanhSachAsync(ma);
    });

    private async void btnXoa_Click(object sender, EventArgs e)
    {
        if (_maDangSua == 0) { ThongBao.CanhBao("Hãy chọn sách cần xóa."); return; }
        if (!ThongBao.XacNhan($"Xóa sách “{txtTenSach.Text}”?\nSách đã có đơn hàng hoặc phiếu kho sẽ chỉ chuyển sang “Ngừng kinh doanh” (BR09).")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            bool daXoa = await _sach.XoaHoacNgungBanAsync(_phien.MaTaiKhoan, _maDangSua);
            ThongBao.ThanhCong(daXoa ? "Đã xóa sách." : "Sách đã có giao dịch nên được chuyển sang “Ngừng kinh doanh”; website sẽ không hiển thị sách này nữa.");
            await TaiDanhSachAsync(daXoa ? null : _maDangSua);
        });
    }

    private void btnXuatExcel_Click(object sender, EventArgs e) =>
        XuatExcel.Xuat(this, _ds, "DANH SÁCH SÁCH", "DanhSachSach");

    private async void btnTim_Click(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private async void btnLamMoi_Click(object sender, EventArgs e)
    {
        _dangNap = true;
        txtTimKiem.Clear();
        cboLocDanhMuc.SelectedIndex = cboLocTrangThai.SelectedIndex = 0;
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
