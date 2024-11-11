using Microsoft.AspNetCore.Mvc;
using ShopBanHang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ShopBanHang.Data;
using System.Linq;

namespace ShopBanHang.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly AaashopContext db;

        public SearchViewComponent(AaashopContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var results = (from hh in db.HangHoas
                           join ncc in db.NhaCungCaps on hh.MaNcc equals ncc.MaNcc
                           join loai in db.Loais on hh.MaLoai equals loai.MaLoai
                           group new { hh, ncc, loai } by ncc.TenCongTy into grouped
                           select new SearchVM
                           {
                               DonGia = (float)(grouped.First().hh.DonGia ?? 0),
                               TenCongTy = grouped.Key,
                               TenLoai = grouped.Select(x => x.loai.TenLoai).Distinct().FirstOrDefault() // Lấy TenLoai không trùng lặp
                           }).ToList();

            return View(results);
        }
    }
}
