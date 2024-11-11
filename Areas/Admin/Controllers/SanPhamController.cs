/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;
using ShopBanHang.Models;
using System.Linq;
using System.Security.Claims;
using X.PagedList;
using System;
using X.PagedList.Extensions;
using ShopBanHang.Areas.Admin.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin/sanpham")]
    public class SanPhamController : Controller 
    {
        private readonly AaashopContext db;
        private readonly IWebHostEnvironment environment;

        public SanPhamController(AaashopContext context,IWebHostEnvironment environment)
        {
            db = context;
            this.environment = environment;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page ?? 1;
            var lstSanPham = db.HangHoas.AsNoTracking().OrderBy(x => x.MaHh);
            PagedList<HangHoa> lst = new PagedList<HangHoa>(lstSanPham, pageNumber, pageSize);
            return View(lst);
        }

        [Route("Add")]
        public ActionResult Add()
        {
            var productCategories = db.Loais.ToList();  // Giả sử bạn lấy danh sách danh mục từ bảng Loais
            var suppliers = db.NhaCungCaps.ToList();

            ViewBag.ProductCategory = new SelectList(db.Loais, "MaLoai", "TenLoai");
            ViewBag.Supplier = new SelectList(db.NhaCungCaps, "MaNcc", "TenCongTy");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Add")]
        public async Task<IActionResult> Add(Product model)
        {
            // Lấy danh sách các danh mục sản phẩm từ cơ sở dữ liệu
            var productCategories = db.Loais.ToList();  // Giả sử bạn lấy danh sách danh mục từ bảng Loais
            var suppliers = db.NhaCungCaps.ToList();    // Giả sử bạn lấy danh sách nhà cung cấp từ bảng NhaCungCaps

            // Tạo SelectList cho Danh mục sản phẩm và Nhà cung cấp
            ViewBag.ProductCategory = new SelectList(productCategories, "MaLoai", "TenLoai");
            ViewBag.Supplier = new SelectList(suppliers, "MaNcc", "TenCongTy");
            if (model.Hinh == null)
            {
                ModelState.AddModelError("ImageFile", "The image file is required");
            }
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            // save image file
            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(model.Hinh.FileName);

            string imageFullPath = environment.WebRootPath +"/products" + newFileName;

            using (var stream= System.IO.File.Create(imageFullPath))
            {
                model.Hinh.CopyTo(stream);
            }

            //save to db
            HangHoa hangHoa = new HangHoa()
            {
                TenHh = model.TenHH,
                TenAlias = model.TenAlias,
                DonGia = model.DonGia,
                MoTa = model.MoTaNgan,
                *//*TenCongTy = model.TenCongTy,
                TenLoai = model.TenLoai,*//*
                Hinh = newFileName,
                NgaySx = DateTime.Now,
                MaLoai = model.MaLoai,  // giả sử MaLoai tồn tại trong HangHoa
                MaNcc = model.MaNcc     // giả sử MaNcc tồn tại trong HangHoa
            };
            db.HangHoas.Add(hangHoa);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Sản phẩm đã được thêm thành công!";
            return RedirectToAction("Index","SanPham");
        }
        [HttpGet]

        [Route("Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var item = db.HangHoas.Find(id);
            if (item == null)

            {
                return RedirectToAction("Index", "SanPham");
            }

            var model = new Product
            {
                TenHH =item.TenHh,
                TenAlias = item.TenAlias,
                DonGia = item.DonGia.HasValue ? item.DonGia.Value : 0,
                MoTaNgan = item.MoTa,
 
                NgaySX = DateTime.Now,
                MaLoai = item.MaLoai,  
                MaNcc = item.MaNcc     
            };

            ViewData["ImageFileName"] = item.Hinh;
            var productCategories = db.Loais.ToList();  // Giả sử bạn lấy danh sách danh mục từ bảng Loais
            var suppliers = db.NhaCungCaps.ToList();

            ViewBag.ProductCategory = new SelectList(db.Loais, "MaLoai", "TenLoai");
            ViewBag.Supplier = new SelectList(db.NhaCungCaps, "MaNcc", "TenCongTy");

            return View(model);
        }

        // POST: Edit product
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Edit")]
        public IActionResult Edit(HangHoaVM model)
        {
            if (ModelState.IsValid)
            {
                var item = db.HangHoas.Find(model.MaHh);
                if (item != null)
                {
                    item.TenHh = model.TenHH;
                    item.TenAlias = model.TenAlias;
                    item.MaLoai = model.MaLoai;
                    item.MoTa = model.MoTaNgan;
                    item.DonGia = model.DonGia;
                   

                    db.SaveChanges(); 
                    return RedirectToAction("Index");
                }
            }

            ViewBag.ProductCategory = new SelectList(db.Loais, "MaLoai", "TenLoai", model.MaLoai);
            return View(model);
        }
        [HttpPost]
        [Route("Delete")]
        public IActionResult Delete(int id)
        {
            var product = db.HangHoas.Find(id);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }
            Console.WriteLine($"Product to be deleted: ID = {product.MaHh}, Name = {product.TenHh}, Price = {product.DonGia}");
            string imageFullPath = environment.WebRootPath + "/Hinh" +product.Hinh;
            System.IO.File.Delete(imageFullPath);

            db.HangHoas.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Index", "SanPham");
        }
        [HttpPost]
        [Route("DeleteAll")]
        public IActionResult DeleteAll(string ids)
        {
            if (string.IsNullOrEmpty(ids))
            {
                return Json(new { success = false, message = "No product selected." });
            }

            // Chuyển chuỗi ID thành một mảng các ID
            var productIds = ids.Split(',').Select(int.Parse).ToList();

            var products = db.HangHoas.Where(p => productIds.Contains(p.MaHh)).ToList();

            if (products.Any())
            {
                foreach (var product in products)
                {
                    // Xóa hình ảnh liên quan
                    string imageFullPath = Path.Combine(environment.WebRootPath, "Hinh", product.Hinh);
                    if (System.IO.File.Exists(imageFullPath))
                    {
                        System.IO.File.Delete(imageFullPath);
                    }

                    // Xóa sản phẩm khỏi database
                    db.HangHoas.Remove(product);
                }

                db.SaveChanges();

                return Json(new { success = true, message = "Products deleted successfully." });
            }

            return Json(new { success = false, message = "No products found to delete." });
        }



    }
}

*/