using System.Security.Claims;
using BookStore.Application.Dtos;

namespace BookStore.Web.Infrastructure;

/// <summary>Chuyển đổi giữa thông tin người dùng và claims trong cookie đăng nhập.</summary>
public static class NguoiDungHienTai
{
    public const string ClaimMaKhachHang = "MaKhachHang";

    public static ClaimsPrincipal TaoPrincipal(NguoiDungDto nd, string scheme)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, nd.MaTaiKhoan.ToString()),
            new(ClaimTypes.Name, nd.TenDangNhap),
            new(ClaimTypes.GivenName, nd.HoTen),
            new(ClaimTypes.Role, nd.VaiTro.ToString())
        };
        if (nd.MaKhachHang is int kh) claims.Add(new Claim(ClaimMaKhachHang, kh.ToString()));
        return new ClaimsPrincipal(new ClaimsIdentity(claims, scheme));
    }

    public static int? MaKhachHang(this ClaimsPrincipal user) =>
        int.TryParse(user.FindFirstValue(ClaimMaKhachHang), out var ma) ? ma : null;

    public static string HoTen(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.GivenName) ?? user.Identity?.Name ?? "";
}
