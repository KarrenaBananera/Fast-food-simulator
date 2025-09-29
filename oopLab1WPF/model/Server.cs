
using System.Windows.Threading;
using System.Xml.Linq;

namespace oopLab1WPF;

public class Server : IWorker
{
	string Name = Guid.NewGuid().ToString().Substring(30);
	private System.Timers.Timer _timer = new();
	public event EventHandler<TicketEventArgs>? OnFinishWork;

	private int _processSpeed { get; }
	public bool IsWorking { get => _timer.Enabled; }

	public Server(int processSpeed)
	{
		_processSpeed = processSpeed;
	}

	public bool TryTakeTicket(Ticket ticket)
	{
		if (_timer.Enabled == true)
			return false;

		Console.WriteLine("Взят тикет оффицантом: " + ticket.customer.Name + "Оффицант " + Name);

		Ticket tic = ticket;
		_timer = new();
		_timer.Interval = _processSpeed;
		_timer.AutoReset = false;
		_timer.Elapsed += (sender, args) =>
		{
			_timer.Stop();
			OnFinishWork?.Invoke(this, new TicketEventArgs(tic));
		};

		_timer.Start();

		return true;
	}
}
		