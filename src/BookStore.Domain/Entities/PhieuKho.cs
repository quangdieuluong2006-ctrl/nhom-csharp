using BookStore.Domain.Enums;

namespace BookStore.Domain.Entities;

public class PhieuKho
{
    public long MaPhieu { get; set; }
    public int MaTaiKhoan { get; set; }
    public LoaiPhieuKho LoaiPhieu { get; set; }
    public DateTime NgayLap { get; set; } = DateTime.Now;
    public string? GhiChu { get; set; }

    public TaiKhoan TaiKhoan { get; set; } = null!;
    public ICollection<ChiTietPhieuKho> ChiTiets { get; set; } = new List<ChiTietPhieuKho>();
}

public class ChiTietPhieuKho
{
    public long MaPhieu { get; set; }
    public int MaSach { get; set; }
    public int SoLuong { get; set; }
    public decimal? DonGiaNhap { get; set; }

    public PhieuKho PhieuKho { get; set; } = null!;
    public Sach Sach { get; set; } = null!;
}
