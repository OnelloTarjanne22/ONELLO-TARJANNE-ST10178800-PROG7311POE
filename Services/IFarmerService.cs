using PROG7311POE_ST10178800.Models;
using Microsoft.AspNetCore.Identity;

//Based on example from (IIE,2025)

namespace PROG7311POE_ST10178800.Services
{
    public interface IFarmerService
    {
        Task AddFarmerAsync(Farmer farmer);
        Task<List<Farmer>> GetAllFarmersAsync();
    }
}

