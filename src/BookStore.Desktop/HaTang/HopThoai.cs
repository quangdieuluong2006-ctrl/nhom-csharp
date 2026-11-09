namespace BookStore.Desktop.HaTang;

/// <summary>Hộp thoại nhập một chuỗi (ví dụ mật khẩu mới, lý do hủy đơn) – dựng bằng code vì rất đơn giản.</summary>
public static class HopThoai
{
    public static string? NhapChuoi(IWin32Window chuSoHuu, string tieuDe, string loiNhac, bool laMatKhau = false, int doDaiToiDa = 200)
    {
        using var f = new Form
        {
            Text = tieuDe, FormBorderStyle = FormBorderStyle.FixedDialog, StartPosition = FormStartPosition.CenterParent,
            MinimizeBox = false, MaximizeBox = false, ClientSize = new Size(420, 150), Font = new Font("Segoe UI", 9F),
            ShowInTaskbar = false
        };
        var lbl = new Label { Text = loiNhac, Location = new Point(16, 16), AutoSize = true };
        var txt = new TextBox
        {
            Location = new Point(16, 44), Width = 388, UseSystemPasswordChar = laMatKhau, MaxLength = doDaiToiDa
        };
        var ok = new Button { Text = "Đồng ý", DialogResult = DialogResult.OK, Location = new Point(212, 96), Size = new Size(92, 34) };
        var huy = new Button { Text = "Bỏ qua", DialogResult = DialogResult.Cancel, Location = new Point(312, 96), Size = new Size(92, 34) };
        f.Controls.AddRange(new Control[] { lbl, txt, ok, huy });
        f.AcceptButton = ok;
        f.CancelButton = huy;
        return f.ShowDialog(chuSoHuu) == DialogResult.OK ? txt.Text : null;
    }
}
