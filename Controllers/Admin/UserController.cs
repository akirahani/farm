using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class UserController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;

    public UserController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/user", Name="admin.user")]
    public  IActionResult List()
    {
        string connectionString = "Server=ADMIN\\SQLEXPRESS;Database=farm;Trusted_Connection=True;TrustServerCertificate=True;";
        SqlConnection conn = new(connectionString);
        String sql = "SELECT * FROM users";
        SqlCommand cmd = new(sql, conn);
        var model = new List<User>();
        var userList = new LoginViewModel();
        conn.Open();

        using(conn)
        {
            using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var user = new User();
                        user.name = reader["name"].ToString();
                        user.username = reader["username"].ToString();
                        user.role = (int)reader["role"];
                        user.id = (int)reader["id"];

                        model.Add(user);
                    }
                }
            }
        }
        userList.userList = model;
        conn.Close();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != null){
            return View("~/Views/Admin/User/List.cshtml",userList);
        }else{
            return Redirect("/login");
        }
    }


    [Route("/admin/user/add", Name="admin.user.add")]

    public  IActionResult Add()
    {
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != null){
            return View("~/Views/Admin/User/Add.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/user/insert", Name="admin.user.insert")]
    public  IActionResult Insert([Bind("username","name","passd","role")] User user)
    {
        var userList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(user != null){
                if (ModelState.IsValid){
                    _context.Add(user);
                    _context.SaveChanges();
                    return RedirectToAction("List","User",userList);
                }else{
                   return View("~/Views/Admin/User/Add.cshtml");
                }
            }else{
                return View("~/Views/Admin/User/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/user/edit", Name="admin.user.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Users.Where(x => x.id == id).FirstOrDefault().name == null){
                    ViewBag.name= "";
                }else{
                    ViewBag.name= _context.Users.Where(x => x.id == id).FirstOrDefault().name;
                }

                if(_context.Users.Where(x => x.id == id).FirstOrDefault().username == null){
                    ViewBag.username= "";
                }else{
                    ViewBag.username = _context.Users.Where(x => x.id == id).FirstOrDefault().username;
                }

                if(_context.Users.Where(x => x.id == id).FirstOrDefault().passd == null){
                    ViewBag.passd= "";
                }else{
                    ViewBag.passd = _context.Users.Where(x => x.id == id).FirstOrDefault().passd;
                }

                ViewBag.role = _context.Users.Where(x => x.id == id).FirstOrDefault().role;
                return View("~/Views/Admin/User/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/user/update", Name="admin.user.update")]
    public  IActionResult Update([Bind("id","username","name","passd","role")] User user){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(user != null){
                if (ModelState.IsValid) {
                    var data = _context.Users.Find(user.id);
                    data.username = user.username;
                    data.name = user.name;
                    data.passd = user.passd;
                    data.role = user.role;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/user");
                }
                else{
                    return Content("<script language='javascript' type='text/javascript'>alert('indefiend Message');</script>");
                }
            }else{
                return Redirect("/admin/user/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/user/delete", Name="admin.user.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Users.Find(id);
            if (data != null) {
                _context.Users.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/user");
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
