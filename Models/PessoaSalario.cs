namespace SalarioWeb.Models;

public class PessoaSalario
{
    public int PessoaSalarioId { get; set; }
    public int PessoaId { get; set; }
    public string Nome { get; set; }
    public decimal Salario { get; set; }
    public DateTime DataCalculo { get; set; }
}
