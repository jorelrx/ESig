using SalarioWeb.DTOs;
using SalarioWeb.Models;

namespace SalarioWeb.Repositories.Interfaces;

public interface IPessoaRepository
{
    Task<List<Pessoa>> GetAllAsync();
    Task<Pessoa?> GetByIdAsync(int id);
    Task AddAsync(Pessoa pessoa);
    Task UpdateAsync(Pessoa pessoa);
    Task DeleteAsync(int id);
    Task ExecutarProcedureCalcularSalarios();
    Task<PessoaRelatorioDTO?> GetRelatorioDetalhadoAsync(int id);
    Task<List<PessoaRelatorioDTO>> GetRelatorioGeralAsync();
}
