using SalarioWeb.Models;

namespace SalarioWeb.Repositories.Interfaces;

public interface IExcelImportRepository
{
    Task<bool> AddCargosAsync(List<Cargo> cargos);
    Task<bool> AddPessoasAsync(List<Pessoa> pessoas);
}
