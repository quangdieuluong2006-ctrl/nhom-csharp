using BookStore.Domain.Enums;

namespace BookStore.Web.Infrastructure;

/// <summary>Chuyển giá trị enum sang chữ tiếng Việt và màu nhãn Bootstrap để hiển thị.</summary>
public static class HienThi
{
    public static (string Ten, string Mau) TrangThai(TrangThaiDonHang t) => t switch
    {
        TrangThaiDonHang.ChoXacNhan => ("Chờ xác nhận", "warning text-dark"),
        TrangThaiDonHang.DaXacNhan => ("Đã xác nhận", "info text-dark"),
        TrangThaiDonHang.DangGiao => ("Đang giao", "primary"),
        TrangThaiDonHang.HoanThanh => ("Hoàn thành", "success"),
        TrangThaiDonHang.DaHuy => ("Đã hủy", "secondary"),
        _ => (t.ToString(), "light")
    };

    public static string ThanhToan(TrangThaiThanhToan t) =>
        t == TrangThaiThanhToan.DaThanhToan ? "Đã thanh toán" : "Chưa thanh toán";

    public static string Tien(decimal so) => so.ToString("N0") + " đ";
}
