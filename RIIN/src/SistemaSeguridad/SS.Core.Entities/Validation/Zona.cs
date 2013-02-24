using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using SS.Core.DataAnnotations.Extensions;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionZona))]
    public partial class Zona
    {
        public string[] Coords { get; set; }
    }

    public class ValidacionZona
    {
        [Max(100)]
        [Requerido]
        [Remote("ExistZona", "Zona")]
        public string Nombre { get; set; }

    }
}
