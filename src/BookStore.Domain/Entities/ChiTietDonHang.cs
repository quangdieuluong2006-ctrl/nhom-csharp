namespace BookStore.Domain.Entities;

public class ChiTietDonHang
{
    public long MaDonHang { get; set; }
    public int MaSach { get; set; }
    public int SoLuong { get; set; }
    /// <summary>Giá tại thời điểm đặt, không đổi khi giá sách thay đổi sau này.</summary>
    public decimal DonGiaDat { get; set; }
    /// <summary>Cột tính toán trong CSDL: SoLuong * DonGiaDat.</summary>
    public decimal ThanhTien { get; private set; }

    public DonHang DonHang { get; set; } = null!;
    public Sach Sach { get; set; } = null!;
}
