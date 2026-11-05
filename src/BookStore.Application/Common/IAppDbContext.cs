using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Common;

/// <summary>
/// Hợp đồng truy cập dữ liệu mà tầng nghiệp vụ cần. Lớp BookStoreDbContext (Infrastructure) hiện thực interface này,
/// đóng vai trò Unit of Work; mỗi DbSet đóng vai trò một repository.
/// Nhờ vậy service không phụ thuộc trực tiếp vào SQL Server và có thể kiểm thử bằng CSDL giả.
/// </summary>
public interface IAppDbContext
{
    DbSet<TaiKhoan> TaiKhoans { get; }
    DbSet<KhachHang> KhachHangs { get; }
    DbSet<DanhMuc> DanhMucs { get; }
    DbSet<NhaXuatBan> NhaXuatBans { get; }
    DbSet<TacGia> TacGias { get; }
    DbSet<Sach> Sachs { get; }
    DbSet<SachTacGia> SachTacGias { get; }
    DbSet<DonHang> DonHangs { get; }
    DbSet<ChiTietDonHang> ChiTietDonHangs { get; }
    DbSet<PhieuKho> PhieuKhos { get; }
    DbSet<ChiTietPhieuKho> ChiTietPhieuKhos { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    /// <summary>
    /// Chạy một nhóm thao tác trong MỘT giao tác (transaction): tất cả thành công thì commit,
    /// có lỗi thì rollback toàn bộ. Dùng cho đặt hàng, hủy đơn, lập phiếu kho.
    /// </summary>
    Task<T> ThucThiTrongGiaoTacAsync<T>(Func<CancellationToken, Task<T>> thaoTac, CancellationToken ct = default);
}
