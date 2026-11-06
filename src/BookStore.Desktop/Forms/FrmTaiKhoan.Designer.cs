namespace BookStore.Desktop.Forms;

partial class FrmTaiKhoan
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
        pnlTieuDe = new Panel();
        lblTieuDe = new Label();
        pnlTimKiem = new Panel();
        flowTimKiem = new FlowLayoutPanel();
        lblTim = new Label();
        txtTimKiem = new TextBox();
        lblLocVaiTro = new Label();
        cboLocVaiTro = new ComboBox();
        btnTim = new Button();
        btnLamMoi = new Button();
        pnlThongTin = new Panel();
        grpThongTin = new GroupBox();
        tlpThongTin = new TableLayoutPanel();
        lblTenDangNhap = new Label();
        txtTenDangNhap = new TextBox();
        lblHoTen = new Label();
        txtHoTen = new TextBox();
        lblVaiTro = new Label();
        cboVaiTro = new ComboBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        lblSoDienThoai = new Label();
        txtSoDienThoai = new TextBox();
        lblMatKhau = new Label();
        txtMatKhau = new TextBox();
        lblTrangThaiNhan = new Label();
        lblTrangThai = new Label();
        flowNut = new FlowLayoutPanel();
        btnThemMoi = new Button();
        btnLuu = new Button();
        btnKhoa = new Button();
        btnDatLaiMatKhau = new Button();
        pnlDuoi = new Panel();
        lblTongSo = new Label();
        dgvDanhSach = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvDanhSach).BeginInit();
        pnlTieuDe.SuspendLayout();
        pnlTimKiem.SuspendLayout();
        flowTimKiem.SuspendLayout();
        pnlThongTin.SuspendLayout();
        grpThongTin.SuspendLayout();
        tlpThongTin.SuspendLayout();
        flowNut.SuspendLayout();
        pnlDuoi.SuspendLayout();
        SuspendLayout();
        //
        // pnlTieuDe
        //
        pnlTieuDe.Controls.Add(lblTieuDe);
        pnlTieuDe.BackColor = Color.FromArgb(22, 50, 79);
        pnlTieuDe.Dock = DockStyle.Top;
        pnlTieuDe.Size = new Size(1100, 52);
        pnlTieuDe.Padding = new Padding(16, 0, 16, 0);
        pnlTieuDe.Name = "pnlTieuDe";
        pnlTieuDe.TabIndex = 0;
        //
        // lblTieuDe
        //
        lblTieuDe.Dock = DockStyle.Fill;
        lblTieuDe.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
        lblTieuDe.ForeColor = Color.White;
        lblTieuDe.Text = "Quản lý tài khoản và phân quyền";
        lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.TabIndex = 1;
        //
        // pnlTimKiem
        //
        pnlTimKiem.Controls.Add(flowTimKiem);
        pnlTimKiem.BackColor = Color.FromArgb(244, 246, 248);
        pnlTimKiem.Dock = DockStyle.Top;
        pnlTimKiem.Padding = new Padding(12, 10, 12, 6);
        pnlTimKiem.Size = new Size(1100, 52);
        pnlTimKiem.Name = "pnlTimKiem";
        pnlTimKiem.TabIndex = 2;
        //
        // flowTimKiem
        //
        flowTimKiem.Controls.Add(lblTim);
        flowTimKiem.Controls.Add(txtTimKiem);
        flowTimKiem.Controls.Add(lblLocVaiTro);
        flowTimKiem.Controls.Add(cboLocVaiTro);
        flowTimKiem.Controls.Add(btnTim);
        flowTimKiem.Controls.Add(btnLamMoi);
        flowTimKiem.Dock = DockStyle.Fill;
        flowTimKiem.WrapContents = false;
        flowTimKiem.Name = "flowTimKiem";
        flowTimKiem.TabIndex = 3;
        //
        // lblTim
        //
        lblTim.Anchor = AnchorStyles.Left;
        lblTim.AutoSize = true;
        lblTim.Margin = new Padding(3, 8, 3, 0);
        lblTim.Text = "Tìm kiếm:";
        lblTim.Name = "lblTim";
        lblTim.TabIndex = 4;
        //
        // txtTimKiem
        //
        txtTimKiem.PlaceholderText = "Nhập từ khóa...";
        txtTimKiem.Size = new Size(260, 27);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.TabIndex = 5;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;
        //
        // lblLocVaiTro
        //
        lblLocVaiTro.Anchor = AnchorStyles.Left;
        lblLocVaiTro.AutoSize = true;
        lblLocVaiTro.Margin = new Padding(3, 8, 3, 0);
        lblLocVaiTro.Text = "Vai trò:";
        lblLocVaiTro.Name = "lblLocVaiTro";
        lblLocVaiTro.TabIndex = 6;
        //
        // cboLocVaiTro
        //
        cboLocVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocVaiTro.Size = new Size(190, 28);
        cboLocVaiTro.Name = "cboLocVaiTro";
        cboLocVaiTro.TabIndex = 7;
        cboLocVaiTro.SelectedIndexChanged += btnTim_Click;
        //
        // btnTim
        //
        btnTim.Size = new Size(100, 30);
        btnTim.Margin = new Padding(6, 1, 3, 3);
        btnTim.Text = "Tìm";
        btnTim.BackColor = Color.FromArgb(242, 184, 75);
        btnTim.FlatStyle = FlatStyle.Flat;
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Name = "btnTim";
        btnTim.TabIndex = 8;
        btnTim.Click += btnTim_Click;
        //
        // btnLamMoi
        //
        btnLamMoi.Size = new Size(100, 30);
        btnLamMoi.Margin = new Padding(6, 1, 3, 3);
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = true;
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.TabIndex = 9;
        btnLamMoi.Click += btnLamMoi_Click;
        //
        // pnlThongTin
        //
        pnlThongTin.Controls.Add(grpThongTin);
        pnlThongTin.Dock = DockStyle.Right;
        pnlThongTin.Padding = new Padding(10);
        pnlThongTin.Size = new Size(420, 500);
        pnlThongTin.Name = "pnlThongTin";
        pnlThongTin.TabIndex = 10;
        //
        // grpThongTin
        //
        grpThongTin.Controls.Add(flowNut);
        grpThongTin.Controls.Add(tlpThongTin);
        grpThongTin.Dock = DockStyle.Fill;
        grpThongTin.Padding = new Padding(10);
        grpThongTin.Text = "Thông tin tài khoản";
        grpThongTin.Name = "grpThongTin";
        grpThongTin.TabIndex = 11;
        //
        // tlpThongTin
        //
        tlpThongTin.Controls.Add(lblTenDangNhap, 0, 0);
        tlpThongTin.Controls.Add(txtTenDangNhap, 1, 0);
        tlpThongTin.Controls.Add(lblHoTen, 0, 1);
        tlpThongTin.Controls.Add(txtHoTen, 1, 1);
        tlpThongTin.Controls.Add(lblVaiTro, 0, 2);
        tlpThongTin.Controls.Add(cboVaiTro, 1, 2);
        tlpThongTin.Controls.Add(lblEmail, 0, 3);
        tlpThongTin.Controls.Add(txtEmail, 1, 3);
        tlpThongTin.Controls.Add(lblSoDienThoai, 0, 4);
        tlpThongTin.Controls.Add(txtSoDienThoai, 1, 4);
        tlpThongTin.Controls.Add(lblMatKhau, 0, 5);
        tlpThongTin.Controls.Add(txtMatKhau, 1, 5);
        tlpThongTin.Controls.Add(lblTrangThaiNhan, 0, 6);
        tlpThongTin.Controls.Add(lblTrangThai, 1, 6);
        tlpThongTin.AutoSize = true;
        tlpThongTin.ColumnCount = 2;
        tlpThongTin.Dock = DockStyle.Top;
        tlpThongTin.RowCount = 7;
        tlpThongTin.Padding = new Padding(0, 4, 0, 4);
        tlpThongTin.Name = "tlpThongTin";
        tlpThongTin.TabIndex = 12;
        tlpThongTin.ColumnStyles.Add(new ColumnStyle());
        tlpThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        //
        // lblTenDangNhap
        //
        lblTenDangNhap.Anchor = AnchorStyles.Left;
        lblTenDangNhap.AutoSize = true;
        lblTenDangNhap.Text = "Tên đăng nhập";
        lblTenDangNhap.Name = "lblTenDangNhap";
        lblTenDangNhap.TabIndex = 13;
        //
        // txtTenDangNhap
        //
        txtTenDangNhap.Dock = DockStyle.Fill;
        txtTenDangNhap.MaxLength = 50;
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.TabIndex = 14;
        //
        // lblHoTen
        //
        lblHoTen.Anchor = AnchorStyles.Left;
        lblHoTen.AutoSize = true;
        lblHoTen.Text = "Họ tên";
        lblHoTen.Name = "lblHoTen";
        lblHoTen.TabIndex = 15;
        //
        // txtHoTen
        //
        txtHoTen.Dock = DockStyle.Fill;
        txtHoTen.MaxLength = 100;
        txtHoTen.Name = "txtHoTen";
        txtHoTen.TabIndex = 16;
        //
        // lblVaiTro
        //
        lblVaiTro.Anchor = AnchorStyles.Left;
        lblVaiTro.AutoSize = true;
        lblVaiTro.Text = "Vai trò";
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.TabIndex = 17;
        //
        // cboVaiTro
        //
        cboVaiTro.Dock = DockStyle.Fill;
        cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVaiTro.Name = "cboVaiTro";
        cboVaiTro.TabIndex = 18;
        //
        // lblEmail
        //
        lblEmail.Anchor = AnchorStyles.Left;
        lblEmail.AutoSize = true;
        lblEmail.Text = "Email";
        lblEmail.Name = "lblEmail";
        lblEmail.TabIndex = 19;
        //
        // txtEmail
        //
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.MaxLength = 254;
        txtEmail.Name = "txtEmail";
        txtEmail.TabIndex = 20;
        //
        // lblSoDienThoai
        //
        lblSoDienThoai.Anchor = AnchorStyles.Left;
        lblSoDienThoai.AutoSize = true;
        lblSoDienThoai.Text = "Số điện thoại";
        lblSoDienThoai.Name = "lblSoDienThoai";
        lblSoDienThoai.TabIndex = 21;
        //
        // txtSoDienThoai
        //
        txtSoDienThoai.Dock = DockStyle.Fill;
        txtSoDienThoai.MaxLength = 15;
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.TabIndex = 22;
        //
        // lblMatKhau
        //
        lblMatKhau.Anchor = AnchorStyles.Left;
        lblMatKhau.AutoSize = true;
        lblMatKhau.Text = "Mật khẩu";
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.TabIndex = 23;
        //
        // txtMatKhau
        //
        txtMatKhau.Dock = DockStyle.Fill;
        txtMatKhau.UseSystemPasswordChar = true;
        txtMatKhau.MaxLength = 100;
        txtMatKhau.PlaceholderText = "Chỉ nhập khi tạo mới";
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.TabIndex = 24;
        //
        // lblTrangThaiNhan
        //
        lblTrangThaiNhan.Anchor = AnchorStyles.Left;
        lblTrangThaiNhan.AutoSize = true;
        lblTrangThaiNhan.Text = "Trạng thái";
        lblTrangThaiNhan.Name = "lblTrangThaiNhan";
        lblTrangThaiNhan.TabIndex = 25;
        //
        // lblTrangThai
        //
        lblTrangThai.Dock = DockStyle.Fill;
        lblTrangThai.AutoSize = true;
        lblTrangThai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTrangThai.Name = "lblTrangThai";
        lblTrangThai.TabIndex = 26;
        //
        // flowNut
        //
        flowNut.Controls.Add(btnThemMoi);
        flowNut.Controls.Add(btnLuu);
        flowNut.Controls.Add(btnKhoa);
        flowNut.Controls.Add(btnDatLaiMatKhau);
        flowNut.AutoSize = true;
        flowNut.Dock = DockStyle.Top;
        flowNut.Padding = new Padding(0, 10, 0, 0);
        flowNut.Name = "flowNut";
        flowNut.TabIndex = 27;
        //
        // btnThemMoi
        //
        btnThemMoi.Size = new Size(110, 36);
        btnThemMoi.Text = "Thêm mới";
        btnThemMoi.UseVisualStyleBackColor = true;
        btnThemMoi.Name = "btnThemMoi";
        btnThemMoi.TabIndex = 28;
        btnThemMoi.Click += btnThemMoi_Click;
        //
        // btnLuu
        //
        btnLuu.Size = new Size(110, 36);
        btnLuu.Text = "Lưu";
        btnLuu.BackColor = Color.FromArgb(242, 184, 75);
        btnLuu.FlatStyle = FlatStyle.Flat;
        btnLuu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnLuu.UseVisualStyleBackColor = false;
        btnLuu.Name = "btnLuu";
        btnLuu.TabIndex = 29;
        btnLuu.Click += btnLuu_Click;
        //
        // btnKhoa
        //
        btnKhoa.Size = new Size(110, 36);
        btnKhoa.Text = "Khóa";
        btnKhoa.UseVisualStyleBackColor = true;
        btnKhoa.Name = "btnKhoa";
        btnKhoa.TabIndex = 30;
        btnKhoa.Click += btnKhoa_Click;
        //
        // btnDatLaiMatKhau
        //
        btnDatLaiMatKhau.Size = new Size(150, 36);
        btnDatLaiMatKhau.Text = "Đặt lại mật khẩu";
        btnDatLaiMatKhau.UseVisualStyleBackColor = true;
        btnDatLaiMatKhau.Name = "btnDatLaiMatKhau";
        btnDatLaiMatKhau.TabIndex = 31;
        btnDatLaiMatKhau.Click += btnDatLaiMatKhau_Click;
        //
        // pnlDuoi
        //
        pnlDuoi.Controls.Add(lblTongSo);
        pnlDuoi.BackColor = Color.FromArgb(244, 246, 248);
        pnlDuoi.Dock = DockStyle.Bottom;
        pnlDuoi.Padding = new Padding(12, 0, 12, 0);
        pnlDuoi.Size = new Size(1100, 30);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.TabIndex = 32;
        //
        // lblTongSo
        //
        lblTongSo.Dock = DockStyle.Fill;
        lblTongSo.Text = "";
        lblTongSo.TextAlign = ContentAlignment.MiddleLeft;
        lblTongSo.Name = "lblTongSo";
        lblTongSo.TabIndex = 33;
        //
        // dgvDanhSach
        //
        dgvDanhSach.Dock = DockStyle.Fill;
        dgvDanhSach.Name = "dgvDanhSach";
        dgvDanhSach.TabIndex = 34;
        dgvDanhSach.SelectionChanged += dgvDanhSach_SelectionChanged;
        //
        // FrmTaiKhoan
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1100, 650);
        Controls.Add(dgvDanhSach);
        Controls.Add(pnlDuoi);
        Controls.Add(pnlThongTin);
        Controls.Add(pnlTimKiem);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmTaiKhoan";
        Text = "Tài khoản";
        ((System.ComponentModel.ISupportInitialize)dgvDanhSach).EndInit();
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        flowNut.ResumeLayout(false);
        flowNut.PerformLayout();
        tlpThongTin.ResumeLayout(false);
        tlpThongTin.PerformLayout();
        grpThongTin.ResumeLayout(false);
        grpThongTin.PerformLayout();
        pnlThongTin.ResumeLayout(false);
        pnlThongTin.PerformLayout();
        flowTimKiem.ResumeLayout(false);
        flowTimKiem.PerformLayout();
        pnlTimKiem.ResumeLayout(false);
        pnlTimKiem.PerformLayout();
        pnlTieuDe.ResumeLayout(false);
        pnlTieuDe.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTieuDe;
    private Label lblTieuDe;
    private Panel pnlTimKiem;
    private FlowLayoutPanel flowTimKiem;
    private Label lblTim;
    private TextBox txtTimKiem;
    private Label lblLocVaiTro;
    private ComboBox cboLocVaiTro;
    private Button btnTim;
    private Button btnLamMoi;
    private Panel pnlThongTin;
    private GroupBox grpThongTin;
    private TableLayoutPanel tlpThongTin;
    private Label lblTenDangNhap;
    private TextBox txtTenDangNhap;
    private Label lblHoTen;
    private TextBox txtHoTen;
    private Label lblVaiTro;
    private ComboBox cboVaiTro;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblSoDienThoai;
    private TextBox txtSoDienThoai;
    private Label lblMatKhau;
    private TextBox txtMatKhau;
    private Label lblTrangThaiNhan;
    private Label lblTrangThai;
    private FlowLayoutPanel flowNut;
    private Button btnThemMoi;
    private Button btnLuu;
    private Button btnKhoa;
    private Button btnDatLaiMatKhau;
    private Panel pnlDuoi;
    private Label lblTongSo;
    private DataGridView dgvDanhSach;
}
