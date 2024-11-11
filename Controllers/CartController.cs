using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;
using ShopBanHang.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using System.Security.Claims;

namespace ShopBanHang.Controllers
{
	public class CartController : Controller
	{
		private readonly AaashopContext db;

		public CartController(AaashopContext context)
		{
			db = context;
		}

		/*public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();*/
		// Lấy giỏ hàng từ Session
		/*[Authorize]*/
		/*public IActionResult Index()
		{
			var userId = User.FindFirstValue(MySetting.CLAIM_CUSTOMERID);
			var cartItems = db.GioHangs
							  .Where(c => c.MaKh == userId)
							  .Select(c => new CartItemVM
							  {
								  Id = c.MaKH,
								  MaHh = c.MaHh,
								  Hinh = c.Hinh,
								  TenHH = c.TenHH,
								  DonGia = c.DonGia,
								  SoLuong = c.SoLuong,
								  UserId = c.UserId
							  }).ToList();

			return View(cartItems);
		}*/
		public List<CartItem> Cart
        {
            get
            {
                var cartJson = HttpContext.Session.GetString(MySetting.CART_KEY);
                return cartJson == null ? new List<CartItem>() : JsonSerializer.Deserialize<List<CartItem>>(cartJson);
            }
            set
            {
                var cartJson = JsonSerializer.Serialize(value);
                HttpContext.Session.SetString(MySetting.CART_KEY, cartJson);
            }
        }
		[Authorize]
		public IActionResult Index()
		{
			return View(Cart);
		}
		[Authorize]
		[HttpGet]
		public IActionResult AddToCart(int id, int quantity = 1)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item == null)
			{
				var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
				if (hangHoa == null)
				{
					TempData["Message"] = $"Không tìm thấy hàng hóa có mã {id}";
					return Redirect("/404");
				}
				item = new CartItem
				{
					MaHh = hangHoa.MaHh,
					TenHH = hangHoa.TenHh,
					DonGia = hangHoa.DonGia ?? 0,
					Hinh = hangHoa.Hinh ?? string.Empty,
					SoLuong = quantity
				};
				gioHang.Add(item);
			}
			else
			{
				item.SoLuong += quantity;
			}

            /*HttpContext.Session.Set(MySetting.CART_KEY, gioHang);*/
            Cart = gioHang;

            return RedirectToAction("Index");
		}
		[Authorize]
		[HttpGet]
		public IActionResult RemoveCart(int id)
		{
			var gioHang = Cart;
			var item = gioHang.SingleOrDefault(p => p.MaHh == id);
			if (item != null)
			{
				gioHang.Remove(item);
				/*HttpContext.Session.Set(MySetting.CART_KEY, gioHang);*/
			}
            Cart = gioHang;
            return RedirectToAction("Index");
		}
		[Authorize]
		[HttpGet]
		public IActionResult Checkout()
		{
			if (Cart.Count == 0)
			{
				return Redirect("/");
			}

			return View(Cart);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Checkout(CheckoutVM model)
		{
			if (ModelState.IsValid)
			{
				var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CLAIM_CUSTOMERID).Value;
				var khachHang = new Data.KhachHang();
				if (model.GiongKhachHang)
				{
					khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);
				}

				var hoadon = new HoaDon
				{
					MaKh = customerId,
					HoTen = model.HoTen ?? khachHang.HoTen,
					DiaChi = model.DiaChi ?? khachHang.DiaChi,
					DienThoai = model.DienThoai ?? khachHang.DienThoai,
					NgayDat = DateTime.Now,
					CachThanhToan = "COD",
					CachVanChuyen = "GRAB",
					MaTrangThai = 0,
					GhiChu = model.GhiChu
				};

				db.Database.BeginTransaction();
				try
				{
					db.Database.CommitTransaction();
					db.Add(hoadon);
					db.SaveChanges();

					var cthds = new List<ChiTietHd>();
					foreach (var item in Cart)
					{
						cthds.Add(new ChiTietHd
						{
							MaHd = hoadon.MaHd,
							SoLuong = item.SoLuong,
							DonGia = item.DonGia,
							MaHh = item.MaHh,
							GiamGia = 0
						});
					}
					db.AddRange(cthds);
					db.SaveChanges();

					HttpContext.Session.Set<List<CartItem>>(MySetting.CART_KEY, new List<CartItem>());

					return View("Success");
				}
				catch
				{
					db.Database.RollbackTransaction();
				}
			}

			return View(Cart);
		}
	}
}
