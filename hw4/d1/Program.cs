namespace d1;

public class Customer
{
    public string Id { get; set; }
    public string Name { get; set; }

    public Customer(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => $"Customer[{Id}, {Name}]";

    public override bool Equals(object? obj) => obj is Customer c && c.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();
}

public class Goods
{
    public string Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Goods(string id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }

    public override string ToString() => $"Goods[{Id}, {Name}, {Price:C}]";

    public override bool Equals(object? obj) => obj is Goods g && g.Id == Id;

    public override int GetHashCode() => Id.GetHashCode();
}

public class OrderDetails
{
    public Goods Goods { get; set; }
    public int Quantity { get; set; }
    public decimal Amount => Goods.Price * Quantity;

    public OrderDetails(Goods goods, int quantity)
    {
        Goods = goods;
        Quantity = quantity;
    }

    public override string ToString() => $"  {Goods} x{Quantity} = {Amount:C}";

    public override bool Equals(object? obj) => obj is OrderDetails od && Equals(od.Goods, Goods);

    public override int GetHashCode() => Goods.GetHashCode();
}

public class Order : IComparable<Order>
{
    public string OrderId { get; set; }
    public Customer Customer { get; set; }
    public List<OrderDetails> Details { get; } = new();
    public decimal TotalAmount => Details.Sum(d => d.Amount);

    public Order(string orderId, Customer customer)
    {
        OrderId = orderId;
        Customer = customer;
    }

    public override string ToString()
    {
        var header = $"Order[{OrderId}] {Customer}  Total: {TotalAmount:C}";
        if (Details.Count == 0) return header + "\n  (no details)";
        return header + "\n" + string.Join("\n", Details.Select(d => d.ToString()));
    }

    public override bool Equals(object? obj) => obj is Order o && o.OrderId == OrderId;

    public override int GetHashCode() => OrderId.GetHashCode();

    public int CompareTo(Order? other) =>
        other == null ? 1 : string.Compare(OrderId, other.OrderId, StringComparison.Ordinal);
}

public class OrderService
{
    List<Order> orders = new();

    public void AddOrder(Order order)
    {
        if (orders.Contains(order))
            throw new Exception($"Order {order.OrderId} already exists.");
        orders.Add(order);
    }

    public void DeleteOrder(string orderId)
    {
        var order = orders.FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
            throw new Exception($"Order {orderId} not found.");
        orders.Remove(order);
    }

    public void ModifyOrder(Order updated)
    {
        var index = orders.FindIndex(o => o.OrderId == updated.OrderId);
        if (index < 0)
            throw new Exception($"Order {updated.OrderId} not found.");
        orders[index] = updated;
    }

    public List<Order> QueryByOrderId(string orderId) =>
        orders.Where(o => o.OrderId.Contains(orderId))
              .OrderBy(o => o.TotalAmount)
              .ToList();

    public List<Order> QueryByGoodsName(string goodsName) =>
        orders.Where(o => o.Details.Any(d => d.Goods.Name.Contains(goodsName)))
              .OrderBy(o => o.TotalAmount)
              .ToList();

    public List<Order> QueryByCustomer(string customerName) =>
        orders.Where(o => o.Customer.Name.Contains(customerName))
              .OrderBy(o => o.TotalAmount)
              .ToList();

    public List<Order> QueryByAmount(decimal min, decimal max) =>
        orders.Where(o => o.TotalAmount >= min && o.TotalAmount <= max)
              .OrderBy(o => o.TotalAmount)
              .ToList();

    public void Sort(Func<Order, IComparable>? keySelector = null)
    {
        if (keySelector == null)
            orders.Sort();
        else
            orders.Sort((a, b) => keySelector(a).CompareTo(keySelector(b)));
    }

    public List<Order> GetAll()
    {
        Sort();
        return orders;
    }
}

internal class Program
{
    static OrderService service = new();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\n=== Order Management System ===");
            Console.WriteLine("1. Add Order");
            Console.WriteLine("2. Delete Order");
            Console.WriteLine("3. Modify Order");
            Console.WriteLine("4. Query Orders");
            Console.WriteLine("5. List All Orders");
            Console.WriteLine("6. Sort Orders");
            Console.WriteLine("7. Exit");
            Console.Write("Select: ");

            var choice = Console.ReadLine();
            Console.WriteLine();

            try
            {
                switch (choice)
                {
                    case "1": AddOrder(); break;
                    case "2": DeleteOrder(); break;
                    case "3": ModifyOrder(); break;
                    case "4": QueryOrders(); break;
                    case "5": ListAllOrders(); break;
                    case "6": SortOrders(); break;
                    case "7": return;
                    default: Console.WriteLine("Invalid choice."); break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void AddOrder()
    {
        Console.Write("Order ID: ");
        var orderId = Console.ReadLine()!;
        Console.Write("Customer ID: ");
        var custId = Console.ReadLine()!;
        Console.Write("Customer Name: ");
        var custName = Console.ReadLine()!;

        var order = new Order(orderId, new Customer(custId, custName));

        while (true)
        {
            Console.Write("Add detail? (y/n): ");
            if (Console.ReadLine()!.ToLower() != "y") break;

            Console.Write("  Goods ID: ");
            var gId = Console.ReadLine()!;
            Console.Write("  Goods Name: ");
            var gName = Console.ReadLine()!;
            Console.Write("  Goods Price: ");
            var gPrice = decimal.Parse(Console.ReadLine()!);
            Console.Write("  Quantity: ");
            var qty = int.Parse(Console.ReadLine()!);

            var detail = new OrderDetails(new Goods(gId, gName, gPrice), qty);
            if (order.Details.Contains(detail))
                Console.WriteLine("  Duplicate goods, skipped.");
            else
                order.Details.Add(detail);
        }

        service.AddOrder(order);
        Console.WriteLine($"Order {orderId} added.");
    }

    static void DeleteOrder()
    {
        Console.Write("Order ID to delete: ");
        var id = Console.ReadLine()!;
        service.DeleteOrder(id);
        Console.WriteLine($"Order {id} deleted.");
    }

    static void ModifyOrder()
    {
        Console.Write("Order ID to modify: ");
        var orderId = Console.ReadLine()!;

        Console.Write("New Customer ID: ");
        var custId = Console.ReadLine()!;
        Console.Write("New Customer Name: ");
        var custName = Console.ReadLine()!;

        var order = new Order(orderId, new Customer(custId, custName));

        while (true)
        {
            Console.Write("Add detail? (y/n): ");
            if (Console.ReadLine()!.ToLower() != "y") break;

            Console.Write("  Goods ID: ");
            var gId = Console.ReadLine()!;
            Console.Write("  Goods Name: ");
            var gName = Console.ReadLine()!;
            Console.Write("  Goods Price: ");
            var gPrice = decimal.Parse(Console.ReadLine()!);
            Console.Write("  Quantity: ");
            var qty = int.Parse(Console.ReadLine()!);

            var detail = new OrderDetails(new Goods(gId, gName, gPrice), qty);
            if (order.Details.Contains(detail))
                Console.WriteLine("  Duplicate goods, skipped.");
            else
                order.Details.Add(detail);
        }

        service.ModifyOrder(order);
        Console.WriteLine($"Order {orderId} modified.");
    }

    static void QueryOrders()
    {
        Console.WriteLine("  a. By Order ID");
        Console.WriteLine("  b. By Goods Name");
        Console.WriteLine("  c. By Customer Name");
        Console.WriteLine("  d. By Amount Range");
        Console.Write("  Select: ");
        var sub = Console.ReadLine()!;

        List<Order> result;
        switch (sub.ToLower())
        {
            case "a":
                Console.Write("  Order ID: ");
                result = service.QueryByOrderId(Console.ReadLine()!);
                break;
            case "b":
                Console.Write("  Goods Name: ");
                result = service.QueryByGoodsName(Console.ReadLine()!);
                break;
            case "c":
                Console.Write("  Customer Name: ");
                result = service.QueryByCustomer(Console.ReadLine()!);
                break;
            case "d":
                Console.Write("  Min Amount: ");
                var min = decimal.Parse(Console.ReadLine()!);
                Console.Write("  Max Amount: ");
                var max = decimal.Parse(Console.ReadLine()!);
                result = service.QueryByAmount(min, max);
                break;
            default:
                Console.WriteLine("  Invalid choice.");
                return;
        }

        PrintOrders(result);
    }

    static void ListAllOrders() => PrintOrders(service.GetAll());

    static void SortOrders()
    {
        Console.WriteLine("  Sort by:");
        Console.WriteLine("  1. Order ID (default)");
        Console.WriteLine("  2. Total Amount");
        Console.Write("  Select: ");
        var choice = Console.ReadLine()!;

        if (choice == "2")
            service.Sort(o => o.TotalAmount);
        else
            service.Sort();

        Console.WriteLine("Sorted.");
        PrintOrders(service.GetAll());
    }

    static void PrintOrders(List<Order> orders)
    {
        if (orders.Count == 0)
        {
            Console.WriteLine("(no orders)");
            return;
        }
        foreach (var o in orders)
            Console.WriteLine(o.ToString());
    }
}
