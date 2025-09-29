
namespace oopLab1WPF;

public class Customer
{
	public string Name = Guid.NewGuid().ToString().Substring(20);
	public Customer(Order order)
	{
		this.Order = order;
	}
	public Order Order { get; }	
}
