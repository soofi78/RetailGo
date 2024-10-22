using HashGo.Core.Enum;
using HashGo.Wpf.App.Views.Controls.KeyboardControl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HashGo.Wpf.App.Converters
{
    public class KeyboardTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var type = (KeyboardType)value;
            switch (type)
            {
                case KeyboardType.Alphabet: return new AlphabetView();
                case KeyboardType.Special: return new SpecialCharacterView();
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
