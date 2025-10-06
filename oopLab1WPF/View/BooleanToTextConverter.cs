using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace oopLab1WPF;

public class BooleanToTextConverter : IValueConverter
{
	public string TrueText { get; set; } = "Работает";
	public string FalseText { get; set; } = "Свободен";

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue && boolValue == true)
			return TrueText;
		return FalseText;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}