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

                // Pagination variables
                int pageSize = 10;
                int pageNumber = 1; // Change this value for different pages

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
                        })
                .OrderBy(o => o.OrderName)   // First sort by OrderName
                .ThenBy(o => o.ItemName)     // Then sort by ItemName
                .Skip((pageNumber - 1) * pageSize)  // Skip items for previous pages
                .Take(pageSize);             // Take only the items for the current page
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
            var random = new Random();
            var orders = new List<Order>();

            // List of phone accessories products
            var products = new List<string>
    {
        "Phone Case", "Screen Protector", "Wireless Charger", "Car Mount", "Power Bank",
        "Bluetooth Headphones", "USB-C Cable", "Lightning Cable", "Phone Stand",
        "PopSocket", "Earbuds", "Portable Speaker", "Phone Ring Holder", "Phone Cleaning Kit",
        "SIM Card Adapter", "Phone Tripod", "Charging Dock", "Memory Card",
        "Wireless Earbuds", "Phone Camera Lens"
    };

            // Create 25 Orders with descriptive names
            for (int i = 1; i <= 25; i++)
            {
                orders.Add(new Order { OrderName = $"Order {i}: Customer {i} Phone Accessories Purchase" });
            }

            context.Orders.AddRange(orders);

            var orderItems = new List<OrderItem>();

            // Add random number of items (between 1 and 10) for each order
            foreach (var order in orders)
            {
                int itemCount = random.Next(1, 11); // Random number between 1 and 10
                for (int i = 1; i <= itemCount; i++)
                {
                    // Randomly pick a product from the list of phone accessories
                    string randomProduct = products[random.Next(products.Count)];
                    orderItems.Add(new OrderItem
                    {
                        Order = order,
                        ItemName = $"{randomProduct} for {order.OrderName}"
                    });
                }
            }

            context.OrderItems.AddRange(orderItems);
            context.SaveChanges();
        }
    }
}