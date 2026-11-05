using BookStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.ViewComponents;

/// <summary>Menu danh mục bên trái (thay cho Partial View "ChudePartial" của Lab02).</summary>
public class MenuDanhMucViewComponent : ViewComponent
{
    private readonly IDanhMucService _svc;
    public MenuDanhMucViewComponent(IDanhMucService svc) => _svc = svc;

    public async Task<IViewComponentResult> InvokeAsync() => View(await _svc.LayDanhMucAsync());
}
