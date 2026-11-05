namespace BookStore.Desktop.HaTang;

/// <summary>Hộp thoại thông báo dùng thống nhất trong toàn bộ phần mềm quản trị.</summary>
public static class ThongBao
{
    private const string TieuDe = "BookStore – Quản trị";

    public static void ThanhCong(string noiDung) =>
        MessageBox.Show(noiDung, TieuDe, MessageBoxButtons.OK, MessageBoxIcon.Information);

    public static void Loi(string noiDung) =>
        MessageBox.Show(noiDung, TieuDe, MessageBoxButtons.OK, MessageBoxIcon.Error);

    public static void CanhBao(string noiDung) =>
        MessageBox.Show(noiDung, TieuDe, MessageBoxButtons.OK, MessageBoxIcon.Warning);

    public static bool XacNhan(string cauHoi) =>
        MessageBox.Show(cauHoi, TieuDe, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
}
