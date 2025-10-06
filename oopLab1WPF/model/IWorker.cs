using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF;

public interface IWorker
{
	public event EventHandler<TicketEventArgs> OnFinishWork;
	public bool TryTakeTicket(Ticket ticket);
	public bool IsWorking { get; }
	public int WorkTime { get; }

}

public class TicketEventArgs : EventArgs
{
	public Ticket Ticket { get; }
	public TicketEventArgs(Ticket ticket) => Ticket = ticket;
}