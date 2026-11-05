using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>Trang lỗi thân thiện: không lộ chi tiết kỹ thuật, chỉ hiện mã tra cứu (RequestId) để đối chiếu log.</summary>
[Route("Loi")]
public class LoiController : Controller
{
    private readonly ILogger<LoiController> _log;
    public LoiController(ILogger<LoiController> log) => _log = log;

    [Route("")]
    public IActionResult Index()
    {
        var loi = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        var ma = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        if (loi != null) _log.LogError(loi.Error, "Lỗi không xử lý tại {Path} (mã {Ma})", loi.Path, ma);
        ViewBag.MaTraCuu = ma;
        return View("Loi", 500);
    }

    [Route("{code:int}")]
    public IActionResult TheoMa(int code)
    {
        Response.StatusCode = code;
        return View("Loi", code);
    }
}
