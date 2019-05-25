using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace IQF.TradeAPI.TestTool.Converter
{
    [SuppressMessage("csharpsquid", "S1172:Unused method parameters should be removed")]
    public class LoginStatusToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((LohinStatus)value== LohinStatus.Login)
            {
                return "已登陆";
            }
            else
            {
                return "未登陆";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
