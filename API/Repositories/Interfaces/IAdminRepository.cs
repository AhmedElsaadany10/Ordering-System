﻿using API.DTOs;
using API.Models;

namespace API.Repositories.Interfaces
{
    public interface IAdminRepository
    {

        Task<IEnumerable<ProductOrderDto>> GetOrdersByCustomerIdAsync(int customerId);
        Task<List<CustomerDto>> GetAllCustomersAsync();

    }
}
