using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;

namespace ShopBanHang.ViewComponents
{
	public class DanhGiaViewComponent : ViewComponent
	{
		private readonly AaashopContext db;
		public DanhGiaViewComponent(AaashopContext context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.YeuThiches.Select(lo => new DanhGiaVM
			{

				NgayChon = lo.NgayChon ?? DateTime.MinValue,
				MoTa = lo.MoTa,
				DanhGia = lo.DanhGia ?? 0,
				TenKh = lo.MaKhNavigation != null ? lo.MaKhNavigation.HoTen : "Unknown",
				Hinh =lo.MaKhNavigation.Hinh
			});

			return View(data);
		}
	}
}
