using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IFoodShareDbContext _context;
        public OrderService(IFoodShareDbContext dbContext) {
            _context = dbContext;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            var donation = await _context.Donations
            .FirstOrDefaultAsync(d => d.Id == order.DonationId);

            // Check if the donation exists
            if (donation == null)
            {
                //return NotFound($"Donation with ID {createOrderDTO.DonationId} not found.");
                throw new NotFoundException("donation",order.DonationId);//donation.Id);
            }

            // Check if the requested quantity is available
            if (donation.Quantity < order.Quantity)
            {
                //return BadRequest($"Requested quantity exceeds available quantity for Donation with ID {createOrderDTO.DonationId}");
                throw new OrderException($"Requested quantity exceeds available quantity for Donation ID {order.DonationId}.");
            }

            donation.Quantity -= order.Quantity;
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

         
            return order;
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            var order = await _context.Orders
            .Include(o => o.Beneficiary)
            .Include(o => o.Donation)
            .Include(o => o.Donation.Product)
            .Include(o => o.OrderStatus)
            .Include(o => o.Courier)
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync();
            if(order == null)
            {
                throw new NotFoundException("order", id);
            }
            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, FoodShareNET.Domain.Enums.OrderStatus orderStatus)
        {

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("order", orderId);
            }
            
            order.OrderStatusId = (int)orderStatus;
            order.DeliveryDate = orderStatus == FoodShareNET.Domain.Enums.OrderStatus.Delivered ? DateTime.UtcNow : order.DeliveryDate;
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
