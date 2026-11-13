using BookStore.Application.Services;
using BookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>Trang chủ: hiển thị sách mới (Lab01 – BookStoreController.Index) và sách bán chạy.</summary>
public class HomeController : Controller
{
    private readonly ISachService _sach;
    public HomeController(ISachService sach) => _sach = sach;

    public async Task<IActionResult> Index() => View(new TrangChuViewModel
    {
        SachMoi = await _sach.LaySachMoiAsync(8),
        SachBanChay = await _sach.LaySachBanChayAsync(4)
    });
}
