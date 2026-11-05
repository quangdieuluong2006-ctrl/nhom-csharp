namespace BookStore.Domain.Entities;

/// <summary>Bảng trung gian Sách – Tác giả (quan hệ N – N).</summary>
public class SachTacGia
{
    public int MaSach { get; set; }
    public int MaTacGia { get; set; }
    public string? VaiTroTacGia { get; set; }

    public Sach Sach { get; set; } = null!;
    public TacGia TacGia { get; set; } = null!;
}
