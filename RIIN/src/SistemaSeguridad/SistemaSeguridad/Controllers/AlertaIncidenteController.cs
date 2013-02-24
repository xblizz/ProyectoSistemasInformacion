using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Text;

using SS.Core.Entities;
using SistemaSeguridad.Content;

namespace SistemaSeguridad.Controllers
{
    public class AlertaIncidenteController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //public JsonResult GetEmpresas(int empresaId)
        //{
        //    var empresas =
        //        from empresa in db.Empresas
        //        where empresa.Id == empresaId
        //        select new { Id = empresa.Id, Descripcion = empresa.Nombre };

        //    return Json(empresas.ToList(), JsonRequestBehavior.AllowGet);
        //}

        //[OutputCache(Location = OutputCacheLocation.None, NoStore = true)]
        public ActionResult ValidateEmailList(string Emails)
        {
            var returnValue = true;
            var emailRegEx = new RegexStringValidator(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            var validEmailList = new StringBuilder();
            var emailArray = Emails.Split(';');

            foreach (var str in emailArray.Where(str => !string.IsNullOrEmpty(str)))
            {
                try
                {
                    emailRegEx.Validate(str);
                    validEmailList.Append(str);
                    validEmailList.Append(";");
                }
                catch (ArgumentException)
                {
                    returnValue = false;
                    break;
                }
            }

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Index()
        {
            return View(db.AlertasIncidente.ToList());
        }

        public ActionResult Create()
        {
            //PopulateEmpresasDropDownList();
            //PopulateTiposIncidenteDropDownList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(AlertaIncidente alerta)
        {
            if (ModelState.IsValid)
            {
                db.AlertasIncidente.Add(alerta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(alerta);
        }

        public ActionResult Edit(int empresaId, int tipoIncidenteId)
        {
            PopulateEmpresasDropDownList(empresaId);
            PopulateTiposIncidenteDropDownList(tipoIncidenteId);

            var alertaToModify = (from x in db.AlertasIncidente
                                              where x.EmpresaId == empresaId
                                              && x.TipoIncidenteId == tipoIncidenteId
                                              select x).Single();

            return View(alertaToModify);
        }

        [HttpPost]
        public ActionResult Edit(AlertaIncidente alerta)
        {
            if (ModelState.IsValid)
            {
                db.AlertasIncidente.Attach(alerta);
                db.Entry(alerta).State = EntityState.Modified;

                return RedirectToAction("Index");
            }

            return View(alerta);
        }

        public ActionResult Delete(int empresaId, int tipoIncidenteId)
        {
            var alertaToModify = from x in db.AlertasIncidente
                                 where x.EmpresaId == empresaId
                                 && x.TipoIncidenteId == tipoIncidenteId
                                 select x;

            return View(alertaToModify.Single());
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int empresaId, int tipoIncidenteId)
        {
            var alertaToModify = from x in db.AlertasIncidente
                                 where x.EmpresaId == empresaId
                                 && x.TipoIncidenteId == tipoIncidenteId
                                 select x;

            db.AlertasIncidente.Remove(alertaToModify.Single());
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public JsonResult ObtenerEmpresas()
        {
            var empresas =
                from empresa in db.Empresas
                where empresa.TipoEmpresa == false
                select new { Id = empresa.Id, Descripcion = empresa.Nombre };



            var emp = from empresa in db.Empresas
                      from usuarioEmpresa
                      in db.relUsuarioEmpresa
                           .Where(ue => ue.EmpresaId == empresa.Id)
                           .DefaultIfEmpty()
                      from usuario
                      in db.Usuarios
                           .Where(u => u.Id == usuarioEmpresa.UsuarioId)
                           .DefaultIfEmpty()
                      from perfilUsuario
                      in db.relPerfilesUsuarios
                           .Where(pu => pu.UsuarioId == usuario.Id)
                           .DefaultIfEmpty()
                      where perfilUsuario.PerfilId == 2
                      select new { Id = empresa.Id, Descripcion = empresa.Nombre };


            //from perfil
            //in db.Perfiles
            //     .Where(p => p.Id == perfilUsuario.PerfilId)
            //     .DefaultIfEmpty()                       
            //where perfil.Id == 2








            //join ue in db.relUsuarioEmpresa
            //on empresa.Id equals ue.EmpresaId into usuarioEmpresa
            //from u in usuarioEmpresa.DefaultIfEmpty()
            //select u;

            //var emp = from e in db.Empresas
            //          orderby e.Nombre
            //          select new 
            //          {
            //            Usuario = e.UsuarioEmpresa.Where(ue => ue.EmpresaId == e.Id)
            //                                      .Select(ue => ue)
            //                                      .Where(
            //          };


            return Json(emp.ToList(), JsonRequestBehavior.AllowGet);
        }

        private void PopulateEmpresasDropDownList(object empresaSeleccionada = null)
        {
            var departmentsQuery = from d in db.Empresas
                                   orderby d.Nombre
                                   select d;

            ViewBag.EmpresaId = new SelectList(departmentsQuery, "EmpresaId", "Nombre", empresaSeleccionada);
        }

        private void PopulateTiposIncidenteDropDownList(object tipoSeleccionado = null)
        {
            var departmentsQuery = from d in db.TiposIncidente
                                   orderby d.Nombre
                                   select d;

            ViewBag.TipoIncidenteId = new SelectList(departmentsQuery, "TipoIncidenteId", "Nombre", tipoSeleccionado);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult GetAlertas()
        {
            var alertas = (from a in db.AlertasIncidente
                           join t in db.TiposIncidente on a.TipoIncidenteId equals t.Id
                           join e in db.Empresas on a.EmpresaId equals e.Id 
                           select new
                           {
                               nombre = e.Nombre,
                               tipo = t.Nombre,
                               otras = a.RecibirOtrasEmpresas,
                               miempresa = a.RecibirMiEmpresa,
                               emails = a.Emails,
                           }).ToList();

            var salida = GridHelper.GetData(alertas.ToList());
            return Json(new { page = 1, total = 1, records = 2, rows = salida }, JsonRequestBehavior.AllowGet);
        }
    }
}