using Bogus;
using Bogus.Extensions.Brazil;
using CleanArch.Application.Dtos;
using CleanArch.Application.Services;
using CleanArch.Application.Validator;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Enumerations;
using CleanArch.Domain.Interfaces;
using CleanArchIntegrantion.Test.Factory;
using CleanArchIntegrantion.Test.Fixture;
using Moq;

namespace CleanArchIntegrantion.Test.FuncionarioTest
{
    [Collection(nameof(IntegrationSharedFixture))]
    public class FuncionarioServiceTest : FuncionarioFactory
    {
        private readonly SharedFixture<Program> _fixture;
        public FuncionarioServiceTest(SharedFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Adicionar Novo Funcionario com sucesso")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task CadastrarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);


            var validation = new CreateFuncionarioValidator();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(funcionario))
                                                             .Returns(Task.CompletedTask);

            // Act
            var result = await service.Create(dto);
            var resultValidator = validation.Validate(dto);

            // Assert

            Assert.Equal(200, result.Code);
            Assert.True(result.Succeeded);
            Assert.True(resultValidator.IsValid);
            Assert.Empty(resultValidator.Errors);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Create(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Adicionar  Funcionario invalido")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task CadastrarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioInValido();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            var validation = new CreateFuncionarioValidator();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(funcionario))
                                                             .Returns(Task.CompletedTask);

            // Act
            var result = await service.Create(dto);
            var resultValidator = validation.Validate(dto);

            // Assert

            Assert.Equal(400, result.Code);
            Assert.False(result.Succeeded);
            Assert.False(resultValidator.IsValid);
            Assert.NotEmpty(resultValidator.Errors);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Create(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Atualizar Funcionario com sucesso")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task AtualizarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange

            var facker = new Faker();

            var funcionario = await CriarFuncionarioValido();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var validationUpdate = new UpdateFuncionarioValidator();


            funcionario.Nome = facker.Name.FullName();
            funcionario.Email = facker.Internet.Email();
            funcionario.CPF = facker.Person.Cpf();
            funcionario.Telefone = facker.Phone.PhoneNumber();
            funcionario.Tipo = facker.PickRandom<TipoFuncionario>();
            funcionario.DataNascimento = facker.Date.Past(50, DateTime.Now);

            var dtoUpdate = _fixture.mapper.Map<UpdateFuncionarioDto>(funcionario);


            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.UpdateAsync(It.IsAny<Funcionario>()))
            .Returns(Task.CompletedTask);

            // Act

            var resultUpdate = await service.UpdateAsync(dtoUpdate);
            var resultValidatorUpdate = validationUpdate.Validate(dtoUpdate);

            // Assert

            Assert.Equal(200, resultUpdate.Code);
            Assert.Equal("Atualizado com sucesso.", resultUpdate.Messages.FirstOrDefault());
            Assert.True(resultUpdate.Succeeded);
            Assert.True(resultValidatorUpdate.IsValid);
            Assert.Empty(resultValidatorUpdate.Errors);

        }

        [Fact(DisplayName = "Atualizar Funcionario invalido")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task AtualizarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange

            var facker = new Faker();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionario = await CriarFuncionarioInValido();

            var dto = _fixture.mapper.Map<UpdateFuncionarioDto>(funcionario);

            var validationUpdate = new UpdateFuncionarioValidator();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Update(funcionario))
            .Returns(Task.CompletedTask);

            // Act

            var resultUpdate = await service.Update(dto);
            var resultValidatorUpdate = validationUpdate.Validate(dto);

            // Assert

            Assert.Equal(400, resultUpdate.Code);
            Assert.False(resultUpdate.Succeeded);
            Assert.False(resultValidatorUpdate.IsValid);
            Assert.NotEmpty(resultValidatorUpdate.Errors);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Excluir Funcionario com sucesso")]
        [Trait("Funcionario Service", " Teste Service")]
        public async Task ExcluirFuncionarioComSucessoDeveRetornarErro()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var dto = _fixture.mapper.Map<CreateFuncionarioDto>(funcionario);

            //primeiro setup
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Create(funcionario))
                .Returns(Task.CompletedTask);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionario.Id))
                 .ReturnsAsync(funcionario);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Remove(funcionario))
                  .Returns(Task.CompletedTask);

            // Act
            var result = await service.Remove(funcionario.Id);

            // Assert

            Assert.Equal(200, result.Code);
            Assert.True(result.Succeeded);

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Remove(funcionario), Times.Once);
        }

        [Fact(DisplayName = "Excluir Funcionario invalido")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task ExcluirFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange

            var facker = new Faker();

            Funcionario funcionario = null;

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.Remove(funcionario))
            .Returns(Task.CompletedTask);

            // Act

            var resultRemove = await service.Remove(Guid.NewGuid());

            // Assert

            Assert.Equal(400, resultRemove.Code);
            Assert.Equal("Funcionário não encontrado", resultRemove.Messages.FirstOrDefault());
            Assert.False(resultRemove.Succeeded);


            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.Update(funcionario), Times.Never);
        }

        [Fact(DisplayName = "Obter Funcionario por ID com sucesso")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task ObterFuncionarioPeloIdComSucesso()
        {
            // Arrange

            var facker = new Faker();

            var funcionario = await CriarFuncionarioValido();

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionario.Id))
            .ReturnsAsync(new Funcionario
            {
                Nome = funcionario.Nome,
                Email = funcionario.Email,
                CPF = funcionario.CPF,
                Telefone = funcionario.Telefone,
                Tipo = funcionario.Tipo,
                DataNascimento = funcionario.DataNascimento
            });

            // Act

            var resultRemove = await service.GetById(funcionario.Id);

            // Assert

            Assert.Equal(200, resultRemove.Code);
            Assert.NotNull(resultRemove.Data);
            Assert.True(resultRemove.Succeeded);


            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.GetById(funcionario.Id), Times.Once);
        }

        [Fact(DisplayName = "Obter Funcionario por ID invalido")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task ObterFuncionarioPeloIdInvalidoDeveRetornarErro()
        {
            // Arrange

            var facker = new Faker();

            Funcionario funcionario = null;

            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionarioId = Guid.NewGuid();

            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.GetById(funcionarioId))
            .ReturnsAsync(funcionario);

            // Act

            var resultRemove = await service.GetById(funcionarioId);

            // Assert

            Assert.Equal(400, resultRemove.Code);
            Assert.Null(resultRemove.Data);
            Assert.False(resultRemove.Succeeded);
            Assert.Equal("Funcionário não encontrado", resultRemove.Messages.FirstOrDefault());


            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.GetById(funcionarioId), Times.Once);
        }

        [Fact(DisplayName = "Retonar Lista de funcionario")]
         [Trait("Funcionario Service", " Teste Service")]
        public async Task RetornarListaDeFuncionarioComSucesso()
        {
            var service = _fixture.MockerCreateInstance<FuncionarioServices>();

            var funcionarios = await CriarListaFuncionario(5);

            var listDto = _fixture.mapper.Map<List<FuncionarioDto>>(funcionarios);


            _fixture.Mocker.GetMock<IFuncionarioRepository>().Setup(c => c.List())
                                                             .ReturnsAsync(funcionarios);

            // Act
            var result = await service.List();

            // Assert

            Assert.Equal(200, result.Code);
            Assert.True(result.Succeeded);
     
            _fixture.Mocker.GetMock<IFuncionarioRepository>().Verify(r => r.List(), Times.Once);

        }

    }
}
