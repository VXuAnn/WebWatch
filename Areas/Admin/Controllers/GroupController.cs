using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Areas.Admin.Models;
using System.Linq.Dynamic.Core;
using ShopBanHang.Data;
using Microsoft.AspNetCore.Authorization;
using ShopBanHang.Areas.Admin.Attributes;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GroupController : Controller
    {
        private readonly AaashopContext db;
        public GroupController(AaashopContext context)
        {
            db = context;
           
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorized(Code = "view-groups")]
        [HttpPost]
        public async Task<IActionResult> getList(jDatatable model)
        {
            var items = (from i in db.NhomQuyens select i);
            int recordsTotal = 0;
            if (!string.IsNullOrEmpty(model.columns[model.order[0].column].name) && !string.IsNullOrEmpty(model.order[0].dir))
            {
                items = items.OrderBy(model.columns[model.order[0].column].name + ' ' + model.order[0].dir);

            }
            if (!string.IsNullOrEmpty(model.search.value))
            {
                items = items.Where(i => i.Name.Contains(model.search.value));
            }
            recordsTotal = items.Count();
            
            var data = await items.Select(i => new
            {
                GroupId = i.GroupId, 
                Name = i.Name
            }).Skip(model.start).Take(model.length).ToListAsync();
            Console.WriteLine("Dữ liệu trả về:");
            foreach (var item in data)
            {
                Console.WriteLine($"GroupId: {item.GroupId}, Name: {item.Name}");
            }
            var jsonData = new { draw = model.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }

        [HttpGet]
        public async Task<IActionResult> getList()
        {
            var items = (from i in db.NhomQuyens select i);
            var data = await items.Select(i => new { i.GroupId, i.Name }).ToListAsync();
            return Ok(data);
        }
        [Authorized(Code = "edit-group")]
        [HttpGet]
        public async Task<IActionResult> getItem(Guid id)
        {
            if (db.NhomQuyens == null)
                return NotFound();
            var item = await db.NhomQuyens.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [Authorized(Code = "save-group")]

        [HttpPost]
        public async Task<IActionResult> Save(GroupViewModel model)
        {
            NhomQuyen item;
            if (model.Id == null)
            {
                
                item = new NhomQuyen();
                item.GroupId = Guid.NewGuid();
                await db.NhomQuyens.AddAsync(item);
            }
            else
            {
                
                item = await db.NhomQuyens.FindAsync(model.Id);
                if (item == null)
                {
                    return NotFound(); 
                }
            }

           
            item.Name = model.Name;

           
            await db.SaveChangesAsync();
            return Ok(item);
        }
       /* [Authorized(Code = "delete-group")]*/
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var memberInGroup = await db.NhanViens.Where(m => m.GroupId == id).FirstOrDefaultAsync();
            if (memberInGroup == null)
            {
                var item = await db.NhomQuyens.FindAsync(id);
                db.Entry(item).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return Ok(true);
            }
            return Ok(false);
        }



    }
}
