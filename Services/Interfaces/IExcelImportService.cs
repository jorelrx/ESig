namespace SalarioWeb.Services.Interfaces;

public interface IExcelImportService
{
    Task ImportDataFromExcelAsync(string filePath);
}
