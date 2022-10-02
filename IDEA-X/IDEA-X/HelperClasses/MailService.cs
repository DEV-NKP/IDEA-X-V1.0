using FluentEmail.Core;
using FluentEmail.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace IDEA_X.HelperClasses
{
    public static class MailService
    {
        public static async Task<bool> SendMail(string email,string code)
        {
            string pass = "cezsxtphsghceajb";
            string senderEmail = "system.confirmation.validity@gmail.com";


            var fromAddress = new MailAddress(senderEmail, "IDEA-X");
            var toAddress = new MailAddress(email, email);
            string fromPassword = pass;
            string subject = "Verification Process For IDEA-X";
            string body = "Your verification code is : " + code;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            })
            {

                smtp.Send(message);
            }


            return true;

        }
    }
}