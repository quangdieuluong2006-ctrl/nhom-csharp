using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

/// <summary>
/// Xử lý đơn hàng phía cửa hàng (D06 – Quản trị viên, NV bán hàng) – phần B của quy trình liên thông:
/// khách đặt trên Web (Chờ xác nhận) → nhân viên xác nhận / giao / hoàn thành hoặc hủy trên Desktop
/// → khách thấy trạng thái mới ở trang "Đơn hàng của tôi".
/// </summary>
public partial interface IDonHangService
{
    Task<List<DonHangQtDto>> TimKiemQuanTriAsync(LocDonHang loc);
    Task<DonHangChiTietDto?> LayChiTietQuanTriAsync(long maDonHang);
    /// <summary>Chuyển đơn sang bước kế tiếp theo sơ đồ trạng thái (Bảng 4.6). Hoàn thành ⇒ đã thanh toán (COD).</summary>
    Task ChuyenTrangThaiAsync(long maDonHang, TrangThaiDonHang trangThaiMoi, int maNhanVien);
    /// <summary>Nhân viên hủy đơn Chờ xác nhận / Đã xác nhận / Đang giao (giao thất bại) và hoàn kho (BR13).</summary>
    Task HuyDonBoiNhanVienAsync(long maDonHang, int maNhanVien, string? lyDo);
}

public partial class DonHangService
{
    public async Task<List<DonHangQtDto>> TimKiemQuanTriAsync(LocDonHang loc)
    {
        var q = _db.DonHangs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(loc.TuKhoa))
        {
            var k = loc.TuKhoa.Trim().TrimStart('#');
            if (long.TryParse(k, out var ma))
                q = q.Where(d => d.MaDonHang == ma || d.SoDienThoaiNhan.Contains(k) || d.KhachHang.SoDienThoai.Contains(k));
            else
                q = q.Where(d => d.HoTenNhan.Contains(k) || d.KhachHang.HoTen.Contains(k));
        }
        if (loc.TrangThai is TrangThaiDonHang tt) q = q.Where(d => d.TrangThai == tt);
        if (loc.TuNgay is DateTime tu) q = q.Where(d => d.NgayDat >= tu.Date);
        if (loc.DenNgay is DateTime den) { var sau = den.Date.AddDays(1); q = q.Where(d => d.NgayDat < sau); }
        if (loc.MaKhachHang is int kh) q = q.Where(d => d.MaKhachHang == kh);

        return await q.OrderByDescending(d => d.NgayDat).Select(d => new DonHangQtDto
        {
            MaDonHang = d.MaDonHang, NgayDat = d.NgayDat, KhachHang = d.KhachHang.HoTen,
            HoTenNhan = d.HoTenNhan, SoDienThoaiNhan = d.SoDienThoaiNhan, TongTien = d.TongTien,
            TrangThai = d.TrangThai, TrangThaiThanhToan = d.TrangThaiThanhToan
        }).Take(500).ToListAsync();
    }

    public Task<DonHangChiTietDto?> LayChiTietQuanTriAsync(long maDonHang) =>
        ChonChiTiet(_db.DonHangs.Where(d => d.MaDonHang == maDonHang)).FirstOrDefaultAsync();

    public async Task ChuyenTrangThaiAsync(long maDonHang, TrangThaiDonHang trangThaiMoi, int maNhanVien)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        if (trangThaiMoi == TrangThaiDonHang.DaHuy)
            throw new BusinessException("Hủy đơn hàng dùng chức năng Hủy đơn (có hoàn kho).");

        var cu = await _db.DonHangs.Where(d => d.MaDonHang == maDonHang)
                     .Select(d => (TrangThaiDonHang?)d.TrangThai).FirstOrDefaultAsync()
                 ?? throw new BusinessException("Đơn hàng không tồn tại.");
        if (!DonHang.LaChuyenHopLe(cu, trangThaiMoi))
            throw new BusinessException($"Không thể chuyển đơn từ “{cu.Ten()}” sang “{trangThaiMoi.Ten()}”.");

        // UPDATE có điều kiện "WHERE TrangThai = trạng thái vừa đọc": nếu trong lúc đó khách đã hủy đơn trên Web
        // (hoặc nhân viên khác đã xử lý) thì cập nhật 0 dòng ⇒ báo cho nhân viên tải lại, không ghi đè.
        var q = _db.DonHangs.Where(d => d.MaDonHang == maDonHang && d.TrangThai == cu);
        int soDong = trangThaiMoi == TrangThaiDonHang.HoanThanh
            ? await q.ExecuteUpdateAsync(u => u
                .SetProperty(d => d.TrangThai, trangThaiMoi)
                .SetProperty(d => d.TrangThaiThanhToan, TrangThaiThanhToan.DaThanhToan)) // COD: giao xong là đã thu tiền
            : await q.ExecuteUpdateAsync(u => u.SetProperty(d => d.TrangThai, trangThaiMoi));
        if (soDong == 0)
            throw new BusinessException("Đơn hàng vừa được thay đổi bởi người khác (ví dụ khách đã hủy). Vui lòng tải lại danh sách.");
    }

    public async Task HuyDonBoiNhanVienAsync(long maDonHang, int maNhanVien, string? lyDo)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        await _db.ThucThiTrongGiaoTacAsync(async ct =>
        {
            var don = await _db.DonHangs.FirstOrDefaultAsync(d => d.MaDonHang == maDonHang, ct)
                      ?? throw new BusinessException("Đơn hàng không tồn tại.");
            var cu = don.TrangThai;
            if (!DonHang.LaChuyenHopLe(cu, TrangThaiDonHang.DaHuy))
                throw new BusinessException($"Đơn hàng đang ở trạng thái “{cu.Ten()}” nên không thể hủy.");

            int soDong = await _db.DonHangs.Where(d => d.MaDonHang == maDonHang && d.TrangThai == cu)
                .ExecuteUpdateAsync(u => u.SetProperty(d => d.TrangThai, TrangThaiDonHang.DaHuy), ct);
            if (soDong == 0)
                throw new BusinessException("Đơn hàng vừa được thay đổi bởi người khác. Vui lòng tải lại danh sách.");

            if (!string.IsNullOrWhiteSpace(lyDo))
            {
                var ghiChu = $"{(don.GhiChu is null ? "" : don.GhiChu + " | ")}Cửa hàng hủy: {lyDo.Trim()}";
                don.GhiChu = ghiChu.Length <= 500 ? ghiChu : ghiChu[..500];
                await _db.SaveChangesAsync(ct);
            }

            await HoanKhoAsync(maDonHang, ct);
            return true;
        });
    }
}
