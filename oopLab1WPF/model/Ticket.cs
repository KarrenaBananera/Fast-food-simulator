using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace oopLab1WPF;

public record struct Ticket (Customer customer, Order order, string id)
{

}
