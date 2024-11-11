using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Areas.Admin.Models;
using ShopBanHang.Data;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace ShopBanHang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
       
        private readonly AaashopContext db;
        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        public OrderController(AaashopContext context, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment)
        {
            db = context;
            _environment = environment;

        }
        public IActionResult Index()
        {
            return View();
        }
        /*public IActionResult Details(int id)
        {
            var data = (from hd in db.HoaDons
                        join kh in db.KhachHangs on hd.MaKh equals kh.MaKh
                        join cthd in db.ChiTietHds on hd.MaHd equals cthd.MaHd
                        join hh in db.HangHoas on cthd.MaHh equals hh.MaHh
                        where hd.MaHd == id
                        select new OrderVM
                        {
                            Mahd = hd.MaHd,
                            MaHh = cthd.MaHh,
                            TenHH = hh.TenHh,
                            Hinh = hh.Hinh,
                            DienThoai = kh.DienThoai,
                            DiaChi = kh.DiaChi,
                            DonGia = cthd.DonGia,
                            NgayDat = hd.NgayDat,         
                            NgayGiao = hd.NgayGiao,
                            HoTen = kh.HoTen,
                            Email = kh.Email,
                        }).FirstOrDefault();

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }*/
        public IActionResult Details(int id)
        {
            var data = (from hd in db.HoaDons
                        join kh in db.KhachHangs on hd.MaKh equals kh.MaKh
                        join cthd in db.ChiTietHds on hd.MaHd equals cthd.MaHd
                        join hh in db.HangHoas on cthd.MaHh equals hh.MaHh
                        where hd.MaHd == id
                        select new OrderVM
                        {
                            Mahd = hd.MaHd,
                            MaHh = cthd.MaHh,
                            TenHH = hh.TenHh,
                            Hinh = hh.Hinh,
                            DienThoai = kh.DienThoai,
                            DiaChi = kh.DiaChi,
                            DonGia = cthd.DonGia,
                            NgayDat = hd.NgayDat,
                            NgayGiao = hd.NgayGiao,
                            HoTen = kh.HoTen,
                            Email = kh.Email,
                            SoLuong= cthd.SoLuong,
                        }).ToList();

            if (data == null || !data.Any())
            {
                return NotFound();
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> getList(jDatatable model)
        {
            var items = (from i in db.HoaDons select i);
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
                i.MaHd,
                Name = i.HoTen,
                i.NgayDat,
                i.NgayGiao,
            }).Skip(model.start).Take(model.length).ToListAsync();
            var jsonData = new { draw = model.draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data };
            return Ok(jsonData);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            
           var item = await db.HoaDons.FindAsync(id);
            if(item.NgayDat !=null)
                return Ok(false);
            var details =await db.ChiTietHds.Where(i => i.MaHd == id).ToListAsync();
            db.ChiTietHds.RemoveRange(details);
            db.Entry(item).State = EntityState.Deleted;
            await db.SaveChangesAsync();
            return Ok(true);
        }

    }
}
