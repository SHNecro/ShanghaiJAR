using Common.OpenAL;
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace MapEditor.Core.Converters
{
    public class OggProgressBarConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
            {
                return Binding.DoNothing;
            }

            var progress = values[0] as OggPlaybackEventArgs;

            if (progress == null || !IsNumber(values[1]) || !IsNumber(values[2]))
            {
                return Binding.DoNothing;
            }

            var progressStar = ToStar(progress.ProgressSamples);
            var remainingStar = ToStar(progress.TotalSamples - progress.ProgressSamples);

            var loopStart = System.Convert.ToInt64(values[1]);
            var loopEnd = System.Convert.ToInt64(values[2]);

            loopStart = Math.Min(progress.TotalSamples, Math.Max(0, loopStart));
            loopEnd = Math.Min(progress.TotalSamples, Math.Max(0, loopEnd));

            var leftLoopMarginStar = ToStar(loopStart);
            var loopStar = ToStar(loopEnd - loopStart);
            var rightLoopMarginStar = ToStar(progress.TotalSamples - loopEnd);

            return new
            {
                ProgressStar = progressStar,
                RemainingStar = remainingStar,

                LeftLoopMarginStar = leftLoopMarginStar,
                LoopStar = loopStar,
                RightLoopMarginStar = rightLoopMarginStar
            };
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return Enumerable.Repeat(Binding.DoNothing, targetTypes.Length).ToArray();
        }

        private static string ToStar(long size)
        {
            return $"{Math.Max(0, size)}*";
        }

        private static bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }
}
