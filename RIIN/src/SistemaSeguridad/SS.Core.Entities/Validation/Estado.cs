using SS.Core.DataAnnotations.Extensions;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionEstado))]
    public partial class Estado
    {
        //int? PoligonoId { get; set; }
        public string[] Coords { get; set; }
    }

    public class ValidacionEstado
    {
        [Max(100)]
        [Requerido]
        [Remote("ExisteEstado", "Estado")]
        public string Nombre { get; set; }
    }
}