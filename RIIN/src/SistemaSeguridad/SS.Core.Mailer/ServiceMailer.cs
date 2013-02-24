using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Net.Mail;

namespace SS.Core.Mailer
{
    public class ServiceMailer
    {
        public bool SendMail(Email email)
        {
            bool response = false;
            var config = new ConfigService();


            try
            {
                var message = new MailMessage();

                message.From = string.IsNullOrEmpty(email.From)
                                   ? new MailAddress(config.FromAddress)
                                   : new MailAddress(email.From);

                foreach (var to in email.To)
                {
                    message.To.Add(to);
                }
                foreach (var cc in email.Cc)
                {
                    message.CC.Add(cc);
                }
                message.Subject = email.Subject;
                message.Body = email.Body;

                var client = new SmtpClient(config.Server, config.Port);
                client.EnableSsl = config.SSL;
                var credential = new NetworkCredential(config.ServerUser, config.ServerPassword);
                client.Credentials = credential;
              
                response = true;
                
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                throw new Exception(ex.InnerException.Message);
            }
            return response;
        }
    }
}
			