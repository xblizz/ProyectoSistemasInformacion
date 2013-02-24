using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Diagnostics;

using SS.Core.Entities;
using SistemaSeguridad.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace SistemaSeguridad.Controllers
{
    public class TipoIncidenteController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /TipoIncidente/

        public JsonResult GetAll()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            //var usuarios = (from usuario in db.Usuarios.Include("UsuarioEmpresa") select usuario);
            var tiposIncidentes = db.TiposIncidente;

            var listaPaginada = tiposIncidentes.OrderBy(e => e.Nombre)
                                        .Skip((paginaActual - 1) * registrosPorPagina)
                                        .Take(registrosPorPagina);

            var girdResult = new Flexgrid { page = paginaActual, total = tiposIncidentes.Count() };
            int i = 1;

            foreach (var empresa in listaPaginada)
            {
                girdResult.rows.Add(new GridRow
                {
                    id = i++,
                    cell = new List<string>()
                           {
                               empresa.Id.ToString(),
                               empresa.Nombre
                           }
                });
            }
            return Json(girdResult);
        }

        public ViewResult Index()
        {
            //var context = new ConorContainer();
            //var tiposIncidentes = context.TiposIncidente;

            return View();
        }

        //
        // GET: /TipoIncidente/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoIncidente/Create

        [HttpPost]
        public ActionResult Create(TipoIncidente tipoincidente)
        {
            try
            {
                tipoincidente.UsuarioAlta = 3;
                tipoincidente.Imagen = TempData["RutaImagen"].ToString();

                //When is new we assign today date
                if (tipoincidente.FechaAlta <= new DateTime(2012, 1, 1))
                    tipoincidente.FechaAlta = DateTime.Today;

                if (string.IsNullOrEmpty(tipoincidente.Imagen))
                    tipoincidente.Imagen = "NINGUNA";

                if (ModelState.IsValid)
                {
                    db.TiposIncidente.Add(tipoincidente);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                //((System.Data.Entity.Validation.DbEntityValidationException)dbEx).EntityValidationErrors
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }

            return View(tipoincidente);
        }

        //
        // GET: /TipoIncidente/Edit/5

        public ActionResult Edit(int id)
        {
            var tipoincidente = db.TiposIncidente.Find(id);
            return View(tipoincidente);
        }

        //
        // POST: /TipoIncidente/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoIncidente tipoincidente)
        {
            //When is new we assign today date
            if (tipoincidente.FechaAlta <= new DateTime(2012, 1, 1))
                tipoincidente.FechaAlta = DateTime.Today;

            tipoincidente.UsuarioAlta = 3;

            if (string.IsNullOrEmpty(tipoincidente.Imagen))
                tipoincidente.Imagen = "NINGUNA";

            if (ModelState.IsValid)
            {
                db.Entry(tipoincidente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoincidente);
        }

        //
        // GET: /TipoIncidente/Delete/5

        public ActionResult Delete(int id)
        {
            var tipoincidente = db.TiposIncidente.Find(id);
            return View(tipoincidente);
        }

        //
        // POST: /TipoIncidente/Delete/5

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";
            try
            {
                var tipoincidente = db.TiposIncidente.Find(id);
                db.TiposIncidente.Remove(tipoincidente);
                db.SaveChanges();
            }
            catch (Exception)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminaTipoIncidente(int id)
        {
            var res = "true";

            var tipoIncidente = db.TiposIncidente.Find(id);
            if (tipoIncidente == null || tipoIncidente.AlertaIncidentes.Count > 0 ||
                tipoIncidente.Incidentes.Count > 0 || tipoIncidente.ParametroSistemaEmpresa.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        #region Subir imagen jpadilla

        public ActionResult Upload()
        {
            string filePath = string.Empty;
            foreach (string inputTagName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[inputTagName];                
                if (file.ContentLength > 0)
                {
                    filePath = Path.Combine(HttpContext.Server.MapPath("../Uploads"), Path.GetFileName(file.FileName));
                    file.SaveAs(filePath);
                }
                
            }
            //ViewBag.RutaImagen = filePath;
            TempData["RutaImagen"] = filePath;
            return RedirectToAction("Create", "TipoIncidente");
        }
        
        #endregion

    }
}