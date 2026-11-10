namespace BookStore.Web.Infrastructure;

/// <summary>
/// Mã QR nhận tiền của cửa hàng, đọc từ mục "ThanhToan" trong appsettings.
/// Ảnh QR thật chứa số tài khoản nên KHÔNG đưa lên GitHub: đặt file vào wwwroot/images/thanhtoan/
/// (thư mục đã được .gitignore, trừ ảnh mẫu) và khai báo đường dẫn trong appsettings.Local.json.
/// </summary>
public class CauHinhThanhToan
{
    public const string Muc = "ThanhToan";

    public KenhThanhToan ChuyenKhoan { get; set; } = new();
    public KenhThanhToan MoMo { get; set; } = new();

    public KenhThanhToan? Lay(string? phuongThuc) => phuongThuc switch
    {
        Domain.Enums.PhuongThuc.ChuyenKhoan => ChuyenKhoan,
        Domain.Enums.PhuongThuc.MoMo => MoMo,
        _ => null
    };
}

public class KenhThanhToan
{
    /// <summary>Đường dẫn ảnh QR trong wwwroot, ví dụ "/images/thanhtoan/qr-ngan-hang.png".</summary>
    public string AnhQR { get; set; } = "/images/thanhtoan/qr-mau.svg";
    /// <summary>Dòng thông tin hiện dưới mã QR, ví dụ "Vietcombank – STK 0123456789 – LUONG DIEU QUANG".</summary>
    public string ThongTin { get; set; } = "";
}
