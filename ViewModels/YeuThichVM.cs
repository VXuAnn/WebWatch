using ShopBanHang.Data;
namespace ShopBanHang.ViewModels
{
	public class YeuThichVM
	{
		public int MaHH { get; set; } // Product ID
		public string TenKh { get; set; } // Customer Name
		public string Email { get; set; } // Customer Email
		public string MoTa { get; set; } // Review text
		public DateTime NgayChon { get; set; } // Date of Review
		public int DanhGia { get; set; } // Rating
	}
	public class DanhGiaVM
	{
		public int MaYT { get; set; }

		public int MaHH { get; set; }

		public string TenKh { get; set; }

		public string Hinh { get; set; }

		public string MoTa { get; set; }

		public DateTime NgayChon { get; set; }

		public int DanhGia { get; set;}

        public string Email { get; set; }

    }
}



