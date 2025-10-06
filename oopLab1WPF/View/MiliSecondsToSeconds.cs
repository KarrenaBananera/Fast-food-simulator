using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace oopLab1WPF;

public class MiliSecondsToSeconds : IValueConverter
{
	public Color TrueColor { get; set; } = Colors.Green;
	public Color FalseColor { get; set; } = Colors.Red;

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int milliseconds)
		{
			double seconds = milliseconds / 1000.0;
			return $"{seconds:F1} сек"; 
		}

		return "Ошибка";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}