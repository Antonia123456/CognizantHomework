using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNET.Domain.Entities
{
    public class OrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; } // Unconfirmed, Confirmed, In Delivery, Delivered
    }
}
