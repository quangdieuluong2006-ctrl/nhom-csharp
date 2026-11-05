using BookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.ViewComponents;

/// <summary>Biểu tượng giỏ hàng kèm số lượng trên thanh điều hướng (Lab05 – GiohangPartial).</summary>
public class GioHangTomTatViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View(GioHang.Doc(HttpContext.Session));
}
