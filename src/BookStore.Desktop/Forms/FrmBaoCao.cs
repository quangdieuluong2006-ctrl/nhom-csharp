using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>D09 – Báo cáo doanh thu theo ngày và sách bán chạy (US16: chỉ tính đơn Hoàn thành). QTV, NV bán hàng.</summary>
public partial class FrmBaoCao : Form
{
    private readonly IBaoCaoService _svc;
    private readonly PhienDangNhap _phien;
    private List<DoanhThuNgayDto> _doanhThu = new();
    private List<SachBanChayDto> _banChay = new();

    public FrmBaoCao(IBaoCaoService svc, PhienDangNhap phien)
    {
        InitializeComponent();
        _svc = svc;
        _phien = phien;
        GiaoDien.KieuBang(dgvTheoNgay);
        GiaoDien.KieuBang(dgvBanChay);
        dtpTuNgay.Value = DateTime.Today.AddDays(-29);
        dtpDenNgay.Value = DateTime.Today;
        Load += async (_, _) => await GiaoDien.ThucHienAsync(this, TaiBaoCaoAsync);
    }

    private async Task TaiBaoCaoAsync()
    {
        var tu = dtpTuNgay.Value.Date;
        var den = dtpDenNgay.Value.Date;
        _doanhThu = await _svc.DoanhThuAsync(tu, den, _phien.MaTaiKhoan);
        _banChay = await _svc.SachBanChayAsync(tu, den, _phien.MaTaiKhoan, 20);

        GiaoDien.GanDanhSach(dgvTheoNgay, _doanhThu);
        if (dgvTheoNgay.Columns["Ngay"] is { } cotNgay) cotNgay.DefaultCellStyle.Format = "dd/MM/yyyy (ddd)";
        GiaoDien.GanDanhSach(dgvBanChay, _banChay);
        GiaoDien.DoRongCot(dgvBanChay, ("MaSach", 25), ("TenSach", 200));
        bieuDo.HienThi(_doanhThu.Select(d => (d.Ngay.ToString("dd/MM"), d.DoanhThu)));

        lblTongKet.Text = $"Từ {tu:dd/MM/yyyy} đến {den:dd/MM/yyyy}:  doanh thu {_doanhThu.Sum(d => d.DoanhThu):N0} đ  ·  "
                          + $"{_doanhThu.Sum(d => d.SoDon)} đơn hoàn thành  ·  {_banChay.Sum(b => b.SoLuongBan)} cuốn đã bán (top 20)";
    }

    private async void btnXem_Click(object sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, TaiBaoCaoAsync);

    private async void btn7Ngay_Click(object sender, EventArgs e)
    {
        dtpTuNgay.Value = DateTime.Today.AddDays(-6);
        dtpDenNgay.Value = DateTime.Today;
        await GiaoDien.ThucHienAsync(this, TaiBaoCaoAsync);
    }

    private async void btnThangNay_Click(object sender, EventArgs e)
    {
        dtpTuNgay.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        dtpDenNgay.Value = DateTime.Today;
        await GiaoDien.ThucHienAsync(this, TaiBaoCaoAsync);
    }

    /// <summary>Xuất bảng đang xem (doanh thu theo ngày hoặc sách bán chạy) ra Excel.</summary>
    private void btnXuatExcel_Click(object sender, EventArgs e)
    {
        var khoang = $"từ {dtpTuNgay.Value:dd/MM/yyyy} đến {dtpDenNgay.Value:dd/MM/yyyy}";
        if (tabBaoCao.SelectedTab == tabBanChay)
            XuatExcel.Xuat(this, _banChay, $"SÁCH BÁN CHẠY {khoang.ToUpper()}", "SachBanChay");
        else
            XuatExcel.Xuat(this, _doanhThu, $"DOANH THU THEO NGÀY {khoang.ToUpper()}", "DoanhThu");
    }
}
