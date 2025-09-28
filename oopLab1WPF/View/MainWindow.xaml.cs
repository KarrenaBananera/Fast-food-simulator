using System;
using System.Windows;

namespace oopLab1WPF
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			var fastFood = new FastFood(new RandomCustomerArrival(5000,10,500));
			//var fastFood = new FastFood(new MockCustomer());


			for (int i = 0; i < 10; i++)
			{
				fastFood.AddCooks(new Cook());
				fastFood.AddTakers(new Taker(500));
				fastFood.AddServers(new Server(500));

			}
			var viewModel = new FastFoodViewModel(fastFood);

			DataContext = viewModel;
			Title = "Fast Food";
		}
	}
}