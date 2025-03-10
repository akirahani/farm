using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.VisualBasic;
using System.Globalization;

namespace Farm.Controllers.Admin;

public class NewsController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;
    private readonly IWebHostEnvironment _environment;

    public NewsController(MvcBasicDbContext context, IWebHostEnvironment environment){
        _context = context;
        _environment = environment;
    }
    public IFormFile FileUploads { get; set; }


    [Route("/admin/news", Name="admin.news")]
    public  IActionResult List()
    {
        var listNewsRlt = new LoginViewModel();
        List<News> newsList = _context.News.ToList();
        listNewsRlt.newsList =newsList;
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/News/List.cshtml",listNewsRlt);
        }else{
            return Redirect("/login");
        }
    }


    [Route("/admin/news/add", Name="admin.news.add")]

    public  IActionResult Add()
    {
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/News/Add.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/news/insert", Name="admin.news.insert")]
    public  async Task<IActionResult> InsertAsync([Bind("title","subtit","file","content","created_at")] News news)
    {
        var newsList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(news != null){
                    // handle time create
                    news.created_at = DateTime.Now;
                    // handle image 
                    news.image = news.file.FileName;
                    if (news.file != null) {
                        var fileName = Path.GetFileName(news.file.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);

                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            await news.file.CopyToAsync(fileSteam);
                        }
                    }else{

                    }
                    _context.Add(news);
                    _context.SaveChanges();
                    return RedirectToAction("List","News",newsList);
            }else{
                return View("~/Views/Admin/News/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/news/edit", Name="admin.news.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                ViewBag.title = _context.News.Find(id).title;
                ViewBag.subtit = _context.News.Where(x => x.id == id).FirstOrDefault().subtit;
                ViewBag.image = _context.News.Where(x => x.id == id).FirstOrDefault().image;
                ViewBag.content = _context.News.Where(x => x.id == id).FirstOrDefault().content;
                ViewBag.created_at = _context.News.Find(id).created_at.ToString("yyyy-MM-dd");
                return View("~/Views/Admin/News/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/news/update", Name="admin.news.update")]
    public  async Task<IActionResult> Update([Bind("id","title","subtit","file","content")] News news){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(news != null){
                    var data = _context.News.Find(news.id);
                    // check olg img
                    var imagePath = Path.Combine(_environment.WebRootPath, "uploads", data.image);
                
                    if(news.file != null){
                        if (System.IO.File.Exists(imagePath)){
                            System.IO.File.Delete(imagePath);
                        }
                        var fileName = Path.GetFileName(news.file.FileName);
                        var filePath = Path.Combine(_environment.WebRootPath, "uploads", fileName);
                        using (var fileSteam = new FileStream(filePath, FileMode.Create))
                        {
                            await news.file.CopyToAsync(fileSteam);
                        }
                    }

                    data.image = news.file.FileName;
                    data.title = news.title;
                    data.subtit = news.subtit;
                    data.content = news.content;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/news");
                
            }else{
                return Redirect("/admin/news/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/news/delete", Name="admin.news.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.News.Find(id);
            if (data != null) {
                _context.News.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/news");
            }else{
                return Redirect("login");
            }
        }else{
            return Redirect("login");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
