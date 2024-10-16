using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;

namespace ShopBanHang.ViewComponents
{
    public class MenuLoaiViewComponent :ViewComponent
    {
        private readonly AaashopContext db;
        public MenuLoaiViewComponent(AaashopContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiVM
            {
                MaLoai= lo.MaLoai,
                TenLoai= lo.TenLoai,
                SoLuong = lo.HangHoas.Count
            });

            return View(data);
        }
    }
}
