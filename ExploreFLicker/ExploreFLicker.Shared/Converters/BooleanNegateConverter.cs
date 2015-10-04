using System;
using Windows.UI.Xaml.Data;

namespace ExploreFlicker.Converters
{
    public class BooleanNegateConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, string language )
        {
            return !( value is bool && (bool)value );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, string language )
        {
            throw new NotImplementedException();
        }
    }
}
