using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.Forms;

/// <summary>
/// D02 – Màn hình tổng quan (mở ngay sau đăng nhập): số đơn chờ xác nhận, doanh thu, sách sắp hết;
/// danh sách đơn mới từ website và sách sắp hết. Tự làm mới mỗi 30 giây.
/// </summary>
public partial class FrmTongQuan : Form
{
    private readonly IServiceScopeFactory _scopes;
    private readonly PhienDangNhap _phien;
    private readonly ILogger<FrmTongQuan> _log;
    private readonly bool _xemBanHang, _xemKho;
    private bool _dangTai;

    /// <summary>Nhấp đúp một đơn → form chính mở màn hình Xử lý đơn hàng tại đơn đó.</summary>
    public event Func<long, Task>? YeuCauMoDon;

    public FrmTongQuan(IServiceScopeFactory scopes, PhienDangNhap phien, ILogger<FrmTongQuan> log)
    {
        InitializeComponent();
        _scopes = scopes;
        _phien = phien;
        _log = log;
        _xemBanHang = phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienBanHang);
        _xemKho = phien.CoQuyen(VaiTro.QuanTriVien, VaiTro.NhanVienKho);
        // Màn hình hẹp: các ô số liệu xuống dòng thì khung tự cao lên, không bị che
        flowThe.AutoSize = true;
        flowThe.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        GiaoDien.KieuBang(dgvDonMoi);
        GiaoDien.KieuBang(dgvSapHet);

        // Mỗi vai trò chỉ thấy số liệu thuộc phạm vi của mình (ma trận chức năng – Bảng 3.13)
        pnlDonCho.Visible = pnlDangXuLy.Visible = pnlDoanhThuNgay.Visible = pnlDoanhThuThang.Visible = _xemBanHang;
        dgvDonMoi.Visible = lblDonMoi.Visible = _xemBanHang;
        if (!_xemBanHang) { tlpBang.ColumnStyles[0].Width = 0; tlpBang.ColumnStyles[1].Width = 100; }
        lblTieuDe.Text = $"Tổng quan cửa hàng – xin chào {phien.NguoiDung?.HoTen}";

        Load += async (_, _) => { await TaiAsync(); tmrLamMoi.Start(); };
    }

    private async Task TaiAsync()
    {
        if (_dangTai) return;
        _dangTai = true;
        try
        {
            // Scope riêng cho mỗi lần làm mới: DbContext mới, không giữ dữ liệu cũ trong bộ nhớ
            using var scope = _scopes.CreateScope();
            var baoCao = scope.ServiceProvider.GetRequiredService<IBaoCaoService>();
            var tq = await baoCao.TongQuanAsync(_phien.MaTaiKhoan);
            lblGiaTriDonCho.Text = tq.DonChoXacNhan.ToString();
            lblGiaTriDonCho.ForeColor = tq.DonChoXacNhan > 0 ? Color.FromArgb(192, 57, 43) : GiaoDien.MauChuDao;
            lblGiaTriDangXuLy.Text = tq.DonDangXuLy.ToString();
            lblGiaTriDoanhThuNgay.Text = $"{tq.DoanhThuHomNay:N0} đ";
            lblGiaTriDoanhThuThang.Text = $"{tq.DoanhThuThangNay:N0} đ";
            lblGiaTriSapHet.Text = tq.SachSapHet.ToString();
            lblGiaTriSapHet.ForeColor = tq.SachSapHet > 0 ? Color.FromArgb(192, 57, 43) : GiaoDien.MauChuDao;
            lblGiaTriDauSach.Text = tq.TongDauSach.ToString();

            if (_xemBanHang)
            {
                var don = await scope.ServiceProvider.GetRequiredService<IDonHangService>()
                    .TimKiemQuanTriAsync(new LocDonHang { TrangThai = TrangThaiDonHang.ChoXacNhan }, _phien.MaTaiKhoan);
                GiaoDien.GanDanhSach(dgvDonMoi, don);
                foreach (var c in new[] { "HoTenNhan", "SoDienThoaiNhan", "TrangThaiText", "ThanhToanText" })
                    if (dgvDonMoi.Columns[c] is { } cot) cot.Visible = false;
            }
            var sapHet = await scope.ServiceProvider.GetRequiredService<IKhoService>().LayTonKhoAsync(null, chiSapHet: true);
            GiaoDien.GanDanhSach(dgvSapHet, sapHet);
            if (dgvSapHet.Columns["DanhMuc"] is { } dm) dm.Visible = false;
            if (dgvSapHet.Columns["MaSach"] is { } ms) ms.FillWeight = 25;
            lblCapNhat.Text = $"Cập nhật lúc {DateTime.Now:HH:mm:ss} · tự làm mới mỗi 30 giây · "
                              + (_xemKho ? "nhập thêm hàng ở menu Kho → Lập phiếu kho" : "đơn mới từ website hiện ở bảng bên trái");
        }
        finally { _dangTai = false; }
    }

    private async void dgvDonMoi_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && dgvDonMoi.Rows[e.RowIndex].DataBoundItem is DonHangQtDto d && YeuCauMoDon != null)
            await YeuCauMoDon(d.MaDonHang);
    }

    private async void btnLamMoi_Click(object sender, EventArgs e) => await GiaoDien.ThucHienAsync(btnLamMoi, TaiAsync);

    private async void tmrLamMoi_Tick(object? sender, EventArgs e)
    {
        try { await TaiAsync(); }
        catch (Exception ex) { _log.LogWarning(ex, "Làm mới màn hình tổng quan thất bại"); }
    }
}
