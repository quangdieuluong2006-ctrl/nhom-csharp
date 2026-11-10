namespace BookStore.Desktop.Forms;

partial class FrmTongQuan
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
        flowThe = new FlowLayoutPanel();
        pnlDonCho = new Panel();
        lblGiaTriDonCho = new Label();
        lblTenDonCho = new Label();
        pnlDangXuLy = new Panel();
        lblGiaTriDangXuLy = new Label();
        lblTenDangXuLy = new Label();
        pnlDoanhThuNgay = new Panel();
        lblGiaTriDoanhThuNgay = new Label();
        lblTenDoanhThuNgay = new Label();
        pnlDoanhThuThang = new Panel();
        lblGiaTriDoanhThuThang = new Label();
        lblTenDoanhThuThang = new Label();
        pnlSapHet = new Panel();
        lblGiaTriSapHet = new Label();
        lblTenSapHet = new Label();
        pnlDauSach = new Panel();
        lblGiaTriDauSach = new Label();
        lblTenDauSach = new Label();
        pnlDuoi = new Panel();
        lblCapNhat = new Label();
        btnLamMoi = new Button();
        tlpBang = new TableLayoutPanel();
        lblDonMoi = new Label();
        lblSapHet = new Label();
        dgvDonMoi = new DataGridView();
        dgvSapHet = new DataGridView();
        tmrLamMoi = new System.Windows.Forms.Timer(components);
        ((System.ComponentModel.ISupportInitialize)dgvDonMoi).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvSapHet).BeginInit();
        pnlTieuDe.SuspendLayout();
        flowThe.SuspendLayout();
        pnlDonCho.SuspendLayout();
        pnlDangXuLy.SuspendLayout();
        pnlDoanhThuNgay.SuspendLayout();
        pnlDoanhThuThang.SuspendLayout();
        pnlSapHet.SuspendLayout();
        pnlDauSach.SuspendLayout();
        pnlDuoi.SuspendLayout();
        tlpBang.SuspendLayout();
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
        lblTieuDe.Text = "Tổng quan cửa hàng";
        lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.TabIndex = 1;
        //
        // flowThe
        //
        flowThe.Controls.Add(pnlDonCho);
        flowThe.Controls.Add(pnlDangXuLy);
        flowThe.Controls.Add(pnlDoanhThuNgay);
        flowThe.Controls.Add(pnlDoanhThuThang);
        flowThe.Controls.Add(pnlSapHet);
        flowThe.Controls.Add(pnlDauSach);
        flowThe.BackColor = Color.FromArgb(244, 246, 248);
        flowThe.Dock = DockStyle.Top;
        flowThe.Padding = new Padding(10);
        flowThe.Size = new Size(1100, 130);
        flowThe.Name = "flowThe";
        flowThe.TabIndex = 2;
        //
        // pnlDonCho
        //
        pnlDonCho.Controls.Add(lblGiaTriDonCho);
        pnlDonCho.Controls.Add(lblTenDonCho);
        pnlDonCho.BackColor = Color.White;
        pnlDonCho.Margin = new Padding(6);
        pnlDonCho.Padding = new Padding(12, 8, 12, 8);
        pnlDonCho.Size = new Size(205, 98);
        pnlDonCho.Name = "pnlDonCho";
        pnlDonCho.TabIndex = 3;
        //
        // lblGiaTriDonCho
        //
        lblGiaTriDonCho.Dock = DockStyle.Fill;
        lblGiaTriDonCho.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGiaTriDonCho.ForeColor = Color.FromArgb(22, 50, 79);
        lblGiaTriDonCho.Text = "–";
        lblGiaTriDonCho.TextAlign = ContentAlignment.MiddleLeft;
        lblGiaTriDonCho.Name = "lblGiaTriDonCho";
        lblGiaTriDonCho.TabIndex = 4;
        //
        // lblTenDonCho
        //
        lblTenDonCho.Dock = DockStyle.Top;
        lblTenDonCho.ForeColor = Color.DimGray;
        lblTenDonCho.Size = new Size(150, 24);
        lblTenDonCho.Text = "Đơn chờ xác nhận";
        lblTenDonCho.Name = "lblTenDonCho";
        lblTenDonCho.TabIndex = 5;
        //
        // pnlDangXuLy
        //
        pnlDangXuLy.Controls.Add(lblGiaTriDangXuLy);
        pnlDangXuLy.Controls.Add(lblTenDangXuLy);
        pnlDangXuLy.BackColor = Color.White;
        pnlDangXuLy.Margin = new Padding(6);
        pnlDangXuLy.Padding = new Padding(12, 8, 12, 8);
        pnlDangXuLy.Size = new Size(205, 98);
        pnlDangXuLy.Name = "pnlDangXuLy";
        pnlDangXuLy.TabIndex = 6;
        //
        // lblGiaTriDangXuLy
        //
        lblGiaTriDangXuLy.Dock = DockStyle.Fill;
        lblGiaTriDangXuLy.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGiaTriDangXuLy.ForeColor = Color.FromArgb(22, 50, 79);
        lblGiaTriDangXuLy.Text = "–";
        lblGiaTriDangXuLy.TextAlign = ContentAlignment.MiddleLeft;
        lblGiaTriDangXuLy.Name = "lblGiaTriDangXuLy";
        lblGiaTriDangXuLy.TabIndex = 7;
        //
        // lblTenDangXuLy
        //
        lblTenDangXuLy.Dock = DockStyle.Top;
        lblTenDangXuLy.ForeColor = Color.DimGray;
        lblTenDangXuLy.Size = new Size(150, 24);
        lblTenDangXuLy.Text = "Đơn đang xử lý";
        lblTenDangXuLy.Name = "lblTenDangXuLy";
        lblTenDangXuLy.TabIndex = 8;
        //
        // pnlDoanhThuNgay
        //
        pnlDoanhThuNgay.Controls.Add(lblGiaTriDoanhThuNgay);
        pnlDoanhThuNgay.Controls.Add(lblTenDoanhThuNgay);
        pnlDoanhThuNgay.BackColor = Color.White;
        pnlDoanhThuNgay.Margin = new Padding(6);
        pnlDoanhThuNgay.Padding = new Padding(12, 8, 12, 8);
        pnlDoanhThuNgay.Size = new Size(205, 98);
        pnlDoanhThuNgay.Name = "pnlDoanhThuNgay";
        pnlDoanhThuNgay.TabIndex = 9;
        //
        // lblGiaTriDoanhThuNgay
        //
        lblGiaTriDoanhThuNgay.Dock = DockStyle.Fill;
        lblGiaTriDoanhThuNgay.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGiaTriDoanhThuNgay.ForeColor = Color.FromArgb(22, 50, 79);
        lblGiaTriDoanhThuNgay.Text = "–";
        lblGiaTriDoanhThuNgay.TextAlign = ContentAlignment.MiddleLeft;
        lblGiaTriDoanhThuNgay.Name = "lblGiaTriDoanhThuNgay";
        lblGiaTriDoanhThuNgay.TabIndex = 10;
        //
        // lblTenDoanhThuNgay
        //
        lblTenDoanhThuNgay.Dock = DockStyle.Top;
        lblTenDoanhThuNgay.ForeColor = Color.DimGray;
        lblTenDoanhThuNgay.Size = new Size(150, 24);
        lblTenDoanhThuNgay.Text = "Doanh thu hôm nay";
        lblTenDoanhThuNgay.Name = "lblTenDoanhThuNgay";
        lblTenDoanhThuNgay.TabIndex = 11;
        //
        // pnlDoanhThuThang
        //
        pnlDoanhThuThang.Controls.Add(lblGiaTriDoanhThuThang);
        pnlDoanhThuThang.Controls.Add(lblTenDoanhThuThang);
        pnlDoanhThuThang.BackColor = Color.White;
        pnlDoanhThuThang.Margin = new Padding(6);
        pnlDoanhThuThang.Padding = new Padding(12, 8, 12, 8);
        pnlDoanhThuThang.Size = new Size(205, 98);
        pnlDoanhThuThang.Name = "pnlDoanhThuThang";
        pnlDoanhThuThang.TabIndex = 12;
        //
        // lblGiaTriDoanhThuThang
        //
        lblGiaTriDoanhThuThang.Dock = DockStyle.Fill;
        lblGiaTriDoanhThuThang.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGiaTriDoanhThuThang.ForeColor = Color.FromArgb(22, 50, 79);
        lblGiaTriDoanhThuThang.Text = "–";
        lblGiaTriDoanhThuThang.TextAlign = ContentAlignment.MiddleLeft;
        lblGiaTriDoanhThuThang.Name = "lblGiaTriDoanhThuThang";
        lblGiaTriDoanhThuThang.TabIndex = 13;
        //
        // lblTenDoanhThuThang
        //
        lblTenDoanhThuThang.Dock = DockStyle.Top;
        lblTenDoanhThuThang.ForeColor = Color.DimGray;
        lblTenDoanhThuThang.Size = new Size(150, 24);
        lblTenDoanhThuThang.Text = "Doanh thu tháng này";
        lblTenDoanhThuThang.Name = "lblTenDoanhThuThang";
        lblTenDoanhThuThang.TabIndex = 14;
        //
        // pnlSapHet
        //
        pnlSapHet.Controls.Add(lblGiaTriSapHet);
        pnlSapHet.Controls.Add(lblTenSapHet);
        pnlSapHet.BackColor = Color.White;
        pnlSapHet.Margin = new Padding(6);
        pnlSapHet.Padding = new Padding(12, 8, 12, 8);
        pnlSapHet.Size = new Size(205, 98);
        pnlSapHet.Name = "pnlSapHet";
        pnlSapHet.TabIndex = 15;
        //
        // lblGiaTriSapHet
        //
        lblGiaTriSapHet.Dock = DockStyle.Fill;
        lblGiaTriSapHet.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGiaTriSapHet.ForeColor = Color.FromArgb(22, 50, 79);
        lblGiaTriSapHet.Text = "–";
        lblGiaTriSapHet.TextAlign = ContentAlignment.MiddleLeft;
        lblGiaTriSapHet.Name = "lblGiaTriSapHet";
        lblGiaTriSapHet.TabIndex = 16;
        //
        // lblTenSapHet
        //
        lblTenSapHet.Dock = DockStyle.Top;
        lblTenSapHet.ForeColor = Color.DimGray;
        lblTenSapHet.Size = new Size(150, 24);
        lblTenSapHet.Text = "Sách sắp hết (≤ 5)";
        lblTenSapHet.Name = "lblTenSapHet";
        lblTenSapHet.TabIndex = 17;
        //
        // pnlDauSach
        //
        pnlDauSach.Controls.Add(lblGiaTriDauSach);
        pnlDauSach.Controls.Add(lblTenDauSach);
        pnlDauSach.BackColor = Color.White;
        pnlDauSach.Margin = new Padding(6);
        pnlDauSach.Padding = new Padding(12, 8, 12, 8);
        pnlDauSach.Size = new Size(205, 98);
        pnlDauSach.Name = "pnlDauSach";
        pnlDauSach.TabIndex = 18;
        //
        // lblGiaTriDauSach
        //
        lblGiaTriDauSach.Dock = DockStyle.Fill;
        lblGiaTriDauSach.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblGiaTriDauSach.ForeColor = Color.FromArgb(22, 50, 79);
        lblGiaTriDauSach.Text = "–";
        lblGiaTriDauSach.TextAlign = ContentAlignment.MiddleLeft;
        lblGiaTriDauSach.Name = "lblGiaTriDauSach";
        lblGiaTriDauSach.TabIndex = 19;
        //
        // lblTenDauSach
        //
        lblTenDauSach.Dock = DockStyle.Top;
        lblTenDauSach.ForeColor = Color.DimGray;
        lblTenDauSach.Size = new Size(150, 24);
        lblTenDauSach.Text = "Đầu sách đang bán";
        lblTenDauSach.Name = "lblTenDauSach";
        lblTenDauSach.TabIndex = 20;
        //
        // pnlDuoi
        //
        pnlDuoi.Controls.Add(lblCapNhat);
        pnlDuoi.Controls.Add(btnLamMoi);
        pnlDuoi.BackColor = Color.FromArgb(244, 246, 248);
        pnlDuoi.Dock = DockStyle.Bottom;
        pnlDuoi.Padding = new Padding(12, 4, 12, 4);
        pnlDuoi.Size = new Size(1100, 44);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.TabIndex = 21;
        //
        // lblCapNhat
        //
        lblCapNhat.Dock = DockStyle.Fill;
        lblCapNhat.ForeColor = Color.DimGray;
        lblCapNhat.TextAlign = ContentAlignment.MiddleLeft;
        lblCapNhat.Name = "lblCapNhat";
        lblCapNhat.TabIndex = 22;
        //
        // btnLamMoi
        //
        btnLamMoi.Dock = DockStyle.Right;
        btnLamMoi.Size = new Size(110, 36);
        btnLamMoi.Text = "Làm mới";
        btnLamMoi.UseVisualStyleBackColor = true;
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.TabIndex = 23;
        btnLamMoi.Click += btnLamMoi_Click;
        //
        // tlpBang
        //
        tlpBang.Controls.Add(lblDonMoi, 0, 0);
        tlpBang.Controls.Add(lblSapHet, 1, 0);
        tlpBang.Controls.Add(dgvDonMoi, 0, 1);
        tlpBang.Controls.Add(dgvSapHet, 1, 1);
        tlpBang.ColumnCount = 2;
        tlpBang.Dock = DockStyle.Fill;
        tlpBang.Padding = new Padding(8);
        tlpBang.RowCount = 2;
        tlpBang.Name = "tlpBang";
        tlpBang.TabIndex = 24;
        tlpBang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        tlpBang.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        tlpBang.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        tlpBang.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // lblDonMoi
        //
        lblDonMoi.Dock = DockStyle.Fill;
        lblDonMoi.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDonMoi.Text = "Đơn hàng chờ xác nhận (nhấp đúp để xử lý)";
        lblDonMoi.TextAlign = ContentAlignment.BottomLeft;
        lblDonMoi.Name = "lblDonMoi";
        lblDonMoi.TabIndex = 25;
        //
        // lblSapHet
        //
        lblSapHet.Dock = DockStyle.Fill;
        lblSapHet.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblSapHet.Text = "Sách sắp hết hàng";
        lblSapHet.TextAlign = ContentAlignment.BottomLeft;
        lblSapHet.Name = "lblSapHet";
        lblSapHet.TabIndex = 26;
        //
        // dgvDonMoi
        //
        dgvDonMoi.Dock = DockStyle.Fill;
        dgvDonMoi.Name = "dgvDonMoi";
        dgvDonMoi.TabIndex = 27;
        dgvDonMoi.CellDoubleClick += dgvDonMoi_CellDoubleClick;
        //
        // dgvSapHet
        //
        dgvSapHet.Dock = DockStyle.Fill;
        dgvSapHet.Name = "dgvSapHet";
        dgvSapHet.TabIndex = 28;
        //
        // tmrLamMoi
        //
        tmrLamMoi.Interval = 30000;
        tmrLamMoi.Tick += tmrLamMoi_Tick;
        //
        // FrmTongQuan
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1150, 680);
        Controls.Add(tlpBang);
        Controls.Add(pnlDuoi);
        Controls.Add(flowThe);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmTongQuan";
        Text = "Tổng quan";
        ((System.ComponentModel.ISupportInitialize)dgvDonMoi).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvSapHet).EndInit();
        tlpBang.ResumeLayout(false);
        tlpBang.PerformLayout();
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        pnlDauSach.ResumeLayout(false);
        pnlDauSach.PerformLayout();
        pnlSapHet.ResumeLayout(false);
        pnlSapHet.PerformLayout();
        pnlDoanhThuThang.ResumeLayout(false);
        pnlDoanhThuThang.PerformLayout();
        pnlDoanhThuNgay.ResumeLayout(false);
        pnlDoanhThuNgay.PerformLayout();
        pnlDangXuLy.ResumeLayout(false);
        pnlDangXuLy.PerformLayout();
        pnlDonCho.ResumeLayout(false);
        pnlDonCho.PerformLayout();
        flowThe.ResumeLayout(false);
        flowThe.PerformLayout();
        pnlTieuDe.ResumeLayout(false);
        pnlTieuDe.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTieuDe;
    private Label lblTieuDe;
    private FlowLayoutPanel flowThe;
    private Panel pnlDonCho;
    private Label lblGiaTriDonCho;
    private Label lblTenDonCho;
    private Panel pnlDangXuLy;
    private Label lblGiaTriDangXuLy;
    private Label lblTenDangXuLy;
    private Panel pnlDoanhThuNgay;
    private Label lblGiaTriDoanhThuNgay;
    private Label lblTenDoanhThuNgay;
    private Panel pnlDoanhThuThang;
    private Label lblGiaTriDoanhThuThang;
    private Label lblTenDoanhThuThang;
    private Panel pnlSapHet;
    private Label lblGiaTriSapHet;
    private Label lblTenSapHet;
    private Panel pnlDauSach;
    private Label lblGiaTriDauSach;
    private Label lblTenDauSach;
    private Panel pnlDuoi;
    private Label lblCapNhat;
    private Button btnLamMoi;
    private TableLayoutPanel tlpBang;
    private Label lblDonMoi;
    private Label lblSapHet;
    private DataGridView dgvDonMoi;
    private DataGridView dgvSapHet;
    private System.Windows.Forms.Timer tmrLamMoi;
}
