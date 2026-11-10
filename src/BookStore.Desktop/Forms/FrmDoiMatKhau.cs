using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>Người dùng Desktop tự đổi mật khẩu (dùng chung TaiKhoanService.DoiMatKhauAsync với Web).</summary>
public partial class FrmDoiMatKhau : Form
{
    private readonly ITaiKhoanService _svc;
    private readonly PhienDangNhap _phien;
    private readonly KiemTraNhap _kiemTra;

    public FrmDoiMatKhau(ITaiKhoanService svc, PhienDangNhap phien)
    {
        InitializeComponent();
        _kiemTra = new KiemTraNhap(this)
            .QuyTac(txtMatKhauCu, () => txtMatKhauCu.Text.Length == 0 ? "Vui lòng nhập mật khẩu hiện tại." : null)
            .QuyTac(txtMatKhauMoi, () => txtMatKhauMoi.Text.Length < 6 ? "Mật khẩu mới phải có ít nhất 6 ký tự." : null)
            .QuyTac(txtNhapLai, () => txtMatKhauMoi.Text != txtNhapLai.Text ? "Mật khẩu nhập lại không khớp." : null);
        _svc = svc;
        _phien = phien;
    }

    private async void btnDoi_Click(object sender, EventArgs e)
    {
        if (!_kiemTra.HopLe()) return;
        bool ok = await GiaoDien.ThucHienAsync(this, () => _svc.DoiMatKhauAsync(_phien.MaTaiKhoan, txtMatKhauCu.Text, txtMatKhauMoi.Text));
        if (!ok) return;
        ThongBao.ThanhCong("Đổi mật khẩu thành công.");
        DialogResult = DialogResult.OK;
        Close();
    }
}
