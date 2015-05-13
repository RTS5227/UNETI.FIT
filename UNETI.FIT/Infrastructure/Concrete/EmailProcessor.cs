using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using UNETI.FIT.Models.Entity;

namespace UNETI.FIT.Infrastructure.Concrete
{
    public class EmailContactProcessor : IContactProcessor
    {
        private EmailSettings emailSettings;

        public EmailContactProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }


        public void ProcessContact(Email model)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress, // From
                    model.To, // To
                    model.Subject, // Subject
                    model.Message); // Body

                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }
        }
    }

    public interface IContactProcessor
    {
        void ProcessContact(Email model);
    }

    public class EmailSettings
    {
        public string MailFromAddress = "tink5uneti@gmail.com";
        public bool UseSsl = true;
        public string Username = "tink5uneti";
        public string Password = "k5tinuneti";
        public string ServerName = "smtp.gmail.com";
        public int ServerPort = 587;
        public bool WriteAsFile = false;
        public string FileLocation = @"c:\uneti_fit_emails";
    }
}