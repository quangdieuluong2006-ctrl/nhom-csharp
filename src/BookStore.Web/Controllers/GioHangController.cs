using BookStore.Application.Services;
using BookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>Giỏ hàng lưu trong Session (Lab05 – GiohangController).</summary>
public class GioHangController : Controller
{
    private readonly ISachService _sach;
    public GioHangController(ISachService sach) => _sach = sach;

    public IActionResult Index() => View(GioHang.Doc(HttpContext.Session));

    /// <summary>Thêm sách vào giỏ; nếu đã có thì cộng dồn số lượng (không vượt quá tồn kho).</summary>
    [HttpPost]
    public async Task<IActionResult> Them(int maSach, int soLuong = 1, string? returnUrl = null)
    {
        var sach = await _sach.LayChiTietAsync(maSach);
        if (sach is null) return NotFound();

        var gio = GioHang.Doc(HttpContext.Session);
        var dong = gio.Dong.FirstOrDefault(d => d.MaSach == maSach);
        int moi = (dong?.SoLuong ?? 0) + Math.Max(soLuong, 1);
        if (moi > sach.SoLuongTon)
        {
            TempData["Loi"] = $"Kho chỉ còn {sach.SoLuongTon} cuốn “{sach.TenSach}”.";
            return RedirectToAction("ChiTiet", "Sach", new { id = maSach });
        }

        if (dong is null)
            gio.Dong.Add(new DongGioHang { MaSach = sach.MaSach, TenSach = sach.TenSach, HinhAnh = sach.HinhAnh, DonGia = sach.DonGia, SoLuong = moi });
        else
            dong.SoLuong = moi;
        gio.Luu(HttpContext.Session);

        TempData["ThongBao"] = $"Đã thêm “{sach.TenSach}” vào giỏ hàng.";
        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction(nameof(Index));
    }

    /// <summary>Cập nhật số lượng của một dòng (US02: không cho nhỏ hơn 1 – muốn bỏ sách thì dùng nút Xóa).</summary>
    [HttpPost]
    public async Task<IActionResult> CapNhat(int maSach, int soLuong)
    {
        var gio = GioHang.Doc(HttpContext.Session);
        var dong = gio.Dong.FirstOrDefault(d => d.MaSach == maSach);
        if (dong is null) return RedirectToAction(nameof(Index));

        if (soLuong < 1)
        {
            TempData["Loi"] = "Số lượng tối thiểu là 1. Muốn bỏ sách khỏi giỏ, hãy bấm nút Xóa.";
            return RedirectToAction(nameof(Index));
        }

        var sach = await _sach.LayChiTietAsync(maSach);
        int ton = sach?.SoLuongTon ?? 0;
        if (ton == 0)
        {
            // Sách vừa hết hàng hoặc ngừng kinh doanh sau khi khách bỏ vào giỏ
            gio.Dong.Remove(dong);
            TempData["Loi"] = $"“{dong.TenSach}” đã hết hàng nên được bỏ khỏi giỏ.";
        }
        else if (soLuong > ton)
        {
            dong.SoLuong = ton;
            TempData["Loi"] = $"Kho chỉ còn {ton} cuốn “{dong.TenSach}”.";
        }
        else
        {
            dong.SoLuong = soLuong;
        }
        gio.Luu(HttpContext.Session);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Xoa(int maSach)
    {
        var gio = GioHang.Doc(HttpContext.Session);
        gio.Dong.RemoveAll(d => d.MaSach == maSach);
        gio.Luu(HttpContext.Session);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult XoaTatCa()
    {
        GioHang.Xoa(HttpContext.Session);
        return RedirectToAction(nameof(Index));
    }
}
