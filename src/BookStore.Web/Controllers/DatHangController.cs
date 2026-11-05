using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Web.Infrastructure;
using BookStore.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>
/// Đặt hàng (Lab06 – GiohangController.DatHang / Xacnhandonhang).
/// [Authorize]: bắt buộc đăng nhập; chưa đăng nhập sẽ được chuyển tới /TaiKhoan/DangNhap?ReturnUrl=/DatHang.
/// </summary>
[Authorize(Roles = "KhachHang")]
public class DatHangController : Controller
{
    private readonly IDonHangService _donHang;
    private readonly IAppDbContext _db;
    private readonly ILogger<DatHangController> _log;

    public DatHangController(IDonHangService donHang, IAppDbContext db, ILogger<DatHangController> log)
    {
        _donHang = donHang;
        _db = db;
        _log = log;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var gio = GioHang.Doc(HttpContext.Session);
        if (gio.Dong.Count == 0) return RedirectToAction("Index", "GioHang");

        // Điền sẵn thông tin người nhận từ hồ sơ khách hàng
        var maKh = User.MaKhachHang();
        var kh = await _db.KhachHangs.FindAsync(maKh);
        var tt = new ThongTinNhanHangDto
        {
            HoTenNhan = kh?.HoTen ?? User.HoTen(),
            SoDienThoaiNhan = kh?.SoDienThoai ?? "",
            DiaChiGiaoHang = kh?.DiaChi ?? ""
        };
        ViewBag.GioHang = gio;
        return View(tt);
    }

    [HttpPost]
    public async Task<IActionResult> XacNhan(ThongTinNhanHangDto tt)
    {
        var gio = GioHang.Doc(HttpContext.Session);
        if (gio.Dong.Count == 0) return RedirectToAction("Index", "GioHang");
        if (!ModelState.IsValid)
        {
            ViewBag.GioHang = gio;
            return View(nameof(Index), tt);
        }

        try
        {
            var maKh = User.MaKhachHang() ?? throw new BusinessException("Tài khoản chưa có hồ sơ khách hàng.");
            var maDon = await _donHang.DatHangAsync(maKh, tt,
                gio.Dong.Select(d => new DongDatHang(d.MaSach, d.SoLuong)).ToList());
            GioHang.Xoa(HttpContext.Session);
            _log.LogInformation("Khách {MaKhachHang} đặt đơn {MaDonHang}", maKh, maDon);
            return RedirectToAction(nameof(ThanhCong), new { id = maDon });
        }
        catch (BusinessException ex)
        {
            // Lỗi nghiệp vụ (hết hàng, sách ngừng bán…): giữ nguyên giỏ, báo cho khách sửa lại
            _log.LogWarning("Đặt hàng thất bại: {LyDo}", ex.Message);
            TempData["Loi"] = ex.Message;
            return RedirectToAction("Index", "GioHang");
        }
    }

    [HttpGet]
    public async Task<IActionResult> ThanhCong(long id)
    {
        var don = await _donHang.LayChiTietAsync(id, User.MaKhachHang() ?? 0);
        return don is null ? NotFound() : View(don);
    }
}
