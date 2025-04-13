using API.Data;
using API.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace API.Middlewares
{
    public class BannedCustomerMiddleware
    {
        private readonly RequestDelegate _next;

        public BannedCustomerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context,AppDbContext dbContext) {
         
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userId, out int customerId))
                {
                var banned = await dbContext.Customers.Where(x=>x.Id == customerId&&x.IsBanned==true).FirstOrDefaultAsync();
                    if (banned != null)
                    {
                    if (banned.BannedUntil > DateTime.Now)
                    { if (context.Request.Path.StartsWithSegments("/api/orders", StringComparison.OrdinalIgnoreCase)
                        && (context.Request.Method == HttpMethods.Post ))
                     {
                            context.Response.StatusCode = 400;
                            context.Response.ContentType = "application/json";
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new
                            {
                                message = $"You are banned from creating or modifying orders until {banned.BannedUntil}"
                            }));
                            return;
                        } }
                    else
                    {
                        banned.IsBanned = false;
                        banned.BannedUntil = null;
                        await dbContext.SaveChangesAsync();

                    }
                }
             // }
            }
            await _next(context);
        }
    }
}
