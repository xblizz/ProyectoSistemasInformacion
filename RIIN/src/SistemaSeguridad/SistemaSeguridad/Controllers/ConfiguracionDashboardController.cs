using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Core.Entities;

namespace SistemaSeguridad.Controllers
{ 
    public class ConfiguracionDashboardController : Controller
    {
        private RiinContainer db = new RiinContainer();

        //
        // GET: /ConfiguracionDashboard/

        public ViewResult Index()
        {
            var configuracionesdashboard = db.ConfiguracionesDashboard.Include(c => c.TipoUnidadTiempo).Include(c => c.ReportesDashboard);
            return View(configuracionesdashboard.ToList());
        }

        //
        // GET: /ConfiguracionDashboard/Details/5

        public ViewResult Details(int id)
        {
            ConfiguracionDashboard configuraciondashboard = db.ConfiguracionesDashboard.Find(id);
            return View(configuraciondashboard);
        }

        //
        // GET: /ConfiguracionDashboard/Create

        public ActionResult Create()
        {
            ViewBag.TipoUnidadTiempoId = new SelectList(db.TiposUnidadTiempo, "Id", "Nombre");
            ViewBag.ReporteId = new SelectList(db.ReportesDashboard, "Id", "Nombre");
            return View();
        } 

        //
        // POST: /ConfiguracionDashboard/Create

        [HttpPost]
        public ActionResult Create(ConfiguracionDashboard configuraciondashboard)
        {
            if (ModelState.IsValid)
            {
                db.ConfiguracionesDashboard.Add(configuraciondashboard);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.TipoUnidadTiempoId = new SelectList(db.TiposUnidadTiempo, "Id", "Nombre", configuraciondashboard.TipoUnidadTiempoId);
            ViewBag.ReporteId = new SelectList(db.ReportesDashboard, "Id", "Nombre", configuraciondashboard.ReporteId);
            return View(configuraciondashboard);
        }
        
        //
        // GET: /ConfiguracionDashboard/Edit/5
 
        public ActionResult Edit(int id)
        {
            ConfiguracionDashboard configuraciondashboard = db.ConfiguracionesDashboard.Find(id);
            ViewBag.TipoUnidadTiempoId = new SelectList(db.TiposUnidadTiempo, "Id", "Nombre", configuraciondashboard.TipoUnidadTiempoId);
            ViewBag.ReporteId = new SelectList(db.ReportesDashboard, "Id", "Nombre", configuraciondashboard.ReporteId);
            return View(configuraciondashboard);
        }

        //
        // POST: /ConfiguracionDashboard/Edit/5

        [HttpPost]
        public ActionResult Edit(ConfiguracionDashboard configuraciondashboard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuraciondashboard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TipoUnidadTiempoId = new SelectList(db.TiposUnidadTiempo, "Id", "Nombre", configuraciondashboard.TipoUnidadTiempoId);
            ViewBag.ReporteId = new SelectList(db.ReportesDashboard, "Id", "Nombre", configuraciondashboard.ReporteId);
            return View(configuraciondashboard);
        }

        //
        // GET: /ConfiguracionDashboard/Delete/5
 
        public ActionResult Delete(int id)
        {
            ConfiguracionDashboard configuraciondashboard = db.ConfiguracionesDashboard.Find(id);
            return View(configuraciondashboard);
        }

        //
        // POST: /ConfiguracionDashboard/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            ConfiguracionDashboard configuraciondashboard = db.ConfiguracionesDashboard.Find(id);
            db.ConfiguracionesDashboard.Remove(configuraciondashboard);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}