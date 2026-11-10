namespace BookStore.Desktop.Forms;

partial class FrmDonHang
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
        pnlTieuDe = new Panel();
        lblTieuDe = new Label();
        pnlTimKiem = new Panel();
        flowTimKiem = new FlowLayoutPanel();
        txtTimKiem = new TextBox();
        lblLocTrangThai = new Label();
        cboTrangThai = new ComboBox();
        lblTuNgay = new Label();
        dtpTuNgay = new DateTimePicker();
        lblDenNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        btnTim = new Button();
        btnLamMoi = new Button();
        chkTuDongLamMoi = new CheckBox();
        pnlThongTin = new Panel();
        grpChiTiet = new GroupBox();
        txtThongTinDon = new TextBox();
        flowNut = new FlowLayoutPanel();
        btnChuyenTiep = new Button();
        btnXacNhanTien = new Button();
        btnHuyDon = new Button();
        lblTongTien = new Label();
        dgvChiTiet = new DataGridView();
        pnlDuoi = new Panel();
        lblTongSo = new Label();
        dgvDanhSach = new DataGridView();
        tmrLamMoi = new System.Windows.Forms.Timer(components);
        ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDanhSach).BeginInit();
        pnlTieuDe.SuspendLayout();
        pnlTimKiem.SuspendLayout();
        flowTimKiem.SuspendLayout();
        pnlThongTin.SuspendLayout();
        grpChiTiet.SuspendLayout();
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
        lblTieuDe.Text = "Xử lý đơn hàng";
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
        flowTimKiem.Controls.Add(txtTimKiem);
        flowTimKiem.Controls.Add(lblLocTrangThai);
        flowTimKiem.Controls.Add(cboTrangThai);
        flowTimKiem.Controls.Add(lblTuNgay);
        flowTimKiem.Controls.Add(dtpTuNgay);
        flowTimKiem.Controls.Add(lblDenNgay);
        flowTimKiem.Controls.Add(dtpDenNgay);
        flowTimKiem.Controls.Add(btnTim);
        flowTimKiem.Controls.Add(btnLamMoi);
        flowTimKiem.Controls.Add(chkTuDongLamMoi);
        flowTimKiem.Dock = DockStyle.Fill;
        flowTimKiem.WrapContents = false;
        flowTimKiem.Name = "flowTimKiem";
        flowTimKiem.TabIndex = 3;
        //
        // txtTimKiem
        //
        txtTimKiem.PlaceholderText = "Mã đơn, tên, SĐT...";
        txtTimKiem.Size = new Size(200, 27);
        txtTimKiem.Name = "txtTimKiem";
        txtTimKiem.TabIndex = 4;
        txtTimKiem.KeyDown += txtTimKiem_KeyDown;
        //
        // lblLocTrangThai
        //
        lblLocTrangThai.Anchor = AnchorStyles.Left;
        lblLocTrangThai.AutoSize = true;
        lblLocTrangThai.Margin = new Padding(3, 8, 3, 0);
        lblLocTrangThai.Text = "Trạng thái:";
        lblLocTrangThai.Name = "lblLocTrangThai";
        lblLocTrangThai.TabIndex = 5;
        //
        // cboTrangThai
        //
        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.Size = new Size(150, 28);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.TabIndex = 6;
        cboTrangThai.SelectedIndexChanged += btnTim_Click;
        //
        // lblTuNgay
        //
        lblTuNgay.Anchor = AnchorStyles.Left;
        lblTuNgay.AutoSize = true;
        lblTuNgay.Margin = new Padding(3, 8, 3, 0);
        lblTuNgay.Text = "Từ:";
        lblTuNgay.Name = "lblTuNgay";
        lblTuNgay.TabIndex = 7;
        //
        // dtpTuNgay
        //
        dtpTuNgay.Format = DateTimePickerFormat.Short;
        dtpTuNgay.Size = new Size(140, 27);
        dtpTuNgay.ShowCheckBox = true;
        dtpTuNgay.Name = "dtpTuNgay";
        dtpTuNgay.TabIndex = 8;
        //
        // lblDenNgay
        //
        lblDenNgay.Anchor = AnchorStyles.Left;
        lblDenNgay.AutoSize = true;
        lblDenNgay.Margin = new Padding(3, 8, 3, 0);
        lblDenNgay.Text = "Đến:";
        lblDenNgay.Name = "lblDenNgay";
        lblDenNgay.TabIndex = 9;
        //
        // dtpDenNgay
        //
        dtpDenNgay.Format = DateTimePickerFormat.Short;
        dtpDenNgay.Size = new Size(140, 27);
        dtpDenNgay.ShowCheckBox = true;
        dtpDenNgay.Name = "dtpDenNgay";
        dtpDenNgay.TabIndex = 10;
        //
        // btnTim
        //
        btnTim.Size = new Size(80, 30);
        btnTim.Margin = new Padding(6, 1, 3, 3);
        btnTim.Text = "Tìm";
        btnTim.BackColor = Color.FromArgb(242, 184, 75);
        btnTim.FlatStyle = FlatStyle.Flat;
        btnTim.UseVisualStyleBackColor = false;
        btnTim.Name = "btnTim";
        btnTim.TabIndex = 11;
        btnTim.Click += btnTim_Click;
        //
        // btnLamMoi
        //
        btnLamMoi.Size = new Size(90, 30);
        btnLamMoi.Margin = new Padding(6, 1, 3, 3);
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = true;
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.TabIndex = 12;
        btnLamMoi.Click += btnLamMoi_Click;
        //
        // chkTuDongLamMoi
        //
        chkTuDongLamMoi.AutoSize = true;
        chkTuDongLamMoi.Margin = new Padding(10, 6, 3, 0);
        chkTuDongLamMoi.Text = "Tự động làm mới (30 giây)";
        chkTuDongLamMoi.Checked = true;
        chkTuDongLamMoi.CheckState = CheckState.Checked;
        chkTuDongLamMoi.Name = "chkTuDongLamMoi";
        chkTuDongLamMoi.TabIndex = 13;
        chkTuDongLamMoi.CheckedChanged += chkTuDongLamMoi_CheckedChanged;
        //
        // pnlThongTin
        //
        pnlThongTin.Controls.Add(grpChiTiet);
        pnlThongTin.Dock = DockStyle.Right;
        pnlThongTin.Padding = new Padding(10);
        pnlThongTin.Size = new Size(470, 500);
        pnlThongTin.Name = "pnlThongTin";
        pnlThongTin.TabIndex = 14;
        //
        // grpChiTiet
        //
        grpChiTiet.Controls.Add(dgvChiTiet);
        grpChiTiet.Controls.Add(lblTongTien);
        grpChiTiet.Controls.Add(flowNut);
        grpChiTiet.Controls.Add(txtThongTinDon);
        grpChiTiet.Dock = DockStyle.Fill;
        grpChiTiet.Padding = new Padding(10);
        grpChiTiet.Text = "Chi tiết đơn hàng";
        grpChiTiet.Name = "grpChiTiet";
        grpChiTiet.TabIndex = 15;
        //
        // txtThongTinDon
        //
        txtThongTinDon.BackColor = Color.White;
        txtThongTinDon.BorderStyle = BorderStyle.None;
        txtThongTinDon.Dock = DockStyle.Top;
        txtThongTinDon.Multiline = true;
        txtThongTinDon.ReadOnly = true;
        txtThongTinDon.Size = new Size(440, 200);
        txtThongTinDon.TabStop = false;
        txtThongTinDon.Name = "txtThongTinDon";
        txtThongTinDon.TabIndex = 16;
        //
        // flowNut
        //
        flowNut.Controls.Add(btnChuyenTiep);
        flowNut.Controls.Add(btnXacNhanTien);
        flowNut.Controls.Add(btnHuyDon);
        flowNut.AutoSize = true;
        flowNut.Dock = DockStyle.Bottom;
        flowNut.Padding = new Padding(0, 8, 0, 0);
        flowNut.Name = "flowNut";
        flowNut.TabIndex = 17;
        //
        // btnChuyenTiep
        //
        btnChuyenTiep.Size = new Size(200, 36);
        btnChuyenTiep.Text = "Xác nhận đơn";
        btnChuyenTiep.BackColor = Color.FromArgb(242, 184, 75);
        btnChuyenTiep.FlatStyle = FlatStyle.Flat;
        btnChuyenTiep.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnChuyenTiep.UseVisualStyleBackColor = false;
        btnChuyenTiep.Name = "btnChuyenTiep";
        btnChuyenTiep.TabIndex = 18;
        btnChuyenTiep.Click += btnChuyenTiep_Click;
        //
        // btnXacNhanTien
        //
        btnXacNhanTien.Size = new Size(200, 36);
        btnXacNhanTien.Text = "Xác nhận đã nhận tiền";
        btnXacNhanTien.UseVisualStyleBackColor = true;
        btnXacNhanTien.Name = "btnXacNhanTien";
        btnXacNhanTien.TabIndex = 19;
        btnXacNhanTien.Click += btnXacNhanTien_Click;
        //
        // btnHuyDon
        //
        btnHuyDon.Size = new Size(120, 36);
        btnHuyDon.Text = "Hủy đơn";
        btnHuyDon.UseVisualStyleBackColor = true;
        btnHuyDon.Name = "btnHuyDon";
        btnHuyDon.TabIndex = 20;
        btnHuyDon.Click += btnHuyDon_Click;
        //
        // lblTongTien
        //
        lblTongTien.Dock = DockStyle.Bottom;
        lblTongTien.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTongTien.ForeColor = Color.FromArgb(192, 57, 43);
        lblTongTien.Size = new Size(440, 34);
        lblTongTien.Text = "";
        lblTongTien.TextAlign = ContentAlignment.MiddleRight;
        lblTongTien.Name = "lblTongTien";
        lblTongTien.TabIndex = 21;
        //
        // dgvChiTiet
        //
        dgvChiTiet.Dock = DockStyle.Fill;
        dgvChiTiet.Name = "dgvChiTiet";
        dgvChiTiet.TabIndex = 22;
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
        dgvDanhSach.CellFormatting += dgvDanhSach_CellFormatting;
        //
        // tmrLamMoi
        //
        tmrLamMoi.Interval = 30000;
        tmrLamMoi.Tick += tmrLamMoi_Tick;
        //
        // FrmDonHang
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1250, 720);
        Controls.Add(dgvDanhSach);
        Controls.Add(pnlDuoi);
        Controls.Add(pnlThongTin);
        Controls.Add(pnlTimKiem);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmDonHang";
        Text = "Đơn hàng";
        ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDanhSach).EndInit();
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        flowNut.ResumeLayout(false);
        flowNut.PerformLayout();
        grpChiTiet.ResumeLayout(false);
        grpChiTiet.PerformLayout();
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
    private TextBox txtTimKiem;
    private Label lblLocTrangThai;
    private ComboBox cboTrangThai;
    private Label lblTuNgay;
    private DateTimePicker dtpTuNgay;
    private Label lblDenNgay;
    private DateTimePicker dtpDenNgay;
    private Button btnTim;
    private Button btnLamMoi;
    private CheckBox chkTuDongLamMoi;
    private Panel pnlThongTin;
    private GroupBox grpChiTiet;
    private TextBox txtThongTinDon;
    private FlowLayoutPanel flowNut;
    private Button btnChuyenTiep;
    private Button btnXacNhanTien;
    private Button btnHuyDon;
    private Label lblTongTien;
    private DataGridView dgvChiTiet;
    private Panel pnlDuoi;
    private Label lblTongSo;
    private DataGridView dgvDanhSach;
    private System.Windows.Forms.Timer tmrLamMoi;
}
