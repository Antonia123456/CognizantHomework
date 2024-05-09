using FoodShareNet.Application.Exceptions;
using FoodShareNet.Application.Interfaces;
using FoodShareNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
//using FoodShareNetAPI.DTO.Beneficiary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Services
{
    public class BeneficiaryService : IBeneficiaryService
    {
        private readonly IFoodShareDbContext _context;
        public BeneficiaryService(IFoodShareDbContext dbContext)
        {
            _context = dbContext;
        }
        public async Task<Beneficiary> CreateBeneficiaryAsync(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Add(beneficiary);
            await _context.SaveChangesAsync();
            return beneficiary;
        }
        public async Task<Beneficiary> GetBeneficiaryAsync(int? id)
        {
            var beneficiary = await _context.Beneficiaries
                .Include(b => b.City)
                .FirstOrDefaultAsync(b => b.Id == id);
            if (beneficiary == null)
            {
                throw new NotFoundException("Beneficiary", id);
            }
            return beneficiary;
        }
        public async Task<IList<Beneficiary>> GetAllBeneficiariesAsync()
        {
            var beneficiaries = await _context.Beneficiaries
                .Include(b => b.City)
                .ToListAsync();
            return beneficiaries;
        }
        public async Task<bool> DeleteBeneficiaryAsync(int beneficiaryId)
        {
            var beneficiary = await _context.Beneficiaries.FindAsync(beneficiaryId);
            if (beneficiary == null)
            {
                throw new NotFoundException("Beneficiary", beneficiaryId);
            }
            _context.Beneficiaries.Remove(beneficiary);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> EditBeneficiaryAsync(int beneficiaryId, Beneficiary editBeneficiary)
        {

            var beneficiary = await _context.Beneficiaries
               .FirstOrDefaultAsync(b => b.Id == editBeneficiary.Id);

            if (beneficiary == null)
            {
                  throw new NotFoundException("beneficiary",beneficiaryId);
            }
            beneficiary.Name = editBeneficiary.Name;
            beneficiary.Address = editBeneficiary.Address;
            beneficiary.CityId = editBeneficiary.CityId;
            beneficiary.Capacity = editBeneficiary.Capacity;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
