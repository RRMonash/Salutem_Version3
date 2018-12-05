using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Salutem_Version3.Utils
{
    public class EmailSender
    {
        // Please use your API KEY here.
        private const String API_KEY = "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";

        public void Send(String toEmail, String subject, String contents)
        {
            // var client = new SendGridClient(API_KEY);
            //var from = new EmailAddress("securelzw@gmail.com", "Invitation from Salutem");
            //var subject = "Notification of Booking Confirmation";

            //var user = db.Applicants.Find(booking.ApplicantId);
            //var to = new EmailAddress("raut145@gmail.com", user.Email);
            var TempData = "Congratulations!!! You've successfully booked for this Event.";
            var cont = "<h4> Hi !" + "</h4>" + TempData + "<h5>Regards Salutem </h5>";
            // var plainTextContent = contents;
            //var htmlContent = "<p>" + contents + "</p>";
            var client = new SendGridClient(API_KEY);
            var from = new EmailAddress("securelzw@gmail.com", "Invitation from Salutem");
            var to = new EmailAddress(toEmail, "");
            var plainTextContent = contents + cont;
            var htmlContent = "<p>" + contents + "</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

    }
}