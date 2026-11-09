using BookStore.Application.Dtos;
using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;

namespace BookStore.Desktop.Forms;

/// <summary>D07/D09 – Tồn kho, sách sắp hết (≤ 5) và lịch sử phiếu kho. QTV, NV kho.</summary>
public partial class FrmKho : Form
{
    private readonly IKhoService _kho;
    private List<TonKhoDto> _tonKho = new();
    private bool _dangNap;

    public FrmKho(IKhoService kho)
    {
        InitializeComponent();
        _kho = kho;
        foreach (var g in new[] { dgvTonKho, dgvPhieu, dgvChiTietPhieu }) GiaoDien.KieuBang(g);
        GiaoDien.GanCombo(cboLoai, Enum.GetValues<LoaiPhieuKho>().Select(l => new MucChonDto((int)l + 1, l.Ten())), "(Tất cả loại)");
        dtpTuNgay.Value = DateTime.Today.AddDays(-30);
        dtpDenNgay.Value = DateTime.Today;
        Load += async (_, _) => await GiaoDien.ThucHienAsync(this, async () => { await TaiTonKhoAsync(); await TaiLichSuAsync(); });
    }

    private async Task TaiTonKhoAsync()
    {
        _tonKho = await _kho.LayTonKhoAsync(txtTimTonKho.Text, chkSapHet.Checked);
        GiaoDien.GanDanhSach(dgvTonKho, _tonKho);
        GiaoDien.DoRongCot(dgvTonKho, ("MaSach", 25), ("TenSach", 180), ("SoLuongTon", 40), ("TinhTrang", 50));
        tabTonKho.Text = $"Tồn kho ({_tonKho.Count(t => t.SoLuongTon <= KhoService.NguongSapHet)} sách sắp hết)";
    }

    private async Task TaiLichSuAsync()
    {
        _dangNap = true;
        LoaiPhieuKho? loai = GiaoDien.MaDangChon(cboLoai) is int m ? (LoaiPhieuKho)(m - 1) : null;
        var ds = await _kho.LayLichSuPhieuAsync(dtpTuNgay.Value, dtpDenNgay.Value, loai);
        GiaoDien.GanDanhSach(dgvPhieu, ds);
        GiaoDien.DoRongCot(dgvPhieu, ("MaPhieu", 30), ("SoDong", 30), ("TongSoLuong", 40), ("GhiChu", 150));
        _dangNap = false;
        await TaiChiTietPhieuAsync();
    }

    private async Task TaiChiTietPhieuAsync()
    {
        if (GiaoDien.DongDangChon<PhieuKhoQtDto>(dgvPhieu) is not { } p)
        {
            dgvChiTietPhieu.DataSource = null;
            lblChiTietPhieu.Text = "Chi tiết phiếu đang chọn";
            return;
        }
        GiaoDien.GanDanhSach(dgvChiTietPhieu, await _kho.LayChiTietPhieuAsync(p.MaPhieu));
        GiaoDien.DoRongCot(dgvChiTietPhieu, ("MaSach", 25), ("TenSach", 200));
        lblChiTietPhieu.Text = $"Chi tiết phiếu #{p.MaPhieu} – {p.LoaiText} – {p.NgayLap:dd/MM/yyyy HH:mm} – {p.NguoiLap}";
    }

    /// <summary>Hết hàng: chữ đỏ; sắp hết: nền vàng nhạt.</summary>
    private void dgvTonKho_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || dgvTonKho.Rows[e.RowIndex].DataBoundItem is not TonKhoDto t) return;
        if (t.SoLuongTon == 0) e.CellStyle!.ForeColor = Color.Firebrick;
        else if (t.SoLuongTon <= KhoService.NguongSapHet) e.CellStyle!.BackColor = Color.FromArgb(255, 248, 220);
    }

    private async void dgvPhieu_SelectionChanged(object sender, EventArgs e)
    {
        if (!_dangNap) await GiaoDien.ChayTuanTuAsync(this, TaiChiTietPhieuAsync);
    }

    private async void btnXemTonKho_Click(object? sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, TaiTonKhoAsync);

    private async void btnXemLichSu_Click(object sender, EventArgs e) => await GiaoDien.ThucHienAsync(this, TaiLichSuAsync);

    private void btnXuatTonKho_Click(object sender, EventArgs e) =>
        XuatExcel.Xuat(this, _tonKho, chkSapHet.Checked ? "BÁO CÁO SÁCH SẮP HẾT HÀNG (TỒN ≤ 5)" : "BÁO CÁO TỒN KHO", "TonKho");

    private void txtTimTonKho_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true;
        btnXemTonKho_Click(sender, e);
    }
}
