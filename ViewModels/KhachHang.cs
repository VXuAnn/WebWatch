using ShopBanHang.Data;
namespace ShopBanHang.ViewModels
{
    public class KhachHang
    {
        public string Id {  get; set; }   
        public string HoTen {  get; set; }

        public bool? GioiTinh { get; set; }

        public string  Email { get; set; }
        public string DienThoai { get; set; }
        public  int VaiTro { get; set; }
        public string DiaChi { get; set; }
        public string Hinh { get; set; }

		public ICollection<YeuThich> YeuThiches { get; set; }
	}
}
   