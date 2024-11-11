using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
       
        private readonly AaashopContext db;
        public RoleController(AaashopContext context)
        {
            db = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAllByCategory(Guid CategoryId, Guid GroupId)
        {
            var items = await (from r in db.Roles.Where(i => i.CategoryId == CategoryId)
                               join a in db.Authorizeds.Where(i => i.GroupId == GroupId)
                               on r.RoleId equals a.RoleId into g
                               from h in g.DefaultIfEmpty()
                               select new
                               {
                                   r.RoleId,
                                   r.Name,
                                   h.GroupId
                               }).ToListAsync();
            return Ok(items);
        }




    }
}
