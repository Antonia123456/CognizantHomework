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
    public class DonorService : IDonorService
    {
        private readonly IFoodShareDbContext _context;
        public DonorService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Donor> CreateDonorAsync(Donor donor)
        {
            _context.Donors.Add(donor);
            await _context.SaveChangesAsync();
            return donor;
        }
        public async Task<bool> EditDonorAsync(int donorId, Donor editDonor)
        {
            var donor = await _context.Donors
            .FirstOrDefaultAsync(d => d.Id == editDonor.Id);

            if (donor == null)
            {
                throw new NotFoundException("donor", donorId);
            }

            donor.Name = editDonor.Name;
            donor.Address = editDonor.Address;
            donor.CityId = editDonor.CityId;

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<Donor> GetDonorAsync(int? id)
        {
            var donor = await _context.Donors
                .Include(d => d.City)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (donor == null)
            {
                throw new NotFoundException("Donor", id);
            }
            return donor;
        }

        public async Task<IList<Donor>> GetAllDonorsAsync()
        {
            var donors = await _context.Donors
                .Include(d => d.City)
                .ToListAsync();
            return donors;
        }
        public async Task<bool> DeleteDonorAsync(int donorId)
        {
            var donor = await _context.Donors.FindAsync(donorId);

            if (donor == null)
            {
                throw new NotFoundException("Donor", donorId);
            }
            _context.Donors.Remove(donor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
