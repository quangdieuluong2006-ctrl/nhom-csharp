using BookStore.Application.Common;
using BookStore.Application.Services;
using BookStore.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>
/// "Đơn hàng của tôi": khách theo dõi trạng thái đơn do nhân viên cập nhật trên Desktop, và tự hủy đơn chờ xác nhận.
/// </summary>
[Authorize(Roles = "KhachHang")]
public class DonHangController : Controller
{
    private readonly IDonHangService _svc;
    private readonly ILogger<DonHangController> _log;

    public DonHangController(IDonHangService svc, ILogger<DonHangController> log)
    {
        _svc = svc;
        _log = log;
    }

    private int MaKhachHang => User.MaKhachHang() ?? 0;

    public async Task<IActionResult> Index() => View(await _svc.LayDonCuaKhachAsync(MaKhachHang));

    public async Task<IActionResult> ChiTiet(long id)
    {
        var don = await _svc.LayChiTietAsync(id, MaKhachHang);
        return don is null ? NotFound() : View(don);
    }

    [HttpPost]
    public async Task<IActionResult> Huy(long id)
    {
        try
        {
            await _svc.HuyDonAsync(id, MaKhachHang);
            _log.LogInformation("Khách {MaKhachHang} hủy đơn {MaDonHang}", MaKhachHang, id);
            TempData["ThongBao"] = $"Đã hủy đơn hàng #{id}.";
        }
        catch (BusinessException ex)
        {
            TempData["Loi"] = ex.Message;
        }
        return RedirectToAction(nameof(ChiTiet), new { id });
    }
}
