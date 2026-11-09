namespace BookStore.Domain.Enums;

/// <summary>Tên tiếng Việt của các giá trị enum để hiển thị trên Web và Desktop.</summary>
public static class TenHienThi
{
    public static string Ten(this VaiTro v) => v switch
    {
        VaiTro.QuanTriVien => "Quản trị viên",
        VaiTro.NhanVienBanHang => "Nhân viên bán hàng",
        VaiTro.NhanVienKho => "Nhân viên kho",
        _ => "Khách hàng"
    };

    public static string Ten(this TrangThaiDonHang t) => t switch
    {
        TrangThaiDonHang.ChoXacNhan => "Chờ xác nhận",
        TrangThaiDonHang.DaXacNhan => "Đã xác nhận",
        TrangThaiDonHang.DangGiao => "Đang giao",
        TrangThaiDonHang.HoanThanh => "Hoàn thành",
        _ => "Đã hủy"
    };

    public static string Ten(this TrangThaiThanhToan t) =>
        t == TrangThaiThanhToan.DaThanhToan ? "Đã thanh toán" : "Chưa thanh toán";

    public static string Ten(this LoaiPhieuKho l) => l switch
    {
        LoaiPhieuKho.Nhap => "Nhập kho",
        LoaiPhieuKho.Xuat => "Xuất kho",
        LoaiPhieuKho.DieuChinhTang => "Điều chỉnh tăng",
        _ => "Điều chỉnh giảm"
    };

    /// <summary>Phiếu làm tăng tồn kho (Nhập, Điều chỉnh tăng) hay giảm (Xuất, Điều chỉnh giảm).</summary>
    public static bool LamTangTon(this LoaiPhieuKho l) => l is LoaiPhieuKho.Nhap or LoaiPhieuKho.DieuChinhTang;
}
