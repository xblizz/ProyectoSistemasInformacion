using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SS.Core.Security.Authorization
{
    public class ConfigAuthorized
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Perfil { get; set; }
    }
}
