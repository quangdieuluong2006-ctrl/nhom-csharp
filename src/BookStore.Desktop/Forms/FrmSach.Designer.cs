namespace BookStore.Desktop.Forms;

partial class FrmSach
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
        lblLocDanhMuc = new Label();
        cboLocDanhMuc = new ComboBox();
        lblLocTrangThai = new Label();
        cboLocTrangThai = new ComboBox();
        btnTim = new Button();
        btnLamMoi = new Button();
        btnXuatExcel = new Button();
        pnlThongTin = new Panel();
        grpThongTin = new GroupBox();
        tlpThongTin = new TableLayoutPanel();
        lblISBN = new Label();
        txtISBN = new TextBox();
        lblTenSach = new Label();
        txtTenSach = new TextBox();
        lblDanhMuc = new Label();
        cboDanhMuc = new ComboBox();
        lblNhaXuatBan = new Label();
        cboNhaXuatBan = new ComboBox();
        lblNamXuatBan = new Label();
        nudNamXuatBan = new NumericUpDown();
        lblDonGia = new Label();
        nudDonGia = new NumericUpDown();
        lblTonKhoNhan = new Label();
        lblTonKho = new Label();
        lblTacGia = new Label();
        clbTacGia = new CheckedListBox();
        lblMoTa = new Label();
        txtMoTa = new TextBox();
        lblAnhBia = new Label();
        picAnhBia = new PictureBox();
        btnChonAnh = new Button();
        lblDangBan = new Label();
        chkDangBan = new CheckBox();
        flowNut = new FlowLayoutPanel();
        btnThemMoi = new Button();
        btnLuu = new Button();
        btnXoa = new Button();
        pnlDuoi = new Panel();
        lblTongSo = new Label();
        dgvDanhSach = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)nudNamXuatBan).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDonGia).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picAnhBia).BeginInit();
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
        lblTieuDe.Text = "Quản lý sách";
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
        flowTimKiem.Controls.Add(lblLocDanhMuc);
        flowTimKiem.Controls.Add(cboLocDanhMuc);
        flowTimKiem.Controls.Add(lblLocTrangThai);
        flowTimKiem.Controls.Add(cboLocTrangThai);
        flowTimKiem.Controls.Add(btnTim);
        flowTimKiem.Controls.Add(btnLamMoi);
        flowTimKiem.Controls.Add(btnXuatExcel);
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
        // lblLocDanhMuc
        //
        lblLocDanhMuc.Anchor = AnchorStyles.Left;
        lblLocDanhMuc.AutoSize = true;
        lblLocDanhMuc.Margin = new Padding(3, 8, 3, 0);
        lblLocDanhMuc.Text = "Danh mục:";
        lblLocDanhMuc.Name = "lblLocDanhMuc";
        lblLocDanhMuc.TabIndex = 6;
        //
        // cboLocDanhMuc
        //
        cboLocDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocDanhMuc.Size = new Size(220, 28);
        cboLocDanhMuc.Name = "cboLocDanhMuc";
        cboLocDanhMuc.TabIndex = 7;
        cboLocDanhMuc.SelectedIndexChanged += btnTim_Click;
        //
        // lblLocTrangThai
        //
        lblLocTrangThai.Anchor = AnchorStyles.Left;
        lblLocTrangThai.AutoSize = true;
        lblLocTrangThai.Margin = new Padding(3, 8, 3, 0);
        lblLocTrangThai.Text = "Trạng thái:";
        lblLocTrangThai.Name = "lblLocTrangThai";
        lblLocTrangThai.TabIndex = 8;
        //
        // cboLocTrangThai
        //
        cboLocTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocTrangThai.Size = new Size(130, 28);
        cboLocTrangThai.Name = "cboLocTrangThai";
        cboLocTrangThai.TabIndex = 9;
        cboLocTrangThai.SelectedIndexChanged += btnTim_Click;
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
        btnTim.TabIndex = 10;
        btnTim.Click += btnTim_Click;
        //
        // btnLamMoi
        //
        btnLamMoi.Size = new Size(100, 30);
        btnLamMoi.Margin = new Padding(6, 1, 3, 3);
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = true;
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.TabIndex = 11;
        btnLamMoi.Click += btnLamMoi_Click;
        //
        // btnXuatExcel
        //
        btnXuatExcel.Size = new Size(110, 30);
        btnXuatExcel.Margin = new Padding(6, 1, 3, 3);
        btnXuatExcel.Text = "Xuất Excel";
        btnXuatExcel.UseVisualStyleBackColor = true;
        btnXuatExcel.Name = "btnXuatExcel";
        btnXuatExcel.TabIndex = 12;
        btnXuatExcel.Click += btnXuatExcel_Click;
        //
        // pnlThongTin
        //
        pnlThongTin.Controls.Add(grpThongTin);
        pnlThongTin.Dock = DockStyle.Right;
        pnlThongTin.Padding = new Padding(10);
        pnlThongTin.Size = new Size(440, 500);
        pnlThongTin.AutoScroll = true;
        pnlThongTin.Name = "pnlThongTin";
        pnlThongTin.TabIndex = 13;
        //
        // grpThongTin
        //
        grpThongTin.Controls.Add(flowNut);
        grpThongTin.Controls.Add(tlpThongTin);
        grpThongTin.Dock = DockStyle.Fill;
        grpThongTin.Padding = new Padding(10);
        grpThongTin.Text = "Thông tin sách";
        grpThongTin.Name = "grpThongTin";
        grpThongTin.TabIndex = 14;
        //
        // tlpThongTin
        //
        tlpThongTin.Controls.Add(lblISBN, 0, 0);
        tlpThongTin.Controls.Add(txtISBN, 1, 0);
        tlpThongTin.Controls.Add(lblTenSach, 0, 1);
        tlpThongTin.Controls.Add(txtTenSach, 1, 1);
        tlpThongTin.Controls.Add(lblDanhMuc, 0, 2);
        tlpThongTin.Controls.Add(cboDanhMuc, 1, 2);
        tlpThongTin.Controls.Add(lblNhaXuatBan, 0, 3);
        tlpThongTin.Controls.Add(cboNhaXuatBan, 1, 3);
        tlpThongTin.Controls.Add(lblNamXuatBan, 0, 4);
        tlpThongTin.Controls.Add(nudNamXuatBan, 1, 4);
        tlpThongTin.Controls.Add(lblDonGia, 0, 5);
        tlpThongTin.Controls.Add(nudDonGia, 1, 5);
        tlpThongTin.Controls.Add(lblTonKhoNhan, 0, 6);
        tlpThongTin.Controls.Add(lblTonKho, 1, 6);
        tlpThongTin.Controls.Add(lblTacGia, 0, 7);
        tlpThongTin.Controls.Add(clbTacGia, 1, 7);
        tlpThongTin.Controls.Add(lblMoTa, 0, 8);
        tlpThongTin.Controls.Add(txtMoTa, 1, 8);
        tlpThongTin.Controls.Add(lblAnhBia, 0, 9);
        tlpThongTin.Controls.Add(picAnhBia, 1, 9);
        tlpThongTin.Controls.Add(btnChonAnh, 1, 10);
        tlpThongTin.Controls.Add(lblDangBan, 0, 11);
        tlpThongTin.Controls.Add(chkDangBan, 1, 11);
        tlpThongTin.AutoSize = true;
        tlpThongTin.ColumnCount = 2;
        tlpThongTin.Dock = DockStyle.Top;
        tlpThongTin.RowCount = 12;
        tlpThongTin.Padding = new Padding(0, 4, 0, 4);
        tlpThongTin.Name = "tlpThongTin";
        tlpThongTin.TabIndex = 15;
        tlpThongTin.ColumnStyles.Add(new ColumnStyle());
        tlpThongTin.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        tlpThongTin.RowStyles.Add(new RowStyle());
        //
        // lblISBN
        //
        lblISBN.Anchor = AnchorStyles.Left;
        lblISBN.AutoSize = true;
        lblISBN.Text = "ISBN";
        lblISBN.Name = "lblISBN";
        lblISBN.TabIndex = 16;
        //
        // txtISBN
        //
        txtISBN.Dock = DockStyle.Fill;
        txtISBN.MaxLength = 20;
        txtISBN.Name = "txtISBN";
        txtISBN.TabIndex = 17;
        //
        // lblTenSach
        //
        lblTenSach.Anchor = AnchorStyles.Left;
        lblTenSach.AutoSize = true;
        lblTenSach.Text = "Tên sách";
        lblTenSach.Name = "lblTenSach";
        lblTenSach.TabIndex = 18;
        //
        // txtTenSach
        //
        txtTenSach.Dock = DockStyle.Fill;
        txtTenSach.MaxLength = 200;
        txtTenSach.Name = "txtTenSach";
        txtTenSach.TabIndex = 19;
        //
        // lblDanhMuc
        //
        lblDanhMuc.Anchor = AnchorStyles.Left;
        lblDanhMuc.AutoSize = true;
        lblDanhMuc.Text = "Danh mục";
        lblDanhMuc.Name = "lblDanhMuc";
        lblDanhMuc.TabIndex = 20;
        //
        // cboDanhMuc
        //
        cboDanhMuc.Dock = DockStyle.Fill;
        cboDanhMuc.DropDownStyle = ComboBoxStyle.DropDownList;
        cboDanhMuc.Name = "cboDanhMuc";
        cboDanhMuc.TabIndex = 21;
        //
        // lblNhaXuatBan
        //
        lblNhaXuatBan.Anchor = AnchorStyles.Left;
        lblNhaXuatBan.AutoSize = true;
        lblNhaXuatBan.Text = "Nhà xuất bản";
        lblNhaXuatBan.Name = "lblNhaXuatBan";
        lblNhaXuatBan.TabIndex = 22;
        //
        // cboNhaXuatBan
        //
        cboNhaXuatBan.Dock = DockStyle.Fill;
        cboNhaXuatBan.DropDownStyle = ComboBoxStyle.DropDownList;
        cboNhaXuatBan.Name = "cboNhaXuatBan";
        cboNhaXuatBan.TabIndex = 23;
        //
        // lblNamXuatBan
        //
        lblNamXuatBan.Anchor = AnchorStyles.Left;
        lblNamXuatBan.AutoSize = true;
        lblNamXuatBan.Text = "Năm xuất bản";
        lblNamXuatBan.Name = "lblNamXuatBan";
        lblNamXuatBan.TabIndex = 24;
        //
        // nudNamXuatBan
        //
        nudNamXuatBan.Dock = DockStyle.Fill;
        nudNamXuatBan.Maximum = new decimal(new int[] { 2100, 0, 0, 0 });
        nudNamXuatBan.Name = "nudNamXuatBan";
        nudNamXuatBan.TabIndex = 25;
        //
        // lblDonGia
        //
        lblDonGia.Anchor = AnchorStyles.Left;
        lblDonGia.AutoSize = true;
        lblDonGia.Text = "Đơn giá (đ)";
        lblDonGia.Name = "lblDonGia";
        lblDonGia.TabIndex = 26;
        //
        // nudDonGia
        //
        nudDonGia.Dock = DockStyle.Fill;
        nudDonGia.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        nudDonGia.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
        nudDonGia.ThousandsSeparator = true;
        nudDonGia.Name = "nudDonGia";
        nudDonGia.TabIndex = 27;
        //
        // lblTonKhoNhan
        //
        lblTonKhoNhan.Anchor = AnchorStyles.Left;
        lblTonKhoNhan.AutoSize = true;
        lblTonKhoNhan.Text = "Tồn kho";
        lblTonKhoNhan.Name = "lblTonKhoNhan";
        lblTonKhoNhan.TabIndex = 28;
        //
        // lblTonKho
        //
        lblTonKho.Dock = DockStyle.Fill;
        lblTonKho.AutoSize = true;
        lblTonKho.Text = "0 (cập nhật qua phiếu kho)";
        lblTonKho.Name = "lblTonKho";
        lblTonKho.TabIndex = 29;
        //
        // lblTacGia
        //
        lblTacGia.Anchor = AnchorStyles.Left;
        lblTacGia.AutoSize = true;
        lblTacGia.Text = "Tác giả";
        lblTacGia.Name = "lblTacGia";
        lblTacGia.TabIndex = 30;
        //
        // clbTacGia
        //
        clbTacGia.Dock = DockStyle.Fill;
        clbTacGia.CheckOnClick = true;
        clbTacGia.Size = new Size(250, 110);
        clbTacGia.Name = "clbTacGia";
        clbTacGia.TabIndex = 31;
        //
        // lblMoTa
        //
        lblMoTa.Anchor = AnchorStyles.Left;
        lblMoTa.AutoSize = true;
        lblMoTa.Text = "Mô tả";
        lblMoTa.Name = "lblMoTa";
        lblMoTa.TabIndex = 32;
        //
        // txtMoTa
        //
        txtMoTa.Dock = DockStyle.Fill;
        txtMoTa.MaxLength = 4000;
        txtMoTa.Multiline = true;
        txtMoTa.ScrollBars = ScrollBars.Vertical;
        txtMoTa.Size = new Size(250, 80);
        txtMoTa.Name = "txtMoTa";
        txtMoTa.TabIndex = 33;
        //
        // lblAnhBia
        //
        lblAnhBia.Anchor = AnchorStyles.Left;
        lblAnhBia.AutoSize = true;
        lblAnhBia.Text = "Ảnh bìa";
        lblAnhBia.Name = "lblAnhBia";
        lblAnhBia.TabIndex = 34;
        //
        // picAnhBia
        //
        picAnhBia.Dock = DockStyle.Fill;
        picAnhBia.BorderStyle = BorderStyle.FixedSingle;
        picAnhBia.Size = new Size(250, 150);
        picAnhBia.SizeMode = PictureBoxSizeMode.Zoom;
        picAnhBia.Name = "picAnhBia";
        picAnhBia.TabIndex = 35;
        //
        // btnChonAnh
        //
        btnChonAnh.Dock = DockStyle.Fill;
        btnChonAnh.Text = "Chọn ảnh bìa...";
        btnChonAnh.UseVisualStyleBackColor = true;
        btnChonAnh.Size = new Size(250, 32);
        btnChonAnh.Name = "btnChonAnh";
        btnChonAnh.TabIndex = 36;
        btnChonAnh.Click += btnChonAnh_Click;
        //
        // lblDangBan
        //
        lblDangBan.Anchor = AnchorStyles.Left;
        lblDangBan.AutoSize = true;
        lblDangBan.Text = "";
        lblDangBan.Name = "lblDangBan";
        lblDangBan.TabIndex = 37;
        //
        // chkDangBan
        //
        chkDangBan.Dock = DockStyle.Fill;
        chkDangBan.Text = "Đang kinh doanh";
        chkDangBan.Checked = true;
        chkDangBan.CheckState = CheckState.Checked;
        chkDangBan.Name = "chkDangBan";
        chkDangBan.TabIndex = 38;
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
        flowNut.TabIndex = 39;
        //
        // btnThemMoi
        //
        btnThemMoi.Size = new Size(110, 36);
        btnThemMoi.Text = "Thêm mới";
        btnThemMoi.UseVisualStyleBackColor = true;
        btnThemMoi.Name = "btnThemMoi";
        btnThemMoi.TabIndex = 40;
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
        btnLuu.TabIndex = 41;
        btnLuu.Click += btnLuu_Click;
        //
        // btnXoa
        //
        btnXoa.Size = new Size(140, 36);
        btnXoa.Text = "Xóa / Ngừng bán";
        btnXoa.UseVisualStyleBackColor = true;
        btnXoa.Name = "btnXoa";
        btnXoa.TabIndex = 42;
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
        pnlDuoi.TabIndex = 43;
        //
        // lblTongSo
        //
        lblTongSo.Dock = DockStyle.Fill;
        lblTongSo.Text = "";
        lblTongSo.TextAlign = ContentAlignment.MiddleLeft;
        lblTongSo.Name = "lblTongSo";
        lblTongSo.TabIndex = 44;
        //
        // dgvDanhSach
        //
        dgvDanhSach.Dock = DockStyle.Fill;
        dgvDanhSach.Name = "dgvDanhSach";
        dgvDanhSach.TabIndex = 45;
        dgvDanhSach.SelectionChanged += dgvDanhSach_SelectionChanged;
        //
        // FrmSach
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1200, 700);
        Controls.Add(dgvDanhSach);
        Controls.Add(pnlDuoi);
        Controls.Add(pnlThongTin);
        Controls.Add(pnlTimKiem);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmSach";
        Text = "Sách";
        ((System.ComponentModel.ISupportInitialize)nudNamXuatBan).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDonGia).EndInit();
        ((System.ComponentModel.ISupportInitialize)picAnhBia).EndInit();
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
    private Label lblLocDanhMuc;
    private ComboBox cboLocDanhMuc;
    private Label lblLocTrangThai;
    private ComboBox cboLocTrangThai;
    private Button btnTim;
    private Button btnLamMoi;
    private Button btnXuatExcel;
    private Panel pnlThongTin;
    private GroupBox grpThongTin;
    private TableLayoutPanel tlpThongTin;
    private Label lblISBN;
    private TextBox txtISBN;
    private Label lblTenSach;
    private TextBox txtTenSach;
    private Label lblDanhMuc;
    private ComboBox cboDanhMuc;
    private Label lblNhaXuatBan;
    private ComboBox cboNhaXuatBan;
    private Label lblNamXuatBan;
    private NumericUpDown nudNamXuatBan;
    private Label lblDonGia;
    private NumericUpDown nudDonGia;
    private Label lblTonKhoNhan;
    private Label lblTonKho;
    private Label lblTacGia;
    private CheckedListBox clbTacGia;
    private Label lblMoTa;
    private TextBox txtMoTa;
    private Label lblAnhBia;
    private PictureBox picAnhBia;
    private Button btnChonAnh;
    private Label lblDangBan;
    private CheckBox chkDangBan;
    private FlowLayoutPanel flowNut;
    private Button btnThemMoi;
    private Button btnLuu;
    private Button btnXoa;
    private Panel pnlDuoi;
    private Label lblTongSo;
    private DataGridView dgvDanhSach;
}
