﻿using FoodShareNet.Application.Interfaces;
using FoodShareNET.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodShareNET.Repository.Data
{
    public class FoodShareNetDbContext : DbContext, IFoodShareDbContext
    {
        public FoodShareNetDbContext(DbContextOptions<FoodShareNetDbContext> options) : base(options) { }
        public DbSet<Courier> Couriers { get; set; }
        public DbSet<Beneficiary> Beneficiaries { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<DonationStatus> DonationStatuses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<City> Cities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(
                new City { Id = 1, Name = "Bucuresti" },
                new City { Id = 2, Name = "Cluj-Napoca" },
                new City { Id = 3, Name = "Timisoara" },
                new City { Id = 4, Name = "Arad" }
            );

            modelBuilder.Entity<Courier>().HasData(
                new Courier { Id = 1, Name = "DPD", Price = 20 },
                new Courier { Id = 2, Name = "DHL", Price = 15 },
                new Courier { Id = 3, Name = "GLS", Price = 17.5M }
            );

            modelBuilder.Entity<DonationStatus>().HasData(
                new DonationStatus { Id = 1, Name = "Pending" },
                new DonationStatus { Id = 2, Name = "Approved" },
                new DonationStatus { Id = 3, Name = "Rejected" }
            );

            modelBuilder.Entity<OrderStatus>().HasData(
                new OrderStatus { Id = 1, Name = "Unconfirmed" },
                new OrderStatus { Id = 2, Name = "Confirmed" },
                new OrderStatus { Id = 3, Name = "InDelivery" },
                new OrderStatus { Id = 4, Name = "Delivered" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Tomatoes" ,Image = "https://i.stack.imgur.com/N6LYW.jpg" },
                new Product { Id = 2, Name = "Potatoes" , Image = "https://scitechdaily.com/images/Potato-Sunlight-777x518.jpg" },
                new Product { Id = 3, Name = "Meat" , Image = "https://www.tastingtable.com/img/gallery/15-ingredients-that-will-seriously-elevate-your-steak/l-intro-1663169111.jpg" }
            );

            modelBuilder.Entity<Courier>()
                .Property(c => c.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Donation)
                .WithMany()
                .HasForeignKey(o => o.DonationId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
