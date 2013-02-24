using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using System;
using SistemaSeguridad.Content;
using SistemaSeguridad.Models;
using System.Transactions;

namespace SistemaSeguridad.Controllers
{
    public class CiudadController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        public ViewResult Index()
        {
            //var ciudades = db.Ciudades.Include(c => c.Estado).Include(c => c.Poligono);
            //return View(ciudades.ToList());
            return View();
        }

        public ActionResult Create()
        {
            //ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre");
            //ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Ciudad ciudad)//Entity ciudadEntity)
        {
            ciudad.EstadoId = ciudad.EstadoId;
            ciudad.FechaAlta = DateTime.Today;
            ciudad.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);

            if (ModelState.IsValid)
            {
                //var helper = new HelperController();
                //helper.SavePoligonoDetalle(ciudadEntity);  //CCRS

                //var ciudad = ToCiudad(ciudadEntity);
                ciudad.Poligono = new Poligono();
                ciudad.Poligono.NivelGeograficoId = 1;

                var coordenadas = ciudad.Coords;
                //.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < coordenadas.Length; i++)
                {
                    var coordenada = coordenadas[i].Split(",".ToCharArray());

                    ciudad.Poligono.PoligonoDetalles.Add(new PoligonoDetalle
                    {
                        Latitud = float.Parse(coordenada[0]),
                        Longitud = float.Parse(coordenada[1])
                    });
                }

                db.Ciudades.Add(ciudad);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", ciudadEntity.EstadoId);
            //ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", ciudadEntity.PoligonoId);
            return View(ciudad);
        }

        public ActionResult Edit(int id)
        {
            //var helper = new HelperController();
            //var ciudad = db.Ciudades.Find(id);

            //var ciudadEntity = ToCiudadEntity(ciudad);

            //ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", ciudad.EstadoId);
            //ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", ciudad.PoligonoId);
            //ciudadEntity.CoordsList = helper.GetPoligonoDetalle(ciudad.PoligonoId);

            var ciudad = db.Ciudades.Where(c => c.Id == id).First();
            return View(ciudad);
        }

        [HttpPost]
        public ActionResult Edit(Ciudad ciudad)//Entity ciudadEntity)
        {
            if (ModelState.IsValid)
            {
                //var helper = new HelperController();
                //helper.SavePoligonoDetalle(ciudadEntity);

                //var ciudad = ToCiudad(ciudadEntity);
                db.Entry(ciudad).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            //ViewBag.EstadoId = new SelectList(db.Estados, "Id", "Nombre", ciudadEntity.EstadoId);
            //ViewBag.PoligonoId = new SelectList(db.Poligonos, "Id", "Id", ciudadEntity.PoligonoId);
            return View(ciudad);
        }

        //public ActionResult Delete(int id)
        //{
        //    var ciudad = db.Ciudades.Find(id);
        //    return View(ciudad);
        //}

        //[HttpPost, ActionName("Delete")]
        public JsonResult DeleteConfirmed(int id, int idEstado)
        {
            var res = "true";
            //using (var transaction = new TransactionScope())
            //{
            try
            {
                using (var transaction = new System.Transactions.TransactionScope())
                {
                    var ciudad = db.Ciudades.Where(c => c.Id == id).First();
                    //var estado = db.Estados.Find(id);
                    var poligono = db.Poligonos.Where(p => p.Id == ciudad.PoligonoId).First();
                    var poligonoDetalles = db.PoligonosDetalle.Where(pd => pd.PoligonoId == poligono.Id);

                    foreach (var poligonoDetalle in poligonoDetalles)
                    {
                        db.PoligonosDetalle.Remove(poligonoDetalle);
                    }

                    db.Poligonos.Remove(poligono);
                    db.Ciudades.Remove(ciudad);

                    db.SaveChanges();
                    transaction.Complete();
                }
                //var ciudad = db.Ciudades.Find(id, idEstado);

                //var poligonoId = ciudad.PoligonoId;
                //var helper = new HelperController();

                //helper.RemovePoligonoDetalles(ciudad.PoligonoId);
                //db.Ciudades.Remove(ciudad);
                //db.SaveChanges();
                //helper.RemovePoligono(poligonoId);


                //db.SaveChanges();
                //transaction.Complete();
            }
            catch (Exception)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }

        //private void RemoveCiudades(Ciudad ciudad)
        //{
        //    var poligonoId = ciudad.PoligonoId;
        //    var helper = new HelperController();

        //    helper.RemovePoligonoDetalles(ciudad.PoligonoId);
        //    db.Ciudades.Remove(ciudad);
        //    db.SaveChanges();
        //    helper.RemovePoligono(poligonoId);
        //}

        public JsonResult RnEliminaCiudad(int id, int idEstado)
        {
            var res = "true";

            var ciudad = db.Ciudades.Find(new object[] { id, idEstado });
            if (ciudad == null || ciudad.Incidentes.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCiudades(int? estado)
        {
            var paginaActual = Convert.ToInt32(Request.Params["page"]);
            var registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var ciudades = (from c in db.Ciudades
                            join e in db.Estados on c.EstadoId equals e.Id
                            join usr in db.Usuarios on c.UsuarioAlta equals usr.Id into lj
                            from sub in lj.DefaultIfEmpty()
                            where e.Id == estado || estado == null
                            select new
                            {
                                id = c.Id,
                                idEstado = e.Id,
                                estado = e.Nombre,
                                ciudad = c.Nombre,
                                user = sub.UserName ?? "",
                                fechaAlta = c.FechaAlta,
                            }).ToList();


            var listaPaginada = ciudades.OrderBy(e => e.estado)
                                        .ThenBy(c => c.ciudad)
                                        .Skip((paginaActual - 1) * registrosPorPagina)
                                        .Take(registrosPorPagina);


            var salida = GridHelper.GetData(listaPaginada.ToList());
            return Json(new { page = paginaActual, total = ciudades.Count(), rows = salida }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarCiudadesPorEstado(int id)
        {
            var paginaActual = Convert.ToInt32(Request.Params["page"]);
            var registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var ciudades = (from c in db.Ciudades
                            join e in db.Estados on c.EstadoId equals e.Id
                            join usr in db.Usuarios on c.UsuarioAlta equals usr.Id into lj
                            from sub in lj.DefaultIfEmpty()
                            select new
                            {
                                id = c.Id,
                                idEstado = e.Id,
                                estado = e.Nombre,
                                ciudad = c.Nombre,
                                user = sub.UserName ?? "",
                                fechaAlta = c.FechaAlta,
                            }).Where(c => c.idEstado == id).ToList();


            var listaPaginada = ciudades.OrderBy(e => e.estado)
                                       .ThenBy(c => c.ciudad)
                                       .Skip((paginaActual - 1) * registrosPorPagina)
                                       .Take(registrosPorPagina);


            var salida = GridHelper.GetData(listaPaginada.ToList());
            return Json(new { page = paginaActual, total = ciudades.Count(), rows = salida }, JsonRequestBehavior.AllowGet);

        }

        //private static CiudadEntity ToCiudadEntity(Ciudad ciudad)
        //{
        //    var ciudadN = new CiudadEntity();
        //    ciudadN.Id = ciudad.Id;
        //    ciudadN.EstadoId = ciudad.EstadoId;
        //    ciudadN.Nombre = ciudad.Nombre;
        //    ciudadN.PoligonoId = ciudad.PoligonoId;
        //    ciudadN.UsuarioAlta = ciudad.UsuarioAlta;
        //    ciudadN.FechaAlta = ciudad.FechaAlta;
        //    return ciudadN;
        //}

        //private static Ciudad ToCiudad(CiudadEntity ciudadEntity)
        //{
        //    var ciudad = new Ciudad();
        //    ciudad.Id = ciudadEntity.Id;
        //    ciudad.EstadoId = ciudadEntity.EstadoId;
        //    ciudad.Nombre = ciudadEntity.Nombre;
        //    ciudad.PoligonoId = ciudadEntity.PoligonoId;
        //    ciudad.UsuarioAlta = ciudadEntity.UsuarioAlta;
        //    ciudad.FechaAlta = ciudadEntity.FechaAlta;
        //    return ciudad;
        //}

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult GetCoords(int filterId)
        {
            var ciudad = db.Ciudades.Where(e => e.Id == filterId).First();

            var Coords =
                (from cords in db.PoligonosDetalle where cords.PoligonoId == ciudad.PoligonoId select new { latitud = cords.Latitud, longitud = cords.Longitud });
            return Json(Coords.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
