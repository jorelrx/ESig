using OfficeOpenXml;
using SalarioWeb.Models;
using SalarioWeb.Services.Interfaces;
using SalarioWeb.Repositories.Interfaces;

namespace SalarioWeb.Services;

public class ExcelImportService(
    IExcelImportRepository excelImportRepository,
    IPessoaService pessoaService,
    ICargoService cargoService
) : IExcelImportService
{
    private readonly IExcelImportRepository _excelImportRepository = excelImportRepository;
    private readonly IPessoaService _pessoaService = pessoaService;
    private readonly ICargoService _cargoService = cargoService;

    public async Task ImportDataFromExcelAsync(string filePath)
    {
        string[] formatosPermitidos = ["MM/dd/yyyy", "dd/MM/yyyy", "yyyy-MM-dd"];
        var cargosParaAdicionar = new List<Cargo>();
        var pessoasParaAdicionar = new List<Pessoa>();
        var erroImportacaoPessoas = new List<string>();

        using var package = new ExcelPackage(new FileInfo(filePath));

        // Importação de Cargos
        var cargosSheet = package.Workbook.Worksheets["Cargo"];
        if (cargosSheet != null)
        {
            for (int row = 2; row <= cargosSheet.Dimension.End.Row; row++)
            {
                var cargoId = int.Parse(cargosSheet.Cells[row, 1].Text);
                var nome = cargosSheet.Cells[row, 2].Text;
                var salario = decimal.Parse(cargosSheet.Cells[row, 3].Text);

                if (await _cargoService.GetByIdAsync(cargoId) == null)
                {
                    var cargo = new Cargo
                    {
                        CargoId = cargoId,
                        Nome = nome,
                        Salario = salario
                    };
                    cargosParaAdicionar.Add(cargo);
                }
            }
        }

        // Importação de Pessoas
        var pessoasSheet = package.Workbook.Worksheets["Pessoa"];
        if (pessoasSheet != null)
        {
            for (int row = 2; row <= pessoasSheet.Dimension.End.Row; row++)
            {
                try
                {
                    var dataNascimentoString = pessoasSheet.Cells[row, 10].Text;
                    DateTime.TryParseExact(dataNascimentoString, formatosPermitidos,
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None,
                                out DateTime dataNascimento);

                    var pessoaId = int.Parse(pessoasSheet.Cells[row, 1].Text);
                    var nome = pessoasSheet.Cells[row, 2].Text;
                    var cidade = pessoasSheet.Cells[row, 3].Text;
                    var email = pessoasSheet.Cells[row, 4].Text;
                    var cep = pessoasSheet.Cells[row, 5].Text;
                    var endereco = pessoasSheet.Cells[row, 6].Text;
                    var pais = pessoasSheet.Cells[row, 7].Text;
                    var usuario = pessoasSheet.Cells[row, 8].Text;
                    var telefone = pessoasSheet.Cells[row, 9].Text;
                    var cargoId = int.Parse(pessoasSheet.Cells[row, 11].Text);

                    if (await _pessoaService.GetByIdAsync(pessoaId) == null)
                    {
                        var pessoa = new Pessoa
                        {
                            PessoaId = pessoaId,
                            Nome = nome,
                            Email = email,
                            Cidade = cidade,
                            CEP = cep,
                            Endereco = endereco,
                            Pais = pais,
                            Usuario = usuario,
                            Telefone = telefone,
                            Data_Nascimento = dataNascimento,
                            Cargo_ID = cargoId
                        };
                        pessoasParaAdicionar.Add(pessoa);
                    }
                }
                catch (Exception)
                {
                    erroImportacaoPessoas.Add(pessoasSheet.Cells[row, 8].Text);
                }
            }
        }

        try
        {
            if (cargosParaAdicionar.Count != 0)
            {
                await _excelImportRepository.AddCargosAsync(cargosParaAdicionar);
            }

            if (pessoasParaAdicionar.Count != 0)
            {
                await _excelImportRepository.AddPessoasAsync(pessoasParaAdicionar);
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Ocorreu um erro durante a importação.");
        }

        // Imprimir os usuários que tiveram erro durante a importação
        if (erroImportacaoPessoas.Count != 0)
        {
            Console.WriteLine("Erro ao importar as seguintes pessoas:");
            foreach (var usuario in erroImportacaoPessoas)
            {
                Console.WriteLine(usuario);
            }
        }
    }
}
