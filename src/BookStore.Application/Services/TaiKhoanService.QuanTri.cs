using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Services;

/// <summary>Quản lý tài khoản, phân quyền (D08 – chỉ Quản trị viên, BR11).</summary>
public partial interface ITaiKhoanService
{
    Task<List<TaiKhoanQtDto>> LayDanhSachAsync(int maNguoiThucHien, string? tuKhoa, VaiTro? vaiTro);
    /// <summary>Tạo tài khoản nhân viên (quản trị viên, bán hàng, kho). Trả về mã tài khoản.</summary>
    Task<int> TaoTaiKhoanNhanVienAsync(int maNguoiThucHien, TaiKhoanLuuDto dto);
    Task CapNhatTaiKhoanAsync(int maNguoiThucHien, TaiKhoanLuuDto dto);
    /// <summary>Khóa / mở khóa tài khoản. Tài khoản bị khóa không đăng nhập được ở cả Web và Desktop.</summary>
    Task DatTrangThaiAsync(int maNguoiThucHien, int maTaiKhoan, bool hoatDong);
    Task DatLaiMatKhauAsync(int maNguoiThucHien, int maTaiKhoan, string matKhauMoi);
}

public partial class TaiKhoanService
{
    public async Task<List<TaiKhoanQtDto>> LayDanhSachAsync(int maNguoiThucHien, string? tuKhoa, VaiTro? vaiTro)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var q = _db.TaiKhoans.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(tuKhoa))
        {
            var k = tuKhoa.Trim();
            q = q.Where(t => t.TenDangNhap.Contains(k) || t.HoTen.Contains(k));
        }
        if (vaiTro is VaiTro vt) q = q.Where(t => t.VaiTro == vt);

        // Liên hệ của khách hàng nằm ở KHACHHANG, của nhân viên nằm ở TAIKHOAN (quy ước mục 3.7)
        return await q.OrderBy(t => t.VaiTro).ThenBy(t => t.TenDangNhap)
            .Select(t => new TaiKhoanQtDto
            {
                MaTaiKhoan = t.MaTaiKhoan, TenDangNhap = t.TenDangNhap, HoTen = t.HoTen, VaiTro = t.VaiTro,
                Email = t.KhachHang != null ? t.KhachHang.Email : t.Email,
                SoDienThoai = t.KhachHang != null ? t.KhachHang.SoDienThoai : t.SoDienThoai,
                TrangThai = t.TrangThai
            })
            .ToListAsync();
    }

    public async Task<int> TaoTaiKhoanNhanVienAsync(int maNguoiThucHien, TaiKhoanLuuDto dto)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        if (dto.VaiTro == VaiTro.KhachHang)
            throw new BusinessException("Tài khoản khách hàng do khách tự đăng ký trên website.");
        var ten = KiemTra.BatBuoc(dto.TenDangNhap, "Tên đăng nhập", 50);
        if (ten.Length < 4 || ten.Any(char.IsWhiteSpace))
            throw new BusinessException("Tên đăng nhập tối thiểu 4 ký tự, không chứa khoảng trắng.");
        if (await _db.TaiKhoans.AnyAsync(t => t.TenDangNhap == ten))
            throw new BusinessException("Tên đăng nhập đã được sử dụng.");
        if (string.IsNullOrEmpty(dto.MatKhau) || dto.MatKhau.Length < 6)
            throw new BusinessException("Mật khẩu phải có ít nhất 6 ký tự.");

        var tk = new TaiKhoan
        {
            TenDangNhap = ten,
            MatKhauHash = BCrypt.Net.BCrypt.HashPassword(dto.MatKhau),
            HoTen = KiemTra.BatBuoc(dto.HoTen, "Họ tên", 100),
            Email = KiemTra.Email(dto.Email),
            SoDienThoai = KiemTra.SoDienThoaiTuyChon(dto.SoDienThoai),
            VaiTro = dto.VaiTro
        };
        _db.TaiKhoans.Add(tk);
        await _db.SaveChangesAsync();
        return tk.MaTaiKhoan;
    }

    public async Task CapNhatTaiKhoanAsync(int maNguoiThucHien, TaiKhoanLuuDto dto)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        var tk = await _db.TaiKhoans.Include(t => t.KhachHang).FirstOrDefaultAsync(t => t.MaTaiKhoan == dto.MaTaiKhoan)
                 ?? throw new BusinessException("Tài khoản không tồn tại.");

        // Thông tin liên hệ của khách sửa ở màn hình Khách hàng; ở đây chỉ khóa/mở khóa, đặt lại mật khẩu.
        if (tk.VaiTro == VaiTro.KhachHang)
            throw new BusinessException("Tài khoản khách hàng chỉ được khóa/mở khóa hoặc đặt lại mật khẩu. " +
                                        "Thông tin liên hệ cập nhật ở màn hình Khách hàng.");
        if (dto.VaiTro == VaiTro.KhachHang)
            throw new BusinessException("Không thể chuyển tài khoản nhân viên thành khách hàng.");
        if (tk.VaiTro == VaiTro.QuanTriVien && dto.VaiTro != VaiTro.QuanTriVien)
            await DamBaoConQuanTriVienKhacAsync(tk.MaTaiKhoan);

        tk.HoTen = KiemTra.BatBuoc(dto.HoTen, "Họ tên", 100);
        tk.Email = KiemTra.Email(dto.Email);
        tk.SoDienThoai = KiemTra.SoDienThoaiTuyChon(dto.SoDienThoai);
        tk.VaiTro = dto.VaiTro;
        await _db.SaveChangesAsync();
    }

    public async Task DatTrangThaiAsync(int maNguoiThucHien, int maTaiKhoan, bool hoatDong)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        if (!hoatDong && maTaiKhoan == maNguoiThucHien)
            throw new BusinessException("Không thể tự khóa tài khoản đang đăng nhập.");
        var tk = await _db.TaiKhoans.FirstOrDefaultAsync(t => t.MaTaiKhoan == maTaiKhoan)
                 ?? throw new BusinessException("Tài khoản không tồn tại.");
        if (!hoatDong && tk.VaiTro == VaiTro.QuanTriVien)
            await DamBaoConQuanTriVienKhacAsync(tk.MaTaiKhoan);
        tk.TrangThai = hoatDong;
        await _db.SaveChangesAsync();
    }

    public async Task DatLaiMatKhauAsync(int maNguoiThucHien, int maTaiKhoan, string matKhauMoi)
    {
        await _db.KiemTraAsync(maNguoiThucHien, QuyenHan.QuanTri);
        if (string.IsNullOrEmpty(matKhauMoi) || matKhauMoi.Length < 6)
            throw new BusinessException("Mật khẩu mới phải có ít nhất 6 ký tự.");
        var tk = await _db.TaiKhoans.FirstOrDefaultAsync(t => t.MaTaiKhoan == maTaiKhoan)
                 ?? throw new BusinessException("Tài khoản không tồn tại.");
        tk.MatKhauHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);
        await _db.SaveChangesAsync();
    }

    /// <summary>Hệ thống luôn phải còn ít nhất một quản trị viên đang hoạt động.</summary>
    private async Task DamBaoConQuanTriVienKhacAsync(int maTaiKhoanDangSua)
    {
        bool conNguoiKhac = await _db.TaiKhoans.AnyAsync(t =>
            t.MaTaiKhoan != maTaiKhoanDangSua && t.VaiTro == VaiTro.QuanTriVien && t.TrangThai);
        if (!conNguoiKhac)
            throw new BusinessException("Hệ thống phải còn ít nhất một quản trị viên đang hoạt động.");
    }
}
