using CleanArch.Domain.Enumerations;

namespace CleanArch.Domain.Entities
{
    public class Funcionario :BaseEntity
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public TipoFuncionario Tipo { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
