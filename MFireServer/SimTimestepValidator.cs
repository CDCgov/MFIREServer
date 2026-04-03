using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Navigation;

namespace MFireServer
{
    internal class SimTimestepValidator : ValidationRule
    {
        private ValidationResult _invalid = new ValidationResult(false, "Invalid Value");

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(value is string))
                return _invalid;

            var strValue = (string)value;
            int intVal;

            if (!int.TryParse(strValue, out intVal))
                return _invalid;            

            if (intVal < 1 || intVal > 10000)
                return _invalid;

            return ValidationResult.ValidResult;
        }
    }
}
