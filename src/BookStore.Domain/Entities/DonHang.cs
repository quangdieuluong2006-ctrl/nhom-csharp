using BookStore.Domain.Enums;

namespace BookStore.Domain.Entities;

public class DonHang
{
    public long MaDonHang { get; set; }
    public int MaKhachHang { get; set; }
    public DateTime NgayDat { get; set; } = DateTime.Now;
    public DateTime? NgayGiaoDuKien { get; set; }
    public string HoTenNhan { get; set; } = "";
    public string SoDienThoaiNhan { get; set; } = "";
    public string DiaChiGiaoHang { get; set; } = "";
    public string? GhiChu { get; set; }
    public decimal TongTien { get; set; }
    public TrangThaiDonHang TrangThai { get; set; } = TrangThaiDonHang.ChoXacNhan;
    public string PhuongThucThanhToan { get; set; } = "COD";
    public TrangThaiThanhToan TrangThaiThanhToan { get; set; } = TrangThaiThanhToan.ChuaThanhToan;

    public KhachHang KhachHang { get; set; } = null!;
    public ICollection<ChiTietDonHang> ChiTiets { get; set; } = new List<ChiTietDonHang>();

    public decimal TinhTongTien() => ChiTiets.Sum(c => c.SoLuong * c.DonGiaDat);

    /// <summary>
    /// Sơ đồ trạng thái đơn hàng (báo cáo mục 3.4, BR07, BR13):
    ///   Chờ xác nhận → Đã xác nhận → Đang giao → Hoàn thành
    ///   Chờ xác nhận / Đã xác nhận / Đang giao → Đã hủy
    /// Đơn Hoàn thành hoặc Đã hủy không được chuyển sang trạng thái khác.
    /// Web (khách hủy) và Desktop (nhân viên xử lý) đều kiểm tra bằng hàm này.
    /// </summary>
    public static bool LaChuyenHopLe(TrangThaiDonHang cu, TrangThaiDonHang moi) => (cu, moi) switch
    {
        (TrangThaiDonHang.ChoXacNhan, TrangThaiDonHang.DaXacNhan) => true,
        (TrangThaiDonHang.DaXacNhan, TrangThaiDonHang.DangGiao) => true,
        (TrangThaiDonHang.DangGiao, TrangThaiDonHang.HoanThanh) => true,
        (TrangThaiDonHang.ChoXacNhan or TrangThaiDonHang.DaXacNhan or TrangThaiDonHang.DangGiao, TrangThaiDonHang.DaHuy) => true,
        _ => false
    };

    /// <summary>Bước xử lý kế tiếp theo quy trình (null nếu đơn đã kết thúc).</summary>
    public static TrangThaiDonHang? TrangThaiKeTiep(TrangThaiDonHang cu) => cu switch
    {
        TrangThaiDonHang.ChoXacNhan => TrangThaiDonHang.DaXacNhan,
        TrangThaiDonHang.DaXacNhan => TrangThaiDonHang.DangGiao,
        TrangThaiDonHang.DangGiao => TrangThaiDonHang.HoanThanh,
        _ => null
    };
}
