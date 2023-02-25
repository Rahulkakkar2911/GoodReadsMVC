using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ACE42023Context db;
        private readonly ISession session;
        public LoginController(ACE42023Context _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            session = httpContextAccessor.HttpContext.Session;
        }

        public ActionResult Login(){
            return View();
        }

        [HttpPost]
        public IActionResult Login(RahulUser u){
            
            var result = (from i in db.RahulUsers
                            where  i.Userid == u.Userid && i.Userpassword == u.Userpassword
                            select i).SingleOrDefault();
            
            if(result != null){
                HttpContext.Session.SetString("username", result.Username);
                HttpContext.Session.SetString("uid", result.Userid.ToString());

                if(result.IsAdmin == true){
                    HttpContext.Session.SetString("IsAdmin", "yes");
                }
                
                return RedirectToAction("GetBooks", "Book");
            }
            else{
                return View();
            }
        }

        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Login");
        }
        
    }
}
