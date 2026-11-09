using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Common;

/// <summary>
/// Kiểm tra quyền ở tầng nghiệp vụ (BR08, ca kiểm thử ST03): Desktop đã ẩn menu theo vai trò,
/// nhưng service vẫn kiểm tra lại để "gọi trực tiếp" chức năng ngoài quyền cũng bị từ chối.
/// Quyền lấy từ CSDL (không tin vai trò do giao diện gửi lên), tài khoản bị khóa cũng bị từ chối.
/// </summary>
public static class QuyenHan
{
    public static async Task KiemTraAsync(this IAppDbContext db, int maTaiKhoan, params VaiTro[] vaiTroDuocPhep)
    {
        var tk = await db.TaiKhoans.AsNoTracking()
            .Where(t => t.MaTaiKhoan == maTaiKhoan)
            .Select(t => new { t.VaiTro, t.TrangThai })
            .FirstOrDefaultAsync();
        if (tk is null || !tk.TrangThai || !vaiTroDuocPhep.Contains(tk.VaiTro))
            throw new BusinessException("Bạn không có quyền thực hiện chức năng này.");
    }

    // Nhóm vai trò theo ma trận chức năng – tác nhân (Bảng 3.13)
    public static readonly VaiTro[] QuanTri = { VaiTro.QuanTriVien };
    public static readonly VaiTro[] BanHang = { VaiTro.QuanTriVien, VaiTro.NhanVienBanHang };
    public static readonly VaiTro[] Kho = { VaiTro.QuanTriVien, VaiTro.NhanVienKho };
    public static readonly VaiTro[] NhanVien = { VaiTro.QuanTriVien, VaiTro.NhanVienBanHang, VaiTro.NhanVienKho };
}
