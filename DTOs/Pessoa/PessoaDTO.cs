namespace SalarioWeb.DTOs.Pessoa
{
    public class PessoaDTO
    {
        public int PessoaId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Cargo { get; set; }
        public decimal Salario { get; set; }
    }
}
