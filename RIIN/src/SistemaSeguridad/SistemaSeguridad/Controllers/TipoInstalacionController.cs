using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using System;
using SistemaSeguridad.Content;

namespace SistemaSeguridad.Controllers
{
    public class TipoInstalacionController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /TipoInstalacion/

        public ViewResult Index()
        {
            return View(db.TiposInstalacion.ToList());
        }

        //
        // GET: /TipoInstalacion/Details/5

        public ViewResult Details(int id)
        {
            var tipoinstalacion = db.TiposInstalacion.Find(id);
            return View(tipoinstalacion);
        }

        //
        // GET: /TipoInstalacion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoInstalacion/Create

        [HttpPost]
        public ActionResult Create(TipoInstalacion tipoinstalacion)
        {
            tipoinstalacion.FechaAlta = DateTime.Today;
            tipoinstalacion.UsuarioAlta = 1;

            if (ModelState.IsValid)
            {
                db.TiposInstalacion.Add(tipoinstalacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoinstalacion);
        }

        //
        // GET: /TipoInstalacion/Edit/5

        public ActionResult Edit(int id)
        {
            var tipoinstalacion = db.TiposInstalacion.Find(id);
            return View(tipoinstalacion);
        }

        //
        // POST: /TipoInstalacion/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoInstalacion tipoinstalacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoinstalacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoinstalacion);
        }

        //
        // GET: /TipoInstalacion/Delete/5

        public ActionResult Delete(int id)
        {
            var tipoinstalacion = db.TiposInstalacion.Find(id);
            return View(tipoinstalacion);
        }

        //
        // POST: /TipoInstalacion/Delete/5

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";
            try
            {
                var tipoinstalacion = db.TiposInstalacion.Find(id);
                db.TiposInstalacion.Remove(tipoinstalacion);
                db.SaveChanges();
            }
            catch (Exception)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminaTipoInstalacion(int id)
        {
            var res = "true";

            var tipoInstalacion = db.TiposInstalacion.Find(id);
            if (tipoInstalacion == null || tipoInstalacion.Instalaciones.Count > 0)
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

        public JsonResult GetTipos()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            //Get empresas only valid for current user
            var tipos = (from e in db.TiposInstalacion
                         join usr in db.Usuarios on e.UsuarioAlta equals usr.Id into lj
                         from sub in lj.DefaultIfEmpty()
                         select new
                         {
                             id = e.Id,
                             nombre = e.Nombre,
                             desc = e.Descripcion,
                             usuario = sub.UserName ?? "",
                             fechaAlta = e.FechaAlta,
                         }).ToList();

            var listaPaginada = tipos.OrderBy(e => e.nombre)
                                     .Skip((paginaActual - 1) * registrosPorPagina)
                                     .Take(registrosPorPagina);

            var salida = GridHelper.GetData(listaPaginada.ToList());
            return Json(new { page = paginaActual, total = tipos.Count(), rows = salida }, JsonRequestBehavior.AllowGet);
        }
    }
}