namespace SalarioWeb.DTOs.Pessoa
{
    public class UpdatePessoaDTO : PessoaDTO
    {
        public string CEP { get; set; }
        public string Endereco { get; set; }
        public string Pais { get; set; }
        public string Usuario { get; set; }
        public string Telefone { get; set; }
        public DateTime Data_Nascimento { get; set; }
        public string Cargo_ID { get; set; }
    }
}
