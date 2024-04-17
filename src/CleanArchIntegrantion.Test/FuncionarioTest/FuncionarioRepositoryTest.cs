using CleanArch.Application.Abstractions;
using CleanArch.Application.Common;
using CleanArch.Application.Dtos;
using CleanArch.Application.Services;
using CleanArch.Application.Validator;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces;
using CleanArchIntegrantion.Test.Factory;
using CleanArchIntegrantion.Test.Fixture;
using Moq;
using Moq.AutoMock;

namespace CleanArchIntegrantion.Test.FuncionarioTest
{
    [Collection(nameof(IntegrationSharedFixture))]
    public class FuncionarioRepositoryTest : FuncionarioFactory
    {
        private readonly SharedFixture<Program> _fixture;
    
        public FuncionarioRepositoryTest(SharedFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Adicionar Novo Funcionario com sucesso")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task CadastrarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            // Act

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(It.IsAny<Funcionario>()))
                .Returns(Task.CompletedTask);

            await service.Create(dto);

            // Assert
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Create(It.IsAny<Funcionario>()), Times.Once);
        }

        [Fact(DisplayName = "Adicionar  Funcionario invalido")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task CadastrarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            CreateFuncionarioDto funcionario = null;

            // Act
            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(It.IsAny<Funcionario>()))
                .Returns(Task.CompletedTask);

            await service.Create(funcionario);

            // Assert
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Create(It.IsAny<Funcionario>()), Times.Never);
        }

        [Fact(DisplayName = "Atualizar Funcionario com sucesso")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task AtualizarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            // Act
            await service.Create(dto);

            var updateDto = _fixture.mapper.Map<UpdateFuncionarioDto>(funcionario);

            updateDto.Nome = "Novo nome";
            updateDto.Email = "pessoa@nome.com";
            updateDto.Telefone = "41998542547";


            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(It.IsAny<Funcionario>()))
                .Returns(Task.CompletedTask);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Update(It.IsAny<Funcionario>()))
                  .Returns(Task.CompletedTask);

            await service.Update(updateDto);

            // Assert
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Create(It.IsAny<Funcionario>()), Times.Once);
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Update(It.IsAny<Funcionario>()), Times.Once);
        }


        [Fact(DisplayName = "Atualizar Funcionario invalido")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task AtualizarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var validator = new UpdateFuncionarioValidator();

            // Act
            var updateDto = _fixture.mapper.Map<UpdateFuncionarioDto>(funcionario);

            updateDto.Nome = string.Empty;
            updateDto.Email = string.Empty;
            updateDto.Telefone = string.Empty;

            var resultValidator = validator.Validate(updateDto);

            //primeiro setup
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Update(It.IsAny<Funcionario>()))
                  .Returns(Task.CompletedTask);

            //depois chama o servico
            await service.Update(updateDto);


            // Assert
            Assert.False(resultValidator.IsValid);
            Assert.True(resultValidator.Errors.Any());
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Update(It.IsAny<Funcionario>()), Times.Never);
        }

        [Fact(DisplayName = "Excluir Funcionario com sucesso")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task ExcluirFuncionarioComSucessoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var Mocker = new AutoMocker();
            var service = _fixture.MockerCreateInstance<FuncionarioServices>();
            // Act

            //primeiro setup
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(funcionario))
                .Returns(Task.CompletedTask);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionario.Id))
                 .ReturnsAsync(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Remove(funcionario))
                  .Returns(Task.CompletedTask);

            //depois chama o servico
            await service.Create(dto);

            await service.Remove(funcionario.Id);


            // Assert
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Create(It.IsAny<Funcionario>()), Times.Once);
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Remove(It.IsAny<Funcionario>()), Times.Once);
        }

        [Fact(DisplayName = "Excluir Funcionario invalido")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task ExcluirFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            Funcionario funcionario = null;

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            // Act

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(Guid.NewGuid()))
                .ReturnsAsync(funcionario);

            await service.Remove(Guid.Empty);

            // Assert
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Remove(It.IsAny<Funcionario>()), Times.Never);
        }

        [Fact(DisplayName = "Obter Funcionario por ID com sucesso")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task ObterFuncionarioPeloIdComSucesso()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();
            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var Mocker = new AutoMocker();
            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            // Act

            //primeiro setup
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionario.Id))
                 .ReturnsAsync(funcionario);

            //depois chama o servico
            var result = await service.GetById(funcionario.Id);

            // Assert
            Assert.True(result.Succeeded);
            Assert.Equal(200, result.Code);
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.GetById(funcionario.Id), Times.Once);

        }

        [Fact(DisplayName = "Obter Funcionario por ID invalido")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task ObterFuncionarioPeloIdInvalidoDeveRetornarErro()
        {
            // Arrange
            Funcionario funcionario = null;
            var funcionarioId = Guid.NewGuid();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            // Act

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionarioId))
                .ReturnsAsync(funcionario);

            var result = await service.GetById(Guid.Empty);

            // Assert
            Assert.False(result.Succeeded);
            Assert.Equal(400, result.Code);
            Assert.True(result.Messages.Any());
            Assert.Equal(result.Messages.FirstOrDefault(), "Funcionário não encontrado");
        }

        [Fact(DisplayName = "Retonar Lista de funcionario")]
        [Trait("Funcionario Repositorio", " Teste Repositorio")]
        public async Task RetornarListaDeFuncionarioComSucesso()
        {
            // Arrange

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionarios = await CriarListaFuncionario(5);

            var listDto = _fixture.mapper.Map<List<FuncionarioDto>>(funcionarios);

            // Act

            //primeiro setup
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.List())
                .ReturnsAsync(funcionarios);

            _fixture.Mocker.GetMock<IFuncionarioService>().Setup(c => c.List())
                .ReturnsAsync(new Result<List<FuncionarioDto>>
                {
                    Data = listDto,
                    Succeeded = true,
                    Code = 200
                }
            );

            //depois chama o servico
            var resultList = await service.List();


            // Assert
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.List(), Times.Once);
            Assert.True(resultList.Succeeded);
            Assert.Equal(200, resultList.Code);
        }
    }
}
  