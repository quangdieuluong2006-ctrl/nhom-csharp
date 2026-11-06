using System.ComponentModel;
using System.Text;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.Forms;

/// <summary>
/// D06 – Xử lý đơn hàng (QTV, NV bán hàng) – phần B của quy trình liên thông:
/// đơn khách đặt trên Web hiện ở đây (tự làm mới mỗi 30 giây); nhân viên xác nhận → giao → hoàn thành hoặc hủy;
/// khách thấy trạng thái mới ngay trên trang "Đơn hàng của tôi".
/// </summary>
public partial class FrmDonHang : Form
{
    private readonly IDonHangService _svc;
    private readonly PhienDangNhap _phien;
    private readonly ILogger<FrmDonHang> _log;
    private DonHangChiTietDto? _donDangXem;
    private bool _dangNap;
    private readonly Font _chuDam;

    /// <summary>Một dòng trong bảng chi tiết đơn (tiêu đề cột tiếng Việt).</summary>
    private sealed class DongChiTiet
    {
        [DisplayName("Tên sách")] public string TenSach { get; init; } = "";
        [DisplayName("SL")] public int SoLuong { get; init; }
        [DisplayName("Đơn giá")] public decimal DonGia { get; init; }
        [DisplayName("Thành tiền")] public decimal ThanhTien { get; init; }
    }

    public FrmDonHang(IDonHangService svc, PhienDangNhap phien, ILogger<FrmDonHang> log)
    {
        InitializeComponent();
        _svc = svc;
        _phien = phien;
        _log = log;
        _chuDam = new Font(dgvDanhSach.Font, FontStyle.Bold);
        Disposed += (_, _) => _chuDam.Dispose();
        GiaoDien.KieuBang(dgvDanhSach);
        GiaoDien.KieuBang(dgvChiTiet);
        _dangNap = true;   // chặn sự kiện SelectedIndexChanged khi đổ dữ liệu vào ComboBox
        GiaoDien.GanCombo(cboTrangThai,
            Enum.GetValues<TrangThaiDonHang>().Select(t => new MucChonDto((int)t + 1, t.Ten())), "(Tất cả trạng thái)");
        dtpTuNgay.Value = DateTime.Today.AddDays(-30);
        dtpTuNgay.Checked = dtpDenNgay.Checked = false;   // mặc định không lọc theo ngày
        _dangNap = false;
        Load += async (_, _) =>
        {
            await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
            tmrLamMoi.Enabled = chkTuDongLamMoi.Checked;
        };
    }

    /// <summary>Mở form và chọn sẵn một đơn (gọi từ màn hình Tổng quan).</summary>
    public async Task ChonDonAsync(long maDon)
    {
        _dangNap = true;
        txtTimKiem.Text = maDon.ToString();
        cboTrangThai.SelectedIndex = 0;
        _dangNap = false;
        await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync(maDon));
    }

    private LocDonHang TaoBoLoc() => new()
    {
        TuKhoa = txtTimKiem.Text,
        TrangThai = GiaoDien.MaDangChon(cboTrangThai) is int m ? (TrangThaiDonHang)(m - 1) : null,
        TuNgay = dtpTuNgay.Checked ? dtpTuNgay.Value : null,
        DenNgay = dtpDenNgay.Checked ? dtpDenNgay.Value : null
    };

    private async Task TaiDanhSachAsync(long? chonMa = null)
    {
        chonMa ??= GiaoDien.DongDangChon<DonHangQtDto>(dgvDanhSach)?.MaDonHang;
        _dangNap = true;
        var ds = await _svc.TimKiemQuanTriAsync(TaoBoLoc());
        GiaoDien.GanDanhSach(dgvDanhSach, ds);
        GiaoDien.DoRongCot(dgvDanhSach, ("MaDonHang", 45), ("NgayDat", 80), ("SoDienThoaiNhan", 65), ("TongTien", 60), ("ThanhToanText", 65));
        int cho = ds.Count(d => d.TrangThai == TrangThaiDonHang.ChoXacNhan);
        lblTongSo.Text = $"{ds.Count} đơn · {cho} đơn chờ xác nhận · cập nhật lúc {DateTime.Now:HH:mm:ss}";
        _dangNap = false;
        if (chonMa is long ma) GiaoDien.ChonDong<DonHangQtDto>(dgvDanhSach, d => d.MaDonHang == ma);
        await HienThiChiTietAsync();
    }

    private async Task HienThiChiTietAsync()
    {
        var dong = GiaoDien.DongDangChon<DonHangQtDto>(dgvDanhSach);
        _donDangXem = dong is null ? null : await _svc.LayChiTietQuanTriAsync(dong.MaDonHang);
        if (_donDangXem is not { } d)
        {
            txtThongTinDon.Text = "Chọn một đơn hàng để xem chi tiết.";
            dgvChiTiet.DataSource = null;
            lblTongTien.Text = "";
            btnChuyenTiep.Enabled = btnHuyDon.Enabled = false;
            return;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Mã đơn: #{d.MaDonHang}        Ngày đặt: {d.NgayDat:dd/MM/yyyy HH:mm}");
        sb.AppendLine($"Khách hàng: {dong!.KhachHang}");
        sb.AppendLine($"Người nhận: {d.HoTenNhan} – {d.SoDienThoaiNhan}");
        sb.AppendLine($"Địa chỉ: {d.DiaChiGiaoHang}");
        if (d.NgayGiaoDuKien is DateTime ngay) sb.AppendLine($"Dự kiến giao: {ngay:dd/MM/yyyy}");
        if (!string.IsNullOrWhiteSpace(d.GhiChu)) sb.AppendLine($"Ghi chú: {d.GhiChu}");
        sb.AppendLine($"Thanh toán: {d.PhuongThucThanhToan} – {d.TrangThaiThanhToan.Ten()}");
        sb.Append($"TRẠNG THÁI: {d.TrangThai.Ten().ToUpper()}");
        txtThongTinDon.Text = sb.ToString();

        GiaoDien.GanDanhSach(dgvChiTiet, d.ChiTiets.Select(c => new DongChiTiet
        {
            TenSach = c.TenSach, SoLuong = c.SoLuong, DonGia = c.DonGiaDat, ThanhTien = c.ThanhTien
        }).ToList());
        GiaoDien.DoRongCot(dgvChiTiet, ("TenSach", 170), ("SoLuong", 30));
        lblTongTien.Text = $"Tổng tiền: {d.TongTien:N0} đ";

        // Nút theo sơ đồ trạng thái (DonHang.LaChuyenHopLe – dùng chung với Web)
        var keTiep = DonHang.TrangThaiKeTiep(d.TrangThai);
        btnChuyenTiep.Enabled = keTiep != null;
        btnChuyenTiep.Text = keTiep switch
        {
            TrangThaiDonHang.DaXacNhan => "Xác nhận đơn",
            TrangThaiDonHang.DangGiao => "Chuyển sang Đang giao",
            TrangThaiDonHang.HoanThanh => "Hoàn thành (đã thu tiền)",
            _ => "Đơn đã kết thúc"
        };
        btnHuyDon.Enabled = DonHang.LaChuyenHopLe(d.TrangThai, TrangThaiDonHang.DaHuy);
    }

    private async void dgvDanhSach_SelectionChanged(object sender, EventArgs e)
    {
        if (!_dangNap) await GiaoDien.ChayTuanTuAsync(this, HienThiChiTietAsync);
    }

    /// <summary>Tô màu dòng theo trạng thái: đơn chờ xác nhận nổi bật để nhân viên xử lý trước.</summary>
    private void dgvDanhSach_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvDanhSach.Rows[e.RowIndex].DataBoundItem is not DonHangQtDto d) return;
        switch (d.TrangThai)
        {
            case TrangThaiDonHang.ChoXacNhan:
                e.CellStyle!.BackColor = Color.FromArgb(255, 248, 220);
                e.CellStyle.Font = _chuDam;
                break;
            case TrangThaiDonHang.DaHuy:
                e.CellStyle!.ForeColor = Color.Gray;
                break;
            case TrangThaiDonHang.HoanThanh:
                e.CellStyle!.ForeColor = Color.SeaGreen;
                break;
        }
    }

    private async void btnChuyenTiep_Click(object sender, EventArgs e)
    {
        if (_donDangXem is not { } d || DonHang.TrangThaiKeTiep(d.TrangThai) is not TrangThaiDonHang moi) return;
        if (!ThongBao.XacNhan($"Chuyển đơn #{d.MaDonHang} từ “{d.TrangThai.Ten()}” sang “{moi.Ten()}”?")) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.ChuyenTrangThaiAsync(d.MaDonHang, moi, _phien.MaTaiKhoan);
            _log.LogInformation("{NhanVien} chuyển đơn {MaDon}: {Cu} → {Moi}", _phien.NguoiDung!.TenDangNhap, d.MaDonHang, d.TrangThai, moi);
            await TaiDanhSachAsync(d.MaDonHang);
        });
    }

    private async void btnHuyDon_Click(object sender, EventArgs e)
    {
        if (_donDangXem is not { } d) return;
        var lyDo = HopThoai.NhapChuoi(this, $"Hủy đơn #{d.MaDonHang}",
            "Lý do hủy (ví dụ: khách không nhận hàng). Tồn kho sẽ được hoàn lại.", doDaiToiDa: 200);
        if (lyDo is null) return;
        await GiaoDien.ThucHienAsync(this, async () =>
        {
            await _svc.HuyDonBoiNhanVienAsync(d.MaDonHang, _phien.MaTaiKhoan, lyDo);
            _log.LogInformation("{NhanVien} hủy đơn {MaDon} (trạng thái trước: {Cu}), lý do: {LyDo}",
                _phien.NguoiDung!.TenDangNhap, d.MaDonHang, d.TrangThai, lyDo);
            ThongBao.ThanhCong($"Đã hủy đơn #{d.MaDonHang} và hoàn lại tồn kho.");
            await TaiDanhSachAsync(d.MaDonHang);
        });
    }

    private async void tmrLamMoi_Tick(object? sender, EventArgs e)
    {
        // Không làm mới khi form đang thao tác (đang bị khóa) để tránh đổi dòng đang chọn giữa chừng
        if (!Enabled || _dangNap) return;
        try { await GiaoDien.ChayTuanTuAsync(this, () => TaiDanhSachAsync()); }
        catch (Exception ex) { _log.LogWarning(ex, "Tự động làm mới danh sách đơn thất bại"); }
    }

    private void chkTuDongLamMoi_CheckedChanged(object sender, EventArgs e) => tmrLamMoi.Enabled = chkTuDongLamMoi.Checked;

    private async void btnTim_Click(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        await GiaoDien.ThucHienAsync(this, () => TaiDanhSachAsync());
    }

    private async void btnLamMoi_Click(object sender, EventArgs e)
    {
        _dangNap = true;
        txtTimKiem.Clear();
        cboTrangThai.SelectedIndex = 0;
        dtpTuNgay.Checked = dtpDenNgay.Checked = false;
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
