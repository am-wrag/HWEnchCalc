using System;
using System.Globalization;
using System.Windows.Data;

namespace HWEnchCalc.Common
{
    public class BigNumberConvertor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (value == null) return new object();

            var val = (double) value;

            double result = 0;
            if (val > 0)
            {
                result = Math.Round((double)value, 2);
            }

            //Каждая решетка обозначает наличие числового символа, если он имеется
            return result.ToString("### ### ###.#", culture).Trim();
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (value == null) return new object();

            var val = (string) value;

            var trimmedData = val.Trim();

            return double.Parse(trimmedData);
        }
    }
}