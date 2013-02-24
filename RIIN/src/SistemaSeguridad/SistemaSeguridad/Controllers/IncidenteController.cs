using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SS.Core.Mailer;
using SS.Core.Security;
using SistemaSeguridad.Content;
using SistemaSeguridad.Enums;
using SS.Core.Entities;
using SistemaSeguridad.Helpers;
using SS.Core.Security.Authorization;

namespace SistemaSeguridad.Controllers
{
    //[Authorize]
    public class IncidenteController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        public JsonResult GetInstalacionCords(int instalacionId)
        {
            var cords = db.Instalaciones.Find(instalacionId);
            return Json(new string[] { cords.Latitud.ToString(), cords.Longitud.ToString() }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetIncientes(bool isFiltered, int grupoId, int empresaId, int tipoIncidenteId, int tipoAfectacionId, int tipoInstalacionId,
                                        int instalacionId, int usuarioId, DateTime fechaDesde, DateTime fechaHasta, string incidenteId)
        {

            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);
            var userId = Convert.ToInt32(Session["userNameId"]);
            tipoAfectacionId = 2;
            var afecta = (from ai in db.AfectacionesIncidente.Where(afe => afe.Id == tipoAfectacionId) select ai);


            var incidentes = (from incidente in db.Incidentes.Include("Instalaciones").Include("AfectacionIncidentes")
                              join t3 in db.TiposInstalacion on incidente.TipoInstalacionId equals t3.Id into tiposIns
                              from ti in tiposIns.DefaultIfEmpty()
                              join t4 in db.Instalaciones on incidente.InstalacionId equals t4.Id into instalaciones
                              from ins in instalaciones.DefaultIfEmpty()
                              join t1 in db.relUsuarioEmpresa on incidente.EmpresaId equals t1.Empresa.Id
                              join t2 in db.TiposIncidente on incidente.TipoIncidenteId equals t2.Id
                              where incidente.FechaCancelacion.HasValue == false //&& incidente.Id ==17
                              //&& t1.UsuarioId.Equals(usuarioId)
                              select incidente).Distinct();

            //var filtrado = incidentes.Where(a => a.AfectacionIncidentes.Equals(afecta));
            var filtrado = incidentes.Where(inc => afecta.Any(afec => afec.IncidenteId.Equals(inc.Id)));

            var listaPaginada = filtrado.OrderByDescending(c => c.FechaIncidente)
                .Skip((paginaActual - 1) * registrosPorPagina)
                .Take(registrosPorPagina);

            var girdResult = new Flexgrid();
            GridRow miRow;
            int i = 0;


            var afectaciones = string.Empty;
            girdResult.total = incidentes.Count();
            girdResult.page = paginaActual;
            foreach (var incidente in listaPaginada)
            {
                afectaciones = incidente.AfectacionIncidentes.Aggregate(afectaciones,
                                                                        (current, afectacion) =>
                                                                        current +
                                                                        (afectacion.TipoAfectacion.Nombre + ", "));
                afectaciones = afectaciones.Length > 2
                                   ? afectaciones.Substring(0, afectaciones.Length - 2)
                                   : string.Empty;

                i += 1;
                miRow = new GridRow();
                miRow.id = i;
                var monto = incidente.MontoAfectacion.HasValue == false ? 0 : incidente.MontoAfectacion;
                var detenidos = incidente.Detenidos.HasValue == false ? false : incidente.Detenidos;
                var convehiculo = incidente.ConVehiculo.HasValue == false ? false : incidente.ConVehiculo;
                var lesionados = incidente.LesionadosId.HasValue == false ? string.Empty : incidente.Lesionado.Nombre;
                
               

                var tipoInstalacion = incidente.TipoInstalacionId.HasValue == false
                                          ? string.Empty
                                          : incidente.TipoInstalacion.Nombre;

                var instalacion = incidente.InstalacionId.HasValue == false
                                      ? string.Empty
                                      : incidente.Instalaciones.Nombre;
                var tipoArma = incidente.TipoArmaId.HasValue == false ? string.Empty : incidente.TipoArma.Nombre;
                var cantidadDelincuentes = incidente.CantidadDelincuentesId.HasValue == false
                                               ? string.Empty
                                               : incidente.CantidadDelincuente.NombreDeCantidad;
                var tipoVehiculo = incidente.TipoVehiculoId.HasValue == false
                                       ? string.Empty
                                       : incidente.TipoVehiculo.Nombre;
                var tipoExtorsion = incidente.TipoExtorsionId.HasValue == false
                                        ? string.Empty
                                        : incidente.TipoExtorsion.Nombre;
                var medioamenaza = incidente.MedioAmenazaId.HasValue == false
                                       ? string.Empty
                                       : incidente.MedioAmenaza.Nombre;
                var motivoamenaza = incidente.MotivoAmenazaId.HasValue == false
                                        ? string.Empty
                                        : incidente.MotivoAmenaza.Nombre;
                var tipoIntrusion = incidente.TipoIntrusionId.HasValue == false
                                        ? string.Empty
                                        : incidente.TipoIntrusion.Nombre;
                var zona = incidente.ZonaId.HasValue == false ? string.Empty : incidente.Zona.Nombre;


                miRow.cell = new List<string>()
                                 {
                                     Convert.ToString(incidente.Id),
                                     incidente.FechaIncidente.ToString("MMMM dd ,yy HH:MM"),
                                     incidente.TipoIncidente.Nombre,
                                     lesionados,
                                     tipoInstalacion,
                                     instalacion,
                                     monto.ToString(),
                                     string.Format("Colonia: {0}, Calle: {1}, Entre calles: {2}", incidente.Colonia,
                                                   incidente.Calle,
                                                   incidente.EntreCalles),
                                     afectaciones,
                                    incidente.TipoIncidente.Nombre,
                                     tipoArma,
                                     detenidos.ToString(),
                                     cantidadDelincuentes,
                                     tipoVehiculo,
                                     convehiculo.ToString(),
                                     tipoExtorsion,
                                     medioamenaza,
                                     motivoamenaza,
                                     tipoIntrusion,
                                     incidente.Comentarios,
                                     zona,
                                     incidente.Ciudad.Nombre,
                                     incidente.Estado.Nombre
                                 };
                girdResult.rows.Add(miRow);
                afectaciones = string.Empty;

            }

            return Json(girdResult);

        }


        //
        // GET: /Incidente/
        [RequireAuthorization]
        public ViewResult Index()
        {
            //var incidentes = db.Incidentes.Include(i => i.TipoArma).Include(i => i.TipoIncidente).Include(i => i.CantidadDelincuente).Include(i => i.Lesionado).Include(i => i.TipoAmenaza).Include(i => i.MedioAmenaza).Include(i => i.TipoIntrusion).Include(i => i.TipoVehiculo).Include(i => i.TipoIncidente).Include(i => i.Usuario).Include(i => i.Empresa).Include(i => i.Estado).Include(i => i.Ciudad).Include(i => i.Zona).Include(i => i.Instalaciones);
            return View();
        }

        public ActionResult GetViewFromIncidente(int tipoIncidenteId)
        {
            string viewName;
            switch (tipoIncidenteId)
            {
                case 1:
                    viewName = "GetRoboConViolencia";
                    break;
                case 2:
                    viewName = "GetRoboSinViolencia";
                    break;
                case 3:
                    viewName = "GetSecuestroEnRuta";
                    break;
                case 4:
                    viewName = "GetExtorcion";
                    break;
                case 5:
                    viewName = "GetAmenaza";
                    break;
                case 6:
                    viewName = "GetIntrusion";
                    break;
                default:
                    viewName = "EmptyView";
                    break;
            }
            return PartialView(viewName);
        }

        public JsonResult GetParametroEdicionIncidente()
        {
            return Json(ParametrosSistemaEnums.DiasValidosParaEdicionIncidente);
        }



        //
        // GET: /Incidente/Details/5

        public ViewResult Details(int id)
        {
            var incidente = db.Incidentes.Find(id);
            return View(incidente);
        }

        //
        // GET: /Incidente/Create
        //[RequireAuthorization]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Incidente/Create

        [HttpPost]
        public ActionResult Create(Incidente incidente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Cambiar correo de notificacion a la configuracion de notificaciones
                    incidente.FechaAlta = DateTime.Now;
                    incidente.FechaIncidente = new DateTime(incidente.TmpFechaIncidente.Year,
                                                            incidente.TmpFechaIncidente.Month, incidente.TmpFechaIncidente.Day,
                                                            incidente.TmpHoraIncidente.Hour, incidente.TmpHoraIncidente.Minute,
                                                            0);
                    incidente.UsuarioAlta = 1;//Convert.ToInt32(Session["userNameId"]);
                    if (incidente.ZonaId == 0)
                        incidente.ZonaId = null;
                    if (incidente.InstalacionId == 0)
                        incidente.InstalacionId = null;

                    db.Incidentes.Add(incidente);
                    db.SaveChanges();
                    SendNotifications(incidente);

                    return Json(new object[] { true, "saved" });
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
            else
            {
                string errorResult = string.Empty;
                foreach (var key in ModelState.Values)
                {
                    foreach (var error in key.Errors)
                    {
                        errorResult += error.ErrorMessage + "|";
                    }
                }
                errorResult = errorResult.Substring(0, errorResult.Length - 1);

                return Json(new object[] { false, errorResult });
            }
            return View(incidente);
        }

        public JsonResult CancelaIncidente(int Id)
        {
            var res = 0;
            var incidente = db.Incidentes.Find(Id);
            if (incidente != null)
            {
                incidente.FechaCancelacion = DateTime.Now;

                db.Entry(incidente).State = EntityState.Modified;
                db.SaveChanges();
                res = 1;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Incidente/Edit/5

        public ActionResult Edit(int id)
        {
            var incidente = db.Incidentes.Find(id);

            return View(incidente);
        }

        //
        // POST: /Incidente/Edit/5

        [HttpPost]
        public ActionResult Edit(Incidente incidente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incidente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incidente);
        }

        //
        // GET: /Incidente/Delete/5

        public ActionResult Delete(int id)
        {
            var incidente = db.Incidentes.Find(id);
            return View(incidente);
        }

        //
        // POST: /Incidente/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var incidente = db.Incidentes.Find(id);
            db.Incidentes.Remove(incidente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult GetZoneForIncident(float lat, float lng)
        {
            var db = new RiinContainer();
            var result = false;
            Zona zone = null;
            PoligonoDetalle point = new PoligonoDetalle();
            point.Latitud = lat;
            point.Longitud = lng;
            var Zonas = db.Zonas.Where(z => z.Poligonos.PoligonoDetalles.Count > 0);
            foreach (var zona in Zonas)
            {
                var polygon = zona.Poligonos.PoligonoDetalles.ToList();

                result = IsPointInPolygon(polygon, point);
                if (result)
                {
                    zone = zona;
                    break;
                }

            }
            return Json(zone != null ? zone.Id : -1, JsonRequestBehavior.AllowGet);
        }

        static bool IsPointInPolygon(IList<PoligonoDetalle> polygon, PoligonoDetalle point)
        {
            int i, j;
            var c = false;
            for (i = 0, j = polygon.Count - 1; i < polygon.Count; j = i++)
            {
                if ((((polygon[i].Latitud <= point.Latitud) && (point.Latitud < polygon[j].Latitud)) ||
                    ((polygon[j].Latitud <= point.Latitud) && (point.Latitud < polygon[i].Latitud))) &&
                    (point.Longitud < (polygon[j].Longitud - polygon[i].Longitud) * (point.Latitud - polygon[i].Latitud) / (polygon[j].Latitud - polygon[i].Latitud) + polygon[i].Longitud))
                    c = !c;
            }
            return c;
        }

        public bool SendNotifications(Incidente incidente)
        {
            var sb = new StringBuilder();
            var res = false;
            var userId = 1;//(int) Session["userNameId"];
            var empresasId = (from empresa in db.relUsuarioEmpresa where empresa.UsuarioId == userId select empresa);
       
            var emails = (from email in db.AlertasIncidente 
                          //join e in db.relUsuarioEmpresa on email.EmpresaId equals e.EmpresaId
                          where email.TipoIncidenteId.Equals(incidente.TipoIncidenteId) //&& e.UsuarioId == userId
                          select email);
            var mensaje = string.Empty;

            var TipoIncidente = db.TiposIncidente.FirstOrDefault(e => e.Id == incidente.TipoIncidenteId);

            foreach (var alerta in emails)
            {
                var nomEmpresa = (db.Empresas.FirstOrDefault(e => e.Id == incidente.EmpresaId));
                var Estado = (db.Estados.FirstOrDefault(e => e.Id == incidente.EstadoId));
                var ciudad = (db.Ciudades.FirstOrDefault(c => c.Id == incidente.CiudadId));
                var zona = (db.Zonas.FirstOrDefault(z => z.Id == incidente.ZonaId));

                var mail = new ServiceMailer();
                var message = new Email();
                var email = alerta.Emails.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
                email.ForEach(e=> message.To.Add(e));
                message.Subject = "Alerta de incidente.";
                mensaje += "Fecha y hora del incidente: " + incidente.FechaIncidente + "\r\n";
                mensaje += "Tipo de Incidente: " + TipoIncidente.Nombre + "\r\n";
                mensaje += "Empresa: " + nomEmpresa.Nombre + "\r\n";
                mensaje += string.Format("Ubicación: Estado: {0}, Ciudad: {1}, Colonia: {2}, Calle: {3}, Entre calles: {4}", Estado.Nombre, ciudad.Nombre, incidente.Colonia,
                                    incidente.Calle, incidente.EntreCalles) + "\r\n";
                mensaje += string.Format("Zona: {0}", incidente.ZonaId != null ? zona.Nombre : "No definida");
                message.Body = mensaje;
                foreach (var empresa in empresasId)
                {
                    if (alerta.RecibirMiEmpresa && empresa.EmpresaId == alerta.EmpresaId)
                    {
                        res = mail.SendMail(message);
                    }

                    if (alerta.RecibirOtrasEmpresas && alerta.EmpresaId != empresa.EmpresaId)
                    {
                        res = mail.SendMail(message);
                    }
                }
            }

          
            return res;
        }


    }
}
