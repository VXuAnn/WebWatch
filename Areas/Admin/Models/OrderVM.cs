namespace ShopBanHang.Areas.Admin.Models
{
    public class OrderVM
    {
        public int Mahd { get; set; }
        public int MaHh { get; set; }
        public string TenHH { get; set; }
        public string Hinh { get; set; }
        public string DienThoai { get; set; }

        public string DiaChi { get; set; }
        public double DonGia { get; set; }

        public DateTime? NgayDat { get; set; }

        public DateTime? NgayGiao { get; set; }

        public string HoTen { get; set; }

        public string Email { get; set; }

        public int SoLuong { get; set; }
    }
}
