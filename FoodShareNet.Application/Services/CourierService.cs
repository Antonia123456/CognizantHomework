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
    public class CourierService : ICourierService
    {
        private readonly IFoodShareDbContext _context;
        public CourierService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<IList<Courier>> GetAllCouriersAsync()
        {
            var couriers = await _context.Couriers
                .ToListAsync();
            return couriers;
        }
    }
}
