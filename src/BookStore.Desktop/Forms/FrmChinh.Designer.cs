namespace BookStore.Desktop.Forms;

partial class FrmChinh
{
    /// <summary>Required designer variable.</summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>Clean up any resources being used.</summary>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        menuStrip = new MenuStrip();
        mnuHeThong = new ToolStripMenuItem();
        mnuTongQuan = new ToolStripMenuItem();
        mnuDoiMatKhau = new ToolStripMenuItem();
        sep1 = new ToolStripSeparator();
        mnuDangXuat = new ToolStripMenuItem();
        mnuThoat = new ToolStripMenuItem();
        mnuDanhMuc = new ToolStripMenuItem();
        mnuSach = new ToolStripMenuItem();
        sep2 = new ToolStripSeparator();
        mnuDanhMucSach = new ToolStripMenuItem();
        mnuNhaXuatBan = new ToolStripMenuItem();
        mnuTacGia = new ToolStripMenuItem();
        mnuBanHang = new ToolStripMenuItem();
        mnuDonHang = new ToolStripMenuItem();
        mnuKhachHang = new ToolStripMenuItem();
        mnuKho = new ToolStripMenuItem();
        mnuPhieuKho = new ToolStripMenuItem();
        mnuTonKho = new ToolStripMenuItem();
        mnuBaoCao = new ToolStripMenuItem();
        mnuDoanhThu = new ToolStripMenuItem();
        mnuQuanTri = new ToolStripMenuItem();
        mnuTaiKhoan = new ToolStripMenuItem();
        mnuCuaSo = new ToolStripMenuItem();
        mnuXepCuaSo = new ToolStripMenuItem();
        mnuDongTatCa = new ToolStripMenuItem();
        statusStrip = new StatusStrip();
        lblNguoiDung = new ToolStripStatusLabel();
        lblVaiTro = new ToolStripStatusLabel();
        lblDonCho = new ToolStripStatusLabel();
        tmrKiemTraDon = new System.Windows.Forms.Timer(components);
        menuStrip.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();
        //
        // menuStrip
        //
        menuStrip.ImageScalingSize = new Size(20, 20);
        menuStrip.Items.AddRange(new ToolStripItem[] { mnuHeThong, mnuDanhMuc, mnuBanHang, mnuKho, mnuBaoCao, mnuQuanTri, mnuCuaSo });
        menuStrip.Location = new Point(0, 0);
        menuStrip.MdiWindowListItem = mnuCuaSo;
        menuStrip.Name = "menuStrip";
        menuStrip.Size = new Size(1200, 28);
        menuStrip.TabIndex = 0;
        //
        // mnuHeThong
        //
        mnuHeThong.DropDownItems.AddRange(new ToolStripItem[] { mnuTongQuan, mnuDoiMatKhau, sep1, mnuDangXuat, mnuThoat });
        mnuHeThong.Name = "mnuHeThong";
        mnuHeThong.Text = "&Hệ thống";
        //
        // mnuTongQuan
        //
        mnuTongQuan.Name = "mnuTongQuan";
        mnuTongQuan.ShortcutKeys = Keys.F1;
        mnuTongQuan.Text = "Tổng quan";
        mnuTongQuan.Click += mnuTongQuan_Click;
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
        mnuThoat.ShortcutKeys = Keys.Alt | Keys.F4;
        mnuThoat.Text = "Thoát";
        mnuThoat.Click += mnuThoat_Click;
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
        mnuSach.ShortcutKeys = Keys.F2;
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
        // mnuBanHang
        //
        mnuBanHang.DropDownItems.AddRange(new ToolStripItem[] { mnuDonHang, mnuKhachHang });
        mnuBanHang.Name = "mnuBanHang";
        mnuBanHang.Text = "&Bán hàng";
        //
        // mnuDonHang
        //
        mnuDonHang.Name = "mnuDonHang";
        mnuDonHang.ShortcutKeys = Keys.F3;
        mnuDonHang.Text = "Xử lý đơn hàng";
        mnuDonHang.Click += mnuDonHang_Click;
        //
        // mnuKhachHang
        //
        mnuKhachHang.Name = "mnuKhachHang";
        mnuKhachHang.Text = "Khách hàng";
        mnuKhachHang.Click += mnuKhachHang_Click;
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
        mnuPhieuKho.ShortcutKeys = Keys.F4;
        mnuPhieuKho.Text = "Lập phiếu kho";
        mnuPhieuKho.Click += mnuPhieuKho_Click;
        //
        // mnuTonKho
        //
        mnuTonKho.Name = "mnuTonKho";
        mnuTonKho.Text = "Tồn kho và lịch sử phiếu";
        mnuTonKho.Click += mnuTonKho_Click;
        //
        // mnuBaoCao
        //
        mnuBaoCao.DropDownItems.AddRange(new ToolStripItem[] { mnuDoanhThu });
        mnuBaoCao.Name = "mnuBaoCao";
        mnuBaoCao.Text = "Báo &cáo";
        //
        // mnuDoanhThu
        //
        mnuDoanhThu.Name = "mnuDoanhThu";
        mnuDoanhThu.ShortcutKeys = Keys.F5;
        mnuDoanhThu.Text = "Doanh thu và sách bán chạy";
        mnuDoanhThu.Click += mnuDoanhThu_Click;
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
        mnuTaiKhoan.Text = "Tài khoản và phân quyền";
        mnuTaiKhoan.Click += mnuTaiKhoan_Click;
        //
        // mnuCuaSo
        //
        mnuCuaSo.DropDownItems.AddRange(new ToolStripItem[] { mnuXepCuaSo, mnuDongTatCa });
        mnuCuaSo.Name = "mnuCuaSo";
        mnuCuaSo.Text = "Cửa &sổ";
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
        statusStrip.Items.AddRange(new ToolStripItem[] { lblNguoiDung, lblVaiTro, lblDonCho });
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
        lblVaiTro.BorderSides = ToolStripStatusLabelBorderSides.Left;
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.Size = new Size(64, 20);
        lblVaiTro.Text = "Vai trò:";
        //
        // lblDonCho
        //
        lblDonCho.BorderSides = ToolStripStatusLabelBorderSides.Left;
        lblDonCho.IsLink = true;
        lblDonCho.Name = "lblDonCho";
        lblDonCho.Size = new Size(160, 20);
        lblDonCho.Spring = true;
        lblDonCho.TextAlign = ContentAlignment.MiddleRight;
        lblDonCho.Text = "";
        lblDonCho.Click += lblDonCho_Click;
        //
        // tmrKiemTraDon
        //
        tmrKiemTraDon.Interval = 30000;
        tmrKiemTraDon.Tick += tmrKiemTraDon_Tick;
        //
        // FrmChinh
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1200, 700);
        Controls.Add(statusStrip);
        Controls.Add(menuStrip);
        Font = new Font("Segoe UI", 9F);
        IsMdiContainer = true;
        MainMenuStrip = menuStrip;
        MinimumSize = new Size(1000, 650);
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
    private ToolStripMenuItem mnuTongQuan;
    private ToolStripMenuItem mnuDoiMatKhau;
    private ToolStripSeparator sep1;
    private ToolStripMenuItem mnuDangXuat;
    private ToolStripMenuItem mnuThoat;
    private ToolStripMenuItem mnuDanhMuc;
    private ToolStripMenuItem mnuSach;
    private ToolStripSeparator sep2;
    private ToolStripMenuItem mnuDanhMucSach;
    private ToolStripMenuItem mnuNhaXuatBan;
    private ToolStripMenuItem mnuTacGia;
    private ToolStripMenuItem mnuBanHang;
    private ToolStripMenuItem mnuDonHang;
    private ToolStripMenuItem mnuKhachHang;
    private ToolStripMenuItem mnuKho;
    private ToolStripMenuItem mnuPhieuKho;
    private ToolStripMenuItem mnuTonKho;
    private ToolStripMenuItem mnuBaoCao;
    private ToolStripMenuItem mnuDoanhThu;
    private ToolStripMenuItem mnuQuanTri;
    private ToolStripMenuItem mnuTaiKhoan;
    private ToolStripMenuItem mnuCuaSo;
    private ToolStripMenuItem mnuXepCuaSo;
    private ToolStripMenuItem mnuDongTatCa;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel lblNguoiDung;
    private ToolStripStatusLabel lblVaiTro;
    private ToolStripStatusLabel lblDonCho;
    private System.Windows.Forms.Timer tmrKiemTraDon;
}
