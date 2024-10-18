using Microsoft.AspNetCore.Mvc;

namespace ShopBanHang.Areas.Admin.Controllers
{
	[Area("admin")]
	/*[Route("/")]*/
	[Route("admin/home")]
	public class HomeAdminController : Controller
	{
		[Route("")]
		[Route("Index")]
		public IActionResult Index()
		{
			return View();
		}
	}
}
