﻿using FoodShareNET.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNET.Domain.Entities
{
    public class Donation
    {
        public int Id { get; set; }
        public int DonorId { get; set; }
        public Donor Donor { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int StatusId { get; set; }

        public DonationStatus Status { get; set; }
    }
}
