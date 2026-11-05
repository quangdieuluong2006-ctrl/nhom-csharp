using BookStore.Application.Dtos;
using X.PagedList;

namespace BookStore.Web.Models;

public class DanhSachSachViewModel
{
    public required LocSach Loc { get; init; }
    public required IPagedList<SachTomTatDto> Sachs { get; init; }
    public required string TieuDe { get; init; }
}
