using System.ComponentModel.DataAnnotations;
using BookStore.Domain.Enums;

namespace BookStore.Application.Dtos;

public record DongDatHang(int MaSach, int SoLuong);

/// <summary>Thông tin người nhận nhập ở trang đặt hàng (Lab06).</summary>
public class ThongTinNhanHangDto
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên người nhận")]
    [StringLength(100)]
    [Display(Name = "Họ tên người nhận")]
    public string HoTenNhan { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại gồm 10–11 chữ số, bắt đầu bằng 0")]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoaiNhan { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
    [StringLength(255)]
    [Display(Name = "Địa chỉ giao hàng")]
    public string DiaChiGiaoHang { get; set; } = "";

    [StringLength(500)]
    [Display(Name = "Ghi chú")]
    public string? GhiChu { get; set; }
}

public record DonHangTomTatDto(long MaDonHang, DateTime NgayDat, decimal TongTien, TrangThaiDonHang TrangThai, int SoSanPham);

public record ChiTietDonDto(int MaSach, string TenSach, string? HinhAnh, int SoLuong, decimal DonGiaDat, decimal ThanhTien);

public record DonHangChiTietDto(
    long MaDonHang, DateTime NgayDat, DateTime? NgayGiaoDuKien, string HoTenNhan, string SoDienThoaiNhan,
    string DiaChiGiaoHang, string? GhiChu, decimal TongTien, TrangThaiDonHang TrangThai,
    string PhuongThucThanhToan, TrangThaiThanhToan TrangThaiThanhToan, IReadOnlyList<ChiTietDonDto> ChiTiets)
{
    /// <summary>Khách chỉ được tự hủy khi đơn còn chờ xác nhận (quy tắc nghiệp vụ trong báo cáo).</summary>
    public bool CoTheHuy => TrangThai == TrangThaiDonHang.ChoXacNhan;
}
