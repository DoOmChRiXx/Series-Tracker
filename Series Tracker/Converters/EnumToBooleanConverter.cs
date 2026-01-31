using System;
using System.Globalization;
using System.Windows.Data;

namespace Series_Tracker.Converters
{
    public class EnumToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return false;

            return value.ToString().Equals(parameter.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null)
                return Binding.DoNothing;

            if (!(bool)value)
                return Binding.DoNothing;

            // If targetType is nullable, get the underlying type
            var enumType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            return Enum.Parse(enumType, parameter.ToString());

        }
    }
}