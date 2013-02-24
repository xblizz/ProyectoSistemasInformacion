using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using SistemaSeguridad.Content;
using SistemaSeguridad.Enums;

namespace SistemaSeguridad.Controllers
{
    public class EstadoController : Controller
    {
        private RiinContainer db = new RiinContainer();

        public ViewResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Estado estado)
        {
            estado.FechaAlta = DateTime.Today;
            estado.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);

            if (ModelState.IsValid)
            {
                estado.Poligonos = new Poligono();
                estado.Poligonos.NivelGeograficoId = (int)NivelesGeograficosEnum.Estado;

                var coordenadas = estado.Coords;//.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < coordenadas.Length; i++)
                {
                    var coordenada = coordenadas[i].Split(",".ToCharArray());

                    estado.Poligonos.PoligonoDetalles.Add(new PoligonoDetalle
                    {
                        Latitud = float.Parse(coordenada[0]),
                        Longitud = float.Parse(coordenada[1])                        
                    });
                }

                db.Estados.Add(estado);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", estadoEntity.PoligonoId);
            return View(estado);
        }

        public ActionResult Edit(int id)
        {
            var estado = db.Estados.Find(id);
            return View(estado);
        }

        public JsonResult GetCoords(int filterId)
        {
            var estado = db.Estados.Where(e => e.Id == filterId).First();

            var Coords =
                (from cords in db.PoligonosDetalle where cords.PoligonoId == estado.PoligonoId select new { latitud = cords.Latitud, longitud = cords.Longitud });
            return Json(Coords.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", estado.PoligonoId);
            return View(estado);
        }

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";

            try
            {
                using (var transaction = new System.Transactions.TransactionScope())
                {
                    var estado = db.Estados.Find(id);
                    var poligono = db.Poligonos.Where(p => p.Id == estado.PoligonoId).First();
                    var poligonoDetalles = db.PoligonosDetalle.Where(pd => pd.PoligonoId == poligono.Id);

                    foreach (var poligonoDetalle in poligonoDetalles)
                    {
                        db.PoligonosDetalle.Remove(poligonoDetalle);
                    }

                    db.Poligonos.Remove(poligono);
                    db.Estados.Remove(estado);

                    //var poligonoId = estado.PoligonoId;
                    //var helper = new HelperController();

                    //helper.RemovePoligonoDetalles(estado.PoligonoId);
                    //db.Estados.Remove(estado);
                    //db.SaveChanges();
                    //helper.RemovePoligono(poligonoId);

                    //RemoveEstado(estado);
                    db.SaveChanges();
                    transaction.Complete();
                }
            }
            catch (Exception)
            {
                res = "false";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //private void RemoveEstado(Estado estado)
        //{

        //}

        public JsonResult RnEliminaEstado(int id)
        {
            var res = "true";

            var estado = db.Estados.Find(id);
            if (estado == null || estado.Ciudades.Count > 0 ||
                estado.Incidentes.Count > 0 || estado.Zonas.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEstados()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var estados = (from e in db.Estados
                           join usr in db.Usuarios on e.UsuarioAlta equals usr.Id into lj
                           from sub in lj.DefaultIfEmpty()
                           select new
                           {
                               id = e.Id,
                               nombre = e.Nombre,
                               user = sub.UserName ?? "",
                               fechaAlta = e.FechaAlta,
                           }).ToList();

            var listaPaginada = estados.OrderBy(e => e.nombre)
                                       .Skip((paginaActual - 1) * registrosPorPagina)
                                       .Take(registrosPorPagina);

            var salida = GridHelper.GetData(listaPaginada.ToList());
            return Json(new { page = paginaActual, total = estados.Count, rows = salida }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ExisteEstado(string nombre)
        {
            var estado = db.Estados.FirstOrDefault(e => e.Nombre.Equals(nombre));
            if (estado != null)
            {
                var res = "Ya existe un estado con ese nombre.";
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
