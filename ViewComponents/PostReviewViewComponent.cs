using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;
using ShopBanHang.ViewModels;


namespace ShopBanHang.ViewComponents
{
    public class PostReviewViewComponent :ViewComponent
    {
        private readonly AaashopContext db;
        public PostReviewViewComponent(AaashopContext context) => db = context;
        public IViewComponentResult Invoke(int productId)
        {
            var postReviewVM = new DanhGiaVM { MaHH = productId };
            return View(postReviewVM);
        }
    }
}
