using System;
using System.Globalization;
using System.Windows.Controls;

namespace MapEditor.Core.ValidationRules
{
    public class MinMaxBindingValidationRule : ValidationRule
    {
        public MinMaxBinding Bounds { get; set; }

        private int? Minimum => this.Bounds.Minimum;
        private int? Maximum => this.Bounds.Maximum;

        private bool MinInclusive => this.Bounds.MinInclusive;
        private bool MaxInclusive => this.Bounds.MaxInclusive;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value?.ToString(), out int intValue))
            {
                Func<int, int, bool> minCheck = (val, min) => this.MinInclusive ? (val < min) : (val <= min);
                Func<int, int, bool> maxCheck = (val, max) => this.MaxInclusive ? (val > max) : (val >= max);
                var minFailString = this.MinInclusive ? "greater than" : "greater than or equal to";
                var maxFailString = this.MaxInclusive ? "less than" : "less than or equal to";

                if (this.Minimum.HasValue && this.Maximum.HasValue && (minCheck(intValue, this.Minimum.Value) || maxCheck(intValue, this.Maximum.Value)))
                {
                    return new ValidationResult(false, $"Value must be {minFailString} {this.Minimum} and {maxFailString} {this.Maximum}.");
                }
                else if (!this.Minimum.HasValue && maxCheck(intValue, this.Maximum.Value))
                {
                    return new ValidationResult(false, $"Value must be {maxFailString} {this.Maximum}.");
                }
                else if (!this.Maximum.HasValue && minCheck(intValue, this.Minimum.Value))
                {
                    return new ValidationResult(false, $"Value must be {minFailString} {this.Minimum}.");
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
