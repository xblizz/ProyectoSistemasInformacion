using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SS.Core.DataAnnotations.Extensions;
using System.ComponentModel.DataAnnotations;

namespace SS.Core.Entities
{
    [MetadataType(typeof(ValidacionEmpresa))]
    public partial class Empresa
    {
        /// <summary>
        /// This property is a wrapper for field Id
        /// Its used in DropDowns DON'T DELETE IT
        /// </summary>
        public int? EmpresaId
        {
            //Is nullable to allow saving when is group
            get { return this.Id; }
            set { this.Id = value ?? 0; }
        }
    }

    public class ValidacionEmpresa
    {
        [Max(12)]
        [Requerido]
        public string Nombre { get; set; }

        //[Min(6)]
        //[StrongPassword]
        //[Requerido]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        //[Requerido]
        //[Max(50)]
        //public string Nombre { get; set; }

        //[Max(50)]
        //[Requerido]
        //public string ApellidoPaterno { get; set; }

        //[Max(50)]
        //public string ApellidoMaterno { get; set; }

        //[Max(120)]
        //[Requerido]
        //[Email]
        //public string Email { get; set; }
    }
}
