using System;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;

namespace SS.Core.Security.Authorization
{
    [HandleError]
    [HandleError(ExceptionType = typeof(SecurityException), View = "URNoAuthorized")]
    public class AuthorizationIHttpModule : IHttpModule
    {
        private HttpApplication oApps = null;
        public void Dispose()
        {
            oApps.Dispose();
        }

        public void Init(HttpApplication context)
        {
            oApps = context;
            context.AuthorizeRequest += OnAuthorizeRequest;
        }

        static void OnAuthorizeRequest(object sender, EventArgs e)
        {
            //var context = ((HttpApplication)sender).Context;
            //var routeData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));

            //if (routeData == null) return;
            //if (string.IsNullOrEmpty(context.User.Identity.Name)) return;

            //if (context.User.Identity.IsAuthenticated)
            //{
            //    var controllerName = routeData.GetRequiredString("controller");
            //    var actionName = routeData.GetRequiredString("action");
            //    var userName = context.User.Identity.Name;

            //    var config = new ConfigAuthorized
            //    {
            //        Controller = controllerName,
            //        Action = actionName,
            //        Perfil = Security.GetPerfilByUserName(userName)
            //    };
            //    if (!IsAuthorized(config))
            //    {
            //        context.Response.StatusCode = 401;
            //        context.Response.End();
            //        throw new SecurityException();
            //    }
            //}
            //else
            //{
            //    context.Response.StatusCode = 401;
            //    context.Response.End();
            //}
        }

        private static bool IsAuthorized(ConfigAuthorized config)
        {
            var context = HttpContext.Current;
            XDocument xDoc = null;
            if (context.Cache["ControllerActionsSecurity"] == null)
            {

                var path = HttpContext.Current.Server.MapPath(@"~/MenuActionSecurity.xml");
                xDoc = XDocument.Load(path);
                context.Cache.Insert("ControllerActionsSecurity", xDoc);
            }

            xDoc = (XDocument)context.Cache["ControllerActionsSecurity"];
            var xElement = xDoc.Descendants("menuitem");
            var elements = xElement.Elements();
            var perfil = (from e in elements
                          where
                              ((string) e.Attribute("controller")) == config.Controller &&
                              ((string) e.Attribute("action")).Split(',').Contains(config.Action)
                          select e);

            return perfil.Any(element => PerfilIsValid(element, config.Perfil));
        }
        public static bool PerfilIsValid(XElement perfil, string perfilName)
        {
            if (perfil != null && !string.IsNullOrEmpty(perfilName))
            {
                var xAttribute = perfil.Attribute("perfiles");
                if (xAttribute == null)
                {
                    if (perfil.Parent.Attribute("perfiles").Value.Split(',').Contains(perfilName))
                    {
                        return true;
                    }
                }else
                {
                    if(xAttribute.Value == perfilName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
