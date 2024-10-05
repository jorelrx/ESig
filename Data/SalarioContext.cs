using Microsoft.EntityFrameworkCore;
using SalarioWeb.Models;

namespace SalarioWeb.Data;

public class SalarioContext(DbContextOptions<SalarioContext> options) : DbContext(options)
{
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Cargo> Cargos { get; set; }
    public DbSet<PessoaSalario> PessoaSalarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>()
            .HasOne(p => p.Cargo)
            .WithMany()
            .HasForeignKey(p => p.Cargo_ID);
    }

    public async Task CreateStoredProcedure()
    {
        await Database.ExecuteSqlRawAsync(@"
            CREATE OR ALTER PROCEDURE CalcularSalarios
            AS
            BEGIN
                SET NOCOUNT ON;

                -- Desaloca o cursor se ele já existir
                IF CURSOR_STATUS('GLOBAL', 'PessoaCursor') >= 0
                BEGIN
                    CLOSE PessoaCursor;
                    DEALLOCATE PessoaCursor;
                END;

                DECLARE @PessoaId INT, 
                        @Nome NVARCHAR(100), 
                        @Salario DECIMAL(18, 2), 
                        @CargoId INT;

                DECLARE PessoaCursor CURSOR FOR
                SELECT 
                    p.PessoaId,
                    p.Nome,
                    c.Salario,
                    p.Cargo_ID
                FROM Pessoas p
                INNER JOIN Cargos c ON p.Cargo_ID = c.CargoId;

                OPEN PessoaCursor;

                FETCH NEXT FROM PessoaCursor INTO @PessoaId, @Nome, @Salario, @CargoId;

                WHILE @@FETCH_STATUS = 0
                BEGIN
                    -- Verifica se já existe um cálculo para o mesmo dia e atualiza
                    IF EXISTS (SELECT 1 
                            FROM PessoaSalarios 
                            WHERE PessoaId = @PessoaId 
                            AND CONVERT(DATE, DataCalculo) = CONVERT(DATE, GETDATE()))
                    BEGIN
                        UPDATE PessoaSalarios
                        SET Salario = @Salario, 
                            DataCalculo = GETDATE()
                        WHERE PessoaId = @PessoaId
                        AND CONVERT(DATE, DataCalculo) = CONVERT(DATE, GETDATE());
                    END
                    ELSE
                    BEGIN
                        -- Insere um novo registro se não houver cálculo anterior para o dia
                        INSERT INTO PessoaSalarios (PessoaId, Nome, Salario, DataCalculo)
                        VALUES (@PessoaId, @Nome, @Salario, GETDATE());
                    END;

                    FETCH NEXT FROM PessoaCursor INTO @PessoaId, @Nome, @Salario, @CargoId;
                END;

                CLOSE PessoaCursor;
                DEALLOCATE PessoaCursor;
            END;
        ");
    }
}
