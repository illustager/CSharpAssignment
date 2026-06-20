// References: hw4/d1 (OrderService, Order, OrderDetails, Customer, Goods)

using d1;

namespace e1;

internal class Program
{
    static void Main()
    {
        var service = new OrderService();

        var order1 = new Order("E001", new Customer("C01", "Alice"));
        order1.Details.Add(new OrderDetails(new Goods("G01", "Apple", 1.5m), 5));
        order1.Details.Add(new OrderDetails(new Goods("G02", "Banana", 2.0m), 3));
        service.AddOrder(order1);

        var order2 = new Order("E002", new Customer("C02", "Bob"));
        order2.Details.Add(new OrderDetails(new Goods("G03", "Cake", 15.0m), 1));
        service.AddOrder(order2);

        Console.WriteLine("=== Orders before export ===");
        foreach (var o in service.GetAll())
            Console.WriteLine(o.ToString());

        service.Export("orders.xml");
        Console.WriteLine("\nExported to orders.xml");

        var service2 = new OrderService();
        service2.Import("orders.xml");
        Console.WriteLine("Imported from orders.xml\n");

        Console.WriteLine("=== Orders after import ===");
        foreach (var o in service2.GetAll())
            Console.WriteLine(o.ToString());
    }
}
