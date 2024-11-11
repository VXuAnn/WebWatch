using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorizedController : Controller
    {
        private readonly AaashopContext db;
        public AuthorizedController(AaashopContext context)
        {
            db = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Save(Guid GroupId, Guid RoleId)
        {
            var item = await db.Authorizeds.Where(i => i.GroupId == GroupId && i.RoleId == RoleId).FirstOrDefaultAsync();
            if (item == null)
            {
                item = new Authorized()
                {
                    
                    Id = Guid.NewGuid(),
                    RoleId = RoleId,
                    GroupId = GroupId
                };
                db.Authorizeds.Add(item);
            }
            else
            {
                db.Remove(item);
            }
            await db.SaveChangesAsync();
            return Ok();
        }
        
    }
}
