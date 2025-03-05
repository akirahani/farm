using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using NuGet.Protocol;

namespace Farm.Controllers.Admin;


public class AuthController : Controller
{
    public const string SessionKeyName = "_Name";
    private readonly MvcBasicDbContext _context;

    private readonly ILogger<AuthController> _logger;

    public AuthController(MvcBasicDbContext context, ILogger<AuthController> logger){
        _context = context;
                _logger = logger;

    }

    [Route("/admin/login", Name="login")]
    [HttpGet]
    public IActionResult Login()
    {
        var viewModel = new LoginViewModel();
        return View("~/Views/Admin/Login.cshtml",viewModel);
    }

    [Route("/admin/login", Name="login")]
    [HttpPost]
    public IActionResult Login(LoginViewModel viewModel)
    {
        string connectionString = "Server=ADMIN\\SQLEXPRESS;Database=farm;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True";
        SqlConnection conn = new(connectionString);
        String sql = "SELECT username, passd, role FROM users WHERE role = 1";
        SqlCommand cmd = new(sql, conn);
        List<string> usernameDB = new List<string>();
        conn.Open();
        using(SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection)){
            int i = 0;
            while(reader.Read()){
                usernameDB.Add(reader["username"].ToString());
                i++;
            }
            if(usernameDB.Contains(viewModel.username)){
                string userName = viewModel.username;
                SqlCommand cmd2 = new ("SELECT * FROM users WHERE CONVERT(VARCHAR, username) = '"+userName+"'",conn );
                using(SqlDataReader reader2 =  cmd2.ExecuteReader(CommandBehavior.CloseConnection)){
                    if(reader2.Read()){
                        if(viewModel.passd.ToString() == reader2["passd"].ToString()){
                            HttpContext.Session.SetString(SessionKeyName, reader2["name"].ToString());
                            var sessionName = HttpContext.Session.GetString(SessionKeyName);
                            TempData["Result"] = sessionName;

                            return View("~/Views/Admin/Index.cshtml",TempData);
                        }else{
                            return View("~/Views/Admin/Login.cshtml"); 
                        }
                    }else{
                        return View("~/Views/Admin/Login.cshtml");

                    }
                }
         
            }else{
                return View("~/Views/Admin/Login.cshtml");
            }
        }

    }

    public IActionResult Logout(){
        HttpContext.Session.Clear();
        return View("~/Views/Admin/Login.cshtml");

    }
}
