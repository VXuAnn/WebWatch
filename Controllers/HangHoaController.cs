using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;
using System.Linq;
using System.Security.Claims;

namespace ShopBanHang.Controllers
{
    public class HangHoaController : Controller
    {
        private readonly AaashopContext db;
        public HangHoaController(AaashopContext context)
        {
            db= context;
        }
        public IActionResult Index(int? loai, int page = 1, int pageSize = 9)
        {
            // Tạo truy vấn cơ bản
            var hangHoas = db.HangHoas.AsQueryable();

            // Lọc theo loại sản phẩm nếu có
            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            // Lấy tổng số sản phẩm sau khi lọc
            var totalItems = hangHoas.Count();

            // Phân trang
            hangHoas = hangHoas
                        .OrderBy(p => p.MaHh)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);

            // Ánh xạ sang ViewModel
            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
               /* MoTaNgan = p.MoTaDonVi ?? "",*/
                TenLoai = p.MaLoaiNavigation.TenLoai,
            }).ToList();

            // Truyền số trang hiện tại và tổng số trang qua ViewData
            ViewData["TotalPages"] = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewData["CurrentPage"] = page;

            return View(result);
        }
        /*public IActionResult Index(int? loai)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (loai.HasValue)
            {
                hangHoas = hangHoas.Where(p => p.MaLoai == loai.Value);
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai,

            });
            return View(result);
        }*/

        /*public IActionResult Search(string? query)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            if (query != null)
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
                MoTaNgan = p.MoTaDonVi ?? "",
                TenLoai = p.MaLoaiNavigation.TenLoai,
                TenCongTy = p.MaNccNavigation.TenCongTy
            });
            return View(result);
        }*/
        public IActionResult Search(string? query, string? priceRange, string? gender, string? brand)
        {
            var hangHoas = db.HangHoas.AsQueryable();

            // Filter by query if provided
            if (!string.IsNullOrEmpty(query))
            {
                hangHoas = hangHoas.Where(p => p.TenHh.Contains(query));
            }

            // Filter by price range
            if (!string.IsNullOrEmpty(priceRange))
            {
                switch (priceRange)
                {
                    case "0 - 2Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia >= 0 && p.DonGia <= 2000000);
                        break;
                    case "2Tr - 4Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 2000000 && p.DonGia <= 4000000);
                        break;
                    case "4Tr - 6Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 4000000 && p.DonGia <= 6000000);
                        break;
                    case "6Tr - 8Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 6000000 && p.DonGia <= 8000000);
                        break;
                    case "8Tr - 10Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 8000000 && p.DonGia <= 10000000);
                        break;
                    case "10Tr - 20Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 10000000 && p.DonGia <= 20000000);
                        break;
                    case "20Tr - 40Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 20000000 && p.DonGia <= 40000000);
                        break;
                    case "40Tr - 100Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 40000000 && p.DonGia <= 100000000);
                        break;
                    case "+100Tr":
                        hangHoas = hangHoas.Where(p => p.DonGia > 100000000);
                        break;
                }
            }

            // Filter by gender
            if (!string.IsNullOrEmpty(gender))
            {
                // Giả sử "Nam", "Nữ" và "Unisex" là các giá trị bạn đang kiểm tra
                hangHoas = hangHoas.Join(db.Loais,
                                          hh => hh.MaLoai,
                                          loai => loai.MaLoai,
                                          (hh, loai) => new { hh, loai })
                                    .Where(x =>
                                        (gender == "Nam" && x.loai.TenLoai == "Nam") ||
                                        (gender == "Nữ" && x.loai.TenLoai == "Nữ") ||
                                        (gender == "Unisex" && x.loai.TenLoai == "Unisex"))
                                    .Select(x => x.hh);
            }
            // Filter by brand
            if (!string.IsNullOrEmpty(brand))
            {
                hangHoas = hangHoas.Where(p => p.MaNccNavigation.TenCongTy == brand);
            }

            var result = hangHoas.Select(p => new HangHoaVM
            {
                MaHh = p.MaHh,
                TenHH = p.TenHh,
                DonGia = p.DonGia ?? 0,
                Hinh = p.Hinh ?? "",
               /* MoTaNgan = p.MoTaDonVi ?? "",*/
                TenLoai = p.MaLoaiNavigation.TenLoai,
                TenCongTy = p.MaNccNavigation.TenCongTy
            });
            return View(result);
        }

        public IActionResult Detail(int id)
        {
            var data = db.HangHoas
				
				.Include(p => p.MaLoaiNavigation)
                .Include(p => p.MaNccNavigation)
				.SingleOrDefault(p => p.MaHh == id);
            if (data == null)
            {
                TempData["Message"] = $"Không thấy sản phẩm có mã {id}";
                return Redirect("/404");
            }
            var relatedProducts = db.HangHoas
           .Where(p => p.MaNcc == data.MaNcc && p.MaHh != id)
           .ToList();
            var result = new ChiTietHangHoaVM
            {
                MaHh = data.MaHh,
                TenHH = data.TenHh,
                DonGia = data.DonGia ?? 0,
                ChiTiet = data.MoTa ?? string.Empty,
                Hinh = data.Hinh ?? string.Empty,
                /*MoTaNgan = data.MoTaDonVi ?? string.Empty,*/
                TenLoai = data.MaLoaiNavigation.TenLoai,
                SoLuongTon = 10,
                DiemDanhGia = 5,
                RelatedProducts = relatedProducts,

            };
            return View(result);
        }
        [HttpPost]
        [Authorize] // Thêm thuộc tính này để yêu cầu người dùng phải đăng nhập
        public IActionResult PostReview(DanhGiaVM reviewVM)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (!User.Identity.IsAuthenticated)
            {
                // Nếu chưa đăng nhập, chuyển hướng đến trang đăng nhập
                return RedirectToAction("DangNhap", "KhachHang"); // Đảm bảo rằng bạn có controller và action Login
            }

            if (ModelState.IsValid)
            {
                var review = new YeuThich
                {
                    MaHh = reviewVM.MaHH,
                    MoTa = reviewVM.MoTa,
                    NgayChon = DateTime.Now,
                    DanhGia = reviewVM.DanhGia,
                    MaKh = User.FindFirstValue(ClaimTypes.NameIdentifier) // Lấy ID người dùng đã đăng nhập
                };

                db.YeuThiches.Add(review);
                db.SaveChanges();

                return RedirectToAction("DanhGia", new { productId = reviewVM.MaHH });
            }

            return View("Error"); // Hoặc hiển thị lỗi xác thực
        }




    }
}
