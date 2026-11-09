using System.Text.RegularExpressions;

namespace BookStore.Application.Common;

/// <summary>Các hàm kiểm tra dữ liệu nhập dùng chung (Desktop không có DataAnnotations như form Web).</summary>
public static partial class KiemTra
{
    public static string BatBuoc(string? giaTri, string tenTruong, int doDaiToiDa)
    {
        var s = giaTri?.Trim();
        if (string.IsNullOrEmpty(s)) throw new BusinessException($"{tenTruong} không được để trống.");
        if (s.Length > doDaiToiDa) throw new BusinessException($"{tenTruong} tối đa {doDaiToiDa} ký tự.");
        return s;
    }

    public static string? TuyChon(string? giaTri, string tenTruong, int doDaiToiDa)
    {
        var s = giaTri?.Trim();
        if (string.IsNullOrEmpty(s)) return null;
        if (s.Length > doDaiToiDa) throw new BusinessException($"{tenTruong} tối đa {doDaiToiDa} ký tự.");
        return s;
    }

    public static string SoDienThoai(string? giaTri)
    {
        var s = BatBuoc(giaTri, "Số điện thoại", 15);
        if (!SdtRegex().IsMatch(s)) throw new BusinessException("Số điện thoại gồm 10–11 chữ số, bắt đầu bằng 0.");
        return s;
    }

    public static string? SoDienThoaiTuyChon(string? giaTri) =>
        string.IsNullOrWhiteSpace(giaTri) ? null : SoDienThoai(giaTri);

    public static string? Email(string? giaTri)
    {
        var s = TuyChon(giaTri, "Email", 254);
        if (s != null && !EmailRegex().IsMatch(s)) throw new BusinessException("Email không hợp lệ.");
        return s;
    }

    [GeneratedRegex(@"^0\d{9,10}$")] private static partial Regex SdtRegex();
    [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")] private static partial Regex EmailRegex();
}
