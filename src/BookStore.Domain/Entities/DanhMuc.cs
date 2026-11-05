namespace BookStore.Domain.Entities;

public class DanhMuc
{
    public int MaDanhMuc { get; set; }
    public int? MaDanhMucCha { get; set; }
    public string TenDanhMuc { get; set; } = "";
    public string? MoTa { get; set; }
    public bool TrangThai { get; set; } = true;

    public DanhMuc? DanhMucCha { get; set; }
    public ICollection<DanhMuc> DanhMucCons { get; set; } = new List<DanhMuc>();
    public ICollection<Sach> Sachs { get; set; } = new List<Sach>();
}
