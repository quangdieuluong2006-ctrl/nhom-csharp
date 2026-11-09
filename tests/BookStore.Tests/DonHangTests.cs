using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests;

[Collection("CSDL")]
public class DonHangTests
{
    private readonly CsdlKiemThu _csdl;
    public DonHangTests(CsdlKiemThu csdl) => _csdl = csdl;

    private static ThongTinNhanHangDto NguoiNhan() => new()
    {
        HoTenNhan = "Người nhận kiểm thử", SoDienThoaiNhan = "0909000111", DiaChiGiaoHang = "1 Lê Lợi, Quận 1"
    };

    private async Task<long> DatHangAsync(int maSach, int soLuong)
    {
        await using var db = _csdl.TaoDb();
        return await new DonHangService(db).DatHangAsync(await _csdl.MaKhachHangAsync("khachhang"), NguoiNhan(),
            new[] { new DongDatHang(maSach, soLuong) });
    }

    [Theory]
    [InlineData(TrangThaiDonHang.ChoXacNhan, TrangThaiDonHang.DaXacNhan, true)]
    [InlineData(TrangThaiDonHang.DaXacNhan, TrangThaiDonHang.DangGiao, true)]
    [InlineData(TrangThaiDonHang.DangGiao, TrangThaiDonHang.HoanThanh, true)]
    [InlineData(TrangThaiDonHang.DangGiao, TrangThaiDonHang.DaHuy, true)]
    [InlineData(TrangThaiDonHang.ChoXacNhan, TrangThaiDonHang.HoanThanh, false)] // nhảy cóc
    [InlineData(TrangThaiDonHang.HoanThanh, TrangThaiDonHang.DaHuy, false)]      // đơn đã kết thúc
    [InlineData(TrangThaiDonHang.DaHuy, TrangThaiDonHang.ChoXacNhan, false)]
    public void LaChuyenHopLe_DungSoDoTrangThai(TrangThaiDonHang cu, TrangThaiDonHang moi, bool mongDoi) =>
        Assert.Equal(mongDoi, DonHang.LaChuyenHopLe(cu, moi));

    [Fact] // UT04
    public async Task DatHang_GioRong_BaoLoi()
    {
        await using var db = _csdl.TaoDb();
        var ex = await Assert.ThrowsAsync<BusinessException>(async () => await
            new DonHangService(db).DatHangAsync(await _csdl.MaKhachHangAsync("khachhang"), NguoiNhan(), Array.Empty<DongDatHang>()));
        Assert.Contains("trống", ex.Message);
    }

    [Fact] // UT05
    public async Task DatHang_DuTon_TaoDonChoXacNhan_TruTon_TinhDungTongTien()
    {
        int maSach = await _csdl.TaoSachAsync(ton: 10, gia: 45_000m);
        long maDon = await DatHangAsync(maSach, 3);

        await using var db = _csdl.TaoDb();
        var don = await db.DonHangs.Include(d => d.ChiTiets).SingleAsync(d => d.MaDonHang == maDon);
        Assert.Equal(TrangThaiDonHang.ChoXacNhan, don.TrangThai);
        Assert.Equal(135_000m, don.TongTien);
        Assert.Equal(135_000m, don.ChiTiets.Single().ThanhTien); // cột tính toán trong SQL Server
        Assert.Equal(7, await _csdl.TonKhoAsync(maSach));
    }

    [Fact] // UT06
    public async Task DatHang_KhongDuTon_KhongTaoDon_TonGiuNguyen()
    {
        int maSach = await _csdl.TaoSachAsync(ton: 2);
        await using var db = _csdl.TaoDb();
        int soDonTruoc = await db.DonHangs.CountAsync();

        await Assert.ThrowsAsync<BusinessException>(async () => await DatHangAsync(maSach, 3));

        Assert.Equal(soDonTruoc, await db.DonHangs.CountAsync());
        Assert.Equal(2, await _csdl.TonKhoAsync(maSach));
    }

    [Fact] // UT07
    public async Task ChuyenTrangThai_NhayCoc_BaoLoi()
    {
        long maDon = await DatHangAsync(await _csdl.TaoSachAsync(5), 1);
        await using var db = _csdl.TaoDb();
        await Assert.ThrowsAsync<BusinessException>(async () => await new DonHangService(db)
            .ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.HoanThanh, await _csdl.MaTaiKhoanAsync("nvbanhang")));
    }

    [Fact] // UT08
    public async Task KhachHuyDonChoXacNhan_HoanTon()
    {
        int maSach = await _csdl.TaoSachAsync(5);
        long maDon = await DatHangAsync(maSach, 2);
        Assert.Equal(3, await _csdl.TonKhoAsync(maSach));

        await using var db = _csdl.TaoDb();
        await new DonHangService(db).HuyDonAsync(maDon, await _csdl.MaKhachHangAsync("khachhang"));

        Assert.Equal(TrangThaiDonHang.DaHuy, (await db.DonHangs.SingleAsync(d => d.MaDonHang == maDon)).TrangThai);
        Assert.Equal(5, await _csdl.TonKhoAsync(maSach));
    }

    [Fact] // Quy trình liên thông Web → Desktop → Web (Bảng 5.6)
    public async Task QuyTrinhLienThong_DatHang_XuLyDon_KhachThayTrangThai()
    {
        int maSach = await _csdl.TaoSachAsync(10);
        long maDon = await DatHangAsync(maSach, 2);                       // Web: khách đặt
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        int kh = await _csdl.MaKhachHangAsync("khachhang");

        await using (var db = _csdl.TaoDb())                              // Desktop: nhân viên xác nhận
            await new DonHangService(db).ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.DaXacNhan, nv);

        await using (var db = _csdl.TaoDb())                              // Web: khách thấy "Đã xác nhận", không hủy được nữa
        {
            var svc = new DonHangService(db);
            var don = await svc.LayChiTietAsync(maDon, kh);
            Assert.Equal(TrangThaiDonHang.DaXacNhan, don!.TrangThai);
            Assert.False(don.CoTheHuy);
            await Assert.ThrowsAsync<BusinessException>(async () => await svc.HuyDonAsync(maDon, kh));
        }

        await using (var db = _csdl.TaoDb())                              // Desktop: giao → hoàn thành
        {
            var svc = new DonHangService(db);
            await svc.ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.DangGiao, nv);
            await svc.ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.HoanThanh, nv);
        }

        await using (var db = _csdl.TaoDb())
        {
            var don = await db.DonHangs.SingleAsync(d => d.MaDonHang == maDon);
            Assert.Equal(TrangThaiDonHang.HoanThanh, don.TrangThai);
            Assert.Equal(TrangThaiThanhToan.DaThanhToan, don.TrangThaiThanhToan); // COD: hoàn thành = đã thu tiền
        }
        Assert.Equal(8, await _csdl.TonKhoAsync(maSach));
    }

    [Fact] // BR13: nhân viên hủy đơn Đang giao (giao thất bại) → hoàn tồn; đơn hoàn thành không hủy được
    public async Task NhanVienHuyDonDangGiao_HoanTon_DonHoanThanhKhongHuyDuoc()
    {
        int maSach = await _csdl.TaoSachAsync(10);
        long maDon = await DatHangAsync(maSach, 4);
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        await using var db = _csdl.TaoDb();
        var svc = new DonHangService(db);
        await svc.ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.DaXacNhan, nv);
        await svc.ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.DangGiao, nv);

        await svc.HuyDonBoiNhanVienAsync(maDon, nv, "Khách không nhận hàng");
        Assert.Equal(10, await _csdl.TonKhoAsync(maSach));
        // Hủy lần hai không được hoàn kho thêm lần nữa
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.HuyDonBoiNhanVienAsync(maDon, nv, null));
        Assert.Equal(10, await _csdl.TonKhoAsync(maSach));

        long donKhac = await DatHangAsync(maSach, 1);
        await svc.ChuyenTrangThaiAsync(donKhac, TrangThaiDonHang.DaXacNhan, nv);
        await svc.ChuyenTrangThaiAsync(donKhac, TrangThaiDonHang.DangGiao, nv);
        await svc.ChuyenTrangThaiAsync(donKhac, TrangThaiDonHang.HoanThanh, nv);
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.HuyDonBoiNhanVienAsync(donKhac, nv, null));
    }

    [Fact] // ST03: nhân viên kho gọi trực tiếp chức năng xử lý đơn bị từ chối
    public async Task NhanVienKho_KhongXuLyDuocDonHang()
    {
        long maDon = await DatHangAsync(await _csdl.TaoSachAsync(5), 1);
        await using var db = _csdl.TaoDb();
        var ex = await Assert.ThrowsAsync<BusinessException>(async () => await new DonHangService(db)
            .ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.DaXacNhan, await _csdl.MaTaiKhoanAsync("nvkho")));
        Assert.Contains("không có quyền", ex.Message);
    }

    [Fact] // Tranh chấp: khách vừa hủy trên Web thì nhân viên xác nhận (dữ liệu cũ) bị từ chối, không ghi đè
    public async Task NhanVienXacNhanDonVuaBiKhachHuy_BiTuChoi()
    {
        long maDon = await DatHangAsync(await _csdl.TaoSachAsync(5), 1);
        await using (var db = _csdl.TaoDb())
            await new DonHangService(db).HuyDonAsync(maDon, await _csdl.MaKhachHangAsync("khachhang"));
        await using (var db = _csdl.TaoDb())
            await Assert.ThrowsAsync<BusinessException>(async () => await new DonHangService(db)
                .ChuyenTrangThaiAsync(maDon, TrangThaiDonHang.DaXacNhan, await _csdl.MaTaiKhoanAsync("nvbanhang")));
    }
}
