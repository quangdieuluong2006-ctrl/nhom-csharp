using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests;

[Collection("CSDL")]
public class QuanTriTests
{
    private readonly CsdlKiemThu _csdl;
    public QuanTriTests(CsdlKiemThu csdl) => _csdl = csdl;

    // ------------------------------- Tài khoản -------------------------------

    [Fact] // UT01, UT02
    public async Task DangNhap_DungMatKhau_TraVaiTro_SaiMatKhauTraNull()
    {
        await using var db = _csdl.TaoDb();
        var svc = new TaiKhoanService(db);
        Assert.Equal(VaiTro.QuanTriVien, (await svc.DangNhapAsync("admin", "123456"))!.VaiTro);
        Assert.Null(await svc.DangNhapAsync("admin", "sai-mat-khau"));
        Assert.Null(await svc.DangNhapAsync("khong-ton-tai", "123456"));
    }

    [Fact] // UT03 + BR11
    public async Task DangKy_TrungTenDangNhap_BaoLoi_MatKhauDuocBam()
    {
        await using var db = _csdl.TaoDb();
        var svc = new TaiKhoanService(db);
        var dto = new DangKyDto { HoTen = "Khách Mới", TenDangNhap = "khachmoi01", MatKhau = "matkhau1", SoDienThoai = "0911222333" };
        await svc.DangKyAsync(dto);
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.DangKyAsync(dto));
        var hash = await db.TaiKhoans.Where(t => t.TenDangNhap == "khachmoi01").Select(t => t.MatKhauHash).SingleAsync();
        Assert.NotEqual("matkhau1", hash);
        Assert.True(BCrypt.Net.BCrypt.Verify("matkhau1", hash));
    }

    [Fact]
    public async Task QuanTri_TaoNhanVien_KhoaTaiKhoan_KhongDangNhapDuoc()
    {
        int admin = await _csdl.MaTaiKhoanAsync("admin");
        await using var db = _csdl.TaoDb();
        var svc = new TaiKhoanService(db);
        int ma = await svc.TaoTaiKhoanNhanVienAsync(admin, new TaiKhoanLuuDto
        {
            TenDangNhap = "banhang02", HoTen = "Nhân viên mới", MatKhau = "123456", VaiTro = VaiTro.NhanVienBanHang
        });
        Assert.NotNull(await svc.DangNhapAsync("banhang02", "123456"));

        await svc.DatTrangThaiAsync(admin, ma, hoatDong: false);
        Assert.Null(await svc.DangNhapAsync("banhang02", "123456")); // tài khoản khóa không đăng nhập được
    }

    [Fact] // ST03
    public async Task NhanVien_KhongQuanLyDuocTaiKhoan()
    {
        await using var db = _csdl.TaoDb();
        await Assert.ThrowsAsync<BusinessException>(async () => await new TaiKhoanService(db)
            .LayDanhSachAsync(await _csdl.MaTaiKhoanAsync("nvbanhang"), null, null));
    }

    [Fact]
    public async Task KhongTheTuKhoa_VaPhaiConQuanTriVien()
    {
        int admin = await _csdl.MaTaiKhoanAsync("admin");
        await using var db = _csdl.TaoDb();
        var svc = new TaiKhoanService(db);
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.DatTrangThaiAsync(admin, admin, false));
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.CapNhatTaiKhoanAsync(admin, new TaiKhoanLuuDto
        {
            MaTaiKhoan = admin, HoTen = "Quản trị viên", VaiTro = VaiTro.NhanVienKho // hạ quyền admin cuối cùng
        }));
    }

    // --------------------------------- Sách ---------------------------------

    private async Task<SachLuuDto> SachHopLeAsync()
    {
        await using var db = _csdl.TaoDb();
        return new SachLuuDto
        {
            TenSach = "Sách mới " + Guid.NewGuid().ToString("N")[..6], DonGia = 99_000m, NamXuatBan = 2024,
            MaDanhMuc = await db.DanhMucs.Select(d => d.MaDanhMuc).FirstAsync(),
            MaTacGias = { await db.TacGias.Select(t => t.MaTacGia).FirstAsync() }
        };
    }

    [Fact] // ST04 / US06
    public async Task ThemSach_HopLeLuuDuoc_TonBangKhong_IsbnTrungVaGiaAmBiChan()
    {
        int admin = await _csdl.MaTaiKhoanAsync("admin");
        await using var db = _csdl.TaoDb();
        var svc = new SachService(db);

        var dto = await SachHopLeAsync();
        dto.ISBN = "978-604-000-0001";
        int ma = await svc.LuuAsync(admin, dto);
        var luu = await svc.LayDeSuaAsync(ma);
        Assert.Equal("9786040000001", luu!.ISBN);
        Assert.Equal(0, luu.SoLuongTon);

        var trung = await SachHopLeAsync(); trung.ISBN = "9786040000001";
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.LuuAsync(admin, trung));
        var giaAm = await SachHopLeAsync(); giaAm.DonGia = -1;
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.LuuAsync(admin, giaAm));
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.LuuAsync(await _csdl.MaTaiKhoanAsync("nvkho"), await SachHopLeAsync()));
    }

    [Fact] // BR09
    public async Task XoaSach_ChuaGiaoDichXoaHan_DaGiaoDichChiNgungBan()
    {
        int admin = await _csdl.MaTaiKhoanAsync("admin");
        await using var db = _csdl.TaoDb();
        var svc = new SachService(db);

        int moi = await svc.LuuAsync(admin, await SachHopLeAsync());
        Assert.True(await svc.XoaHoacNgungBanAsync(admin, moi));
        Assert.False(await db.Sachs.AnyAsync(s => s.MaSach == moi));

        int daBan = await db.ChiTietPhieuKhos.Select(c => c.MaSach).FirstAsync(); // có trong phiếu nhập đầu kỳ
        Assert.False(await svc.XoaHoacNgungBanAsync(admin, daBan));
        Assert.False((await db.Sachs.AsNoTracking().SingleAsync(s => s.MaSach == daBan)).TrangThai);
    }

    // -------------------------------- Danh mục --------------------------------

    [Fact]
    public async Task DanhMuc_TenTrung_BiChan_DangCoSachThiChiNgungDung()
    {
        int admin = await _csdl.MaTaiKhoanAsync("admin");
        await using var db = _csdl.TaoDb();
        var svc = new DanhMucService(db);
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.LuuDanhMucAsync(admin, new DanhMucLuuDto { TenDanhMuc = "Kinh tế" }));

        int coSach = await db.Sachs.Select(s => s.MaDanhMuc).FirstAsync();
        Assert.False(await svc.XoaDanhMucAsync(admin, coSach));

        int trong = await svc.LuuDanhMucAsync(admin, new DanhMucLuuDto { TenDanhMuc = "Danh mục trống " + Guid.NewGuid().ToString("N")[..4] });
        Assert.True(await svc.XoaDanhMucAsync(admin, trong));
    }

    [Fact]
    public async Task KhachHang_CapNhat_SdtSaiBiChan()
    {
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        int kh = await _csdl.MaKhachHangAsync("khachhang2");
        await using var db = _csdl.TaoDb();
        var svc = new KhachHangService(db);
        var dto = (await svc.LayAsync(kh))!;
        dto.SoDienThoai = "12345";
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.CapNhatAsync(nv, dto));
        dto.SoDienThoai = "0988777666";
        await svc.CapNhatAsync(nv, dto);
        Assert.Contains(await svc.TimKiemAsync("0988777666"), x => x.MaKhachHang == kh);
    }
}
