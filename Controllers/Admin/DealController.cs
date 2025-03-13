using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class DealController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;
    public DealController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/deal", Name="admin.deal")]
    public  IActionResult List()
    {
        ViewBag.dealList = _context.Deal.ToList();
        ViewBag.animalList = _context.Animal.ToList();

        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Deal/List.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [Route("/admin/deal/add", Name="admin.deal.add")]

    public  IActionResult Add()
    {
        List<Animal> animalList = _context.Animal.ToList();
        ViewBag.arr_animal  = animalList;

        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(ViewBag.arr_animal != null){
                    return View("~/Views/Admin/Deal/Add.cshtml");
            }else{
                return  View("~/Views/Admin/Animal/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/deal/insert", Name="admin.deal.insert")]
    public  IActionResult Insert([Bind("customer","animal_id","total_price","date_out","status","created_at")] Deal deal)
    {
        var dealList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(deal != null){
                if(deal.customer != null){
                    deal.customer = deal.customer;
                }else{
                    deal.customer = "";
                }
                
                
                if(deal.animal_id != null)
                {
                    deal.animal_id = ""+deal.animal_id;
                }else{
                    deal.animal_id = "";
                }

                if( deal.total_price != null){
                    deal.total_price = deal.total_price;
                }else{
                    deal.total_price = 0;
                }

                if( deal.status != null){
                    deal.status = deal.status;
                }else{
                    deal.status = 1;
                }
                
                
                if( deal.date_out != null){
                    deal.date_out = deal.date_out;
                }else{
                    deal.date_out = DateTime.Now;
                }


                string[] arr =  deal.animal_id.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
                var id_animal = int.Parse(arr[0]);
                var data = _context.Animal.Find(id_animal);
                data.status = 3;
                _context.Entry(data).State = EntityState.Modified;



                _context.Add(deal);
                _context.SaveChanges();
                return RedirectToAction("List","Deal",dealList);
            }else{
                return View("~/Views/Admin/Deal/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/deal/edit", Name="admin.deal.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Deal.Where(x => x.id == id).FirstOrDefault().customer == null){
                    ViewBag.customer= "";
                }else{
                    ViewBag.customer= _context.Deal.Where(x => x.id == id).FirstOrDefault().customer;
                }

           
                if(_context.Deal.Where(x => x.id == id).FirstOrDefault().total_price ==null){
                    ViewBag.total_price = 0;
                }else{
                    ViewBag.total_price = _context.Deal.Where(x => x.id == id).FirstOrDefault().total_price;
                }

                if(_context.Deal.Where(x => x.id == id).FirstOrDefault().date_out == null){
                    ViewBag.date_out= DateTime.Now;
                }else{
                    ViewBag.date_out= _context.Deal.Where(x => x.id == id).FirstOrDefault().date_out;
                }

                if(_context.Deal.Where(x => x.id == id).FirstOrDefault().status == null){
                    ViewBag.status= 0;
                }else{
                    ViewBag.status= _context.Deal.Where(x => x.id == id).FirstOrDefault().status;
                }

                ViewBag.animal_id = _context.Deal.Where(x => x.id == id).FirstOrDefault().animal_id;
                

                List<Animal> animalList = _context.Animal.ToList();
                ViewBag.arr_animal = animalList;
                return View("~/Views/Admin/Deal/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/deal/update", Name="admin.deal.update")]
    public  IActionResult Update([Bind("id","customer","animal_id","total_price","date_out","status")] Deal deal){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(deal != null){
                    var data = _context.Deal.Find(deal.id);
                    if(deal.customer != null){
                        deal.customer = deal.customer;
                    }else{
                        deal.customer = "";
                    }
                    
                    
                    if(deal.animal_id != null)
                    {
                        deal.animal_id = deal.animal_id;
                    }else{
                        deal.animal_id = "";
                    }

                    if( deal.total_price != null){
                        deal.total_price = deal.total_price;
                    }else{
                        deal.total_price = 0;
                    }

                    if( deal.status != null){
                        deal.status = deal.status;
                    }else{
                        deal.status = 1;
                    }
                    
                    
                    if( deal.date_out != null){
                        deal.date_out = deal.date_out;
                    }else{
                        deal.date_out = DateTime.Now;
                    }
            
                    data.customer = deal.customer;
                    data.animal_id = deal.animal_id;
                    data.total_price = deal.total_price;
                    data.date_out = deal.date_out;
                    data.status = deal.status;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/deal");
         
            }else{
                return Redirect("/admin/deal/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/deal/delete", Name="admin.deal.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Deal.Find(id);
            if (data != null) {
                _context.Deal.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/deal");
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
