using BookStore.Application.Dtos;

namespace BookStore.Web.Models;

/// <summary>Dữ liệu trang chủ: sách mới cập nhật và sách bán chạy.</summary>
public class TrangChuViewModel
{
    public IReadOnlyList<SachTomTatDto> SachMoi { get; init; } = Array.Empty<SachTomTatDto>();
    public IReadOnlyList<SachTomTatDto> SachBanChay { get; init; } = Array.Empty<SachTomTatDto>();
}
