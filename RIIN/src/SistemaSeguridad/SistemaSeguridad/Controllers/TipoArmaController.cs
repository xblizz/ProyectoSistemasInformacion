using System.Data;
using System.Linq;
using System.Web.Mvc;
using SS.Core.Entities;
using System;
using SistemaSeguridad.Helpers;
using System.Collections.Generic;

namespace SistemaSeguridad.Controllers
{
    public class TipoArmaController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /TipoArma/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /TipoArma/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoArma/Create

        [HttpPost]
        public ActionResult Create(TipoArma tipoarma)
        {
            tipoarma.FechaAlta = DateTime.Today;
            tipoarma.UsuarioAlta = 1;

            if (ModelState.IsValid)
            {
                db.TiposArma.Add(tipoarma);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoarma);
        }

        //
        // GET: /TipoArma/Edit/5

        public ActionResult Edit(int id)
        {
            var tipoarma = db.TiposArma.Find(id);
            return View(tipoarma);
        }

        //
        // POST: /TipoArma/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoArma tipoarma)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoarma).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoarma);
        }

        //
        // GET: /TipoArma/Delete/5

        public ActionResult Delete(int id)
        {
            var tipoarma = db.TiposArma.Find(id);
            return View(tipoarma);
        }

        //
        // POST: /TipoArma/Delete/5

        //[HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";

            try
            {
                var tipoarma = db.TiposArma.Find(id);

                if (tipoarma == null || tipoarma.Incidentes.Count > 0)
                {
                    res = "false";
                }
                else
                {
                    db.TiposArma.Remove(tipoarma);
                    db.SaveChanges();
                }
            }
            catch (Exception)
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

        public JsonResult RnEliminaTipoArma(int id)
        {
            var res = "true";
            
            var tipoArma = db.TiposArma.Find(id);
            if (tipoArma == null || tipoArma.Incidentes.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTipos()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            //Get tipos de arma only valid for current user (we perform a left join)
            var tipos = (from e in db.TiposArma
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

            //var salida = GridHelper.GetData(listaPaginada.ToList());

            var salida = new List<GridRow>();

            int i = 1;
            foreach (var item in listaPaginada)
            {
                salida.Add(new GridRow
                {
                    id = i++,
                    cell = new List<string>
                    {
                        item.id.ToString(),
                        item.nombre,
                        item.usuario,
                        item.fechaAlta.ToString()
                    }
                });
            }

            return Json(new { page = paginaActual, total = tipos.Count(), rows = salida }, JsonRequestBehavior.AllowGet);
        }
    }
}
