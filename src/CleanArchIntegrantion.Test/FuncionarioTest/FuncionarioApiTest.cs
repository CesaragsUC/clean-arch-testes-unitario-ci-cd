using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using CleanArch.Application.Common;
using CleanArch.Application.Dtos;
using CleanArch.Domain.Enumerations;
using CleanArchIntegrantion.Test.Factory;
using CleanArchIntegrantion.Test.Fixture;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchIntegrantion.Test.FuncionarioTest
{
    [Collection(nameof(IntegrationSharedFixture))]
    public class FuncionarioApiTest : FuncionarioFactory
    {
        private readonly SharedFixture<Program> _fixture;
        public FuncionarioApiTest(SharedFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Adicionar Novo Funcionario com sucesso")]
        [Trait("Funcionario API", " Teste API")]
        public async Task CadastrarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            // Act
            var postResponse = await _fixture.Client.PostAsJsonAsync($"api/funcionarios",dto);

            // Assert
            postResponse.EnsureSuccessStatusCode();

            var result = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(postResponse);

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.Code);    
        }

        [Fact(DisplayName = "Adicionar  Funcionario invalido")]
        [Trait("Funcionario API", " Teste API")]
        public async Task CadastrarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioInValido();
            var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            // Act
            var postResponse = await _fixture.Client.PostAsJsonAsync($"api/funcionarios", dto);

            // Assert
            var result = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(postResponse);

            Assert.NotNull(result);
            Assert.False(result.Succeeded);
            Assert.Null(result.Data);
            Assert.Equal(400, result.Code);
            Assert.True(result.Messages.Any()); // deve conter mensagens de erro
        }

        [Fact(DisplayName = "Atualizar Funcionario com sucesso")]
        [Trait("Funcionario API", " Teste API")]
        public async Task AtualizarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            // Act
            var postResponse = await _fixture.Client.PostAsJsonAsync($"api/funcionarios", dto);

            var result = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(postResponse);

            var updateDto = _fixture.mapper.Map<UpdateFuncionarioDto>(result.Data);  

            var faker = new Faker();

            updateDto.Nome = faker.Name.FullName();
            updateDto.Email =  faker.Internet.Email();
            updateDto.CPF =  faker.Person.Cpf();
            updateDto.Telefone =  faker.Phone.PhoneNumber();
            updateDto.Tipo =  faker.PickRandom<TipoFuncionario>();
            updateDto.DataNascimento = faker.Date.Past(50, DateTime.Now);

            var updateResponse = await _fixture.Client.PutAsJsonAsync($"api/funcionarios", updateDto);

            var resultUpdate = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(updateResponse);

            // Assert

            postResponse.EnsureSuccessStatusCode();
            updateResponse.EnsureSuccessStatusCode();  

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.Code);
            Assert.Equal(200, resultUpdate.Code);
            Assert.True(resultUpdate.Succeeded);
            Assert.NotNull(resultUpdate.Data);
        }

        [Fact(DisplayName = "Atualizar Funcionario invalido")]
        [Trait("Funcionario API", " Teste API")]
        public async Task AtualizarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            // Act
            var postResponse = await _fixture.Client.PostAsJsonAsync($"api/funcionarios", dto);

            var result = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(postResponse);

            var updateDto = _fixture.mapper.Map<UpdateFuncionarioDto>(result.Data);

            var faker = new Faker();

            updateDto.Nome = string.Empty;
            updateDto.Email = "email.com";
            updateDto.CPF = "051.000000000";
            updateDto.Telefone = string.Empty;
            updateDto.Tipo = faker.PickRandom<TipoFuncionario>();
            updateDto.DataNascimento = faker.Date.Past(50, DateTime.Now);

            var updateResponse = await _fixture.Client.PutAsJsonAsync($"api/funcionarios", updateDto);

            var resultUpdate = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(updateResponse);

            // Assert

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.Code);
            Assert.NotNull(resultUpdate);
            Assert.False(resultUpdate.Succeeded);
            Assert.Equal(400, resultUpdate.Code);
        }

        [Fact(DisplayName = "Excluir Funcionario com sucesso")]
        [Trait("Funcionario API", " Teste API")]
        public async Task ExcluirFuncionarioComSucessoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            // Act
            var postResponse = await _fixture.Client.PostAsJsonAsync($"api/funcionarios", dto);

            var result = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(postResponse);

            var entidade = _fixture.mapper.Map<UpdateFuncionarioDto>(result.Data);

            var deleteResponse = await _fixture.Client.DeleteAsync($"api/funcionarios/{entidade.Id}");

            var resultDelete = await _fixture.DeseralizaObjetoResponse<Result<bool>>(deleteResponse);

            // Assert

            Assert.NotNull(result);
            Assert.True(result.Succeeded);
            Assert.NotNull(result.Data);
            Assert.Equal(200, result.Code);
            Assert.NotNull(deleteResponse);
            Assert.True(resultDelete.Succeeded);
            Assert.Equal(200, resultDelete.Code);
            Assert.Contains(resultDelete.Messages.FirstOrDefault(), "Removido com sucesso.");
        }

        [Fact(DisplayName = "Excluir Funcionario invalido")]
        [Trait("Funcionario API", " Teste API")]
        public async Task ExcluirFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange

            var deleteResponse = await _fixture.Client.DeleteAsync($"api/funcionarios/{Guid.Empty}");

            // Assert
            Assert.False(deleteResponse.IsSuccessStatusCode);

        }

        [Fact(DisplayName = "Obter Funcionario por ID com sucesso")]
        [Trait("Funcionario API", " Teste API")]
        public async Task ObterFuncionarioPeloIdComSucesso()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            // Act
            var postResponse = await _fixture.Client.PostAsJsonAsync($"api/funcionarios", dto);

            var result = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(postResponse);


            var getResponse = await _fixture.Client.GetAsync($"api/funcionarios/{result.Data.Id}");

            var resulGet = await _fixture.DeseralizaObjetoResponse<Result<FuncionarioDto>>(getResponse);

            // Assert

            Assert.NotNull(resulGet);
            Assert.True(resulGet.Succeeded);
            Assert.NotNull(resulGet.Data);
            Assert.Equal(200, resulGet.Code);

        }

        [Fact(DisplayName = "Obter Funcionario por ID invalido")]
        [Trait("Funcionario API", " Teste API")]
        public async Task ObterFuncionarioPeloIdInvalidoDeveRetornarErro()
        {
            // Arrange

            var deleteResponse = await _fixture.Client.GetAsync($"api/funcionarios/{Guid.Empty}");

            // Assert
            Assert.False(deleteResponse.IsSuccessStatusCode);
        }

        [Fact(DisplayName = "Retonar Lista de funcionario")]
        [Trait("Funcionario API", " Teste API")]
        public async Task RetornarListaDeFuncionarioComSucesso()
        {
            // Arrange
            int total = 5;
            var funcionarios = await CriarListaFuncionario(total);

            foreach (var funcionario in funcionarios)
            {
                var dto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

                // Act
                await _fixture.Client.PostAsJsonAsync($"api/funcionarios", dto);
            }

           
            var getResponse = await _fixture.Client.GetAsync($"api/funcionarios/all");

            var resulGet = await _fixture.DeseralizaObjetoResponse<Result<IEnumerable<FuncionarioDto>>>(getResponse);

            // Assert

            Assert.NotNull(resulGet);
            Assert.True(resulGet.Succeeded);
            Assert.NotNull(resulGet.Data);
            Assert.Equal(200, resulGet.Code);
            Assert.True(resulGet.Data.Count() > 0);

        }
    }
}
