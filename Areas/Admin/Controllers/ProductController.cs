using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Areas.Admin.Extensions;
using ShopBanHang.Areas.Admin.Models;
using ShopBanHang.Data;
using System.Linq.Dynamic.Core;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AaashopContext db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public ProductController(AaashopContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            db = context;
            _environment = environment;

        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> getList(jDatatable model)
        {
            var items = (from i in db.HangHoas select i);
            int recordsTotal = 0;
            if (!string.IsNullOrEmpty(model.columns[model.order[0].column].name) && !string.IsNullOrEmpty(model.order[0].dir))
            {
                items = items.OrderBy(model.columns[model.order[0].column].name + ' ' + model.order[0].dir);
            }
            if (!string.IsNullOrEmpty(model.search.value))
            {
                items = items.Where(i => i.TenHh.Contains(model.search.value));
            }
            recordsTotal = items.Count();
            var data = await items.Select(i => new
            {
                i.MaHh,
                tenhh=i.TenHh,
                categoryName = i.Category.Name,
                donGia = i.DonGia,
                hinh=i.Hinh,

            }).Skip(model.start).Take(model.length).ToListAsync();
            var jsonData = new { draw = model.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        [HttpGet]
        public async Task<IActionResult> getItem(int id)
        {
            if (db.HangHoas == null)
                return NotFound();
            var item = await db.HangHoas.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Save(ProductViewModel model, IFormFile Picture)
        {
            HangHoa item;
            var loggedMember = HttpContext.Session.GetObject<NhanVien>("member");
            if (model.Id == null)
            {
                item = new HangHoa();
               
                item.CreatedBy = loggedMember.MaNv;
                item.CreatedOn = DateTime.Now;
                await db.HangHoas.AddAsync(item);
            }
            else
            {
                item = await db.HangHoas.FindAsync(model.Id);
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = loggedMember.MaNv;
            }
            item.TenHh = model.Tenhh;
            item.MoTa = model.Mota;
           
            item.DonGia = model.Price;

            if (Picture != null)
            {
                var path = Path.Combine(this._environment.WebRootPath, "/Hinh/HangHoa/", Picture.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await Picture.CopyToAsync(stream);
                    stream.Close();
                }
                item.Hinh = "/Hinh/HangHoa/" + Picture.FileName;
            }
            item.CategoryId = model.CategoryId;
            await db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var item = await db.HangHoas.FindAsync(id);
                if (item == null)
                {
                    return NotFound("Item not found.");
                }
                string path = this._environment.WebRootPath + item.Hinh;
                db.Entry(item).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                return Ok(true);
            }
            catch
            {
                return Ok(false);
            }
        }
    }
}
