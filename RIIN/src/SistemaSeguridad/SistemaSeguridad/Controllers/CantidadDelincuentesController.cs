using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Core.Entities;
using SistemaSeguridad.Helpers;

namespace SistemaSeguridad.Controllers
{
    public class CantidadDelincuentesController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();
        //
        // GET: /TipoIntrusion/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var cantidadDelincuentes = db.CantidadDelincuentes.ToList();

            var listaPaginada = cantidadDelincuentes.OrderBy(ti => ti.Orden)
                                                    .Skip((paginaActual - 1) * registrosPorPagina)
                                                    .Take(registrosPorPagina);


            var salida = new List<GridRow>();

            int i = 1;
            foreach (var item in listaPaginada)
            {
                salida.Add(new GridRow
                {
                    id = i++,
                    cell = new List<string>
                    {
                        item.Id.ToString(),
                        item.NombreDeCantidad
                    }
                });
            }

            return Json(new { page = paginaActual, total = cantidadDelincuentes.Count(), rows = salida }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /TipoIntrusion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoIntrusion/Create

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "Id")]
                                   CantidadDelincuentes cantidadDelincuentes)
        {
            if (ModelState.IsValid)
            {
                db.CantidadDelincuentes.Add(cantidadDelincuentes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cantidadDelincuentes);
        }

        //
        // GET: /TipoIntrusion/Edit/5

        public ActionResult Edit(int id)
        {
            var cantidadDelincuentes = db.CantidadDelincuentes.Find(id);
            return View(cantidadDelincuentes);
        }

        //
        // POST: /TipoIntrusion/Edit/5

        [HttpPost]
        public ActionResult Edit(CantidadDelincuentes cantidadDelincuentes)
        {
            var intrusion = db.CantidadDelincuentes.Find(cantidadDelincuentes.Id);

            if (ModelState.IsValid)
            {
                intrusion.NombreDeCantidad = cantidadDelincuentes.NombreDeCantidad;
                intrusion.Orden = cantidadDelincuentes.Orden;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cantidadDelincuentes);
        }

        public JsonResult RnEliminaCantidadDelincuentes(int id)
        {
            var res = "true";

            var cantidadDelincuentes = db.CantidadDelincuentes.Find(id);
            if (cantidadDelincuentes == null || cantidadDelincuentes.Incidentes.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";

            try
            {
                var cantidadDelincuentes = db.CantidadDelincuentes.Find(id);

                if (cantidadDelincuentes == null || cantidadDelincuentes.Incidentes.Count > 0)
                {
                    res = "false";
                }
                else
                {
                    db.CantidadDelincuentes.Remove(cantidadDelincuentes);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                res = "false";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}
