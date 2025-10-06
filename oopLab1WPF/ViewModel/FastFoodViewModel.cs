using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace oopLab1WPF;

public class FastFoodViewModel : INotifyPropertyChanged
{
	private FastFood? _fastFood;
	private DispatcherTimer _refreshTimer;
	public event PropertyChangedEventHandler? PropertyChanged;

	public ObservableCollection<Cook> KitchenWorkers { get; } = new();
	public ObservableCollection<Server> ServerWorkers { get; } = new();
	public ObservableCollection<Taker> TakerWorkers { get; } = new();


	private int _customers;
	private int _customersWaitingTaker;
	private int _customersWaitingCook;
	private int _customersWaitingServer;


	private string _customersDelay = "0.5";
	private string _workersWorkTime = "1.5";

	public ICommand StartCommand { get; }
	public ICommand AddCookCommand { get; }
	public ICommand AddTakerCommand { get; }
	public ICommand AddServerCommand { get; }

	public string CustomersDelay
	{
		get => _customersDelay;
		set
		{
			_customersDelay = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomersDelay)));
		}
	}

	public string WorkersWorkTime
	{
		get => _workersWorkTime;
		set
		{
			_workersWorkTime = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WorkersWorkTime)));
		}
	}
	public int Customers
	{
		get => _customers;
		set
		{
			_customers = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Customers)));
		}
	}

	public int CustomersWaitingTaker
	{
		get => _customersWaitingTaker;
		set
		{
			_customersWaitingTaker = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomersWaitingTaker)));
		}
	}

	public int CustomersWaitingCook
	{
		get => _customersWaitingCook;
		set
		{
			_customersWaitingCook = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomersWaitingCook)));
		}
	}

	public int CustomersWaitingServer
	{
		get => _customersWaitingServer;
		set
		{
			_customersWaitingServer = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CustomersWaitingServer)));
		}
	}


	public FastFoodViewModel()
	{

		StartCommand = new Command(Start);
		AddCookCommand = new Command(AddCook);
		AddServerCommand = new Command(AddServer);
		AddTakerCommand = new Command(AddTaker);

		_refreshTimer = new DispatcherTimer();
		_refreshTimer.Interval = TimeSpan.FromMilliseconds(500);
		_refreshTimer.Tick += (s, e) => Refresh();

		Refresh();
	}

	private void Start()
	{
		if (_fastFood is not null)
			return;

		var customersArrivalTime = GetValueInt(_customersDelay);

		if (customersArrivalTime == -1)
			return;

		_fastFood = new FastFood(new RandomCustomerArrival(customersArrivalTime, 
			customersArrivalTime/3));
		_fastFood.AddServers(ServerWorkers);
		_fastFood.AddTakers(TakerWorkers);
		_fastFood.AddCooks(KitchenWorkers);
		_fastFood.Start();

		Refresh();
		_refreshTimer.Start();
	}

	private void AddCook()
	{
		var value = GetValueInt(_workersWorkTime);

		if (value == -1)
			return;

		var newTaker = new Cook(value);
		if (_fastFood is not null)
			_fastFood.AddCooks(newTaker);
		else
			KitchenWorkers.Add(newTaker);
	}

	private void AddTaker()
	{

		var value = GetValueInt(_workersWorkTime);

		if (value == -1)
			return;
			
		var newTaker = new Taker(value);
		if (_fastFood is not null)
			_fastFood.AddTakers(newTaker);
		else
			TakerWorkers.Add(newTaker);
	}

	private void AddServer()
	{
		var value = GetValueInt(_workersWorkTime);

		if (value == -1)
			return;

		var newServer = new Server(value);
		if (_fastFood is not null)
			_fastFood.AddServers(newServer);
		else
			ServerWorkers.Add(newServer);
	}

	private int GetValueInt(string value)
	{
		float result = 0;
		if (!float.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out result) || (int)(result * 1000) <= 0)
		{
			MessageBox.Show($"Неверное время: {value}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
			return -1;
		}

		return (int)(result * 1000);
	}
	private void Refresh()
	{
		if (_fastFood is null)
			return;
		RefreshCustomers();
		RefreshCollections();
	}


	private void RefreshCollections()
	{
		KitchenWorkers.Clear();
		TakerWorkers.Clear();
		ServerWorkers.Clear();

		foreach (var cook in _fastFood.KitchenWorkers)
			KitchenWorkers.Add(cook);

		foreach (var taker in _fastFood.TakerWorkers)
			TakerWorkers.Add(taker);

		foreach (var server in _fastFood.ServerWorkers)
			ServerWorkers.Add(server);
		
	}

	private void RefreshCustomers()
	{
		Customers =	_fastFood.Customers;
		CustomersWaitingCook = _fastFood.CustomersWaitingKitchen;
		CustomersWaitingServer = _fastFood.CustomersWaitingServer;
		CustomersWaitingTaker = _fastFood.CustomersWaitingTaker;

	}
}