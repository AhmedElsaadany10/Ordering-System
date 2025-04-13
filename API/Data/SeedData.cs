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
                new Product { Name = "Mouse", Description = "Wireless Mouse", Price = 50 }
            };

                context.Products.AddRange(products);
                context.SaveChanges();
            }
        }
    }
}