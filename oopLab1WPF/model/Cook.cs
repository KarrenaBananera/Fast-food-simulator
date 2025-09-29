using System.Windows.Threading;
using System.Xml.Linq;

namespace oopLab1WPF;

public class Cook : IWorker
{
	private System.Timers.Timer _timer = new ();
	public event EventHandler<TicketEventArgs>? OnFinishWork;
	string Name = Guid.NewGuid().ToString().Substring(30);

	public bool IsWorking { get => _timer.Enabled; }
	public Cook()
	{
	}
	public bool TryTakeTicket(Ticket ticket)
	{
		if (_timer.Enabled == true)
			return false;

		Console.WriteLine("Взят тикет на готовку: " + ticket.customer.Name + " Повар " + Name);
		Ticket tic = ticket;
		_timer = new();
		_timer.Interval = ticket.order.CookTime;
		_timer.AutoReset = false;
		_timer.Elapsed += (sender, args)
			=>
		{
			_timer.Stop();
			OnFinishWork?.Invoke(this, new TicketEventArgs(tic));
		};
		_timer.Start();

		return true;
	}

}
