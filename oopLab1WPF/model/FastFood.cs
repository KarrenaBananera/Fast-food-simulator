
using System.Collections.Specialized;
using System.ComponentModel;

namespace oopLab1WPF;

public class FastFood
{
	public int Customers { get; protected set; } = 0;
	public int CustomersWaitingTaker { get; protected set; } = 0;
	public int CustomersWaitingKitchen { get; protected set; } = 0;
	public int CustomersWaitingServer { get; protected set; } = 0;

	protected Facility<Cook> _kitchen = new();
	protected Facility<Server> _servers = new();
	protected Facility<Taker> _takers = new();

	protected ICustomerArrival _customerArrival;



	public IReadOnlyList<Cook> KitchenWorkers => _kitchen.Workers;
	public IReadOnlyList<Server> ServerWorkers => _servers.Workers;
	public IReadOnlyList<Taker> TakerWorkers => _takers.Workers;
		
	public FastFood(ICustomerArrival customers)
	{
		_customerArrival = customers;
		customers.CustomerArrived += OnCustomerArrival;

		_takers.FinishedWork += OnTakingOrder;
		_kitchen.FinishedWork += OnCookingOrder;
		_servers.FinishedWork += OnServingOrder;
	}
	
	public void AddTakers(params IEnumerable<Taker> takers)
	{
		_takers.AddWorkers(takers);
	}

	public void AddCooks(params IEnumerable<Cook> cooks)
	{
		_kitchen.AddWorkers(cooks);
	}
	public void AddServers(params IEnumerable<Server> servers)
	{
		_servers.AddWorkers(servers);
	}
	private void OnTakingOrder(Ticket ticket, Taker? usedTaker)
	{
		lock (_customerArrival)
		{
			CustomersWaitingTaker--;
			CustomersWaitingKitchen++;

			Console.WriteLine("Тикет передан на кухню: " + ticket.customer.Name);
			_kitchen.AddTicket(ticket);
		}
	}

	private void OnCookingOrder(Ticket ticket, Cook? usedCook)
	{
		lock (_customerArrival)
		{
			CustomersWaitingKitchen--;
			CustomersWaitingServer++;
			Console.WriteLine("Тикет передан офицантскому корпусу: " + ticket.customer.Name);

			_servers.AddTicket(ticket);
		}
	}

	private void OnServingOrder(Ticket ticket, Server? usedServer)
	{
		lock (_customerArrival)
		{
			Console.WriteLine("Тикет передан посетителю!!!: " + ticket.customer.Name);
			Customers--;
			CustomersWaitingServer--;
		}
	}

	public void Start()
	{
		_customerArrival.Start();
	}

	private void OnCustomerArrival(Customer arrivedCustomer)
	{
		lock (_customerArrival)
		{
			Ticket ticket = new Ticket(arrivedCustomer,
				arrivedCustomer.Order, Guid.NewGuid().ToString());

			Customers++;
			CustomersWaitingTaker++;
			Console.WriteLine("Появился кастомер, передан кассирам: " + arrivedCustomer.Name);
			_takers.AddTicket(ticket);
		}
	}

}
