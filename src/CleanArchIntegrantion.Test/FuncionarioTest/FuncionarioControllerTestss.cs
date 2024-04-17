using Bogus;
using Bogus.Extensions.Brazil;
using CleanArch.Application.Abstractions;
using CleanArch.Application.Common;
using CleanArch.Application.Dtos;
using CleanArch.Application.Services;
using CleanArch.Application.Validator;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Enumerations;
using CleanArch.Domain.Interfaces;
using CleanArch.Presentation.Controllers;
using CleanArchIntegrantion.Test.Factory;
using CleanArchIntegrantion.Test.Fixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;

namespace CleanArchIntegrantion.Test.FuncionarioTest
{
    [Collection(nameof(IntegrationSharedFixture))]
    public class FuncionarioControllerTestss : FuncionarioFactory
    {
        private readonly SharedFixture<Program> _fixture;
        public FuncionarioControllerTestss(SharedFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Adicionar Novo Funcionario com sucesso")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task CadastrarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionario = await CriarFuncionarioValido();

            var createFuncionarioDto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var funcionarioDto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.Create(createFuncionarioDto))
            .ReturnsAsync(new Result<FuncionarioDto> { 
                Data = funcionarioDto,
                Code = 200,
                Succeeded = true
            });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Create(createFuncionarioDto);

            var resultValue = (result as OkObjectResult).Value as Result<FuncionarioDto>;


            // Assert
            Assert.NotNull(resultValue); 
            Assert.IsType<Result<FuncionarioDto>>(resultValue);
            Assert.Equal(resultValue.Code, 200);
            Assert.Equal(resultValue.Messages.FirstOrDefault(), "Cadastrado com sucesso.");
        }

        [Fact(DisplayName = "Adicionar  Funcionario invalido")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task CadastrarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionario = await CriarFuncionarioInValido();

            var createFuncionarioDto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var funcionarioDto = _fixture.mapper.Map<FuncionarioDto>(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.Create(createFuncionarioDto))
            .ReturnsAsync(new Result<FuncionarioDto>
            {
                Data = funcionarioDto,
                Code = 400,
                Succeeded = false
            });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Create(createFuncionarioDto);

            var resultValue = (result as BadRequestObjectResult).Value as Result<FuncionarioDto>;


            // Assert

            Assert.True(resultValue.Messages.Count() > 0);
            Assert.Equal(resultValue.Code, 400);
        }


        [Fact(DisplayName = "Adicionar Funcionario Nulo")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task AdicionarFuncionarioNuloDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            Funcionario funcionario = null;
            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Create(dto);

            var response = result as BadRequestObjectResult;

            ////Assert
            Assert.Equal(response.StatusCode, 400);
            Assert.Equal(response.Value, "Dados inválidos");

        }


        [Fact(DisplayName = "Atualizar Funcionario com sucesso")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task AtualizarFuncionarioComSucessoDeveRetornarOk()
        {
            //Arrange
            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var dto = await AtualizarFuncionarioValido(Guid.NewGuid());

            var validor = new UpdateFuncionarioValidator(); 
            var resultValidator = validor.Validate(dto);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.Update(dto))
                .ReturnsAsync(new Result<FuncionarioDto>
                {
                    Data = _fixture.mapper.Map<FuncionarioDto>(dto),
                    Code = 200,
                    Succeeded = true
                });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Update(dto);

            var response = (result as OkObjectResult).Value as Result<FuncionarioDto>;

            //Assert
            Assert.True(response.Messages.Count() > 0);
            Assert.Equal(response.Code, 200);
            Assert.True(resultValidator.IsValid);
            Assert.Empty(resultValidator.Errors);
        }

        [Fact(DisplayName = "Atualizar Funcionario invalido")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task AtualizarFuncionarioInvalidoDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var dto = await AtualizarFuncionarioInvalidoValido(Guid.NewGuid());

            var validor = new UpdateFuncionarioValidator();
            var resultValidator = validor.Validate(dto);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.Update(dto))
                .ReturnsAsync(new Result<FuncionarioDto>
                {
                    Data = _fixture.mapper.Map<FuncionarioDto>(dto),
                    Code = 200,
                    Succeeded = true
                });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Update(dto);

            var response = (result as BadRequestObjectResult).Value as Result<FuncionarioDto>;

            //Assert
            Assert.True(response.Messages.Count() > 0);
            Assert.Equal(response.Code, 400);
            Assert.False(resultValidator.IsValid);
            Assert.NotEmpty(resultValidator.Errors);
        }

        [Fact(DisplayName = "Atualizar Funcionario Nulo")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task AtualizarFuncionarioNuloDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            Funcionario funcionario = null;
            var dto = _fixture.mapper.Map<UpdateFuncionarioDto>(funcionario);

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Update(dto);

            var response = result as BadRequestObjectResult;

            ////Assert
            Assert.Equal(response.StatusCode, 400);
            Assert.Equal(response.Value, "Dados inválidos");

        }

        [Fact(DisplayName = "Excluir Funcionario com sucesso")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task ExcluirFuncionarioComSucessoDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionario = await CriarFuncionarioValido();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionario.Id))
                 .ReturnsAsync(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.Remove(funcionario.Id))
                .ReturnsAsync(new Result<bool>
                {
                    Code = 200,
                    Succeeded = true
                });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Delete(funcionario.Id);

            var response = (result as OkObjectResult).Value as Result<bool>;

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal(response.Code, 200);
            Assert.Equal(response.Messages.FirstOrDefault(), "Removido com sucesso.");
        }

        [Fact(DisplayName = "Excluir Funcionario invalido")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task ExcluirFuncionarioInvalidoDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionarioId = Guid.NewGuid();

            Funcionario funcionario = null;

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionarioId))
                 .ReturnsAsync(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.Remove(funcionarioId))
                .ReturnsAsync(new Result<bool>
                {
                    Code = 400,
                    Succeeded = false
                });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Delete(funcionarioId);

            var response = (result as BadRequestObjectResult).Value as Result<bool>;

            //Assert
            Assert.False(response.Succeeded);
            Assert.Equal(response.Code, 400);
            Assert.Equal(response.Messages.FirstOrDefault(), "Funcionário não encontrado");
        }

        [Fact(DisplayName = "Excluir Funcionario Id invalido")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task ExcluirFuncionarioComIdInvalidoDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionarioId = Guid.Empty;

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Delete(funcionarioId);

            var response = result as BadRequestObjectResult;

            ////Assert
            Assert.Equal(response.StatusCode,400);
            Assert.Equal(response.Value, "Id inválido");

        }

        [Fact(DisplayName = "Obter Funcionario por ID com sucesso")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task ObterFuncionarioPeloIdComSucesso()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionario =  await CriarFuncionarioValido();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionario.Id))
                 .ReturnsAsync(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.GetById(funcionario.Id))
                .ReturnsAsync(new Result<FuncionarioDto>
                {
                    Data = _fixture.mapper.Map<FuncionarioDto>(funcionario),
                    Code = 200,
                    Succeeded = true
                });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Get(funcionario.Id);

            var response = (result as OkObjectResult).Value as Result<FuncionarioDto>;

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal(response.Code, 200);
            Assert.NotNull(response.Data);
        }

        [Fact(DisplayName = "Obter Funcionario por ID invalido")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task ObterFuncionarioPeloIdInvalidoDeveRetornarErro()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionarioId = Guid.Empty;

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Get(funcionarioId);

            var response = result as BadRequestObjectResult;

            //Assert
            Assert.Equal(response.Value, "Id inválido");
            Assert.Equal(response.StatusCode, 400);

        }

        [Fact(DisplayName = "Retonar Lista de funcionario")]
        [Trait("Funcionario Controller", " Teste Controller")]
        public async Task RetornarListaDeFuncionarioComSucesso()
        {
            //Arrange

            var mockService = _fixture.MockerCreateInstance<FuncionarioServices>();
            

            var funcionarios = await CriarListaFuncionario(10);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.List())
                 .ReturnsAsync(funcionarios);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.List())
                .ReturnsAsync(new Result<List<FuncionarioDto>>
                {
                    Data = _fixture.mapper.Map<List<FuncionarioDto>>(funcionarios),
                    Code = 200,
                    Succeeded = true
                });

            var controller = new FuncionariosController(mockService);

            //Act

            var result = await controller.Get();

            var response = (result as OkObjectResult).Value as Result<List<FuncionarioDto>>;

            //Assert
            Assert.True(response.Succeeded);
            Assert.Equal(response.Code, 200);
            Assert.NotNull(response.Data);
            Assert.True(response.Data.Count() > 0);
        }
    }
}
