using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using SS.Core.DataAnnotations.Extensions;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionIncidente))]
    public partial class Incidente
    {
        public DateTime TmpFechaIncidente { get; set; }
        public DateTime TmpHoraIncidente { get; set; }
    }

    public class ValidacionIncidente
    {
        
        [Requerido]
        [Max(50)]
        public string Calle { get; set; }

        [Requerido]
        [Max(50)]
        public string Colonia { get; set; }

    }
}
