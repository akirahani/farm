using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;



namespace Farm.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    [Route("/", Name="default")]
    public IActionResult Index()
    {
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
