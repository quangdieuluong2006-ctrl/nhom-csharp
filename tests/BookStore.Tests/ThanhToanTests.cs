using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests;

/// <summary>Thanh toán COD / chuyển khoản / MoMo và liên thông Web ↔ Desktop khi xác nhận đã nhận tiền.</summary>
[Collection("CSDL")]
public class ThanhToanTests
{
    private readonly CsdlKiemThu _csdl;
    public ThanhToanTests(CsdlKiemThu csdl) => _csdl = csdl;

    private async Task<long> DatHangAsync(string phuongThuc, int ton = 10)
    {
        int maSach = await _csdl.TaoSachAsync(ton);
        await using var db = _csdl.TaoDb();
        return await new DonHangService(db).DatHangAsync(await _csdl.MaKhachHangAsync("khachhang"),
            new ThongTinNhanHangDto { HoTenNhan = "A", SoDienThoaiNhan = "0909000333", DiaChiGiaoHang = "B", PhuongThucThanhToan = phuongThuc },
            new[] { new DongDatHang(maSach, 1) });
    }

    [Fact]
    public async Task PhuongThucKhongHopLe_BiTuChoi()
    {
        await Assert.ThrowsAsync<BusinessException>(() => DatHangAsync("TheTinDung"));
    }

    [Fact] // Web đặt MoMo → Desktop chưa giao được → xác nhận đã nhận tiền → giao, hoàn thành
    public async Task MoMo_PhaiXacNhanDaNhanTienMoiDuocGiao()
    {
        long ma = await DatHangAsync(PhuongThuc.MoMo);
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang"), kh = await _csdl.MaKhachHangAsync("khachhang");
        await using var db = _csdl.TaoDb();
        var svc = new DonHangService(db);

        var chiTiet = (await svc.LayChiTietAsync(ma, kh))!;
        Assert.Equal(PhuongThuc.MoMo, chiTiet.PhuongThucThanhToan);
        Assert.True(chiTiet.CanThanhToanQr);                       // Web hiện mã QR

        await svc.ChuyenTrangThaiAsync(ma, TrangThaiDonHang.DaXacNhan, nv);
        var ex = await Assert.ThrowsAsync<BusinessException>(() => svc.ChuyenTrangThaiAsync(ma, TrangThaiDonHang.DangGiao, nv));
        Assert.Contains("chưa nhận được tiền", ex.Message);

        await svc.XacNhanDaThanhToanAsync(ma, nv);                  // Desktop: đã thấy tiền vào tài khoản
        Assert.False((await svc.LayChiTietAsync(ma, kh))!.CanThanhToanQr);   // Web: hết hiện QR
        await svc.ChuyenTrangThaiAsync(ma, TrangThaiDonHang.DangGiao, nv);
        await svc.ChuyenTrangThaiAsync(ma, TrangThaiDonHang.HoanThanh, nv);
        Assert.Equal(TrangThaiThanhToan.DaThanhToan, (await db.DonHangs.AsNoTracking().SingleAsync(d => d.MaDonHang == ma)).TrangThaiThanhToan);
    }

    [Fact]
    public async Task KhachKhongTuHuyDonDaThanhToan_NhanVienHuyThiGhiChuHoanTien()
    {
        long ma = await DatHangAsync(PhuongThuc.ChuyenKhoan);
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang"), kh = await _csdl.MaKhachHangAsync("khachhang");
        await using var db = _csdl.TaoDb();
        var svc = new DonHangService(db);
        await svc.XacNhanDaThanhToanAsync(ma, nv);

        Assert.False((await svc.LayChiTietAsync(ma, kh))!.CoTheHuy);
        await Assert.ThrowsAsync<BusinessException>(() => svc.HuyDonAsync(ma, kh));

        await svc.HuyDonBoiNhanVienAsync(ma, nv, "Hết hàng ở kho");
        var don = await db.DonHangs.AsNoTracking().SingleAsync(d => d.MaDonHang == ma);
        Assert.Equal(TrangThaiDonHang.DaHuy, don.TrangThai);
        Assert.Contains("CẦN HOÀN", don.GhiChu);
    }

    [Fact]
    public async Task DonCOD_KhongXacNhanThanhToanTruoc_GiaoDuocNgay()
    {
        long ma = await DatHangAsync(PhuongThuc.COD);
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        await using var db = _csdl.TaoDb();
        var svc = new DonHangService(db);
        await Assert.ThrowsAsync<BusinessException>(() => svc.XacNhanDaThanhToanAsync(ma, nv));
        await svc.ChuyenTrangThaiAsync(ma, TrangThaiDonHang.DaXacNhan, nv);
        await svc.ChuyenTrangThaiAsync(ma, TrangThaiDonHang.DangGiao, nv);   // COD: thu tiền khi giao
        await Assert.ThrowsAsync<BusinessException>(async () =>
            await svc.XacNhanDaThanhToanAsync(ma, await _csdl.MaTaiKhoanAsync("nvkho")));  // NV kho không có quyền
    }
}
