using System.ComponentModel;
using System.Reflection;
using ClosedXML.Excel;

namespace BookStore.Desktop.HaTang;

/// <summary>Xuất danh sách DTO ra file Excel (.xlsx) bằng thư viện ClosedXML; tiêu đề cột lấy từ [DisplayName].</summary>
public static class XuatExcel
{
    public static void Xuat<T>(IWin32Window chuSoHuu, IEnumerable<T> ds, string tieuDe, string tenFileGoiY)
    {
        using var dlg = new SaveFileDialog
        {
            Filter = "Excel (*.xlsx)|*.xlsx", FileName = $"{tenFileGoiY}_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
        };
        if (dlg.ShowDialog(chuSoHuu) != DialogResult.OK) return;

        var cots = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.GetCustomAttribute<BrowsableAttribute>()?.Browsable != false)
            .Select(p => (Prop: p, Ten: p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? p.Name))
            .ToList();

        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Báo cáo");
        ws.Cell(1, 1).Value = tieuDe;
        ws.Cell(1, 1).Style.Font.SetBold().Font.SetFontSize(14);
        ws.Cell(2, 1).Value = $"Xuất lúc {DateTime.Now:dd/MM/yyyy HH:mm}";
        for (int c = 0; c < cots.Count; c++)
        {
            var o = ws.Cell(4, c + 1);
            o.Value = cots[c].Ten;
            o.Style.Font.SetBold().Fill.SetBackgroundColor(XLColor.FromArgb(22, 50, 79)).Font.SetFontColor(XLColor.White);
        }
        int dong = 5;
        foreach (var item in ds)
        {
            for (int c = 0; c < cots.Count; c++)
            {
                var v = cots[c].Prop.GetValue(item);
                var o = ws.Cell(dong, c + 1);
                switch (v)
                {
                    case null: break;
                    case decimal d: o.Value = d; o.Style.NumberFormat.Format = "#,##0"; break;
                    case int i: o.Value = i; break;
                    case long l: o.Value = l; break;
                    case short sh: o.Value = sh; break;
                    case DateTime dt: o.Value = dt; o.Style.DateFormat.Format = dt.TimeOfDay == TimeSpan.Zero ? "dd/MM/yyyy" : "dd/MM/yyyy HH:mm"; break;
                    default: o.Value = v.ToString(); break;
                }
            }
            dong++;
        }
        ws.Range(4, 1, Math.Max(4, dong - 1), cots.Count).Style.Border.SetOutsideBorder(XLBorderStyleValues.Thin)
            .Border.SetInsideBorder(XLBorderStyleValues.Thin);
        ws.Columns().AdjustToContents();
        wb.SaveAs(dlg.FileName);
        ThongBao.ThanhCong($"Đã xuất file:\n{dlg.FileName}");
    }
}
