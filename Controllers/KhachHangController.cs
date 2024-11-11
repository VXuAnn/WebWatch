using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;
using ShopBanHang.Helpers;
using ShopBanHang.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;



namespace ShopBanHang.Controllers
{
	public class KhachHangController : Controller
	{
		private readonly AaashopContext db;
		private readonly IMapper _mapper;

		public KhachHangController(AaashopContext context, IMapper mapper)
		{
			db = context;
			_mapper = mapper;
		}
		public IActionResult DangKy()
		{
			return View();
		}
		[HttpPost]
		public IActionResult DangKy(RegisterVM model, IFormFile Hinh)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var khachHang = _mapper.Map<Data.KhachHang>(model);
					khachHang.RandomKey = MyUtil.GenerateRamdomKey();
					khachHang.MatKhau = model.MatKhau.ToMd5Hash(khachHang.RandomKey);
					khachHang.HieuLuc = true;//sẽ xử lý khi dùng Mail để active
					khachHang.VaiTro = 0;

					if (Hinh != null)
					{
						khachHang.Hinh = MyUtil.UploadHinh(Hinh, "KhachHang");
					}

					db.Add(khachHang);
					db.SaveChanges();
					return RedirectToAction("Index", "HangHoa");
				}
				catch (Exception ex)
				{
					var mess = $"{ex.Message} shh";
				}
			}
			return View();
		}
		#region Login
		[HttpGet]
		public IActionResult DangNhap(string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnUrl)
		{
			ViewBag.ReturnUrl = ReturnUrl;
			if (ModelState.IsValid)
			{
				var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == model.UserName);
				if (khachHang == null)
				{
					ModelState.AddModelError("loi", "Không có khách hàng này");
				}
				else
				{
					if (!khachHang.HieuLuc)
					{
						ModelState.AddModelError("loi", "Tài khoản đã bị khóa. Vui lòng liên hệ Admin.");
					}
					else
					{
						if (khachHang.MatKhau != model.Password.ToMd5Hash(khachHang.RandomKey))
						{
							ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
						}
						else
						{
							var claims = new List<Claim> {
								new Claim(ClaimTypes.Email, khachHang.Email),
								new Claim(ClaimTypes.Name, khachHang.HoTen),
								new Claim(MySetting.CLAIM_CUSTOMERID, khachHang.MaKh),

								//claim - role động
								new Claim(ClaimTypes.Role, "Customer")
							};

							var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
							var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

							await HttpContext.SignInAsync(claimsPrincipal);

							if (Url.IsLocalUrl(ReturnUrl))
							{
								return Redirect(ReturnUrl);
							}
							else
							{
								return Redirect("/");
							}
						}
					}
				}
			}
			return View();
		
		} 
		#endregion

		[Authorize]
        public IActionResult Profile()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Bạn cần đăng nhập để xem trang hồ sơ.";
                return Redirect("/KhachHang/Dangnhap"); // Chuyển hướng đến trang đăng nhập
            }
            // Lấy mã khách hàng (id) từ thông tin của người dùng đã đăng nhập
            var userId = User.FindFirstValue(MySetting.CLAIM_CUSTOMERID); // Lấy ID người dùng từ Claim

			if (string.IsNullOrEmpty(userId))
			{
				TempData["Message"] = "Không tìm thấy thông tin tài khoản";
				return Redirect("/KhachHang/Dangnhap"); // Chuyển hướng đến trang đăng nhập nếu không tìm thấy người dùng
			}

			// Truy xuất thông tin khách hàng dựa vào id đã đăng nhập
			var data = db.KhachHangs.SingleOrDefault(p => p.MaKh== userId);

			// Kiểm tra xem khách hàng có tồn tại không
			if (data == null)
			{
				TempData["Message"] = "Không tìm thấy khách hàng";
				return Redirect("/404");
			}
			var result = new ShopBanHang.ViewModels.KhachHang
			{
                Id = data.MaKh,
                HoTen = data.HoTen,
				GioiTinh = data.GioiTinh,
                Email = data.Email ?? "Email chưa cập nhật", // Hiển thị thông báo mặc định nếu Email null
                DienThoai = data.DienThoai ?? "Số điện thoại chưa cập nhật", // Tương tự với số điện thoại
      
                DiaChi = data.DiaChi ?? "Địa chỉ chưa cập nhật", // Tương tự với địa chỉ
                Hinh = data.Hinh ?? string.Empty  // Kiểm tra nếu hình null thì dùng chuỗi rỗng
            };
         

            return View(result);
		}

		public IActionResult ThongTinTaiKhoan()
		{
            var userId = User.FindFirstValue(MySetting.CLAIM_CUSTOMERID);
            if (string.IsNullOrEmpty(userId))
            {
                // If no customer ID is found, redirect to login or handle accordingly
                TempData["Message"] = "Bạn cần đăng nhập.";
                return RedirectToAction("DangNhap", "KhachHang");
            }
			var data = (from kh in db.KhachHangs
						where kh.MaKh == userId
						select new ShopBanHang.ViewModels.KhachHang
						{
							HoTen = kh.HoTen,
							Email= kh.Email,
							DiaChi =kh.DiaChi,
							DienThoai=kh.DienThoai,
							Hinh=kh.Hinh
						}).FirstOrDefault();

            return PartialView("_ThongTinTaiKhoan",data);
        }

		public IActionResult ThongTinDonHang()
		{
			var userId = User.FindFirstValue(MySetting.CLAIM_CUSTOMERID);
			if (string.IsNullOrEmpty(userId))
			{
				// If no customer ID is found, redirect to login or handle accordingly
				TempData["Message"] = "Bạn cần đăng nhập để xem thông tin đơn hàng.";
				return RedirectToAction("DangNhap", "KhachHang");
			}
			var orders = (from hd in db.HoaDons
						  join kh in db.KhachHangs on hd.MaKh equals kh.MaKh
						  join th in db.TrangThais on hd.MaTrangThai equals th.MaTrangThai
						  join cthd in db.ChiTietHds on hd.MaHd equals cthd.MaHd
						  join hh in db.HangHoas on cthd.MaHh equals hh.MaHh
						  where kh.MaKh == userId
						  select new HoaDonVM
						  {
							  DonGia = (float)(hh.DonGia ?? 0),
							  Hinh = hh.Hinh ?? string.Empty,
							  SoLuong = cthd.SoLuong,
							  TenTrangThai = th.TenTrangThai,
							  TongTien = (float)(hh.DonGia * cthd.SoLuong ?? 0),
							  TenKH = kh.HoTen,
							  DienThoai = kh.DienThoai
						  }).ToList();

			return PartialView("_ThongTinDonHang", orders);
		}



		[Authorize]
		public async Task<IActionResult> DangXuat()
		{
			await HttpContext.SignOutAsync();
			return Redirect("/");
		}
	}
}

// 6LfQPW8qAAAAAORQiIql_HYqzW9k6Uq_eY6n749Q
// 6LfQPW8qAAAAADNMtPJLrOkaL5OKb4ro-0JLRNJW