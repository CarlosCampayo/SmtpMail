using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmtpMail.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SmtpMail.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration Configuration;
        public HomeController(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(String receptor, String asunto, String mensaje)
        {
            MailMessage mail = new MailMessage();
            String usermail = this.Configuration["usuariomail"];
            String passwordmail = this.Configuration["passwordmail"];
            mail.From = new MailAddress(usermail);
            mail.To.Add(receptor);
            mail.Subject = asunto;
            mail.Body = "<h2 style=\"color:red;\">" + mensaje + "</h2>";
            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            SmtpClient smtpClient = new SmtpClient();
            //"host": "smtp.office365.com",
            smtpClient.Host = this.Configuration["host"];
            //"puerto": "587",
            smtpClient.Port = int.Parse(this.Configuration["puerto"]);
            //"ssl": "true",
            smtpClient.EnableSsl = bool.Parse(this.Configuration["ssl"]);
            //"defaultcredentials": "true"
            smtpClient.UseDefaultCredentials = bool.Parse(this.Configuration["defaultcredentials"]);
            NetworkCredential credential =
                new NetworkCredential(usermail, passwordmail);
            smtpClient.Credentials = credential;
            smtpClient.Send(mail);
            ViewBag.Mensaje = "Mensaje enviado a " + receptor;
            return View();
        }
    }
}
