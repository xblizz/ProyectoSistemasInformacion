using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using SS.Core.DataAnnotations.Resources;

namespace SS.Core.DataAnnotations.Extensions
{
    [AttributeUsage(AttributeTargets.Property,AllowMultiple = false)]
    public class RequeridoAttribute : DataTypeAttribute
    {
       
        public RequeridoAttribute()
            : base("val")
        {
        }
        
        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = Messages.RequieredValidation;
            }
            return string.Format(CultureInfo.CurrentCulture, ErrorMessageString, name);
        }

        public override bool IsValid(object value)
        {
            return value != null;
        }
    }
}
