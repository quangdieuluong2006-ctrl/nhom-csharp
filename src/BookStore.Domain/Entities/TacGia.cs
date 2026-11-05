namespace BookStore.Domain.Entities;

public class TacGia
{
    public int MaTacGia { get; set; }
    public string TenTacGia { get; set; } = "";
    public string? TieuSu { get; set; }
    public bool TrangThai { get; set; } = true;

    public ICollection<SachTacGia> SachTacGias { get; set; } = new List<SachTacGia>();
}
