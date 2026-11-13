using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public partial interface ISachService
{
    Task<IReadOnlyList<SachTomTatDto>> LaySachMoiAsync(int soLuong);
    Task<IReadOnlyList<SachTomTatDto>> LaySachBanChayAsync(int soLuong);
    Task<TrangKetQua<SachTomTatDto>> TimKiemAsync(LocSach loc);
    Task<SachChiTietDto?> LayChiTietAsync(int maSach);
    Task<IReadOnlyList<SachTomTatDto>> LaySachCungDanhMucAsync(int maDanhMuc, int boQuaMaSach, int soLuong);
}

/// <summary>Nghiệp vụ tra cứu sách phía người dùng (Lab 02, 03, 07). Chỉ trả về sách đang kinh doanh.</summary>
public partial class SachService : ISachService
{
    private readonly IAppDbContext _db;

    public SachService(IAppDbContext db) => _db = db;

    private IQueryable<Sach> SachDangBan() => _db.Sachs.AsNoTracking().Where(s => s.TrangThai);

    // Các dòng chi tiết thuộc đơn chưa bị hủy: căn cứ tính "đã bán" và xếp hạng "bán chạy".
    private IQueryable<ChiTietDonHang> DongDaBan() =>
        _db.ChiTietDonHangs.Where(c => c.DonHang.TrangThai != TrangThaiDonHang.DaHuy);

    // Chuyển thực thể sang DTO ngay trong câu truy vấn để EF Core chỉ SELECT các cột cần dùng
    // (số đã bán là truy vấn con SUM, SQL Server tính luôn trong cùng một câu lệnh).
    private IQueryable<SachTomTatDto> ChonTomTat(IQueryable<Sach> q) =>
        q.Select(s => new SachTomTatDto(
            s.MaSach, s.TenSach,
            string.Join(", ", s.SachTacGias.Select(t => t.TacGia.TenTacGia)),
            s.DonGia, s.HinhAnh, s.SoLuongTon)
        {
            DaBan = DongDaBan().Where(c => c.MaSach == s.MaSach).Sum(c => (int?)c.SoLuong) ?? 0
        });

    // Sắp theo số đã bán giảm dần; cùng số đã bán thì sách mới cập nhật đứng trước.
    private IOrderedQueryable<Sach> XepBanChay(IQueryable<Sach> q) =>
        q.OrderByDescending(s => DongDaBan().Where(c => c.MaSach == s.MaSach).Sum(c => (int?)c.SoLuong) ?? 0)
         .ThenByDescending(s => s.NgayCapNhat);

    public async Task<IReadOnlyList<SachTomTatDto>> LaySachMoiAsync(int soLuong) =>
        await ChonTomTat(SachDangBan().OrderByDescending(s => s.NgayCapNhat).Take(soLuong)).ToListAsync();

    /// <summary>Sách bán chạy cho trang chủ: chỉ lấy sách đã bán được ít nhất một cuốn.</summary>
    public async Task<IReadOnlyList<SachTomTatDto>> LaySachBanChayAsync(int soLuong) =>
        await ChonTomTat(XepBanChay(SachDangBan().Where(s => DongDaBan().Any(c => c.MaSach == s.MaSach))).Take(soLuong))
            .ToListAsync();

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
            SapXepSach.BanChay => XepBanChay(q),
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
