using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface ISachService
{
    Task<IReadOnlyList<SachTomTatDto>> LaySachMoiAsync(int soLuong);
    Task<TrangKetQua<SachTomTatDto>> TimKiemAsync(LocSach loc);
    Task<SachChiTietDto?> LayChiTietAsync(int maSach);
    Task<IReadOnlyList<SachTomTatDto>> LaySachCungDanhMucAsync(int maDanhMuc, int boQuaMaSach, int soLuong);
}

/// <summary>Nghiệp vụ tra cứu sách phía người dùng (Lab 02, 03, 07). Chỉ trả về sách đang kinh doanh.</summary>
public class SachService : ISachService
{
    private readonly IAppDbContext _db;

    public SachService(IAppDbContext db) => _db = db;

    private IQueryable<Sach> SachDangBan() => _db.Sachs.AsNoTracking().Where(s => s.TrangThai);

    // Chuyển thực thể sang DTO ngay trong câu truy vấn để EF Core chỉ SELECT các cột cần dùng.
    private static IQueryable<SachTomTatDto> ChonTomTat(IQueryable<Sach> q) =>
        q.Select(s => new SachTomTatDto(
            s.MaSach, s.TenSach,
            string.Join(", ", s.SachTacGias.Select(t => t.TacGia.TenTacGia)),
            s.DonGia, s.HinhAnh, s.SoLuongTon));

    public async Task<IReadOnlyList<SachTomTatDto>> LaySachMoiAsync(int soLuong) =>
        await ChonTomTat(SachDangBan().OrderByDescending(s => s.NgayCapNhat).Take(soLuong)).ToListAsync();

    public async Task<TrangKetQua<SachTomTatDto>> TimKiemAsync(LocSach loc)
    {
        var q = SachDangBan();
        if (!string.IsNullOrWhiteSpace(loc.TuKhoa))
        {
            var k = loc.TuKhoa.Trim();
            q = q.Where(s => s.TenSach.Contains(k) || s.ISBN == k
                             || s.SachTacGias.Any(t => t.TacGia.TenTacGia.Contains(k)));
        }
        if (loc.MaDanhMuc is int dm)
            q = q.Where(s => s.MaDanhMuc == dm || s.DanhMuc.MaDanhMucCha == dm);
        if (loc.MaNhaXuatBan is int nxb) q = q.Where(s => s.MaNhaXuatBan == nxb);
        if (loc.GiaTu is decimal tu) q = q.Where(s => s.DonGia >= tu);
        if (loc.GiaDen is decimal den) q = q.Where(s => s.DonGia <= den);

        q = loc.SapXep switch
        {
            SapXepSach.GiaTang => q.OrderBy(s => s.DonGia),
            SapXepSach.GiaGiam => q.OrderByDescending(s => s.DonGia),
            SapXepSach.TenAZ => q.OrderBy(s => s.TenSach),
            _ => q.OrderByDescending(s => s.NgayCapNhat)
        };

        var trang = Math.Max(1, loc.Trang);
        var tong = await q.CountAsync();
        var items = await ChonTomTat(q.Skip((trang - 1) * loc.KichThuocTrang).Take(loc.KichThuocTrang)).ToListAsync();
        return new TrangKetQua<SachTomTatDto> { Items = items, Trang = trang, KichThuocTrang = loc.KichThuocTrang, TongSo = tong };
    }

    public async Task<SachChiTietDto?> LayChiTietAsync(int maSach) =>
        await SachDangBan().Where(s => s.MaSach == maSach)
            .Select(s => new SachChiTietDto(
                s.MaSach, s.TenSach,
                string.Join(", ", s.SachTacGias.Select(t => t.TacGia.TenTacGia)),
                s.NhaXuatBan != null ? s.NhaXuatBan.TenNhaXuatBan : null, s.MaNhaXuatBan,
                s.DanhMuc.TenDanhMuc, s.MaDanhMuc, s.NamXuatBan, s.ISBN,
                s.DonGia, s.SoLuongTon, s.MoTa, s.HinhAnh))
            .FirstOrDefaultAsync();

    public async Task<IReadOnlyList<SachTomTatDto>> LaySachCungDanhMucAsync(int maDanhMuc, int boQuaMaSach, int soLuong) =>
        await ChonTomTat(SachDangBan().Where(s => s.MaDanhMuc == maDanhMuc && s.MaSach != boQuaMaSach)
            .OrderByDescending(s => s.NgayCapNhat).Take(soLuong)).ToListAsync();
}
