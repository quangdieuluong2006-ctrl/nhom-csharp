namespace BookStore.Desktop.Forms;

partial class FrmDoiMatKhau
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
        pnlNoiDung = new Panel();
        tlpMatKhau = new TableLayoutPanel();
        lblMatKhauCu = new Label();
        txtMatKhauCu = new TextBox();
        lblMatKhauMoi = new Label();
        txtMatKhauMoi = new TextBox();
        lblNhapLai = new Label();
        txtNhapLai = new TextBox();
        flowNut = new FlowLayoutPanel();
        btnDong = new Button();
        btnDoi = new Button();
        pnlNoiDung.SuspendLayout();
        tlpMatKhau.SuspendLayout();
        flowNut.SuspendLayout();
        SuspendLayout();
        //
        // pnlNoiDung
        //
        pnlNoiDung.Controls.Add(tlpMatKhau);
        pnlNoiDung.Controls.Add(flowNut);
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Padding = new Padding(16);
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.TabIndex = 0;
        //
        // tlpMatKhau
        //
        tlpMatKhau.Controls.Add(lblMatKhauCu, 0, 0);
        tlpMatKhau.Controls.Add(txtMatKhauCu, 1, 0);
        tlpMatKhau.Controls.Add(lblMatKhauMoi, 0, 1);
        tlpMatKhau.Controls.Add(txtMatKhauMoi, 1, 1);
        tlpMatKhau.Controls.Add(lblNhapLai, 0, 2);
        tlpMatKhau.Controls.Add(txtNhapLai, 1, 2);
        tlpMatKhau.AutoSize = true;
        tlpMatKhau.ColumnCount = 2;
        tlpMatKhau.Dock = DockStyle.Top;
        tlpMatKhau.RowCount = 3;
        tlpMatKhau.Padding = new Padding(0, 4, 0, 4);
        tlpMatKhau.Name = "tlpMatKhau";
        tlpMatKhau.TabIndex = 1;
        tlpMatKhau.ColumnStyles.Add(new ColumnStyle());
        tlpMatKhau.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpMatKhau.RowStyles.Add(new RowStyle());
        tlpMatKhau.RowStyles.Add(new RowStyle());
        tlpMatKhau.RowStyles.Add(new RowStyle());
        //
        // lblMatKhauCu
        //
        lblMatKhauCu.Anchor = AnchorStyles.Left;
        lblMatKhauCu.AutoSize = true;
        lblMatKhauCu.Text = "Mật khẩu hiện tại";
        lblMatKhauCu.Name = "lblMatKhauCu";
        lblMatKhauCu.TabIndex = 2;
        //
        // txtMatKhauCu
        //
        txtMatKhauCu.Dock = DockStyle.Fill;
        txtMatKhauCu.UseSystemPasswordChar = true;
        txtMatKhauCu.Name = "txtMatKhauCu";
        txtMatKhauCu.TabIndex = 3;
        //
        // lblMatKhauMoi
        //
        lblMatKhauMoi.Anchor = AnchorStyles.Left;
        lblMatKhauMoi.AutoSize = true;
        lblMatKhauMoi.Text = "Mật khẩu mới";
        lblMatKhauMoi.Name = "lblMatKhauMoi";
        lblMatKhauMoi.TabIndex = 4;
        //
        // txtMatKhauMoi
        //
        txtMatKhauMoi.Dock = DockStyle.Fill;
        txtMatKhauMoi.UseSystemPasswordChar = true;
        txtMatKhauMoi.Name = "txtMatKhauMoi";
        txtMatKhauMoi.TabIndex = 5;
        //
        // lblNhapLai
        //
        lblNhapLai.Anchor = AnchorStyles.Left;
        lblNhapLai.AutoSize = true;
        lblNhapLai.Text = "Nhập lại mật khẩu mới";
        lblNhapLai.Name = "lblNhapLai";
        lblNhapLai.TabIndex = 6;
        //
        // txtNhapLai
        //
        txtNhapLai.Dock = DockStyle.Fill;
        txtNhapLai.UseSystemPasswordChar = true;
        txtNhapLai.Name = "txtNhapLai";
        txtNhapLai.TabIndex = 7;
        //
        // flowNut
        //
        flowNut.Controls.Add(btnDong);
        flowNut.Controls.Add(btnDoi);
        flowNut.Dock = DockStyle.Bottom;
        flowNut.FlowDirection = FlowDirection.RightToLeft;
        flowNut.Size = new Size(400, 44);
        flowNut.Name = "flowNut";
        flowNut.TabIndex = 8;
        //
        // btnDong
        //
        btnDong.Size = new Size(100, 36);
        btnDong.Text = "Đóng";
        btnDong.UseVisualStyleBackColor = true;
        btnDong.DialogResult = DialogResult.Cancel;
        btnDong.Name = "btnDong";
        btnDong.TabIndex = 9;
        //
        // btnDoi
        //
        btnDoi.Size = new Size(130, 36);
        btnDoi.Text = "Đổi mật khẩu";
        btnDoi.BackColor = Color.FromArgb(242, 184, 75);
        btnDoi.FlatStyle = FlatStyle.Flat;
        btnDoi.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnDoi.UseVisualStyleBackColor = false;
        btnDoi.Name = "btnDoi";
        btnDoi.TabIndex = 10;
        btnDoi.Click += btnDoi_Click;
        //
        // FrmDoiMatKhau
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(440, 250);
        Controls.Add(pnlNoiDung);
        Font = new Font("Segoe UI", 9F);
        Name = "FrmDoiMatKhau";
        Text = "Đổi mật khẩu";
        AcceptButton = btnDoi;
        CancelButton = btnDong;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        flowNut.ResumeLayout(false);
        flowNut.PerformLayout();
        tlpMatKhau.ResumeLayout(false);
        tlpMatKhau.PerformLayout();
        pnlNoiDung.ResumeLayout(false);
        pnlNoiDung.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Panel pnlNoiDung;
    private TableLayoutPanel tlpMatKhau;
    private Label lblMatKhauCu;
    private TextBox txtMatKhauCu;
    private Label lblMatKhauMoi;
    private TextBox txtMatKhauMoi;
    private Label lblNhapLai;
    private TextBox txtNhapLai;
    private FlowLayoutPanel flowNut;
    private Button btnDong;
    private Button btnDoi;
}
