using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;

using SS.Core.DataAnnotations.Resources;

namespace SS.Core.DataAnnotations.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class FileExtensionsAttribute : DataTypeAttribute
    {
        public string Extensions { get; private set; }

        /// <summary>
        /// Provide the allowed file extensions, seperated via "|" (or a comma, ","), defaults to "png|jpe?g|gif" 
        /// </summary>
        public FileExtensionsAttribute(string allowedExtensions = "png,jpg,jpeg,gif")
            : base("fileextension")
        {
            Extensions = string.IsNullOrWhiteSpace(allowedExtensions) ? "png,jpg,jpeg,gif" : allowedExtensions.Replace("|", ",").Replace(" ", "");
        }

        public override string FormatErrorMessage(string name)
        {
            if (ErrorMessage == null && ErrorMessageResourceName == null)
            {
                ErrorMessage = Messages.EmailValidation;
            }

            return String.Format(CultureInfo.CurrentCulture, ErrorMessageString, name, Extensions);
        }

        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            var valueAsString = value as string;
            return valueAsString != null && ValidateExtension(valueAsString);
        }

        private bool ValidateExtension(string fileName)
        {
            try
            {
                var extension = Path.GetExtension(fileName);
                return extension != null && Extensions.Split(',').Contains(extension.Replace(".", "").ToLowerInvariant());
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
    }
}
