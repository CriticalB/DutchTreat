using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext ctx;
        private readonly IWebHostEnvironment env;

        public DutchSeeder(DutchContext ctx, IWebHostEnvironment env)
        {
            this.ctx = ctx;
            this.env = env;
        }

        public void Seed()
        {
            this.ctx.Database.EnsureCreated();

            if (!this.ctx.Products.Any())
            {
                //Need to create sample data
                var filePath = Path.Combine(this.env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);

                this.ctx.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    }
                };

                this.ctx.Orders.Add(order);

                this.ctx.SaveChanges();
            }
        }

    }
}
