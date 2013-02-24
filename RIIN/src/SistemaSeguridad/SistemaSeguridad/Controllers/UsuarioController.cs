using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Transactions;

using SS.Core.Entities;
using SS.Core.Security;
using SistemaSeguridad.Helpers;
using SistemaSeguridad.Models;
using Usuario = SS.Core.Entities.Usuario;

namespace SistemaSeguridad.Controllers
{
    //[Authorize]
    public class UsuarioController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();
        private readonly HelperContainer dbHelper = new HelperContainer();
      
        public JsonResult GetUsers(bool isFiltered, string grupo, int empresa, int perfil, string nombre)
        {
            if (isFiltered)
            {
                return GetUsuariosFiltered(grupo, empresa, perfil, nombre);
            }
            else
            {
                return GetAllUsers();
            }

        }

        public JsonResult GetAllUsers()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);
 
            var usuarios = (from usuario in db.Usuarios.Include("UsuarioEmpresa") select usuario);

            var listaPaginada = usuarios.OrderBy(c => c.UserName)
                                      .Skip((paginaActual - 1) * registrosPorPagina)
                                      .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;
            var empresas= string.Empty;
            

            girdResult.total = usuarios.Count();
            girdResult.page = paginaActual;
            foreach (var usuario in listaPaginada)
            {
                empresas = usuario.UsuarioEmpresa.Aggregate(empresas, (current, empresa) => current + (empresa.Empresa.Nombre + ", "));
                empresas = empresas.Length > 2 ? empresas.Substring(0, empresas.Length - 2) : string.Empty;

               
                int empresaId = 0;
                foreach (var empresa in usuario.UsuarioEmpresa)
                {
                    empresaId = Convert.ToInt32(empresa.Empresa.GrupoId);
                    break;
                }



                var gpo = db.Empresas.Where(grupo => grupo.Id.Equals(empresaId)).FirstOrDefault();
                string nombreGrupo= string.Empty;

                if (gpo != null)
                {
                    nombreGrupo = gpo.Nombre;
                }

                i += 1;
                miRow = new GridRow();
                miRow.id = i;
                miRow.cell = new List<string>()
                                 {
                                     Convert.ToString(usuario.Id),
                                     usuario.UserName,
                                     nombreGrupo,
                                     empresas,
                                     string.Format("{0} {1} {2}", usuario.Nombre, usuario.ApellidoPaterno,
                                                   usuario.ApellidoMaterno),
                                     usuario.Email,
                                     Convert.ToString(usuario.Activo),
                                     usuario.FechaAlta.ToString("MMMM dd, yyyy"),
                                     Convert.ToString(usuario.FechaBaja),
                                     Convert.ToString(usuario.FechaUltimoLogin)
                                 };
                girdResult.rows.Add(miRow);
                empresas = string.Empty;
            }
            return Json(girdResult);
        }

        public JsonResult GetUsuariosFiltered(string grupo, int Empresa, int perfil, string nombre)
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            //UsuarioEmpresa emp = new UsuarioEmpresa();
            //if(Empresa != 0)
            //{
            //    emp = db.relUsuarioEmpresa.Find(Empresa);&& usuario.UsuarioEmpresa.Contains(emp)
            //}
            var usuarios = (from usuario in db.Usuarios.Include("UsuarioEmpresa")
                            where
                                (usuario.Nombre.Contains(nombre) || usuario.ApellidoPaterno.Contains(nombre) ||
                                 usuario.ApellidoMaterno.Contains(nombre)) 
                            select usuario);

            var listaPaginada = usuarios.OrderBy(c => c.UserName)
                          .Skip((paginaActual - 1) * registrosPorPagina)
                          .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;
            var empresas = string.Empty;


            girdResult.total = usuarios.Count();
            girdResult.page = paginaActual;
            foreach (var usuario in listaPaginada)
            {

                empresas = usuario.UsuarioEmpresa.Aggregate(empresas, (current, empresa) => current + (empresa.Empresa.Nombre + ", "));
                empresas = empresas.Length > 2 ? empresas.Substring(0, empresas.Length - 2) : string.Empty;


                int empresaId = 0;
                foreach (var empresa in usuario.UsuarioEmpresa)
                {
                    empresaId = Convert.ToInt32(empresa.Empresa.GrupoId);
                    break;
                }



                var gpo = db.Empresas.Where(grupoid => grupoid.Id.Equals(empresaId)).FirstOrDefault();
                string nombreGrupo = string.Empty;

                if (gpo != null)
                {
                    nombreGrupo = gpo.Nombre;
                }

                i += 1;
                miRow = new GridRow();
                miRow.id = i;
                miRow.cell = new List<string>()
                                 {
                                     Convert.ToString(usuario.Id),
                                     usuario.UserName,
                                     nombreGrupo,
                                     empresas,
                                     string.Format("{0} {1} {2}", usuario.Nombre, usuario.ApellidoPaterno,
                                                   usuario.ApellidoMaterno),
                                     usuario.Email,
                                     Convert.ToString(usuario.Activo),
                                     usuario.FechaAlta.ToShortDateString(),
                                     Convert.ToString(usuario.FechaBaja),
                                     Convert.ToString(usuario.FechaUltimoLogin)
                                 };
                girdResult.rows.Add(miRow);
                empresas = string.Empty;
            }
            return Json(girdResult);
        }

        public ViewResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        public JsonResult ExistUser(string username)
        {
            if(Security.ExistUser(username))
            {
                var res = "El nombre de usuario ya existe.";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult Create()
        {
            ViewData["ddGrupos"] = new SelectList(new[] { "(Selecciona)" });
            ViewData["ddEmpresas"] = new SelectList(new[] { "(Selecciona)" });
            ViewData["ddPerfiles"] = new SelectList(new[] { "(Selecciona)" });
            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            var res = 0;

            if (ModelState.IsValid)
            {
                if (Security.ExistUser(usuario.UserName))
                {
                    return Json(res);
                }
                using (var transaction = new TransactionScope())
                {
                    try
                    {
                        usuario.FechaAlta = DateTime.Now;
                        usuario.Password = Security.Encript(usuario.Password);
                        var desc = Security.Decript(usuario.Password);
                        usuario.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);
                        db.Usuarios.Add(usuario);
                        
                        foreach (Int32 id in usuario.PerfilId)
                        {
                            var perfiltemp = new UsuarioPerfil {UsuarioId = usuario.Id, PerfilId = id};
                            db.relPerfilesUsuarios.Add(perfiltemp);
                        }
                        
                        foreach (Int32 id in usuario.EmpresaId)
                        {
                            var empresatemp = new UsuarioEmpresa { UsuarioId = usuario.Id, EmpresaId = id };
                            db.relUsuarioEmpresa.Add(empresatemp);
                        }

                        db.SaveChanges();
                        transaction.Complete();
                        res = 1;
                        return Json(res);
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (var validationErrors in ex.EntityValidationErrors)
                        {
                            foreach (var validationError in validationErrors.ValidationErrors)
                            {
                                Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            }
                        }
                    }
                }
            }
            return View(usuario);
        }

        public ActionResult Edit(int id)
        {
            var usuario = db.Usuarios.Find(id);
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Edit(UsuarioModel usuario)
        {
            if (ModelState.IsValid)
            {
                var usr = db.Usuarios.Find(usuario.Id);

                usr.Nombre = usuario.Nombre;
                usr.ApellidoPaterno = usuario.ApellidoPaterno;
                usr.ApellidoMaterno = usuario.ApellidoMaterno;
                usr.Email = usuario.Email;
                usr.Activo = usuario.Activo;
                usr.UsuarioBaja = null;
                usr.FechaBaja = null;

                if(!usuario.Activo)
                {
                    usr.UsuarioBaja = Convert.ToInt32(Session["userNameId"]);
                    usr.FechaBaja = DateTime.Now;
                }
                db.Entry(usr).State = EntityState.Modified;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }
            return View(usuario);
        }

        public JsonResult RnEliminaUsuario(int id)
        {
            var res = "true";

            var usuario = db.Usuarios.Find(id);
            if (usuario == null || usuario.Incidentes.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            var usuario = db.Usuarios.Find(id);
            return View(usuario);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";
            using (var transaction = new TransactionScope())
            {
                try
                {
                    var usuario = db.Usuarios.Find(id);

                    for (int i = 0; i < usuario.UsuarioPerfiles.Count; i++)
                    {
                        usuario.UsuarioPerfiles.Remove(usuario.UsuarioPerfiles.Last());
                    }

                    for (int i = 0; i < usuario.UsuarioEmpresa.Count; i++)
                    {
                        usuario.UsuarioEmpresa.Remove(usuario.UsuarioEmpresa.Last());
                    }

                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    res = "false";
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
