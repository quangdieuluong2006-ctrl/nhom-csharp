using System.ComponentModel;
using System.Runtime.CompilerServices;
using BookStore.Application.Common;

namespace BookStore.Desktop.HaTang;

/// <summary>Các hàm giao diện dùng chung để mọi form có cùng phong cách và cách xử lý.</summary>
public static class GiaoDien
{
    public static readonly Color MauChuDao = Color.FromArgb(22, 50, 79);
    public static readonly Color MauNhan = Color.FromArgb(242, 184, 75);
    public static readonly Color MauNenPhu = Color.FromArgb(244, 246, 248);

    /// <summary>Định dạng DataGridView: chỉ đọc, chọn cả dòng, cột tự giãn, tiêu đề màu chủ đạo.</summary>
    public static void KieuBang(DataGridView dgv)
    {
        dgv.ReadOnly = true;
        dgv.AllowUserToAddRows = false;
        dgv.AllowUserToDeleteRows = false;
        dgv.AllowUserToResizeRows = false;
        dgv.MultiSelect = false;
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgv.RowHeadersVisible = false;
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgv.BackgroundColor = Color.White;
        dgv.BorderStyle = BorderStyle.None;
        dgv.EnableHeadersVisualStyles = false;
        dgv.ColumnHeadersDefaultCellStyle.BackColor = MauChuDao;
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = MauChuDao;
        dgv.ColumnHeadersHeight = 34;
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        dgv.RowTemplate.Height = 30;
        dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 236, 196);
        dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 252);
        dgv.GridColor = Color.FromArgb(228, 231, 235);
    }

    /// <summary>
    /// Gán danh sách vào lưới rồi định dạng cột theo kiểu dữ liệu: tiền/số căn phải "N0", ngày "dd/MM/yyyy HH:mm".
    /// Tiêu đề cột lấy từ [DisplayName] của DTO; thuộc tính [Browsable(false)] không tạo cột.
    /// </summary>
    public static void GanDanhSach<T>(DataGridView dgv, IList<T> ds)
    {
        dgv.DataSource = new BindingList<T>(ds);
        foreach (DataGridViewColumn cot in dgv.Columns)
        {
            var kieu = Nullable.GetUnderlyingType(cot.ValueType ?? typeof(object)) ?? cot.ValueType;
            if (kieu == typeof(decimal))
            {
                cot.DefaultCellStyle.Format = "N0";
                cot.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (kieu == typeof(int) || kieu == typeof(long) || kieu == typeof(short))
            {
                cot.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (kieu == typeof(DateTime))
            {
                cot.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            }
        }
    }

    /// <summary>Đặt độ rộng tương đối (FillWeight) cho các cột hẹp như Mã, Số lượng.</summary>
    public static void DoRongCot(DataGridView dgv, params (string Cot, float TyLe)[] cots)
    {
        foreach (var (cot, tyLe) in cots)
            if (dgv.Columns[cot] is DataGridViewColumn c) c.FillWeight = tyLe;
    }

    public static T? DongDangChon<T>(DataGridView dgv) where T : class => dgv.CurrentRow?.DataBoundItem as T;

    /// <summary>Chọn lại dòng thỏa điều kiện (ví dụ dòng vừa lưu) sau khi nạp lại lưới.</summary>
    public static void ChonDong<T>(DataGridView dgv, Func<T, bool> dieuKien) where T : class
    {
        foreach (DataGridViewRow r in dgv.Rows)
        {
            if (r.DataBoundItem is T item && dieuKien(item))
            {
                dgv.CurrentCell = r.Cells.Cast<DataGridViewCell>().First(c => c.Visible);
                return;
            }
        }
    }

    /// <summary>
    /// Chạy một thao tác bất đồng bộ: hiện con trỏ chờ, khóa control trong lúc chạy;
    /// lỗi nghiệp vụ (BusinessException) hiện cảnh báo; lỗi khác đẩy lên bộ xử lý lỗi toàn cục (ghi log).
    /// </summary>
    public static async Task<bool> ThucHienAsync(Control vungKhoa, Func<Task> thaoTac)
    {
        var form = vungKhoa.FindForm();
        try
        {
            vungKhoa.Enabled = false;
            if (form != null) form.UseWaitCursor = true;
            await ChayTuanTuAsync(vungKhoa, thaoTac);
            return true;
        }
        catch (BusinessException ex)
        {
            ThongBao.CanhBao(ex.Message);
            return false;
        }
        finally
        {
            vungKhoa.Enabled = true;
            if (form != null) form.UseWaitCursor = false;
        }
    }

    // Mỗi form một "khóa": các thao tác CSDL của form chạy lần lượt, không chồng lên nhau.
    private static readonly ConditionalWeakTable<Form, SemaphoreSlim> KhoaTheoForm = new();

    /// <summary>
    /// Chạy thao tác CSDL theo hàng đợi của form. Lý do: mỗi form dùng MỘT DbContext, mà DbContext không cho
    /// hai truy vấn chạy song song (ví dụ người dùng chuyển dòng thật nhanh, hoặc bộ hẹn giờ làm mới đúng lúc đang lưu).
    /// </summary>
    public static async Task ChayTuanTuAsync(Control control, Func<Task> thaoTac)
    {
        var form = control as Form ?? control.FindForm();
        if (form is null) { await thaoTac(); return; }
        var khoa = KhoaTheoForm.GetValue(form, _ => new SemaphoreSlim(1, 1));
        await khoa.WaitAsync();
        try { await thaoTac(); }
        finally { khoa.Release(); }
    }

    /// <summary>Đổ danh sách vào ComboBox; có thể thêm một mục đầu như "(Tất cả)" với mã 0.</summary>
    public static void GanCombo(ComboBox cbo, IEnumerable<object> ds, string? mucDau = null)
    {
        var list = new List<object>();
        if (mucDau != null) list.Add(new Application.Dtos.MucChonDto(0, mucDau));
        list.AddRange(ds);
        cbo.DataSource = list;
        cbo.DisplayMember = "Ten";
        cbo.ValueMember = "Ma";
    }

    /// <summary>Mã đang chọn trong ComboBox; null nếu chọn mục đầu (mã 0) hoặc chưa chọn.</summary>
    public static int? MaDangChon(ComboBox cbo) =>
        cbo.SelectedItem is Application.Dtos.MucChonDto m && m.Ma != 0 ? m.Ma : null;

    public static void ChonTheoMa(ComboBox cbo, int? ma)
    {
        if (cbo.DataSource is not IList<object> ds) return;
        var i = ds.Cast<Application.Dtos.MucChonDto>().ToList().FindIndex(m => m.Ma == (ma ?? 0));
        cbo.SelectedIndex = i >= 0 ? i : (ds.Count > 0 ? 0 : -1);
    }
}
