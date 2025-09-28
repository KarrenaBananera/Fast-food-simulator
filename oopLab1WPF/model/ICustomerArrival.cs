using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF;

public interface ICustomerArrival
{
	delegate void CustomerArrivedEventHandler(Customer arrivedCustomer);

	event CustomerArrivedEventHandler CustomerArrived;
	public void Start();
}
