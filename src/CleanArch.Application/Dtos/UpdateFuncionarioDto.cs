using CleanArch.Domain.Enumerations;

namespace CleanArch.Application.Dtos
{
    public class UpdateFuncionarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public TipoFuncionario Tipo { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}
