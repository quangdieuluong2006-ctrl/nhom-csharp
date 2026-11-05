using BookStore.Application.Common;
using BookStore.Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface IDanhMucService
{
    Task<IReadOnlyList<MucMenuDto>> LayDanhMucAsync();
    Task<IReadOnlyList<MucMenuDto>> LayNhaXuatBanAsync();
    Task<string?> LayTenDanhMucAsync(int ma);
    Task<string?> LayTenNhaXuatBanAsync(int ma);
}

/// <summary>Dữ liệu cho menu trái: danh mục và nhà xuất bản (Lab 02).</summary>
public class DanhMucService : IDanhMucService
{
    private readonly IAppDbContext _db;

    public DanhMucService(IAppDbContext db) => _db = db;

    /// <summary>Cây danh mục 2 cấp: danh mục gốc kèm danh sách danh mục con.</summary>
    public async Task<IReadOnlyList<MucMenuDto>> LayDanhMucAsync()
    {
        var ds = await _db.DanhMucs.AsNoTracking().Where(d => d.TrangThai)
            .Select(d => new { d.MaDanhMuc, d.MaDanhMucCha, d.TenDanhMuc, SoSach = d.Sachs.Count(s => s.TrangThai) })
            .ToListAsync();

        return ds.Where(d => d.MaDanhMucCha == null)
            .OrderBy(d => d.TenDanhMuc)
            .Select(cha =>
            {
                var con = ds.Where(c => c.MaDanhMucCha == cha.MaDanhMuc).OrderBy(c => c.TenDanhMuc)
                    .Select(c => new MucMenuDto(c.MaDanhMuc, c.TenDanhMuc, c.SoSach)).ToList();
                return new MucMenuDto(cha.MaDanhMuc, cha.TenDanhMuc, cha.SoSach + con.Sum(c => c.SoSach)) { Con = con };
            })
            .ToList();
    }

    public async Task<IReadOnlyList<MucMenuDto>> LayNhaXuatBanAsync() =>
        await _db.NhaXuatBans.AsNoTracking()
            .OrderBy(n => n.TenNhaXuatBan)
            .Select(n => new MucMenuDto(n.MaNhaXuatBan, n.TenNhaXuatBan, n.Sachs.Count(s => s.TrangThai)))
            .ToListAsync();

    public Task<string?> LayTenDanhMucAsync(int ma) =>
        _db.DanhMucs.Where(d => d.MaDanhMuc == ma).Select(d => d.TenDanhMuc).FirstOrDefaultAsync();

    public Task<string?> LayTenNhaXuatBanAsync(int ma) =>
        _db.NhaXuatBans.Where(n => n.MaNhaXuatBan == ma).Select(n => n.TenNhaXuatBan).FirstOrDefaultAsync();
}
