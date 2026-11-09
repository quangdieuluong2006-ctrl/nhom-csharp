using BookStore.Domain.Entities;
using BookStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests;

/// <summary>
/// CSDL dùng cho kiểm thử tích hợp (integration test): tạo mới từ migration + dữ liệu mẫu trước khi chạy,
/// xóa sau khi chạy xong. Mặc định dùng LocalDB; có thể đổi bằng biến môi trường BOOKSTORE_TEST_DB.
/// Kiểm thử chạy trên SQL Server thật nên kiểm tra được cả giao tác, UPDATE có điều kiện, ràng buộc CHECK.
/// </summary>
public sealed class CsdlKiemThu : IAsyncLifetime
{
    private readonly string _chuoiKetNoi = Environment.GetEnvironmentVariable("BOOKSTORE_TEST_DB")
        ?? @"Server=(localdb)\MSSQLLocalDB;Database=BookStoreDB_KiemThu;Trusted_Connection=True;TrustServerCertificate=True";

    public BookStoreDbContext TaoDb() => new(new DbContextOptionsBuilder<BookStoreDbContext>()
        .UseSqlServer(_chuoiKetNoi, o => o.EnableRetryOnFailure()).Options);

    public async Task InitializeAsync()
    {
        await using var db = TaoDb();
        await db.Database.EnsureDeletedAsync();
        await db.Database.MigrateAsync();
        await DbSeeder.SeedAsync(db);
    }

    public async Task DisposeAsync()
    {
        await using var db = TaoDb();
        await db.Database.EnsureDeletedAsync();
    }

    // ----------------------------- Hàm hỗ trợ -----------------------------

    public async Task<int> MaTaiKhoanAsync(string tenDangNhap)
    {
        await using var db = TaoDb();
        return await db.TaiKhoans.Where(t => t.TenDangNhap == tenDangNhap).Select(t => t.MaTaiKhoan).SingleAsync();
    }

    public async Task<int> MaKhachHangAsync(string tenDangNhap)
    {
        await using var db = TaoDb();
        return await db.KhachHangs.Where(k => k.TaiKhoan!.TenDangNhap == tenDangNhap).Select(k => k.MaKhachHang).SingleAsync();
    }

    /// <summary>Tạo một cuốn sách riêng cho từng ca kiểm thử để các ca không ảnh hưởng lẫn nhau.</summary>
    public async Task<int> TaoSachAsync(int ton, decimal gia = 50_000m)
    {
        await using var db = TaoDb();
        var s = new Sach
        {
            TenSach = "Sách kiểm thử " + Guid.NewGuid().ToString("N")[..8], DonGia = gia, SoLuongTon = ton,
            MaDanhMuc = await db.DanhMucs.Select(d => d.MaDanhMuc).FirstAsync()
        };
        s.SachTacGias.Add(new SachTacGia { MaTacGia = await db.TacGias.Select(t => t.MaTacGia).FirstAsync() });
        db.Sachs.Add(s);
        await db.SaveChangesAsync();
        return s.MaSach;
    }

    public async Task<int> TonKhoAsync(int maSach)
    {
        await using var db = TaoDb();
        return await db.Sachs.Where(s => s.MaSach == maSach).Select(s => s.SoLuongTon).SingleAsync();
    }
}

/// <summary>Mọi lớp kiểm thử dùng chung một CSDL và chạy tuần tự.</summary>
[CollectionDefinition("CSDL")]
public class NhomCsdl : ICollectionFixture<CsdlKiemThu> { }
