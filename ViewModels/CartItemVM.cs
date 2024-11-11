using ShopBanHang.Data;


namespace ShopBanHang.ViewModels
{
	public class CartItemVM
	{
		public int Id { get; set; }
		public int MaHh { get; set; }
		public string Hinh { get; set; }
		public string TenHH { get; set; }
		public double DonGia { get; set; }
		public int SoLuong { get; set; }
		public string UserId { get; set; }
	}
}
