using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class KhoiTaoCSDL : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DANHMUC",
                columns: table => new
                {
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDanhMucCha = table.Column<int>(type: "int", nullable: true),
                    TenDanhMuc = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DANHMUC", x => x.MaDanhMuc);
                    table.ForeignKey(
                        name: "FK_DANHMUC_DANHMUC_MaDanhMucCha",
                        column: x => x.MaDanhMucCha,
                        principalTable: "DANHMUC",
                        principalColumn: "MaDanhMuc");
                });

            migrationBuilder.CreateTable(
                name: "NHAXUATBAN",
                columns: table => new
                {
                    MaNhaXuatBan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenNhaXuatBan = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NHAXUATBAN", x => x.MaNhaXuatBan);
                });

            migrationBuilder.CreateTable(
                name: "TACGIA",
                columns: table => new
                {
                    MaTacGia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTacGia = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TieuSu = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TACGIA", x => x.MaTacGia);
                });

            migrationBuilder.CreateTable(
                name: "TAIKHOAN",
                columns: table => new
                {
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDangNhap = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MatKhauHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    VaiTro = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "KhachHang"),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TAIKHOAN", x => x.MaTaiKhoan);
                    table.CheckConstraint("CK_TAIKHOAN_VaiTro", "[VaiTro] IN ('QuanTriVien','NhanVienBanHang','NhanVienKho','KhachHang')");
                });

            migrationBuilder.CreateTable(
                name: "SACH",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDanhMuc = table.Column<int>(type: "int", nullable: false),
                    MaNhaXuatBan = table.Column<int>(type: "int", nullable: true),
                    ISBN = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TenSach = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NamXuatBan = table.Column<short>(type: "smallint", nullable: true),
                    DonGia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoLuongTon = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TrangThai = table.Column<bool>(type: "bit", nullable: false),
                    NgayCapNhat = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSDATETIME()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SACH", x => x.MaSach);
                    table.CheckConstraint("CK_SACH_DonGia", "[DonGia] >= 0");
                    table.CheckConstraint("CK_SACH_SoLuongTon", "[SoLuongTon] >= 0");
                    table.ForeignKey(
                        name: "FK_SACH_DANHMUC_MaDanhMuc",
                        column: x => x.MaDanhMuc,
                        principalTable: "DANHMUC",
                        principalColumn: "MaDanhMuc");
                    table.ForeignKey(
                        name: "FK_SACH_NHAXUATBAN_MaNhaXuatBan",
                        column: x => x.MaNhaXuatBan,
                        principalTable: "NHAXUATBAN",
                        principalColumn: "MaNhaXuatBan");
                });

            migrationBuilder.CreateTable(
                name: "KHACHHANG",
                columns: table => new
                {
                    MaKhachHang = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: true),
                    HoTen = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: true),
                    SoDienThoai = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    DiaChi = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KHACHHANG", x => x.MaKhachHang);
                    table.ForeignKey(
                        name: "FK_KHACHHANG_TAIKHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MaTaiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "PHIEUKHO",
                columns: table => new
                {
                    MaPhieu = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTaiKhoan = table.Column<int>(type: "int", nullable: false),
                    LoaiPhieu = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    NgayLap = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PHIEUKHO", x => x.MaPhieu);
                    table.ForeignKey(
                        name: "FK_PHIEUKHO_TAIKHOAN_MaTaiKhoan",
                        column: x => x.MaTaiKhoan,
                        principalTable: "TAIKHOAN",
                        principalColumn: "MaTaiKhoan");
                });

            migrationBuilder.CreateTable(
                name: "SACHTACGIA",
                columns: table => new
                {
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    MaTacGia = table.Column<int>(type: "int", nullable: false),
                    VaiTroTacGia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SACHTACGIA", x => new { x.MaSach, x.MaTacGia });
                    table.ForeignKey(
                        name: "FK_SACHTACGIA_SACH_MaSach",
                        column: x => x.MaSach,
                        principalTable: "SACH",
                        principalColumn: "MaSach",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SACHTACGIA_TACGIA_MaTacGia",
                        column: x => x.MaTacGia,
                        principalTable: "TACGIA",
                        principalColumn: "MaTacGia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DONHANG",
                columns: table => new
                {
                    MaDonHang = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaKhachHang = table.Column<int>(type: "int", nullable: false),
                    NgayDat = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "SYSDATETIME()"),
                    NgayGiaoDuKien = table.Column<DateTime>(type: "date", nullable: true),
                    HoTenNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SoDienThoaiNhan = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: false),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    GhiChu = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TongTien = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TrangThaiDonHang = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ChoXacNhan"),
                    PhuongThucThanhToan = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "COD"),
                    TrangThaiThanhToan = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: false, defaultValue: "ChuaThanhToan")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DONHANG", x => x.MaDonHang);
                    table.CheckConstraint("CK_DONHANG_TongTien", "[TongTien] >= 0");
                    table.ForeignKey(
                        name: "FK_DONHANG_KHACHHANG_MaKhachHang",
                        column: x => x.MaKhachHang,
                        principalTable: "KHACHHANG",
                        principalColumn: "MaKhachHang");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETPHIEUKHO",
                columns: table => new
                {
                    MaPhieu = table.Column<long>(type: "bigint", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGiaNhap = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETPHIEUKHO", x => new { x.MaPhieu, x.MaSach });
                    table.CheckConstraint("CK_CTPK_DonGia", "[DonGiaNhap] >= 0");
                    table.CheckConstraint("CK_CTPK_SoLuong", "[SoLuong] > 0");
                    table.ForeignKey(
                        name: "FK_CHITIETPHIEUKHO_PHIEUKHO_MaPhieu",
                        column: x => x.MaPhieu,
                        principalTable: "PHIEUKHO",
                        principalColumn: "MaPhieu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHITIETPHIEUKHO_SACH_MaSach",
                        column: x => x.MaSach,
                        principalTable: "SACH",
                        principalColumn: "MaSach");
                });

            migrationBuilder.CreateTable(
                name: "CHITIETDONHANG",
                columns: table => new
                {
                    MaDonHang = table.Column<long>(type: "bigint", nullable: false),
                    MaSach = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGiaDat = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ThanhTien = table.Column<decimal>(type: "decimal(29,2)", nullable: false, computedColumnSql: "[SoLuong] * [DonGiaDat]", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CHITIETDONHANG", x => new { x.MaDonHang, x.MaSach });
                    table.CheckConstraint("CK_CTDH_DonGia", "[DonGiaDat] >= 0");
                    table.CheckConstraint("CK_CTDH_SoLuong", "[SoLuong] > 0");
                    table.ForeignKey(
                        name: "FK_CHITIETDONHANG_DONHANG_MaDonHang",
                        column: x => x.MaDonHang,
                        principalTable: "DONHANG",
                        principalColumn: "MaDonHang",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CHITIETDONHANG_SACH_MaSach",
                        column: x => x.MaSach,
                        principalTable: "SACH",
                        principalColumn: "MaSach");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETDONHANG_MaSach",
                table: "CHITIETDONHANG",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_CHITIETPHIEUKHO_MaSach",
                table: "CHITIETPHIEUKHO",
                column: "MaSach");

            migrationBuilder.CreateIndex(
                name: "IX_DANHMUC_MaDanhMucCha",
                table: "DANHMUC",
                column: "MaDanhMucCha");

            migrationBuilder.CreateIndex(
                name: "IX_DANHMUC_TenDanhMuc",
                table: "DANHMUC",
                column: "TenDanhMuc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DONHANG_MaKhachHang_NgayDat",
                table: "DONHANG",
                columns: new[] { "MaKhachHang", "NgayDat" });

            migrationBuilder.CreateIndex(
                name: "IX_KHACHHANG_MaTaiKhoan",
                table: "KHACHHANG",
                column: "MaTaiKhoan",
                unique: true,
                filter: "[MaTaiKhoan] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_NHAXUATBAN_TenNhaXuatBan",
                table: "NHAXUATBAN",
                column: "TenNhaXuatBan",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PHIEUKHO_MaTaiKhoan",
                table: "PHIEUKHO",
                column: "MaTaiKhoan");

            migrationBuilder.CreateIndex(
                name: "IX_SACH_ISBN",
                table: "SACH",
                column: "ISBN",
                unique: true,
                filter: "[ISBN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_SACH_MaDanhMuc",
                table: "SACH",
                column: "MaDanhMuc");

            migrationBuilder.CreateIndex(
                name: "IX_SACH_MaNhaXuatBan",
                table: "SACH",
                column: "MaNhaXuatBan");

            migrationBuilder.CreateIndex(
                name: "IX_SACH_TenSach",
                table: "SACH",
                column: "TenSach");

            migrationBuilder.CreateIndex(
                name: "IX_SACHTACGIA_MaTacGia",
                table: "SACHTACGIA",
                column: "MaTacGia");

            migrationBuilder.CreateIndex(
                name: "IX_TAIKHOAN_TenDangNhap",
                table: "TAIKHOAN",
                column: "TenDangNhap",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CHITIETDONHANG");

            migrationBuilder.DropTable(
                name: "CHITIETPHIEUKHO");

            migrationBuilder.DropTable(
                name: "SACHTACGIA");

            migrationBuilder.DropTable(
                name: "DONHANG");

            migrationBuilder.DropTable(
                name: "PHIEUKHO");

            migrationBuilder.DropTable(
                name: "SACH");

            migrationBuilder.DropTable(
                name: "TACGIA");

            migrationBuilder.DropTable(
                name: "KHACHHANG");

            migrationBuilder.DropTable(
                name: "DANHMUC");

            migrationBuilder.DropTable(
                name: "NHAXUATBAN");

            migrationBuilder.DropTable(
                name: "TAIKHOAN");
        }
    }
}
