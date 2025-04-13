using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;



namespace Farm.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly MvcBasicDbContext _context;

    public HomeController(ILogger<HomeController> logger, MvcBasicDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [Route("/", Name="default")]
    public IActionResult Index()
    {
        var listNews = _context.News.ToList();
        ViewBag.listNewGet = listNews;
        return View("~/Views/Home/Index.cshtml");
    }

    [Route("/privacy", Name="privacy")]
    public IActionResult Privacy()
    {
        return View("~/Views/Home/Privacy.cshtml");
    }

    [Route("/news", Name="news")]
    public IActionResult News()
    {
        var listNews = _context.News.ToList();
        ViewBag.listNewGet = listNews;
        return View("~/Views/Home/News.cshtml");
    }

    [Route("/introduce", Name="introduce")]
    public IActionResult Introduce()
    {
        return View("~/Views/Home/Introduce.cshtml");
    }

    [Route("/contact", Name="contact")]
    public IActionResult Contact()
    {
        return View("~/Views/Home/Contact.cshtml");
    }

    [HttpGet]
    [Route("/blog")]
    public  IActionResult NewsDetail(int? id){
        ViewBag.id = id;
        ViewBag.title = _context.News.Find(id).title;
        ViewBag.subtit = _context.News.Where(x => x.id == id).FirstOrDefault().subtit;
        ViewBag.image = _context.News.Where(x => x.id == id).FirstOrDefault().image;
        ViewBag.content = _context.News.Where(x => x.id == id).FirstOrDefault().content;
        ViewBag.created_at = _context.News.Find(id).created_at.ToString("yyyy-MM-dd");        

        ViewBag.listNewOther = _context.News.Where(x => x.id != id).ToList();
        return View("~/Views/Home/Detail/News.cshtml");

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
