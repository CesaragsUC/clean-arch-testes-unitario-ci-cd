using Bogus;
using Bogus.Extensions.Brazil;
using CleanArch.Application.Dtos;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Enumerations;
using System.Text.Json;

namespace CleanArchIntegrantion.Test.Factory
{
    public class FuncionarioFactory
    {
        public FuncionarioFactory()
        {
                
        }

        public async Task<Funcionario> CriarFuncionarioValido()
        {
            var Facker = new Faker();

            return new Funcionario
            {
                Nome = Facker.Name.FullName(),
                Email = Facker.Internet.Email(),
                CPF = Facker.Person.Cpf(),
                Telefone = Facker.Phone.PhoneNumber(),
                Tipo = Facker.PickRandom<TipoFuncionario>(),
                DataNascimento = Facker.Date.Past(50, DateTime.Now)

            };
        }
        public async Task<UpdateFuncionarioDto> AtualizarFuncionarioValido(Guid id)
        {
            var Facker = new Faker();

            return new UpdateFuncionarioDto
            {
                Id = id,
                Nome = Facker.Name.FullName(),
                Email = Facker.Internet.Email(),
                CPF = Facker.Person.Cpf(),
                Telefone = Facker.Phone.PhoneNumber(),
                Tipo = Facker.PickRandom<TipoFuncionario>(),
                DataNascimento = Facker.Date.Past(50, DateTime.Now)

            };
        }

        public async Task<UpdateFuncionarioDto> AtualizarFuncionarioInvalidoValido(Guid id)
        {
            var Facker = new Faker();

            return new UpdateFuncionarioDto
            {
                Id = id,
                Nome = string.Empty,
                Email = "email.com",
                CPF = "123.000.123.23",
                Telefone = string.Empty,
                Tipo = Facker.PickRandom<TipoFuncionario>(),
                DataNascimento = Facker.Date.Past(50, DateTime.Now)

            };
        }

        public async Task<Funcionario> CriarFuncionarioInValido()
        {
            var Facker = new Faker();

            return new Funcionario
            {
                Id = Guid.NewGuid(),
                Nome = string.Empty,
                Email = "email.com",
                CPF = "000.000.000.00",
                Telefone = string.Empty,
                Tipo = Facker.PickRandom<TipoFuncionario>(),
                DataNascimento = Facker.Date.Past(50, DateTime.Now)

            };
        }

        public async Task<IQueryable<Funcionario>> CriarListaFuncionarioQueryable(int total)
        {
            var Facker = new Faker();

            var listaFuncionario = new List<Funcionario>();

            for (int i = 0; i < total; i++)
            {
                var funcionario =  new Funcionario
                {
                    Id = Guid.NewGuid(),
                    Nome = Facker.Name.FullName(),
                    Email = Facker.Internet.Email(),
                    CPF = Facker.Person.Cpf(),
                    Telefone = Facker.Phone.PhoneNumber(),
                    Tipo = Facker.PickRandom<TipoFuncionario>(),
                    DataNascimento = Facker.Date.Past(50, DateTime.Now)

                };
                listaFuncionario.Add(funcionario);
            }
            return listaFuncionario.AsQueryable();
        }

        public async Task<List<Funcionario>> CriarListaFuncionario(int total)
        {
            var Facker = new Faker();

            var listaFuncionario = new List<Funcionario>();

            for (int i = 0; i < total; i++)
            {
                var funcionario = new Funcionario
                {
                    Id = Guid.NewGuid(),
                    Nome = Facker.Name.FullName(),
                    Email = Facker.Internet.Email(),
                    CPF = Facker.Person.Cpf(),
                    Telefone = Facker.Phone.PhoneNumber(),
                    Tipo = Facker.PickRandom<TipoFuncionario>(),
                    DataNascimento = Facker.Date.Past(50, DateTime.Now)

                };
                listaFuncionario.Add(funcionario);
            }
            return listaFuncionario;
        }


        protected async Task<T> DeseralizaObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }
    }
}
