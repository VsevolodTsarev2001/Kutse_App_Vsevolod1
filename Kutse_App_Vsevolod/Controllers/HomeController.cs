using Kutse_App_Vsevolod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.IO;
using System.Net.Mail;


namespace Kutse_App_Vsevolod.Controllers
{
    public class HolidayInfo
    {
        public string Message { get; set; }
        public string ImagePath { get; set; }
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            HolidayInfo holidayInfo = GetHolidayMessage(); // Get the holiday info with message and image
            ViewBag.HolidayInfo = holidayInfo;

            int hour = DateTime.Now.Hour;
            // Update greeting based on time of day (four times of the day)
            if (hour < 6)
            {
                ViewBag.Greeting = "Head ööd, kallis sõber!"; // Night
            }
            else if (hour < 12)
            {
                ViewBag.Greeting = "Tere hommikust, kallis sõber!"; // Morning
            }
            else if (hour < 18)
            {
                ViewBag.Greeting = "Tere päevast, kallis sõber!"; // Afternoon
            }
            else
            {
                ViewBag.Greeting = "Tere õhtust, kallis sõber!"; // Evening
            }

            return View();
        }

        private HolidayInfo GetHolidayMessage()
        {
            int month = DateTime.Now.Month;
            HolidayInfo holidayInfo = new HolidayInfo
            {
                Message = "Tere tulemast meie peole!", // Default message
                ImagePath = "~/Images/default.jpg" // Default image
            };

            switch (month)
            {
                case 1:
                    holidayInfo.Message = "Head uut aastat! Tere tulemast uude aastasse!"; // New Year (January)
                    holidayInfo.ImagePath = "~/Images/newyear.jpg"; // New Year image
                    break;
                case 2:
                    holidayInfo.Message = "Kena sõbrapäeva! Ootan sind peole!"; // Valentine's Day (February)
                    holidayInfo.ImagePath = "~/Images/valentine.jpg"; // Valentine's Day image
                    break;
                case 3:
                    holidayInfo.Message = "Head kevadet! Tule ja tähistame koos!"; // Spring (March)
                    holidayInfo.ImagePath = "~/Images/spring.jpg"; // Spring image
                    break;
                case 4:
                    holidayInfo.Message = "Head aprillinalja! Ole valmis naljadeks!"; // April Fool's Day (April)
                    holidayInfo.ImagePath = "~/Images/aprilfools.png"; // April Fool's Day image
                    break;
                case 5:
                    holidayInfo.Message = "Head emadepäeva! Täname emasid!"; // Mother's Day (May)
                    holidayInfo.ImagePath = "~/Images/mothersday.jpg"; // Mother's Day image
                    break;
                case 6:
                    holidayInfo.Message = "Kena suve algust! Tere suvepidu!"; // Summer (June)
                    holidayInfo.ImagePath = "~/Images/summer.jpg"; // Summer image
                    break;
                case 7:
                    holidayInfo.Message = "Head Jaanipäeva! Tule ja naudi meie suvepidu!"; // Jaanipäev (Midsummer Day, Estonia)
                    holidayInfo.ImagePath = "~/Images/jaanipaev.jpg"; // Jaanipäev image (image of a bonfire or something related to Midsummer)
                    break;
                case 8:
                    holidayInfo.Message = "Tere sügis! Naudi õunapuu alla pidu!"; // Fall (August)
                    holidayInfo.ImagePath = "~/Images/fall.jpg"; // Fall image
                    break;
                case 9:
                    holidayInfo.Message = "Tere sügis! Tähistame uue hooaja algust!"; // Autumn (September)
                    holidayInfo.ImagePath = "~/Images/autumn.jpg"; // Autumn image
                    break;
                case 10:
                    holidayInfo.Message = "Tere halloween! Tule ja naudi pidu!"; // Halloween (October)
                    holidayInfo.ImagePath = "~/Images/halloween.jpg"; // Halloween image
                    break;
                case 11:
                    holidayInfo.Message = "Head Iseseisvuspäeva! Täna on meie päeva tähistamine!"; // Iseseisvuspäev (Independence Day, Estonia) - 24th of February
                    holidayInfo.ImagePath = "~/Images/iseseisvuspäev.jpg"; // Iseseisvuspäev image (image of Estonia's flag or other related images)
                    break;
                case 12:
                    holidayInfo.Message = "Häid jõulupühi ja head uut aastat!"; // Christmas and New Year (December)
                    holidayInfo.ImagePath = "~/Images/christmas.jpg"; // Christmas image
                    break;
                default:
                    holidayInfo.Message = "Tule ja naudi pidu!"; // Default message
                    holidayInfo.ImagePath = "~/Images/kutse.jpg"; // Default image
                    break;
            }

            return holidayInfo;
        }
        GuestContext db = new GuestContext();
        
        public ActionResult Guests()
        {
            IEnumerable<Guest> guests = db.Guests;
            return View (guests);
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
                db.Guests.Add(guest);
                db.SaveChanges();
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
                WebMail.Password = "zqpz hgve dbba tcou"; 
                WebMail.From = "seva.tsarev@gmail.com";

                string message = guest.Name + " Vastus" + ((guest.WillAttend ?? false) ? " tuleb poele " : " ei tule poele");
                WebMail.Send(guest.Email, "Vastus kutsele", message);

                if (guest.WillAttend == true)
                {
                    string reminderMessage = $"{guest.Name}, ära unusta. Pidu toimub 12.03.20! Sind ootavad väga!";

                    string attachmentPath = Path.Combine(Server.MapPath("~/Images/"), "kutse.jpg");

                    if (System.IO.File.Exists(attachmentPath))
                    {
                        WebMail.Send(
                            guest.Email,
                            "Meeldetuletus",
                            reminderMessage,
                            null,
                            guest.Email,
                            filesToAttach: new[] { attachmentPath }
                        );

                        ViewBag.Message = "Meeldetuletus on saadetud!";
                    }
                    else
                    {
                        ViewBag.Message = "Pilt ei ole saadaval!";
                    }
                }
                else
                {
                    ViewBag.Message = "Kahjuks ei saa tulla!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Vabandust, meeldetuletuse saatmisel tekkis viga. " + ex.Message;
            }
        }
        [HttpPost]
        public ActionResult SendReminder(Guest guest)
        {
            try
            {
                WebMail.SmtpServer = "smtp.gmail.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "seva.tsarev@gmail.com";
                WebMail.Password = "zqpz hgve dbba tcou";
                WebMail.From = "seva.tsarev@gmail.com";

                // Reminder message
                string reminderMessage = $"{guest.Name}, ära unusta. Pidu toimub 12.03.20! Sind ootavad väga!";

                string attachmentPath = Path.Combine(Server.MapPath("~/Images/"), "kutse.jpg");

                if (System.IO.File.Exists(attachmentPath))
                {
                    WebMail.Send(
                        guest.Email, 
                        "Meeldetuletus",
                        reminderMessage,
                        null,
                        guest.Email,
                        filesToAttach: new[] { attachmentPath }
                    );

                    ViewBag.Message = "Meeldetuletus on saadetud!";
                }
                else
                {
                    ViewBag.Message = "Pilt ei ole saadaval!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Vabandust, meeldetuletuse saatmisel tekkis viga. " + ex.Message;
            }

            return View("Thanks", guest);
        }
    }
}