using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Farm.Models;
using mvcbasic.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace Farm.Controllers.Admin;

public class ContactController : Controller
{
    public const string SessionKeyName = "_Name";

    private readonly MvcBasicDbContext _context;
    public ContactController(MvcBasicDbContext context){
        _context = context;
    }

    [Route("/admin/contact", Name="admin.contact")]
    public  IActionResult List()
    {
        ViewBag.contactList = _context.Contact.ToList();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Contact/List.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [Route("/admin/contact/add", Name="admin.contact.add")]

    public  IActionResult Add()
    {   
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            return View("~/Views/Admin/Contact/Add.cshtml");
        }else{
            return Redirect("/login");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/contact/insert", Name="admin.contact.insert")]
    public  IActionResult Insert([Bind("name","email","phone","address","note","created_at")] Contact contact)
    {
        var contactList = new LoginViewModel();
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(contact != null){
                if(contact.name != null){
                    contact.name = contact.name;
                    }else{
                    contact.name = "";
                }
                
                
                if(contact.email != null)
                {
                    contact.email = ""+contact.email;
                }else{
                    contact.email = "";
                }

                if( contact.phone != null){
                    contact.phone = contact.phone;
                }else{
                    contact.phone = "";
                }

                if( contact.address != null){
                    contact.address = contact.address;
                }else{
                    contact.address = "";
                }
                
                
                if( contact.note != null){
                    contact.note = contact.note;
                }else{
                    contact.note = ""; 
                }

                contact.created_at = DateTime.Now;

                _context.Add(contact);
                _context.SaveChanges();
                return RedirectToAction("List","Contact",contactList);
            }else{
                return View("~/Views/Admin/Contact/Add.cshtml");
            }
        }else{
            return Redirect("/login");
        }
    }

    
    [HttpGet]
    [Route("/admin/contact/edit", Name="admin.contact.edit")]
    public  IActionResult Edit(int? id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(id != null){
                ViewBag.id = id;
                if(_context.Contact.Where(x => x.id == id).FirstOrDefault().name == null){
                    ViewBag.name= "";
                }else{
                    ViewBag.name= _context.Contact.Where(x => x.id == id).FirstOrDefault().name;
                }

           
                if(_context.Contact.Where(x => x.id == id).FirstOrDefault().email ==null){
                    ViewBag.email = 0;
                }else{
                    ViewBag.email = _context.Contact.Where(x => x.id == id).FirstOrDefault().email;
                }

                if(_context.Contact.Where(x => x.id == id).FirstOrDefault().phone == null){
                    ViewBag.phone= "";
                }else{
                    ViewBag.phone= _context.Contact.Where(x => x.id == id).FirstOrDefault().phone;
                }

                if(_context.Contact.Where(x => x.id == id).FirstOrDefault().address == null){
                    ViewBag.address= "";
                }else{
                    ViewBag.address= _context.Contact.Where(x => x.id == id).FirstOrDefault().address;
                }

                ViewBag.note = _context.Contact.Where(x => x.id == id).FirstOrDefault().note;
                
                return View("~/Views/Admin/Contact/Edit.cshtml");
            }else{
                return Content("not ok");
            }
        }else{
            return Redirect("/login");
        }

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/contact/update", Name="admin.contact.update")]
    public  IActionResult Update([Bind("id","name","email","phone","address","note","created_at")] Contact contact){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            if(contact != null){
                    var data = _context.Contact.Find(contact.id);
                    if(contact.name != null){
                        contact.name = contact.name;
                    }else{
                        contact.name = "";
                    }
                    
                    
                    if(contact.email != null)
                    {
                        contact.email = contact.email;
                    }else{
                        contact.email = "";
                    }

                    if( contact.phone != null){
                        contact.phone = contact.phone;
                    }else{
                        contact.phone = "";
                    }

                    if( contact.address != null){
                        contact.address = contact.address;
                    }else{
                        contact.address = "";
                    }
                    
                    
                    if( contact.note != null){
                        contact.note = contact.note;
                    }else{
                        contact.note = "";
                    }
            
                    data.name = contact.name;
                    data.email = contact.email;
                    data.phone = contact.phone;
                    data.address = contact.address;
                    data.note = contact.note;

                    _context.Entry(data).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Redirect("/admin/contact");
         
            }else{
                return Redirect("/admin/contact/edit");

            }
        }else{
            return Redirect("/login");

        }
    }

    [HttpGet]
    [Route("/admin/contact/delete", Name="admin.contact.delete")]
    public  IActionResult Delete(int id){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            var data = _context.Contact.Find(id);
            if (data != null) {
                _context.Contact.Remove(data);
                _context.SaveChanges();
                return Redirect("/admin/contact");
            }else{
                return Redirect("login");
            }
        }else{
            return Redirect("login");
        }
    }

    [HttpPost]
    [Route("/admin/contact/search", Name="admin.contact.search")]
    public  JsonResult Search(string search){
        var sessionName = HttpContext.Session.GetString(SessionKeyName);
        ViewBag.nameSession = sessionName;
        if(ViewBag.nameSession != ""){
            string query = "select * from contact where name like '%"+@search+"%' or phone like '%"+@search+"%'";
            var lst = _context.Contact.FromSqlRaw(query).ToList();

            if (lst != null) {
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
