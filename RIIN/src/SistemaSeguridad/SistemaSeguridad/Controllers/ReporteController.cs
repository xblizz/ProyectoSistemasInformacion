using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

using SS.Core.Entities;
using SistemaSeguridad.Models;

namespace SistemaSeguridad.Controllers
{
    public class ReporteController : Controller
    {
        private readonly RiinContainer db = new RiinContainer();
        #region Mapa
        public JsonResult FilteredIncidentsToTableGroup(FiltroReporteModel filtro)
        {
            return Json(GetAllIncidentsToTableGroup(filtro), JsonRequestBehavior.AllowGet);
        }
        public JsonResult FilteredIncidents(FiltroReporteModel filtro)
        {
            return Json(GetAllIncidents().Where(FuncFilter(filtro)).ToList(), JsonRequestBehavior.AllowGet);
        }
        public List<GroupIncidentModel> GetIncidentsToTableGroup()
        {
            return (from incidente in GetAllIncidents()
                    group incidente by incidente.TipoIncidenteId into g
                    select new GroupIncidentModel {
                        NombreTipoIncidente = g.Select(f => f.TipoIncidente.Nombre).FirstOrDefault(),
                        NumeroEventos = g.Count().ToString(CultureInfo.InvariantCulture),
                        Semaforo = g.Count() >= 200 ? (Color) 1 : (Color) 2,
                        Zona = ""//g.Select(f => f.Zona.Nombre).FirstOrDefault()
                    }).ToList();
        }
        public JsonResult GetIncidents()
        {
            return Json(GetAllIncidents(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetZonas()
        {
            return Json(db.Estados.First(e => e.Id == 2).Zonas.Where(z => z.Poligonos.PoligonoDetalles.Count > 0).ToList(), JsonRequestBehavior.AllowGet);
        }

        private List<GroupIncidentModel> GetAllIncidentsToTableGroup(FiltroReporteModel filtro)
        {
            var filtroIncidente = GetAllIncidents().Where(FuncFilter(filtro)).ToList();
            return (from incidente in filtroIncidente
                    group incidente by incidente.TipoIncidenteId into g
                    select new GroupIncidentModel
                    {
                        NombreTipoIncidente = g.Select(f => f.TipoIncidente.Nombre).FirstOrDefault(),
                        NumeroEventos = g.Count().ToString(CultureInfo.InvariantCulture),
                        Semaforo = g.Count() >= 200 ? (Color)1 : (Color)2,
                        Zona = g.Select(f => f.Zona.Nombre).FirstOrDefault()
                    }).ToList();
        }
        private List<Incidente> GetAllIncidents()
        {
            return db.Incidentes.Include(i => i.TipoArma).Include(i => i.TipoExtorsion).Include(i => i.CantidadDelincuente).Include(i => i.Lesionado).Include(i => i.MotivoAmenaza).Include(i => i.MedioAmenaza).Include(i => i.TipoIntrusion).Include(i => i.TipoVehiculo).Include(i => i.TipoIncidente).Include(i => i.Usuario).Include(i => i.Empresa).Include(i => i.Estado).Include(i => i.Ciudad).Include(i => i.Zona).Include(i => i.Instalaciones).ToList();
        }
        private static Func<Incidente, bool> FuncFilter(FiltroReporteModel filtro)
        {
            Func<Incidente, bool> filtroo = incidente => incidente.EstadoId == filtro.EstadoId &&
                                                         incidente.CiudadId == filtro.CiudadId &&
                                                         incidente.FechaIncidente == filtro.FechaInicio &&
                                                         incidente.TipoIncidenteId == filtro.TipoIncidenteId &&
                                                         incidente.ZonaId == filtro.ZonaId;
            return filtroo;
        }
        #endregion

        //Remy
        #region Fields
        //URL Reporting Services: Server Name
        string serverName;
        //Folder where reports are located
        string folderName;
        string reportServerPath, reportFolderPath;
        string rptTabularName;
        string rptTabular;
        string rptGeneral, rptGeneralChart, rptGeneralTable;
        string rptTendenciaMes, rptTendenciaMesChart, rptTendenciaMesTable;
        string rptTendenciaDia, rptTendenciaDiaChart, rptTendenciaDiaTable;
        string rptTendenciaHora, rptTendeciaHoraChart, rptTendenciaHoraTable;
        #endregion

        #region Vistas con Filtros
        public ActionResult Dashboard()
        {
            return View();
        }
        public ViewResult AllReports()
        {
            return View();
        }
        public ActionResult Mapa()
        {
            return View(GetIncidentsToTableGroup());
        }
        public ActionResult Tabular()
        {
            return View();
        }
        public ActionResult General()
        {
            return View();
        }
        public ActionResult TendenciaDia()
        {
            return View();
        }
        public ActionResult TendenciaHora()
        {
            return View();
        }
        public ActionResult TendenciaMes()
        {
            return View();
        }
        #endregion

        #region AssignParameters
        private static StringBuilder AssignCommonParams(List<string> values)
        {
            var parametros = new StringBuilder();

            if (values[0].Check()) parametros.Append("&EmpresaId=" + values[0]);
            if (values[1].Check()) parametros.Append("&EstadoId=" + values[1]);
            if (values[2].Check()) parametros.Append("&CiudadId=" + values[2]);
            if (values[3].Check()) parametros.Append("&ZonaId=" + values[3]);
            if (values[4].Check()) parametros.Append("&ConsolidadoFlg=" + values[4]);

            return parametros;
        }
        private static StringBuilder AssignTabularParams(List<string> values)
        {
            var parametros = AssignCommonParams(values);

            if (values[5].Check()) parametros.Append("&FechaInicio=" + values[5]);
            if (values[6].Check()) parametros.Append("&FechaFinal=" + values[6]);
            if (values[7].Check()) parametros.Append("&MontoMenor=" + values[7]);
            if (values[8].Check()) parametros.Append("&MontoMayor=" + values[8]);
            if (values[9].Check()) parametros.Append("&strTipoIncidentesId=" + values[9]);

            return parametros;
        }
        private static StringBuilder AssignGeneralParams(List<string> values)
        {
            var parametros = AssignCommonParams(values);

            if (values[5].Check()) parametros.Append("&FechaInicio=" + values[5]);
            if (values[6].Check()) parametros.Append("&FechaFinal=" + values[6]);

            return parametros;
        }
        #endregion

        private void ReadCfgPaths()
        {
            var rootWebConfig = WebConfigurationManager.OpenWebConfiguration("~");
            var seccionPages = rootWebConfig.GetSection("appSettings");

            serverName = seccionPages.CurrentConfiguration.AppSettings.Settings["ReportServer"].Value;
            folderName = seccionPages.CurrentConfiguration.AppSettings.Settings["ReportFolder"].Value;

            rptTabular = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTabular"].Value;

            rptGeneral = seccionPages.CurrentConfiguration.AppSettings.Settings["rptGeneral"].Value;
            rptGeneralChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptGeneralChart"].Value;
            rptGeneralTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptGeneralTable"].Value;

            rptTendenciaMes = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMes"].Value;
            rptTendenciaMesChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMesChart"].Value;
            rptTendenciaMesTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMesTable"].Value;

            rptTendenciaDia = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaDia"].Value;
            rptTendenciaDiaChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaDiaChart"].Value;
            rptTendenciaDiaTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaDiaTable"].Value;

            rptTendenciaHora = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaHora"].Value;
            rptTendeciaHoraChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaHoraChart"].Value;
            rptTendenciaHoraTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaHoraTable"].Value;
        }
        private StringBuilder CreateFrame(string reportName, StringBuilder parametros)
        {
            var sParametroValor = parametros.ToString();

            //Comandos a pasar al Visor de Reporting Services
            //Esos comandos los consigue en: http://technet.microsoft.com/es-ve/library/ms152835.aspx
            var sComandosRS = "&rs:Command=Render&rs:Format=HTML4.0&rc:Parameters=false";
            //StringBuilder para crear un iFrame
            var sb = new StringBuilder();
            sb.Append("<iframe id='ifReporte' width='100%' style='height: 480px' frameborder='0'");
            sb.AppendFormat("src='{0}?/{1}/{2}{3}{4}'", serverName, folderName, reportName, sParametroValor, sComandosRS);
            sb.Append("></iframe>");
            return sb;
        }
        public JsonResult ShowTabular(List<string> values)
        {
            ReadCfgPaths();

            //Nombre del Reporte
            string reportName = rptTabular;

            //Los parámetros con sus respectivos valores
            var parametros = AssignTabularParams(values);

            //We build report Title
            //parametros.Append("Empresa+Parametros"); 

            var sb = CreateFrame(reportName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowGeneral(List<string> values)
        {
            ReadCfgPaths();

            //Nombre del Reporte
            string reportName = rptGeneral;

            //Los parámetros con sus respectivos valores
            var parametros = AssignGeneralParams(values);

            if (values[7].Check())
            {
                if (values[7] == "all") reportName = rptGeneral;
                else if (values[7] == "chart") reportName = rptGeneralChart;
                else if (values[7] == "table") reportName = rptGeneralTable;
            }

            var sb = CreateFrame(reportName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowTendencia(List<string> values)
        {
            ReadCfgPaths();

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            var sb = new StringBuilder();

            //Check if is tendencia mes/tendencia dia/tendencia hora
            if (values[15].Check())
            {
                //Filters for Tendencia Mes
                if (values[15] == "1")
                {
                    if (values[6].Check()) parametros.Append("&MesInicial=" + values[6]);
                    if (values[7].Check()) parametros.Append("&MesFinal=" + values[7]);
                    if (values[8].Check()) parametros.Append("&AnioInicial=" + values[8]);
                    if (values[9].Check()) parametros.Append("&AnioFinal=" + values[9]);
                }

                //Filters for Tendencia Dia
                if (values[15] == "2")
                {
                    if (values[10].Check()) parametros.Append("&FechaInicio=" + values[10]);
                    if (values[11].Check()) parametros.Append("&FechaFinal=" + values[11]);
                }

                //Filters for Tendencia Hora
                if (values[15] == "3")
                {
                    if (values[12].Check()) parametros.Append("&FechaInicio=" + values[12]);
                    if (values[13].Check()) parametros.Append("&HoraInicio=" + values[13]);
                    if (values[14].Check()) parametros.Append("&HoraFinal=" + values[13]);
                }

                //Tendencia Mes
                if (values[15] == "1" && values[16] == "all")
                    sb = CreateFrame(rptTendenciaMes, parametros);
                else if (values[15] == "1" && values[16] == "chart")
                    sb = CreateFrame(rptTendenciaMesChart, parametros);
                else if (values[15] == "1" && values[16] == "table")
                    sb = CreateFrame(rptTendenciaMesTable, parametros);

                //Tendencia Dia
                if (values[15] == "2" && values[16] == "all")
                    sb = CreateFrame(rptTendenciaDia, parametros);
                else if (values[15] == "2" && values[16] == "chart")
                    sb = CreateFrame(rptTendenciaDiaChart, parametros);
                else if (values[15] == "2" && values[16] == "table")
                    sb = CreateFrame(rptTendenciaDiaTable, parametros);

                //Tendencia Hora
                if (values[15] == "3" && values[16] == "all")
                    sb = CreateFrame(rptTendenciaHora, parametros);
                else if (values[15] == "3" && values[16] == "chart")
                    sb = CreateFrame(rptTendeciaHoraChart, parametros);
                else if (values[15] == "3" && values[16] == "table")
                    sb = CreateFrame(rptTendenciaHoraTable, parametros);
            }

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowTendenciaMes(List<string> values)
        {
            ReadCfgPaths();

            //URL Visor del Servidor de Reporting Services
            string serverName = reportServerPath;

            //Carpeta donde tenemos los reportes
            string folderName = reportFolderPath;

            //Nombre del Reporte
            string reportName = rptTabularName;

            //Los parámetros con sus respectivos valores
            var parametros = new StringBuilder();
            var zero = "0";

            if (values[0] != zero && values[0] != "undefined")
                parametros.Append("&EmpresaId=" + values[0]);

            if (values[1] != zero && values[1] != "undefined")
                parametros.Append("&EstadoId=" + values[1]);

            if (values[2] != zero && values[2] != "undefined")
                parametros.Append("&CiudadId=" + values[2]);

            if (values[3] != zero && values[3] != "undefined")
                parametros.Append("&ZonaId=" + values[3]);

            if (values[4] != zero && values[4] != "undefined")
                parametros.Append("&ConsolidadoFlg=" + values[4]);
            else
                parametros.Append("&ConsolidadoFlg=" + "true");

            if (values[5] != zero && values[5] != "undefined")
                parametros.Append("&FechaInicio=" + values[5]);
            else
                parametros.Append("&FechaInicio=" + "01/01/2012");

            if (values[6] != zero && values[6] != "undefined")
                parametros.Append("&FechaFinal=" + values[6]);
            else
                parametros.Append("&FechaFinal=" + "12/30/2012");

            if (values[7] != zero && values[7] != "undefined")
                parametros.Append("&MontoMenor=" + values[7]);

            if (values[8] != zero && values[8] != "undefined")
                parametros.Append("&MontoMayor=" + values[8]);

            if (values[9] != zero && values[9] != "undefined")
                parametros.Append("&strTipoIncidentesId=" + values[9]);

            //"&ContactID=" + id.Trim();
            string sParametroValor = parametros.ToString();

            //Comandos a pasar al Visor de Reporting Services
            //Esos comandos los consigue en: http://technet.microsoft.com/es-ve/library/ms152835.aspx
            string sComandosRS = "&rs:Command=Render&rs:Format=HTML4.0&rc:Parameters=false";

            //StringBuilder para crear un iFrame
            var sb = new StringBuilder();

            sb.Append("<iframe id='ifReporte' width='100%' style='height: 480px' frameborder='0'");
            sb.AppendFormat("src='{0}?/{1}/{2}{3}{4}'", serverName, folderName, reportName, sParametroValor, sComandosRS);
            sb.Append("></iframe>");

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowTendenciaDia(List<string> values)
        {
            ReadCfgPaths();

            //URL Visor del Servidor de Reporting Services
            string serverName = reportServerPath;

            //Carpeta donde tenemos los reportes
            string folderName = reportFolderPath;

            //Nombre del Reporte
            string reportName = rptTabularName;

            //Los parámetros con sus respectivos valores
            var parametros = new StringBuilder();
            var zero = "0";

            if (values[0] != zero && values[0] != "undefined")
                parametros.Append("&EmpresaId=" + values[0]);

            if (values[1] != zero && values[1] != "undefined")
                parametros.Append("&EstadoId=" + values[1]);

            if (values[2] != zero && values[2] != "undefined")
                parametros.Append("&CiudadId=" + values[2]);

            if (values[3] != zero && values[3] != "undefined")
                parametros.Append("&ZonaId=" + values[3]);

            if (values[4] != zero && values[4] != "undefined")
                parametros.Append("&ConsolidadoFlg=" + values[4]);
            else
                parametros.Append("&ConsolidadoFlg=" + "true");

            if (values[5] != zero && values[5] != "undefined")
                parametros.Append("&FechaInicio=" + values[5]);
            else
                parametros.Append("&FechaInicio=" + "01/01/2012");

            if (values[6] != zero && values[6] != "undefined")
                parametros.Append("&FechaFinal=" + values[6]);
            else
                parametros.Append("&FechaFinal=" + "12/30/2012");

            if (values[7] != zero && values[7] != "undefined")
                parametros.Append("&MontoMenor=" + values[7]);

            if (values[8] != zero && values[8] != "undefined")
                parametros.Append("&MontoMayor=" + values[8]);

            if (values[9] != zero && values[9] != "undefined")
                parametros.Append("&strTipoIncidentesId=" + values[9]);

            //"&ContactID=" + id.Trim();
            string sParametroValor = parametros.ToString();

            //Comandos a pasar al Visor de Reporting Services
            //Esos comandos los consigue en: http://technet.microsoft.com/es-ve/library/ms152835.aspx
            string sComandosRS = "&rs:Command=Render&rs:Format=HTML4.0&rc:Parameters=false";

            //StringBuilder para crear un iFrame
            var sb = new StringBuilder();

            sb.Append("<iframe id='ifReporte' width='100%' style='height: 480px' frameborder='0'");
            sb.AppendFormat("src='{0}?/{1}/{2}{3}{4}'", serverName, folderName, reportName, sParametroValor, sComandosRS);
            sb.Append("></iframe>");

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ShowTendenciaHora(List<string> values)
        {
            ReadCfgPaths();

            //URL Visor del Servidor de Reporting Services
            string serverName = reportServerPath;

            //Carpeta donde tenemos los reportes
            string folderName = reportFolderPath;

            //Nombre del Reporte
            string reportName = rptTabularName;

            //Los parámetros con sus respectivos valores
            var parametros = new StringBuilder();
            var zero = "0";

            if (values[0] != zero && values[0] != "undefined")
                parametros.Append("&EmpresaId=" + values[0]);

            if (values[1] != zero && values[1] != "undefined")
                parametros.Append("&EstadoId=" + values[1]);

            if (values[2] != zero && values[2] != "undefined")
                parametros.Append("&CiudadId=" + values[2]);

            if (values[3] != zero && values[3] != "undefined")
                parametros.Append("&ZonaId=" + values[3]);

            if (values[4] != zero && values[4] != "undefined")
                parametros.Append("&ConsolidadoFlg=" + values[4]);
            else
                parametros.Append("&ConsolidadoFlg=" + "true");

            if (values[5] != zero && values[5] != "undefined")
                parametros.Append("&FechaInicio=" + values[5]);
            else
                parametros.Append("&FechaInicio=" + "01/01/2012");

            if (values[6] != zero && values[6] != "undefined")
                parametros.Append("&FechaFinal=" + values[6]);
            else
                parametros.Append("&FechaFinal=" + "12/30/2012");

            if (values[7] != zero && values[7] != "undefined")
                parametros.Append("&MontoMenor=" + values[7]);

            if (values[8] != zero && values[8] != "undefined")
                parametros.Append("&MontoMayor=" + values[8]);

            if (values[9] != zero && values[9] != "undefined")
                parametros.Append("&strTipoIncidentesId=" + values[9]);

            //"&ContactID=" + id.Trim();
            string sParametroValor = parametros.ToString();

            //Comandos a pasar al Visor de Reporting Services
            //Esos comandos los consigue en: http://technet.microsoft.com/es-ve/library/ms152835.aspx
            string sComandosRS = "&rs:Command=Render&rs:Format=HTML4.0&rc:Parameters=false";

            //StringBuilder para crear un iFrame
            var sb = new StringBuilder();

            sb.Append("<iframe id='ifReporte' width='100%' style='height: 480px' frameborder='0'");
            sb.AppendFormat("src='{0}?/{1}/{2}{3}{4}'", serverName, folderName, reportName, sParametroValor, sComandosRS);
            sb.Append("></iframe>");

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}