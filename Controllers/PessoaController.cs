using Microsoft.AspNetCore.Mvc;
using SalarioWeb.DTOs;
using SalarioWeb.Services.Interfaces;
using FluentValidation;
using SalarioWeb.DTOs.Pessoa;

namespace SalarioWeb.Controllers;

public class PessoaController : Controller
{
    private readonly IPessoaService _pessoaService;
    private readonly ICargoService _cargoService;

    public PessoaController(IPessoaService pessoaService, ICargoService cargoService)
    {
        _pessoaService = pessoaService;
        _cargoService = cargoService;
    }

    public async Task<IActionResult> Index()
    {
        var pessoas = await _pessoaService.GetAllAsync();
        return View(pessoas);
    }

    public async Task<IActionResult> Details(int id)
    {
        var pessoa = await _pessoaService.GetByIdAsync(id);
        if (pessoa == null)
        {
            return NotFound();
        }

        var relatorio = await _pessoaService.GetRelatorioDetalhadoAsync(id);
        ViewBag.Relatorio = relatorio;

        return View(pessoa);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Cargos = await _cargoService.GetAllAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreatePessoaDTO pessoa)
    {
        Console.WriteLine("ModelState: " + ModelState.IsValid);
        Console.WriteLine("pessoa: " + pessoa);
        try
        {
            await _pessoaService.AddAsync(pessoa);
            return RedirectToAction(nameof(Index));
        }
        catch (ValidationException ex)
        {
            foreach (var error in ex.Errors)
            {
                ModelState.AddModelError(string.Empty, error.ErrorMessage);
            }
        }

        ViewBag.Cargos = await _cargoService.GetAllAsync();
        return View(pessoa);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var pessoa = await _pessoaService.GetByIdAsync(id);
        if (pessoa == null)
        {
            return NotFound();
        }

        ViewBag.Cargos = await _cargoService.GetAllAsync();
        return View(pessoa as UpdatePessoaDTO);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, UpdatePessoaDTO pessoa)
    {
        if (id != pessoa.PessoaId)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                await _pessoaService.UpdateAsync(pessoa);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex)
            {
                foreach (var error in ex.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
            }
        }

        ViewBag.Cargos = await _cargoService.GetAllAsync();
        return View(pessoa);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var pessoa = await _pessoaService.GetByIdAsync(id);
        if (pessoa == null)
        {
            return NotFound();
        }

        return View(pessoa);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _pessoaService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // Novo método para calcular os salários
    public async Task<IActionResult> CalcularSalarios()
    {
        try
        {
            await _pessoaService.CalcularSalariosAsync();
            TempData["Message"] = "Salários calculados e atualizados com sucesso!";

            return RedirectToAction("RelatorioGeral");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, "Erro ao calcular os salários: " + ex.Message);
            return RedirectToAction(nameof(Index));
        }
    }

    public async Task<IActionResult> RelatorioDetalhado(int id)
    {
        var relatorio = await _pessoaService.GetRelatorioDetalhadoAsync(id);
        return View(relatorio);
    }

    public async Task<IActionResult> RelatorioGeral()
    {
        var relatorioGeral = await _pessoaService.GetRelatorioGeralAsync();
        return View(relatorioGeral);
    }
}
