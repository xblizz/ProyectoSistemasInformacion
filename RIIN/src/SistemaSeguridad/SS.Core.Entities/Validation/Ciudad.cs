using SS.Core.DataAnnotations.Extensions;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionCiudad))]
    public partial class Ciudad
    {
        public string[] Coords { get; set; }
    }

    public class ValidacionCiudad
    {
        [Max(100)]
        [Requerido]
        [Remote("ExisteCiudad", "Ciudad")]
        public string Nombre { get; set; }
    }
}
