using Microsoft.EntityFrameworkCore;
using SalarioWeb.Data;
using SalarioWeb.DTOs;
using SalarioWeb.Models;
using SalarioWeb.Repositories.Interfaces;

namespace SalarioWeb.Repositories;

public class PessoaRepository(SalarioContext context) : IPessoaRepository
{
    private readonly SalarioContext _context = context;

    public async Task<List<Pessoa>> GetAllAsync()
    {
        return await _context.Pessoas.Include(p => p.Cargo).ToListAsync();
    }

    public async Task<Pessoa?> GetByIdAsync(int id)
    {
        return await _context.Pessoas.Include(p => p.Cargo)
                                     .FirstOrDefaultAsync(p => p.PessoaId == id);
    }

    public async Task AddAsync(Pessoa pessoa)
    {
        await _context.Pessoas.AddAsync(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Pessoa pessoa)
    {
        _context.Pessoas.Update(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pessoa = await GetByIdAsync(id);
        if (pessoa != null)
        {
            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ExecutarProcedureCalcularSalarios()
    {
        await _context.Database.ExecuteSqlRawAsync("EXEC CalcularSalarios");
    }

    public async Task<PessoaRelatorioDTO?> GetRelatorioDetalhadoAsync(int id)
    {
        var pessoa = await _context.Pessoas.Include(p => p.Cargo)
                                            .FirstOrDefaultAsync(p => p.PessoaId == id);

        if (pessoa == null)
        {
            return null;
        }

        var salarios = await _context.PessoaSalarios
                                        .Where(ps => ps.PessoaId == id)
                                        .OrderBy(ps => ps.DataCalculo)
                                        .Select(ps => new SalarioDetalhadoDTO
                                        {
                                            Ano = ps.DataCalculo.Year,
                                            Salario = ps.Salario,
                                            DataCalculo = ps.DataCalculo
                                        }).ToListAsync();

        var salarioInicial = salarios.FirstOrDefault()?.Salario ?? 0;
        var salarioAtual = salarios.LastOrDefault()?.Salario ?? 0;

        return new PessoaRelatorioDTO
        {
            PessoaId = pessoa.PessoaId,
            Nome = pessoa.Nome,
            Cargo = pessoa.Cargo.Nome,
            SalarioInicial = salarioInicial,
            SalarioAnualInicial = salarioInicial * 12,
            SalarioAtual = salarioAtual,
            SalarioAnualAtual = salarioAtual * 12,
            DataCalculo = salarios.LastOrDefault()?.DataCalculo ?? DateTime.MinValue,
            DetalhesSalario = salarios
        };
    }

    public async Task<List<PessoaRelatorioDTO>> GetRelatorioGeralAsync()
    {
        var pessoas = await _context.Pessoas.Include(p => p.Cargo)
                                            .ToListAsync();

        var relatorios = new List<PessoaRelatorioDTO>();

        foreach (var pessoa in pessoas)
        {
            var salarios = await _context.PessoaSalarios
                                            .Where(ps => ps.PessoaId == pessoa.PessoaId)
                                            .OrderBy(ps => ps.DataCalculo)
                                            .Select(ps => new SalarioDetalhadoDTO
                                            {
                                                Ano = ps.DataCalculo.Year,
                                                Salario = ps.Salario,
                                                DataCalculo = ps.DataCalculo
                                            }).ToListAsync();

            var salarioInicial = salarios.FirstOrDefault()?.Salario ?? 0;
            var salarioAtual = salarios.LastOrDefault()?.Salario ?? 0;

            var relatorio = new PessoaRelatorioDTO
            {
                PessoaId = pessoa.PessoaId,
                Nome = pessoa.Nome,
                Cargo = pessoa.Cargo.Nome,
                SalarioInicial = salarioInicial,
                SalarioAnualInicial = salarioInicial * 12,
                SalarioAtual = salarioAtual,
                SalarioAnualAtual = salarioAtual * 12,
                DataCalculo = salarios.LastOrDefault()?.DataCalculo ?? DateTime.MinValue,
                DetalhesSalario = salarios
            };

            relatorios.Add(relatorio);
        }

        return relatorios;
    }
}
