using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Salutem_Version3.Models;
using System.Net.Mail;
using System.Threading.Tasks;
using Salutem_Version3.Utils;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace Salutem_Version3.Controllers
{
    public class EventBookingsController : Controller
    {
        private Salutem_Model db = new Salutem_Model();
        private const String API_KEY = "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";
        EmailSender es = new EmailSender();
        // GET: EventBookings
        public ActionResult Index()
        {
            var eventBookings = db.EventBookings.Include(e => e.Applicant).Include(e => e.Event);
            return View(eventBookings.ToList());
        }
        public ActionResult MyBooking()
        {
            string currentUserId = User.Identity.GetUserId();
            var bookingList = db.EventBookings.Where(a => a.Applicant.UserId == currentUserId).Include(b => b.Applicant).Include(b => b.Event);
            return View(bookingList.ToList());
        }

        // GET: EventBookings/Create
        public ActionResult Create()
        {
            string currentUserId = User.Identity.GetUserId();
            var userName = db.Applicants.Where(a => a.UserId == currentUserId)
                .Select(a => new SelectListItem
                {
                    Value = a.ApplicantId.ToString(),
                    Text = a.FirstName
                });

            ViewBag.ApplicantId = new SelectList(userName, "Value", "Text");

            var events = db.Events
                .Select(a => new SelectListItem
                {
                    Value = a.EventId.ToString(),
                    Text = a.EventName
                });

            ViewBag.EventId = new SelectList(events, "Value", "Text");
            return View();
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Contact(EventBooking model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
        //        var message = new MailMessage();
        //        //message.To.Add(new MailAddress("recipient@gmail.com"));  // replace with valid value 
        //        message.To.Add(new MailAddress("securelzw@gmail.com"));  // replace with valid value 
        //        //message.From = new MailAddress("sender@outlook.com");  // replace with valid value
        //        message.From = new MailAddress("rrau0001@student.monash.edu");  // replace with valid value
        //        message.Subject = "Your email subject";
        //        message.Body = string.Format(body, "securelzw@gmail.com", "rrau0001@student.monash.edu", "asbnjhgasasjd"); //model.FromName, model.FromEmail, model.Message
        //        message.IsBodyHtml = true;
        //        //if (model.Upload != null && model.Upload.ContentLength > 0)
        //        //{
        //        //    message.Attachments.Add(new Attachment(model.Upload.InputStream, System.IO.Path.GetFileName(model.Upload.FileName)));
        //        //}
        //        using (var smtp = new SmtpClient())
        //        {
        //            var credential = new NetworkCredential
        //            {
        //                UserName = "rrau0001@student.monash.edu",  // replace with valid value
        //                Password = "debbyBabu%95Bhujing"  // replace with valid value
        //            };
        //            smtp.Credentials = credential;
        //            //smtp.Host = "smtp-mail.outlook.com";
        //            smtp.Host = "smtp.monash.edu.au";
        //            //smtp.Port = 587;
        //            //smtp.EnableSsl = true;
        //            await smtp.SendMailAsync(message);
        //            return RedirectToAction("Sent");
        //        }
        //    }
        //    return View(model);
        //}
        // GET: EventBookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = db.EventBookings.Find(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // POST: EventBookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ApplicantId,EventId,Id")] EventBooking booking)
        {
            bool success = (db.EventBookings.Any(a => a.ApplicantId == booking.ApplicantId && a.EventId == booking.EventId));

            if (!success)
            {
                if (ModelState.IsValid)
                {
                    db.EventBookings.Add(booking);
                    db.SaveChanges();
                    //ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                    //var client = new SendGridClient(API_KEY);
                    //var from = new EmailAddress("securelzw@gmail.com", "Invitation from Salutem");
                    var subject = "Notification of Booking Confirmation";

                    var user = db.Applicants.Find(booking.ApplicantId);
                    //var to = new EmailAddress("raut145@gmail.com", user.Email);
                    //TempData["message"] = "Congratulations!!! You've successfully booked for this Event.";
                    var eventName = db.Events.Find(booking.EventId);
                    var contents = "<h4> Hi " + user.FirstName + "</h4>" + "Congratulations!!! You've successfully booked for " + eventName.EventName + "<h5>Regards Salutem </h5>";
                    // var plainTextContent = contents;
                    //var htmlContent = "<p>" + contents + "</p>";
                    //var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    //var response =  client.SendEmailAsync(msg);

                    es.Send(user.Email, subject, contents);

                    TempData["msg"] = "<script>alert('Congratulations!!! You've successfully booked for this Event.');</script>";
                    return RedirectToAction("UserIndex", "Events");
                }
            }
            else
            {
                TempData["message"] = "Congratulations!!! You're already enrolled for this Event.";
                TempData["msg"] = "<script>alert('Sorry! You're already booked for this Event.');</script>";
            }

            string currentUserId = User.Identity.GetUserId();
            var fullName = db.Applicants.Where(m => m.UserId == currentUserId)
                .Select(a => new SelectListItem
                {
                    Value = a.ApplicantId.ToString(),
                    Text = a.FirstName + " " + a.LastName
                });

            ViewBag.ApplicantId = new SelectList(fullName, "Value", "Text");
            var events = db.Events
                .Select(a => new SelectListItem
                {
                    Value = a.EventId.ToString(),
                    Text = a.EventName
                });
            ViewBag.EventId = new SelectList(events, "Value", "Text");

            return View(booking);
        }
        // GET: EventBookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = db.EventBookings.Find(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "FirstName", eventBooking.ApplicantId);
            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventName", eventBooking.EventId);
            return View(eventBooking);
        }

        // POST: EventBookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ApplicantId,EventId,Id")] EventBooking eventBooking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eventBooking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ApplicantId = new SelectList(db.Applicants, "ApplicantId", "FirstName", eventBooking.ApplicantId);
            ViewBag.EventId = new SelectList(db.Events, "EventId", "EventName", eventBooking.EventId);
            return View(eventBooking);
        }

        // GET: EventBookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EventBooking eventBooking = db.EventBookings.Find(id);
            if (eventBooking == null)
            {
                return HttpNotFound();
            }
            return View(eventBooking);
        }

        // POST: EventBookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EventBooking eventBooking = db.EventBookings.Find(id);
            db.EventBookings.Remove(eventBooking);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    //internal class Salutem_Version3
    //{
    //    private static void Main()
    //    {
    //        Execute().Wait();
    //    }

    //    static async Task Execute()
    //    {
    //        var apikey = environment.getenvironmentvariable("sendgrid_api_key");
    //        var client = new sendgridclient(apikey);
    //        var from = new emailaddress("securelzw@gmail.com", "example user");
    //        var subject = "sending with sendgrid is fun";
    //        var to = new emailaddress("raut145@gmail.com", "example user");
    //        var plaintextcontent = "and easy to do anywhere, even with c#";
    //        var htmlcontent = "<strong>and easy to do anywhere, even with c#</strong>";
    //        var msg = mailhelper.createsingleemail(from, to, subject, plaintextcontent, htmlcontent);
    //        var response = await client.sendemailasync(msg);
    //    }
    //}
}
