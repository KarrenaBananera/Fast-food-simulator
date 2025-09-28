using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace oopLab1WPF;

public class FastFoodViewModel : INotifyPropertyChanged
{
	private readonly FastFood _fastFood;
	private readonly DispatcherTimer _refreshTimer;
	public event PropertyChangedEventHandler? PropertyChanged;

	public ObservableCollection<WorkerViewModel> KitchenWorkers { get; } = new();
	public ObservableCollection<WorkerViewModel> ServerWorkers { get; } = new();
	public ObservableCollection<WorkerViewModel> TakerWorkers { get; } = new();

	private IEnumerable<IWorker>? _oldCollectionKitchen = null;
	private IEnumerable<IWorker>? _oldCollectionServers = null;
	private IEnumerable<IWorker>? _oldCollectionTakers = null;

	private Dictionary<IWorker, bool> _oldWorking = new();
	private int _customers;
	public int Customers
	{
		get => _customers;
		set
		{
			_customers = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Customers)));
		}
	}

	public ICommand StartCommand { get; }

	public FastFoodViewModel(FastFood fastFood)
	{
		_fastFood = fastFood;

		StartCommand = new Command(Start);

		_refreshTimer = new DispatcherTimer();
		_refreshTimer.Interval = TimeSpan.FromMilliseconds(500);
		_refreshTimer.Tick += (s, e) => Refresh();
		_refreshTimer.Start();

		Refresh();
	}

	private void Start()
	{
		_fastFood.Start();
		Refresh();
	}

	private void Refresh()
	{
		RefreshCustomers();
		RefreshCollections();
		//RefreshWorking();
	}

	//private void RefreshWorking()
	//{
	//	var allWorkers = _oldCollectionKitchen.Concat(_oldCollectionTakers).Concat(_oldCollectionServers);

	//	foreach(var worker in allWorkers)
	//	{
	//		if (_oldWorking.TryGetValue(worker, out bool oldWorking))
	//		{
	//			if (worker.IsWorking != oldWorking) 
	//			{
	//				_oldWorking[worker] = worker.IsWorking;
	//				PropertyChanged?.Invoke(worker, new PropertyChangedEventArgs(nameof()));
	//			}
	//		}
	//	}
	//}

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