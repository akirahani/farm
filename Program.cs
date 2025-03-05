
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using mvcbasic.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//database 
var connect = builder.Services.AddDbContext<MvcBasicDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("FarmContext")));
//session
builder.Services.AddDistributedMemoryCache();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.UseSession();



// ROUTER
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
//UI
////
app.MapControllerRoute(
    name: "introduce",
    pattern: "{controller=Home}/{action=Introduce}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "news",
    pattern: "{controller=Home}/{action=News}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "privacy",
    pattern: "{controller=Home}/{action=Privacy}/{id?}")
    .WithStaticAssets();
app.MapControllerRoute(
    name: "contact",
    pattern: "{controller=Home}/{action=Contact}/{id?}")
    .WithStaticAssets();
//Admin
////User
app.MapControllerRoute(
    name: "admin",
    pattern: "~/Admin/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "login",
    pattern: "~/Admin/{controller=Auth}/{action=Login}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "admin.user",
    pattern: "~/Admin/{controller=User}/{action=List}/{id?}")
    .WithStaticAssets();



app.Run();
