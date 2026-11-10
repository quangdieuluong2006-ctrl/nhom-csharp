namespace BookStore.Desktop.HaTang;

/// <summary>
/// Chủ đề giao diện dùng chung cho mọi form (đồng bộ màu với website: xanh đậm + vàng).
/// FormFactory gọi ApDung(form) ngay sau khi tạo form, nên các form không phải tự định dạng.
/// Muốn đổi màu/kiểu cho toàn bộ phần mềm chỉ cần sửa file này.
/// </summary>
public static class ChuDe
{
    public static readonly Color XanhDam = Color.FromArgb(22, 50, 79);        // màu chủ đạo
    public static readonly Color XanhRatDam = Color.FromArgb(14, 33, 54);     // thanh điều hướng
    public static readonly Color XanhNhat = Color.FromArgb(36, 80, 122);      // di chuột trên nền xanh
    public static readonly Color Vang = Color.FromArgb(242, 184, 75);         // nút chính
    public static readonly Color VangDam = Color.FromArgb(217, 154, 34);
    public static readonly Color Do = Color.FromArgb(192, 57, 43);            // nút nguy hiểm
    public static readonly Color NenTrang = Color.White;
    public static readonly Color NenXam = Color.FromArgb(244, 246, 248);      // nền vùng làm việc
    public static readonly Color VienXam = Color.FromArgb(214, 219, 225);
    public static readonly Color ChuPhu = Color.FromArgb(100, 110, 122);

    // Giữ cỡ chữ 9 như Designer: đổi Font của cả form lúc chạy sẽ khiến WinForms tự phóng to toàn bộ bố cục.
    public static readonly Font ChuThuong = new("Segoe UI", 9F);
    public static readonly Font ChuDamNho = new("Segoe UI Semibold", 9F);
    public static readonly Font ChuTieuDe = new("Segoe UI Semibold", 14F);

    // Phân loại nút theo tên (đặt tên control theo quy ước btnXxx)
    private static readonly HashSet<string> NutChinh = new()
    {
        "btnLuu", "btnTim", "btnXem", "btnLuuPhieu", "btnThemDong", "btnChuyenTiep", "btnDangNhap", "btnDoi",
        "btnXemTonKho", "btnXemLichSu", "btnXacNhanTien"
    };
    private static readonly HashSet<string> NutNguyHiem = new() { "btnXoa", "btnHuyDon", "btnKhoa", "btnXoaDong" };

    /// <summary>Áp dụng chủ đề cho form và toàn bộ control con.</summary>
    public static void ApDung(Form form)
    {
        if (form.IsMdiContainer) return;   // form chính tự dựng giao diện riêng (FrmChinh)
        form.BackColor = NenTrang;
        DuyetControl(form);
    }

    private static void DuyetControl(Control cha)
    {
        foreach (Control c in cha.Controls)
        {
            switch (c)
            {
                case Button b:
                    KieuNut(b);
                    break;
                case DataGridView g:
                    KieuLuoi(g);
                    continue;                       // không duyệt vào bên trong lưới
                case GroupBox gb:
                    // Màu/font của GroupBox được control con kế thừa: giữ lại màu/font cũ cho các con đang kế thừa
                    // (control đã có màu riêng như dòng "Tổng tiền" màu đỏ thì không bị đổi)
                    var mauCu = gb.ForeColor;
                    var fontCu = gb.Font;
                    foreach (Control con in gb.Controls)
                    {
                        if (con.ForeColor == mauCu) con.ForeColor = mauCu;
                        if (con.Font.Equals(fontCu)) con.Font = fontCu;
                    }
                    gb.ForeColor = XanhDam;
                    gb.Font = ChuDamNho;
                    break;
                case Panel p when p.Name == "pnlTieuDe":
                    KieuTieuDe(p);
                    break;
                case Panel p when p.Name is "pnlTimKiem" or "pnlNhapDong" or "pnlDuoi":
                    p.BackColor = NenXam;
                    break;
                case TabControl t:
                    t.Font = ChuDamNho;
                    t.Padding = new Point(14, 6);
                    break;
                case TextBox t when !t.Multiline:
                    t.BorderStyle = BorderStyle.FixedSingle;
                    break;
            }
            if (c.HasChildren) DuyetControl(c);
        }
    }

    /// <summary>
    /// Thanh tiêu đề nền xanh: tự giãn chiều cao theo cỡ chữ để không bị cắt chữ khi Windows phóng to 125–150%.
    /// </summary>
    private static void KieuTieuDe(Panel p)
    {
        p.BackColor = XanhDam;
        // Form có bố cục tự đặt vị trí (form Đăng nhập) thì chỉ đổi màu, không đổi kích thước
        if (p.Controls.OfType<Label>().Any(l => l.Dock == DockStyle.None)) return;
        p.AutoSize = true;
        p.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        p.Padding = new Padding(20, 10, 20, 10);
        foreach (var l in p.Controls.OfType<Label>())
        {
            l.Font = ChuTieuDe;
            l.ForeColor = Color.White;
            l.AutoSize = true;
            l.Dock = DockStyle.Top;
            l.Padding = new Padding(0, 2, 0, 2);
            l.MinimumSize = new Size(0, 34);
            l.TextAlign = ContentAlignment.MiddleLeft;
        }
        // Dải vàng mảnh dưới tiêu đề
        if (p.Parent != null && p.Parent.Controls.Find("pnlVachVang", false).Length == 0)
        {
            var vach = new Panel { Name = "pnlVachVang", Height = 3, Dock = DockStyle.Top, BackColor = Vang };
            p.Parent.Controls.Add(vach);
            // Control ở cuối danh sách Controls được neo (Dock) trước: đưa vạch rồi tiêu đề xuống cuối
            // để tiêu đề nằm trên cùng, vạch vàng ngay bên dưới, các panel khác giữ thứ tự cũ.
            vach.SendToBack();
            p.SendToBack();
        }
    }

    public static void KieuNut(Button b)
    {
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 1;
        b.Cursor = Cursors.Hand;
        b.UseVisualStyleBackColor = false;
        b.Font = ChuDamNho;
        if (NutChinh.Contains(b.Name))
        {
            b.BackColor = Vang;
            b.ForeColor = Color.FromArgb(31, 41, 51);
            b.FlatAppearance.BorderColor = VangDam;
            b.FlatAppearance.MouseOverBackColor = VangDam;
            b.FlatAppearance.MouseDownBackColor = VangDam;
        }
        else if (NutNguyHiem.Contains(b.Name))
        {
            b.BackColor = NenTrang;
            b.ForeColor = Do;
            b.FlatAppearance.BorderColor = Do;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(253, 236, 234);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(250, 219, 215);
        }
        else
        {
            b.BackColor = NenTrang;
            b.ForeColor = XanhDam;
            b.FlatAppearance.BorderColor = VienXam;
            b.FlatAppearance.MouseOverBackColor = Color.FromArgb(234, 240, 246);
            b.FlatAppearance.MouseDownBackColor = Color.FromArgb(220, 230, 240);
        }
    }

    /// <summary>Lưới thoáng: chỉ kẻ ngang, dòng cao hơn, chọn dòng màu vàng nhạt.</summary>
    private static void KieuLuoi(DataGridView g)
    {
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeight = 38;
        g.RowTemplate.Height = 32;
        g.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        g.DefaultCellStyle.Font = ChuThuong;
        g.GridColor = Color.FromArgb(232, 235, 239);
        g.BackgroundColor = NenTrang;
    }

    // ------------------------------- Menu, thanh trạng thái -------------------------------

    /// <summary>Bảng màu cho MenuStrip: nền xanh đậm, chữ trắng; menu thả xuống nền trắng.</summary>
    public sealed class MauMenu : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => XanhDam;
        public override Color MenuStripGradientEnd => XanhDam;
        public override Color MenuItemSelected => Color.FromArgb(234, 240, 246);
        public override Color MenuItemSelectedGradientBegin => XanhNhat;
        public override Color MenuItemSelectedGradientEnd => XanhNhat;
        public override Color MenuItemPressedGradientBegin => XanhRatDam;
        public override Color MenuItemPressedGradientEnd => XanhRatDam;
        public override Color MenuItemBorder => Color.Transparent;
        public override Color MenuBorder => VienXam;
        public override Color ToolStripDropDownBackground => NenTrang;
        public override Color ImageMarginGradientBegin => NenTrang;
        public override Color ImageMarginGradientMiddle => NenTrang;
        public override Color ImageMarginGradientEnd => NenTrang;
        public override Color SeparatorDark => VienXam;
        public override Color StatusStripGradientBegin => NenXam;
        public override Color StatusStripGradientEnd => NenXam;
    }

    public static void KieuMenu(MenuStrip menu, StatusStrip? trangThai)
    {
        var renderer = new ToolStripProfessionalRenderer(new MauMenu()) { RoundedEdges = false };
        menu.Renderer = renderer;
        menu.Font = ChuThuong;
        menu.Padding = new Padding(8, 4, 0, 4);
        foreach (ToolStripItem muc in menu.Items)
        {
            muc.ForeColor = Color.White;
            muc.Padding = new Padding(6, 2, 6, 2);
            if (muc is ToolStripMenuItem m)
                foreach (ToolStripItem con in m.DropDownItems)
                {
                    con.ForeColor = Color.FromArgb(31, 41, 51);
                    con.Padding = new Padding(0, 3, 0, 3);
                }
        }
        if (trangThai != null)
        {
            trangThai.Renderer = renderer;
            trangThai.Font = ChuThuong;
        }
    }
}
