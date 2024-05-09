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
    public class DonationService : IDonationService
    {
        private readonly IFoodShareDbContext _context;
        public DonationService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Donation> CreateDonationAsync(Donation donation)
        {
            var product = await _context.Products
          .FirstOrDefaultAsync(p => p.Id == donation.ProductId);
            var status = await _context.DonationStatuses
               .FirstOrDefaultAsync(s => s.Id == donation.StatusId);
            // Check if the product exists
            if (product == null)
            {
                throw new NotFoundException("Donation Product",donation.ProductId);
            }
            // Check if the status exists
            if (status == null)
            {
                throw new NotFoundException("Donation Status",donation.StatusId);
            }

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return donation;
        }
        public async Task<Donation> GetDonationAsync(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Product)
                .Include(d => d.Status)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();
            if (donation == null)
            {
                throw new NotFoundException("donation", id);
            }
            return donation;
        }

        public async Task<IList<Donation>> GetDonationsByCityIdAsync(int cityId)
        {
            var donations = await _context.Donations
                .Include(d => d.Donor)
                .Include(d => d.Product)
                .Include(d => d.Status)
                .Where(d => d.Donor.CityId == cityId)
                .ToListAsync();
           
            return donations;
        }

    }
}
