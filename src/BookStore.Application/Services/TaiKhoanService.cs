using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public interface ITaiKhoanService
{
    Task<NguoiDungDto> DangKyAsync(DangKyDto dto);
    /// <summary>Trả về null nếu sai tên đăng nhập/mật khẩu hoặc tài khoản bị khóa.</summary>
    Task<NguoiDungDto?> DangNhapAsync(string tenDangNhap, string matKhau);
}

/// <summary>Đăng ký, đăng nhập (Lab04). Mật khẩu được băm bằng BCrypt, không lưu dạng rõ.</summary>
public class TaiKhoanService : ITaiKhoanService
{
    private readonly IAppDbContext _db;
    public TaiKhoanService(IAppDbContext db) => _db = db;

    public async Task<NguoiDungDto> DangKyAsync(DangKyDto dto)
    {
        var ten = dto.TenDangNhap.Trim();
        if (await _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == ten))
            throw new BusinessException("Tên đăng nhập đã được sử dụng.");
        var email = string.IsNullOrWhiteSpace(dto.Email) ? null : dto.Email.Trim();
        if (email != null && await _db.TaiKhoans.AnyAsync(t => t.Email == email))
            throw new BusinessException("Email đã được dùng cho tài khoản khác.");

        // Tạo đồng thời TAIKHOAN và KHACHHANG; EF lưu cả hai trong cùng một lệnh SaveChanges (một giao tác).
        var tk = new TaiKhoan
        {
            TenDangNhap = ten,
            MatKhauHash = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
            HoTen = dto.HoTen.Trim(),
            Email = email,
            SoDienThoai = dto.SoDienThoai.Trim(),
            VaiTro = VaiTro.KhachHang,
            KhachHang = new KhachHang
            {
                HoTen = dto.HoTen.Trim(), Email = email, SoDienThoai = dto.SoDienThoai.Trim(),
                DiaChi = string.IsNullOrWhiteSpace(dto.DiaChi) ? null : dto.DiaChi.Trim()
            }
        };
        _db.TaiKhoans.Add(tk);
        await _db.SaveChangesAsync();
        return new NguoiDungDto(tk.MaTaiKhoan, tk.TenDangNhap, tk.HoTen, tk.VaiTro, tk.KhachHang.MaKhachHang);
    }

    public async Task<NguoiDungDto?> DangNhapAsync(string tenDangNhap, string matKhau)
    {
        var tk = await _db.TaiKhoans.AsNoTracking().Include(t => t.KhachHang)
            .FirstOrDefaultAsync(t => t.TenDangNhap == tenDangNhap.Trim());
        // Không phân biệt "sai tên" hay "sai mật khẩu" để tránh lộ tên đăng nhập tồn tại.
        if (tk is null || !tk.TrangThai || !BCrypt.Net.BCrypt.Verify(matKhau, tk.MatKhauHash)) return null;
        return new NguoiDungDto(tk.MaTaiKhoan, tk.TenDangNhap, tk.HoTen, tk.VaiTro, tk.KhachHang?.MaKhachHang);
    }
}
