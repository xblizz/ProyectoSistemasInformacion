using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using SS.Core.DataAnnotations.Resources;

namespace SS.Core.DataAnnotations.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class MaxAttribute : DataTypeAttribute
    {
        private readonly double _max;

        public MaxAttribute(int maximo)
            : base("maximo")
        {
            _max = maximo;
        }

        public MaxAttribute(double maximo)
            : base("maximo")
        {
            _max = maximo;
        }

        public override string FormatErrorMessage(string name)
        {
            if(ErrorMessage==null && ErrorMessageResourceName==null)
            {
                ErrorMessage = Messages.MaxValidation;
            }
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, _max);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return value.ToString().Length <= _max;
        }
    }
}
