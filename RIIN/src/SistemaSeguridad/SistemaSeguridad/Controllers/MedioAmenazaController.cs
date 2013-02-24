using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using SistemaSeguridad.Helpers;

namespace SistemaSeguridad.Controllers
{ 
    public class MedioAmenazaController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /MedioAmenaza/

        public ViewResult Index()
        {
            return View(db.MediosAmenaza.ToList());
        }

        //
        // GET: /MedioAmenaza/Details/5

        public ViewResult Details(int id)
        {
            var medioamenaza = db.MediosAmenaza.Find(id);
            return View(medioamenaza);
        }

        //
        // GET: /MedioAmenaza/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /MedioAmenaza/Create

        [HttpPost]
        public ActionResult Create(MedioAmenaza medioamenaza)
        {
            if (ModelState.IsValid)
            {
                medioamenaza.UsuarioAlta =Convert.ToInt32(Session["userNameId"]);
                medioamenaza.FechaAlta = DateTime.Now;

                db.MediosAmenaza.Add(medioamenaza);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(medioamenaza);
        }
        
        //
        // GET: /MedioAmenaza/Edit/5
 
        public ActionResult Edit(int id)
        {
            var medioamenaza = db.MediosAmenaza.Find(id);
            return View(medioamenaza);
        }

        //
        // POST: /MedioAmenaza/Edit/5

        [HttpPost]
        public ActionResult Edit(MedioAmenaza medioamenaza)
        {
            if (ModelState.IsValid)
            {

                medioamenaza.FechaAlta = DateTime.Now;
                db.Entry(medioamenaza).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(medioamenaza);
        }

        //
        // GET: /MedioAmenaza/Delete/5
 
        public ActionResult Delete(int id)
        {
            var medioamenaza = db.MediosAmenaza.Find(id);
            return View(medioamenaza);
        }

        //
        // POST: /MedioAmenaza/Delete/5

        //HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {            
            var medioamenaza = db.MediosAmenaza.Find(id);
            db.MediosAmenaza.Remove(medioamenaza);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminar(int id)
        {
            var medio = db.MediosAmenaza.Find(id);
            if (medio.Incidentes.Count > 0)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult GetAmenazas()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var medios =
               from medio in db.MediosAmenaza
               select new { Id = medio.Id, Descripcion = medio.Nombre };

            var listaPaginada = medios.OrderBy(c => c.Descripcion)
                                      .Skip((paginaActual - 1) * registrosPorPagina)
                                      .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;

            girdResult.total = medios.Count();
            girdResult.page = paginaActual;

            foreach (var medio in listaPaginada)
            {
                i += 1;
                miRow = new GridRow();
                miRow.id = i;
                miRow.cell = new List<string>()
                                 {
                                     Convert.ToString(medio.Id),
                                     medio.Descripcion
                                 };
                girdResult.rows.Add(miRow);
            }

            return Json(girdResult);
        }
    }
}