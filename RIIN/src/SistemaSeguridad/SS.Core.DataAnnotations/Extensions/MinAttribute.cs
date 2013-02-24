using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using SS.Core.DataAnnotations.Resources;

namespace SS.Core.DataAnnotations.Extensions
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class MinAttribute:DataTypeAttribute
    {
        private readonly double _minimo;

        public MinAttribute(int minimo)
            : base("minimo")
        {
            _minimo = minimo;
        }

        public MinAttribute(double minimo)
            : base("minimo")
        {
            _minimo = minimo;
        }

        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = Messages.MinValidation;
            }
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _minimo);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return value.ToString().Length >= _minimo;
        }
    }
}
