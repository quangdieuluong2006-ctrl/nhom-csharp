using BookStore.Application.Common;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Kiểm tra dữ liệu nhập ngay trên form bằng ErrorProvider (biểu tượng đỏ cạnh ô nhập sai, rê chuột để xem lỗi).
/// Quy tắc dùng lại đúng các hàm KiemTra của tầng nghiệp vụ (BookStore.Application.Common.KiemTra) nên thông báo
/// trên Desktop giống hệt thông báo của service – không viết lặp quy tắc ở giao diện.
/// Service vẫn kiểm tra lại khi lưu (phòng thủ 2 lớp); lớp này chỉ giúp người dùng thấy lỗi sớm, đúng chỗ.
/// </summary>
/// <example>
/// _kiemTra = new KiemTraNhap(this)
///     .BatBuoc(txtTenSach, "Tên sách", 200)
///     .SoDienThoai(txtSoDienThoai, batBuoc: false);
/// ...
/// if (!_kiemTra.HopLe()) return;   // trong nút Lưu
/// </example>
public sealed class KiemTraNhap
{
    private readonly ErrorProvider _loi;
    private readonly List<(Control O, Func<string?> QuyTac)> _ds = new();

    public KiemTraNhap(Form form)
    {
        _loi = new ErrorProvider { BlinkStyle = ErrorBlinkStyle.NeverBlink, ContainerControl = form };
        form.Disposed += (_, _) => _loi.Dispose();
    }

    /// <summary>Ô bắt buộc nhập, tối đa <paramref name="doDaiToiDa"/> ký tự.</summary>
    public KiemTraNhap BatBuoc(Control o, string tenTruong, int doDaiToiDa) =>
        Them(o, () => Thu(() => KiemTra.BatBuoc(o.Text, tenTruong, doDaiToiDa)));

    /// <summary>Ô không bắt buộc nhưng giới hạn độ dài.</summary>
    public KiemTraNhap ToiDa(Control o, string tenTruong, int doDaiToiDa) =>
        Them(o, () => Thu(() => KiemTra.TuyChon(o.Text, tenTruong, doDaiToiDa)));

    public KiemTraNhap SoDienThoai(Control o, bool batBuoc) =>
        Them(o, () => Thu(() => { if (batBuoc) KiemTra.SoDienThoai(o.Text); else KiemTra.SoDienThoaiTuyChon(o.Text); }));

    public KiemTraNhap Email(Control o) => Them(o, () => Thu(() => KiemTra.Email(o.Text)));

    /// <summary>ComboBox bắt buộc chọn một mục có mã (MucChonDto).</summary>
    public KiemTraNhap PhaiChon(ComboBox o, string thongBao) =>
        Them(o, () => GiaoDien.MaDangChon(o) is null ? thongBao : null);

    /// <summary>Quy tắc riêng: trả về thông báo lỗi, hoặc null nếu hợp lệ.</summary>
    public KiemTraNhap QuyTac(Control o, Func<string?> quyTac) => Them(o, quyTac);

    /// <summary>Kiểm tra mọi ô; đánh dấu các ô sai và đưa con trỏ về ô sai đầu tiên.</summary>
    public bool HopLe()
    {
        Control? oSaiDauTien = null;
        foreach (var o in _ds.Select(x => x.O).Distinct())
            if (!KiemTraMotO(o)) oSaiDauTien ??= o;
        if (oSaiDauTien is null) return true;
        oSaiDauTien.Focus();
        return false;
    }

    /// <summary>Xóa mọi dấu lỗi (khi chọn dòng khác hoặc bấm Thêm mới).</summary>
    public void XoaLoi()
    {
        foreach (var o in _ds.Select(x => x.O).Distinct()) _loi.SetError(o, "");
    }

    /// <summary>Kiểm tra lại một ô đang bị đánh dấu lỗi (dùng cho control không có sự kiện TextChanged, như CheckedListBox).</summary>
    public void KiemTraLai(Control o)
    {
        if (_loi.GetError(o).Length > 0) KiemTraMotO(o);
    }

    private KiemTraNhap Them(Control o, Func<string?> quyTac)
    {
        if (_ds.All(x => x.O != o))
        {
            // Biểu tượng lỗi vẽ bên phải ô; ô nằm sát mép bảng thì chừa lề phải để biểu tượng không bị che
            _loi.SetIconPadding(o, 2);
            if (o.Parent is TableLayoutPanel bang && bang.Padding.Right < 22)
                bang.Padding = new Padding(bang.Padding.Left, bang.Padding.Top, 22, bang.Padding.Bottom);
            // Đã báo lỗi ở ô này thì kiểm tra lại ngay khi người dùng sửa, để dấu đỏ biến mất khi đã đúng
            o.TextChanged += (_, _) => KiemTraLai(o);
            if (o is ComboBox cbo) cbo.SelectedIndexChanged += (_, _) => KiemTraLai(o);
        }
        _ds.Add((o, quyTac));
        return this;
    }

    private bool KiemTraMotO(Control o)
    {
        var loi = _ds.Where(x => x.O == o).Select(x => x.QuyTac()).FirstOrDefault(l => l != null);
        _loi.SetError(o, loi ?? "");
        return loi is null;
    }

    private static string? Thu(Action kiemTra)
    {
        try { kiemTra(); return null; }
        catch (BusinessException ex) { return ex.Message; }
    }
}
