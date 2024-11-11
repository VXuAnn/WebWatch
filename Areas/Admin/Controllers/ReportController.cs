using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController : Controller
    {
        private readonly AaashopContext db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public ReportController(AaashopContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            db = context;
            _environment = environment;

        }
        
        public IActionResult InComeByMonth()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetIncomeByMonth(int year)
        {
            var items = from order in db.HoaDons
                        where order.NgayDat.Year == year && order.NgayGiao != null
                        join detail in db.ChiTietHds
                        on order.MaHd equals detail.MaHd
                        select new
                        {
                            Month = order.NgayDat.Month,
                            Income = detail.SoLuong * detail.DonGia * 1.1
                        };

            var monthlyIncome = from item in items
                                group item by item.Month into monthlyGroup
                                select new
                                {
                                    Month = monthlyGroup.Key,
                                    Income = monthlyGroup.Sum(x => x.Income)
                                };

            var result = await monthlyIncome
                              .OrderBy(m => m.Month)
                              .ToListAsync();

            return Ok(result);
        }


    }
}
