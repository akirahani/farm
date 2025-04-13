using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class CageController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;
    public CageController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/cage", Name="admin.cage")]
    public  IActionResult List()
    {
        ViewBag.cageList = _context.Cage.ToList();
        ViewBag.userList = _context.Users.ToList();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Cage/List.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [Route("/admin/cage/add", Name="admin.cage.add")]

    public  IActionResult Add()
    {
        List<User> userLists = _context.Users.ToList();
        ViewBag.arr_user  = userLists;
        List<Category> cateLists = _context.Category.ToList();
        ViewBag.arr_cate = cateLists;
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(ViewBag.arr_cate != null){
                if(ViewBag.arr_user != null){
                    return View("~/Views/Admin/Cage/Add.cshtml");
                }else{
                    return Content("not ok");
                }
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/cage/insert", Name="admin.cage.insert")]
    public  IActionResult Insert([Bind("name","users_id","category_id","note","quantity","status","created_at","close_at")] Cage cage)
    {
        var cageList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(cage != null){
                if(cage.name != null){
                    cage.name= cage.name;
                    }else{
                    cage.name= "";
                }
                
                if(cage.users_id != null)
                {
                    cage.users_id = cage.users_id;
                }else{
                    cage.users_id = 0;
                }

                if( cage.note!= null){
                    cage.note = cage.note;
                }else{
                    cage.note = "";
                }

                if( cage.category_id != null){
                    cage.category_id = cage.category_id;
                }else{
                    cage.category_id = 0;
                }

                if( cage.quantity != null){
                    cage.quantity = cage.quantity;
                }else{
                    cage.quantity = 0; 
                }

                if( cage.status != null){
                    cage.status = cage.status;
                }else{
                    cage.status = 1; 
                }
                
                if( cage.created_at != null){
                    cage.created_at = cage.created_at;
                }else{
                    cage.created_at = DateTime.Now; 
                }
           
                _context.Add(cage);
                _context.SaveChanges();
                return RedirectToAction("List","Cage",cageList);
            }else{
                return View("~/Views/Admin/Cage/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/cage/edit", Name="admin.cage.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Cage.Where(x => x.id == id).FirstOrDefault().name == null){
                    ViewBag.name= "";
                }else{
                    ViewBag.name= _context.Cage.Where(x => x.id == id).FirstOrDefault().name;
                }

                if(_context.Cage.Where(x => x.id == id).FirstOrDefault().quantity == null){
                    ViewBag.quantity= 0;
                }else{
                    ViewBag.quantity= _context.Cage.Where(x => x.id == id).FirstOrDefault().quantity;
                }

                if(_context.Cage.Where(x => x.id == id).FirstOrDefault().status == 0){
                    ViewBag.status= 0;
                }else{
                    ViewBag.status= _context.Cage.Where(x => x.id == id).FirstOrDefault().status;
                }

                if(_context.Cage.Where(x => x.id == id).FirstOrDefault().note == null){
                    ViewBag.note= "";
                }else{
                    ViewBag.note= _context.Cage.Where(x => x.id == id).FirstOrDefault().note;
                }

                if(_context.Cage.Where(x => x.id == id).FirstOrDefault().created_at == null){
                    ViewBag.created_at= "";
                }else{
                    ViewBag.created_at= _context.Cage.Where(x => x.id == id).FirstOrDefault().created_at;
                }

                if(_context.Cage.Where(x => x.id == id).FirstOrDefault().close_at == null){
                    ViewBag.close_at= "";
                }else{
                    ViewBag.close_at= _context.Cage.Where(x => x.id == id).FirstOrDefault().close_at;
                }

                ViewBag.category_id = _context.Cage.Where(x => x.id == id).FirstOrDefault().category_id;
                ViewBag.users_id = _context.Cage.Where(x => x.id == id).FirstOrDefault().users_id;
                
                List<User> userLists = _context.Users.ToList();
                ViewBag.arr_user  = userLists;
                List<Category> cateLists = _context.Category.ToList();
                ViewBag.arr_cate = cateLists;
                return View("~/Views/Admin/Cage/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/cages/update", Name="admin.Cages.update")]
    public  IActionResult Update([Bind("id","name","users_id","category_id","quantity","status","note","created_at","close_at")] Cage cage){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(cage != null){
                    var data = _context.Cage.Find(cage.id);

                    if(cage.name != null){
                        cage.name= cage.name;
                    }else{
                        cage.name= "";
                    }
                    
                    if(cage.users_id != null)
                    {
                        cage.users_id = cage.users_id;
                    }else{
                        cage.users_id = 0;
                    }

                    if( cage.note!= null){
                        cage.note = cage.note;
                    }else{
                        cage.note = "";
                    }

                    if( cage.category_id != null){
                        cage.category_id = cage.category_id;
                    }else{
                        cage.category_id = 0;
                    }

                    if( cage.quantity != null){
                        cage.quantity = cage.quantity;
                    }else{
                        cage.quantity = 0; 
                    }

                    if( cage.status != null){
                        cage.status = cage.status;
                    }else{
                        cage.status = 1; 
                    }
                    data.name = cage.name;
                    data.users_id = cage.users_id;
                    data.category_id = cage.category_id;
                    data.quantity = cage.quantity;
                    data.status = cage.status;
                    data.note = cage.note;
                    data.created_at = cage.created_at;
                    data.close_at = cage.close_at;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/cage");
         
            }else{
                return Redirect("/admin/cage/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/cage/delete", Name="admin.cage.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Cage.Find(id);
            if (data != null) {
                _context.Cage.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/cage");
            }else{
                return Redirect("login");
            }
        }else{
            return Redirect("login");
        }
    }

    [HttpPost]
    [Route("/admin/cage/search", Name="admin.cage.search")]
    public  JsonResult Search(string search){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            string query = "select * from cage where name like '%"+@search+"%' ";
            var lst = _context.Cage.FromSqlRaw(query).ToList();
            var listUser = _context.Users.ToList();
            string[] arr_user = new string[10];
            foreach(var itemUser in listUser){
                arr_user[itemUser.id] = itemUser.name;
            }
           
            if (lst != null) {
                var data = new { arr_cage = lst, user= arr_user };
                return Json(data);
            }else{
                return Json(data: "not ok1");
            }
        }else{
            return Json(data: "not ok");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
