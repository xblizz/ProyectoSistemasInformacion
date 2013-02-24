using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Collections;
using SS.Core.Entities;
using SistemaSeguridad.Models;
using SistemaSeguridad.Helpers;
using System.Collections.Generic;

namespace SistemaSeguridad.Controllers
{
    public class EmpresaController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAll()
        {
            int paginaActual = Convert.ToInt32(Request.Params["page"]);
            int registrosPorPagina = Convert.ToInt32(Request.Params["rp"]);

            //var usuarios = (from usuario in db.Usuarios.Include("UsuarioEmpresa") select usuario);
            var empresas = from e in db.Empresas
                           join u in db.Usuarios on e.UsuarioAlta equals u.Id into leftJoin
                           from sub in leftJoin.DefaultIfEmpty()
                           select new
                           {
                               id = e.Id,
                               nombre = e.Nombre,
                               belongToGroup = e.GrupoId,
                               nombreGrupo = (from e2 in db.Empresas where e2.Id == e.GrupoId select e2.Nombre),
                               tipo = e.TipoEmpresa == true ? "Grupo" : "Empresa",
                               user = sub.UserName ?? "",
                               fechaAlta = e.FechaAlta,
                           };


            var listaPaginada = empresas.OrderBy(e => e.nombre)
                                        .Skip((paginaActual - 1) * registrosPorPagina)
                                        .Take(registrosPorPagina);

            var girdResult = new Flexgrid { page = paginaActual, total = empresas.Count() };
            //GridRow miRow;
            int i = 1;

            //girdResult.total = empresas.Count();
            //girdResult.page = paginaActual;

            foreach (var empresa in listaPaginada)
            {
                //miRow = new GridRow();
                //miRow.id = i++;
                girdResult.rows.Add(new GridRow
                {
                    id = i++,
                    cell = new List<string>()
                           {
                               Convert.ToString(empresa.id),
                               empresa.nombre,
                               empresa.tipo,
                               empresa.nombreGrupo.ToString(),
                               empresa.user,
                               empresa.fechaAlta.ToString()
                           }
                });
                //girdResult.rows.Add(miRow);
            }
            return Json(girdResult);
        }


        ////
        //// GET: /Usuarios/
        //public JsonResult GetEmpresas()
        //{
        //    //var listComplete = from empresa in db.Empresas
        //    //                   select empresa;

        //    var listEmpresas = (from e in db.Empresas
        //                        join u in db.Usuarios on e.UsuarioAlta equals u.Id into leftJoin
        //                        from sub in leftJoin.DefaultIfEmpty()
        //                        select new
        //                        {
        //                            id = e.Id,
        //                            nombre = e.Nombre,
        //                            belongToGroup = e.GrupoId,
        //                            nombreGrupo = (from e2 in db.Empresas where e2.Id == e.GrupoId select e2.Nombre),
        //                            tipo = e.TipoEmpresa == true ? "Grupo" : "Empresa",
        //                            user = sub.UserName ?? "",
        //                            fechaAlta = e.FechaAlta,
        //                        }).ToList();

        //    var salida = GridHelper.GetData(listEmpresas.ToList());
        //    return Json(new { page = 1, total = 1, records = 2, rows = salida });
        //}

        //
        // GET: /Empresa/

        //public ViewResult Index()
        //{
        //    return View(db.Empresas.ToList());
        //}

        //
        // GET: /Empresa/Create

        public ActionResult Create()
        {
            //PopulateEmpresasDropDownList();
            return View();
        }

        //
        // POST: /Empresa/Create

        [HttpPost]
        public ActionResult Create(Empresa empresa)
        {
            empresa.FechaAlta = DateTime.Today;
            empresa.UsuarioAlta = 1;

            //if (string.IsNullOrEmpty(empresa.Nombre))
            //{
            //    ModelState.AddModelError("Nombre", "El nombre es requerido");
            //}


            ////Is group
            //if (empresa.TipoEmpresa) empresa.GrupoId = 0;

            //if ((!empresa.TipoEmpresa) && (empresa.Id <= 0))
            //    throw new Exception("esta empresa no es grupo y el grupo al que pertenece viene en blanco");

            //TODO: Validate that name is not repeated

            if (ModelState.IsValid)
            {
                empresa.GrupoId = empresa.Id; //We set the group
                db.Empresas.Add(empresa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //PopulateEmpresasDropDownList();
            return View(empresa);

            ////empresaEntity.FechaAlta = DateTime.Today;
            ////empresaEntity.UsuarioAlta = 1;

            ////Is group
            //if (empresaEntity.TipoEmpresa) empresaEntity.GrupoId = 0;

            //if ((!empresaEntity.TipoEmpresa) && (empresaEntity.EmpresaId <= 0))
            //    throw new Exception("coño esta empresa no es grupo y el grupo al que pertenece viene en blanco");

            ////TODO: Validate that name is not repeated

            //if (ModelState.IsValid)
            //{
            //    //var empresa = ToEmpresa(empresaEntity);
            //    empresa.GrupoId = empresaEntity.EmpresaId; //We set the group
            //    db.Empresas.Add(empresa);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");  
            //}

            //return View(empresaEntity);
        }

        //
        // GET: /Empresa/Edit/5
        public ActionResult Edit(int id)
        {
            var empresa = db.Empresas.Single(e => e.Id == id);
            return View(empresa);
        }

        //
        // POST: /Empresa/Edit/5

        [HttpPost]
        public ActionResult Edit(Empresa empresa)
        {
            if (ModelState.IsValid)
            {
                db.Empresas.Attach(empresa);
                db.Entry(empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }

        //
        // GET: /Empresa/Delete/5

        public ActionResult Temp()
        {
            //Validar si se puede borrar o no
            //Instalaciones
            //Usuarios
            //Incidentes
            //Parametros

            return Content("0");
        }

        public bool Delete(int id)
        {
            //Validar si se puede borrar o no
            //Instalaciones
            //Usuarios
            //Incidentes
            //Parametros

            //var empresa = db.Empresas.Single(e => e.Id == id);
            //return View(empresa);

            var sepuede = true;

            if (sepuede)
            {
                DeleteConfirmed(id);
                return true;
            }
            else
                return false;
        }

        //
        // POST: /Empresa/Delete/5

        public JsonResult DeleteConfirmed(int id)
        {
            var res = "true";
            try
            {
                var empresa = db.Empresas.Single(e => e.Id == id);
                db.Empresas.Remove(empresa);
                db.SaveChanges();
            }
            catch (Exception)
            {
               res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RnEliminaEmpresa(int id)
        {
            var res = "true";

            var empresa = db.Empresas.Find(id);

            if (empresa == null || empresa.Incidentes.Count > 0 ||
                empresa.AlertaIncidentes.Count > 0 || empresa.Instalaciones.Count > 0 ||
                empresa.ParametrosSistemaEmpresas.Count > 0 || empresa.UsuarioEmpresa.Count > 0)
            {
                res = "false";
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get a list of Empresas that are group
        /// </summary>
        /// <param name="empresaSeleccionada"></param>
        private void PopulateEmpresasDropDownList(object empresaSeleccionada = null)
        {
            var grupoEmpresasQry = from d in db.Empresas
                                   where d.TipoEmpresa
                                   orderby d.Nombre
                                   select d;

            ViewBag.EmpresaId = new SelectList(grupoEmpresasQry, "Id", "Nombre", empresaSeleccionada);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private static EmpresaEntity ToEmpresaEntity(Empresa empresa)
        {
            var empresaEntity = new EmpresaEntity();
            empresaEntity.Id = empresa.Id;
            empresaEntity.Nombre = empresa.Nombre;
            empresaEntity.GrupoId = empresa.GrupoId;
            empresaEntity.TipoEmpresa = empresa.TipoEmpresa;
            empresaEntity.UsuarioAlta = empresa.UsuarioAlta;
            empresaEntity.FechaAlta = empresa.FechaAlta;
            return empresaEntity;
        }

        private static Empresa ToEmpresa(EmpresaEntity empresaEntity)
        {
            var empresa = new Empresa();
            empresa.Id = empresaEntity.Id;
            empresa.Nombre = empresaEntity.Nombre;
            empresa.GrupoId = empresaEntity.GrupoId;
            empresa.TipoEmpresa = empresaEntity.TipoEmpresa;
            empresa.UsuarioAlta = empresaEntity.UsuarioAlta;
            empresa.FechaAlta = empresaEntity.FechaAlta;
            return empresa;
        }
    }
}
