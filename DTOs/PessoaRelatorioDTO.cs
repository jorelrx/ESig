namespace SalarioWeb.DTOs
{
public class PessoaRelatorioDTO
{
    public int PessoaId { get; set; }
    public string Nome { get; set; }
    public string Cargo { get; set; }
    public decimal SalarioInicial { get; set; }
    public decimal SalarioAnualInicial { get; set; }
    public decimal SalarioAtual { get; set; }
    public decimal SalarioAnualAtual { get; set; }
    public DateTime DataCalculo { get; set; }
    public List<SalarioDetalhadoDTO> DetalhesSalario { get; set; }
    }
}
