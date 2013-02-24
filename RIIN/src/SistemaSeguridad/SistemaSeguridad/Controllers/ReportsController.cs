using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Configuration;
using System.Text;
using SS.Core.Entities;

namespace SistemaSeguridad.Controllers
{
    public class ReportsController : Controller
    {
        #region Fields
        ////URL Reporting Services: Server Name
        //string serverName;

        ////Folder where reports are located
        //string folderName;

        //string rptTabular;
        //string rptGeneral, rptGeneralChart, rptGeneralTable;
        //string rptTendenciaMes, rptTendenciaMesChart, rptTendenciaMesTable;
        //string rptTendenciaDia, rptTendenciaDiaChart, rptTendenciaDiaTable;
        //string rptTendenciaHora, rptTendeciaHoraChart, rptTendenciaHoraTable; 
        #endregion

        #region Vistas con Filtros
        public ActionResult Tabular()
        {
            return View();
        }
        public ActionResult General()
        {
            return View();
        }
        public ActionResult Tendencia2()
        {
            return View();
        }
        #endregion

        #region AssignParameters
        private StringBuilder AssignCommonParams(List<string> values)
        {
            var parametros = new StringBuilder();

            if (values[0].Check()) parametros.Append("EmpresaId=" + values[0]);
            if (values[1].Check()) parametros.Append("&EstadoId=" + values[1]);
            if (values[2].Check()) parametros.Append("&CiudadId=" + values[2]);
            if (values[3].Check()) parametros.Append("&ZonaId=" + values[3]);
            if (values[4].Check()) parametros.Append("&ConsolidadoFlg=" + values[4]);

            return parametros;
        }

        private StringBuilder AssignTabularParams(List<string> values)
        {
            var parametros = AssignCommonParams(values);

            if (values[5].Check()) parametros.Append("&FechaInicio=" + values[5]);
            if (values[6].Check()) parametros.Append("&FechaFinal=" + values[6]);
            if (values[7].Check()) parametros.Append("&MontoMenor=" + values[7]);
            if (values[8].Check()) parametros.Append("&MontoMayor=" + values[8]);
            if (values[9].Check()) parametros.Append("&strTipoIncidentesId=" + values[9]);

            //var filter = parametros.ToString();
            //var x = filter.Replace('&', ' ');
            //var a = x.Replace('=',':');

            CreateReportTitle(values[0],ref parametros);

            return parametros;
        }

        private StringBuilder AssignGeneralParams(List<string> values)
        {
            var parametros = AssignCommonParams(values);

            if (values[5].Check()) parametros.Append("&FechaInicio=" + values[5]);
            if (values[6].Check()) parametros.Append("&FechaFinal=" + values[6]);

            CreateReportTitle(values[0], ref parametros);

            return parametros;
        }
        #endregion

        private void ReadCfgPaths()
        {
            //var rootWebConfig = WebConfigurationManager.OpenWebConfiguration("~");
            //var seccionPages = rootWebConfig.GetSection("appSettings");

            //serverName = seccionPages.CurrentConfiguration.AppSettings.Settings["ReportServer"].Value;
            //folderName = seccionPages.CurrentConfiguration.AppSettings.Settings["ReportFolder"].Value;

            //rptTabular = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTabular"].Value;

            //rptGeneral = seccionPages.CurrentConfiguration.AppSettings.Settings["rptGeneral"].Value;
            //rptGeneralChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptGeneralChart"].Value;
            //rptGeneralTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptGeneralTable"].Value;

            //rptTendenciaMes = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMes"].Value;
            //rptTendenciaMesChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMesChart"].Value;
            //rptTendenciaMesTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMesTable"].Value;

            //rptTendenciaDia = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaDia"].Value;
            //rptTendenciaDiaChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaDiaChart"].Value;
            //rptTendenciaDiaTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaDiaTable"].Value;

            //rptTendenciaHora = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaHora"].Value;
            //rptTendeciaHoraChart = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaHoraChart"].Value;
            //rptTendenciaHoraTable = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaHoraTable"].Value;
        }

        private void CreateReportTitle(string empresaId, ref StringBuilder parametros)
        {
            var empresa = GetEmpresaName(empresaId);

            parametros.Append("&Notes=");
            parametros.Append(empresa);
        }

        private string GetEmpresaName(string id)
        {
            RiinContainer db = new RiinContainer();
            var empresaId = int.Parse(id);

            var empresa = from e in db.Empresas
                          where e.Id == empresaId
                          select e.Nombre;

            var nombre = empresa.ToList().First();

            return nombre;
        }

        private StringBuilder CreateFrame(string reportName, StringBuilder parametros)
        {
            var sb = new StringBuilder();

            sb.Append("<iframe id='ifReporte' width='100%' style='height: 600px' frameborder='0'");
            sb.Append("src='");
            sb.Append("../Reports/");
            sb.Append(reportName);
            sb.Append(".aspx?");
            sb.Append(parametros.ToString());
            sb.Append("'");
            sb.Append("></iframe>"); 
            return sb;
        }

        #region Tabular
        public JsonResult ShowTabular(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "Tabular";

            //Los parámetros con sus respectivos valores
            var parametros = AssignTabularParams(values);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region General
        public JsonResult ShowGeneral(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "General";

            //Los parámetros con sus respectivos valores
            var parametros = AssignGeneralParams(values);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowGeneralChart(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "GeneralChart";

            //Los parámetros con sus respectivos valores
            var parametros = AssignGeneralParams(values);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowGeneralTable(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "GeneralTable";

            //Los parámetros con sus respectivos valores
            var parametros = AssignGeneralParams(values);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region Tendencia Mes
        public JsonResult ShowTendenciaMes(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaMes";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[6].Check()) parametros.Append("&MesInicial=" + values[6]);
            if (values[7].Check()) parametros.Append("&MesFinal=" + values[7]);
            if (values[8].Check()) parametros.Append("&AnioInicial=" + values[8]);
            if (values[9].Check()) parametros.Append("&AnioFinal=" + values[9]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTendenciaMesChart(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaMesChart";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[6].Check()) parametros.Append("&MesInicial=" + values[6]);
            if (values[7].Check()) parametros.Append("&MesFinal=" + values[7]);
            if (values[8].Check()) parametros.Append("&AnioInicial=" + values[8]);
            if (values[9].Check()) parametros.Append("&AnioFinal=" + values[9]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTendenciaMesTable(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaMesTable";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[6].Check()) parametros.Append("&MesInicial=" + values[6]);
            if (values[7].Check()) parametros.Append("&MesFinal=" + values[7]);
            if (values[8].Check()) parametros.Append("&AnioInicial=" + values[8]);
            if (values[9].Check()) parametros.Append("&AnioFinal=" + values[9]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region Tendencia Dia
        public JsonResult ShowTendenciaDia(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaDia";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[10].Check()) parametros.Append("&FechaInicio=" + values[10]);
            if (values[11].Check()) parametros.Append("&FechaFinal=" + values[11]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTendenciaDiaChart(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaDiaChart";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[10].Check()) parametros.Append("&FechaInicio=" + values[10]);
            if (values[11].Check()) parametros.Append("&FechaFinal=" + values[11]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTendenciaDiaTable(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaDiaTable";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[10].Check()) parametros.Append("&FechaInicio=" + values[10]);
            if (values[11].Check()) parametros.Append("&FechaFinal=" + values[11]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region Tendencia Hora
        public JsonResult ShowTendenciaHora(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaHora";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[12].Check()) parametros.Append("&FechaInicio=" + values[12]);
            if (values[13].Check()) parametros.Append("&HoraInicio=" + values[13]);
            if (values[14].Check()) parametros.Append("&HoraFinal=" + values[14]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTendenciaHoraChart(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaHoraChart";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[12].Check()) parametros.Append("&FechaInicio=" + values[12]);
            if (values[13].Check()) parametros.Append("&HoraInicio=" + values[13]);
            if (values[14].Check()) parametros.Append("&HoraFinal=" + values[13]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ShowTendenciaHoraTable(List<string> values)
        {
            ReadCfgPaths();

            //Report name to show
            string viewerPageName = "TendenciaHoraTable";

            //Los parámetros con sus respectivos valores
            var parametros = AssignCommonParams(values);

            if (values[12].Check()) parametros.Append("&FechaInicio=" + values[12]);
            if (values[13].Check()) parametros.Append("&HoraInicio=" + values[13]);
            if (values[14].Check()) parametros.Append("&HoraFinal=" + values[13]);

            var sb = CreateFrame(viewerPageName, parametros);

            //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
            return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        } 
        #endregion

        public JsonResult GetMonths()
        {
            var monthList = new Dictionary<string, string>();
            monthList.Add("1", "Enero");
            monthList.Add("2", "Febrero");
            monthList.Add("3", "Marzo");
            monthList.Add("4", "Abril");
            monthList.Add("5", "Mayo");
            monthList.Add("6", "Junio");
            monthList.Add("7", "Julio");
            monthList.Add("8", "Agosto");
            monthList.Add("9", "Septiembre");
            monthList.Add("10", "Octubre");
            monthList.Add("11", "Noviembre");
            monthList.Add("12", "Diciembre");

            var result = from r in monthList
                         select new {Id = r.Key, Descripcion = r.Value};

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetYears()
        {
            var currentYear = DateTime.Today.Year;
            var minimumYear = currentYear - 4;
            var validYears = new Dictionary<string, string>();

            validYears.Add(currentYear.ToString(), currentYear.ToString());

            for (int i = 1; i < 5; i++)
            {
                var thisYear = currentYear - i;
                validYears.Add(thisYear.ToString(), thisYear.ToString());
            }

            var result = from r in validYears
                         select new { Id = r.Key, Descripcion = r.Value };

            return Json(result.ToList(), JsonRequestBehavior.AllowGet);
        }
    }

    internal static class ExtensionMethods
    {
        public static bool Check(this string thisValue)
        {
            if (
                thisValue != null &&
                thisValue != "0" &&
                thisValue != "undefined" &&
                thisValue != "null" &&
                !string.IsNullOrEmpty(thisValue)
                )
                return true;
            else
                return false;
        }
    }
}


//public JsonResult ShowTendencia(List<string> values)
//{
//    ReadCfgPaths();

//    //Los parámetros con sus respectivos valores
//    var parametros = AssignCommonParams(values);

//    var sb = new StringBuilder();

//    //Check if is tendencia mes/tendencia dia/tendencia hora
//    if (values[15].Check())
//    {
//        //Filters for Tendencia Mes
//        if (values[15] == "1")
//        {
//            if (values[6].Check()) parametros.Append("&MesInicial=" + values[6]);
//            if (values[7].Check()) parametros.Append("&MesFinal=" + values[7]);
//            if (values[8].Check()) parametros.Append("&AnioInicial=" + values[8]);
//            if (values[9].Check()) parametros.Append("&AnioFinal=" + values[9]);
//        }

//        //Filters for Tendencia Dia
//        if (values[15] == "2")
//        {
//            if (values[10].Check()) parametros.Append("&FechaInicio=" + values[10]);
//            if (values[11].Check()) parametros.Append("&FechaFinal=" + values[11]);
//        }

//        //Filters for Tendencia Hora
//        if (values[15] == "3")
//        {
//            if (values[12].Check()) parametros.Append("&FechaInicio=" + values[12]);
//            if (values[13].Check()) parametros.Append("&HoraInicio=" + values[13]);
//            if (values[14].Check()) parametros.Append("&HoraFinal=" + values[13]);
//        }

//        //Tendencia Mes
//        if (values[15] == "1" && values[16] == "all")
//            sb = CreateFrame(rptTendenciaMes, parametros);
//        else if (values[15] == "1" && values[16] == "chart")
//            sb = CreateFrame(rptTendenciaMesChart, parametros);
//        else if (values[15] == "1" && values[16] == "table")
//            sb = CreateFrame(rptTendenciaMesTable, parametros);

//        //Tendencia Dia
//        if (values[15] == "2" && values[16] == "all")
//            sb = CreateFrame(rptTendenciaDia, parametros);
//        else if (values[15] == "2" && values[16] == "chart")
//            sb = CreateFrame(rptTendenciaDiaChart, parametros);
//        else if (values[15] == "2" && values[16] == "table")
//            sb = CreateFrame(rptTendenciaDiaTable, parametros);

//        //Tendencia Hora
//        if (values[15] == "3" && values[16] == "all")
//            sb = CreateFrame(rptTendenciaHora, parametros);
//        else if (values[15] == "3" && values[16] == "chart")
//            sb = CreateFrame(rptTendeciaHoraChart, parametros);
//        else if (values[15] == "3" && values[16] == "table")
//            sb = CreateFrame(rptTendenciaHoraTable, parametros);
//    }

//    CreateReportTitle(values[0], ref parametros);

//    //Retorna el stringBuilder en JSON y se permite todas las peticiones GET
//    return this.Json(sb.ToString(), JsonRequestBehavior.AllowGet);
//}