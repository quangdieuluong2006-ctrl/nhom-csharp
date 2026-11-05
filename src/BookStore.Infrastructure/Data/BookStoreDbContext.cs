using BookStore.Application.Common;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookStore.Infrastructure.Data;

/// <summary>
/// Ánh xạ thực thể ↔ bảng của CSDL BookStoreDB (Code First, Fluent API).
/// Tên bảng, cột, ràng buộc giữ đúng theo từ điển dữ liệu trong báo cáo (mục 3.7).
/// </summary>
public class BookStoreDbContext : DbContext, IAppDbContext
{
    public BookStoreDbContext(DbContextOptions<BookStoreDbContext> options) : base(options) { }

    public DbSet<TaiKhoan> TaiKhoans => Set<TaiKhoan>();
    public DbSet<KhachHang> KhachHangs => Set<KhachHang>();
    public DbSet<DanhMuc> DanhMucs => Set<DanhMuc>();
    public DbSet<NhaXuatBan> NhaXuatBans => Set<NhaXuatBan>();
    public DbSet<TacGia> TacGias => Set<TacGia>();
    public DbSet<Sach> Sachs => Set<Sach>();
    public DbSet<SachTacGia> SachTacGias => Set<SachTacGia>();
    public DbSet<DonHang> DonHangs => Set<DonHang>();
    public DbSet<ChiTietDonHang> ChiTietDonHangs => Set<ChiTietDonHang>();
    public DbSet<PhieuKho> PhieuKhos => Set<PhieuKho>();
    public DbSet<ChiTietPhieuKho> ChiTietPhieuKhos => Set<ChiTietPhieuKho>();

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default) =>
        Database.BeginTransactionAsync(ct);

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Ghi chú: các cột bool TrangThai không khai báo HasDefaultValue vì EF sẽ bỏ qua giá trị false
        // (trùng giá trị mặc định CLR) và để CSDL tự gán true. Giá trị mặc định đặt ở thực thể (= true).
        mb.Entity<TaiKhoan>(e =>
        {
            e.ToTable("TAIKHOAN", t => t.HasCheckConstraint("CK_TAIKHOAN_VaiTro",
                "[VaiTro] IN ('QuanTriVien','NhanVienBanHang','NhanVienKho','KhachHang')"));
            e.HasKey(x => x.MaTaiKhoan);
            e.Property(x => x.TenDangNhap).HasMaxLength(50).IsRequired();
            e.HasIndex(x => x.TenDangNhap).IsUnique();
            e.Property(x => x.MatKhauHash).HasMaxLength(255).IsRequired();
            e.Property(x => x.HoTen).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(254);
            e.Property(x => x.SoDienThoai).HasMaxLength(15).IsUnicode(false);
            e.Property(x => x.VaiTro).HasConversion<string>().HasMaxLength(20).IsUnicode(false)
                .HasDefaultValue(VaiTro.KhachHang);
        });

        mb.Entity<KhachHang>(e =>
        {
            e.ToTable("KHACHHANG");
            e.HasKey(x => x.MaKhachHang);
            e.Property(x => x.HoTen).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(254);
            e.Property(x => x.SoDienThoai).HasMaxLength(15).IsUnicode(false).IsRequired();
            e.Property(x => x.DiaChi).HasMaxLength(255);
            // 1 tài khoản – tối đa 1 khách hàng; cho phép NULL (khách vãng lai) => chỉ mục duy nhất có lọc.
            e.HasOne(x => x.TaiKhoan).WithOne(t => t.KhachHang).HasForeignKey<KhachHang>(x => x.MaTaiKhoan)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasIndex(x => x.MaTaiKhoan).IsUnique().HasFilter("[MaTaiKhoan] IS NOT NULL");
        });

        mb.Entity<DanhMuc>(e =>
        {
            e.ToTable("DANHMUC");
            e.HasKey(x => x.MaDanhMuc);
            e.Property(x => x.TenDanhMuc).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.TenDanhMuc).IsUnique();
            e.Property(x => x.MoTa).HasMaxLength(500);
            e.HasOne(x => x.DanhMucCha).WithMany(x => x.DanhMucCons).HasForeignKey(x => x.MaDanhMucCha)
                .OnDelete(DeleteBehavior.NoAction);
        });

        mb.Entity<NhaXuatBan>(e =>
        {
            e.ToTable("NHAXUATBAN");
            e.HasKey(x => x.MaNhaXuatBan);
            e.Property(x => x.TenNhaXuatBan).HasMaxLength(150).IsRequired();
            e.HasIndex(x => x.TenNhaXuatBan).IsUnique();
            e.Property(x => x.DiaChi).HasMaxLength(255);
            e.Property(x => x.SoDienThoai).HasMaxLength(15).IsUnicode(false);
            e.Property(x => x.Email).HasMaxLength(254);
        });

        mb.Entity<TacGia>(e =>
        {
            e.ToTable("TACGIA");
            e.HasKey(x => x.MaTacGia);
            e.Property(x => x.TenTacGia).HasMaxLength(150).IsRequired();
            e.Property(x => x.TieuSu).HasMaxLength(1000);
        });

        mb.Entity<Sach>(e =>
        {
            e.ToTable("SACH", t =>
            {
                t.HasCheckConstraint("CK_SACH_DonGia", "[DonGia] >= 0");
                t.HasCheckConstraint("CK_SACH_SoLuongTon", "[SoLuongTon] >= 0");
            });
            e.HasKey(x => x.MaSach);
            e.Property(x => x.ISBN).HasMaxLength(20).IsUnicode(false);
            e.HasIndex(x => x.ISBN).IsUnique().HasFilter("[ISBN] IS NOT NULL");
            e.Property(x => x.TenSach).HasMaxLength(200).IsRequired();
            e.HasIndex(x => x.TenSach);
            e.Property(x => x.DonGia).HasColumnType("decimal(18,2)");
            e.Property(x => x.SoLuongTon).HasDefaultValue(0);
            e.Property(x => x.HinhAnh).HasMaxLength(500);
            e.Property(x => x.NgayCapNhat).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");
            e.HasOne(x => x.DanhMuc).WithMany(d => d.Sachs).HasForeignKey(x => x.MaDanhMuc)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.NhaXuatBan).WithMany(n => n.Sachs).HasForeignKey(x => x.MaNhaXuatBan)
                .OnDelete(DeleteBehavior.NoAction);
        });

        mb.Entity<SachTacGia>(e =>
        {
            e.ToTable("SACHTACGIA");
            e.HasKey(x => new { x.MaSach, x.MaTacGia });
            e.Property(x => x.VaiTroTacGia).HasMaxLength(50);
            e.HasOne(x => x.Sach).WithMany(s => s.SachTacGias).HasForeignKey(x => x.MaSach);
            e.HasOne(x => x.TacGia).WithMany(t => t.SachTacGias).HasForeignKey(x => x.MaTacGia);
        });

        mb.Entity<DonHang>(e =>
        {
            e.ToTable("DONHANG", t => t.HasCheckConstraint("CK_DONHANG_TongTien", "[TongTien] >= 0"));
            e.HasKey(x => x.MaDonHang);
            e.Property(x => x.NgayDat).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");
            e.Property(x => x.NgayGiaoDuKien).HasColumnType("date");
            e.Property(x => x.HoTenNhan).HasMaxLength(100).IsRequired();
            e.Property(x => x.SoDienThoaiNhan).HasMaxLength(15).IsUnicode(false).IsRequired();
            e.Property(x => x.DiaChiGiaoHang).HasMaxLength(255).IsRequired();
            e.Property(x => x.GhiChu).HasMaxLength(500);
            e.Property(x => x.TongTien).HasColumnType("decimal(18,2)");
            e.Property(x => x.TrangThai).HasColumnName("TrangThaiDonHang").HasConversion<string>()
                .HasMaxLength(20).IsUnicode(false).HasDefaultValue(TrangThaiDonHang.ChoXacNhan);
            e.Property(x => x.PhuongThucThanhToan).HasMaxLength(20).IsUnicode(false).HasDefaultValue("COD");
            e.Property(x => x.TrangThaiThanhToan).HasConversion<string>().HasMaxLength(20).IsUnicode(false)
                .HasDefaultValue(TrangThaiThanhToan.ChuaThanhToan);
            e.HasOne(x => x.KhachHang).WithMany(k => k.DonHangs).HasForeignKey(x => x.MaKhachHang)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasIndex(x => new { x.MaKhachHang, x.NgayDat });
        });

        mb.Entity<ChiTietDonHang>(e =>
        {
            e.ToTable("CHITIETDONHANG", t =>
            {
                t.HasCheckConstraint("CK_CTDH_SoLuong", "[SoLuong] > 0");
                t.HasCheckConstraint("CK_CTDH_DonGia", "[DonGiaDat] >= 0");
            });
            e.HasKey(x => new { x.MaDonHang, x.MaSach });
            e.Property(x => x.DonGiaDat).HasColumnType("decimal(18,2)");
            e.Property(x => x.ThanhTien).HasColumnType("decimal(29,2)")
                .HasComputedColumnSql("[SoLuong] * [DonGiaDat]", stored: true);
            e.HasOne(x => x.DonHang).WithMany(d => d.ChiTiets).HasForeignKey(x => x.MaDonHang);
            e.HasOne(x => x.Sach).WithMany().HasForeignKey(x => x.MaSach).OnDelete(DeleteBehavior.NoAction);
        });

        mb.Entity<PhieuKho>(e =>
        {
            e.ToTable("PHIEUKHO");
            e.HasKey(x => x.MaPhieu);
            e.Property(x => x.LoaiPhieu).HasConversion<string>().HasMaxLength(15).IsUnicode(false);
            e.Property(x => x.NgayLap).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");
            e.Property(x => x.GhiChu).HasMaxLength(500);
            e.HasOne(x => x.TaiKhoan).WithMany().HasForeignKey(x => x.MaTaiKhoan).OnDelete(DeleteBehavior.NoAction);
        });

        mb.Entity<ChiTietPhieuKho>(e =>
        {
            e.ToTable("CHITIETPHIEUKHO", t =>
            {
                t.HasCheckConstraint("CK_CTPK_SoLuong", "[SoLuong] > 0");
                t.HasCheckConstraint("CK_CTPK_DonGia", "[DonGiaNhap] >= 0");
            });
            e.HasKey(x => new { x.MaPhieu, x.MaSach });
            e.Property(x => x.DonGiaNhap).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.PhieuKho).WithMany(p => p.ChiTiets).HasForeignKey(x => x.MaPhieu);
            e.HasOne(x => x.Sach).WithMany().HasForeignKey(x => x.MaSach).OnDelete(DeleteBehavior.NoAction);
        });
    }
}
