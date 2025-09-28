using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF;

class MockCustomer : ICustomerArrival
{
	public event ICustomerArrival.CustomerArrivedEventHandler CustomerArrived;

	public void Start()
	{
		var custmoer = new Customer(new Order(1000));
		CustomerArrived?.Invoke(custmoer);
	}
}
