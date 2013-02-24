using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace SS.Core.Mailer
{
    internal class ConfigService
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string ServerUser { get; set; }
        public string ServerPassword { get; set; }
        public bool SSL { get; set; }


        protected internal ConfigService()
        {
            var c = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            var settings = (MailSettingsSectionGroup)c.GetSectionGroup("system.net/mailSettings");

            if (settings != null)
            {
                Server = settings.Smtp.Network.Host;
                Port = settings.Smtp.Network.Port;
                ServerUser = settings.Smtp.Network.UserName;
                ServerPassword = settings.Smtp.Network.Password;
                FromAddress = settings.Smtp.From;
                SSL = settings.Smtp.Network.EnableSsl;
            }

        }

       

    }
}
