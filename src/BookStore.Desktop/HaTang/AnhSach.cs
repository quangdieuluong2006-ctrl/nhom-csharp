using System.Text;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Ảnh bìa sách được Web phục vụ từ thư mục wwwroot/images/sach; CSDL chỉ lưu đường dẫn "/images/sach/ten-file".
/// Desktop chép file ảnh người dùng chọn vào thư mục đó để Web hiển thị được ngay (cùng máy chủ/cùng mã nguồn).
/// Thư mục lấy từ cấu hình "ThuMucAnhWeb"; nếu không có thì tự dò thư mục src/BookStore.Web/wwwroot khi chạy từ mã nguồn.
/// </summary>
public class AnhSach
{
    private const string TienToUrl = "/images/sach/";
    public string? ThuMucWwwroot { get; }

    public AnhSach(IConfiguration cfg)
    {
        var cauHinh = cfg["ThuMucAnhWeb"];
        ThuMucWwwroot = !string.IsNullOrWhiteSpace(cauHinh) && Directory.Exists(cauHinh) ? cauHinh : DoTimWwwroot();
    }

    private static string? DoTimWwwroot()
    {
        for (var d = new DirectoryInfo(AppContext.BaseDirectory); d != null; d = d.Parent)
        {
            var thu = Path.Combine(d.FullName, "src", "BookStore.Web", "wwwroot");
            if (Directory.Exists(thu)) return thu;
            thu = Path.Combine(d.FullName, "BookStore.Web", "wwwroot");
            if (Directory.Exists(thu)) return thu;
        }
        return null;
    }

    /// <summary>Đường dẫn file trên đĩa của ảnh có URL "/images/sach/x.png" (null nếu không tìm thấy).</summary>
    public string? DuongDanFile(string? url)
    {
        if (ThuMucWwwroot is null || string.IsNullOrWhiteSpace(url) || !url.StartsWith('/')) return null;
        var file = Path.Combine(ThuMucWwwroot, url.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
        return File.Exists(file) ? file : null;
    }

    /// <summary>Chép ảnh vào wwwroot/images/sach với tên không dấu, không trùng; trả về URL lưu vào CSDL.</summary>
    public string LuuAnh(string fileNguon, string tenSach)
    {
        if (ThuMucWwwroot is null)
            throw new Application.Common.BusinessException(
                "Không tìm thấy thư mục ảnh của Web. Hãy khai báo \"ThuMucAnhWeb\" trong appsettings.json.");
        var thuMuc = Path.Combine(ThuMucWwwroot, "images", "sach");
        Directory.CreateDirectory(thuMuc);
        var ten = $"{KhongDau(tenSach)}-{DateTime.Now:yyMMddHHmmss}{Path.GetExtension(fileNguon).ToLowerInvariant()}";
        File.Copy(fileNguon, Path.Combine(thuMuc, ten), overwrite: true);
        return TienToUrl + ten;
    }

    /// <summary>Mở ảnh mà không khóa file (đọc vào bộ nhớ). SVG không hiển thị được trong PictureBox → trả về null.</summary>
    public static Image? MoAnh(string? duongDan)
    {
        if (duongDan is null || duongDan.EndsWith(".svg", StringComparison.OrdinalIgnoreCase)) return null;
        try
        {
            using var ms = new MemoryStream(File.ReadAllBytes(duongDan));
            return Image.FromStream(ms);
        }
        catch { return null; }
    }

    /// <summary>Ảnh thay thế có dòng chữ (dùng khi sách chưa có ảnh hoặc ảnh dạng SVG).</summary>
    public static Image AnhThayThe(string dongChu)
    {
        var bmp = new Bitmap(250, 150);
        using var g = Graphics.FromImage(bmp);
        g.Clear(Color.FromArgb(233, 238, 242));
        using var font = new Font("Segoe UI", 9F);
        var vung = new RectangleF(10, 10, 230, 130);
        g.DrawString(dongChu, font, Brushes.DimGray, vung, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
        return bmp;
    }

    private static string KhongDau(string s)
    {
        var sb = new StringBuilder();
        foreach (var c in s.Replace('Đ', 'D').Replace('đ', 'd').Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark) continue;
            sb.Append(char.IsLetterOrDigit(c) ? char.ToLowerInvariant(c) : '-');
        }
        var kq = string.Join('-', sb.ToString().Split('-', StringSplitOptions.RemoveEmptyEntries));
        return kq.Length > 60 ? kq[..60] : (kq.Length == 0 ? "sach" : kq);
    }
}
