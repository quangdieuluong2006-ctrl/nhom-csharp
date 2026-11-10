namespace BookStore.Domain.Enums;

/// <summary>
/// Phương thức thanh toán, lưu dạng chuỗi trong cột DONHANG.PhuongThucThanhToan (VARCHAR(20), mặc định 'COD').
/// - COD: thanh toán khi nhận hàng – đơn Hoàn thành thì tự ghi nhận Đã thanh toán.
/// - Chuyển khoản, MoMo: khách trả trước bằng mã QR; nhân viên xác nhận đã nhận tiền trên Desktop rồi mới giao hàng.
/// </summary>
public static class PhuongThuc
{
    public const string COD = "COD";
    public const string ChuyenKhoan = "ChuyenKhoan";
    public const string MoMo = "MoMo";

    public static readonly string[] TatCa = { COD, ChuyenKhoan, MoMo };

    public static bool HopLe(string? ma) => ma is not null && TatCa.Contains(ma);

    /// <summary>Trả trước (chuyển khoản, MoMo) – cần nhân viên xác nhận đã nhận tiền.</summary>
    public static bool LaTraTruoc(string? ma) => ma is ChuyenKhoan or MoMo;

    public static string Ten(string? ma) => ma switch
    {
        ChuyenKhoan => "Chuyển khoản ngân hàng",
        MoMo => "Ví MoMo",
        _ => "Thanh toán khi nhận hàng (COD)"
    };

    public static string TenNgan(string? ma) => ma switch
    {
        ChuyenKhoan => "Chuyển khoản",
        MoMo => "MoMo",
        _ => "COD"
    };

    /// <summary>Nội dung chuyển khoản khách cần ghi để cửa hàng đối soát với đơn.</summary>
    public static string NoiDungChuyenKhoan(long maDonHang) => $"BOOKSTORE DH{maDonHang}";
}
