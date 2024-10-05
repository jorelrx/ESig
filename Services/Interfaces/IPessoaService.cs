using SalarioWeb.DTOs;
using SalarioWeb.DTOs.Pessoa;

namespace SalarioWeb.Services.Interfaces;

public interface IPessoaService
{
    Task<List<PessoaDTO>> GetAllAsync();
    Task<PessoaDTO?> GetByIdAsync(int id);
    Task AddAsync(CreatePessoaDTO pessoa);
    Task UpdateAsync(UpdatePessoaDTO pessoa);
    Task DeleteAsync(int id);
    Task CalcularSalariosAsync();
    Task<PessoaRelatorioDTO?> GetRelatorioDetalhadoAsync(int id);
    Task<List<PessoaRelatorioDTO>> GetRelatorioGeralAsync();
}
