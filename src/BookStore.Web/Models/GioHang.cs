using System.Text.Json;

namespace BookStore.Web.Models;

/// <summary>Một dòng trong giỏ hàng (Lab05 – lớp Giohang).</summary>
public class DongGioHang
{
    public int MaSach { get; set; }
    public string TenSach { get; set; } = "";
    public string? HinhAnh { get; set; }
    public decimal DonGia { get; set; }
    public int SoLuong { get; set; }
    public decimal ThanhTien => DonGia * SoLuong;
}

/// <summary>
/// Giỏ hàng lưu trong Session (Lab05 dùng Session["Giohang"] = List&lt;Giohang&gt;).
/// ASP.NET Core Session chỉ lưu chuỗi/byte nên giỏ được tuần tự hóa (serialize) thành JSON.
/// </summary>
public class GioHang
{
    private const string Khoa = "GioHang";
    public List<DongGioHang> Dong { get; set; } = new();

    public int TongSoLuong => Dong.Sum(d => d.SoLuong);
    public decimal TongTien => Dong.Sum(d => d.ThanhTien);

    public static GioHang Doc(ISession session) =>
        session.GetString(Khoa) is string json ? JsonSerializer.Deserialize<GioHang>(json) ?? new() : new();

    public void Luu(ISession session) => session.SetString(Khoa, JsonSerializer.Serialize(this));

    public static void Xoa(ISession session) => session.Remove(Khoa);
}
