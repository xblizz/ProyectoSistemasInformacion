using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using SS.Core.Entities;
using SistemaSeguridad.Enums;
using SistemaSeguridad.Models;

namespace SistemaSeguridad.Controllers
{
    public class HelperController : Controller
    {
        private RiinContainer db = new RiinContainer();

        public JsonResult GetGruposByUser(int userId = 0)
        {
            if (userId == 0) userId = Convert.ToInt32(Session["userNameId"]);

            //TODO: Delete this on release
            if (userId == 0) userId = 3;

            var grupoId = (from usrEmp in db.relUsuarioEmpresa
                         join emp in db.Empresas on usrEmp.EmpresaId equals emp.Id
                         where usrEmp.UsuarioId == userId
                         select usrEmp.Empresa.GrupoId);

            var gpoId = grupoId.First();
            var grupos = (from grupo in db.Empresas where grupo.TipoEmpresa && grupo.Id == gpoId select new { Id = grupo.Id, Descripcion = grupo.Nombre });
        
            return Json(grupos.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetGrupos()
        {
            var esAdmin = false;
            var userId = 1;// (int)Session["userNameId"];
            IList<UsuarioPerfil> perfilType = null;

            if (userId > 0)
                perfilType = GetPerfilesPorUsuario(userId);


            if (perfilType != null &&
                perfilType.Any(perfil => perfil.PerfilId == (int) PerfilesUsuarioEnum.AdministradorGeneral))
                esAdmin = true;

            if (!esAdmin)
            {
               return GetGruposByUser(userId);
            }
            else
            {
                var grupos =
                    from empresa in db.Empresas
                    where empresa.TipoEmpresa == true
                    select new {Id = empresa.Id, Descripcion = empresa.Nombre};
                return Json(grupos.ToList(), JsonRequestBehavior.AllowGet);
            }
          
        }

        public JsonResult ObtenerEmpresas()
        {
            var empresas =
                from empresa in db.Empresas
                where empresa.TipoEmpresa == false
                select new { Id = empresa.Id, Descripcion = empresa.Nombre };

            return Json(empresas.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParametrosSistema(int filterId)
        {
            var parametrosSistema = ((filterId == 1 || filterId == 2) ? 
                                                      db.ParametrosSistema.Where(ps => ps.TipoDeParametro == 1) :
                                                     db.ParametrosSistema.Where(ps => ps.TipoDeParametro == 2))
                                                     .Select(ps => new
                                                     {
                                                         Id = ps.Id,
                                                         Descripcion = ps.Nombre
                                                     });

            return Json(parametrosSistema.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmpresas(int filterId)
        {
            var empresas =
                from empresa in db.Empresas
                where empresa.TipoEmpresa == false && empresa.GrupoId == filterId
                select new { Id = empresa.Id, Descripcion = empresa.Nombre };

            return Json(empresas.ToList(), JsonRequestBehavior.AllowGet);
        }

        public List<UsuarioPerfil> GetPerfilesPorUsuario(int usuarioId)
        {
            var Perfiles =
                (from perfiles in db.relPerfilesUsuarios.Where(p => p.UsuarioId == usuarioId) select perfiles);
            return Perfiles.ToList();

        }

        public List<Empresa> GetEmpresasPorUsuario(int userId)
        {
            IList<Empresa> Empresas = null;
            var EmpresasUsuario = (from empresas in db.relUsuarioEmpresa.Where(e => e.UsuarioId == userId) select empresas);
            EmpresasUsuario.ToList().ForEach(e=> Empresas.Add(e.Empresa));
            
            return Empresas.ToList();
        }

        public int GetGrupoPorUsuario(int userId)
        {
            IList<Empresa> Empresas = GetEmpresasPorUsuario(userId);
            var grupo = 0;
            Empresas.ToList().ForEach(e => { if (e.GrupoId != null) grupo = (int) e.GrupoId; });
            return grupo;
        }

        public JsonResult GetPerfiles()
        {
            var perfiles =
                from perfil in db.Perfiles
                select new { Id = perfil.Id, Descripcion = perfil.Nombre };

            return Json(perfiles.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEstados()
        {
            var estados =
                from estado in db.Estados.OrderBy(e => e.Nombre)
                select new {Id = estado.Id, Descripcion = estado.Nombre};

            return Json(estados.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEstadoIdByDesc(string filter)
        {
            var estados =
                from estado in db.Estados.OrderBy(e => e.Nombre)
                where estado.Nombre.Contains(filter)
                select new { Id = estado.Id };

            return Json(estados.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCiudades(int filterId)
        {
            var ciudades =
                from ciudad in db.Ciudades.OrderBy(c => c.Nombre)
                where ciudad.EstadoId == filterId
                select new {Id = ciudad.Id, Descripcion = ciudad.Nombre};
                

            return Json(ciudades.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCiudadIdByDesc(string filter)
        {
            var ciudades =
                from ciudad in db.Ciudades.OrderBy(c => c.Nombre)
                where ciudad.Nombre.Contains(filter)
                select new { Id = ciudad.Id};


            return Json(ciudades.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZonas(int filterId)
        {
            var zonas =
                from zona in db.Zonas
                where zona.EstadoId == filterId
                select new { Id = zona.Id, Descripcion = zona.Nombre };

                return Json(zonas.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetZonasReports(int filterId)
        {
            var zonas =
                from zona in db.Zonas
                where zona.EstadoId == filterId
                select new { Id = zona.Id, Descripcion = zona.Nombre };

            var results = new List<Object>();
            results.Add(new Zona { Id = -1, Descripcion = "Sin Zona" });

            foreach (var item in zonas.ToList())
            {
                results.Add(item);
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLesionados()
        {
            var lesionados =
                from lesionado in db.Lesionados
                select new { Id = lesionado.Id, Descripcion = lesionado.Nombre };

            return Json(lesionados.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInstalaciones(int filterId)
        {
            var instalaciones =
                from instalacion in db.Instalaciones
                where instalacion.TipoInstalacionId == filterId
                select new { Id = instalacion.Id, Descripcion = instalacion.Nombre };

            return Json(instalaciones.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposArma()
        {
            var armas =
                from arma in db.TiposArma
                select new { Id = arma.Id, Descripcion = arma.Nombre };

            return Json(armas.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDelincuentes()
        {
            var delincuentes =
                from delincuente in db.CantidadDelincuentes
                select new { Id = delincuente.Id, Descripcion = delincuente.NombreDeCantidad };

            return Json(delincuentes.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposVehiculo()
        {
            var vehiculos =
                from vehiculo in db.TiposVehiculo
                select new { Id = vehiculo.Id, Descripcion = vehiculo.Nombre };

            return Json(vehiculos.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposExtorcion()
        {
            var extorciones =
                from extorcion in db.TiposExtorsion
                select new { Id = extorcion.Id, Descripcion = extorcion.Nombre };

            return Json(extorciones.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMotivosAmenaza()
        {
            var amenazas =
                from amenaza in db.MotivosAmenaza
                select new { Id = amenaza.Id, Descripcion = amenaza.Nombre };

            return Json(amenazas.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMediosAmenaza()
        {
            var medios =
                from medio in db.MediosAmenaza
                select new { Id = medio.Id, Descripcion = medio.Nombre };

            return Json(medios.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposIntrusion()
        {
            var intrusiones =
                from intrusion in db.TiposIntrusion
                select new { Id = intrusion.Id, Descripcion = intrusion.Nombre };

            return Json(intrusiones.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposIncidente()
        {
            var incidentes =
                from incidente in db.TiposIncidente
                select new { Id = incidente.Id, Descripcion = incidente.Nombre };

            return Json(incidentes.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposAfectacion()
        {
            var afectaciones =
                from afectacion in db.TiposAfectacion
                select new { Id = afectacion.Id, Descripcion = afectacion.Nombre };

            return Json(afectaciones.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTiposInstalacion()
        {
            var rows =
                from row in db.TiposInstalacion
                select new { Id = row.Id, Descripcion = row.Nombre };

            return Json(rows.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUsuarios(int filterId)
        {
            //var empresas = db.relUsuarioEmpresa.Where(empresa => empresa.Empresa.GrupoId.Equals(filterId)).FirstOrDefault();

            var emp =
                from empresa in db.relUsuarioEmpresa
                where empresa.Empresa.GrupoId == filterId
                select new { empresa };
            // return db.Usuarios.Where(user => user.UserName.Equals(userName)).FirstOrDefault();

            if (emp != null)
            {
                var usuarios = from usuario in
                                   db.Usuarios//.Include("UsuarioEmpresa").Where(usuario => usuario.UsuarioEmpresa.Contains(emp)).Select(
                               //usuario => 
                               select new { Id = usuario.Id, Descripcion = usuario.UserName };

                return Json(usuarios.ToList(), JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        internal void SavePoligonoDetalle(ICoordList mainObj)
        {
            var helper = new HelperController();

            //For edit we remove previously saved poligonos
            if (mainObj.PoligonoId != null && mainObj.PoligonoId != 0) helper.RemovePoligonoDetalles(mainObj.PoligonoId);

            if (!string.IsNullOrEmpty(mainObj.CoordsList))
            {
                var listPoligonos = new List<PoligonoDetalle>();

                //If is a new we create a poligono
                if (mainObj.PoligonoId == null || mainObj.PoligonoId == 0)
                {
                    //Create a new poligono
                    var poligono = new Poligono() { NivelGeograficoId = 1 };
                    db.Poligonos.Add(poligono);
                    db.SaveChanges();

                    mainObj.PoligonoId = poligono.Id;
                }

                mainObj.CoordsList = mainObj.CoordsList.TrimEnd(',');
                var splitted = mainObj.CoordsList.Split(',');

                var i = 0;
                while (i < splitted.Length - 1)
                {
                    float lat, lng;
                    if ((float.TryParse(splitted[i], out lat)) && (float.TryParse(splitted[i + 1], out lng)))
                    {
                        listPoligonos.Add(new PoligonoDetalle
                        {
                            PoligonoId = mainObj.PoligonoId ?? 1,
                            Latitud = lat,
                            Longitud = lng
                        });
                        i += 2;
                    }
                }

                //Prepares each detalle to be saved
                foreach (var detalle in listPoligonos)
                {
                    db.PoligonosDetalle.Add(detalle);
                }
            }

            db.SaveChanges();
        }

        /// <summary>
        /// Removes from DB memory collection PoligonosDetalle list
        /// </summary>
        /// <param name="poligonoId">Id to find</param>
        /// <remarks>This method doesn't apply commit or db.SaveChanges();</remarks>
        internal void RemovePoligonoDetalles(int? poligonoId)
        {
            if (poligonoId == null) return;

            var poligono = db.Poligonos.Find(poligonoId);

            if (poligono == null) return;

            var listDetail = from d in db.PoligonosDetalle
                             where d.PoligonoId == poligono.Id
                             select d;

            if (listDetail.Count() > 0)
            {
                //Removes PoligonoDetalle list from associated Poligono
                foreach (var item in listDetail)
                {
                    db.PoligonosDetalle.Remove(item);
                }
            }

            db.SaveChanges();
        }

        internal void RemovePoligono(int? poligonoId)
        {
            //Removes associated Poligono 
            //(We can't remove poligono here since Estado, Ciudad or Zona has an association
            if (poligonoId != null && poligonoId != 0)
            {
                var poligono = db.Poligonos.Find(poligonoId);

                if (poligono != null)
                {
                    //var listDetail = from d in db.PoligonosDetalle
                    //                 where d.PoligonoId == poligono.Id
                    //                 select d;

                    db.Poligonos.Remove(poligono);
                    db.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Used to retrieve associated detail from poligono
        /// </summary>
        /// <param name="poligonoId">Id to find</param>
        /// <param name="coordList">List of coordinates sent to HTML Input</param>
        /// <returns></returns>
        internal string GetPoligonoDetalle(int? poligonoId)
        {
            string coordList;

            if (poligonoId == null)
            {
                coordList = null;
                return null;
            }

            var ListPoligonoDetalle = from detalle in db.PoligonosDetalle
                                      where detalle.PoligonoId == poligonoId
                                      select detalle;

            var poligonosList = ListPoligonoDetalle.ToList();

            var myArray = new StringBuilder();

            foreach (var poligono in poligonosList)
            {
                myArray.Append(poligono.Latitud);
                myArray.Append(',');
                myArray.Append(poligono.Longitud);
                myArray.Append('|');
            }

            coordList = myArray.ToString();
            return coordList;
        }
    }
}
