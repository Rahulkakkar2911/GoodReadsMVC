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
    public class UserController : Controller
    {
        private readonly ACE42023Context db;
        private readonly ISession session;
        public UserController(ACE42023Context _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            session = httpContextAccessor.HttpContext.Session;
        }

        public ActionResult Create(){
            return View();
        }    
        [HttpPost]
        public ActionResult Create(RahulUser u){
            db.RahulUsers.Add(u);
            db.SaveChanges();
            HttpContext.Session.SetString("username", u.Username);
            return RedirectToAction("GetBooks", "Book");
        }
    }
}
