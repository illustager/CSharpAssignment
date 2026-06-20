// References: hw4/d1 (OrderService, Order, OrderDetails, Customer, Goods)

using d1;
using Xunit;

namespace e2;

public class OrderServiceTests
{
    OrderService CreateService()
    {
        var service = new OrderService();

        var order1 = new Order("O001", new Customer("C01", "Alice"));
        order1.Details.Add(new OrderDetails(new Goods("G01", "Apple", 1.5m), 5));
        order1.Details.Add(new OrderDetails(new Goods("G02", "Banana", 2.0m), 3));
        service.AddOrder(order1);

        var order2 = new Order("O002", new Customer("C02", "Bob"));
        order2.Details.Add(new OrderDetails(new Goods("G03", "Cake", 15.0m), 1));
        service.AddOrder(order2);

        var order3 = new Order("O003", new Customer("C03", "Charlie"));
        order3.Details.Add(new OrderDetails(new Goods("G04", "Donut", 3.0m), 4));
        order3.Details.Add(new OrderDetails(new Goods("G01", "Apple", 1.5m), 2));
        service.AddOrder(order3);

        return service;
    }

    [Fact]
    public void AddOrder_ShouldAddOrder()
    {
        var service = new OrderService();
        var order = new Order("X01", new Customer("C01", "Test"));
        order.Details.Add(new OrderDetails(new Goods("G01", "Item", 10m), 1));

        service.AddOrder(order);
        var all = service.GetAll();

        Assert.Single(all);
        Assert.Equal("X01", all[0].OrderId);
    }

    [Fact]
    public void AddOrder_ShouldThrowOnDuplicate()
    {
        var service = CreateService();

        var duplicate = new Order("O001", new Customer("CX", "X"));
        duplicate.Details.Add(new OrderDetails(new Goods("GX", "X", 1m), 1));

        Assert.Throws<Exception>(() => service.AddOrder(duplicate));
    }

    [Fact]
    public void DeleteOrder_ShouldRemoveOrder()
    {
        var service = CreateService();

        service.DeleteOrder("O002");
        var all = service.GetAll();

        Assert.Equal(2, all.Count);
        Assert.DoesNotContain(all, o => o.OrderId == "O002");
    }

    [Fact]
    public void DeleteOrder_ShouldThrowWhenNotFound()
    {
        var service = CreateService();

        Assert.Throws<Exception>(() => service.DeleteOrder("NOTEXIST"));
    }

    [Fact]
    public void ModifyOrder_ShouldUpdateOrder()
    {
        var service = CreateService();

        var updated = new Order("O001", new Customer("C99", "NewName"));
        updated.Details.Add(new OrderDetails(new Goods("G99", "NewItem", 99m), 1));
        service.ModifyOrder(updated);

        var all = service.GetAll();
        var modified = all.First(o => o.OrderId == "O001");

        Assert.Equal("NewName", modified.Customer.Name);
        Assert.Equal(99m, modified.TotalAmount);
    }

    [Fact]
    public void ModifyOrder_ShouldThrowWhenNotFound()
    {
        var service = CreateService();
        var notExist = new Order("NOTEXIST", new Customer("C01", "X"));

        Assert.Throws<Exception>(() => service.ModifyOrder(notExist));
    }

    [Fact]
    public void QueryByOrderId_ShouldReturnMatching()
    {
        var service = CreateService();

        var result = service.QueryByOrderId("O00");

        Assert.Equal(3, result.Count);
        Assert.True(result[0].TotalAmount <= result[1].TotalAmount);
    }

    [Fact]
    public void QueryByGoodsName_ShouldReturnMatching()
    {
        var service = CreateService();

        var result = service.QueryByGoodsName("Apple");

        Assert.Equal(2, result.Count);
        Assert.All(result, o => Assert.Contains(o.Details, d => d.Goods.Name == "Apple"));
    }

    [Fact]
    public void QueryByCustomer_ShouldReturnMatching()
    {
        var service = CreateService();

        var result = service.QueryByCustomer("Bob");

        Assert.Single(result);
        Assert.Equal("O002", result[0].OrderId);
    }

    [Fact]
    public void QueryByAmount_ShouldReturnInRange()
    {
        var service = CreateService();

        var result = service.QueryByAmount(10m, 20m);

        Assert.Equal(3, result.Count);
        Assert.All(result, o => Assert.InRange(o.TotalAmount, 10m, 20m));
    }

    [Fact]
    public void QueryResults_ShouldBeSortedByTotalAmount()
    {
        var service = CreateService();

        var result = service.QueryByOrderId("O00");

        for (int i = 1; i < result.Count; i++)
            Assert.True(result[i - 1].TotalAmount <= result[i].TotalAmount);
    }

    [Fact]
    public void Sort_Default_ShouldSortByOrderId()
    {
        var service = new OrderService();
        service.AddOrder(new Order("Z99", new Customer("C01", "A")));
        service.AddOrder(new Order("A01", new Customer("C02", "B")));
        service.AddOrder(new Order("M50", new Customer("C03", "C")));

        service.Sort();
        var all = service.GetAll();

        Assert.Equal("A01", all[0].OrderId);
        Assert.Equal("M50", all[1].OrderId);
        Assert.Equal("Z99", all[2].OrderId);
    }

    [Fact]
    public void Sort_ByTotalAmount_ShouldSortCorrectly()
    {
        var service = new OrderService();

        var cheap = new Order("O001", new Customer("C01", "A"));
        cheap.Details.Add(new OrderDetails(new Goods("G01", "Item", 1m), 1));
        service.AddOrder(cheap);

        var expensive = new Order("O002", new Customer("C02", "B"));
        expensive.Details.Add(new OrderDetails(new Goods("G02", "Item", 100m), 1));
        service.AddOrder(expensive);

        service.Sort(o => o.TotalAmount);
        var all = service.GetAll();

        Assert.Equal(1m, all[0].TotalAmount);
        Assert.Equal(100m, all[1].TotalAmount);
    }

    [Fact]
    public void Export_ShouldCreateFile()
    {
        var service = CreateService();
        var path = "test_export.xml";

        service.Export(path);

        Assert.True(File.Exists(path));
        File.Delete(path);
    }

    [Fact]
    public void Import_ShouldLoadOrders()
    {
        var service = CreateService();
        var path = "test_import.xml";
        service.Export(path);

        var service2 = new OrderService();
        service2.Import(path);

        var all = service2.GetAll();
        Assert.Equal(3, all.Count);

        File.Delete(path);
    }

    [Fact]
    public void ExportImport_RoundTrip()
    {
        var service = CreateService();
        var path = "test_roundtrip.xml";
        service.Export(path);

        var service2 = new OrderService();
        service2.Import(path);

        var original = service.GetAll();
        var restored = service2.GetAll();

        Assert.Equal(original.Count, restored.Count);
        for (int i = 0; i < original.Count; i++)
        {
            Assert.Equal(original[i].OrderId, restored[i].OrderId);
            Assert.Equal(original[i].Customer.Name, restored[i].Customer.Name);
            Assert.Equal(original[i].TotalAmount, restored[i].TotalAmount);
        }

        File.Delete(path);
    }

    [Fact]
    public void Order_Equals_ById()
    {
        var order1 = new Order("O001", new Customer("C01", "A"));
        var order2 = new Order("O001", new Customer("C02", "B"));
        var order3 = new Order("O002", new Customer("C01", "A"));

        Assert.True(order1.Equals(order2));
        Assert.False(order1.Equals(order3));
    }

    [Fact]
    public void OrderDetails_Equals_ByGoods()
    {
        var goods1 = new Goods("G01", "A", 1m);
        var goods2 = new Goods("G01", "A", 999m);
        var goods3 = new Goods("G02", "B", 1m);

        var detail1 = new OrderDetails(goods1, 5);
        var detail2 = new OrderDetails(goods2, 10);
        var detail3 = new OrderDetails(goods3, 5);

        Assert.True(detail1.Equals(detail2));
        Assert.False(detail1.Equals(detail3));
    }

    [Fact]
    public void ToString_ShouldFormatCorrectly()
    {
        var customer = new Customer("C01", "Alice");
        var goods = new Goods("G01", "Apple", 1.5m);
        var detail = new OrderDetails(goods, 3);
        var order = new Order("O001", customer);
        order.Details.Add(detail);

        Assert.Contains("Customer[C01, Alice]", customer.ToString());
        Assert.Contains("Goods[G01, Apple", goods.ToString());
        Assert.Contains("x3", detail.ToString());
        Assert.Contains("Order[O001]", order.ToString());
    }
}
