using FluentValidation;
using SalarioWeb.Models;
using SalarioWeb.Repositories.Interfaces;
using SalarioWeb.Services.Interfaces;

namespace SalarioWeb.Services;

public class CargoService(ICargoRepository cargoRepository, IValidator<Cargo> cargoValidator) : ICargoService
{
    private readonly ICargoRepository _cargoRepository = cargoRepository;
    private readonly IValidator<Cargo> _cargoValidator = cargoValidator;

    public async Task<List<Cargo>> GetAllAsync()
    {
        return await _cargoRepository.GetAllAsync();
    }

    public async Task<Cargo?> GetByIdAsync(int id)
    {
        return await _cargoRepository.GetByIdAsync(id);
    }

    public async Task AddAsync(Cargo cargo)
    {
        var validationResult = await _cargoValidator.ValidateAsync(cargo);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _cargoRepository.AddAsync(cargo);
    }

    public async Task UpdateAsync(Cargo cargo)
    {
        var validationResult = await _cargoValidator.ValidateAsync(cargo);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _cargoRepository.UpdateAsync(cargo);
    }

    public async Task DeleteAsync(int id)
    {
        await _cargoRepository.DeleteAsync(id);
    }
}
