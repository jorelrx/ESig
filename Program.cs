using Microsoft.EntityFrameworkCore;
using SalarioWeb.Application.Configurations;
using SalarioWeb.Data;
using SalarioWeb.Services;
using SalarioWeb.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddApplicationServices(builder.Configuration.GetConnectionString("DefaultConnection"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SalarioContext>();
    var excelImportService = scope.ServiceProvider.GetRequiredService<IExcelImportService>();
    dbContext.Database.Migrate();

    await dbContext.CreateStoredProcedure();

    var excelFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Excels", "dados.xlsx");
    await excelImportService.ImportDataFromExcelAsync(excelFilePath);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
