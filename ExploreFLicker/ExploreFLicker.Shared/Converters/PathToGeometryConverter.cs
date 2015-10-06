using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ExploreFlicker.Converters
{
    public class PathToGeometryConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert ( object value, Type targetType, object parameter, string language )
        {
            if(value == null) return Geometry.Empty;
            try
            {
                string xaml = "<Path " + "xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'>" +
            "<Path.Data>" + value + "</Path.Data></Path>";
                var path = (Path)XamlReader.Load(xaml);
                Geometry geometry = path.Data;
                path.Data = null;
                return geometry;
            }
            catch(Exception)
            {
                return Geometry.Empty;
            }

        }

        public object ConvertBack ( object value, Type targetType, object parameter, string language )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
