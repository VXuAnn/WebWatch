using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;
using System.Linq;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.Extensions;


namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
  /*  [Route("admin/category")]*/
    public class ProductCategoryController : Controller
    {
        private readonly AaashopContext db;
        public ProductCategoryController(AaashopContext context)
        {
            db = context;
        }
        [Route("")] // Điều này cho phép /admin/sanpham
        [Route("Index")] // Điều này cho phép /admin/sanpham/Index
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Add()
        {

            return View();
        }
    }
}
