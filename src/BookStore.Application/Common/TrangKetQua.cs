namespace BookStore.Application.Common;

/// <summary>Kết quả có phân trang: danh sách của trang hiện tại kèm tổng số bản ghi.</summary>
public class TrangKetQua<T>
{
    public IReadOnlyList<T> Items { get; init; } = Array.Empty<T>();
    public int Trang { get; init; }
    public int KichThuocTrang { get; init; }
    public int TongSo { get; init; }
    public int SoTrang => KichThuocTrang == 0 ? 0 : (int)Math.Ceiling(TongSo / (double)KichThuocTrang);
}
