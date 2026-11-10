using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface IKhachHangService
{
    Task<List<KhachHangQtDto>> TimKiemAsync(int maNhanVien, string? tuKhoa);
    Task<KhachHangLuuDto?> LayAsync(int maNhanVien, int maKhachHang);
    Task CapNhatAsync(int maNhanVien, KhachHangLuuDto dto);
}

/// <summary>Tra cứu, cập nhật thông tin khách hàng (D05 – Quản trị viên, NV bán hàng; US08).</summary>
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
}
