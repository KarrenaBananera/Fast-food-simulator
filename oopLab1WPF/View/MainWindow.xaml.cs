using System;
using System.Windows;

namespace oopLab1WPF
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			//var fastFood = new FastFood(new MockCustomer());

			var viewModel = new FastFoodViewModel();

			DataContext = viewModel;
			Title = "Fast Food";
		}
	}
}