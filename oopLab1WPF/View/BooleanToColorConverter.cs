using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace oopLab1WPF;

public class BooleanToColorConverter: IValueConverter
{
	public Color TrueColor { get; set; } = Colors.Green;
	public Color FalseColor { get; set; } = Colors.Red;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is bool boolValue && boolValue)
			return new SolidColorBrush(TrueColor);
		return new SolidColorBrush(FalseColor);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}