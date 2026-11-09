using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface IBaoCaoService
{
    /// <summary>Doanh thu theo ngày trong khoảng (Bảng 4.6). Chỉ tính đơn "Hoàn thành" (US16); ngày không có đơn = 0.</summary>
    Task<List<DoanhThuNgayDto>> DoanhThuAsync(DateTime tuNgay, DateTime denNgay);
    Task<List<SachBanChayDto>> SachBanChayAsync(DateTime tuNgay, DateTime denNgay, int soLuong = 10);
    Task<TongQuanDto> TongQuanAsync();
}

/// <summary>
/// Báo cáo thống kê (D09). Doanh thu ghi nhận theo ngày đặt của các đơn đã hoàn thành
/// (CSDL chưa lưu ngày hoàn thành – xem phần hạn chế trong báo cáo).
/// </summary>
public class BaoCaoService : IBaoCaoService
{
    private readonly IAppDbContext _db;
    public BaoCaoService(IAppDbContext db) => _db = db;

    private IQueryable<Domain.Entities.DonHang> DonHoanThanh(DateTime tuNgay, DateTime denNgay)
    {
        var tu = tuNgay.Date;
        var sau = denNgay.Date.AddDays(1);
        return _db.DonHangs.AsNoTracking()
            .Where(d => d.TrangThai == TrangThaiDonHang.HoanThanh && d.NgayDat >= tu && d.NgayDat < sau);
    }

    public async Task<List<DoanhThuNgayDto>> DoanhThuAsync(DateTime tuNgay, DateTime denNgay)
    {
        if (denNgay.Date < tuNgay.Date) throw new BusinessException("Ngày kết thúc phải sau ngày bắt đầu.");
        if ((denNgay.Date - tuNgay.Date).TotalDays > 366) throw new BusinessException("Chỉ xem tối đa 1 năm mỗi lần.");

        var theoNgay = await DonHoanThanh(tuNgay, denNgay)
            .GroupBy(d => d.NgayDat.Date)
            .Select(g => new { Ngay = g.Key, SoDon = g.Count(), DoanhThu = g.Sum(d => d.TongTien) })
            .ToDictionaryAsync(x => x.Ngay);

        // Bổ sung các ngày không có đơn để biểu đồ liên tục
        var kq = new List<DoanhThuNgayDto>();
        for (var ngay = tuNgay.Date; ngay <= denNgay.Date; ngay = ngay.AddDays(1))
            kq.Add(theoNgay.TryGetValue(ngay, out var x)
                ? new DoanhThuNgayDto { Ngay = ngay, SoDon = x.SoDon, DoanhThu = x.DoanhThu }
                : new DoanhThuNgayDto { Ngay = ngay });
        return kq;
    }

    public async Task<List<SachBanChayDto>> SachBanChayAsync(DateTime tuNgay, DateTime denNgay, int soLuong = 10)
    {
        var maDons = DonHoanThanh(tuNgay, denNgay).Select(d => d.MaDonHang);
        return await _db.ChiTietDonHangs.AsNoTracking()
            .Where(c => maDons.Contains(c.MaDonHang))
            .GroupBy(c => new { c.MaSach, c.Sach.TenSach })
            .Select(g => new SachBanChayDto
            {
                MaSach = g.Key.MaSach, TenSach = g.Key.TenSach,
                SoLuongBan = g.Sum(c => c.SoLuong), DoanhThu = g.Sum(c => c.SoLuong * c.DonGiaDat)
            })
            .OrderByDescending(x => x.SoLuongBan).ThenByDescending(x => x.DoanhThu)
            .Take(soLuong).ToListAsync();
    }

    public async Task<TongQuanDto> TongQuanAsync()
    {
        var homNay = DateTime.Today;
        var dauThang = new DateTime(homNay.Year, homNay.Month, 1);
        var hoanThanhThang = DonHoanThanh(dauThang, homNay);
        return new TongQuanDto
        {
            DonChoXacNhan = await _db.DonHangs.CountAsync(d => d.TrangThai == TrangThaiDonHang.ChoXacNhan),
            DonDangXuLy = await _db.DonHangs.CountAsync(d => d.TrangThai == TrangThaiDonHang.DaXacNhan || d.TrangThai == TrangThaiDonHang.DangGiao),
            DoanhThuHomNay = await hoanThanhThang.Where(d => d.NgayDat >= homNay).SumAsync(d => d.TongTien),
            DoanhThuThangNay = await hoanThanhThang.SumAsync(d => d.TongTien),
            SachSapHet = await _db.Sachs.CountAsync(s => s.TrangThai && s.SoLuongTon <= KhoService.NguongSapHet),
            TongDauSach = await _db.Sachs.CountAsync(s => s.TrangThai),
            TongKhachHang = await _db.KhachHangs.CountAsync()
        };
    }
}
