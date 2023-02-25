using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;
// using AttributeRouting.Web.Http;


namespace LibraryManagement.Controllers
{
    public class BookController : Controller
    {
        private readonly ACE42023Context db;
        public BookController(ACE42023Context _db){
            db = _db;
        }

        [HttpGet]
        public ActionResult GetBooks(){
            ViewBag.username = HttpContext.Session.GetString("username");
            if(ViewBag.username != null){
                var IsAdmin = HttpContext.Session.GetString("IsAdmin");
                if(IsAdmin != null && IsAdmin == "yes"){
                    ViewBag.isAdmin = true;
                }
                List<RahulBook> li = new List<RahulBook>();
                foreach (var item in db.RahulBooks)
                {
                    if(item.BookQty > 0){
                        li.Add(item);
                    }
                }
                return View(li);
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public ActionResult GetDetails(int id){
            
            ViewBag.username = HttpContext.Session.GetString("username");
            if(ViewBag.username != null){
                var IsAdmin = HttpContext.Session.GetString("IsAdmin");
                if(IsAdmin != null && IsAdmin == "yes"){
                    ViewBag.isAdmin = true;
                }
                string sbid = Convert.ToString(id);
                ViewBag.id = id;
                RahulBook b = db.RahulBooks.Find(id);
                ViewBag.bname = b.BookName;
                return View(b);
            }
            else{
                return RedirectToAction("Login", "Login");
            }
            
        }

        public ActionResult Create(){
            var user_logged_in = HttpContext.Session.GetString("username");
            var logged_in_u = db.RahulUsers.Where(x => x.Username == user_logged_in).SingleOrDefault();
            if(logged_in_u == null){
                return RedirectToAction("Login", "Login");
            }
            else if(logged_in_u.IsAdmin == true){
                return View();
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult Create(RahulBook b){
            var user_logged_in = HttpContext.Session.GetString("username");
            var logged_in_u = db.RahulUsers.Where(x => x.Username == user_logged_in).SingleOrDefault();
            if(logged_in_u == null){
                return RedirectToAction("Login", "Login");
            }
            else if(logged_in_u.IsAdmin == true){
                db.RahulBooks.Add(b);
                db.SaveChanges();
                return RedirectToAction("GetBooks", "Book");
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult Edit(int id){
            var user_logged_in = HttpContext.Session.GetString("username");
            var logged_in_u = db.RahulUsers.Where(x => x.Username == user_logged_in).SingleOrDefault();
            if(logged_in_u == null){
                return RedirectToAction("Login", "Login");
            }
            else if(logged_in_u.IsAdmin == true){
                RahulBook b = db.RahulBooks.Where(x => x.BookId == id).SingleOrDefault();
                return View(b);
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }
        
        [HttpPost]
        public ActionResult Edit(int id, IFormCollection formValues){
            var user_logged_in = HttpContext.Session.GetString("username");
            var logged_in_u = db.RahulUsers.Where(x => x.Username == user_logged_in).SingleOrDefault();
            if(logged_in_u == null){
                return RedirectToAction("Login", "Login");
            }
            else if(logged_in_u.IsAdmin == true){
                var record = db.RahulBooks.Where(x => x.BookId == id).SingleOrDefault();
                record.BookName = Request.Form["bname"];
                record.BookAuthor = Request.Form["bauthor"];
                record.BookQty = Convert.ToInt32(Request.Form["bqty"]);
                db.RahulBooks.Update(record);
                db.SaveChanges();
                return RedirectToAction("GetBooks", "Book");
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public ActionResult Delete(int id){
            var user_logged_in = HttpContext.Session.GetString("username");
            var logged_in_u = db.RahulUsers.Where(x => x.Username == user_logged_in).SingleOrDefault();
            if(logged_in_u == null){
                return RedirectToAction("Login", "Login");
            }
            else if(logged_in_u.IsAdmin == true){
                RahulBook b = db.RahulBooks.Where(x => x.BookId == id).SingleOrDefault();
                db.RahulBooks.Remove(b);
                db.SaveChanges();
                return RedirectToAction("GetBooks", "Book");
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult SearchBooks(IFormCollection f){
            string keyword = f["keyword"].ToString();
            ViewBag.keyword = keyword;
            var result = db.RahulBooks.Where(x => x.BookName.Contains(keyword)).Select(x => x).ToList();
            return View(result);
        }
    }
}
