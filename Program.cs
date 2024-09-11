using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFCorePlayground
{
    public class Order
    {
        public int ID { get; set; }
        public string OrderName { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }

    public class OrderItem
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public string ItemName { get; set; }
        public Order Order { get; set; }
    }

    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=orders.db");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new AppDbContext())
            {
                // Create the database and seed some data
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                SeedData(context);

                // Query to join Order and OrderItem
                var query = context.Orders
                    .Join(context.OrderItems,
                        order => order.ID,
                        orderItem => orderItem.OrderID,
                        (order, orderItem) => new
                        {
                            OrderID = order.ID,
                            OrderName = order.OrderName,
                            OrderItemID = orderItem.ID,
                            ItemName = orderItem.ItemName
                        });

                // Print out the results
                foreach (var result in query)
                {
                    Console.WriteLine($"Order ID: {result.OrderID}, Order Name: {result.OrderName}, " +
                                      $"Item ID: {result.OrderItemID}, Item Name: {result.ItemName}");
                }
            }
        }

        static void SeedData(AppDbContext context)
        {
            var order1 = new Order { OrderName = "Order 1" };
            var order2 = new Order { OrderName = "Order 2" };

            context.Orders.AddRange(order1, order2);

            var orderItems = new List<OrderItem>
            {
                new OrderItem { Order = order1, ItemName = "Item 1A" },
                new OrderItem { Order = order1, ItemName = "Item 1B" },
                new OrderItem { Order = order2, ItemName = "Item 2A" }
            };

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();
        }
    }
}