using BookStore.Domain.Enums;

namespace BookStore.Domain.Entities;

public class TaiKhoan
{
    public int MaTaiKhoan { get; set; }
    public string TenDangNhap { get; set; } = "";
    public string MatKhauHash { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public VaiTro VaiTro { get; set; } = VaiTro.KhachHang;
    public bool TrangThai { get; set; } = true;

    public KhachHang? KhachHang { get; set; }
}
