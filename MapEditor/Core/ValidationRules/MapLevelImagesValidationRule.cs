using System.Globalization;
using System.Windows.Controls;

namespace MapEditor.Core.ValidationRules
{
    public class MapLevelImagesValidationRule : ValidationRule
    {
        public BoundObject ImagePrefix { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!int.TryParse(value.ToString(), out int levels))
            {
                return new ValidationResult(false, "Value must be an integer.");
            }
            for (int i = 1; i < levels * 2; i++)
            {
                var fileName = $"{(string)this.ImagePrefix.Binding}{i}";
                if (!Constants.TextureLoadStrategy.CanProvideFile(fileName))
                {
                    return new ValidationResult(false, $"Map image\"{fileName}\" could not be found.");
                }
            }

            return new ValidationResult(true, null);
        }
    }
}
