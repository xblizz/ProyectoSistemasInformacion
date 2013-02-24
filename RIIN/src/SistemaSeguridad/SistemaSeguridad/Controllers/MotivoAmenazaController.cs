using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Core.Entities;
using SistemaSeguridad.Helpers;

namespace SistemaSeguridad.Controllers
{ 
    public class MotivoAmenazaController : Controller
    {
        private RiinContainer db = new RiinContainer();

        //
        // GET: /MotivoAmenaza/

        public ViewResult Index()
        {
            return View(db.MotivosAmenaza.ToList());
        }

        //
        // GET: /MotivoAmenaza/Details/5

        public ViewResult Details(int id)
        {
            MotivoAmenaza motivoamenaza = db.MotivosAmenaza.Find(id);
            return View(motivoamenaza);
        }

        //
        // GET: /MotivoAmenaza/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /MotivoAmenaza/Create

        [HttpPost]
        public ActionResult Create(MotivoAmenaza motivoamenaza)
        {
            if (ModelState.IsValid)
            {
                motivoamenaza.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);
                motivoamenaza.FechaAlta =DateTime.Now;
                db.MotivosAmenaza.Add(motivoamenaza);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(motivoamenaza);
        }
        
        //
        // GET: /MotivoAmenaza/Edit/5
 
        public ActionResult Edit(int id)
        {
            MotivoAmenaza motivoamenaza = db.MotivosAmenaza.Find(id);
            return View(motivoamenaza);
        }

        //
        // POST: /MotivoAmenaza/Edit/5

        [HttpPost]
        public ActionResult Edit(MotivoAmenaza motivoamenaza)
        {
            if (ModelState.IsValid)
            {
                motivoamenaza.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);
                motivoamenaza.FechaAlta = DateTime.Now;
                db.Entry(motivoamenaza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(motivoamenaza);
        }

        //
        // GET: /MotivoAmenaza/Delete/5
 
        public ActionResult Delete(int id)
        {
            MotivoAmenaza motivoamenaza = db.MotivosAmenaza.Find(id);
            return View(motivoamenaza);
        }

        //
        // POST: /MotivoAmenaza/Delete/5

        //[HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {            
            MotivoAmenaza motivoamenaza = db.MotivosAmenaza.Find(id);
            db.MotivosAmenaza.Remove(motivoamenaza);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public JsonResult RnEliminar(int id)
        {
            var motivos = db.MotivosAmenaza.Find(id);
            if (motivos.Incidentes.Count > 0)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMotivosAmenazas()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var motivos =
               from motivo in db.MotivosAmenaza
               select new { Id = motivo.Id, Descripcion = motivo.Nombre };

            var listaPaginada = motivos.OrderBy(c => c.Descripcion)
                                      .Skip((paginaActual - 1) * registrosPorPagina)
                                      .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;

            girdResult.total = motivos.Count();
            girdResult.page = paginaActual;

            foreach (var motivo in listaPaginada)
            {
                i += 1;
                miRow = new GridRow();
                miRow.id = i;
                miRow.cell = new List<string>()
                                 {
                                     Convert.ToString(motivo.Id),
                                     motivo.Descripcion
                                 };
                girdResult.rows.Add(miRow);
            }

            return Json(girdResult);
        }

    }
}