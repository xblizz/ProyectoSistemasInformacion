using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using SistemaSeguridad.Helpers;

namespace SistemaSeguridad.Controllers
{ 
    public class TipoVehiculoController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /TipoVehiculo/

        public ViewResult Index()
        {
            return View(db.TiposVehiculo.ToList());
        }

        //
        // GET: /TipoVehiculo/Details/5

        public ViewResult Details(int id)
        {
            var tipovehiculo = db.TiposVehiculo.Find(id);
            return View(tipovehiculo);
        }

        //
        // GET: /TipoVehiculo/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TipoVehiculo/Create

        [HttpPost]
        public ActionResult Create(TipoVehiculo tipovehiculo)
        {
            if (ModelState.IsValid)
            {
                tipovehiculo.FechaAlta = DateTime.Now;
                tipovehiculo.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);
                db.TiposVehiculo.Add(tipovehiculo);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(tipovehiculo);
        }
        
        //
        // GET: /TipoVehiculo/Edit/5
 
        public ActionResult Edit(int id)
        {
            var tipovehiculo = db.TiposVehiculo.Find(id);
            return View(tipovehiculo);
        }

        //
        // POST: /TipoVehiculo/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoVehiculo tipovehiculo)
        {
            if (ModelState.IsValid)
            {
                tipovehiculo.FechaAlta = DateTime.Now;
                db.Entry(tipovehiculo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipovehiculo);
        }

        //
        // GET: /TipoVehiculo/Delete/5
 
        public ActionResult Delete(int id)
        {
            var tipovehiculo = db.TiposVehiculo.Find(id);
            return View(tipovehiculo);
        }

        //
        // POST: /TipoVehiculo/Delete/5

        //[HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {            
            var tipovehiculo = db.TiposVehiculo.Find(id);
            db.TiposVehiculo.Remove(tipovehiculo);
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
            var tVehiculos = db.TiposVehiculo.Find(id);
            if (tVehiculos.Incidentes.Count > 0)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetTiposVehiculo()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var tVehiculos =
               from tvehiculo in db.TiposVehiculo
               select new { Id = tvehiculo.Id, Descripcion = tvehiculo.Nombre };

            var listaPaginada = tVehiculos.OrderBy(c => c.Descripcion)
                                      .Skip((paginaActual - 1) * registrosPorPagina)
                                      .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;

            girdResult.total = tVehiculos.Count();
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