using System.ComponentModel;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.Forms;

/// <summary>
/// D07 – Lập phiếu nhập / xuất / điều chỉnh kho (UC-D07, US15). QTV, NV kho.
/// Phiếu gồm nhiều dòng; lưu phiếu, chi tiết và cập nhật tồn trong một giao tác (KhoService.LuuPhieuAsync).
/// </summary>
public partial class FrmPhieuKho : Form
{
    private readonly IKhoService _kho;
    private readonly ISachService _sach;
    private readonly PhienDangNhap _phien;
    private readonly ILogger<FrmPhieuKho> _log;
    private readonly BindingList<DongPhieu> _dong = new();

    private sealed class DongPhieu
    {
        [DisplayName("Mã sách")] public int MaSach { get; init; }
        [DisplayName("Tên sách")] public string TenSach { get; init; } = "";
        [DisplayName("Số lượng")] public int SoLuong { get; set; }
        [DisplayName("Đơn giá nhập")] public decimal? DonGiaNhap { get; set; }
        [DisplayName("Thành tiền")] public decimal? ThanhTien => DonGiaNhap * SoLuong;
    }

    public FrmPhieuKho(IKhoService kho, ISachService sach, PhienDangNhap phien, ILogger<FrmPhieuKho> log)
    {
        InitializeComponent();
        _kho = kho;
        _sach = sach;
        _phien = phien;
        _log = log;
        GiaoDien.KieuBang(dgvDong);
        GiaoDien.GanCombo(cboLoaiPhieu, Enum.GetValues<LoaiPhieuKho>().Select(l => new MucChonDto((int)l + 1, l.Ten())));
        cboLoaiPhieu.SelectedIndexChanged += (_, _) => CapNhatTheoLoaiPhieu();
        dgvDong.DataSource = _dong;
        _dong.ListChanged += (_, _) => CapNhatTongCong();
        Load += async (_, _) => await GiaoDien.ThucHienAsync(this, async () => { await NapSachAsync(); CapNhatTheoLoaiPhieu(); DinhDangLuoi(); });
    }

    private LoaiPhieuKho LoaiDangChon => (LoaiPhieuKho)((GiaoDien.MaDangChon(cboLoaiPhieu) ?? 1) - 1);

    private async Task NapSachAsync()
    {
        var ds = await _sach.LaySachChonAsync();
        cboSach.DataSource = ds;
        cboSach.DisplayMember = "Ten";
        cboSach.ValueMember = "Ma";
        cboSach.SelectedIndex = -1;
    }

    private void DinhDangLuoi()
    {
        foreach (DataGridViewColumn c in dgvDong.Columns)
            if (c.ValueType == typeof(decimal?) || c.ValueType == typeof(decimal))
            {
                c.DefaultCellStyle.Format = "N0";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        GiaoDien.DoRongCot(dgvDong, ("MaSach", 30), ("TenSach", 200), ("SoLuong", 40));
    }

    /// <summary>Chỉ phiếu nhập mới có đơn giá nhập.</summary>
    private void CapNhatTheoLoaiPhieu()
    {
        bool laNhap = LoaiDangChon == LoaiPhieuKho.Nhap;
        nudDonGiaNhap.Enabled = laNhap;
        if (!laNhap)
        {
            foreach (var d in _dong) d.DonGiaNhap = null;
            _dong.ResetBindings();
        }
        txtGhiChu.PlaceholderText = LoaiDangChon is LoaiPhieuKho.DieuChinhTang or LoaiPhieuKho.DieuChinhGiam
            ? "Bắt buộc: ghi rõ lý do điều chỉnh (kiểm kê, hư hỏng...)" : "Ghi chú (không bắt buộc)";
    }

    private void CapNhatTongCong()
    {
        if (_dong.Count == 0) { lblTong.Text = "Chưa có dòng nào"; return; }
        var tien = _dong.Sum(d => d.ThanhTien ?? 0);
        lblTong.Text = $"{_dong.Count} dòng · tổng số lượng {_dong.Sum(d => d.SoLuong):N0}"
                       + (LoaiDangChon == LoaiPhieuKho.Nhap ? $" · tổng tiền nhập {tien:N0} đ" : "");
    }

    private void btnThemDong_Click(object sender, EventArgs e)
    {
        if (cboSach.SelectedItem is not MucChonDto sach) { ThongBao.CanhBao("Hãy chọn sách trong danh sách."); return; }
        bool laNhap = LoaiDangChon == LoaiPhieuKho.Nhap;
        if (laNhap && nudDonGiaNhap.Value <= 0) { ThongBao.CanhBao("Phiếu nhập cần đơn giá nhập lớn hơn 0."); return; }
        int soLuong = (int)nudSoLuong.Value;
        decimal? donGia = laNhap ? nudDonGiaNhap.Value : null;

        // Sách đã có trong phiếu: cộng dồn số lượng vào dòng hiện có (UC-D07 – luồng 2a)
        var coSan = _dong.FirstOrDefault(d => d.MaSach == sach.Ma);
        if (coSan != null)
        {
            coSan.SoLuong += soLuong;
            coSan.DonGiaNhap = donGia;
            _dong.ResetBindings();
        }
        else
        {
            var ten = sach.Ten;
            int i = ten.LastIndexOf(" (tồn:", StringComparison.Ordinal);
            _dong.Add(new DongPhieu { MaSach = sach.Ma, TenSach = i > 0 ? ten[..i] : ten, SoLuong = soLuong, DonGiaNhap = donGia });
            if (_dong.Count == 1) DinhDangLuoi();
        }
        nudSoLuong.Value = 1;
        cboSach.Focus();
    }

    private void btnXoaDong_Click(object sender, EventArgs e)
    {
        if (GiaoDien.DongDangChon<DongPhieu>(dgvDong) is { } d) _dong.Remove(d);
    }

    private void btnPhieuMoi_Click(object sender, EventArgs e)
    {
        if (_dong.Count > 0 && !ThongBao.XacNhan("Bỏ các dòng đang nhập để lập phiếu mới?")) return;
        LamMoiPhieu();
    }

    private void LamMoiPhieu()
    {
        _dong.Clear();
        txtGhiChu.Clear();
        cboSach.SelectedIndex = -1;
        nudSoLuong.Value = 1;
        nudDonGiaNhap.Value = 0;
    }

    private async void btnLuuPhieu_Click(object sender, EventArgs e)
    {
        if (_dong.Count == 0) { ThongBao.CanhBao("Phiếu phải có ít nhất một dòng."); return; }
        var loai = LoaiDangChon;
        if (!ThongBao.XacNhan($"Lưu {loai.Ten().ToLower()} gồm {_dong.Count} dòng?\nTồn kho sẽ được cập nhật ngay.")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            long ma = await _kho.LuuPhieuAsync(new PhieuKhoDto
            {
                LoaiPhieu = loai, GhiChu = txtGhiChu.Text,
                Dong = _dong.Select(d => new DongPhieuKhoDto(d.MaSach, d.SoLuong, d.DonGiaNhap)).ToList()
            }, _phien.MaTaiKhoan);
            _log.LogInformation("{NhanVien} lập {Loai} #{MaPhieu} gồm {SoDong} dòng", _phien.NguoiDung!.TenDangNhap, loai, ma, _dong.Count);
            ThongBao.ThanhCong($"Đã lưu phiếu #{ma}. Tồn kho đã được cập nhật.");
            LamMoiPhieu();
            await NapSachAsync();   // số tồn trong danh sách chọn sách thay đổi
        });
    }
}
