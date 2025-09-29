using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace oopLab1WPF;

public class RandomCustomerArrival : ICustomerArrival
{
	public event ICustomerArrival.CustomerArrivedEventHandler? CustomerArrived;

	private bool _started = false;
	int _delay;
	int _delta;
	int _cookTime;
	private System.Timers.Timer _timer = new ();
	private Random _random = new Random();
	public RandomCustomerArrival(int delay, int delta, int cookTime)
	{
		if (delay <= 0 || delta > delay)
			throw new ArgumentException();
		_delay = delay;
		_delta = delta;
		_cookTime = cookTime;
	}
	public void Start()
	{
		if (_started == false)
			RunTimer();
		_started = true;
	}

	private void RunTimer()
	{
		_timer.Interval = _delay + _random.Next(-_delta / 2, _delta / 2);
		_timer.AutoReset = true;
		_timer.Elapsed +=
			(sender, args) => GenerateCustomer();

		_timer.Start();
	}

	private void GenerateCustomer()
	{
		var custmoer = new Customer(new Order(_cookTime));
		Console.WriteLine("Создан посетитель: " + custmoer.Name + "Поток ");
		CustomerArrived?.Invoke(custmoer);
	}
}
