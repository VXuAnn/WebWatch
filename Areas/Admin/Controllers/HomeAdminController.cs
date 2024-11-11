using Microsoft.AspNetCore.Mvc;
using ShopBanHang.Data;
using System.Web;

namespace ShopBanHang.Areas.Admin.Controllers
{
	/*[Area("admin")]*/
	[Area("Admin")]
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
