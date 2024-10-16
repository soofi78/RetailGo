using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace HashGo.Wpf.App.Common
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            //if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            //    return new ValidationResult(false, "Email is required.");

            if (Regex.IsMatch(value.ToString(), emailPattern))
            {
                return ValidationResult.ValidResult;
            }

            return new ValidationResult(false, "Invalid email format.");
        }
    }
}
