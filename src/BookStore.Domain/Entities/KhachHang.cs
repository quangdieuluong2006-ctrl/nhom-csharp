namespace BookStore.Domain.Entities;

public class KhachHang
{
    public int MaKhachHang { get; set; }
    public int? MaTaiKhoan { get; set; }
    public string HoTen { get; set; } = "";
    public string? Email { get; set; }
    public string SoDienThoai { get; set; } = "";
    public string? DiaChi { get; set; }

    public TaiKhoan? TaiKhoan { get; set; }
    public ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
