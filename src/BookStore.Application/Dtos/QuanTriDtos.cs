using System.ComponentModel;
using BookStore.Domain.Enums;

namespace BookStore.Application.Dtos;

// =====================================================================================
// DTO cho phần mềm quản trị (Desktop).
// - Thuộc tính có [DisplayName] → tiêu đề cột khi gán vào DataGridView (AutoGenerateColumns).
// - Thuộc tính có [Browsable(false)] → không tạo cột (dùng nội bộ: mã, giá trị enum...).
// =====================================================================================

/// <summary>Một mục trong ComboBox/CheckedListBox: mã + tên.</summary>
public record MucChonDto(int Ma, string Ten)
{
    public override string ToString() => Ten;
}

// ------------------------------------ Tài khoản ------------------------------------

public class TaiKhoanQtDto
{
    [DisplayName("Mã")] public int MaTaiKhoan { get; init; }
    [DisplayName("Tên đăng nhập")] public string TenDangNhap { get; init; } = "";
    [DisplayName("Họ tên")] public string HoTen { get; init; } = "";
    [Browsable(false)] public VaiTro VaiTro { get; init; }
    [DisplayName("Vai trò")] public string VaiTroText => VaiTro.Ten();
    [DisplayName("Email")] public string? Email { get; init; }
    [DisplayName("Số điện thoại")] public string? SoDienThoai { get; init; }
    [Browsable(false)] public bool TrangThai { get; init; }
    [DisplayName("Trạng thái")] public string TrangThaiText => TrangThai ? "Hoạt động" : "Đã khóa";
}

public class TaiKhoanLuuDto
{
    public int MaTaiKhoan { get; set; }
    public string TenDangNhap { get; set; } = "";
    public string HoTen { get; set; } = "";
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public VaiTro VaiTro { get; set; } = VaiTro.NhanVienBanHang;
    /// <summary>Chỉ dùng khi tạo mới.</summary>
    public string? MatKhau { get; set; }
}

// ------------------------------- Danh mục, NXB, Tác giả -------------------------------

public class DanhMucQtDto
{
    [DisplayName("Mã")] public int MaDanhMuc { get; init; }
    [DisplayName("Tên danh mục")] public string TenDanhMuc { get; init; } = "";
    [Browsable(false)] public int? MaDanhMucCha { get; init; }
    [DisplayName("Danh mục cha")] public string? TenDanhMucCha { get; init; }
    [DisplayName("Mô tả")] public string? MoTa { get; init; }
    [DisplayName("Số sách")] public int SoSach { get; init; }
    [Browsable(false)] public bool TrangThai { get; init; }
    [DisplayName("Trạng thái")] public string TrangThaiText => TrangThai ? "Đang dùng" : "Ngừng dùng";
}

public class DanhMucLuuDto
{
    public int MaDanhMuc { get; set; }
    public string TenDanhMuc { get; set; } = "";
    public int? MaDanhMucCha { get; set; }
    public string? MoTa { get; set; }
    public bool TrangThai { get; set; } = true;
}

public class NhaXuatBanQtDto
{
    [DisplayName("Mã")] public int MaNhaXuatBan { get; init; }
    [DisplayName("Tên nhà xuất bản")] public string TenNhaXuatBan { get; init; } = "";
    [DisplayName("Địa chỉ")] public string? DiaChi { get; init; }
    [DisplayName("Số điện thoại")] public string? SoDienThoai { get; init; }
    [DisplayName("Email")] public string? Email { get; init; }
    [DisplayName("Số sách")] public int SoSach { get; init; }
}

public class NhaXuatBanLuuDto
{
    public int MaNhaXuatBan { get; set; }
    public string TenNhaXuatBan { get; set; } = "";
    public string? DiaChi { get; set; }
    public string? SoDienThoai { get; set; }
    public string? Email { get; set; }
}

public class TacGiaQtDto
{
    [DisplayName("Mã")] public int MaTacGia { get; init; }
    [DisplayName("Tên tác giả")] public string TenTacGia { get; init; } = "";
    [DisplayName("Tiểu sử")] public string? TieuSu { get; init; }
    [DisplayName("Số sách")] public int SoSach { get; init; }
    [Browsable(false)] public bool TrangThai { get; init; }
    [DisplayName("Trạng thái")] public string TrangThaiText => TrangThai ? "Đang dùng" : "Ngừng dùng";
}

public class TacGiaLuuDto
{
    public int MaTacGia { get; set; }
    public string TenTacGia { get; set; } = "";
    public string? TieuSu { get; set; }
    public bool TrangThai { get; set; } = true;
}

// ------------------------------------- Sách -------------------------------------

public class SachQtDto
{
    [DisplayName("Mã")] public int MaSach { get; init; }
    [DisplayName("ISBN")] public string? ISBN { get; init; }
    [DisplayName("Tên sách")] public string TenSach { get; init; } = "";
    [DisplayName("Tác giả")] public string TacGia { get; init; } = "";
    [DisplayName("Danh mục")] public string DanhMuc { get; init; } = "";
    [DisplayName("Nhà xuất bản")] public string? NhaXuatBan { get; init; }
    [DisplayName("Năm XB")] public short? NamXuatBan { get; init; }
    [DisplayName("Đơn giá")] public decimal DonGia { get; init; }
    [DisplayName("Tồn kho")] public int SoLuongTon { get; init; }
    [Browsable(false)] public bool TrangThai { get; init; }
    [DisplayName("Trạng thái")] public string TrangThaiText => TrangThai ? "Đang bán" : "Ngừng bán";
}

public class SachLuuDto
{
    public int MaSach { get; set; }
    public string? ISBN { get; set; }
    public string TenSach { get; set; } = "";
    public int MaDanhMuc { get; set; }
    public int? MaNhaXuatBan { get; set; }
    public short? NamXuatBan { get; set; }
    public decimal DonGia { get; set; }
    public string? MoTa { get; set; }
    public string? HinhAnh { get; set; }
    public bool TrangThai { get; set; } = true;
    public List<int> MaTacGias { get; set; } = new();
    /// <summary>Chỉ để hiển thị: tồn kho chỉ thay đổi qua phiếu kho hoặc đơn hàng.</summary>
    public int SoLuongTon { get; set; }
}

// ---------------------------------- Khách hàng ----------------------------------

public class KhachHangQtDto
{
    [DisplayName("Mã")] public int MaKhachHang { get; init; }
    [DisplayName("Họ tên")] public string HoTen { get; init; } = "";
    [DisplayName("Số điện thoại")] public string SoDienThoai { get; init; } = "";
    [DisplayName("Email")] public string? Email { get; init; }
    [DisplayName("Địa chỉ")] public string? DiaChi { get; init; }
    [DisplayName("Tài khoản Web")] public string TaiKhoan { get; init; } = "";
    /// <summary>null: khách vãng lai (không có tài khoản Web); true: đang hoạt động; false: đã khóa.</summary>
    [Browsable(false)] public bool? TaiKhoanHoatDong { get; init; }
    [DisplayName("Trạng thái")] public string TrangThaiText => TaiKhoanHoatDong switch
    {
        null => "Vãng lai",
        true => "Hoạt động",
        false => "Đã khóa"
    };
    [DisplayName("Số đơn")] public int SoDonHang { get; init; }
    [DisplayName("Đã mua (đơn hoàn thành)")] public decimal TongChiTieu { get; init; }
}

public class KhachHangLuuDto
{
    public int MaKhachHang { get; set; }
    public string HoTen { get; set; } = "";
    public string SoDienThoai { get; set; } = "";
    public string? Email { get; set; }
    public string? DiaChi { get; set; }
}

// ---------------------------------- Đơn hàng ----------------------------------

public class LocDonHang
{
    /// <summary>Tìm theo mã đơn, tên/SĐT người nhận hoặc khách hàng.</summary>
    public string? TuKhoa { get; set; }
    public TrangThaiDonHang? TrangThai { get; set; }
    public DateTime? TuNgay { get; set; }
    public DateTime? DenNgay { get; set; }
    public int? MaKhachHang { get; set; }
}

public class DonHangQtDto
{
    [DisplayName("Mã đơn")] public long MaDonHang { get; init; }
    [DisplayName("Ngày đặt")] public DateTime NgayDat { get; init; }
    [DisplayName("Khách hàng")] public string KhachHang { get; init; } = "";
    [DisplayName("Người nhận")] public string HoTenNhan { get; init; } = "";
    [DisplayName("SĐT nhận")] public string SoDienThoaiNhan { get; init; } = "";
    [DisplayName("Tổng tiền")] public decimal TongTien { get; init; }
    [Browsable(false)] public TrangThaiDonHang TrangThai { get; init; }
    [DisplayName("Trạng thái")] public string TrangThaiText => TrangThai.Ten();
    [Browsable(false)] public TrangThaiThanhToan TrangThaiThanhToan { get; init; }
    [Browsable(false)] public string PhuongThucThanhToan { get; init; } = PhuongThuc.COD;
    [DisplayName("Thanh toán")] public string ThanhToanText => $"{PhuongThuc.TenNgan(PhuongThucThanhToan)} – {TrangThaiThanhToan.Ten()}";
}

// ------------------------------------- Kho -------------------------------------

public record DongPhieuKhoDto(int MaSach, int SoLuong, decimal? DonGiaNhap);

/// <summary>Dữ liệu lập phiếu kho (Bảng 4.6: KhoService.LuuPhieuAsync).</summary>
public class PhieuKhoDto
{
    public LoaiPhieuKho LoaiPhieu { get; set; }
    public string? GhiChu { get; set; }
    public List<DongPhieuKhoDto> Dong { get; set; } = new();
}

public class PhieuKhoQtDto
{
    [DisplayName("Mã phiếu")] public long MaPhieu { get; init; }
    [DisplayName("Ngày lập")] public DateTime NgayLap { get; init; }
    [Browsable(false)] public LoaiPhieuKho LoaiPhieu { get; init; }
    [DisplayName("Loại phiếu")] public string LoaiText => LoaiPhieu.Ten();
    [DisplayName("Người lập")] public string NguoiLap { get; init; } = "";
    [DisplayName("Số dòng")] public int SoDong { get; init; }
    [DisplayName("Tổng số lượng")] public int TongSoLuong { get; init; }
    [DisplayName("Ghi chú")] public string? GhiChu { get; init; }
}

public class ChiTietPhieuQtDto
{
    [DisplayName("Mã sách")] public int MaSach { get; init; }
    [DisplayName("Tên sách")] public string TenSach { get; init; } = "";
    [DisplayName("Số lượng")] public int SoLuong { get; init; }
    [DisplayName("Đơn giá nhập")] public decimal? DonGiaNhap { get; init; }
}

public class TonKhoDto
{
    [DisplayName("Mã")] public int MaSach { get; init; }
    [DisplayName("Tên sách")] public string TenSach { get; init; } = "";
    [DisplayName("Danh mục")] public string DanhMuc { get; init; } = "";
    [DisplayName("Tồn kho")] public int SoLuongTon { get; init; }
    [DisplayName("Tình trạng")] public string TinhTrang { get; init; } = "";
}

// ------------------------------------ Báo cáo ------------------------------------

public class DoanhThuNgayDto
{
    [DisplayName("Ngày")] public DateTime Ngay { get; init; }
    [DisplayName("Số đơn")] public int SoDon { get; init; }
    [DisplayName("Doanh thu")] public decimal DoanhThu { get; init; }
}

public class SachBanChayDto
{
    [DisplayName("Mã")] public int MaSach { get; init; }
    [DisplayName("Tên sách")] public string TenSach { get; init; } = "";
    [DisplayName("Số lượng bán")] public int SoLuongBan { get; init; }
    [DisplayName("Doanh thu")] public decimal DoanhThu { get; init; }
}

public class TongQuanDto
{
    public int DonChoXacNhan { get; init; }
    public int DonDangXuLy { get; init; }
    public decimal DoanhThuHomNay { get; init; }
    public decimal DoanhThuThangNay { get; init; }
    public int SachSapHet { get; init; }
    public int TongDauSach { get; init; }
    public int TongKhachHang { get; init; }
}
