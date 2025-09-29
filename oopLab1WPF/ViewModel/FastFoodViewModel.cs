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

	public ObservableCollection<WorkerViewModel> KitchenWorkers { get; } = new();
	public ObservableCollection<WorkerViewModel> ServerWorkers { get; } = new();
	public ObservableCollection<WorkerViewModel> TakerWorkers { get; } = new();

	private IEnumerable<IWorker>? _oldCollectionKitchen = null;
	private IEnumerable<IWorker>? _oldCollectionServers = null;
	private IEnumerable<IWorker>? _oldCollectionTakers = null;

	private int _customers;
	private string _customersDelay = "0.5";
	private string _workersWorkTime = "1,5";

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


	public FastFoodViewModel()
	{

		StartCommand = new Command(Start);

		_refreshTimer = new DispatcherTimer();
		_refreshTimer.Interval = TimeSpan.FromMilliseconds(500);
		_refreshTimer.Tick += (s, e) => Refresh();
		_refreshTimer.Start();

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
			customersArrivalTime/3, 
			customersArrivalTime *5));
		_fastFood.Start();
		Refresh();
	}

	private void AddCook()
	{
		GetValueInt(_workersWorkTime);

		if (_fastFood is not null)
			_fastFood.AddCooks(new Cook());
	}

	private void AddTaker()
	{

		var value = GetValueInt(_workersWorkTime);

		if (value != -1 && _fastFood is not null)
			_fastFood.AddTakers(new Taker(value));
	}

	private void AddServer()
	{
		var value = GetValueInt(_workersWorkTime);

		if (value != -1 && _fastFood is not null)
			_fastFood.AddServers(new Server(value));
	}

	private int GetValueInt(string value)
	{
		double result = 0;
		if (!double.TryParse(value, CultureInfo.InvariantCulture, out result) || result <= 0)
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
		RefreshWorkerCollection(_fastFood.KitchenWorkers,
			_oldCollectionKitchen, KitchenWorkers);
		RefreshWorkerCollection(_fastFood.ServerWorkers,
			_oldCollectionServers, ServerWorkers);
		RefreshWorkerCollection(_fastFood.TakerWorkers,
			_oldCollectionTakers, TakerWorkers);
	}

	private void RefreshWorkerCollection(IEnumerable<IWorker> currentWorkers, IEnumerable<IWorker>? oldCollection,
		ObservableCollection<WorkerViewModel> targetCollection)
	{
		targetCollection.Clear();
		foreach (var worker in currentWorkers)
		{
			targetCollection.Add(new WorkerViewModel(worker));
		}

		oldCollection = currentWorkers;
	}

	private void RefreshCustomers()
	{
		if (Customers != _fastFood.Customers)
			Customers =	_fastFood.Customers;
	}
}