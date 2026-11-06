namespace BookStore.Desktop.Forms;

partial class FrmBaoCao
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
        lblTuNgay = new Label();
        dtpTuNgay = new DateTimePicker();
        lblDenNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        btnXem = new Button();
        btn7Ngay = new Button();
        btnThangNay = new Button();
        btnXuatExcel = new Button();
        pnlTongKet = new Panel();
        lblTongKet = new Label();
        tabBaoCao = new TabControl();
        tabBieuDo = new TabPage();
        tabTheoNgay = new TabPage();
        tabBanChay = new TabPage();
        bieuDo = new HaTang.BieuDoCot();
        dgvTheoNgay = new DataGridView();
        dgvBanChay = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvTheoNgay).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvBanChay).BeginInit();
        pnlTieuDe.SuspendLayout();
        pnlTimKiem.SuspendLayout();
        flowTimKiem.SuspendLayout();
        pnlTongKet.SuspendLayout();
        tabBaoCao.SuspendLayout();
        tabBieuDo.SuspendLayout();
        tabTheoNgay.SuspendLayout();
        tabBanChay.SuspendLayout();
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
        lblTieuDe.Text = "Báo cáo doanh thu và sách bán chạy";
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
        flowTimKiem.Controls.Add(lblTuNgay);
        flowTimKiem.Controls.Add(dtpTuNgay);
        flowTimKiem.Controls.Add(lblDenNgay);
        flowTimKiem.Controls.Add(dtpDenNgay);
        flowTimKiem.Controls.Add(btnXem);
        flowTimKiem.Controls.Add(btn7Ngay);
        flowTimKiem.Controls.Add(btnThangNay);
        flowTimKiem.Controls.Add(btnXuatExcel);
        flowTimKiem.Dock = DockStyle.Fill;
        flowTimKiem.WrapContents = false;
        flowTimKiem.Name = "flowTimKiem";
        flowTimKiem.TabIndex = 3;
        //
        // lblTuNgay
        //
        lblTuNgay.Anchor = AnchorStyles.Left;
        lblTuNgay.AutoSize = true;
        lblTuNgay.Margin = new Padding(3, 8, 3, 0);
        lblTuNgay.Text = "Từ ngày:";
        lblTuNgay.Name = "lblTuNgay";
        lblTuNgay.TabIndex = 4;
        //
        // dtpTuNgay
        //
        dtpTuNgay.Format = DateTimePickerFormat.Short;
        dtpTuNgay.Size = new Size(140, 27);
        dtpTuNgay.Name = "dtpTuNgay";
        dtpTuNgay.TabIndex = 5;
        //
        // lblDenNgay
        //
        lblDenNgay.Anchor = AnchorStyles.Left;
        lblDenNgay.AutoSize = true;
        lblDenNgay.Margin = new Padding(3, 8, 3, 0);
        lblDenNgay.Text = "Đến ngày:";
        lblDenNgay.Name = "lblDenNgay";
        lblDenNgay.TabIndex = 6;
        //
        // dtpDenNgay
        //
        dtpDenNgay.Format = DateTimePickerFormat.Short;
        dtpDenNgay.Size = new Size(140, 27);
        dtpDenNgay.Name = "dtpDenNgay";
        dtpDenNgay.TabIndex = 7;
        //
        // btnXem
        //
        btnXem.Size = new Size(120, 30);
        btnXem.Margin = new Padding(6, 1, 3, 3);
        btnXem.Text = "Xem báo cáo";
        btnXem.BackColor = Color.FromArgb(242, 184, 75);
        btnXem.FlatStyle = FlatStyle.Flat;
        btnXem.UseVisualStyleBackColor = false;
        btnXem.Name = "btnXem";
        btnXem.TabIndex = 8;
        btnXem.Click += btnXem_Click;
        //
        // btn7Ngay
        //
        btn7Ngay.Size = new Size(80, 30);
        btn7Ngay.Margin = new Padding(6, 1, 3, 3);
        btn7Ngay.Text = "7 ngày";
        btn7Ngay.UseVisualStyleBackColor = true;
        btn7Ngay.Name = "btn7Ngay";
        btn7Ngay.TabIndex = 9;
        btn7Ngay.Click += btn7Ngay_Click;
        //
        // btnThangNay
        //
        btnThangNay.Size = new Size(100, 30);
        btnThangNay.Margin = new Padding(6, 1, 3, 3);
        btnThangNay.Text = "Tháng này";
        btnThangNay.UseVisualStyleBackColor = true;
        btnThangNay.Name = "btnThangNay";
        btnThangNay.TabIndex = 10;
        btnThangNay.Click += btnThangNay_Click;
        //
        // btnXuatExcel
        //
        btnXuatExcel.Size = new Size(110, 30);
        btnXuatExcel.Margin = new Padding(6, 1, 3, 3);
        btnXuatExcel.Text = "Xuất Excel";
        btnXuatExcel.UseVisualStyleBackColor = true;
        btnXuatExcel.Name = "btnXuatExcel";
        btnXuatExcel.TabIndex = 11;
        btnXuatExcel.Click += btnXuatExcel_Click;
        //
        // pnlTongKet
        //
        pnlTongKet.Controls.Add(lblTongKet);
        pnlTongKet.Dock = DockStyle.Top;
        pnlTongKet.Padding = new Padding(16, 6, 16, 6);
        pnlTongKet.Size = new Size(1100, 44);
        pnlTongKet.Name = "pnlTongKet";
        pnlTongKet.TabIndex = 12;
        //
        // lblTongKet
        //
        lblTongKet.Dock = DockStyle.Fill;
        lblTongKet.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTongKet.ForeColor = Color.FromArgb(22, 50, 79);
        lblTongKet.TextAlign = ContentAlignment.MiddleLeft;
        lblTongKet.Name = "lblTongKet";
        lblTongKet.TabIndex = 13;
        //
        // tabBaoCao
        //
        tabBaoCao.Controls.Add(tabBieuDo);
        tabBaoCao.Controls.Add(tabTheoNgay);
        tabBaoCao.Controls.Add(tabBanChay);
        tabBaoCao.Dock = DockStyle.Fill;
        tabBaoCao.Name = "tabBaoCao";
        tabBaoCao.TabIndex = 14;
        //
        // tabBieuDo
        //
        tabBieuDo.Controls.Add(bieuDo);
        tabBieuDo.Padding = new Padding(6);
        tabBieuDo.Text = "Biểu đồ doanh thu";
        tabBieuDo.UseVisualStyleBackColor = true;
        tabBieuDo.Name = "tabBieuDo";
        tabBieuDo.TabIndex = 15;
        //
        // tabTheoNgay
        //
        tabTheoNgay.Controls.Add(dgvTheoNgay);
        tabTheoNgay.Padding = new Padding(6);
        tabTheoNgay.Text = "Doanh thu theo ngày";
        tabTheoNgay.UseVisualStyleBackColor = true;
        tabTheoNgay.Name = "tabTheoNgay";
        tabTheoNgay.TabIndex = 16;
        //
        // tabBanChay
        //
        tabBanChay.Controls.Add(dgvBanChay);
        tabBanChay.Padding = new Padding(6);
        tabBanChay.Text = "Sách bán chạy";
        tabBanChay.UseVisualStyleBackColor = true;
        tabBanChay.Name = "tabBanChay";
        tabBanChay.TabIndex = 17;
        //
        // bieuDo
        //
        bieuDo.Dock = DockStyle.Fill;
        bieuDo.TieuDe = "Doanh thu theo ngày (đơn hoàn thành)";
        bieuDo.Name = "bieuDo";
        bieuDo.TabIndex = 18;
        //
        // dgvTheoNgay
        //
        dgvTheoNgay.Dock = DockStyle.Fill;
        dgvTheoNgay.Name = "dgvTheoNgay";
        dgvTheoNgay.TabIndex = 19;
        //
        // dgvBanChay
        //
        dgvBanChay.Dock = DockStyle.Fill;
        dgvBanChay.Name = "dgvBanChay";
        dgvBanChay.TabIndex = 20;
        //
        // FrmBaoCao
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1150, 680);
        Controls.Add(tabBaoCao);
        Controls.Add(pnlTongKet);
        Controls.Add(pnlTimKiem);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmBaoCao";
        Text = "Báo cáo";
        ((System.ComponentModel.ISupportInitialize)dgvTheoNgay).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvBanChay).EndInit();
        tabBanChay.ResumeLayout(false);
        tabBanChay.PerformLayout();
        tabTheoNgay.ResumeLayout(false);
        tabTheoNgay.PerformLayout();
        tabBieuDo.ResumeLayout(false);
        tabBieuDo.PerformLayout();
        tabBaoCao.ResumeLayout(false);
        pnlTongKet.ResumeLayout(false);
        pnlTongKet.PerformLayout();
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
    private Label lblTuNgay;
    private DateTimePicker dtpTuNgay;
    private Label lblDenNgay;
    private DateTimePicker dtpDenNgay;
    private Button btnXem;
    private Button btn7Ngay;
    private Button btnThangNay;
    private Button btnXuatExcel;
    private Panel pnlTongKet;
    private Label lblTongKet;
    private TabControl tabBaoCao;
    private TabPage tabBieuDo;
    private TabPage tabTheoNgay;
    private TabPage tabBanChay;
    private HaTang.BieuDoCot bieuDo;
    private DataGridView dgvTheoNgay;
    private DataGridView dgvBanChay;
}
