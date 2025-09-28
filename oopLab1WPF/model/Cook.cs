using System.Windows.Threading;

namespace oopLab1WPF;

public class Cook : IWorker
{
	private DispatcherTimer _timer = new ();
	public event EventHandler<TicketEventArgs>? OnFinishWork;

	public bool IsWorking { get => _timer.IsEnabled; }
	public Cook()
	{
	}
	public bool TryTakeTicket(Ticket ticket)
	{
		if (_timer.IsEnabled == true)
			return false;

		Ticket tic = ticket;
		_timer.Interval = new TimeSpan((long)ticket.order.CookTime);
		_timer.Tick += (sender, args)
			=>
		{
			_timer.Stop();
			OnFinishWork?.Invoke(this, new TicketEventArgs(tic));
		};
		_timer.Start();

		return true;
	}

}
