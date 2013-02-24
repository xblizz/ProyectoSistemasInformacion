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
    public class TipoExtorsionController : Controller
    {
        private RiinContainer db = new RiinContainer();

        //
        // GET: /TipoExtorcion/

        public ViewResult Index()
        {
            return View(db.TiposExtorsion.ToList());
        }

        //
        // GET: /TipoExtorcion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoExtorcion/Create

        [HttpPost]
        public ActionResult Create(TipoExtorsion tipoextorsion)
        {
            if (ModelState.IsValid)
            {
                tipoextorsion.FechaAlta = DateTime.Now;
                tipoextorsion.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);
                db.TiposExtorsion.Add(tipoextorsion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoextorsion);
        }

        //
        // GET: /TipoExtorcion/Edit/5

        public ActionResult Edit(int id)
        {
            TipoExtorsion tipoextorsion = db.TiposExtorsion.Find(id);
            return View(tipoextorsion);
        }

        //
        // POST: /TipoExtorcion/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoExtorsion tipoextorsion)
        {
            if (ModelState.IsValid)
            {
                tipoextorsion.FechaAlta = DateTime.Now;
                db.Entry(tipoextorsion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoextorsion);
        }

        //
        // GET: /TipoExtorcion/Delete/5

        public ActionResult Delete(int id)
        {
            TipoExtorsion tipoextorsion = db.TiposExtorsion.Find(id);
            return View(tipoextorsion);
        }

        //
        // POST: /TipoExtorcion/Delete/5

        //[HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            TipoExtorsion tipoextorsion = db.TiposExtorsion.Find(id);
            db.TiposExtorsion.Remove(tipoextorsion);
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
            var tExtorcion = db.TiposExtorsion.Find(id);
            if (tExtorcion.Incidentes.Count > 0)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
            return Json("true", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetTiposExtorciones()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var tExtorciones =
                from tExtorcion in db.TiposExtorsion
                select new {Id = tExtorcion.Id, Descripcion = tExtorcion.Nombre};

            var listaPaginada = tExtorciones.OrderBy(c => c.Descripcion)
                .Skip((paginaActual - 1)*registrosPorPagina)
                .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;

            girdResult.total = tExtorciones.Count();
            girdResult.page = paginaActual;

            foreach (var extorcion in listaPaginada)
            {
                i += 1;
                miRow = new GridRow();
                miRow.id = i;
                miRow.cell = new List<string>()
                                 {
                                     Convert.ToString(extorcion.Id),
                                     extorcion.Descripcion
                                 };
                girdResult.rows.Add(miRow);
            }

            return Json(girdResult);
        }



    }
}