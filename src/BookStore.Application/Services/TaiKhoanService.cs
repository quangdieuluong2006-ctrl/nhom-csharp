using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

public partial interface ITaiKhoanService
{
    Task<NguoiDungDto> DangKyAsync(DangKyDto dto);
    /// <summary>Trả về null nếu sai tên đăng nhập/mật khẩu hoặc tài khoản bị khóa.</summary>
    Task<NguoiDungDto?> DangNhapAsync(string tenDangNhap, string matKhau);
    Task<HoSoKhachHangDto?> LayHoSoAsync(int maTaiKhoan);
    Task CapNhatHoSoAsync(int maTaiKhoan, HoSoKhachHangDto dto);
    Task DoiMatKhauAsync(int maTaiKhoan, string matKhauCu, string matKhauMoi);
    /// <summary>Tài khoản còn tồn tại, chưa bị khóa và đúng vai trò – dùng để kiểm tra lại phiên đăng nhập Web.</summary>
    Task<bool> ConHieuLucAsync(int maTaiKhoan, VaiTro vaiTro);
}

/// <summary>
/// Đăng ký, đăng nhập (Lab04), hồ sơ và đổi mật khẩu. Mật khẩu được băm bằng BCrypt, không lưu dạng rõ.
/// Quy ước dữ liệu (báo cáo mục 3.7): thông tin liên hệ của khách hàng chỉ đọc/ghi ở KHACHHANG;
/// Email, SoDienThoai của TAIKHOAN chỉ dùng cho tài khoản nhân viên. TAIKHOAN.HoTen là tên hiển thị.
/// </summary>
public partial class TaiKhoanService : ITaiKhoanService
{
    private readonly IAppDbContext _db;
    public TaiKhoanService(IAppDbContext db) => _db = db;

    private static string? Gon(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    public async Task<NguoiDungDto> DangKyAsync(DangKyDto dto)
    {
        var ten = dto.TenDangNhap.Trim();
        if (await _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == ten))
            throw new BusinessException("Tên đăng nhập đã được sử dụng.");
        var email = Gon(dto.Email);
        if (email != null && await _db.KhachHangs.AnyAsync(k => k.Email == email && k.MaTaiKhoan != null))
            throw new BusinessException("Email đã được dùng cho tài khoản khác.");

        // Tạo đồng thời TAIKHOAN và KHACHHANG; EF lưu cả hai trong cùng một lệnh SaveChanges (một giao tác).
        var tk = new TaiKhoan
        {
            TenDangNhap = ten,
            MatKhauHash = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
            HoTen = dto.HoTen.Trim(),
            VaiTro = VaiTro.KhachHang,
            KhachHang = new KhachHang
            {
                HoTen = dto.HoTen.Trim(), Email = email, SoDienThoai = dto.SoDienThoai.Trim(), DiaChi = Gon(dto.DiaChi)
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
        return new NguoiDungDto(tk.MaTaiKhoan, tk.TenDangNhap, tk.KhachHang?.HoTen ?? tk.HoTen, tk.VaiTro,
            tk.KhachHang?.MaKhachHang);
    }

    public Task<HoSoKhachHangDto?> LayHoSoAsync(int maTaiKhoan) =>
        _db.KhachHangs.AsNoTracking().Where(k => k.MaTaiKhoan == maTaiKhoan)
            .Select(k => new HoSoKhachHangDto
            {
                TenDangNhap = k.TaiKhoan!.TenDangNhap, HoTen = k.HoTen, Email = k.Email,
                SoDienThoai = k.SoDienThoai, DiaChi = k.DiaChi
            })
            .FirstOrDefaultAsync();

    public async Task CapNhatHoSoAsync(int maTaiKhoan, HoSoKhachHangDto dto)
    {
        var kh = await _db.KhachHangs.Include(k => k.TaiKhoan).FirstOrDefaultAsync(k => k.MaTaiKhoan == maTaiKhoan)
                 ?? throw new BusinessException("Không tìm thấy hồ sơ khách hàng.");
        var email = Gon(dto.Email);
        if (email != null && await _db.KhachHangs.AnyAsync(k => k.Email == email && k.MaTaiKhoan != null && k.MaKhachHang != kh.MaKhachHang))
            throw new BusinessException("Email đã được dùng cho tài khoản khác.");

        kh.HoTen = dto.HoTen.Trim();
        kh.Email = email;
        kh.SoDienThoai = dto.SoDienThoai.Trim();
        kh.DiaChi = Gon(dto.DiaChi);
        kh.TaiKhoan!.HoTen = kh.HoTen; // giữ tên hiển thị đồng bộ
        // Đơn hàng cũ KHÔNG đổi: người nhận/địa chỉ trên DONHANG là dữ liệu "chốt" tại thời điểm đặt.
        await _db.SaveChangesAsync();
    }

    public Task<bool> ConHieuLucAsync(int maTaiKhoan, VaiTro vaiTro) =>
        _db.TaiKhoans.AnyAsync(t => t.MaTaiKhoan == maTaiKhoan && t.TrangThai && t.VaiTro == vaiTro);

    public async Task DoiMatKhauAsync(int maTaiKhoan, string matKhauCu, string matKhauMoi)
    {
        var tk = await _db.TaiKhoans.FirstOrDefaultAsync(t => t.MaTaiKhoan == maTaiKhoan)
                 ?? throw new BusinessException("Tài khoản không tồn tại.");
        if (!BCrypt.Net.BCrypt.Verify(matKhauCu, tk.MatKhauHash))
            throw new BusinessException("Mật khẩu hiện tại không đúng.");
        if (matKhauCu == matKhauMoi)
            throw new BusinessException("Mật khẩu mới phải khác mật khẩu hiện tại.");
        tk.MatKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
        await _db.SaveChangesAsync();
    }
}
