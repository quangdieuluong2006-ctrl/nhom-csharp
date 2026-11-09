using BookStore.Application.Services;
using BookStore.Desktop.HaTang;

namespace BookStore.Desktop.Forms;

/// <summary>Người dùng Desktop tự đổi mật khẩu (dùng chung TaiKhoanService.DoiMatKhauAsync với Web).</summary>
public partial class FrmDoiMatKhau : Form
{
    private readonly ITaiKhoanService _svc;
    private readonly PhienDangNhap _phien;

    public FrmDoiMatKhau(ITaiKhoanService svc, PhienDangNhap phien)
    {
        InitializeComponent();
        _svc = svc;
        _phien = phien;
    }

    private async void btnDoi_Click(object sender, EventArgs e)
    {
        if (txtMatKhauMoi.Text.Length < 6) { ThongBao.CanhBao("Mật khẩu mới phải có ít nhất 6 ký tự."); return; }
        if (txtMatKhauMoi.Text != txtNhapLai.Text) { ThongBao.CanhBao("Mật khẩu nhập lại không khớp."); return; }
        bool ok = await GiaoDien.ThucHienAsync(this, () => _svc.DoiMatKhauAsync(_phien.MaTaiKhoan, txtMatKhauCu.Text, txtMatKhauMoi.Text));
        if (!ok) return;
        ThongBao.ThanhCong("Đổi mật khẩu thành công.");
        DialogResult = DialogResult.OK;
        Close();
    }
}
