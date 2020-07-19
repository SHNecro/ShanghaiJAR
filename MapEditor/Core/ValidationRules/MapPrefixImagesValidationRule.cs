using System.Globalization;
using System.Windows.Controls;

namespace MapEditor.Core.ValidationRules
{
    public class MapPrefixImagesValidationRule : ValidationRule
    {
        public BoundObject Levels { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!(this.Levels.Binding is int))
            {
                return new ValidationResult(false, $"Levels set to invalid value.");
            }

            var levels = (int)this.Levels.Binding;
            for (int i = 1; i < levels * 2; i++)
            {
                var fileName = $"{value.ToString()}{i}";
                if (!Constants.TextureLoadStrategy.CanProvideFile(fileName))
                {
                    return new ValidationResult(false, $"Map image\"{fileName}\" could not be found.");
                }
            }

            return new ValidationResult(true, null);
        }
    }
}
