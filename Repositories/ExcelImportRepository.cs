using Microsoft.EntityFrameworkCore;
using SalarioWeb.Models;
using SalarioWeb.Data;
using SalarioWeb.Repositories.Interfaces;

namespace SalarioWeb.Repositories;

public class ExcelImportRepository(SalarioContext context) : IExcelImportRepository
{
    private readonly SalarioContext _context = context;

    public async Task<bool> AddCargosAsync(List<Cargo> cargos)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Cargos ON");
            await _context.Cargos.AddRangeAsync(cargos);
            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Cargos OFF");

            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }

    public async Task<bool> AddPessoasAsync(List<Pessoa> pessoas)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Pessoas ON");
            await _context.Pessoas.AddRangeAsync(pessoas);
            await _context.SaveChangesAsync();
            await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Pessoas OFF");

            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            return false;
        }
    }
}
