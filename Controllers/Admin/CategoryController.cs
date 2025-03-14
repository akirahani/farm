using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class CategoryController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;

    public CategoryController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/category", Name="admin.category")]
    public  IActionResult List()
    {
        var categoryList = new LoginViewModel();
        List<Category> cateList = _context.Category.ToList();
        categoryList.categoryList = cateList;
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Category/List.cshtml",categoryList);
        }else{
            return Redirect("/login");
        }
    }


    [Route("/admin/category/add", Name="admin.category.add")]

    public  IActionResult Add()
    {
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Category/Add.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/category/insert", Name="admin.Category.insert")]
    public  IActionResult Insert([Bind("name","parent_id")] Category category)
    {
        var categoryList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(category != null){
                    _context.Add(category);
                    _context.SaveChanges();
                    return RedirectToAction("List","Category",categoryList);
          
            }else{
                return View("~/Views/Admin/Category/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/category/edit", Name="admin.category.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Category.Where(x => x.id == id).FirstOrDefault().name == null){
                    ViewBag.name= "";
                }else{
                    ViewBag.name= _context.Category.Where(x => x.id == id).FirstOrDefault().name;
                }

                ViewBag.parent_id = _context.Category.Where(x => x.id == id).FirstOrDefault().parent_id;
                return View("~/Views/Admin/Category/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/category/update", Name="admin.category.update")]
    public  IActionResult Update([Bind("id","name","parent_id")] Category category){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(category != null){
                if (ModelState.IsValid) {
                    var data = _context.Category.Find(category.id);
                    data.name = category.name;
                    data.parent_id = category.parent_id;
                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/category");
                }
                else{
                    return Content("<script language='javascript' type='text/javascript'>alert('indefiend Message');</script>");
                }
            }else{
                return Redirect("/admin/category/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/category/delete", Name="admin.category.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Category.Find(id);
            if (data != null) {
                _context.Category.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/category");
            }else{
                return Redirect("login");
            }
        }else{
            return Redirect("login");
        }
    }

    [HttpPost]
    [Route("/admin/category/search", Name="admin.category.search")]
    public  JsonResult Search(string search){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            string query = "select * from category where name like '%"+@search+"%' ";
            var lst = _context.Category.FromSqlRaw(query).ToList();
           
            if (lst != null) {;
                return Json(lst);
            }else{
                return Json("not ok1");
            }
        }else{
            return Json("not ok");
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
