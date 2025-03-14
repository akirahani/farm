using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class TherapyController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;
    public TherapyController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/therapy", Name="admin.therapy")]
    public  IActionResult List()
    {
        ViewBag.therapyList = _context.Therapy.ToList();
        ViewBag.animalList = _context.Animal.ToList();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Therapy/List.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [Route("/admin/therapy/add", Name="admin.therapy.add")]

    public  IActionResult Add()
    {
        List<Animal> animalList = _context.Animal.ToList();
        ViewBag.arr_animal  = animalList;

        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(ViewBag.arr_animal != null){
                    return View("~/Views/Admin/Therapy/Add.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/therapy/insert", Name="admin.therapy.insert")]
    public  IActionResult Insert([Bind("name","animal_id","doctor","medicine","note","quantity","created_at")] Therapy therapy)
    {
        var therapyList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(therapy != null){
                if(therapy.name != null){
                    therapy.name = therapy.name;
                }else{
                    therapy.name = "";
                }
                
                
                if(therapy.animal_id != null)
                {
                    therapy.animal_id = therapy.animal_id;
                }else{
                    therapy.animal_id = 1;
                }

                if( therapy.doctor != null){
                    therapy.doctor = therapy.doctor;
                }else{
                    therapy.doctor = "";
                }

                if( therapy.medicine != null){
                    therapy.medicine = therapy.medicine;
                }else{
                    therapy.medicine = "";
                }
                
                
                if( therapy.note != null){
                     therapy.note = therapy.note;
                }else{
                    therapy.note = "";
                }
                
                if( therapy.quantity != null){
                    therapy.quantity = therapy.quantity;
                }else{
                    therapy.quantity = 0;
                }

                therapy.created_at = DateTime.Now;
                _context.Add(therapy);
                _context.SaveChanges();
                return RedirectToAction("List","Therapy",therapyList);
            }else{
                return View("~/Views/Admin/Therapy/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/therapy/edit", Name="admin.therapy.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Therapy.Where(x => x.id == id).FirstOrDefault().name == null){
                    ViewBag.name= "";
                }else{
                    ViewBag.name= _context.Therapy.Where(x => x.id == id).FirstOrDefault().name;
                }

           
                if(_context.Therapy.Where(x => x.id == id).FirstOrDefault().doctor ==null){
                    ViewBag.doctor= "";
                }else{
                    ViewBag.doctor= _context.Therapy.Where(x => x.id == id).FirstOrDefault().doctor;
                }

                if(_context.Therapy.Where(x => x.id == id).FirstOrDefault().medicine == null){
                    ViewBag.medicine= "";
                }else{
                    ViewBag.medicine= _context.Therapy.Where(x => x.id == id).FirstOrDefault().medicine;
                }

                if(_context.Therapy.Where(x => x.id == id).FirstOrDefault().note == null){
                    ViewBag.note= "";
                }else{
                    ViewBag.note= _context.Therapy.Where(x => x.id == id).FirstOrDefault().note;
                }

                if(_context.Therapy.Where(x => x.id == id).FirstOrDefault().quantity == null){
                    ViewBag.quantity= 0;
                }else{
                    ViewBag.quantity= _context.Therapy.Where(x => x.id == id).FirstOrDefault().quantity;
                }

                ViewBag.animal_id = _context.Therapy.Where(x => x.id == id).FirstOrDefault().animal_id;
                

                List<Animal> animalList = _context.Animal.ToList();
                ViewBag.arr_animal = animalList;
                return View("~/Views/Admin/Therapy/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/therapy/update", Name="admin.therapy.update")]
    public  IActionResult Update([Bind("id","name","animal_id","doctor","medicine","note","quantity")] Therapy therapy){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(therapy != null){
                    var data = _context.Therapy.Find(therapy.id);
                    if(therapy.name != null){
                        therapy.name = therapy.name;
                    }else{
                        therapy.name = "";
                    }
                    
                    
                    if(therapy.animal_id != null)
                    {
                        therapy.animal_id = therapy.animal_id;
                    }else{
                        therapy.animal_id = 1;
                    }

                    if( therapy.doctor != null){
                        therapy.doctor = therapy.doctor;
                    }else{
                        therapy.doctor = "";
                    }

                    if( therapy.medicine != null){
                        therapy.medicine = therapy.medicine;
                    }else{
                        therapy.medicine = "";
                    }
                    
                    
                    if( therapy.note != null){
                        therapy.note = therapy.note;
                    }else{
                        therapy.note = "";
                    }
                    
                    if( therapy.quantity != null){
                        therapy.quantity = therapy.quantity;
                    }else{
                        therapy.quantity = 0;
                    }
                    data.name = therapy.name;
                    data.animal_id = therapy.animal_id;
                    data.doctor = therapy.doctor;
                    data.medicine = therapy.medicine;
                    data.note = therapy.note;
                    data.quantity = therapy.quantity;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/therapy");
         
            }else{
                return Redirect("/admin/therapy/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/therapy/delete", Name="admin.therapy.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Therapy.Find(id);
            if (data != null) {
                _context.Therapy.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/therapy");
            }else{
                return Redirect("login");
            }
        }else{
            return Redirect("login");
        }
    }

    [HttpPost]
    [Route("/admin/therapy/search", Name="admin.therapy.search")]
    public  JsonResult Search(string search){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            string query = "select * from therapy where name like '%"+@search+"%'";
            var lst = _context.Therapy.FromSqlRaw(query).ToList();
            string[] arr_animal = new string[10];
            var animalList = _context.Animal.ToList();
            foreach(var item in animalList){
                arr_animal[item.id] = item.name;
            }
            if (lst != null) {
                var data = new { arr_therapy= lst , animal = arr_animal};
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
