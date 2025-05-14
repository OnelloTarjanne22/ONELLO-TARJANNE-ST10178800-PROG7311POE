using PROG7311POE_ST10178800.Data;
using PROG7311POE_ST10178800.Models;
using Microsoft.EntityFrameworkCore;
using PROG7311POE_ST10178800.Services;
public class FarmerService : IFarmerService
{
    private readonly AppDbContext _context;

    public FarmerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Farmer>> GetAllFarmersAsync()
    {
        return await _context.Farmers.ToListAsync();
    }

    public async Task AddFarmerAsync(Farmer farmer)
    {
        _context.Farmers.Add(farmer);
        await _context.SaveChangesAsync();
    }
}
