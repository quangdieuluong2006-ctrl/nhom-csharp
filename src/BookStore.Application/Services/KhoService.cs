using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface IKhoService
{
    /// <summary>Lập phiếu nhập/xuất/điều chỉnh (UC-D07, US15). Trả về mã phiếu.</summary>
    Task<long> LuuPhieuAsync(PhieuKhoDto phieu, int maNhanVien);
    Task<List<TonKhoDto>> LayTonKhoAsync(string? tuKhoa, bool chiSapHet);
    Task<List<PhieuKhoQtDto>> LayLichSuPhieuAsync(DateTime? tuNgay, DateTime? denNgay, LoaiPhieuKho? loai, int maNhanVien);
    Task<List<ChiTietPhieuQtDto>> LayChiTietPhieuAsync(long maPhieu, int maNhanVien);
}

/// <summary>
/// Nghiệp vụ kho (D07 – Quản trị viên, NV kho). Tồn kho chỉ thay đổi qua phiếu kho hoặc đơn hàng (BR06);
/// phiếu, chi tiết phiếu và tồn kho được ghi trong MỘT giao tác.
/// </summary>
public class KhoService : IKhoService
{
    /// <summary>Ngưỡng "sắp hết hàng" (US16: tồn ≤ 5).</summary>
    public const int NguongSapHet = 5;

    private readonly IAppDbContext _db;
    public KhoService(IAppDbContext db) => _db = db;

    public async Task<long> LuuPhieuAsync(PhieuKhoDto phieu, int maNhanVien)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.Kho);

        // Gộp các dòng trùng sách (UC-D07 – 2a: cộng dồn số lượng)
        var dong = phieu.Dong.GroupBy(d => d.MaSach)
            .Select(g => new DongPhieuKhoDto(g.Key, g.Sum(x => x.SoLuong), g.Last().DonGiaNhap)).ToList();
        if (dong.Count == 0) throw new BusinessException("Phiếu phải có ít nhất một dòng.");
        if (dong.Any(d => d.SoLuong <= 0)) throw new BusinessException("Số lượng mỗi dòng phải lớn hơn 0.");
        if (dong.Any(d => d.DonGiaNhap < 0)) throw new BusinessException("Đơn giá nhập không được âm.");
        if (phieu.LoaiPhieu == LoaiPhieuKho.Nhap && dong.Any(d => d.DonGiaNhap is null))
            throw new BusinessException("Phiếu nhập phải có đơn giá nhập cho từng dòng.");
        var ghiChu = KiemTra.TuyChon(phieu.GhiChu, "Ghi chú", 500);
        if (phieu.LoaiPhieu is LoaiPhieuKho.DieuChinhTang or LoaiPhieuKho.DieuChinhGiam && ghiChu is null)
            throw new BusinessException("Phiếu điều chỉnh cần ghi rõ lý do ở phần ghi chú.");

        return await _db.ThucThiTrongGiaoTacAsync(async ct =>
        {
            var maSachs = dong.Select(d => d.MaSach).ToList();
            var tenSach = await _db.Sachs.Where(s => maSachs.Contains(s.MaSach))
                .ToDictionaryAsync(s => s.MaSach, s => s.TenSach, ct);
            if (tenSach.Count != maSachs.Count) throw new BusinessException("Có sách trong phiếu không tồn tại.");

            bool tang = phieu.LoaiPhieu.LamTangTon();
            var p = new PhieuKho { MaTaiKhoan = maNhanVien, LoaiPhieu = phieu.LoaiPhieu, NgayLap = DateTime.Now, GhiChu = ghiChu };
            foreach (var d in dong)
            {
                int soDong = tang
                    ? await _db.Sachs.Where(s => s.MaSach == d.MaSach)
                        .ExecuteUpdateAsync(u => u.SetProperty(s => s.SoLuongTon, s => s.SoLuongTon + d.SoLuong), ct)
                    // Phiếu làm giảm tồn: chỉ trừ khi còn đủ (UPDATE có điều kiện) – tồn không bao giờ âm
                    : await _db.Sachs.Where(s => s.MaSach == d.MaSach && s.SoLuongTon >= d.SoLuong)
                        .ExecuteUpdateAsync(u => u.SetProperty(s => s.SoLuongTon, s => s.SoLuongTon - d.SoLuong), ct);
                if (soDong == 0)
                    throw new BusinessException($"Sách “{tenSach[d.MaSach]}” không đủ tồn kho để {phieu.LoaiPhieu.Ten().ToLower()}.");
                p.ChiTiets.Add(new ChiTietPhieuKho { MaSach = d.MaSach, SoLuong = d.SoLuong, DonGiaNhap = d.DonGiaNhap });
            }
            _db.PhieuKhos.Add(p);
            await _db.SaveChangesAsync(ct);
            return p.MaPhieu;
        });
    }

    public async Task<List<TonKhoDto>> LayTonKhoAsync(string? tuKhoa, bool chiSapHet)
    {
        var q = _db.Sachs.AsNoTracking().Where(s => s.TrangThai);
        if (!string.IsNullOrWhiteSpace(tuKhoa)) q = q.Where(s => s.TenSach.Contains(tuKhoa.Trim()));
        if (chiSapHet) q = q.Where(s => s.SoLuongTon <= NguongSapHet);
        return await q.OrderBy(s => s.SoLuongTon).ThenBy(s => s.TenSach).Select(s => new TonKhoDto
        {
            MaSach = s.MaSach, TenSach = s.TenSach, DanhMuc = s.DanhMuc.TenDanhMuc, SoLuongTon = s.SoLuongTon,
            TinhTrang = s.SoLuongTon == 0 ? "Hết hàng" : s.SoLuongTon <= NguongSapHet ? "Sắp hết" : "Còn hàng"
        }).ToListAsync();
    }

    public async Task<List<PhieuKhoQtDto>> LayLichSuPhieuAsync(DateTime? tuNgay, DateTime? denNgay, LoaiPhieuKho? loai, int maNhanVien)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.Kho);
        var q = _db.PhieuKhos.AsNoTracking();
        if (tuNgay is DateTime tu) q = q.Where(p => p.NgayLap >= tu.Date);
        if (denNgay is DateTime den) { var sau = den.Date.AddDays(1); q = q.Where(p => p.NgayLap < sau); }
        if (loai is LoaiPhieuKho l) q = q.Where(p => p.LoaiPhieu == l);
        return await q.OrderByDescending(p => p.NgayLap).Select(p => new PhieuKhoQtDto
        {
            MaPhieu = p.MaPhieu, NgayLap = p.NgayLap, LoaiPhieu = p.LoaiPhieu, NguoiLap = p.TaiKhoan.HoTen,
            SoDong = p.ChiTiets.Count, TongSoLuong = p.ChiTiets.Sum(c => c.SoLuong), GhiChu = p.GhiChu
        }).Take(500).ToListAsync();
    }

    public async Task<List<ChiTietPhieuQtDto>> LayChiTietPhieuAsync(long maPhieu, int maNhanVien)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.Kho);
        return await _db.ChiTietPhieuKhos.AsNoTracking().Where(c => c.MaPhieu == maPhieu)
            .Select(c => new ChiTietPhieuQtDto { MaSach = c.MaSach, TenSach = c.Sach.TenSach, SoLuong = c.SoLuong, DonGiaNhap = c.DonGiaNhap })
            .ToListAsync();
    }
}
