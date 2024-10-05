using AutoMapper;
using FluentValidation;
using SalarioWeb.DTOs;
using SalarioWeb.DTOs.Pessoa;
using SalarioWeb.Models;
using SalarioWeb.Repositories.Interfaces;
using SalarioWeb.Services.Interfaces;

namespace SalarioWeb.Services;

public class PessoaService(
    IPessoaRepository pessoaRepository,
    IMapper mapper,
    IValidator<CreatePessoaDTO>
    createPessoaValidator,
    IValidator<UpdatePessoaDTO> updatePessoaValidator
) : IPessoaService
{
    private readonly IMapper _mapper = mapper;
    private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
    private readonly IValidator<CreatePessoaDTO> _createPessoaValidator = createPessoaValidator;
    private readonly IValidator<UpdatePessoaDTO> _updatePessoaValidator = updatePessoaValidator;

    public async Task<List<PessoaDTO>> GetAllAsync()
    {
        var pessoas = await _pessoaRepository.GetAllAsync();
        return _mapper.Map<List<PessoaDTO>>(pessoas);
    }

    public async Task<PessoaDTO?> GetByIdAsync(int id)
    {
        var pessoa = await _pessoaRepository.GetByIdAsync(id);
        return _mapper.Map<PessoaDTO>(pessoa);
    }

    public async Task AddAsync(CreatePessoaDTO createPessoaDTO)
    {
        var validationResult = await _createPessoaValidator.ValidateAsync(createPessoaDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var pessoa = _mapper.Map<Pessoa>(createPessoaDTO);
        
        await _pessoaRepository.AddAsync(pessoa);
    }

    public async Task UpdateAsync(UpdatePessoaDTO updatePessoaDTO)
    {
        var validationResult = await _updatePessoaValidator.ValidateAsync(updatePessoaDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var pessoa = _mapper.Map<Pessoa>(updatePessoaDTO);
        
        await _pessoaRepository.UpdateAsync(pessoa);
    }

    public async Task DeleteAsync(int id)
    {
        await _pessoaRepository.DeleteAsync(id);
    }

    public async Task CalcularSalariosAsync()
    {
        await _pessoaRepository.ExecutarProcedureCalcularSalarios();
    }

    public async Task<PessoaRelatorioDTO?> GetRelatorioDetalhadoAsync(int id)
    {
        return await _pessoaRepository.GetRelatorioDetalhadoAsync(id);
    }

    public async Task<List<PessoaRelatorioDTO>> GetRelatorioGeralAsync()
    {
        return await _pessoaRepository.GetRelatorioGeralAsync();
    }
}
