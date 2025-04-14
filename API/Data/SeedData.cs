using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.Enums;
using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class SeedData
    {
        public static void SeedAdmin(IServiceProvider serviceProvider, AppDbContext context)
        {
            context.Database.Migrate(); 

            if (!context.Customers.Any(c => c.Role == UserRole.Admin.ToString()))
            {
                using var hmac = new HMACSHA512();
                var admin = new Customer
                {
                    Username = "admin",
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("admin")),
                    PasswordSalt = hmac.Key,
                    Role = UserRole.Admin.ToString(),
                   
                };

                context.Customers.Add(admin);
                context.SaveChanges();
            }

        }

        public static void SeedProducts(AppDbContext context)
        {
            if (!context.Products.Any())
            {
                var products = new List<Product>
            {
                new Product { Name = "Laptop", Description = "Dell XPS 13", Price = 1500 },
                new Product { Name = "Smartphone", Description = "Samsung Galaxy S24", Price = 1200 },
                new Product { Name = "Monitor", Description = "27-inch 4K Display", Price = 400 },
                new Product { Name = "Keyboard", Description = "Mechanical Keyboard", Price = 100 },
                new Product { Name = "Mouse", Description = "Wireless Mouse", Price = 50 },
                new Product { Name = "Tablet", Description = "Apple iPad Pro 11-inch", Price = 999 },
                new Product { Name = "Headphones", Description = "Sony WH-1000XM5 Noise Cancelling", Price = 350 },
                new Product { Name = "Webcam", Description = "Logitech 4K Pro Webcam", Price = 200 },
                new Product { Name = "External Hard Drive", Description = "2TB Portable SSD", Price = 180 },
                new Product { Name = "Printer", Description = "HP Wireless All-in-One Printer", Price = 250 },
            };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}