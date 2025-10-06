using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF.model;

public class Worker : IWorker
{
	string Name = Guid.NewGuid().ToString().Substring(30);
	private System.Timers.Timer _workTimer = new();
	public event EventHandler<TicketEventArgs>? OnFinishWork;

	public int WorkTime { get; private set; }
	public bool IsWorking { get => _workTimer.Enabled; }

	public Worker(int processSpeed)
	{
		WorkTime = processSpeed;
	}

	public bool TryTakeTicket(Ticket ticket)
	{
		if (_workTimer.Enabled == true)
			return false;

		Ticket ticketForTimer = ticket;
		_workTimer = new();
		_workTimer.Interval = WorkTime;
		_workTimer.AutoReset = false;
		_workTimer.Elapsed +=
			(sender, args) =>
			{
				_workTimer.Stop();
				OnFinishWork?.Invoke(this, new TicketEventArgs(ticketForTimer));
			};

		_workTimer.Start();

		return true;
	}
}
