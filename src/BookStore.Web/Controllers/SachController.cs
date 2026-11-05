using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace BookStore.Web.Controllers;

/// <summary>
/// Danh sách và chi tiết sách.
/// Gộp các action của Lab02–03 (SPTheochude, SPTheoNXB, Details) và phân trang Lab07 vào một trang lọc chung:
///   /Sach?danhmuc=2   – sách theo danh mục (chủ đề)
///   /Sach?nxb=1       – sách theo nhà xuất bản
///   /Sach?tukhoa=...  – tìm kiếm theo tên sách, tác giả, ISBN
/// </summary>
public class SachController : Controller
{
    private const int KichThuocTrang = 12;
    private readonly ISachService _sach;
    private readonly IDanhMucService _danhMuc;

    public SachController(ISachService sach, IDanhMucService danhMuc)
    {
        _sach = sach;
        _danhMuc = danhMuc;
    }

    public async Task<IActionResult> Index(string? tukhoa, int? danhmuc, int? nxb, decimal? giatu, decimal? giaden,
        SapXepSach sapxep = SapXepSach.MoiNhat, int trang = 1)
    {
        var loc = new LocSach
        {
            TuKhoa = tukhoa?.Trim(), MaDanhMuc = danhmuc, MaNhaXuatBan = nxb, GiaTu = giatu, GiaDen = giaden,
            SapXep = sapxep, Trang = Math.Max(trang, 1), KichThuocTrang = KichThuocTrang
        };
        var kq = await _sach.TimKiemAsync(loc);

        var vm = new DanhSachSachViewModel
        {
            Loc = loc,
            // StaticPagedList: dữ liệu đã được phân trang ở CSDL (Skip/Take), chỉ cần bọc lại để dùng PagedListPager
            Sachs = new StaticPagedList<SachTomTatDto>(kq.Items, kq.Trang, kq.KichThuocTrang, kq.TongSo),
            TieuDe = danhmuc is int d ? await _danhMuc.LayTenDanhMucAsync(d) ?? "Danh mục"
                   : nxb is int n ? await _danhMuc.LayTenNhaXuatBanAsync(n) ?? "Nhà xuất bản"
                   : !string.IsNullOrWhiteSpace(tukhoa) ? $"Kết quả tìm kiếm “{tukhoa.Trim()}”"
                   : "Tất cả sách"
        };
        return View(vm);
    }

    public async Task<IActionResult> ChiTiet(int id)
    {
        var sach = await _sach.LayChiTietAsync(id);
        if (sach is null) return NotFound();
        ViewBag.CungDanhMuc = await _sach.LaySachCungDanhMucAsync(sach.MaDanhMuc, sach.MaSach, 4);
        return View(sach);
    }
}
