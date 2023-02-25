using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
// using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
// using System.Linq;
using Microsoft.Extensions.Logging;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;



namespace LibraryManagement.Controllers
{
    public class BookingController : Controller
    {

        private readonly ACE42023Context db;
        public BookingController(ACE42023Context _db){
            db = _db;
        }

        public ActionResult Book(){
            return View();
        }

        [HttpPost]
        public ActionResult Book(int id){
            var usern = HttpContext.Session.GetString("username");
            ViewBag.username = HttpContext.Session.GetString("username");
            
            if(usern != null){
                //Decreasing a book quantity
                RahulBook b = db.RahulBooks.Find(id);
                RahulUser user = db.RahulUsers.Where(x => x.Username == usern).SingleOrDefault();
                
                List<RahulBooking> bookings_of_user = new List<RahulBooking>();
                foreach (var item in db.RahulBookings)
                {
                    if(item.Bid == id && item.Uid == user.Userid){
                        ViewBag.error = "Book was Already Present!";
                        return View();
                    }                
                }
                // if no bookings are there or if there is no booking with the current book id.

                // checking if the book is now outofstock
                if((b.BookQty) == 0){
                    ViewBag.error = "Out of Stock!";
                    return View();
                }
                b.BookQty = b.BookQty - 1;
                db.RahulBooks.Update(b);                   
                    //adding new Booking
                RahulBooking newBooking = new RahulBooking();
                newBooking.Uid = user.Userid;
                newBooking.Bid = id;
                db.RahulBookings.Add(newBooking);
                db.SaveChanges();
                    
                return RedirectToAction("ViewBookings", "Booking", new {uid = user.Userid});
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

        public ActionResult ViewBookings(int uid){
            uid = Convert.ToInt32(uid);
            var user = HttpContext.Session.GetString("username");
            ViewBag.user = user;

            var u = db.RahulUsers.Where(x => x.Username == user).SingleOrDefault();

            if(user != null){
                var result = (from o in db.RahulBookings.Include(x=>x.BidNavigation) where o.Uid == uid select o).ToList();
                return View(result);
            }
            else{
                return RedirectToAction("Login", "Login");
            }


        }

        [HttpPost]
        public ActionResult Delete(int id){

            var usern = HttpContext.Session.GetString("username");            
            if(usern != null){
                //Finding the booking
                RahulBooking booking = db.RahulBookings.Find(id);
                var u = db.RahulUsers.Where(x => x.Username == usern).SingleOrDefault();
                int? bookid = booking.Bid;
                ViewBag.booking_id = booking.Bookingid;
                //Delete the Booking            
                db.RahulBookings.Remove(booking);
                
                //Increasing a book quantity
                RahulBook b = db.RahulBooks.Find(bookid);
                b.BookQty = b.BookQty + 1;
                db.RahulBooks.Update(b);
                db.SaveChanges();
    
                return RedirectToAction("ViewBookings", "Booking", new {uid = u.Userid});
            }
            else{
                return RedirectToAction("Login", "Login");
            }
        }

    }
}





















