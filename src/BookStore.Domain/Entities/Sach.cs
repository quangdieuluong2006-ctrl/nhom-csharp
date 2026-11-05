namespace BookStore.Domain.Entities;

public class Sach
{
    public int MaSach { get; set; }
    public int MaDanhMuc { get; set; }
    public int? MaNhaXuatBan { get; set; }
    public string? ISBN { get; set; }
    public string TenSach { get; set; } = "";
    public short? NamXuatBan { get; set; }
    public decimal DonGia { get; set; }
    public int SoLuongTon { get; set; }
    public string? MoTa { get; set; }
    public string? HinhAnh { get; set; }
    public bool TrangThai { get; set; } = true;
    /// <summary>Ngày thêm sách, dùng để lấy "sách mới" trên trang chủ.</summary>
    public DateTime NgayCapNhat { get; set; } = DateTime.Now;

    public DanhMuc DanhMuc { get; set; } = null!;
    public NhaXuatBan? NhaXuatBan { get; set; }
    public ICollection<SachTacGia> SachTacGias { get; set; } = new List<SachTacGia>();
}
