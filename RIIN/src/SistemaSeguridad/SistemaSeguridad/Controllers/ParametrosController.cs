using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using SistemaSeguridad.Helpers;
using System.Collections.Generic;

namespace SistemaSeguridad.Controllers
{
    public class ParametrosController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /Parametros/

        public ViewResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            var parametros = db.ParametrosSistemaEmpresa
                                                        .Select(e => new
                                                                {
                                                                    Id = e.Id,
                                                                    EmpresaId = e.EmpresaId,
                                                                    TipoIncidenteId = e.TipoIncidenteId,
                                                                    Empresa = e.Empresa.Nombre,
                                                                    Incidente = e.TipoIncidente.Nombre,
                                                                    Parametro = e.ParametrosSistema.Nombre,
                                                                    e.ValorInicial,
                                                                    e.Valorfinal,
                                                                    e.FechaAlta
                                                                });

            var listaPaginada = parametros.OrderBy(p => p.Empresa)
                                          .ThenBy(p => p.Incidente)
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
                        item.EmpresaId.ToString(),
                        item.TipoIncidenteId.ToString(),
                        item.Empresa,
                        item.Incidente,
                        item.Parametro,
                        item.ValorInicial.ToString(),
                        item.Valorfinal.ToString(),
                        item.FechaAlta.ToString()
                    }
                });
            }

            var girdResult = new Flexgrid { page = paginaActual, total = parametros.Count(), rows = salida };

            return Json(girdResult);
        }

        //
        // GET: /Parametros/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Parametros/Create

        [HttpPost]
        public ActionResult Create(ParametroSistemaEmpresa parametrosistemaempresa)
        {
            parametrosistemaempresa.FechaAlta = DateTime.Today;
            parametrosistemaempresa.UsuarioAlta = Convert.ToInt32(Session["userNameId"]);

            ValidaParametrosSistemaEmpresa(parametrosistemaempresa);

            if (ModelState.IsValid)
            {
                db.ParametrosSistemaEmpresa.Add(parametrosistemaempresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(parametrosistemaempresa);
        }

        public void ValidaParametrosSistemaEmpresa(ParametroSistemaEmpresa parametroSistemaEmpresa)
        {
            var parametroSistema = db.ParametrosSistema.Where(ps => ps.Id == parametroSistemaEmpresa.ParametrosSistemaId)
                                                       .First();

            if (!TipoEmpresaValido(parametroSistemaEmpresa, parametroSistema))
                ModelState.AddModelError("TipoParametro", "El tipo de parametro especificado no es válido para la empresa seleccionada.");

            if (parametroSistema.TipoIncidenteEsRequerido &&
                parametroSistemaEmpresa.TipoIncidenteId <= 0)
            {
                ModelState.AddModelError("TipoIncidenteId", "El tipo de incidente es requerido.");
            }
            if (parametroSistema.ValorFinalEsRequerido)
            {
                if (!ValorFinarValido(parametroSistemaEmpresa))
                {
                    ModelState.AddModelError("ValorFinal", "El valor final es requerido.");
                }
                else if (parametroSistemaEmpresa.ValorInicial > parametroSistemaEmpresa.Valorfinal.Value)
                {
                    ModelState.AddModelError("ValorFinal", "El valor final debe ser mayor al valor inicial.");
                }
            }
        }

        private bool ValorFinarValido(ParametroSistemaEmpresa parametroSistemaEmpresa)
        {
            return parametroSistemaEmpresa.Valorfinal.HasValue && parametroSistemaEmpresa.Valorfinal.Value > 0;
        }

        private bool TipoEmpresaValido(ParametroSistemaEmpresa parametroSistemaEmpresa, ParametrosSistema parametroSistema)
        {
            var reglas = new Func<ParametroSistemaEmpresa, bool>[]
            {
                ps => (ps.EmpresaId == 1 || ps.EmpresaId == 2) && parametroSistema.TipoDeParametro != 1,
                ps => (ps.EmpresaId != 1 && ps.EmpresaId != 2) && parametroSistema.TipoDeParametro != 2
            };

            return reglas.All(regla => regla(parametroSistemaEmpresa) == false);           
        }

        //
        // GET: /Parametros/Edit/5

        public ActionResult Edit(int id)
        {
            var parametrosistemaempresa = db.ParametrosSistemaEmpresa.Where(ps => ps.Id == id).First();
            return View(parametrosistemaempresa);
        }

        //
        // POST: /Parametros/Edit/5

        [HttpPost]
        public ActionResult Edit(ParametroSistemaEmpresa parametrosistemaempresa)
        {
            var parametro = db.ParametrosSistemaEmpresa.Where(ps => ps.Id == parametrosistemaempresa.Id).First();

            ValidaParametrosSistemaEmpresa(parametrosistemaempresa);

            if (ModelState.IsValid)
            {
                parametro.EmpresaId = parametrosistemaempresa.EmpresaId;
                parametro.ParametrosSistemaId = parametrosistemaempresa.ParametrosSistemaId;
                parametro.TipoIncidenteId = parametrosistemaempresa.TipoIncidenteId;
                parametro.ValorInicial = parametrosistemaempresa.ValorInicial;
                parametro.Valorfinal = parametrosistemaempresa.Valorfinal;
                parametro.UsuarioModificacion = Convert.ToInt32(Session["userNameId"]);
                parametro.FechaUltimaModificacion = DateTime.Now;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parametrosistemaempresa);
        }

        public ActionResult Delete(int id, int empresaId, int tipoIncidenteId)
        {
            var parametrosistemaempresa = (from x in db.ParametrosSistemaEmpresa
                                           where x.Id == id
                                           && x.EmpresaId == empresaId
                                           && x.TipoIncidenteId == tipoIncidenteId
                                           select x).Single();

            return View(parametrosistemaempresa);
        }

        public JsonResult RnEliminaTipoIntrusion(int id)
        {
            var res = "true";

            var parametro = db.ParametrosSistemaEmpresa.Find(id);
            if (parametro == null)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteConfirmed(int id, int empresaId, int tipoIncidenteId)
        {
            var res = "true";

            try
            {
                var parametrosistemaempresa = db.ParametrosSistemaEmpresa.Where(ps => ps.Id == id).First();

                db.ParametrosSistemaEmpresa.Remove(parametrosistemaempresa);
                db.SaveChanges();
            }
            catch (Exception)
            {
                res = "false";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminaParametro(int id)
        {
            var res = "true";

            var parametro = db.ParametrosSistemaEmpresa.Find(id);
            if (parametro == null)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ValidateRange(string minValue, string maxValue)
        {
            var returnValue = false;
            var min = int.Parse(minValue);
            var max = int.Parse(maxValue);

            if (min < max)
                returnValue = true;

            return Json(returnValue, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerEsRequerido(int id)
        {
            var res = "true";

            var parametroSistema = db.ParametrosSistema.Find(id);
            if (!parametroSistema.ValorFinalEsRequerido)
            {
                res = "false";
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerIncidenteEsRequerido(int id)
        {
            var res = "true";

            var parametroSistema = db.ParametrosSistema.Find(id);
            if (!parametroSistema.TipoIncidenteEsRequerido)
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
    }
}