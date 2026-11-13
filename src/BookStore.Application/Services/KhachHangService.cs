using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface IKhachHangService
{
    Task<List<KhachHangQtDto>> TimKiemAsync(int maNhanVien, string? tuKhoa);
    Task<KhachHangLuuDto?> LayAsync(int maNhanVien, int maKhachHang);
    Task CapNhatAsync(int maNhanVien, KhachHangLuuDto dto);
    Task<int> ThemAsync(int maNhanVien, KhachHangLuuDto dto);
    Task<bool> XoaAsync(int maNhanVien, int maKhachHang);
    Task DatTrangThaiTaiKhoanAsync(int maNhanVien, int maKhachHang, bool hoatDong);
}

/// <summary>
/// Quản lý khách hàng (D05 – Quản trị viên, NV bán hàng; US08): tra cứu, thêm, sửa, xóa, khóa/mở khóa tài khoản Web.
/// Khách thêm từ Desktop là khách vãng lai (mua tại quầy, đặt qua điện thoại) nên không có tài khoản Web;
/// khách muốn mua online thì tự đăng ký trên website.
/// </summary>
public class KhachHangService : IKhachHangService
{
    private readonly IAppDbContext _db;
    public KhachHangService(IAppDbContext db) => _db = db;

    public async Task<List<KhachHangQtDto>> TimKiemAsync(int maNhanVien, string? tuKhoa)
    {
        // Thông tin cá nhân của khách: chỉ quản trị viên, NV bán hàng được xem (Bảng 3.13, dòng 9)
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        var q = _db.KhachHangs.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var k = tuKhoa.Trim();
            q = q.Where(c => c.HoTen.Contains(k) || c.SoDienThoai.Contains(k) || (c.Email != null && c.Email.Contains(k)));
        }
        return await q.OrderBy(c => c.HoTen).Select(c => new KhachHangQtDto
        {
            MaKhachHang = c.MaKhachHang, HoTen = c.HoTen, SoDienThoai = c.SoDienThoai, Email = c.Email, DiaChi = c.DiaChi,
            TaiKhoan = c.TaiKhoan != null ? c.TaiKhoan.TenDangNhap : "—",
            TaiKhoanHoatDong = c.TaiKhoan != null ? c.TaiKhoan.TrangThai : null,
            SoDonHang = c.DonHangs.Count,
            TongChiTieu = c.DonHangs.Where(d => d.TrangThai == TrangThaiDonHang.HoanThanh).Sum(d => (decimal?)d.TongTien) ?? 0
        }).ToListAsync();
    }

    public async Task<KhachHangLuuDto?> LayAsync(int maNhanVien, int maKhachHang)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        return await _db.KhachHangs.AsNoTracking().Where(c => c.MaKhachHang == maKhachHang)
            .Select(c => new KhachHangLuuDto
            {
                MaKhachHang = c.MaKhachHang, HoTen = c.HoTen, SoDienThoai = c.SoDienThoai, Email = c.Email, DiaChi = c.DiaChi
            }).FirstOrDefaultAsync();
    }

    public async Task CapNhatAsync(int maNhanVien, KhachHangLuuDto dto)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        var kh = await _db.KhachHangs.Include(c => c.TaiKhoan).FirstOrDefaultAsync(c => c.MaKhachHang == dto.MaKhachHang)
                 ?? throw new BusinessException("Khách hàng không tồn tại.");
        var email = KiemTra.Email(dto.Email);
        if (email != null && await _db.KhachHangs.AnyAsync(c => c.Email == email && c.MaTaiKhoan != null && c.MaKhachHang != kh.MaKhachHang))
            throw new BusinessException("Email đã được dùng cho tài khoản khác.");

        kh.HoTen = KiemTra.BatBuoc(dto.HoTen, "Họ tên", 100);
        kh.SoDienThoai = KiemTra.SoDienThoai(dto.SoDienThoai);
        kh.Email = email;
        kh.DiaChi = KiemTra.TuyChon(dto.DiaChi, "Địa chỉ", 255);
        if (kh.TaiKhoan != null) kh.TaiKhoan.HoTen = kh.HoTen; // tên hiển thị trên Web
        await _db.SaveChangesAsync();
    }

    public async Task<int> ThemAsync(int maNhanVien, KhachHangLuuDto dto)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        var email = KiemTra.Email(dto.Email);
        if (email != null && await _db.KhachHangs.AnyAsync(c => c.Email == email && c.MaTaiKhoan != null))
            throw new BusinessException("Email đã được dùng cho tài khoản khác.");

        var kh = new KhachHang
        {
            HoTen = KiemTra.BatBuoc(dto.HoTen, "Họ tên", 100),
            SoDienThoai = KiemTra.SoDienThoai(dto.SoDienThoai),
            Email = email,
            DiaChi = KiemTra.TuyChon(dto.DiaChi, "Địa chỉ", 255)
        };
        if (await _db.KhachHangs.AnyAsync(c => c.SoDienThoai == kh.SoDienThoai))
            throw new BusinessException("Số điện thoại này đã có trong danh sách khách hàng.");
        _db.KhachHangs.Add(kh);
        await _db.SaveChangesAsync();
        return kh.MaKhachHang;
    }

    /// <summary>
    /// Xóa khách hàng. Khách đã có đơn hàng thì không xóa để giữ lịch sử đơn (BR09):
    /// nếu có tài khoản Web thì khóa tài khoản (trả về false), nếu là khách vãng lai thì báo lỗi.
    /// Khách chưa có đơn thì xóa hẳn, kèm tài khoản Web của khách (nếu có) trong cùng một lần lưu.
    /// </summary>
    /// <returns>true nếu đã xóa hẳn; false nếu chỉ khóa tài khoản.</returns>
    public async Task<bool> XoaAsync(int maNhanVien, int maKhachHang)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        var kh = await _db.KhachHangs.Include(c => c.TaiKhoan).FirstOrDefaultAsync(c => c.MaKhachHang == maKhachHang)
                 ?? throw new BusinessException("Khách hàng không tồn tại.");

        if (await _db.DonHangs.AnyAsync(d => d.MaKhachHang == maKhachHang))
        {
            if (kh.TaiKhoan is null)
                throw new BusinessException("Khách hàng đã có đơn hàng nên không thể xóa (cần giữ lịch sử đơn).");
            kh.TaiKhoan.TrangThai = false;
            await _db.SaveChangesAsync();
            return false;
        }

        _db.KhachHangs.Remove(kh);
        if (kh.TaiKhoan is not null) _db.TaiKhoans.Remove(kh.TaiKhoan);
        await _db.SaveChangesAsync(); // EF Core gộp hai lệnh DELETE trong một giao dịch
        return true;
    }

    /// <summary>Khóa / mở khóa tài khoản Web của khách. Tài khoản bị khóa mất phiên đăng nhập ngay ở yêu cầu kế tiếp.</summary>
    public async Task DatTrangThaiTaiKhoanAsync(int maNhanVien, int maKhachHang, bool hoatDong)
    {
        await _db.KiemTraAsync(maNhanVien, QuyenHan.BanHang);
        var kh = await _db.KhachHangs.Include(c => c.TaiKhoan).FirstOrDefaultAsync(c => c.MaKhachHang == maKhachHang)
                 ?? throw new BusinessException("Khách hàng không tồn tại.");
        if (kh.TaiKhoan is null)
            throw new BusinessException("Khách vãng lai không có tài khoản Web để khóa.");
        kh.TaiKhoan.TrangThai = hoatDong;
        await _db.SaveChangesAsync();
    }
}
