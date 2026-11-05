using BookStore.Application.Services;
using BookStore.Desktop.HaTang;
using BookStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace BookStore.Desktop.Forms;

/// <summary>Đăng nhập phần mềm quản trị – chỉ dành cho quản trị viên và nhân viên.</summary>
public partial class FrmDangNhap : Form
{
    private readonly ITaiKhoanService _taiKhoan;
    private readonly PhienDangNhap _phien;
    private readonly ILogger<FrmDangNhap> _log;

    public FrmDangNhap(ITaiKhoanService taiKhoan, PhienDangNhap phien, ILogger<FrmDangNhap> log)
    {
        InitializeComponent();
        _taiKhoan = taiKhoan;
        _phien = phien;
        _log = log;
    }

    private async void btnDangNhap_Click(object sender, EventArgs e)
    {
        lblLoi.Text = "";
        var ten = txtTenDangNhap.Text.Trim();
        if (ten.Length == 0 || txtMatKhau.Text.Length == 0)
        {
            lblLoi.Text = "Vui lòng nhập tên đăng nhập và mật khẩu.";
            return;
        }

        DatTrangThaiCho(true);
        try
        {
            var nd = await _taiKhoan.DangNhapAsync(ten, txtMatKhau.Text);
            if (nd is null)
            {
                _log.LogWarning("Đăng nhập Desktop thất bại: {TenDangNhap}", ten);
                lblLoi.Text = "Tên đăng nhập hoặc mật khẩu không đúng.";
                txtMatKhau.SelectAll();
                txtMatKhau.Focus();
                return;
            }
            if (nd.VaiTro == VaiTro.KhachHang)
            {
                lblLoi.Text = "Tài khoản khách hàng chỉ dùng trên website.";
                return;
            }

            _phien.DangNhap(nd);
            _log.LogInformation("{TenDangNhap} ({VaiTro}) đăng nhập Desktop", nd.TenDangNhap, nd.VaiTro);
            DialogResult = DialogResult.OK;
            Close();
        }
        finally
        {
            DatTrangThaiCho(false);
        }
    }

    private void btnThoat_Click(object sender, EventArgs e) => Close();

    /// <summary>Khóa nút trong lúc chờ CSDL trả lời để tránh bấm nhiều lần.</summary>
    private void DatTrangThaiCho(bool dangCho)
    {
        btnDangNhap.Enabled = !dangCho;
        UseWaitCursor = dangCho;
    }
}
