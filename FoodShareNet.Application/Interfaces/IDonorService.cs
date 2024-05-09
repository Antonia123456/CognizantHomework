using FoodShareNET.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNet.Application.Interfaces
{
    public interface IDonorService
    {
        Task<Donor> CreateDonorAsync(Donor donor);
        Task<Donor> GetDonorAsync(int? id);
        Task<IList<Donor>> GetAllDonorsAsync();

        Task<bool> EditDonorAsync(int donorId, Donor donor);

        Task<bool> DeleteDonorAsync(int donorId);
    }
}
