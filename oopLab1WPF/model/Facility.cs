using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF;

public class Facility<T> where T : IWorker 
{
	private ConcurrentBag<T> _workers = new();
	public event Action<Ticket, T?>? FinishedWork;

	public IReadOnlyList<T> Workers
	{
		get => _workers.ToArray();
	}

	ConcurrentQueue<Ticket> _ticketsToProcess = new();



	private bool TryTakeTicket(out T? choosedWorker)
	{
		lock (_ticketsToProcess)
		{
			choosedWorker = default;	
			if (_ticketsToProcess.Count == 0)
				return false;

			_ticketsToProcess.TryPeek(out var ticket);

			foreach (var worker in _workers)
			{
				if (worker.TryTakeTicket(ticket))
				{
					choosedWorker = worker;
					_ticketsToProcess.TryDequeue(out _);
					return true;
				}
			}
		}
		return false;
	}
	public void AddTicket(Ticket ticket)
	{
		lock (_ticketsToProcess)
		{
			_ticketsToProcess.Enqueue(ticket);
			TryTakeTicket(out _);	
		}
	}

	public void AddWorkers(params IEnumerable<T> workers)
	{
		lock (_ticketsToProcess)
		{
			foreach (var worker in workers)
			{
				worker.OnFinishWork += Worker_OnFinishWork;
				_workers.Add(worker);
			}
		}
	}

	private void Worker_OnFinishWork(object? sender, TicketEventArgs args)
	{
	
			T? senderClass = default;
			if (sender is T)
				senderClass = (T)sender;

			FinishedWork?.Invoke(args.Ticket, senderClass);
			TryTakeTicket(out _);
	}
}
