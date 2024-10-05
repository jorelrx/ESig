using Microsoft.AspNetCore.Mvc;
using SalarioWeb.Services.Interfaces;

namespace SalarioWeb.Controllers;

public class CargoController(ICargoService cargoService) : Controller
{
    private readonly ICargoService _cargoService = cargoService;

    public async Task<IActionResult> Index()
    {
        var cargos = await _cargoService.GetAllAsync();
        return View(cargos);
    }

    public async Task<IActionResult> Details(int id)
    {
        var cargo = await _cargoService.GetByIdAsync(id);
        if (cargo == null)
        {
            return NotFound();
        }

        return View(cargo);
    }
}
