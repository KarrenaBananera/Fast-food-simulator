
using System.Windows.Threading;

namespace oopLab1WPF;

public class Taker : IWorker
{
	string Name = Guid.NewGuid().ToString().Substring(30);
	private System.Timers.Timer _timer = new();
	public event EventHandler<TicketEventArgs>? OnFinishWork;

	private int _processSpeed { get; }
	public bool IsWorking { get => _timer.Enabled; }

	public Taker(int processSpeed)
	{
		_processSpeed = processSpeed;
	}

	public bool TryTakeTicket(Ticket ticket)
	{
		if (_timer.Enabled == true) 
			return false;

		Console.WriteLine("Взят тикет кассиром: " + ticket.customer.Name + "Кассир " + Name);
		Ticket tic = ticket;
		_timer = new();
		_timer.Interval = _processSpeed;
		_timer.AutoReset = false;
		_timer.Elapsed += 
			(sender, args) =>
			{
				Console.WriteLine("Кассир закончил работу: " + ticket.customer.Name + " Кассир " + Name);
				_timer.Stop();
				OnFinishWork?.Invoke(this, new TicketEventArgs(tic));
			};

		_timer.Start();

		return true;
	}
}
