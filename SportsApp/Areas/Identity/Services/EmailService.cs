using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SportsApp.Areas.Identity.Services
{
    public class EmailService : IEmailSender
    {
        private string host;
        private int port;
        private bool enableSSL;
        private string Email;
        private string password;

        public EmailService(string host, int port, bool enableSSL, string email, string password)
        {
            this.host = host;
            this.port = port;
            this.enableSSL = enableSSL;
            this.Email = email;
            this.password = password;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(Email, password),
                EnableSsl = enableSSL
            };
            return client.SendMailAsync(
                new MailMessage(Email, email, subject, htmlMessage) { IsBodyHtml = true }
            );
        }
    }
}