using Kutse_App_Vsevolod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Kutse_App_Vsevolod.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Ootan sind minu peole! Palun tule!!!";
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 10 ? "Tere hommikust!" : "Tere päevast!";
            return View();
        }
        [HttpGet]

        public ViewResult Ankeet()
        {
            return View(); 
        }
        [HttpPost]
        public ViewResult Ankeet(Guest guest)
        {
            E_mail(guest); // Функция для отправки письма с ответом
            if (ModelState.IsValid)
            {
                return View("Thanks", guest);
            }
            else
            {
                return View();
            }
        }
        public void E_mail(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "seva.tsarev@gmail.com";
                WebMail.Password = "dhuj iozk xjwk sugy";
                WebMail.From = "seva.tsarev@gmail.com";
                WebMail.Send("seva.tsarev@gmail.com", "Vastus kutsele", guest.Name + " Vastus" + ((guest.WillAttend ?? false) ?
                    "tuleb poele " : "ei tule poele"));
                ViewBag.Message = "Kiri on saatnud!";

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Mul on kuhju! Ei saa kirja saada!!!";
            }
        }
    }
}