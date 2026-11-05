namespace BookStore.Domain.Entities;

public class NhaXuatBan
{
    public int MaNhaXuatBan { get; set; }
    public string TenNhaXuatBan { get; set; } = "";
    public string? DiaChi { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }

    public ICollection<Sach> Sachs { get; set; } = new List<Sach>();
}
