using ShopBanHang.Data;

namespace ShopBanHang.ViewModels
{
    public class HangHoaVM
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; }
        public string Hinh { get; set; }


        public string TenAlias { get; set; }    

        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }

        public int MaLoai { get; set; }

        public string TenCongTy {  get; set; }
        public string MaNcc { get; set; }

        public ICollection<YeuThich> YeuThiches { get; set; }
    }
    public class ChiTietHangHoaVM
    {
        public int MaHh { get; set; }
        public string TenHH { get; set; }
        public string Hinh { get; set; }
        public double DonGia { get; set; }
        public string MoTaNgan { get; set; }
        public string TenLoai { get; set; }
        public string ChiTiet { get; set; }
        public int DiemDanhGia { get; set; }
        public int SoLuongTon { get; set; }
        public List<HangHoa> RelatedProducts { get; set; }

		public List<YeuThichVM> Reviews { get; set; } = new List<YeuThichVM>();
	}

    public class ProductCategory
    {
        public int MaLoai { get; set; }

        public string TenLoai { get; set; }

        public string TenLoaiAlias { get; set; }

        public string MoTa { get; set; }

    }

}
