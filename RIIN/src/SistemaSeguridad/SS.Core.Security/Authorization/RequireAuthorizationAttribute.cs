using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;
using System.Xml.Linq;
using System.Security;

namespace SS.Core.Security.Authorization
{
    [HandleError(ExceptionType = typeof(SecurityException), View = "URNoAuthorized")]
    public class RequireAuthorization : AuthorizeAttribute
    {
        // the "new" must be used here because we are hiding
        // the Roles property on the underlying class
        public new PerfilesEnum Roles;

        public RequireAuthorization()
            : base()
        { }

        protected override bool  AuthorizeCore(HttpContextBase httpContext)
        {
            var context = httpContext;
            var routeData = RouteTable.Routes.GetRouteData(context);

            if (routeData == null) return false;
            if (string.IsNullOrEmpty(context.User.Identity.Name)) return false;

            if (context.User.Identity.IsAuthenticated)
            {
                var controllerName = routeData.GetRequiredString("controller");
                var actionName = routeData.GetRequiredString("action");
                var userName = context.User.Identity.Name;

                var config = new ConfigAuthorized
                {
                    Controller = controllerName.ToLower(),
                    Action = actionName.ToLower(),
                    Perfil = Security.GetPerfilByUserName(userName).ToLower()
                };
                if (!IsAuthorized(config))
                {
                    context.Response.StatusCode = 530;
                    context.Response.TrySkipIisCustomErrors = true;
                    context.Response.End();
                    return false;
                }
                return true;
            }
            return false;            
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
            filterContext.Controller.TempData["ReferrerMessage"] = "";
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
                              ((string) e.Attribute("controller")).ToLower() == config.Controller &&
                              ((string)e.Attribute("action")).ToLower().Split(',').Contains(config.Action)
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
                    if (perfil.Parent.Attribute("perfiles").Value.ToLower().Split(',').Contains(perfilName))
                    {
                        return true;
                    }
                }else
                {
                    if(xAttribute.Value.ToLower() == perfilName)
                    {
                        return true;
                    }
                }                                                                                                                                                                                                                                                                                                                                                                                                           
            }
            return false;
        }
    }
}