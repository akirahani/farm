using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class AnimalController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;
    public AnimalController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/animal", Name="admin.animal")]
    public  IActionResult List()
    {
        ViewBag.animalList = _context.Animal.ToList();
        ViewBag.cateList = _context.Category.ToList();
        ViewBag.cageList = _context.Cage.ToList();

        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Animal/List.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [Route("/admin/animal/add", Name="admin.animal.add")]

    public  IActionResult Add()
    {
        List<Cage> cageList = _context.Cage.ToList();
        ViewBag.arr_cage  = cageList;
        List<Category> cateList = _context.Category.ToList();
        ViewBag.arr_cate = cateList ;
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(ViewBag.arr_cate != null){
                if(ViewBag.arr_cage != null){
                    return View("~/Views/Admin/Animal/Add.cshtml");
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
    [Route("/admin/animal/insert", Name="admin.animal.insert")]
    public  IActionResult Insert([Bind("name","cage_id","category_id","date_in","date_out","status","created_at","height")] Animal animal)
    {
        var animalList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(animal != null){
                if(animal.name != null){
                    animal.name = animal.name;
                }else{
                    animal.name = "";
                }
                
                if(animal.cage_id != null)
                {
                    animal.cage_id = animal.cage_id;
                }else{
                   animal.cage_id = 1;
                }

                if( animal.height  != null){
                    animal.height  = animal.height ;
                }else{
                    animal.height  = 0;
                }

                if( animal.category_id  != null){
                    animal.category_id  = animal.category_id ;
                }else{
                    animal.category_id  = 0;
                }

                if( animal.status  != null){
                    animal.status  = animal.status ;
                }else{
                    animal.status  = 1;
                }

                animal.created_at = DateTime.Now;

                _context.Add(animal);
                _context.SaveChanges();
                return RedirectToAction("List","animal",animalList);
            }else{
                return View("~/Views/Admin/Animal/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/animal/edit", Name="admin.animal.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Animal.Where(x => x.id == id).FirstOrDefault().name == null){
                    ViewBag.name= "";
                }else{
                    ViewBag.name= _context.Animal.Where(x => x.id == id).FirstOrDefault().name;
                }

           
                if(_context.Animal.Where(x => x.id == id).FirstOrDefault().status == 0){
                    ViewBag.status= 0;
                }else{
                    ViewBag.status= _context.Animal.Where(x => x.id == id).FirstOrDefault().status;
                }

                if(_context.Animal.Where(x => x.id == id).FirstOrDefault().height == null){
                    ViewBag.height= 0;
                }else{
                    ViewBag.height= _context.Animal.Where(x => x.id == id).FirstOrDefault().height;
                }

                ViewBag.date_in= _context.Animal.Where(x => x.id == id).FirstOrDefault().date_in;
                ViewBag.date_out= _context.Animal.Where(x => x.id == id).FirstOrDefault().date_out;

                ViewBag.category_id = _context.Animal.Where(x => x.id == id).FirstOrDefault().category_id;
                ViewBag.cage_id = _context.Animal.Where(x => x.id == id).FirstOrDefault().cage_id;
                
                List<Cage> cageList = _context.Cage.ToList();
                ViewBag.arr_cage  = cageList;
                List<Category> cateList = _context.Category.ToList();
                ViewBag.arr_cate = cateList;
                return View("~/Views/Admin/Animal/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/animal/update", Name="admin.animal.update")]
    public  IActionResult Update([Bind("id","name","cage_id","category_id","date_in","date_out","status","created_at","height")] Animal animal){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(animal != null){
                    var data = _context.Animal.Find(animal.id);
                    if(animal.name != null){
                        animal.name = animal.name;
                    }else{
                        animal.name = "";
                    }
                    
                    if(animal.cage_id != null)
                    {
                        animal.cage_id = animal.cage_id;
                    }else{
                        animal.cage_id = 1;
                    }

                    if( animal.height  != null){
                        animal.height  = animal.height ;
                    }else{
                        animal.height  = 0;
                    }

                    if( animal.category_id  != null){
                        animal.category_id  = animal.category_id ;
                    }else{
                        animal.category_id  = 0;
                    }

                    if( animal.status  != null){
                        animal.status  = animal.status ;
                    }else{
                        animal.status  = 1;
                    }

                    data.name = animal.name;
                    data.cage_id = animal.cage_id;
                    data.category_id = animal.category_id;
                    data.date_in = animal.date_in;
                    data.status = animal.status;
                    data.date_out = animal.date_out;
                    data.height = animal.height;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/animal");
         
            }else{
                return Redirect("/admin/animal/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/animal/delete", Name="admin.animal.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Animal.Find(id);
            if (data != null) {
                _context.Animal.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/animal");
            }else{
                return Redirect("login");
            }
        }else{
            return Redirect("login");
        }
    }

    [HttpPost]
    [Route("/admin/animal/search", Name="admin.animal.search")]
    public  JsonResult Search(string search){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            string query = "select * from animal where name like '%"+@search+"%' ";
            var lst = _context.Animal.FromSqlRaw(query).ToList();
            var listCate = _context.Category.ToList();
            string[] arr_cate = new string[10];
            foreach(var itemCate in listCate){
                arr_cate[itemCate.id] = itemCate.name;
            }
            var listCage = _context.Cage.ToList();
            string[] arr_cage = new string[10];
            foreach(var itemCage in listCage){
                arr_cage[itemCage.id] = itemCage.name;
            }
            if (lst != null) {
                var data = new { arr_animal = lst, cage= arr_cage, cate = arr_cate };
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
