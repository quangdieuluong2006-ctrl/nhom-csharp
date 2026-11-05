namespace BookStore.Application.Dtos;

/// <summary>Thông tin rút gọn để hiển thị sách trên lưới (trang chủ, danh sách).</summary>
public record SachTomTatDto(int MaSach, string TenSach, string TacGia, decimal DonGia, string? HinhAnh, int SoLuongTon);

/// <summary>Thông tin đầy đủ cho trang chi tiết sách.</summary>
public record SachChiTietDto(
    int MaSach, string TenSach, string TacGia, string? TenNhaXuatBan, int? MaNhaXuatBan,
    string TenDanhMuc, int MaDanhMuc, short? NamXuatBan, string? ISBN,
    decimal DonGia, int SoLuongTon, string? MoTa, string? HinhAnh);

/// <summary>Một mục trên menu trái. Với danh mục cha, SoSach gồm cả sách của danh mục con.</summary>
public record MucMenuDto(int Ma, string Ten, int SoSach)
{
    public IReadOnlyList<MucMenuDto> Con { get; init; } = Array.Empty<MucMenuDto>();
}

public enum SapXepSach { MoiNhat, GiaTang, GiaGiam, TenAZ }

/// <summary>Điều kiện lọc sách: theo từ khóa, danh mục, nhà xuất bản, khoảng giá.</summary>
public class LocSach
{
    public string? TuKhoa { get; set; }
    public int? MaDanhMuc { get; set; }
    public int? MaNhaXuatBan { get; set; }
    public decimal? GiaTu { get; set; }
    public decimal? GiaDen { get; set; }
    public SapXepSach SapXep { get; set; } = SapXepSach.MoiNhat;
    public int Trang { get; set; } = 1;
    public int KichThuocTrang { get; set; } = 12;
}
