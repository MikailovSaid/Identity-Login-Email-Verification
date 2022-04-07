using EntityFramework.Data;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityFramework.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmai(string emailAddress, string url)
        {

            try
            { 
                var apiKey = "";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("bakhtiyaria@code.edu.az", "Example User");
                var subject = "Sending with SendGrid is Fun";
                var to = new EmailAddress(emailAddress, "Example User");
                var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = $"<a href={url}>Click Here</a>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
