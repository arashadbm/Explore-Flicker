using System;
using System.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace ExploreFlicker.Converters
{
    public class VisibleNotEmptyConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, string language )
        {
            if (value == null) return Visibility.Collapsed;
            //special handling for Ilist
            if (( value as IList ) != null) return ( value as IList ).Count == 0 ? Visibility.Collapsed : Visibility.Visible;
            return value.ToString().Length == 0 ? Visibility.Collapsed : Visibility.Visible;

        }

        public object ConvertBack ( object value, Type targetType, object parameter, string language )
        {
            throw new NotImplementedException("");
        }
    }
}
