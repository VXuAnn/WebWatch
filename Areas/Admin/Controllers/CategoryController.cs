using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Areas.Admin.Models;
using System.Linq.Dynamic.Core;
using ShopBanHang.Data;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class CategoryController : Controller
    {
        private readonly AaashopContext db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public CategoryController(AaashopContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            db = context;
            _environment = environment;

        }
        public IActionResult Index(Guid? parentId)
        {
            return View(parentId);
        }
        [HttpPost]
        public async Task<IActionResult> getList(jDatatable model, Guid? parentId)
        {
            if (parentId == Guid.Empty)
                parentId = null;
            var items = (from i in db.Categories where i.ParentId == parentId select i);
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
                i.CategoryId,
                i.Name
            }).Skip(model.start).Take(model.length).ToListAsync();
            var jsonData = new { draw = model.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        [HttpGet]
        public async Task<IActionResult> getItem(Guid id)
        {
            if (db.Categories == null)
                return NotFound();
            var item = await db.Categories.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Save(CategoryViewModel model)
        {
            Category item;
            if (model.Id == null)
            {
                item = new Category();
                item.CategoryId = Guid.NewGuid();
                await db.Categories.AddAsync(item);
            }
            else
            {
                item = await db.Categories.FindAsync(model.Id);
            }
            item.Name = model.Name;
            if (model.ParentId == Guid.Empty)
                model.ParentId = null;
            item.ParentId = model.ParentId;
            await db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> GetChild(Guid parentId)
        {
            var items = from i in db.Categories where i.ParentId == parentId select i;
            var data = await items.Select(i => new { i.CategoryId, i.Name }).ToListAsync();
            return Ok(data);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryInProduct = await db.HangHoas.Where(m => m.CategoryId == id).FirstOrDefaultAsync();
            if (categoryInProduct != null) { return Ok(false); }
            var categoryInRole = await db.Roles.Where(m => m.CategoryId == id).FirstOrDefaultAsync();
            if (categoryInRole != null) { return Ok(false); }
            var item = await db.Categories.FindAsync(id);
            db.Entry(item).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Ok(true);
        }
    }
}
