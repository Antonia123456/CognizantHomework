using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodShareNET.Domain.Entities;

namespace FoodShareNet.Application.Interfaces
{
    public interface ICourierService
    {
        Task<IList<Courier>> GetAllCouriersAsync();
    }
}
