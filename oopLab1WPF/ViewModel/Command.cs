using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace oopLab1WPF;
public class Command : ICommand
{
	private readonly Action _execute;
	private readonly Func<bool> _canExecute;
	public event EventHandler _canExecuteChanged;

	public Command(Action execute, Func<bool> canExecute = null)
	{
		_execute = execute;
		_canExecute = canExecute;
	}

	public event EventHandler CanExecuteChanged
	{
		add { _canExecuteChanged += value; }
		remove { _canExecuteChanged -= value; }
	}

	public bool CanExecute(object? parameter) => true;
	public void Execute(object? parameter) => _execute();
}