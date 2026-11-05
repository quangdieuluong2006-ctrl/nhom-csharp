using BookStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>Trang chủ: hiển thị sách mới (Lab01 – BookStoreController.Index).</summary>
public class HomeController : Controller
{
    private readonly ISachService _sach;
    public HomeController(ISachService sach) => _sach = sach;

    public async Task<IActionResult> Index() => View(await _sach.LaySachMoiAsync(8));
}
