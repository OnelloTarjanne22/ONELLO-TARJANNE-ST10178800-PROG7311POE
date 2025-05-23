﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PROG7311POE_ST10178800.Models;
//Adapted from tutorial by (Tech & Code with Phi,2022)
namespace PROG7311POE_ST10178800.Data
{
    public class AppDbContext : IdentityDbContext<Employee>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Farmer> Farmers { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
