using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Core.Entities;
using SistemaSeguridad.Helpers;
using System.Data;

namespace SistemaSeguridad.Controllers
{
    public class TipoIntrusionController : Controller
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

            var tiposIntrusion = db.TiposIntrusion.Join(db.Usuarios,
                                                       (ti => ti.UsuarioAlta),
                                                       (u => u.Id),
                                                       ((ti, u) => new
                                                       {
                                                           ti.Id,
                                                           ti.Nombre,
                                                           Usuario = u.Nombre,
                                                           ti.FechaAlta
                                                       }));

            //var tiposIntrusion = (from ti in db.TiposIntrusion
            //                      join usr in db.Usuarios on ti.UsuarioAlta equals usr.Id into lj
            //                      from sub in lj.DefaultIfEmpty()
            //                      select new
            //                      {
            //                          Id = ti.Id,
            //                          Nombre = ti.Nombre,
            //                          Usuario = sub.UserName ?? "",
            //                          FechaAlta = ti.FechaAlta,
            //                      }).ToList();

            var listaPaginada = tiposIntrusion.OrderBy(ti => ti.Nombre)
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
                        item.Nombre,
                        item.Usuario,
                        item.FechaAlta.ToString()
                    }
                });
            }

            return Json(new { page = paginaActual, total = tiposIntrusion.Count(), rows = salida }, JsonRequestBehavior.AllowGet);
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
                                   TipoIntrusion tipoIntrusion)
        {
            tipoIntrusion.FechaAlta = DateTime.Today;
            tipoIntrusion.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);

            if (ModelState.IsValid)
            {
                db.TiposIntrusion.Add(tipoIntrusion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoIntrusion);
        }

        //
        // GET: /TipoIntrusion/Edit/5

        public ActionResult Edit(int id)
        {
            var tipoIntrusion = db.TiposIntrusion.Find(id);
            return View(tipoIntrusion);
        }

        //
        // POST: /TipoIntrusion/Edit/5

        [HttpPost]
        public ActionResult Edit(TipoIntrusion tipoIntrusion)
        {
            var intrusion = db.TiposIntrusion.Find(tipoIntrusion.Id);

            if (ModelState.IsValid)
            {
                //db.Entry(tipoIntrusion).State = EntityState.Modified;
                intrusion.Nombre = tipoIntrusion.Nombre;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoIntrusion);
        }

        public JsonResult RnEliminaTipoIntrusion(int id)
        {
            var res = "true";

            var tipoIntrusion = db.TiposIntrusion.Find(id);
            if (tipoIntrusion == null || tipoIntrusion.Incidentes.Count > 0)
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
                var tipoIntrusion = db.TiposIntrusion.Find(id);

                if (tipoIntrusion == null || tipoIntrusion.Incidentes.Count > 0)
                {
                    res = "false";
                }
                else
                {
                    db.TiposIntrusion.Remove(tipoIntrusion);
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
