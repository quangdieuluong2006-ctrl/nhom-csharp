namespace BookStore.Desktop.Forms;

partial class FrmKho
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
        tabKho = new TabControl();
        tabTonKho = new TabPage();
        tabLichSu = new TabPage();
        flowTonKho = new FlowLayoutPanel();
        txtTimTonKho = new TextBox();
        chkSapHet = new CheckBox();
        btnXemTonKho = new Button();
        btnXuatTonKho = new Button();
        dgvTonKho = new DataGridView();
        flowLichSu = new FlowLayoutPanel();
        lblTuNgay = new Label();
        dtpTuNgay = new DateTimePicker();
        lblDenNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        lblLoai = new Label();
        cboLoai = new ComboBox();
        btnXemLichSu = new Button();
        lblChiTietPhieu = new Label();
        dgvChiTietPhieu = new DataGridView();
        dgvPhieu = new DataGridView();
        ((System.ComponentModel.ISupportInitialize)dgvTonKho).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvChiTietPhieu).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvPhieu).BeginInit();
        pnlTieuDe.SuspendLayout();
        tabKho.SuspendLayout();
        tabTonKho.SuspendLayout();
        tabLichSu.SuspendLayout();
        flowTonKho.SuspendLayout();
        flowLichSu.SuspendLayout();
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
        lblTieuDe.Text = "Tồn kho và lịch sử phiếu kho";
        lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.TabIndex = 1;
        //
        // tabKho
        //
        tabKho.Controls.Add(tabTonKho);
        tabKho.Controls.Add(tabLichSu);
        tabKho.Dock = DockStyle.Fill;
        tabKho.Name = "tabKho";
        tabKho.TabIndex = 2;
        //
        // tabTonKho
        //
        tabTonKho.Controls.Add(dgvTonKho);
        tabTonKho.Controls.Add(flowTonKho);
        tabTonKho.Padding = new Padding(6);
        tabTonKho.Text = "Tồn kho";
        tabTonKho.UseVisualStyleBackColor = true;
        tabTonKho.Name = "tabTonKho";
        tabTonKho.TabIndex = 3;
        //
        // tabLichSu
        //
        tabLichSu.Controls.Add(dgvPhieu);
        tabLichSu.Controls.Add(lblChiTietPhieu);
        tabLichSu.Controls.Add(dgvChiTietPhieu);
        tabLichSu.Controls.Add(flowLichSu);
        tabLichSu.Padding = new Padding(6);
        tabLichSu.Text = "Lịch sử phiếu kho";
        tabLichSu.UseVisualStyleBackColor = true;
        tabLichSu.Name = "tabLichSu";
        tabLichSu.TabIndex = 4;
        //
        // flowTonKho
        //
        flowTonKho.Controls.Add(txtTimTonKho);
        flowTonKho.Controls.Add(chkSapHet);
        flowTonKho.Controls.Add(btnXemTonKho);
        flowTonKho.Controls.Add(btnXuatTonKho);
        flowTonKho.Dock = DockStyle.Top;
        flowTonKho.Size = new Size(1100, 42);
        flowTonKho.WrapContents = false;
        flowTonKho.Name = "flowTonKho";
        flowTonKho.TabIndex = 5;
        //
        // txtTimTonKho
        //
        txtTimTonKho.PlaceholderText = "Tên sách...";
        txtTimTonKho.Size = new Size(240, 27);
        txtTimTonKho.Name = "txtTimTonKho";
        txtTimTonKho.TabIndex = 6;
        txtTimTonKho.KeyDown += txtTimTonKho_KeyDown;
        //
        // chkSapHet
        //
        chkSapHet.AutoSize = true;
        chkSapHet.Margin = new Padding(10, 6, 3, 0);
        chkSapHet.Text = "Chỉ sách sắp hết (tồn ≤ 5)";
        chkSapHet.Name = "chkSapHet";
        chkSapHet.TabIndex = 7;
        chkSapHet.CheckedChanged += btnXemTonKho_Click;
        //
        // btnXemTonKho
        //
        btnXemTonKho.Size = new Size(90, 30);
        btnXemTonKho.Margin = new Padding(6, 1, 3, 3);
        btnXemTonKho.Text = "Xem";
        btnXemTonKho.BackColor = Color.FromArgb(242, 184, 75);
        btnXemTonKho.FlatStyle = FlatStyle.Flat;
        btnXemTonKho.UseVisualStyleBackColor = false;
        btnXemTonKho.Name = "btnXemTonKho";
        btnXemTonKho.TabIndex = 8;
        btnXemTonKho.Click += btnXemTonKho_Click;
        //
        // btnXuatTonKho
        //
        btnXuatTonKho.Size = new Size(110, 30);
        btnXuatTonKho.Margin = new Padding(6, 1, 3, 3);
        btnXuatTonKho.Text = "Xuất Excel";
        btnXuatTonKho.UseVisualStyleBackColor = true;
        btnXuatTonKho.Name = "btnXuatTonKho";
        btnXuatTonKho.TabIndex = 9;
        btnXuatTonKho.Click += btnXuatTonKho_Click;
        //
        // dgvTonKho
        //
        dgvTonKho.Dock = DockStyle.Fill;
        dgvTonKho.Name = "dgvTonKho";
        dgvTonKho.TabIndex = 10;
        dgvTonKho.CellFormatting += dgvTonKho_CellFormatting;
        //
        // flowLichSu
        //
        flowLichSu.Controls.Add(lblTuNgay);
        flowLichSu.Controls.Add(dtpTuNgay);
        flowLichSu.Controls.Add(lblDenNgay);
        flowLichSu.Controls.Add(dtpDenNgay);
        flowLichSu.Controls.Add(lblLoai);
        flowLichSu.Controls.Add(cboLoai);
        flowLichSu.Controls.Add(btnXemLichSu);
        flowLichSu.Dock = DockStyle.Top;
        flowLichSu.Size = new Size(1100, 42);
        flowLichSu.WrapContents = false;
        flowLichSu.Name = "flowLichSu";
        flowLichSu.TabIndex = 11;
        //
        // lblTuNgay
        //
        lblTuNgay.Anchor = AnchorStyles.Left;
        lblTuNgay.AutoSize = true;
        lblTuNgay.Margin = new Padding(3, 8, 3, 0);
        lblTuNgay.Text = "Từ:";
        lblTuNgay.Name = "lblTuNgay";
        lblTuNgay.TabIndex = 12;
        //
        // dtpTuNgay
        //
        dtpTuNgay.Format = DateTimePickerFormat.Short;
        dtpTuNgay.Size = new Size(140, 27);
        dtpTuNgay.Name = "dtpTuNgay";
        dtpTuNgay.TabIndex = 13;
        //
        // lblDenNgay
        //
        lblDenNgay.Anchor = AnchorStyles.Left;
        lblDenNgay.AutoSize = true;
        lblDenNgay.Margin = new Padding(3, 8, 3, 0);
        lblDenNgay.Text = "Đến:";
        lblDenNgay.Name = "lblDenNgay";
        lblDenNgay.TabIndex = 14;
        //
        // dtpDenNgay
        //
        dtpDenNgay.Format = DateTimePickerFormat.Short;
        dtpDenNgay.Size = new Size(140, 27);
        dtpDenNgay.Name = "dtpDenNgay";
        dtpDenNgay.TabIndex = 15;
        //
        // lblLoai
        //
        lblLoai.Anchor = AnchorStyles.Left;
        lblLoai.AutoSize = true;
        lblLoai.Margin = new Padding(3, 8, 3, 0);
        lblLoai.Text = "Loại:";
        lblLoai.Name = "lblLoai";
        lblLoai.TabIndex = 16;
        //
        // cboLoai
        //
        cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoai.Size = new Size(170, 28);
        cboLoai.Name = "cboLoai";
        cboLoai.TabIndex = 17;
        //
        // btnXemLichSu
        //
        btnXemLichSu.Size = new Size(90, 30);
        btnXemLichSu.Margin = new Padding(6, 1, 3, 3);
        btnXemLichSu.Text = "Xem";
        btnXemLichSu.BackColor = Color.FromArgb(242, 184, 75);
        btnXemLichSu.FlatStyle = FlatStyle.Flat;
        btnXemLichSu.UseVisualStyleBackColor = false;
        btnXemLichSu.Name = "btnXemLichSu";
        btnXemLichSu.TabIndex = 18;
        btnXemLichSu.Click += btnXemLichSu_Click;
        //
        // lblChiTietPhieu
        //
        lblChiTietPhieu.Dock = DockStyle.Bottom;
        lblChiTietPhieu.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblChiTietPhieu.Size = new Size(1100, 28);
        lblChiTietPhieu.Text = "Chi tiết phiếu đang chọn";
        lblChiTietPhieu.TextAlign = ContentAlignment.BottomLeft;
        lblChiTietPhieu.Name = "lblChiTietPhieu";
        lblChiTietPhieu.TabIndex = 19;
        //
        // dgvChiTietPhieu
        //
        dgvChiTietPhieu.Dock = DockStyle.Bottom;
        dgvChiTietPhieu.Size = new Size(1100, 200);
        dgvChiTietPhieu.Name = "dgvChiTietPhieu";
        dgvChiTietPhieu.TabIndex = 20;
        //
        // dgvPhieu
        //
        dgvPhieu.Dock = DockStyle.Fill;
        dgvPhieu.Name = "dgvPhieu";
        dgvPhieu.TabIndex = 21;
        dgvPhieu.SelectionChanged += dgvPhieu_SelectionChanged;
        //
        // FrmKho
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(1150, 680);
        Controls.Add(tabKho);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmKho";
        Text = "Tồn kho";
        ((System.ComponentModel.ISupportInitialize)dgvTonKho).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvChiTietPhieu).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvPhieu).EndInit();
        flowLichSu.ResumeLayout(false);
        flowLichSu.PerformLayout();
        flowTonKho.ResumeLayout(false);
        flowTonKho.PerformLayout();
        tabLichSu.ResumeLayout(false);
        tabLichSu.PerformLayout();
        tabTonKho.ResumeLayout(false);
        tabTonKho.PerformLayout();
        tabKho.ResumeLayout(false);
        pnlTieuDe.ResumeLayout(false);
        pnlTieuDe.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTieuDe;
    private Label lblTieuDe;
    private TabControl tabKho;
    private TabPage tabTonKho;
    private TabPage tabLichSu;
    private FlowLayoutPanel flowTonKho;
    private TextBox txtTimTonKho;
    private CheckBox chkSapHet;
    private Button btnXemTonKho;
    private Button btnXuatTonKho;
    private DataGridView dgvTonKho;
    private FlowLayoutPanel flowLichSu;
    private Label lblTuNgay;
    private DateTimePicker dtpTuNgay;
    private Label lblDenNgay;
    private DateTimePicker dtpDenNgay;
    private Label lblLoai;
    private ComboBox cboLoai;
    private Button btnXemLichSu;
    private Label lblChiTietPhieu;
    private DataGridView dgvChiTietPhieu;
    private DataGridView dgvPhieu;
}
