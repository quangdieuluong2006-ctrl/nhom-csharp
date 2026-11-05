using System.Security.Claims;
using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Domain.Enums;
using BookStore.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Web.Controllers;

/// <summary>Đăng ký, đăng nhập, đăng xuất (Lab04 – NguoidungController). Dùng Cookie Authentication thay cho Session["Taikhoan"].</summary>
public class TaiKhoanController : Controller
{
    private readonly ITaiKhoanService _svc;
    private readonly ILogger<TaiKhoanController> _log;

    public TaiKhoanController(ITaiKhoanService svc, ILogger<TaiKhoanController> log)
    {
        _svc = svc;
        _log = log;
    }

    [HttpGet]
    public IActionResult DangKy() => View(new DangKyDto());

    [HttpPost]
    public async Task<IActionResult> DangKy(DangKyDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        try
        {
            var nd = await _svc.DangKyAsync(dto);
            await DangNhapCookieAsync(nd, ghiNho: false);
            _log.LogInformation("Khách hàng mới đăng ký: {TenDangNhap}", nd.TenDangNhap);
            TempData["ThongBao"] = $"Đăng ký thành công. Chào mừng {nd.HoTen}!";
            return RedirectToAction("Index", "Home");
        }
        catch (BusinessException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    [HttpGet]
    public IActionResult DangNhap(string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new DangNhapDto());
    }

    [HttpPost]
    public async Task<IActionResult> DangNhap(DangNhapDto dto, string? returnUrl)
    {
        ViewBag.ReturnUrl = returnUrl;
        if (!ModelState.IsValid) return View(dto);

        var nd = await _svc.DangNhapAsync(dto.TenDangNhap, dto.MatKhau);
        if (nd is null)
        {
            _log.LogWarning("Đăng nhập thất bại: {TenDangNhap}", dto.TenDangNhap);
            ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
            return View(dto);
        }
        // Website dành cho khách hàng; nhân viên/quản trị dùng phần mềm quản trị (Desktop)
        if (nd.VaiTro != VaiTro.KhachHang || nd.MaKhachHang is null)
        {
            ModelState.AddModelError(string.Empty, "Tài khoản nhân viên vui lòng đăng nhập trên phần mềm quản trị.");
            return View(dto);
        }

        await DangNhapCookieAsync(nd, dto.GhiNho);
        TempData["ThongBao"] = $"Xin chào {nd.HoTen}!";
        // Chỉ chuyển hướng tới URL nội bộ để tránh lỗ hổng open redirect
        return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> DangXuat()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    // ===================== Tài khoản của tôi =====================

    private int MaTaiKhoan => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet, Authorize(Roles = "KhachHang")]
    public async Task<IActionResult> HoSo()
    {
        var hoSo = await _svc.LayHoSoAsync(MaTaiKhoan);
        return hoSo is null ? NotFound() : View(hoSo);
    }

    [HttpPost, Authorize(Roles = "KhachHang")]
    public async Task<IActionResult> HoSo(HoSoKhachHangDto dto)
    {
        dto.TenDangNhap = User.Identity!.Name!;
        if (!ModelState.IsValid) return View(dto);
        try
        {
            await _svc.CapNhatHoSoAsync(MaTaiKhoan, dto);
            // Họ tên hiển thị nằm trong cookie (claim) → đăng nhập lại để cookie có tên mới
            var nd = new NguoiDungDto(MaTaiKhoan, dto.TenDangNhap, dto.HoTen.Trim(), VaiTro.KhachHang, User.MaKhachHang());
            var ghiNho = (await HttpContext.AuthenticateAsync()).Properties?.IsPersistent ?? false;
            await DangNhapCookieAsync(nd, ghiNho);
            TempData["ThongBao"] = "Đã cập nhật thông tin tài khoản.";
            return RedirectToAction(nameof(HoSo));
        }
        catch (BusinessException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    [HttpGet, Authorize(Roles = "KhachHang")]
    public IActionResult DoiMatKhau() => View(new DoiMatKhauDto());

    [HttpPost, Authorize(Roles = "KhachHang")]
    public async Task<IActionResult> DoiMatKhau(DoiMatKhauDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        try
        {
            await _svc.DoiMatKhauAsync(MaTaiKhoan, dto.MatKhauCu, dto.MatKhauMoi);
            _log.LogInformation("Tài khoản {MaTaiKhoan} đổi mật khẩu", MaTaiKhoan);
            TempData["ThongBao"] = "Đổi mật khẩu thành công.";
            return RedirectToAction(nameof(HoSo));
        }
        catch (BusinessException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(new DoiMatKhauDto());
        }
    }

    private Task DangNhapCookieAsync(NguoiDungDto nd, bool ghiNho) =>
        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            NguoiDungHienTai.TaoPrincipal(nd, CookieAuthenticationDefaults.AuthenticationScheme),
            new AuthenticationProperties { IsPersistent = ghiNho });
}
