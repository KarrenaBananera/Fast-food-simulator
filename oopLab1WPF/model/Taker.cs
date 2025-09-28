
using System.Windows.Threading;

namespace oopLab1WPF;

public class Taker : IWorker
{
	
	private DispatcherTimer _timer = new();
	public event EventHandler<TicketEventArgs>? OnFinishWork;

	private double _processSpeed { get; }
	public bool IsWorking { get => _timer.IsEnabled; }

	public Taker(double processSpeed)
	{
		_processSpeed = processSpeed;
	}

	public bool TryTakeTicket(Ticket ticket)
	{
		if (_timer.IsEnabled == true) 
			return false;

		Ticket tic = ticket;
		_timer.Interval = new TimeSpan((long)_processSpeed);
		_timer.Tick += 
			(sender, args) =>
			{
				_timer.Stop();
				OnFinishWork?.Invoke(this, new TicketEventArgs(tic));
			};

		_timer.Start();

		return true;
	}
}
