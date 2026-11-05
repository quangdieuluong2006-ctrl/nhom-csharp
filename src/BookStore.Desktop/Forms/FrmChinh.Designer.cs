namespace BookStore.Desktop.Forms;

partial class FrmChinh
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
        menuStrip = new MenuStrip();
        mnuHeThong = new ToolStripMenuItem();
        mnuDoiMatKhau = new ToolStripMenuItem();
        sep1 = new ToolStripSeparator();
        mnuDangXuat = new ToolStripMenuItem();
        mnuThoat = new ToolStripMenuItem();
        mnuQuanTri = new ToolStripMenuItem();
        mnuTaiKhoan = new ToolStripMenuItem();
        mnuDanhMuc = new ToolStripMenuItem();
        mnuSach = new ToolStripMenuItem();
        sep2 = new ToolStripSeparator();
        mnuDanhMucSach = new ToolStripMenuItem();
        mnuNhaXuatBan = new ToolStripMenuItem();
        mnuTacGia = new ToolStripMenuItem();
        mnuKho = new ToolStripMenuItem();
        mnuPhieuKho = new ToolStripMenuItem();
        mnuTonKho = new ToolStripMenuItem();
        mnuDonHang = new ToolStripMenuItem();
        mnuXuLyDon = new ToolStripMenuItem();
        mnuBaoCao = new ToolStripMenuItem();
        mnuDoanhThu = new ToolStripMenuItem();
        mnuSachBanChay = new ToolStripMenuItem();
        mnuCuaSo = new ToolStripMenuItem();
        mnuXepCuaSo = new ToolStripMenuItem();
        mnuDongTatCa = new ToolStripMenuItem();
        statusStrip = new StatusStrip();
        lblNguoiDung = new ToolStripStatusLabel();
        lblVaiTro = new ToolStripStatusLabel();
        lblChao = new Label();
        menuStrip.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // menuStrip
        //
        menuStrip.ImageScalingSize = new Size(20, 20);
        menuStrip.Items.AddRange(new ToolStripItem[] { mnuHeThong, mnuQuanTri, mnuDanhMuc, mnuKho, mnuDonHang, mnuBaoCao, mnuCuaSo });
        menuStrip.Location = new Point(0, 0);
        menuStrip.MdiWindowListItem = mnuCuaSo;
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1200, 28);
        menuStrip.TabIndex = 0;
        //
        // mnuHeThong
        //
        mnuHeThong.DropDownItems.AddRange(new ToolStripItem[] { mnuDoiMatKhau, sep1, mnuDangXuat, mnuThoat });
        mnuHeThong.Name = "mnuHeThong";
        mnuHeThong.Text = "&Hệ thống";
        //
        // mnuDoiMatKhau
        //
        mnuDoiMatKhau.Name = "mnuDoiMatKhau";
        mnuDoiMatKhau.Text = "Đổi mật khẩu";
        mnuDoiMatKhau.Click += mnuDoiMatKhau_Click;
        //
        // sep1
        //
        sep1.Name = "sep1";
        //
        // mnuDangXuat
        //
        mnuDangXuat.Name = "mnuDangXuat";
        mnuDangXuat.Text = "Đăng xuất";
        mnuDangXuat.Click += mnuDangXuat_Click;
        //
        // mnuThoat
        //
        mnuThoat.Name = "mnuThoat";
        mnuThoat.Text = "Thoát";
        mnuThoat.Click += mnuThoat_Click;
        //
        // mnuQuanTri
        //
        mnuQuanTri.DropDownItems.AddRange(new ToolStripItem[] { mnuTaiKhoan });
        mnuQuanTri.Name = "mnuQuanTri";
        mnuQuanTri.Text = "&Quản trị";
        //
        // mnuTaiKhoan
        //
        mnuTaiKhoan.Name = "mnuTaiKhoan";
        mnuTaiKhoan.Text = "Tài khoản nhân viên";
        mnuTaiKhoan.Click += mnuTaiKhoan_Click;
        //
        // mnuDanhMuc
        //
        mnuDanhMuc.DropDownItems.AddRange(new ToolStripItem[] { mnuSach, sep2, mnuDanhMucSach, mnuNhaXuatBan, mnuTacGia });
        mnuDanhMuc.Name = "mnuDanhMuc";
        mnuDanhMuc.Text = "&Danh mục";
        //
        // mnuSach
        //
        mnuSach.Name = "mnuSach";
        mnuSach.Text = "Sách";
        mnuSach.Click += mnuSach_Click;
        //
        // sep2
        //
        sep2.Name = "sep2";
        //
        // mnuDanhMucSach
        //
        mnuDanhMucSach.Name = "mnuDanhMucSach";
        mnuDanhMucSach.Text = "Danh mục sách";
        mnuDanhMucSach.Click += mnuDanhMucSach_Click;
        //
        // mnuNhaXuatBan
        //
        mnuNhaXuatBan.Name = "mnuNhaXuatBan";
        mnuNhaXuatBan.Text = "Nhà xuất bản";
        mnuNhaXuatBan.Click += mnuNhaXuatBan_Click;
        //
        // mnuTacGia
        //
        mnuTacGia.Name = "mnuTacGia";
        mnuTacGia.Text = "Tác giả";
        mnuTacGia.Click += mnuTacGia_Click;
        //
        // mnuKho
        //
        mnuKho.DropDownItems.AddRange(new ToolStripItem[] { mnuPhieuKho, mnuTonKho });
        mnuKho.Name = "mnuKho";
        mnuKho.Text = "&Kho";
        //
        // mnuPhieuKho
        //
        mnuPhieuKho.Name = "mnuPhieuKho";
        mnuPhieuKho.Text = "Lập phiếu kho";
        mnuPhieuKho.Click += mnuPhieuKho_Click;
        //
        // mnuTonKho
        //
        mnuTonKho.Name = "mnuTonKho";
        mnuTonKho.Text = "Tồn kho";
        mnuTonKho.Click += mnuTonKho_Click;
        //
        // mnuDonHang
        //
        mnuDonHang.DropDownItems.AddRange(new ToolStripItem[] { mnuXuLyDon });
        mnuDonHang.Name = "mnuDonHang";
        mnuDonHang.Text = "Đơn &hàng";
        //
        // mnuXuLyDon
        //
        mnuXuLyDon.Name = "mnuXuLyDon";
        mnuXuLyDon.Text = "Xử lý đơn hàng";
        mnuXuLyDon.Click += mnuXuLyDon_Click;
        //
        // mnuBaoCao
        //
        mnuBaoCao.DropDownItems.AddRange(new ToolStripItem[] { mnuDoanhThu, mnuSachBanChay });
        mnuBaoCao.Name = "mnuBaoCao";
        mnuBaoCao.Text = "&Báo cáo";
        //
        // mnuDoanhThu
        //
        mnuDoanhThu.Name = "mnuDoanhThu";
        mnuDoanhThu.Text = "Doanh thu";
        mnuDoanhThu.Click += mnuDoanhThu_Click;
        //
        // mnuSachBanChay
        //
        mnuSachBanChay.Name = "mnuSachBanChay";
        mnuSachBanChay.Text = "Sách bán chạy";
        mnuSachBanChay.Click += mnuSachBanChay_Click;
        //
        // mnuCuaSo
        //
        mnuCuaSo.DropDownItems.AddRange(new ToolStripItem[] { mnuXepCuaSo, mnuDongTatCa });
        mnuCuaSo.Name = "mnuCuaSo";
        mnuCuaSo.Text = "&Cửa sổ";
        //
        // mnuXepCuaSo
        //
        mnuXepCuaSo.Name = "mnuXepCuaSo";
        mnuXepCuaSo.Text = "Xếp chồng cửa sổ";
        mnuXepCuaSo.Click += mnuXepCuaSo_Click;
        //
        // mnuDongTatCa
        //
        mnuDongTatCa.Name = "mnuDongTatCa";
        mnuDongTatCa.Text = "Đóng tất cả";
        mnuDongTatCa.Click += mnuDongTatCa_Click;
        //
        // statusStrip
        //
        statusStrip.ImageScalingSize = new Size(20, 20);
        statusStrip.Items.AddRange(new ToolStripItem[] { lblNguoiDung, lblVaiTro });
        statusStrip.Location = new Point(0, 674);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(1200, 26);
        statusStrip.TabIndex = 1;
        //
        // lblNguoiDung
        //
        lblNguoiDung.Name = "lblNguoiDung";
        lblNguoiDung.Size = new Size(90, 20);
        lblNguoiDung.Text = "Người dùng:";
        //
        // lblVaiTro
        //
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.Size = new Size(64, 20);
        lblVaiTro.Text = "Vai trò:";
        //
        // lblChao
        //
        lblChao.Anchor = AnchorStyles.None;
        lblChao.AutoSize = true;
        lblChao.BackColor = Color.Transparent;
        lblChao.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblChao.ForeColor = Color.FromArgb(22, 50, 79);
        lblChao.Location = new Point(460, 320);
        lblChao.Name = "lblChao";
        lblChao.Size = new Size(160, 41);
        lblChao.TabIndex = 2;
        lblChao.Text = "Xin chào!";
        //
        // FrmChinh
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 700);
        Controls.Add(lblChao);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        Font = new Font("Segoe UI", 9F);
        IsMdiContainer = true;
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(900, 600);
        Name = "FrmChinh";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "BookStore – Phần mềm quản trị";
        WindowState = FormWindowState.Maximized;
        Load += FrmChinh_Load;
        menuStrip.ResumeLayout(false);
        menuStrip.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip menuStrip;
    private ToolStripMenuItem mnuHeThong;
    private ToolStripMenuItem mnuDoiMatKhau;
    private ToolStripSeparator sep1;
    private ToolStripMenuItem mnuDangXuat;
    private ToolStripMenuItem mnuThoat;
    private ToolStripMenuItem mnuQuanTri;
    private ToolStripMenuItem mnuTaiKhoan;
    private ToolStripMenuItem mnuDanhMuc;
    private ToolStripMenuItem mnuSach;
    private ToolStripSeparator sep2;
    private ToolStripMenuItem mnuDanhMucSach;
    private ToolStripMenuItem mnuNhaXuatBan;
    private ToolStripMenuItem mnuTacGia;
    private ToolStripMenuItem mnuKho;
    private ToolStripMenuItem mnuPhieuKho;
    private ToolStripMenuItem mnuTonKho;
    private ToolStripMenuItem mnuDonHang;
    private ToolStripMenuItem mnuXuLyDon;
    private ToolStripMenuItem mnuBaoCao;
    private ToolStripMenuItem mnuDoanhThu;
    private ToolStripMenuItem mnuSachBanChay;
    private ToolStripMenuItem mnuCuaSo;
    private ToolStripMenuItem mnuXepCuaSo;
    private ToolStripMenuItem mnuDongTatCa;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblNguoiDung;
    private ToolStripStatusLabel lblVaiTro;
    private Label lblChao;
}
