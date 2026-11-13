using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests;

/// <summary>Màn hình Khách hàng trên Desktop (thêm, xóa, khóa) và mục "Sách bán chạy" trên Web.</summary>
[Collection("CSDL")]
public class KhachHangVaBanChayTests
{
    private readonly CsdlKiemThu _csdl;
    public KhachHangVaBanChayTests(CsdlKiemThu csdl) => _csdl = csdl;

    private static string SoDienThoaiNgauNhien() => "09" + Random.Shared.Next(10_000_000, 99_999_999);

    private static ThongTinNhanHangDto NguoiNhan() => new()
    {
        HoTenNhan = "Người nhận kiểm thử", SoDienThoaiNhan = "0909000111", DiaChiGiaoHang = "1 Lê Lợi, Quận 1"
    };

    private async Task<long> DatHangAsync(int maKhachHang, int maSach, int soLuong)
    {
        await using var db = _csdl.TaoDb();
        return await new DonHangService(db).DatHangAsync(maKhachHang, NguoiNhan(), new[] { new DongDatHang(maSach, soLuong) });
    }

    // ------------------------------- Sách bán chạy (Web) -------------------------------

    [Fact]
    public async Task BanChay_XepTheoSoDaBan_KhongTinhDonDaHuy()
    {
        int khach = await _csdl.MaKhachHangAsync("khachhang");
        int sachA = await _csdl.TaoSachAsync(ton: 50), sachB = await _csdl.TaoSachAsync(ton: 50);
        await DatHangAsync(khach, sachA, 5);
        await DatHangAsync(khach, sachB, 2);
        long donHuy = await DatHangAsync(khach, sachB, 9);
        await using (var db = _csdl.TaoDb()) await new DonHangService(db).HuyDonAsync(donHuy, khach);

        await using var db2 = _csdl.TaoDb();
        var kq = await new SachService(db2).TimKiemAsync(new LocSach
        {
            TuKhoa = "Sách kiểm thử", SapXep = SapXepSach.BanChay, KichThuocTrang = 1000
        });
        var a = kq.Items.Single(s => s.MaSach == sachA);
        var b = kq.Items.Single(s => s.MaSach == sachB);
        Assert.Equal(5, a.DaBan);
        Assert.Equal(2, b.DaBan);                       // 9 cuốn của đơn đã hủy không được tính
        Assert.True(kq.Items.ToList().IndexOf(a) < kq.Items.ToList().IndexOf(b));

        var banChay = await new SachService(db2).LaySachBanChayAsync(100);
        Assert.Contains(banChay, s => s.MaSach == sachA);
        Assert.All(banChay, s => Assert.True(s.DaBan > 0)); // trang chủ chỉ hiện sách đã bán được
    }

    // ------------------------------- Khách hàng (Desktop) -------------------------------

    [Fact]
    public async Task ThemKhachVangLai_TrungSoDienThoai_BaoLoi_XoaKhachChuaCoDon()
    {
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        var sdt = SoDienThoaiNgauNhien();
        await using var db = _csdl.TaoDb();
        var svc = new KhachHangService(db);

        int ma = await svc.ThemAsync(nv, new KhachHangLuuDto { HoTen = "Khách tại quầy", SoDienThoai = sdt });
        var kh = (await svc.TimKiemAsync(nv, sdt)).Single();
        Assert.Equal(ma, kh.MaKhachHang);
        Assert.Null(kh.TaiKhoanHoatDong);               // khách vãng lai: không có tài khoản Web

        await Assert.ThrowsAsync<BusinessException>(async () =>
            await svc.ThemAsync(nv, new KhachHangLuuDto { HoTen = "Trùng số", SoDienThoai = sdt }));

        Assert.True(await svc.XoaAsync(nv, ma));        // chưa có đơn → xóa hẳn
        Assert.False(await db.KhachHangs.AnyAsync(c => c.MaKhachHang == ma));
    }

    [Fact]
    public async Task XoaKhachDaCoDon_CoTaiKhoan_ChiKhoaTaiKhoan_MoKhoaDangNhapLaiDuoc()
    {
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        var ten = "kh" + Guid.NewGuid().ToString("N")[..8];
        await using (var db = _csdl.TaoDb())
            await new TaiKhoanService(db).DangKyAsync(new DangKyDto
            {
                HoTen = "Khách có đơn", TenDangNhap = ten, MatKhau = "123456", SoDienThoai = SoDienThoaiNgauNhien()
            });
        int maKh = await _csdl.MaKhachHangAsync(ten);
        await DatHangAsync(maKh, await _csdl.TaoSachAsync(ton: 5), 1);

        await using var db2 = _csdl.TaoDb();
        var svc = new KhachHangService(db2);
        var tk = new TaiKhoanService(db2);
        Assert.False(await svc.XoaAsync(nv, maKh));     // không xóa để giữ lịch sử đơn
        Assert.True(await db2.KhachHangs.AnyAsync(c => c.MaKhachHang == maKh));
        Assert.Null(await tk.DangNhapAsync(ten, "123456")); // tài khoản đã bị khóa

        await svc.DatTrangThaiTaiKhoanAsync(nv, maKh, hoatDong: true);
        Assert.NotNull(await tk.DangNhapAsync(ten, "123456"));
    }

    [Fact]
    public async Task XoaKhachVangLaiDaCoDon_BaoLoi_KhoaKhachVangLai_BaoLoi()
    {
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        await using var db = _csdl.TaoDb();
        var svc = new KhachHangService(db);
        int ma = await svc.ThemAsync(nv, new KhachHangLuuDto { HoTen = "Khách gọi điện", SoDienThoai = SoDienThoaiNgauNhien() });
        await DatHangAsync(ma, await _csdl.TaoSachAsync(ton: 5), 1);

        await Assert.ThrowsAsync<BusinessException>(async () => await svc.XoaAsync(nv, ma));
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.DatTrangThaiTaiKhoanAsync(nv, ma, false));
    }

    [Fact] // BR08: NV kho không được quản lý khách hàng
    public async Task NhanVienKho_KhongThemDuocKhachHang()
    {
        int nvKho = await _csdl.MaTaiKhoanAsync("nvkho");
        await using var db = _csdl.TaoDb();
        await Assert.ThrowsAsync<BusinessException>(async () => await new KhachHangService(db)
            .ThemAsync(nvKho, new KhachHangLuuDto { HoTen = "Không được", SoDienThoai = SoDienThoaiNgauNhien() }));
    }
}
