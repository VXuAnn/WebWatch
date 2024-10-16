using Microsoft.AspNetCore.Mvc;
using ShopBanHang.ViewModels;
using ShopBanHang.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ShopBanHang.ViewComponents
{
	public class CartViewComponent : ViewComponent
    {
		public IViewComponentResult Invoke()
        {
			var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

            return View("CartPanel", new CartModel
            {
                Quantity = cart.Sum(p => p.SoLuong),
                Total = cart.Sum(p => p.ThanhTien)
            });
        }
    }
}
