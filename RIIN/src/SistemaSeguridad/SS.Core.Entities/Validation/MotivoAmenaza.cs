using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using SS.Core.DataAnnotations.Extensions;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionMotivoAmenaza))]
    public partial class MotivoAmenaza
    {
    }

    public class ValidacionMotivoAmenaza
    {
        [Max(50)]
        [Requerido]
        public string Nombre { get; set; }
    }
}
