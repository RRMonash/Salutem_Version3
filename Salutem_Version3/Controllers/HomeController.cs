using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Salutem_Version3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //EmailSender es = new EmailSender();
            //es.RegisterAPIKey();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        //public ActionResult Send(String toEmail)
        //{
        //    private const String API_KEY = "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";
        //    var client = new SendGridClient(API_KEY);
        //    var from = new EmailAddress("noreply@localhost.com", "FIT5032 Example Email User");
        //    var to = new EmailAddress(toEmail, "");
        //    var subject = "Sending with SendGrid is Fun";
        //     //var to = new EmailAddress("test@example.com", "Example User");
        //    var plainTextContent = "and easy to do anywhere, even with C#";
       
        //    var htmlContent = "<p>" + contents + "</p>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        
        //    return View();
        //}   

    }
}