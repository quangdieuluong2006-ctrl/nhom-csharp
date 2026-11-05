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
}
