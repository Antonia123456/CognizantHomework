using FoodShareNET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> GetOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, FoodShareNET.Domain.Enums.OrderStatus orderStatus);
    }
}
