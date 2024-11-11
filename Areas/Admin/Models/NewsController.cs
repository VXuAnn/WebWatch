using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Areas.Admin.Extensions;
using ShopBanHang.Data;
using System.Linq.Dynamic.Core;

namespace ShopBanHang.Areas.Admin.Models
{
    [Area("Admin")]
    public class NewsController : Controller
    {
       
        private readonly AaashopContext db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public NewsController(AaashopContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
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
            var items = (from i in db.Articles select i);
            int recordsTotal = 0;
            if (!string.IsNullOrEmpty(model.columns[model.order[0].column].name) && !string.IsNullOrEmpty(model.order[0].dir))
            {
                items = items.OrderBy(model.columns[model.order[0].column].name + ' ' + model.order[0].dir);
            }
            if (!string.IsNullOrEmpty(model.search.value))
            {
                items = items.Where(i => i.Title.Contains(model.search.value));
            }
            recordsTotal = items.Count();
            var data = await items.Select(i => new
            {
                i.ArticleId,
                i.Title,
                i.CreatedOn
            }).Skip(model.start).Take(model.length).ToListAsync();
            var jsonData = new { draw = model.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        [HttpGet]
        public async Task<IActionResult> getItem(Guid id)
        {
            if (db.Articles == null)
                return NotFound();
            var item = await db.Articles.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }
        [HttpPost]
        public async Task<IActionResult> Save(NewsViewModel model, IFormFile Picture)
        {
            Article item;
            var loggedMember = HttpContext.Session.GetObject<NhanVien>("member");
            if (model.Id == null)
            {
                item = new Article();
                item.ArticleId = Guid.NewGuid();
                item.CreatedBy = loggedMember.MaNv;
                item.CreatedOn = DateTime.Now;
                await db.Articles.AddAsync(item);
            }
            else
            {
                item = await db.Articles.FindAsync(model.Id);
                item.ModifiedOn = DateTime.Now;
                item.ModifiedBy = loggedMember.MaNv;
            }
            item.Title = model.Title;
            item.Content = model.Content;
            item.Keyword = model.Keyword;
            item.Description = model.Description;

            if (Picture != null)
            {
                var path = Path.Combine(this._environment.WebRootPath, "img/news/", Picture.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await Picture.CopyToAsync(stream);
                    stream.Close();
                }
                item.Picture = "/img/news/" + Picture.FileName;
            }
            await db.SaveChangesAsync();
            return Ok(item);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var item = await db.Articles.FindAsync(id);
                string path = this._environment.WebRootPath + item.Picture;
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
        [HttpPost]
        public async Task<IActionResult> UploadPicture(IFormFile Picture)
        {
            if (Picture != null)
            {
                var path = Path.Combine(this._environment.WebRootPath, "img/news/", Picture.FileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await Picture.CopyToAsync(stream);
                    stream.Close();
                }
                return Ok("/img/news/" + Picture.FileName);
            }
            return Ok("");
        }
    }
}
