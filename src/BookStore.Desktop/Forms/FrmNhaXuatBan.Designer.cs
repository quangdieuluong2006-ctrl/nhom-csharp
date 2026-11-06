namespace BookStore.Desktop.Forms;

partial class FrmNhaXuatBan
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
        btnTim = new Button();
        btnLamMoi = new Button();
        pnlThongTin = new Panel();
        grpThongTin = new GroupBox();
        tlpThongTin = new TableLayoutPanel();
        lblTenNhaXuatBan = new Label();
        txtTenNhaXuatBan = new TextBox();
        lblDiaChi = new Label();
        txtDiaChi = new TextBox();
        lblSoDienThoai = new Label();
        txtSoDienThoai = new TextBox();
        lblEmail = new Label();
        txtEmail = new TextBox();
        flowNut = new FlowLayoutPanel();
        btnThemMoi = new Button();
        btnLuu = new Button();
        btnXoa = new Button();
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
        lblTieuDe.Text = "Quản lý nhà xuất bản";
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
        // btnTim
        //
        btnTim.Size = new Size(100, 30);
        btnTim.Margin = new Padding(6, 1, 3, 3);
        btnTim.Text = "Tìm";
        btnTim.BackColor = Color.FromArgb(242, 184, 75);
        btnTim.FlatStyle = FlatStyle.Flat;
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Name = "btnTim";
        btnTim.TabIndex = 6;
        btnTim.Click += btnTim_Click;
        //
        // btnLamMoi
        //
        btnLamMoi.Size = new Size(100, 30);
        btnLamMoi.Margin = new Padding(6, 1, 3, 3);
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = true;
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.TabIndex = 7;
        btnLamMoi.Click += btnLamMoi_Click;
        //
        // pnlThongTin
        //
        pnlThongTin.Controls.Add(grpThongTin);
        pnlThongTin.Dock = DockStyle.Right;
        pnlThongTin.Padding = new Padding(10);
        pnlThongTin.Size = new Size(400, 500);
        pnlThongTin.Name = "pnlThongTin";
        pnlThongTin.TabIndex = 8;
        //
        // grpThongTin
        //
        grpThongTin.Controls.Add(flowNut);
        grpThongTin.Controls.Add(tlpThongTin);
        grpThongTin.Dock = DockStyle.Fill;
        grpThongTin.Padding = new Padding(10);
        grpThongTin.Text = "Thông tin nhà xuất bản";
        grpThongTin.Name = "grpThongTin";
        grpThongTin.TabIndex = 9;
        //
        // tlpThongTin
        //
        tlpThongTin.Controls.Add(lblTenNhaXuatBan, 0, 0);
        tlpThongTin.Controls.Add(txtTenNhaXuatBan, 1, 0);
        tlpThongTin.Controls.Add(lblDiaChi, 0, 1);
        tlpThongTin.Controls.Add(txtDiaChi, 1, 1);
        tlpThongTin.Controls.Add(lblSoDienThoai, 0, 2);
        tlpThongTin.Controls.Add(txtSoDienThoai, 1, 2);
        tlpThongTin.Controls.Add(lblEmail, 0, 3);
        tlpThongTin.Controls.Add(txtEmail, 1, 3);
        tlpThongTin.AutoSize = true;
        tlpThongTin.ColumnCount = 2;
        tlpThongTin.Dock = DockStyle.Top;
        tlpThongTin.RowCount = 4;
        tlpThongTin.Padding = new Padding(0, 4, 0, 4);
        tlpThongTin.Name = "tlpThongTin";
        tlpThongTin.TabIndex = 10;
        tlpThongTin.ColumnStyles.Add(new ColumnStyle());
        tlpThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        //
        // lblTenNhaXuatBan
        //
        lblTenNhaXuatBan.Anchor = AnchorStyles.Left;
        lblTenNhaXuatBan.AutoSize = true;
        lblTenNhaXuatBan.Text = "Tên NXB";
        lblTenNhaXuatBan.Name = "lblTenNhaXuatBan";
        lblTenNhaXuatBan.TabIndex = 11;
        //
        // txtTenNhaXuatBan
        //
        txtTenNhaXuatBan.Dock = DockStyle.Fill;
        txtTenNhaXuatBan.MaxLength = 150;
        txtTenNhaXuatBan.Name = "txtTenNhaXuatBan";
        txtTenNhaXuatBan.TabIndex = 12;
        //
        // lblDiaChi
        //
        lblDiaChi.Anchor = AnchorStyles.Left;
        lblDiaChi.AutoSize = true;
        lblDiaChi.Text = "Địa chỉ";
        lblDiaChi.Name = "lblDiaChi";
        lblDiaChi.TabIndex = 13;
        //
        // txtDiaChi
        //
        txtDiaChi.Dock = DockStyle.Fill;
        txtDiaChi.MaxLength = 255;
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.TabIndex = 14;
        //
        // lblSoDienThoai
        //
        lblSoDienThoai.Anchor = AnchorStyles.Left;
        lblSoDienThoai.AutoSize = true;
        lblSoDienThoai.Text = "Số điện thoại";
        lblSoDienThoai.Name = "lblSoDienThoai";
        lblSoDienThoai.TabIndex = 15;
        //
        // txtSoDienThoai
        //
        txtSoDienThoai.Dock = DockStyle.Fill;
        txtSoDienThoai.MaxLength = 15;
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.TabIndex = 16;
        //
        // lblEmail
        //
        lblEmail.Anchor = AnchorStyles.Left;
        lblEmail.AutoSize = true;
        lblEmail.Text = "Email";
        lblEmail.Name = "lblEmail";
        lblEmail.TabIndex = 17;
        //
        // txtEmail
        //
        txtEmail.Dock = DockStyle.Fill;
        txtEmail.MaxLength = 254;
        txtEmail.Name = "txtEmail";
        txtEmail.TabIndex = 18;
        //
        // flowNut
        //
        flowNut.Controls.Add(btnThemMoi);
        flowNut.Controls.Add(btnLuu);
        flowNut.Controls.Add(btnXoa);
        flowNut.AutoSize = true;
        flowNut.Dock = DockStyle.Top;
        flowNut.Padding = new Padding(0, 10, 0, 0);
        flowNut.Name = "flowNut";
        flowNut.TabIndex = 19;
        //
        // btnThemMoi
        //
        btnThemMoi.Size = new Size(110, 36);
        btnThemMoi.Text = "Thêm mới";
        btnThemMoi.UseVisualStyleBackColor = true;
        btnThemMoi.Name = "btnThemMoi";
        btnThemMoi.TabIndex = 20;
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
        btnLuu.TabIndex = 21;
        btnLuu.Click += btnLuu_Click;
        //
        // btnXoa
        //
        btnXoa.Size = new Size(110, 36);
        btnXoa.Text = "Xóa";
        btnXoa.UseVisualStyleBackColor = true;
        btnXoa.Name = "btnXoa";
        btnXoa.TabIndex = 22;
        btnXoa.Click += btnXoa_Click;
        //
        // pnlDuoi
        //
        pnlDuoi.Controls.Add(lblTongSo);
        pnlDuoi.BackColor = Color.FromArgb(244, 246, 248);
        pnlDuoi.Dock = DockStyle.Bottom;
        pnlDuoi.Padding = new Padding(12, 0, 12, 0);
        pnlDuoi.Size = new Size(1100, 30);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.TabIndex = 23;
        //
        // lblTongSo
        //
        lblTongSo.Dock = DockStyle.Fill;
        lblTongSo.Text = "";
        lblTongSo.TextAlign = ContentAlignment.MiddleLeft;
        lblTongSo.Name = "lblTongSo";
        lblTongSo.TabIndex = 24;
        //
        // dgvDanhSach
        //
        dgvDanhSach.Dock = DockStyle.Fill;
        dgvDanhSach.Name = "dgvDanhSach";
        dgvDanhSach.TabIndex = 25;
        dgvDanhSach.SelectionChanged += dgvDanhSach_SelectionChanged;
        //
        // FrmNhaXuatBan
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
        Name = "FrmNhaXuatBan";
        Text = "Nhà xuất bản";
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
    private Button btnTim;
    private Button btnLamMoi;
    private Panel pnlThongTin;
    private GroupBox grpThongTin;
    private TableLayoutPanel tlpThongTin;
    private Label lblTenNhaXuatBan;
    private TextBox txtTenNhaXuatBan;
    private Label lblDiaChi;
    private TextBox txtDiaChi;
    private Label lblSoDienThoai;
    private TextBox txtSoDienThoai;
    private Label lblEmail;
    private TextBox txtEmail;
    private FlowLayoutPanel flowNut;
    private Button btnThemMoi;
    private Button btnLuu;
    private Button btnXoa;
    private Panel pnlDuoi;
    private Label lblTongSo;
    private DataGridView dgvDanhSach;
}
