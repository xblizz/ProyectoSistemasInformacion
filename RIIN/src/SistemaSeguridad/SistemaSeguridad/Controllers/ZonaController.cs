using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SS.Core.Entities;
using SistemaSeguridad.Enums;
using SistemaSeguridad.Content;

namespace SistemaSeguridad.Controllers
{
    public class ZonaController : Controller
    {
        private RiinContainer db = new RiinContainer();

        //
        // GET: /Zona/

        public ViewResult Index()
        {
            var zonas = db.Zonas.Include(z => z.Estado).Include(z => z.Poligonos);
            return View(zonas.ToList());
        }

        public JsonResult GetZonas()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var zonas = (from e in db.Zonas
                           join usr in db.Usuarios on e.UsuarioAlta equals usr.Id into lj
                           from sub in lj.DefaultIfEmpty()
                           select new
                           {
                               id = e.Id,
                               nombre = e.Nombre,
                               user = sub.UserName ?? "",
                               fechaAlta = e.FechaAlta,
                           }).ToList();

            var listaPaginada = zonas.OrderBy(z => z.nombre)
                                     .Skip((paginaActual - 1) * registrosPorPagina)
                                     .Take(registrosPorPagina);

            var salida = GridHelper.GetData(listaPaginada.ToList());
            return Json(new { page = paginaActual, total = zonas.Count, rows = salida }, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Zona/Details/5

        public ViewResult Details(int id)
        {
            Zona zona = db.Zonas.Find(id);
            return View(zona);
        }

        //
        // GET: /Zona/Create

        public ActionResult Create()
        {
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre");
            ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id");
            return View();
        }

        //
        // POST: /Zona/Create

        [HttpPost]
        public ActionResult Create(Zona zona)
        {
            if (ModelState.IsValid)
            {
                //var cords = zona.Coords;//.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //PoligonoDetalle pDetalle;
                //Poligono pol = new Poligono();

                //foreach (var cord in cords)
                //{
                //    var latlng = cord.Split(Convert.ToChar(","));

                //    pDetalle = new PoligonoDetalle();
                //    pDetalle.Latitud = Convert.ToDouble(latlng[0]);
                //    pDetalle.Longitud = Convert.ToDouble(latlng[1]);
                //    pol.PoligonoDetalles.Add(pDetalle);
                //}
                //zona.Poligonos = pol;

                //zona.Poligonos.NivelGeograficoId = (int)NivelesGeograficosEnum.Zona;
                //zona.UsuarioAlta = 1;
                //zona.FechaAlta = DateTime.Now;

                //db.Zonas.Add(zona);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", zona.EstadoId);
            ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", zona.PoligonoId);
            return View(zona);
        }

        //
        // GET: /Zona/Edit/5

        public ActionResult Edit(int id)
        {
            Zona zona = db.Zonas.Find(id);
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", zona.EstadoId);
            ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", zona.PoligonoId);
            return View(zona);
        }

        //
        // POST: /Zona/Edit/5

        [HttpPost]
        public ActionResult Edit(Zona zona)
        {
            if (ModelState.IsValid)
            {
                db.Entry(zona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", zona.EstadoId);
            ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", zona.PoligonoId);
            return View(zona);
        }

        //
        // GET: /Zona/Delete/5

        public ActionResult Delete(int id)
        {
            Zona zona = db.Zonas.Find(id);
            return View(zona);
        }

        //
        // POST: /Zona/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Zona zona = db.Zonas.Find(id);
            db.Zonas.Remove(zona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public JsonResult ExistZona(string nombre)
        {

            var zone = db.Zonas.FirstOrDefault(zona => zona.Nombre.Equals(nombre));
            if (zone != null)
            {
                var res = "Ya existe una zona con ese nombre.";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCoords(int filterId)
        {
            var zona = db.Zonas.FirstOrDefault(z => z.Id.Equals(filterId));
            if (zona != null)
            {
                var Coords =
                    (from cords in db.PoligonosDetalle
                     where cords.PoligonoId == zona.PoligonoId
                     select new { latitud = cords.Latitud, longitud = cords.Longitud });
                return Json(Coords.ToList(), JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}