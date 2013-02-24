using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

using SS.Core.Entities;
using SistemaSeguridad.Content;

namespace SistemaSeguridad.Controllers
{
    public class InstalacionController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();

        //
        // GET: /Instalacion/
        public ViewResult Index()
        {
            var instalaciones = db.Instalaciones.Include(i => i.Empresa).Include(i => i.TipoInstalacion);
            return View(instalaciones.ToList());
        }

        //
        // GET: /Instalacion/Create
        public ActionResult Create()
        {
            ViewBag.EmpresaId = new SelectList(db.Empresas, "Id", "Nombre");
            ViewBag.TipoInstalacionId = new SelectList(db.TiposInstalacion, "Id", "Nombre");
            return View();
        }

        //
        // POST: /Instalacion/Create

        [HttpPost]
        public ActionResult Create(Instalacion instalacion)
        {
            instalacion.FechaAlta = DateTime.Today;
            instalacion.UsuarioAlta = 3;

            if (ModelState.IsValid)
            {
                db.Instalaciones.Add(instalacion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmpresaId = new SelectList(db.Empresas, "Id", "Nombre", instalacion.EmpresaId);
            ViewBag.TipoInstalacionId = new SelectList(db.TiposInstalacion, "Id", "Nombre", instalacion.TipoInstalacionId);
            return View(instalacion);
        }

        //
        // GET: /Instalacion/Edit/5

        public ActionResult Edit(int id)
        {
            var instalacion = db.Instalaciones.Find(id);
            ViewBag.EmpresaId = new SelectList(db.Empresas, "Id", "Nombre", instalacion.EmpresaId);
            ViewBag.TipoInstalacionId = new SelectList(db.TiposInstalacion, "Id", "Nombre", instalacion.TipoInstalacionId);
            return View(instalacion);
        }

        //
        // POST: /Instalacion/Edit/5

        [HttpPost]
        public ActionResult Edit(Instalacion instalacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instalacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpresaId = new SelectList(db.Empresas, "Id", "Nombre", instalacion.EmpresaId);
            ViewBag.TipoInstalacionId = new SelectList(db.TiposInstalacion, "Id", "Nombre", instalacion.TipoInstalacionId);
            return View(instalacion);
        }

        //
        // GET: /Instalacion/Delete/5

        public ActionResult Delete(int id)
        {
            var instalacion = db.Instalaciones.Find(id);
            return View(instalacion);
        }

        //
        // POST: /Instalacion/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var instalacion = db.Instalaciones.Find(id);
            db.Instalaciones.Remove(instalacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult GetInstalacion()
        {
            //Get empresas only valid for current user
            var instalaciones = (from e in db.Instalaciones
                                 join empresa in db.Empresas on e.EmpresaId equals empresa.Id
                                 join tipo in db.TiposInstalacion on e.TipoInstalacionId equals tipo.Id
                                 select new
                                 {
                                     id = e.Id,
                                     empresa = empresa.Nombre,
                                     tipo = tipo.Nombre,
                                     nombre = e.Nombre,
                                     fechaAlta = e.FechaAlta,
                                 }).ToList();

            var salida = GridHelper.GetData(instalaciones.ToList());
            return Json(new { page = 1, total = 1, records = 2, rows = salida }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ObtenerEmpresas()
        {
            var empresas =
                from empresa in db.Empresas
                where empresa.TipoEmpresa == false
                select new { Id = empresa.Id, Descripcion = empresa.Nombre };



            //select e.* from Empresas e
            //left join relUsuarioEmpresa ue
            //on e.Id = ue.EmpresaId
            //left join Usuarios u
            //on u.Id = ue.UsuarioId
            //left join relPerfilesUsuarios pu
            //on pu.UsuarioId = u.Id
            //left join Perfiles p
            //on p.Id = pu.PerfilId
            //where p.Id = 2 AND u.Id = 6

            var emp = from empresa in db.Empresas
                      from usuarioEmpresa
                      in db.relUsuarioEmpresa
                           .Where(ue => ue.EmpresaId == empresa.Id)
                           .DefaultIfEmpty()
                      from usuario
                      in db.Usuarios
                           .Where(u => u.Id == usuarioEmpresa.UsuarioId)
                           .DefaultIfEmpty()
                      from perfilUsuario
                      in db.relPerfilesUsuarios
                           .Where(pu => pu.UsuarioId == usuario.Id)
                           .DefaultIfEmpty()
                      where perfilUsuario.PerfilId == 2
                      select new { Id = empresa.Id, Descripcion = empresa.Nombre };


                      //from perfil
                      //in db.Perfiles
                      //     .Where(p => p.Id == perfilUsuario.PerfilId)
                      //     .DefaultIfEmpty()                       
                      //where perfil.Id == 2
                      



                        
                        
             

            //join ue in db.relUsuarioEmpresa
            //on empresa.Id equals ue.EmpresaId into usuarioEmpresa
            //from u in usuarioEmpresa.DefaultIfEmpty()
            //select u;

            //var emp = from e in db.Empresas
            //          orderby e.Nombre
            //          select new 
            //          {
            //            Usuario = e.UsuarioEmpresa.Where(ue => ue.EmpresaId == e.Id)
            //                                      .Select(ue => ue)
            //                                      .Where(
            //          };


            return Json(emp.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}