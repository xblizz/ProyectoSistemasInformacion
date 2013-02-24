using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using System;
using SistemaSeguridad.Content;

namespace SistemaSeguridad.Controllers
{ 
    public class TipoLesionadoController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /TipoLesionado/

        public ViewResult Index()
        {
            return View();
        }

        //
        // GET: /TipoLesionado/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /TipoLesionado/Create

        [HttpPost]
        public ActionResult Create(Lesionado lesionado)
        {
            lesionado.FechaAlta = DateTime.Today;
            lesionado.UsuarioAlta = 1;

            if (ModelState.IsValid)
            {
                db.Lesionados.Add(lesionado);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(lesionado);
        }
        
        //
        // GET: /TipoLesionado/Edit/5
 
        public ActionResult Edit(int id)
        {
            var lesionado = db.Lesionados.Find(id);
            return View(lesionado);
        }

        //
        // POST: /TipoLesionado/Edit/5

        [HttpPost]
        public ActionResult Edit(Lesionado lesionado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesionado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lesionado);
        }

        //
        // GET: /TipoLesionado/Delete/5
 
        public ActionResult Delete(int id)
        {
            var lesionado = db.Lesionados.Find(id);
            return View(lesionado);
        }

        //
        // POST: /TipoLesionado/Delete/5

        public ActionResult DeleteConfirmed(int id)
        {
            var res = "true";
            try
            {
                var lesionado = db.Lesionados.Find(id);
                db.Lesionados.Remove(lesionado);
                db.SaveChanges();
            }
            catch (Exception)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminaTipoLesionado(int id)
        {
            var res = "true";

            var tipoLesionado = db.Lesionados.Find(id);
            if (tipoLesionado == null || tipoLesionado.Incidentes.Count > 0)
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

            var tipos = (from e in db.Lesionados
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
            return Json(new { page = paginaActual, total = tipos.Count, rows = salida }, JsonRequestBehavior.AllowGet);
        }
    }
}