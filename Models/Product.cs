namespace ShopBanHang.Models
{
    public class Product
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; }
        public IFormFile? Hinh { get; set; }
    /*    public byte[] Hinh { get; set; }*/
        public string TenAlias { get; set; }

        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }

        public int MaLoai { get; set; }

        public DateTime NgaySX { get; set; }
        public string TenCongTy { get; set; }
        public string MaNcc { get; set; }
    }
}
