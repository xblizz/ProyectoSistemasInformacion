using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using SS.Core.Entities;
using SS.Core.Mailer;
using SS.Core.Security;
using SistemaSeguridad.Enums;
using SistemaSeguridad.Models;
using SS.Core.Security.Authorization;

namespace SistemaSeguridad.Controllers
{
    public class AccountController : Controller
    {
        RiinContainer db = new RiinContainer();
        //
        // GET: /Login/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RecuperaContraseña()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CambiaContraseña(PasswordModel password)
        {
            if (ModelState.IsValid)
            {
                var usuario = Security.GetUserByUserName(Session["userName"].ToString());
                if (usuario != null)
                {
                    usuario.Password = password.NewPassword;
                    db.SaveChanges();

                }
            }

            return View("CambioContraseña");
        }

        [RequireAuthorization]
        public ActionResult CambioContraseña()
        {
            return View();
        }

        // POST: /Account/LogOn
        [HttpPost]
        public JsonResult LogIn(LoginModel login)
        {
            var errorResult = string.Empty;
            bool status = false;

            var userName = login.Username;
            if (ModelState.IsValid)
            {

                if (Security.ExistUser(userName) && Security.ValidateUser(userName, login.Password))
                {
                    
                    var usuario = Security.GetUserByUserName(userName);
                    if (usuario.Activo == false)
                    {
                        status = false;
                        errorResult = "dialog-Inactivo";
                    }
                    else
                    {
                        var usr = db.Usuarios.Find(usuario.Id);
                        FormsAuthentication.SetAuthCookie(userName, false);

                        usr.FechaUltimoLogin = DateTime.Now;
                        usr.Bloqueado = false;
                        db.Entry(usr).State = EntityState.Modified;
                        db.SaveChanges();

                        Session["userNameId"] = usuario.Id;
                        Session["userName"] = userName;
                        Session["fullNameUser"] = string.Format("{0} {1}", usuario.Nombre, usuario.ApellidoPaterno);

                        status = true;
                        errorResult = string.Empty;
                    }
                }
                else
                {
                    if (Session["IntentosFallidos"] == null)
                        Session.Add("IntentosFallidos", 0);

                    var intentosLogin = Session["IntentosFallidos"] as int?;
                    if (intentosLogin.HasValue)
                        intentosLogin += 1;

                    status = false;
                    errorResult = "dialog-usuariopassword";

                    if (Security.ExistUser(userName))
                    {
                        var maximoIntentos = Convert.ToInt32(ParametrosSistemaEnums.IntentosFallidosLogin);
                        var intentosFallidos = (from param in db.ParametrosSistemaEmpresa
                                                where param.ParametrosSistemaId == maximoIntentos
                                                select param.ValorInicial);
                        if (intentosLogin >= intentosFallidos.First())
                        {
                            var usuario = Security.GetUserByUserName(userName);
                            var usr = db.Usuarios.Find(usuario.Id);
                            usr.Bloqueado = true;
                            db.Entry(usr).State = EntityState.Modified;
                            db.SaveChanges();


                            status = false;
                            errorResult = "dialog-Bloqueo";
                        }
                        Session["IntentosFallidos"] = intentosLogin;
                    }
                }
            }
            return Json(new object[] { status, errorResult });
        }

        [Authorize]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Account");
        }

        [HttpPost]
        public JsonResult RecuperaContraseña(RetrivePasswordModel retrievePassword)
        {
            var response = 1;

            if (ModelState.IsValid)
            {
                var successfullSending = 2;
                var errorSending = 4;

                if (Security.ExistUser(retrievePassword.Username))
                {
                    var usuario = Security.GetUserByUserName(retrievePassword.Username);
                    if (usuario.Activo)
                    {
                        if (retrievePassword.Email.ToUpper() == usuario.Email.ToUpper())
                        {
                            var mail = new ServiceMailer();
                            var message = new Email();

                            message.To.Add(usuario.Email);
                            message.Subject = "Recuperación de Contraseña.";
                            message.Body = "Su contraseña de acceso es: " + Security.Decript(usuario.Password);

                            response = mail.SendMail(message) ? successfullSending : errorSending;
                        }
                        else
                        {
                            response = 5;
                        }
                    }
                    else
                    {
                        response = 3;
                    }
                }
            }

            return Json(response);
        }
    }
}
