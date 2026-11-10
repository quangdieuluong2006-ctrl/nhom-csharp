using BookStore.Application.Common;
using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests;

[Collection("CSDL")]
public class KhoVaBaoCaoTests
{
    private readonly CsdlKiemThu _csdl;
    public KhoVaBaoCaoTests(CsdlKiemThu csdl) => _csdl = csdl;

    [Fact] // UT09
    public async Task PhieuXuatVuotTon_BaoLoi_KhongTaoPhieu_TonGiuNguyen()
    {
        int a = await _csdl.TaoSachAsync(10), b = await _csdl.TaoSachAsync(1);
        await using var db = _csdl.TaoDb();
        int soPhieuTruoc = await db.PhieuKhos.CountAsync();
        var phieu = new PhieuKhoDto
        {
            LoaiPhieu = LoaiPhieuKho.Xuat,
            Dong = { new DongPhieuKhoDto(a, 3, null), new DongPhieuKhoDto(b, 2, null) } // dòng 2 vượt tồn
        };

        await Assert.ThrowsAsync<BusinessException>(async () => await
            new KhoService(db).LuuPhieuAsync(phieu, await _csdl.MaTaiKhoanAsync("nvkho")));

        // Giao tác bị hoàn tác: dòng 1 dù đã trừ cũng được trả lại
        Assert.Equal(soPhieuTruoc, await db.PhieuKhos.CountAsync());
        Assert.Equal(10, await _csdl.TonKhoAsync(a));
        Assert.Equal(1, await _csdl.TonKhoAsync(b));
    }

    [Fact] // US15: phiếu nhập 2 dòng, dòng trùng sách được cộng dồn
    public async Task PhieuNhap_TangTon_GopDongTrung()
    {
        int a = await _csdl.TaoSachAsync(0), b = await _csdl.TaoSachAsync(4);
        await using var db = _csdl.TaoDb();
        long maPhieu = await new KhoService(db).LuuPhieuAsync(new PhieuKhoDto
        {
            LoaiPhieu = LoaiPhieuKho.Nhap,
            Dong = { new(a, 5, 30_000m), new(b, 2, 20_000m), new(a, 1, 30_000m) }
        }, await _csdl.MaTaiKhoanAsync("nvkho"));

        Assert.Equal(6, await _csdl.TonKhoAsync(a));
        Assert.Equal(6, await _csdl.TonKhoAsync(b));
        Assert.Equal(2, await db.ChiTietPhieuKhos.CountAsync(c => c.MaPhieu == maPhieu));
    }

    [Fact]
    public async Task PhieuNhapThieuDonGia_VaNhanVienBanHangLapPhieu_BiTuChoi()
    {
        int a = await _csdl.TaoSachAsync(0);
        await using var db = _csdl.TaoDb();
        var svc = new KhoService(db);
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.LuuPhieuAsync(
            new PhieuKhoDto { LoaiPhieu = LoaiPhieuKho.Nhap, Dong = { new(a, 1, null) } }, await _csdl.MaTaiKhoanAsync("nvkho")));
        await Assert.ThrowsAsync<BusinessException>(async () => await svc.LuuPhieuAsync(
            new PhieuKhoDto { LoaiPhieu = LoaiPhieuKho.Nhap, Dong = { new(a, 1, 1000m) } }, await _csdl.MaTaiKhoanAsync("nvbanhang")));
        Assert.Equal(0, await _csdl.TonKhoAsync(a));
    }

    [Fact] // UT10: doanh thu chỉ tính đơn Hoàn thành
    public async Task DoanhThu_ChiCongDonHoanThanh()
    {
        var homNay = DateTime.Today;
        int admin = await _csdl.MaTaiKhoanAsync("admin");
        decimal truoc;
        await using (var db = _csdl.TaoDb())
            truoc = (await new BaoCaoService(db).DoanhThuAsync(homNay, homNay, admin)).Single().DoanhThu;

        int maSach = await _csdl.TaoSachAsync(10, gia: 100_000m);
        int kh = await _csdl.MaKhachHangAsync("khachhang2");
        int nv = await _csdl.MaTaiKhoanAsync("nvbanhang");
        var nhan = new ThongTinNhanHangDto { HoTenNhan = "A", SoDienThoaiNhan = "0909000222", DiaChiGiaoHang = "B" };
        await using (var db = _csdl.TaoDb())
        {
            var svc = new DonHangService(db);
            long hoanThanh = await svc.DatHangAsync(kh, nhan, new[] { new DongDatHang(maSach, 2) }); // 200.000
            long biHuy = await svc.DatHangAsync(kh, nhan, new[] { new DongDatHang(maSach, 1) });     // 100.000 – hủy
            long dangGiao = await svc.DatHangAsync(kh, nhan, new[] { new DongDatHang(maSach, 1) });  // 100.000 – chưa xong
            foreach (var t in new[] { TrangThaiDonHang.DaXacNhan, TrangThaiDonHang.DangGiao, TrangThaiDonHang.HoanThanh })
                await svc.ChuyenTrangThaiAsync(hoanThanh, t, nv);
            await svc.HuyDonBoiNhanVienAsync(biHuy, nv, null);
            await svc.ChuyenTrangThaiAsync(dangGiao, TrangThaiDonHang.DaXacNhan, nv);
        }

        await using (var db = _csdl.TaoDb())
        {
            var bc = new BaoCaoService(db);
            Assert.Equal(truoc + 200_000m, (await bc.DoanhThuAsync(homNay, homNay, admin)).Single().DoanhThu);
            var banChay = await bc.SachBanChayAsync(homNay, homNay, admin, 100);
            Assert.Equal(2, banChay.Single(x => x.MaSach == maSach).SoLuongBan);
        }
    }

    [Fact]
    public async Task DoanhThu_DuNgayKeCaNgayKhongCoDon()
    {
        await using var db = _csdl.TaoDb();
        var kq = await new BaoCaoService(db).DoanhThuAsync(DateTime.Today.AddDays(-6), DateTime.Today, await _csdl.MaTaiKhoanAsync("nvbanhang"));
        Assert.Equal(7, kq.Count);
    }

    [Fact]
    public async Task TonKho_SapHet_ChiGomSachTonToiDa5()
    {
        int it = await _csdl.TaoSachAsync(3), nhieu = await _csdl.TaoSachAsync(50);
        await using var db = _csdl.TaoDb();
        var ds = await new KhoService(db).LayTonKhoAsync(null, chiSapHet: true);
        Assert.Contains(ds, x => x.MaSach == it && x.TinhTrang == "Sắp hết");
        Assert.DoesNotContain(ds, x => x.MaSach == nhieu);
    }

    [Fact] // ST03 – thao tác đọc dữ liệu nhạy cảm cũng kiểm tra quyền ở service
    public async Task NhanVienKho_KhongXemDuocDoanhThu_NhanVienBanHang_KhongXemDuocLichSuKho()
    {
        int kho = await _csdl.MaTaiKhoanAsync("nvkho"), banHang = await _csdl.MaTaiKhoanAsync("nvbanhang");
        await using var db = _csdl.TaoDb();
        await Assert.ThrowsAsync<BusinessException>(async () => await new BaoCaoService(db).DoanhThuAsync(DateTime.Today, DateTime.Today, kho));
        await Assert.ThrowsAsync<BusinessException>(async () => await new KhoService(db).LayLichSuPhieuAsync(null, null, null, banHang));
        await Assert.ThrowsAsync<BusinessException>(async () => await new KhachHangService(db).TimKiemAsync(kho, null));

        // Tổng quan: NV kho xem được số liệu kho nhưng không thấy doanh thu
        var tq = await new BaoCaoService(db).TongQuanAsync(kho);
        Assert.Equal(0m, tq.DoanhThuThangNay);
        Assert.True(tq.TongDauSach > 0);
    }
}
