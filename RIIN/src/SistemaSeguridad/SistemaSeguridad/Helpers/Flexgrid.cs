using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemaSeguridad.Helpers
{
    public class Flexgrid
    {
        public int page { get; set; }
        public int total { get; set; }
        public List<GridRow> rows = new List<GridRow>();
 

    }
    public class GridRow
    {
        public int id { get; set; }
        public List<string> cell { get; set; }

    }
}