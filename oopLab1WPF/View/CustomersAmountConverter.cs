using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace oopLab1WPF;

public class CustomersAmountConverter : IValueConverter
{

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int customers)
		{
			string ret = customers.ToString();

			if (customers > 10)
				ret += " большая очередь";

			return ret;
		}

		return "Ошибка";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}