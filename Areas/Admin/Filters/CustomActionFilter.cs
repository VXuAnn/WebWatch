using AutoMapper.Execution;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopBanHang.Areas.Admin.Extensions;

namespace ShopBanHang.Areas.Admin.Filters
{
    public class CustomActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var area = context.RouteData.Values["area"];
            if (context.RouteData.Values["action"].ToString() != "Login" && area?.ToString() == "Admin")
            {
                var member = context.HttpContext.Session.GetObject<Member>("member");
                if (member == null)
                {
                    context.Result = new RedirectResult("/Admin/Member/Login");
                }
            }
        }
    }
}
