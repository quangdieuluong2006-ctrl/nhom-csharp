using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public partial interface IDonHangService
{
    /// <summary>Tạo đơn hàng từ giỏ, trừ tồn kho trong cùng một giao tác. Trả về mã đơn.</summary>
    Task<long> DatHangAsync(int maKhachHang, ThongTinNhanHangDto nhanHang, IReadOnlyList<DongDatHang> gioHang);
    Task<IReadOnlyList<DonHangTomTatDto>> LayDonCuaKhachAsync(int maKhachHang);
    /// <summary>Chi tiết đơn; chỉ trả về nếu đơn thuộc đúng khách hàng (chống xem đơn của người khác).</summary>
    Task<DonHangChiTietDto?> LayChiTietAsync(long maDonHang, int maKhachHang);
    /// <summary>Khách tự hủy đơn khi đơn còn "Chờ xác nhận"; hoàn lại tồn kho.</summary>
    Task HuyDonAsync(long maDonHang, int maKhachHang);
}

/// <summary>
/// Nghiệp vụ đơn hàng phía khách (Lab06 + quy trình liên thông Web ↔ Desktop trong báo cáo):
/// Web tạo đơn "Chờ xác nhận" → Desktop xác nhận/giao/hoàn thành → khách theo dõi trạng thái trên Web.
/// </summary>
public partial class DonHangService : IDonHangService
{
    private readonly IAppDbContext _db;
    public DonHangService(IAppDbContext db) => _db = db;

    public async Task<long> DatHangAsync(int maKhachHang, ThongTinNhanHangDto nhanHang, IReadOnlyList<DongDatHang> gioHang)
    {
        // Gộp các dòng trùng mã sách và kiểm tra dữ liệu đầu vào trước khi chạm vào CSDL
        var dong = gioHang.GroupBy(d => d.MaSach)
            .Select(g => new DongDatHang(g.Key, g.Sum(x => x.SoLuong))).ToList();
        if (dong.Count == 0) throw new BusinessException("Giỏ hàng đang trống.");
        if (dong.Any(d => d.SoLuong <= 0)) throw new BusinessException("Số lượng sách không hợp lệ.");
        if (!PhuongThuc.HopLe(nhanHang.PhuongThucThanhToan))
            throw new BusinessException("Phương thức thanh toán không hợp lệ.");

        return await _db.ThucThiTrongGiaoTacAsync(async ct =>
        {
            var maSachs = dong.Select(d => d.MaSach).ToList();
            // Giá lấy từ CSDL tại thời điểm đặt, KHÔNG tin giá gửi lên từ trình duyệt
            var sachs = await _db.Sachs.Where(s => maSachs.Contains(s.MaSach))
                .Select(s => new { s.MaSach, s.TenSach, s.DonGia, s.TrangThai })
                .ToDictionaryAsync(s => s.MaSach, ct);

            var don = new DonHang
            {
                MaKhachHang = maKhachHang,
                NgayDat = DateTime.Now,
                NgayGiaoDuKien = DateTime.Today.AddDays(3),
                HoTenNhan = nhanHang.HoTenNhan.Trim(),
                SoDienThoaiNhan = nhanHang.SoDienThoaiNhan.Trim(),
                DiaChiGiaoHang = nhanHang.DiaChiGiaoHang.Trim(),
                GhiChu = string.IsNullOrWhiteSpace(nhanHang.GhiChu) ? null : nhanHang.GhiChu.Trim(),
                PhuongThucThanhToan = nhanHang.PhuongThucThanhToan
            };

            foreach (var d in dong)
            {
                if (!sachs.TryGetValue(d.MaSach, out var s) || !s.TrangThai)
                    throw new BusinessException("Có sách trong giỏ đã ngừng kinh doanh. Vui lòng cập nhật lại giỏ hàng.");

                // UPDATE có điều kiện: chỉ trừ khi còn đủ hàng. Hai khách đặt cùng lúc cuốn cuối cùng
                // thì chỉ một lệnh UPDATE thành công (0 dòng bị ảnh hưởng = không đủ hàng) → không bao giờ tồn âm.
                int soDong = await _db.Sachs
                    .Where(x => x.MaSach == d.MaSach && x.SoLuongTon >= d.SoLuong)
                    .ExecuteUpdateAsync(u => u.SetProperty(x => x.SoLuongTon, x => x.SoLuongTon - d.SoLuong), ct);
                if (soDong == 0)
                    throw new BusinessException($"Sách “{s.TenSach}” không đủ số lượng trong kho.");

                don.ChiTiets.Add(new ChiTietDonHang { MaSach = d.MaSach, SoLuong = d.SoLuong, DonGiaDat = s.DonGia });
            }

            don.TongTien = don.TinhTongTien();
            _db.DonHangs.Add(don);
            await _db.SaveChangesAsync(ct);
            return don.MaDonHang;
        });
    }

    public async Task<IReadOnlyList<DonHangTomTatDto>> LayDonCuaKhachAsync(int maKhachHang) =>
        await _db.DonHangs.AsNoTracking().Where(d => d.MaKhachHang == maKhachHang)
            .OrderByDescending(d => d.NgayDat)
            .Select(d => new DonHangTomTatDto(d.MaDonHang, d.NgayDat, d.TongTien, d.TrangThai, d.ChiTiets.Sum(c => c.SoLuong)))
            .ToListAsync();

    public Task<DonHangChiTietDto?> LayChiTietAsync(long maDonHang, int maKhachHang) =>
        ChonChiTiet(_db.DonHangs.Where(d => d.MaDonHang == maDonHang && d.MaKhachHang == maKhachHang)).FirstOrDefaultAsync();

    /// <summary>Phép chiếu (projection) đơn hàng → DTO chi tiết, dùng chung cho Web và Desktop.</summary>
    private static IQueryable<DonHangChiTietDto> ChonChiTiet(IQueryable<DonHang> q) =>
        q.AsNoTracking().Select(d => new DonHangChiTietDto(
            d.MaDonHang, d.NgayDat, d.NgayGiaoDuKien, d.HoTenNhan, d.SoDienThoaiNhan, d.DiaChiGiaoHang, d.GhiChu,
            d.TongTien, d.TrangThai, d.PhuongThucThanhToan, d.TrangThaiThanhToan,
            d.ChiTiets.Select(c => new ChiTietDonDto(c.MaSach, c.Sach.TenSach, c.Sach.HinhAnh, c.SoLuong, c.DonGiaDat, c.ThanhTien))
                .ToList()));

    public async Task HuyDonAsync(long maDonHang, int maKhachHang)
    {
        await _db.ThucThiTrongGiaoTacAsync(async ct =>
        {
            // Đổi trạng thái có điều kiện: nếu nhân viên vừa xác nhận đơn trên Desktop thì lệnh này cập nhật 0 dòng.
            int soDong = await _db.DonHangs
                .Where(d => d.MaDonHang == maDonHang && d.MaKhachHang == maKhachHang && d.TrangThai == TrangThaiDonHang.ChoXacNhan
                            && d.TrangThaiThanhToan == TrangThaiThanhToan.ChuaThanhToan)
                .ExecuteUpdateAsync(u => u.SetProperty(d => d.TrangThai, TrangThaiDonHang.DaHuy), ct);
            if (soDong == 0)
                throw new BusinessException("Đơn hàng đã được cửa hàng xử lý hoặc đã thanh toán nên không thể tự hủy. " +
                                            "Vui lòng liên hệ cửa hàng để được hỗ trợ.");

            await HoanKhoAsync(maDonHang, ct);
            return true;
        });
    }

    /// <summary>
    /// Hoàn lại tồn kho cho từng dòng của đơn bị hủy. Chỉ được gọi SAU KHI lệnh đổi trạng thái có điều kiện
    /// thành công (1 dòng), nên một đơn không bao giờ bị hoàn kho hai lần (BR07).
    /// </summary>
    private async Task HoanKhoAsync(long maDonHang, CancellationToken ct)
    {
        var chiTiets = await _db.ChiTietDonHangs.Where(c => c.MaDonHang == maDonHang)
            .Select(c => new { c.MaSach, c.SoLuong }).ToListAsync(ct);
        foreach (var c in chiTiets)
            await _db.Sachs.Where(s => s.MaSach == c.MaSach)
                .ExecuteUpdateAsync(u => u.SetProperty(s => s.SoLuongTon, s => s.SoLuongTon + c.SoLuong), ct);
    }
}
