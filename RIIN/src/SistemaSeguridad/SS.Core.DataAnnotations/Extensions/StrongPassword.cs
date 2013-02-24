using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using SS.Core.DataAnnotations.Resources;

namespace SS.Core.DataAnnotations.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StrongPassword : DataTypeAttribute
    {
   

        private static Regex _regex =
            new Regex(
                @"
                        ^                   # inicio
                       (?=.*\d)             # Debe de contener un digito
                       (?=.*[[A-Za-z])      # Debe de contener un caracter alfabetico
                       (?=.*[^\w\d\s])      # Debe de incluir un caracter especial
                       (?=.*[A-Za-z0-9])    # Acepta letras, numeros y caracteres especiales.
                       .{6,}              # La longitud debe ser al menos de 6 caracteres y maximo 15
                       $                    # fin",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);

        public StrongPassword()
            : base(DataType.Text)
        {
        }
        public StrongPassword(DataType dataType) : base(dataType)
        {
        }

        public StrongPassword(string customDataType) : base(customDataType)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = Messages.StrongPassword;
            }
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var valueAsString = value as string;
            return valueAsString != null && _regex.Match(valueAsString).Length > 0;
            
        }
    }
}
