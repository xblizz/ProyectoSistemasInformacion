using System.Web.Mvc;

namespace SistemaSeguridad.Helpers
{
    public static class UrlHelperExtension
    {
        public static string Image(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/Content/images/{0}", fileName));
        }

        public static string JavaScript(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/Scripts/{0}", fileName));
        }
        public static string JavaScript(this UrlHelper helper, Plugin type, string fileName)
        {
            var path = string.Empty;
            switch (type)
            {
                case Plugin.Multiselect:
                    path = string.Format("~/Scripts/Plugins/Multiselect/{0}", fileName);
                    break;
                case Plugin.DataPicker:
                    path = string.Format("~/Scripts/Plugins/DatePicker/{0}", fileName);
                    break;
                case Plugin.FlexGrid:
                    path = string.Format("~/Scripts/Plugins/Flexigrid/js/{0}", fileName);
                    break;
                case Plugin.Visualize:
                    path = string.Format("~/Content/js/{0}", fileName);
                    break;
                case Plugin.AnimatedDIV:
                    path = string.Format("~/Scripts/Plugins/AnimatedDIV/{0}", fileName);
                    break;
                case Plugin.TimePicker:
                    path = string.Format("~/Scripts/Plugins/TimePicker/{0}", fileName);
                    break;
                default:
                    path = string.Format("~/Scripts/Plugins/jCombo/{0}", fileName);
                    break;
            }
            return helper.Content(path);
        }

        public static string Stylesheet(this UrlHelper helper, string fileName)
        {
            return helper.Content(string.Format("~/Content/css/{0}", fileName));
        }
        public static string Stylesheet(this UrlHelper helper, Plugin type, string fileName)
        {
            var path = string.Empty;
            switch (type)
            {
                case Plugin.Multiselect:
                    path = string.Format("~/Scripts/Plugins/Multiselect/css/{0}", fileName);
                    break;
                case Plugin.FlexGrid:
                    path = string.Format("~/Scripts/Plugins/Flexigrid/css/{0}", fileName);
                    break;
                case Plugin.DataPicker:
                    return helper.Content(fileName);
                case Plugin.TimePicker:
                    path = string.Format("~/Scripts/Plugins/TimePicker/{0}", fileName);
                    break;
                default:
                    return helper.Content(fileName);
            }
            return helper.Content(path);
        }
    }

    public enum Plugin
    {
        Multiselect = 0,
        DataPicker = 1,
        JCombo = 2,
        FlexGrid = 3,
        Visualize = 4,
        AnimatedDIV = 5,
        TimePicker = 6
    }
}