using System.ComponentModel.DataAnnotations;
using BookStore.Domain.Enums;

namespace BookStore.Application.Dtos;

/// <summary>Thông tin người dùng sau khi đăng nhập – được lưu vào cookie dưới dạng claims.</summary>
public record NguoiDungDto(int MaTaiKhoan, string TenDangNhap, string HoTen, VaiTro VaiTro, int? MaKhachHang);

/// <summary>Dữ liệu form đăng ký (Lab04). Các DataAnnotations được kiểm tra ở cả client và server.</summary>
public class DangKyDto
{
    [Required(ErrorMessage = "Vui lòng nhập họ tên")]
    [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
    [Display(Name = "Họ và tên")]
    public string HoTen { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [RegularExpression(@"^[a-zA-Z0-9_.]{4,50}$", ErrorMessage = "Tên đăng nhập 4–50 ký tự, chỉ gồm chữ không dấu, số, dấu _ và .")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
    [Compare(nameof(MatKhau), ErrorMessage = "Mật khẩu nhập lại không khớp")]
    [DataType(DataType.Password)]
    [Display(Name = "Nhập lại mật khẩu")]
    public string NhapLaiMatKhau { get; set; } = "";

    [EmailAddress(ErrorMessage = "Email không hợp lệ")]
    [StringLength(254)]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
    [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại gồm 10–11 chữ số, bắt đầu bằng 0")]
    [Display(Name = "Số điện thoại")]
    public string SoDienThoai { get; set; } = "";

    [StringLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? DiaChi { get; set; }
}

public class DangNhapDto
{
    [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
    [Display(Name = "Tên đăng nhập")]
    public string TenDangNhap { get; set; } = "";

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string MatKhau { get; set; } = "";

    [Display(Name = "Ghi nhớ đăng nhập")]
    public bool GhiNho { get; set; }
}
