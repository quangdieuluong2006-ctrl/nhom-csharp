using BookStore.Application.Dtos;
using BookStore.Domain.Enums;

namespace BookStore.Desktop.HaTang;

/// <summary>
/// Người dùng đang đăng nhập phần mềm quản trị (đăng ký Singleton – dùng chung cho mọi form).
/// Form nào cần biết ai đang thao tác (ví dụ lập phiếu kho cần MaTaiKhoan) thì nhận PhienDangNhap qua constructor.
/// </summary>
public class PhienDangNhap
{
    public NguoiDungDto? NguoiDung { get; private set; }

    public bool DaDangNhap => NguoiDung is not null;
    public int MaTaiKhoan => NguoiDung?.MaTaiKhoan ?? throw new InvalidOperationException("Chưa đăng nhập");

    public void DangNhap(NguoiDungDto nd) => NguoiDung = nd;
    public void DangXuat() => NguoiDung = null;

    /// <summary>Kiểm tra người dùng hiện tại có thuộc một trong các vai trò được phép không.</summary>
    public bool CoQuyen(params VaiTro[] vaiTros) => NguoiDung is not null && vaiTros.Contains(NguoiDung.VaiTro);

    public static string TenVaiTro(VaiTro v) => v.Ten();
}
