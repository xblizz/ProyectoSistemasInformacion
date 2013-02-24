using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using Microsoft.Reporting.WebForms;
using System.Web.Configuration;

namespace SistemaSeguridad.Reports
{
    public partial class TendenciaMesTable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var rootWebConfig = WebConfigurationManager.OpenWebConfiguration("~");
                var seccionPages = rootWebConfig.GetSection("appSettings");

                var serverName = seccionPages.CurrentConfiguration.AppSettings.Settings["ReportServer"].Value;
                var folderName = seccionPages.CurrentConfiguration.AppSettings.Settings["ReportFolder"].Value;
                var reportName = seccionPages.CurrentConfiguration.AppSettings.Settings["rptTendenciaMesTable"].Value;
                var userName = WebConfigurationManager.AppSettings["ReportServerUser"];
                var password = WebConfigurationManager.AppSettings["ReportServerPassword"];
                var domain = WebConfigurationManager.AppSettings["ReportServerDomain"];

                ReportViewer1.ProcessingMode = ProcessingMode.Remote;

                //Specify the report server
                ReportViewer1.ServerReport.ReportServerUrl = new Uri(serverName);

                //Specify the report name
                ReportViewer1.ServerReport.ReportPath = "/" + folderName + "/" + reportName;

                //Specify the server credentials
                ReportViewer1.ServerReport.ReportServerCredentials = new CustomReportCredentials(userName, password, domain);

                ReportViewer1.ShowCredentialPrompts = false;
                ReportViewer1.ShowFindControls = false;
                ReportViewer1.ShowParameterPrompts = false;

                var reportParameters = new Dictionary<string, string>();

                GetQryStrParam("EmpresaId", ref reportParameters);
                GetQryStrParam("EstadoId", ref reportParameters);
                //TryIt("CiudadId", ref reportParameters);
                GetQryStrParam("ZonaId", ref reportParameters);
                GetQryStrParam("ConsolidadoFlg", ref reportParameters);
                GetQryStrParam("MesInicial", ref reportParameters);
                GetQryStrParam("MesFinal", ref reportParameters);
                GetQryStrParam("AnioInicial", ref reportParameters);
                GetQryStrParam("AnioFinal", ref reportParameters);
                GetQryStrParam("Notes", ref reportParameters);


                foreach (var item in reportParameters)
                {
                    ReportViewer1.ServerReport.SetParameters(new List<ReportParameter>() 
                    { 
                        new ReportParameter(item.Key, item.Value) 
                    });
                }
            }
        }

        public bool GetQryStrParam(string value, ref Dictionary<string, string> parameters)
        {
            if (Request.Url.AbsoluteUri.IndexOf(value + "=") >= 0 ? true : false)
            {
                parameters.Add(value, Request.QueryString[value].ToString());
                return true;
            }
            else
                return false;

            #region Old
            //if (Request.Url.AbsoluteUri.IndexOf("EmpresaId=") >= 0 ? true : false) reportParameters.Add("EmpresaId", Request.QueryString["EmpresaId"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("EstadoId=") >= 0 ? true : false) reportParameters.Add("EstadoId", Request.QueryString["EstadoId"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("CiudadId=") >= 0 ? true : false) reportParameters.Add("CiudadId", Request.QueryString["CiudadId"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("ZonaId=") >= 0 ? true : false) reportParameters.Add("ZonaId", Request.QueryString["ZonaId"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("ConsolidadoFlg=") >= 0 ? true : false) reportParameters.Add("ConsolidadoFlg", Request.QueryString["ConsolidadoFlg"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("FechaInicio=") >= 0 ? true : false) reportParameters.Add("FechaInicio", Request.QueryString["FechaInicio"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("FechaFinal=") >= 0 ? true : false) reportParameters.Add("FechaFinal", Request.QueryString["FechaFinal"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("MontoMenor=") >= 0 ? true : false) reportParameters.Add("MontoMenor", Request.QueryString["MontoMenor"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("MontoMayor=") >= 0 ? true : false) reportParameters.Add("MontoMayor", Request.QueryString["MontoMayor"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("strTipoIncidentesId=") >= 0 ? true : false) reportParameters.Add("strTipoIncidentesId", Request.QueryString["strTipoIncidentesId"].ToString());
            //if (Request.Url.AbsoluteUri.IndexOf("Notes=") >= 0 ? true : false) reportParameters.Add("Notes", Request.QueryString["Notes"].ToString());
            #endregion
        }

        public class CustomReportCredentials : IReportServerCredentials
        {
            private string _UserName;
            private string _PassWord;
            private string _DomainName;

            public CustomReportCredentials(string UserName, string PassWord, string DomainName)
            {
                _UserName = UserName;
                _PassWord = PassWord;
                _DomainName = DomainName;
            }

            public System.Security.Principal.WindowsIdentity ImpersonationUser
            {
                get
                {
                    return null;
                }
            }

            public System.Net.ICredentials NetworkCredentials
            {
                get
                {
                    return
                        new System.Net.NetworkCredential(_UserName, _PassWord, _DomainName);
                }
            }

            public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
            {
                authCookie = null;
                user = password = authority = null;
                return false;
            }
        }
    }
}