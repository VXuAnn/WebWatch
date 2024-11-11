using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShopBanHang.Data;
using ShopBanHang.Helpers;
using static ShopBanHang.Data.AaashopContext;
using Microsoft.AspNetCore.Http;
using ShopBanHang.Areas.Admin.Filters;
using Microsoft.AspNetCore.Authorization;
using ShopBanHang.Areas.Admin.Attributes;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AaashopContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("AAASHOP"));
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    /*options.IdleTimeout = TimeSpan.FromDays(7);*/
    options.IdleTimeout = new TimeSpan(0, 15, 0);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc(cfg =>
{
    cfg.Filters.Add(new CustomActionFilter());
});

// https://docs.automapper.org/en/stable/Dependency-injection.html
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/KhachHang/DangNhap";
    options.AccessDeniedPath = "/AccessDenied";
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthentication();

app.UseAuthorization();



app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/admin/member/login");
    }
    else
    {
        await next();
    }
});
app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);
/*app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=HomeAdmin}/{action=Index}/{id?}",
    defaults: new { area = "Admin" }
);*/

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

*/
app.Run();