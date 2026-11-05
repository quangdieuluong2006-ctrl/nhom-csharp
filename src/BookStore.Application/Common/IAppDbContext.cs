using BookStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
}
