using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;

using ShopBanHang.Areas.Admin.Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using ShopBanHang.Areas.Admin.Extensions;
using ShopBanHang.Areas.Admin.Attributes;
using Microsoft.AspNetCore.Authorization;


namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MemberController : Controller
    {
        private readonly AaashopContext db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public MemberController(AaashopContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            db = context;
            _environment = environment;

        }
        [Authorized(Code = "view-members")]
        [HttpPost]
        public async Task<IActionResult> getList(jDatatable model)
        {
            var items = (from i in db.NhanViens select i);
            int recordsTotal = 0;
            if (!string.IsNullOrEmpty(model.columns[model.order[0].column].name) && !string.IsNullOrEmpty(model.order[0].dir))
            {
                items = items.OrderBy(model.columns[model.order[0].column].name + ' ' + model.order[0].dir);
            }
            if (!string.IsNullOrEmpty(model.search.value))
            {
                items = items.Where(i => i.HoTen.Contains(model.search.value));
            }
            recordsTotal = items.Count();
            var data = await items.Select(i => new
            {
                i.MaNv,
                name=i.HoTen,
                groupName = i.Group.Name,
                i.LastLogin,
            }).Skip(model.start).Take(model.length).ToListAsync();
            var jsonData = new { draw = model.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        public IActionResult Index()
        {
            return View();
        }
        [Authorized(Code = "edit-member")]
        [HttpGet]
        public async Task<IActionResult> getItem(string id)
        {
            if (db.NhanViens== null)
                return NotFound();
            var item = await db.NhanViens.FindAsync(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [Authorized(Code = "save-member")]
        [HttpPost]
        public async Task<IActionResult> Save(MemberViewModel model)
        {
            NhanVien item;
            if (model.Id == null)
            {
                item = new NhanVien();
                item.MaNv = model.Id ?? Guid.NewGuid().ToString();
                await db.NhanViens.AddAsync(item);
            }
            else
            {
                item = await db.NhanViens.FirstOrDefaultAsync(nv => nv.MaNv == model.Id);
                if (item == null)
                {
                    return NotFound();
                }

            }
            item.HoTen = model.Name;
            item.LoginName = model.LoginName;
            if (!string.IsNullOrEmpty(model.Password))
            {
                item.Password = model.Password;
            }
            item.Email = model.Email;
            item.GroupId = model.GroupId;
            await db.SaveChangesAsync();
            return Ok(item);
        }

        [Authorized(Code = "delete-member")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await db.NhanViens.FindAsync(id);  // Tìm Member theo id
            if (item == null)
            {
                return NotFound();  // Nếu không tìm thấy Member thì trả về lỗi NotFound
            }

            db.NhanViens.Remove(item);  
            await db.SaveChangesAsync();  // Lưu thay đổi vào cơ sở dữ liệu

            return Ok(true);  // Trả về Ok nếu xóa thành công
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel item)
        {
         
            var member = db.NhanViens
                           .Where(i => i.LoginName == item.LoginName && i.Password == item.Password)
                           .FirstOrDefault();

            if (member != null)
            {
               
                member.LastLogin = DateTime.Now;

                
                db.SaveChanges();

                HttpContext.Session.SetObject("member", member);
                var codes = db.Authorizeds
                                      .Where(i => i.GroupId == member.GroupId)
                                      .Select(i => i.Role.Code)
                                      .ToList();
                HttpContext.Session.SetObject("codes", codes);
                return RedirectToAction("Index", "HomeAdmin");
            }

            return RedirectToAction("Login", "Member");
        }
     
        public IActionResult Logout()
        {
            HttpContext.Session.SetObject("member", null);
            return RedirectToAction("Index", "Home", new { area = "" });

        }
    }
}