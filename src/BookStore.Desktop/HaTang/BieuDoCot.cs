using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Biểu đồ cột đơn giản tự vẽ bằng GDI+ (.NET 8 không còn control Chart của .NET Framework).
/// Gán dữ liệu bằng HienThi(nhãn, giá trị).
/// </summary>
public class BieuDoCot : Control
{
    private IReadOnlyList<(string Nhan, decimal GiaTri)> _duLieu = Array.Empty<(string, decimal)>();

    [Category("Appearance"), DefaultValue("")]
    public string TieuDe { get; set; } = "";

    public BieuDoCot()
    {
        DoubleBuffered = true;
        ResizeRedraw = true;
        BackColor = Color.White;
    }

    public void HienThi(IEnumerable<(string Nhan, decimal GiaTri)> duLieu)
    {
        _duLieu = duLieu.ToList();
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        using var chu = new Font("Segoe UI", 8.5F);
        using var chuDam = new Font("Segoe UI", 11F, FontStyle.Bold);
        g.DrawString(TieuDe, chuDam, Brushes.Black, 12, 8);

        if (_duLieu.Count == 0 || _duLieu.All(d => d.GiaTri == 0))
        {
            g.DrawString("Không có dữ liệu trong khoảng thời gian đã chọn.", chu, Brushes.Gray, 12, 40);
            return;
        }

        var vung = new Rectangle(80, 44, Width - 100, Height - 90);
        if (vung.Width < 50 || vung.Height < 50) return;
        decimal max = _duLieu.Max(d => d.GiaTri);
        if (max <= 0) max = 1;

        // Trục và 4 vạch chia
        using var butLuoi = new Pen(Color.FromArgb(230, 233, 237));
        for (int i = 0; i <= 4; i++)
        {
            int y = vung.Bottom - vung.Height * i / 4;
            g.DrawLine(butLuoi, vung.Left, y, vung.Right, y);
            var nhan = (max * i / 4).ToString("N0");
            var kt = g.MeasureString(nhan, chu);
            g.DrawString(nhan, chu, Brushes.DimGray, vung.Left - kt.Width - 4, y - kt.Height / 2);
        }

        float rongO = (float)vung.Width / _duLieu.Count;
        float rongCot = Math.Max(2, rongO * 0.6f);
        int buocNhan = Math.Max(1, (int)Math.Ceiling(_duLieu.Count / (vung.Width / 60f)));
        using var co = new SolidBrush(GiaoDien.MauChuDao);
        for (int i = 0; i < _duLieu.Count; i++)
        {
            var (nhan, giaTri) = _duLieu[i];
            float cao = (float)(giaTri / max) * vung.Height;
            float x = vung.Left + i * rongO + (rongO - rongCot) / 2;
            g.FillRectangle(co, x, vung.Bottom - cao, rongCot, cao);
            if (i % buocNhan == 0)
            {
                var kt = g.MeasureString(nhan, chu);
                g.DrawString(nhan, chu, Brushes.DimGray, x + rongCot / 2 - kt.Width / 2, vung.Bottom + 4);
            }
        }
    }
}
