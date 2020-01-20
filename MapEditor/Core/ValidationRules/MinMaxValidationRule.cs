using System.Globalization;
using System.Windows.Controls;

namespace MapEditor.Core.ValidationRules
{
    public class MinMaxValidationRule : ValidationRule
    {
        public int? Minimum { get; set; }

        public int? Maximum { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int intValue))
            {
                if (this.Minimum.HasValue && this.Maximum.HasValue && intValue < this.Minimum || intValue > this.Maximum)
                {
                    return new ValidationResult(false, $"Value must be equal to or between {this.Minimum} and {this.Maximum}.");
                }
                else if (!this.Minimum.HasValue && intValue > this.Maximum)
                {
                    return new ValidationResult(false, $"Value must be equal to or below {this.Maximum}.");
                }
                else if (!this.Maximum.HasValue && intValue < this.Minimum)
                {
                    return new ValidationResult(false, $"Value must be equal to or above {this.Minimum}.");
                }
                else
                {
                    return new ValidationResult(true, null);
                }
            }

            return new ValidationResult(false, "Value must be an integer.");
        }
    }
}
