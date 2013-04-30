using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using WPControls;

namespace SpaceTrek.Helper
{
    public class IdToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string id = value.ToString();

            return String.Format(EndpointHelper.USER_PHOTO, id);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
           // throw new NotImplementedException();
            return null;
        }
    }

    public class BackgroundConverter : IDateToBrushConverter
    {

        public Brush Convert(DateTime dateTime, bool isSelected, Brush defaultValue)
        {
            if (dateTime == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 5))
            {
                return new SolidColorBrush(Colors.Yellow );
            }
            else
            {
                return defaultValue;
            }
        }

        public Brush Convert(DateTime dateTime, bool isSelected, Brush defaultValue, BrushType brushType)
        {
            throw new NotImplementedException();
        }
    }
}
