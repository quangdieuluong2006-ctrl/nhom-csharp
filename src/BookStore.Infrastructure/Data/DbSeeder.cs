using BookStore.Domain.Entities;
using BookStore.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Infrastructure.Data;

/// <summary>
/// Tạo dữ liệu mẫu khi CSDL còn trống (chỉ chạy một lần): danh mục, NXB, tác giả, sách,
/// tài khoản thử nghiệm (mỗi vai trò, 2 khách hàng) và phiếu nhập đầu kỳ.
/// Mật khẩu mẫu của mọi tài khoản: 123456 (được băm bằng BCrypt).
/// </summary>
public static class DbSeeder
{
    private record SachMau(string Ten, string[] TacGia, string DanhMuc, string Nxb, short Nam, decimal Gia,
        int Ton, string Isbn, string MoTa, string HinhAnh);

    public static async Task SeedAsync(BookStoreDbContext db)
    {
        if (await db.Sachs.AnyAsync()) return;

        // 1. Danh mục (2 cấp: cha trước, con sau)
        var dm = new Dictionary<string, DanhMuc>();
        foreach (var (ten, cha, moTa) in DanhMucMau)
        {
            var d = new DanhMuc { TenDanhMuc = ten, MoTa = moTa, DanhMucCha = cha is null ? null : dm[cha] };
            dm[ten] = d;
            db.DanhMucs.Add(d);
        }

        // 2. Nhà xuất bản, tác giả
        var nxb = NhaXuatBanMau.ToDictionary(t => t, t => new NhaXuatBan { TenNhaXuatBan = t });
        db.NhaXuatBans.AddRange(nxb.Values);
        var tg = TacGiaMau.ToDictionary(t => t, t => new TacGia { TenTacGia = t });
        db.TacGias.AddRange(tg.Values);

        // 3. Sách: NgayCapNhat lùi dần để trang chủ có thứ tự "sách mới" ổn định
        var now = DateTime.Now;
        for (int i = 0; i < SachMauList.Length; i++)
        {
            var m = SachMauList[i];
            var s = new Sach
            {
                TenSach = m.Ten, DanhMuc = dm[m.DanhMuc], NhaXuatBan = nxb[m.Nxb], NamXuatBan = m.Nam,
                DonGia = m.Gia, SoLuongTon = m.Ton, ISBN = m.Isbn, MoTa = m.MoTa, HinhAnh = m.HinhAnh,
                NgayCapNhat = now.AddDays(-i)
            };
            foreach (var t in m.TacGia) s.SachTacGias.Add(new SachTacGia { TacGia = tg[t], VaiTroTacGia = "Tác giả" });
            db.Sachs.Add(s);
        }

        // 4. Tài khoản thử nghiệm: mỗi vai trò nhân viên một tài khoản và hai khách hàng
        string hash = BCrypt.Net.BCrypt.HashPassword("123456");
        var admin = new TaiKhoan { TenDangNhap = "admin", MatKhauHash = hash, HoTen = "Quản trị viên", VaiTro = VaiTro.QuanTriVien };
        db.TaiKhoans.AddRange(admin,
            new TaiKhoan { TenDangNhap = "nvbanhang", MatKhauHash = hash, HoTen = "Nhân viên bán hàng", VaiTro = VaiTro.NhanVienBanHang },
            new TaiKhoan { TenDangNhap = "nvkho", MatKhauHash = hash, HoTen = "Nhân viên kho", VaiTro = VaiTro.NhanVienKho },
            TaoKhachHang("khachhang", hash, "Nguyễn Văn An", "an.nguyen@example.com", "0901234567",
                "273 An Dương Vương, Phường 3, Quận 5, TP.HCM"),
            TaoKhachHang("khachhang2", hash, "Trần Thị Bình", "binh.tran@example.com", "0912345678",
                "12 Nguyễn Trãi, Phường Bến Thành, Quận 1, TP.HCM"));

        // 5. Phiếu nhập đầu kỳ: tồn kho ban đầu của mọi sách đều có chứng từ (tồn chỉ thay đổi qua phiếu kho/đơn hàng)
        var phieu = new PhieuKho
        {
            TaiKhoan = admin, LoaiPhieu = LoaiPhieuKho.Nhap, NgayLap = now.AddDays(-SachMauList.Length - 1),
            GhiChu = "Nhập kho đầu kỳ (dữ liệu mẫu)"
        };
        foreach (var s in db.Sachs.Local.Where(s => s.SoLuongTon > 0))
            phieu.ChiTiets.Add(new ChiTietPhieuKho { Sach = s, SoLuong = s.SoLuongTon, DonGiaNhap = Math.Round(s.DonGia * 0.7m / 1000) * 1000 });
        db.PhieuKhos.Add(phieu);

        await db.SaveChangesAsync();
    }

    /// <summary>Khách hàng: thông tin liên hệ lưu ở KHACHHANG; TAIKHOAN chỉ giữ tên đăng nhập, mật khẩu, tên hiển thị.</summary>
    private static TaiKhoan TaoKhachHang(string ten, string hash, string hoTen, string email, string sdt, string diaChi) => new()
    {
        TenDangNhap = ten, MatKhauHash = hash, HoTen = hoTen, VaiTro = VaiTro.KhachHang,
        KhachHang = new KhachHang { HoTen = hoTen, Email = email, SoDienThoai = sdt, DiaChi = diaChi }
    };

    private static readonly (string Ten, string? Cha, string? MoTa)[] DanhMucMau =
    {
        ("Văn học", null, "Tác phẩm văn học trong và ngoài nước"),
        ("Tiểu thuyết", "Văn học", null),
        ("Truyện ngắn", "Văn học", null),
        ("Thiếu nhi", null, "Sách dành cho thiếu nhi"),
        ("Văn học thiếu nhi", "Thiếu nhi", null),
        ("Truyện tranh", "Thiếu nhi", null),
        ("Kinh tế", null, "Kinh doanh, tài chính, quản trị"),
        ("Kỹ năng sống", null, "Phát triển bản thân"),
        ("Công nghệ thông tin", null, "Sách chuyên ngành CNTT"),
        ("Lập trình", "Công nghệ thông tin", null),
        ("Cơ sở dữ liệu", "Công nghệ thông tin", null),
        ("Ngoại ngữ", null, "Sách học ngoại ngữ")
    };

    private static readonly string[] NhaXuatBanMau =
    {
        "NXB Trẻ",
        "NXB Kim Đồng",
        "NXB Văn Học",
        "NXB Hội Nhà Văn",
        "NXB Tổng hợp TP.HCM",
        "NXB Thế Giới",
        "NXB Thông tin và Truyền thông",
        "NXB Đại học Quốc gia TP.HCM"
    };

    private static readonly string[] TacGiaMau =
    {
        "Antoine de Saint-Exupéry",
        "Dale Carnegie",
        "Eric Ries",
        "Fujiko F. Fujio",
        "Lê Hoàng Nam",
        "Lê Linh",
        "Napoleon Hill",
        "Nguyễn Nhật Ánh",
        "Nguyễn Văn Hiệp",
        "Ngô Tất Tố",
        "Paulo Coelho",
        "Phạm Minh Tuấn",
        "Raymond Murphy",
        "Robert C. Martin",
        "Robert T. Kiyosaki",
        "Robin Sharma",
        "Rosie Nguyễn",
        "Trần Thu Hà",
        "Tô Hoài",
        "Vũ Trọng Phụng"
    };

    private static readonly SachMau[] SachMauList =
    {
        new("Dế Mèn Phiêu Lưu Ký", new[] { "Tô Hoài" }, "Văn học thiếu nhi", "NXB Kim Đồng", 2020, 45000m, 120, "9786042088001",
            "Tác phẩm kinh điển kể về cuộc phiêu lưu của chú Dế Mèn qua nhiều vùng đất, học được bài học về lòng dũng cảm và tình bạn.", "/images/sach/de-men-phieu-luu-ky.svg"),
        new("Tôi Thấy Hoa Vàng Trên Cỏ Xanh", new[] { "Nguyễn Nhật Ánh" }, "Tiểu thuyết", "NXB Trẻ", 2018, 125000m, 80, "9786041067442",
            "Câu chuyện tuổi thơ ở một làng quê nghèo với những rung động đầu đời trong trẻo.", "/images/sach/toi-thay-hoa-vang-tren-co-xanh.svg"),
        new("Mắt Biếc", new[] { "Nguyễn Nhật Ánh" }, "Tiểu thuyết", "NXB Trẻ", 2019, 110000m, 65, "9786041082391",
            "Mối tình đơn phương của Ngạn dành cho Hà Lan, cô bạn có đôi mắt biếc.", "/images/sach/mat-biec.svg"),
        new("Cho Tôi Xin Một Vé Đi Tuổi Thơ", new[] { "Nguyễn Nhật Ánh" }, "Truyện ngắn", "NXB Trẻ", 2018, 85000m, 90, "9786041043682",
            "Những trang viết hóm hỉnh đưa người đọc trở về thế giới tuổi thơ.", "/images/sach/cho-toi-xin-mot-ve-di-tuoi-tho.svg"),
        new("Số Đỏ", new[] { "Vũ Trọng Phụng" }, "Tiểu thuyết", "NXB Văn Học", 2017, 78000m, 40, "9786049691327",
            "Tiểu thuyết trào phúng nổi tiếng về xã hội thành thị Việt Nam thời Pháp thuộc.", "/images/sach/so-do.svg"),
        new("Tắt Đèn", new[] { "Ngô Tất Tố" }, "Tiểu thuyết", "NXB Văn Học", 2016, 62000m, 35, "9786049542117",
            "Bức tranh chân thực về cuộc sống người nông dân trước Cách mạng tháng Tám.", "/images/sach/tat-den.svg"),
        new("Nhà Giả Kim", new[] { "Paulo Coelho" }, "Tiểu thuyết", "NXB Hội Nhà Văn", 2020, 79000m, 150, "9786043075817",
            "Hành trình theo đuổi giấc mơ của chàng chăn cừu Santiago.", "/images/sach/nha-gia-kim.svg"),
        new("Đắc Nhân Tâm", new[] { "Dale Carnegie" }, "Kỹ năng sống", "NXB Tổng hợp TP.HCM", 2021, 86000m, 200, "9786045874458",
            "Nghệ thuật thu phục lòng người, cuốn sách kỹ năng giao tiếp kinh điển.", "/images/sach/dac-nhan-tam.svg"),
        new("Đời Ngắn Đừng Ngủ Dài", new[] { "Robin Sharma" }, "Kỹ năng sống", "NXB Trẻ", 2019, 72000m, 70, "9786041124985",
            "Những lời khuyên giản dị giúp sống tích cực và hiệu quả hơn mỗi ngày.", "/images/sach/doi-ngan-dung-ngu-dai.svg"),
        new("Tuổi Trẻ Đáng Giá Bao Nhiêu", new[] { "Rosie Nguyễn" }, "Kỹ năng sống", "NXB Hội Nhà Văn", 2018, 90000m, 110, "9786049447214",
            "Cuốn sách truyền cảm hứng về học tập, làm việc và trải nghiệm khi còn trẻ.", "/images/sach/tuoi-tre-dang-gia-bao-nhieu.svg"),
        new("Nghĩ Giàu Làm Giàu", new[] { "Napoleon Hill" }, "Kinh tế", "NXB Tổng hợp TP.HCM", 2020, 98000m, 60, "9786045888516",
            "Mười ba nguyên tắc thành công được đúc kết từ những người giàu có nhất thế kỷ 20.", "/images/sach/nghi-giau-lam-giau.svg"),
        new("Cha Giàu Cha Nghèo", new[] { "Robert T. Kiyosaki" }, "Kinh tế", "NXB Trẻ", 2021, 105000m, 95, "9786041185672",
            "Bài học về tư duy tài chính cá nhân qua câu chuyện hai người cha.", "/images/sach/cha-giau-cha-ngheo.svg"),
        new("Khởi Nghiệp Tinh Gọn", new[] { "Eric Ries" }, "Kinh tế", "NXB Thế Giới", 2019, 139000m, 30, "9786047758812",
            "Phương pháp xây dựng doanh nghiệp khởi nghiệp dựa trên thử nghiệm và học hỏi nhanh.", "/images/sach/khoi-nghiep-tinh-gon.svg"),
        new("Lập Trình C# Căn Bản", new[] { "Phạm Minh Tuấn" }, "Lập trình", "NXB Thông tin và Truyền thông", 2023, 150000m, 45, "9786048052129",
            "Giáo trình nhập môn ngôn ngữ C# và lập trình hướng đối tượng trên nền tảng .NET.", "/images/sach/lap-trinh-c-can-ban.svg"),
        new("ASP.NET Core MVC Thực Hành", new[] { "Phạm Minh Tuấn", "Lê Hoàng Nam" }, "Lập trình", "NXB Thông tin và Truyền thông", 2024, 185000m, 25, "9786048052136",
            "Xây dựng ứng dụng web theo mô hình MVC với ASP.NET Core và Entity Framework Core.", "/images/sach/asp-net-core-mvc-thuc-hanh.svg"),
        new("Cấu Trúc Dữ Liệu Và Giải Thuật", new[] { "Lê Hoàng Nam" }, "Lập trình", "NXB Đại học Quốc gia TP.HCM", 2022, 120000m, 50, "9786047385124",
            "Các cấu trúc dữ liệu cơ bản, kỹ thuật phân tích độ phức tạp và thiết kế giải thuật.", "/images/sach/cau-truc-du-lieu-va-giai-thuat.svg"),
        new("Cơ Sở Dữ Liệu SQL Server", new[] { "Trần Thu Hà" }, "Cơ sở dữ liệu", "NXB Đại học Quốc gia TP.HCM", 2022, 135000m, 40, "9786047385131",
            "Thiết kế CSDL quan hệ, ngôn ngữ T-SQL, chỉ mục, giao tác và thủ tục lưu trữ.", "/images/sach/co-so-du-lieu-sql-server.svg"),
        new("Clean Code", new[] { "Robert C. Martin" }, "Lập trình", "NXB Thế Giới", 2021, 245000m, 20, "9786047789878",
            "Những nguyên tắc viết mã sạch, dễ đọc và dễ bảo trì dành cho lập trình viên.", "/images/sach/clean-code.svg"),
        new("Doraemon Tập 1", new[] { "Fujiko F. Fujio" }, "Truyện tranh", "NXB Kim Đồng", 2022, 25000m, 300, "9786042127717",
            "Chú mèo máy đến từ thế kỷ 22 cùng những bảo bối thần kỳ.", "/images/sach/doraemon-tap-1.svg"),
        new("Thần Đồng Đất Việt Tập 1", new[] { "Lê Linh" }, "Truyện tranh", "NXB Trẻ", 2021, 22000m, 180, "9786041171125",
            "Những câu chuyện dí dỏm về Trạng Tí và các bạn.", "/images/sach/than-dong-dat-viet-tap-1.svg"),
        new("Kính Vạn Hoa Tập 1", new[] { "Nguyễn Nhật Ánh" }, "Văn học thiếu nhi", "NXB Kim Đồng", 2019, 55000m, 75, "9786042147302",
            "Những câu chuyện học trò vui nhộn của nhóm bạn Quý ròm, Tiểu Long và Nhỏ Hạnh.", "/images/sach/kinh-van-hoa-tap-1.svg"),
        new("Hoàng Tử Bé", new[] { "Antoine de Saint-Exupéry" }, "Văn học thiếu nhi", "NXB Hội Nhà Văn", 2020, 68000m, 85, "9786043102308",
            "Câu chuyện triết lý nhẹ nhàng về tình yêu, tình bạn và cách nhìn thế giới.", "/images/sach/hoang-tu-be.svg"),
        new("English Grammar In Use", new[] { "Raymond Murphy" }, "Ngoại ngữ", "NXB Tổng hợp TP.HCM", 2019, 175000m, 55, "9786045869805",
            "Sách ngữ pháp tiếng Anh tự học thông dụng, có bài tập và đáp án.", "/images/sach/english-grammar-in-use.svg"),
        new("Hack Não 1500 Từ Tiếng Anh", new[] { "Nguyễn Văn Hiệp" }, "Ngoại ngữ", "NXB Thế Giới", 2020, 149000m, 0, "9786047766275",
            "Phương pháp ghi nhớ từ vựng tiếng Anh bằng âm thanh tương tự và hình ảnh.", "/images/sach/hack-nao-1500-tu-tieng-anh.svg")
    };
}
