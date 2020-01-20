using System.Globalization;
using System.Windows.Controls;

namespace MapEditor.Core.ValidationRules
{
    public class FloorHeightValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int intValue))
            {
                if (intValue < 0)
                {
                    return new ValidationResult(false, "Value must be greater than 0.");
                }
                else if (intValue % 16 != 0)
                {
                    return new ValidationResult(false, "Value must be a multiple of 16 (1 walkable tile distance).");
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
