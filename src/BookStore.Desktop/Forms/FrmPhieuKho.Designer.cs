namespace BookStore.Desktop.Forms;

partial class FrmPhieuKho
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
        pnlPhieu = new Panel();
        tlpPhieu = new TableLayoutPanel();
        lblLoaiPhieu = new Label();
        cboLoaiPhieu = new ComboBox();
        lblGhiChu = new Label();
        txtGhiChu = new TextBox();
        pnlNhapDong = new Panel();
        flowNhapDong = new FlowLayoutPanel();
        lblSach = new Label();
        cboSach = new ComboBox();
        lblSoLuong = new Label();
        nudSoLuong = new NumericUpDown();
        lblDonGiaNhap = new Label();
        nudDonGiaNhap = new NumericUpDown();
        btnThemDong = new Button();
        pnlDuoi = new Panel();
        flowNut = new FlowLayoutPanel();
        btnLuuPhieu = new Button();
        btnPhieuMoi = new Button();
        btnXoaDong = new Button();
        lblTong = new Label();
        dgvDong = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)nudSoLuong).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDonGiaNhap).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDong).BeginInit();
        pnlTieuDe.SuspendLayout();
        pnlPhieu.SuspendLayout();
        tlpPhieu.SuspendLayout();
        pnlNhapDong.SuspendLayout();
        flowNhapDong.SuspendLayout();
        pnlDuoi.SuspendLayout();
        flowNut.SuspendLayout();
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
        lblTieuDe.Text = "Lập phiếu nhập / xuất / điều chỉnh kho";
        lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.TabIndex = 1;
        //
        // pnlPhieu
        //
        pnlPhieu.Controls.Add(tlpPhieu);
        pnlPhieu.Dock = DockStyle.Top;
        pnlPhieu.Padding = new Padding(12, 8, 12, 0);
        pnlPhieu.Size = new Size(1100, 86);
        pnlPhieu.Name = "pnlPhieu";
        pnlPhieu.TabIndex = 2;
        //
        // tlpPhieu
        //
        tlpPhieu.Controls.Add(lblLoaiPhieu, 0, 0);
        tlpPhieu.Controls.Add(cboLoaiPhieu, 1, 0);
        tlpPhieu.Controls.Add(lblGhiChu, 0, 1);
        tlpPhieu.Controls.Add(txtGhiChu, 1, 1);
        tlpPhieu.ColumnCount = 2;
        tlpPhieu.Dock = DockStyle.Top;
        tlpPhieu.RowCount = 2;
        tlpPhieu.Padding = new Padding(0, 4, 0, 4);
        tlpPhieu.Size = new Size(700, 70);
        tlpPhieu.AutoSize = false;
        tlpPhieu.Name = "tlpPhieu";
        tlpPhieu.TabIndex = 3;
        tlpPhieu.ColumnStyles.Add(new ColumnStyle());
        tlpPhieu.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPhieu.RowStyles.Add(new RowStyle());
        tlpPhieu.RowStyles.Add(new RowStyle());
        //
        // lblLoaiPhieu
        //
        lblLoaiPhieu.Anchor = AnchorStyles.Left;
        lblLoaiPhieu.AutoSize = true;
        lblLoaiPhieu.Text = "Loại phiếu";
        lblLoaiPhieu.Name = "lblLoaiPhieu";
        lblLoaiPhieu.TabIndex = 4;
        //
        // cboLoaiPhieu
        //
        cboLoaiPhieu.Dock = DockStyle.Fill;
        cboLoaiPhieu.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoaiPhieu.Size = new Size(220, 28);
        cboLoaiPhieu.Name = "cboLoaiPhieu";
        cboLoaiPhieu.TabIndex = 5;
        //
        // lblGhiChu
        //
        lblGhiChu.Anchor = AnchorStyles.Left;
        lblGhiChu.AutoSize = true;
        lblGhiChu.Text = "Ghi chú";
        lblGhiChu.Name = "lblGhiChu";
        lblGhiChu.TabIndex = 6;
        //
        // txtGhiChu
        //
        txtGhiChu.Dock = DockStyle.Fill;
        txtGhiChu.MaxLength = 500;
        txtGhiChu.PlaceholderText = "Bắt buộc với phiếu điều chỉnh: ghi rõ lý do";
        txtGhiChu.Name = "txtGhiChu";
        txtGhiChu.TabIndex = 7;
        //
        // pnlNhapDong
        //
        pnlNhapDong.Controls.Add(flowNhapDong);
        pnlNhapDong.BackColor = Color.FromArgb(244, 246, 248);
        pnlNhapDong.Dock = DockStyle.Top;
        pnlNhapDong.Padding = new Padding(12, 10, 12, 6);
        pnlNhapDong.Size = new Size(1100, 52);
        pnlNhapDong.Name = "pnlNhapDong";
        pnlNhapDong.TabIndex = 8;
        //
        // flowNhapDong
        //
        flowNhapDong.Controls.Add(lblSach);
        flowNhapDong.Controls.Add(cboSach);
        flowNhapDong.Controls.Add(lblSoLuong);
        flowNhapDong.Controls.Add(nudSoLuong);
        flowNhapDong.Controls.Add(lblDonGiaNhap);
        flowNhapDong.Controls.Add(nudDonGiaNhap);
        flowNhapDong.Controls.Add(btnThemDong);
        flowNhapDong.Dock = DockStyle.Fill;
        flowNhapDong.WrapContents = false;
        flowNhapDong.Name = "flowNhapDong";
        flowNhapDong.TabIndex = 9;
        //
        // lblSach
        //
        lblSach.Anchor = AnchorStyles.Left;
        lblSach.AutoSize = true;
        lblSach.Margin = new Padding(3, 8, 3, 0);
        lblSach.Text = "Sách:";
        lblSach.Name = "lblSach";
        lblSach.TabIndex = 10;
        //
        // cboSach
        //
        cboSach.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboSach.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboSach.Size = new Size(380, 28);
        cboSach.Name = "cboSach";
        cboSach.TabIndex = 11;
        //
        // lblSoLuong
        //
        lblSoLuong.Anchor = AnchorStyles.Left;
        lblSoLuong.AutoSize = true;
        lblSoLuong.Margin = new Padding(3, 8, 3, 0);
        lblSoLuong.Text = "Số lượng:";
        lblSoLuong.Name = "lblSoLuong";
        lblSoLuong.TabIndex = 12;
        //
        // nudSoLuong
        //
        nudSoLuong.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudSoLuong.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSoLuong.Size = new Size(90, 27);
        nudSoLuong.Value = new decimal(new int[] { 1, 0, 0, 0 });
        nudSoLuong.Name = "nudSoLuong";
        nudSoLuong.TabIndex = 13;
        //
        // lblDonGiaNhap
        //
        lblDonGiaNhap.Anchor = AnchorStyles.Left;
        lblDonGiaNhap.AutoSize = true;
        lblDonGiaNhap.Margin = new Padding(3, 8, 3, 0);
        lblDonGiaNhap.Text = "Đơn giá nhập:";
        lblDonGiaNhap.Name = "lblDonGiaNhap";
        lblDonGiaNhap.TabIndex = 14;
        //
        // nudDonGiaNhap
        //
        nudDonGiaNhap.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
        nudDonGiaNhap.Increment = new decimal(new int[] { 1000, 0, 0, 0 });
        nudDonGiaNhap.ThousandsSeparator = true;
        nudDonGiaNhap.Size = new Size(130, 27);
        nudDonGiaNhap.Name = "nudDonGiaNhap";
        nudDonGiaNhap.TabIndex = 15;
        //
        // btnThemDong
        //
        btnThemDong.Size = new Size(110, 30);
        btnThemDong.Margin = new Padding(6, 1, 3, 3);
        btnThemDong.Text = "Thêm dòng";
        btnThemDong.BackColor = Color.FromArgb(242, 184, 75);
        btnThemDong.FlatStyle = FlatStyle.Flat;
        btnThemDong.UseVisualStyleBackColor = false;
        btnThemDong.Name = "btnThemDong";
        btnThemDong.TabIndex = 16;
        btnThemDong.Click += btnThemDong_Click;
        //
        // pnlDuoi
        //
        pnlDuoi.Controls.Add(lblTong);
        pnlDuoi.Controls.Add(flowNut);
        pnlDuoi.BackColor = Color.FromArgb(244, 246, 248);
        pnlDuoi.Dock = DockStyle.Bottom;
        pnlDuoi.Padding = new Padding(12, 6, 12, 6);
        pnlDuoi.Size = new Size(1100, 54);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.TabIndex = 17;
        //
        // flowNut
        //
        flowNut.Controls.Add(btnLuuPhieu);
        flowNut.Controls.Add(btnPhieuMoi);
        flowNut.Controls.Add(btnXoaDong);
        flowNut.Dock = DockStyle.Right;
        flowNut.FlowDirection = FlowDirection.RightToLeft;
        flowNut.Size = new Size(520, 42);
        flowNut.WrapContents = false;
        flowNut.Name = "flowNut";
        flowNut.TabIndex = 18;
        //
        // btnLuuPhieu
        //
        btnLuuPhieu.Size = new Size(130, 36);
        btnLuuPhieu.Text = "Lưu phiếu";
        btnLuuPhieu.BackColor = Color.FromArgb(242, 184, 75);
        btnLuuPhieu.FlatStyle = FlatStyle.Flat;
        btnLuuPhieu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnLuuPhieu.UseVisualStyleBackColor = false;
        btnLuuPhieu.Name = "btnLuuPhieu";
        btnLuuPhieu.TabIndex = 19;
        btnLuuPhieu.Click += btnLuuPhieu_Click;
        //
        // btnPhieuMoi
        //
        btnPhieuMoi.Size = new Size(110, 36);
        btnPhieuMoi.Text = "Phiếu mới";
        btnPhieuMoi.UseVisualStyleBackColor = true;
        btnPhieuMoi.Name = "btnPhieuMoi";
        btnPhieuMoi.TabIndex = 20;
        btnPhieuMoi.Click += btnPhieuMoi_Click;
        //
        // btnXoaDong
        //
        btnXoaDong.Size = new Size(160, 36);
        btnXoaDong.Text = "Xóa dòng đang chọn";
        btnXoaDong.UseVisualStyleBackColor = true;
        btnXoaDong.Name = "btnXoaDong";
        btnXoaDong.TabIndex = 21;
        btnXoaDong.Click += btnXoaDong_Click;
        //
        // lblTong
        //
        lblTong.Dock = DockStyle.Fill;
        lblTong.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTong.Text = "Chưa có dòng nào";
        lblTong.TextAlign = ContentAlignment.MiddleLeft;
        lblTong.Name = "lblTong";
        lblTong.TabIndex = 22;
        //
        // dgvDong
        //
        dgvDong.Dock = DockStyle.Fill;
        dgvDong.Name = "dgvDong";
        dgvDong.TabIndex = 23;
        //
        // FrmPhieuKho
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1150, 650);
        Controls.Add(dgvDong);
        Controls.Add(pnlDuoi);
        Controls.Add(pnlNhapDong);
        Controls.Add(pnlPhieu);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmPhieuKho";
        Text = "Lập phiếu kho";
        ((System.ComponentModel.ISupportInitialize)nudSoLuong).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDonGiaNhap).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDong).EndInit();
        flowNut.ResumeLayout(false);
        flowNut.PerformLayout();
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        flowNhapDong.ResumeLayout(false);
        flowNhapDong.PerformLayout();
        pnlNhapDong.ResumeLayout(false);
        pnlNhapDong.PerformLayout();
        tlpPhieu.ResumeLayout(false);
        tlpPhieu.PerformLayout();
        pnlPhieu.ResumeLayout(false);
        pnlPhieu.PerformLayout();
        pnlTieuDe.ResumeLayout(false);
        pnlTieuDe.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTieuDe;
    private Label lblTieuDe;
    private Panel pnlPhieu;
    private TableLayoutPanel tlpPhieu;
    private Label lblLoaiPhieu;
    private ComboBox cboLoaiPhieu;
    private Label lblGhiChu;
    private TextBox txtGhiChu;
    private Panel pnlNhapDong;
    private FlowLayoutPanel flowNhapDong;
    private Label lblSach;
    private ComboBox cboSach;
    private Label lblSoLuong;
    private NumericUpDown nudSoLuong;
    private Label lblDonGiaNhap;
    private NumericUpDown nudDonGiaNhap;
    private Button btnThemDong;
    private Panel pnlDuoi;
    private FlowLayoutPanel flowNut;
    private Button btnLuuPhieu;
    private Button btnPhieuMoi;
    private Button btnXoaDong;
    private Label lblTong;
    private DataGridView dgvDong;
}
