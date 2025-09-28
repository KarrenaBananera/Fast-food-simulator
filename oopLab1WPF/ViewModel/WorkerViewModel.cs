using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF;

public class WorkerViewModel
{
	private readonly IWorker _worker;

	public WorkerViewModel(IWorker worker)
	{
		_worker = worker;
	}

	public bool IsWorking => _worker.IsWorking;
}