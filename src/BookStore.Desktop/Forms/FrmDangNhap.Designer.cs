namespace BookStore.Desktop.Forms;

partial class FrmDangNhap
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
        pnlTieuDe = new Panel();
        lblTieuDe = new Label();
        lblPhuDe = new Label();
        lblTenDangNhap = new Label();
        txtTenDangNhap = new TextBox();
        lblMatKhau = new Label();
        txtMatKhau = new TextBox();
        lblLoi = new Label();
        btnDangNhap = new Button();
        btnThoat = new Button();
        pnlTieuDe.SuspendLayout();
        SuspendLayout();
        //
        // pnlTieuDe
        //
        pnlTieuDe.BackColor = Color.FromArgb(22, 50, 79);
        pnlTieuDe.Controls.Add(lblPhuDe);
        pnlTieuDe.Controls.Add(lblTieuDe);
        pnlTieuDe.Dock = DockStyle.Top;
        pnlTieuDe.Location = new Point(0, 0);
        pnlTieuDe.Name = "pnlTieuDe";
        pnlTieuDe.Size = new Size(420, 90);
        pnlTieuDe.TabIndex = 0;
        //
        // lblTieuDe
        //
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTieuDe.ForeColor = Color.FromArgb(242, 184, 75);
        lblTieuDe.Location = new Point(24, 14);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new Size(140, 37);
        lblTieuDe.TabIndex = 0;
        lblTieuDe.Text = "BookStore";
        //
        // lblPhuDe
        //
        lblPhuDe.AutoSize = true;
        lblPhuDe.ForeColor = Color.White;
        lblPhuDe.Location = new Point(27, 55);
        lblPhuDe.Name = "lblPhuDe";
        lblPhuDe.Size = new Size(205, 20);
        lblPhuDe.TabIndex = 1;
        lblPhuDe.Text = "Phần mềm quản trị nhà sách";
        //
        // lblTenDangNhap
        //
        lblTenDangNhap.AutoSize = true;
        lblTenDangNhap.Location = new Point(27, 112);
        lblTenDangNhap.Name = "lblTenDangNhap";
        lblTenDangNhap.Size = new Size(110, 20);
        lblTenDangNhap.TabIndex = 1;
        lblTenDangNhap.Text = "Tên đăng nhập";
        //
        // txtTenDangNhap
        //
        txtTenDangNhap.Location = new Point(27, 135);
        txtTenDangNhap.MaxLength = 50;
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.Size = new Size(366, 27);
        txtTenDangNhap.TabIndex = 2;
        //
        // lblMatKhau
        //
        lblMatKhau.AutoSize = true;
        lblMatKhau.Location = new Point(27, 175);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.Size = new Size(70, 20);
        lblMatKhau.TabIndex = 3;
        lblMatKhau.Text = "Mật khẩu";
        //
        // txtMatKhau
        //
        txtMatKhau.Location = new Point(27, 198);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.Size = new Size(366, 27);
        txtMatKhau.TabIndex = 4;
        txtMatKhau.UseSystemPasswordChar = true;
        //
        // lblLoi
        //
        lblLoi.ForeColor = Color.FromArgb(192, 57, 43);
        lblLoi.Location = new Point(27, 232);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(366, 40);
        lblLoi.TabIndex = 5;
        //
        // btnDangNhap
        //
        btnDangNhap.BackColor = Color.FromArgb(242, 184, 75);
        btnDangNhap.FlatStyle = FlatStyle.Flat;
        btnDangNhap.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnDangNhap.Location = new Point(193, 278);
        btnDangNhap.Name = "btnDangNhap";
        btnDangNhap.Size = new Size(110, 36);
        btnDangNhap.TabIndex = 6;
        btnDangNhap.Text = "Đăng nhập";
        btnDangNhap.UseVisualStyleBackColor = false;
        btnDangNhap.Click += btnDangNhap_Click;
        //
        // btnThoat
        //
        btnThoat.DialogResult = DialogResult.Cancel;
        btnThoat.Location = new Point(313, 278);
        btnThoat.Name = "btnThoat";
        btnThoat.Size = new Size(80, 36);
        btnThoat.TabIndex = 7;
        btnThoat.Text = "Thoát";
        btnThoat.UseVisualStyleBackColor = true;
        btnThoat.Click += btnThoat_Click;
        //
        // FrmDangNhap
        //
        AcceptButton = btnDangNhap;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        CancelButton = btnThoat;
        ClientSize = new Size(420, 336);
        Controls.Add(btnThoat);
        Controls.Add(btnDangNhap);
        Controls.Add(lblLoi);
        Controls.Add(txtMatKhau);
        Controls.Add(lblMatKhau);
        Controls.Add(txtTenDangNhap);
        Controls.Add(lblTenDangNhap);
        Controls.Add(pnlTieuDe);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmDangNhap";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Đăng nhập – BookStore";
        pnlTieuDe.ResumeLayout(false);
        pnlTieuDe.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlTieuDe;
    private Label lblTieuDe;
    private Label lblPhuDe;
    private Label lblTenDangNhap;
    private TextBox txtTenDangNhap;
    private Label lblMatKhau;
    private TextBox txtMatKhau;
    private Label lblLoi;
    private Button btnDangNhap;
    private Button btnThoat;
}
