using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using System;
using SistemaSeguridad.Content;

namespace SistemaSeguridad.Controllers
{ 
    public class TipoAfectacionController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /TipoAfectacion/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /TipoAfectacion/Details/5

        public ViewResult Details(int id)
        {
            var tipoafectacion = db.TiposAfectacion.Find(id);
            return View(tipoafectacion);
        }

        //
        // GET: /TipoAfectacion/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TipoAfectacion/Create

        [HttpPost]
        public ActionResult Create(TipoAfectacion tipoafectacion)
        {
            tipoafectacion.FechaAlta = DateTime.Today;
            tipoafectacion.UsuarioAlta = 1;

            if (ModelState.IsValid)
            {
                db.TiposAfectacion.Add(tipoafectacion);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(tipoafectacion);
        }
        
        //
        // GET: /TipoAfectacion/Edit/5
 
        public ActionResult Edit(int id)
        {
            var tipoafectacion = db.TiposAfectacion.Find(id);
            return View(tipoafectacion);
        }

        //
        // POST: /TipoAfectacion/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoAfectacion tipoafectacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoafectacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoafectacion);
        }

        //
        // GET: /TipoAfectacion/Delete/5
 
        public ActionResult Delete(int id)
        {
            var tipoafectacion = db.TiposAfectacion.Find(id);
            return View(tipoafectacion);
        }

        //
        // POST: /TipoAfectacion/Delete/5

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";
            try
            {
                var tipoafectacion = db.TiposAfectacion.Find(id);
                db.TiposAfectacion.Remove(tipoafectacion);
                db.SaveChanges();
            }
            catch (Exception)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminaTipoAfectacion(int id)
        {
            var res = "true";

            var tipoAfectacion = db.TiposAfectacion.Find(id);
            if (tipoAfectacion == null || tipoAfectacion.AfectacionIncidentes.Count > 0)
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
            
            var tipos = (from e in db.TiposAfectacion
                         join usr in db.Usuarios on e.UsuarioAlta equals usr.Id into lj
                         from sub in lj.DefaultIfEmpty()
                         select new
                         {
                             id = e.Id,
                             nombre = e.Nombre,
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