using MapEditor.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace MapEditor.Core.ValidationRules
{
    public class EnemyOverlapValidationRule : ValidationRule
    {
        public BoundObject Enemies { get; set; }

        public BoundObject CurrentPosition { get; set; }

        public bool IsX { get; set; }

        private Point CurrentPositionValue => (Point)this.CurrentPosition.Binding;

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (int.TryParse(value.ToString(), out int intValue))
            {
                if ((this.IsX && intValue == CurrentPositionValue.X) || (!this.IsX && intValue == CurrentPositionValue.Y))
                {
                    return new ValidationResult(true, null);
                }
                var checkedPosition = this.IsX ? new Point(intValue, this.CurrentPositionValue.Y) : new Point(CurrentPositionValue.X, intValue);
                var enemiesInSpace = ((IEnumerable<Enemy>)this.Enemies.Binding).Where(e => e.X == checkedPosition.X && e.Y == checkedPosition.Y);
                return enemiesInSpace.Any()
                    ? new ValidationResult(false, "Another enemy is in that space.")
                    : new ValidationResult(true, null);
            }

            return new ValidationResult(false, "Value must be an integer.");
        }
    }
}
