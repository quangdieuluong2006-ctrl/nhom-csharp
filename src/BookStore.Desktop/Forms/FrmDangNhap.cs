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

    // Chống dò mật khẩu: sai liên tiếp SoLanSaiToiDa lần thì tạm khóa nút Đăng nhập trong ThoiGianKhoa
    private const int SoLanSaiToiDa = 5;
    private static readonly TimeSpan ThoiGianKhoa = TimeSpan.FromSeconds(30);
    private int _soLanSai;
    private DateTime _khoaDen = DateTime.MinValue;
    private readonly System.Windows.Forms.Timer _tmrKhoa = new() { Interval = 1000 };

    public FrmDangNhap(ITaiKhoanService taiKhoan, PhienDangNhap phien, ILogger<FrmDangNhap> log)
    {
        InitializeComponent();
        _taiKhoan = taiKhoan;
        _phien = phien;
        _log = log;
        _tmrKhoa.Tick += (_, _) => CapNhatTrangThaiKhoa();
        Disposed += (_, _) => _tmrKhoa.Dispose();
    }

    private bool DangBiKhoa => DateTime.Now < _khoaDen;

    private async void btnDangNhap_Click(object sender, EventArgs e)
    {
        if (DangBiKhoa) return;
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
                _soLanSai++;
                _log.LogWarning("Đăng nhập Desktop thất bại: {TenDangNhap} (lần {Lan})", ten, _soLanSai);
                if (_soLanSai >= SoLanSaiToiDa)
                {
                    _soLanSai = 0;
                    _khoaDen = DateTime.Now + ThoiGianKhoa;
                    _log.LogWarning("Tạm khóa đăng nhập Desktop {Giay} giây sau {Lan} lần sai liên tiếp", ThoiGianKhoa.TotalSeconds, SoLanSaiToiDa);
                    CapNhatTrangThaiKhoa();
                    _tmrKhoa.Start();
                    return;
                }
                lblLoi.Text = $"Tên đăng nhập hoặc mật khẩu không đúng (còn {SoLanSaiToiDa - _soLanSai} lần thử).";
                txtMatKhau.SelectAll();
                txtMatKhau.Focus();
                return;
            }
            if (nd.VaiTro == VaiTro.KhachHang)
            {
                lblLoi.Text = "Tài khoản khách hàng chỉ dùng trên website.";
                return;
            }

            _soLanSai = 0;
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
        btnDangNhap.Enabled = !dangCho && !DangBiKhoa;
        UseWaitCursor = dangCho;
    }

    /// <summary>Hiện thời gian còn bị khóa; hết thời gian thì mở lại nút Đăng nhập.</summary>
    private void CapNhatTrangThaiKhoa()
    {
        if (DangBiKhoa)
        {
            int giay = (int)Math.Ceiling((_khoaDen - DateTime.Now).TotalSeconds);
            lblLoi.Text = $"Sai mật khẩu {SoLanSaiToiDa} lần liên tiếp. Vui lòng thử lại sau {giay} giây.";
            btnDangNhap.Enabled = false;
            return;
        }
        _tmrKhoa.Stop();
        lblLoi.Text = "";
        btnDangNhap.Enabled = true;
        txtMatKhau.Clear();
        txtMatKhau.Focus();
    }
}
