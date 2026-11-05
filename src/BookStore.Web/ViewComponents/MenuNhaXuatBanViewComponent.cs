using BookStore.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.ViewComponents;

/// <summary>Menu nhà xuất bản bên trái (thay cho Partial View "NhaxuatbanPartial" của Lab02).</summary>
public class MenuNhaXuatBanViewComponent : ViewComponent
{
    private readonly IDanhMucService _svc;
    public MenuNhaXuatBanViewComponent(IDanhMucService svc) => _svc = svc;

    public async Task<IViewComponentResult> InvokeAsync() => View(await _svc.LayNhaXuatBanAsync());
}
