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

    /// <summary>Cập nhật số lượng của một dòng; số lượng ≤ 0 thì xóa dòng.</summary>
    [HttpPost]
    public async Task<IActionResult> CapNhat(int maSach, int soLuong)
    {
        var gio = GioHang.Doc(HttpContext.Session);
        var dong = gio.Dong.FirstOrDefault(d => d.MaSach == maSach);
        if (dong is null) return RedirectToAction(nameof(Index));

        if (soLuong <= 0)
            gio.Dong.Remove(dong);
        else
        {
            var sach = await _sach.LayChiTietAsync(maSach);
            int ton = sach?.SoLuongTon ?? 0;
            if (soLuong > ton)
            {
                TempData["Loi"] = $"Kho chỉ còn {ton} cuốn “{dong.TenSach}”.";
                soLuong = ton;
            }
            if (soLuong == 0) gio.Dong.Remove(dong); else dong.SoLuong = soLuong;
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
