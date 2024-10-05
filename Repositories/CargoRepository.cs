using Microsoft.EntityFrameworkCore;
using SalarioWeb.Models;
using SalarioWeb.Data;
using SalarioWeb.Repositories.Interfaces;

namespace SalarioWeb.Repositories;

public class CargoRepository(SalarioContext context) : ICargoRepository
{
    private readonly SalarioContext _context = context;

    public async Task<List<Cargo>> GetAllAsync()
    {
        return await _context.Cargos.ToListAsync();
    }

    public async Task<Cargo?> GetByIdAsync(int id)
    {
        return await _context.Cargos.FirstOrDefaultAsync(c => c.CargoId == id);
    }

    public async Task AddAsync(Cargo cargo)
    {
        await _context.Cargos.AddAsync(cargo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Cargo cargo)
    {
        _context.Cargos.Update(cargo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var cargo = await GetByIdAsync(id);
        if (cargo != null)
        {
            _context.Cargos.Remove(cargo);
            await _context.SaveChangesAsync();
        }
    }
}
