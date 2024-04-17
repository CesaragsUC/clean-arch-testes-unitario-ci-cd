using Bogus;
using Bogus.Extensions.Brazil;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Enumerations;
using CleanArch.Persistence;
using CleanArch.Persistence.Repository;
using CleanArchIntegrantion.Test.Factory;
using CleanArchIntegrantion.Test.Fixture;
using MockQueryable.Moq;
using Moq;

namespace CleanArchIntegrantion.Test.FuncionarioTest
{
    [Collection(nameof(IntegrationSharedFixture))]
    public class FuncionariodbContextTest : FuncionarioFactory
    {
        private readonly SharedFixture<Program> _fixture;
        public FuncionariodbContextTest(SharedFixture<Program> fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Adicionar Novo Funcionario com sucesso")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task CadastrarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var funcionario = await CriarFuncionarioValido();

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();

            mockContext.Setup(c => c.Funcionarios.Add(funcionario));

            // Act

            var repository = new FuncionarioRepository(mockContext.Object);

            await repository.Create(funcionario);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Add(funcionario), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact(DisplayName = "Adicionar  Funcionario invalido")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task CadastrarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange
            Funcionario funcionario = null;

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();

            mockContext.Setup(c => c.Funcionarios.Add(funcionario));

            // Act

            var repository = new FuncionarioRepository(mockContext.Object);

            await repository.Create(funcionario);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Add(funcionario), Times.Never);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact(DisplayName = "Atualizar Funcionario com sucesso")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task AtualizarFuncionarioComSucessoDeveRetornarOk()
        {
            // Arrange
            var faker = new Faker();

            var funcionario = await CriarFuncionarioValido();

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();
            var repository = new FuncionarioRepository(mockContext.Object);


            // Act

            mockContext.Setup(c => c.Funcionarios.Add(funcionario));

            await repository.Create(funcionario);


            funcionario.Nome = faker.Name.FullName();
            funcionario.Email = faker.Internet.Email();
            funcionario.CPF = faker.Person.Cpf();
            funcionario.Telefone = faker.Phone.PhoneNumber();
            funcionario.Tipo = faker.PickRandom<TipoFuncionario>();
            funcionario.DataNascimento = faker.Date.Past(50, DateTime.Now);

            mockContext.Setup(c => c.Funcionarios.Update(funcionario));

            await repository.Update(funcionario);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Add(funcionario), Times.Once);
            mockContext.Verify(m => m.Funcionarios.Update(funcionario), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Exactly(2));
           
        }

        [Fact(DisplayName = "Atualizar Funcionario invalido")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task AtualizarFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange

            Funcionario funcionario = null;

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();
            var repository = new FuncionarioRepository(mockContext.Object);


            // Act

            mockContext.Setup(c => c.Funcionarios.Update(funcionario));

            await repository.Update(funcionario);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Update(funcionario), Times.Never);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact(DisplayName = "Excluir Funcionario com sucesso")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task ExcluirFuncionarioComSucessoDeveRetornarErro()
        {
            // Arrange
            var faker = new Faker();

            var funcionario = await CriarFuncionarioValido();

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();
            var repository = new FuncionarioRepository(mockContext.Object);

            // Act

            mockContext.Setup(c => c.Funcionarios.Add(funcionario));

            await repository.Create(funcionario);

            var toRemove = funcionario;

            mockContext.Setup(c => c.Funcionarios.Remove(toRemove));

            await repository.Remove(toRemove);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Add(funcionario), Times.Once);
            mockContext.Verify(m => m.Funcionarios.Remove(toRemove), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Exactly(2));
        }

        [Fact(DisplayName = "Excluir Funcionario invalido")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task ExcluirFuncionarioInvalidoDeveRetornarErro()
        {
            // Arrange

            Funcionario funcionario = null;

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();
            var repository = new FuncionarioRepository(mockContext.Object);


            // Act

            mockContext.Setup(c => c.Funcionarios.Remove(funcionario));

            await repository.Remove(funcionario);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Update(funcionario), Times.Never);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact(DisplayName = "Obter Funcionario por ID com sucesso")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task ObterFuncionarioPeloIdComSucesso()
        {
            // Arrange
            var faker = new Faker();

            var funcionario = await CriarFuncionarioValido();

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();
            var repository = new FuncionarioRepository(mockContext.Object);

            // Act

            mockContext.Setup(c => c.Funcionarios.Add(funcionario));

            await repository.Create(funcionario);

            mockContext.Setup(c => c.Funcionarios.FindAsync(funcionario.Id))
                .ReturnsAsync(funcionario);

            await repository.GetById(funcionario.Id);

            //Assert
            mockContext.Verify(m => m.Funcionarios.Add(funcionario), Times.Once);
            mockContext.Verify(m => m.Funcionarios.FindAsync(funcionario.Id), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact(DisplayName = "Obter Funcionario por ID invalido")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task ObterFuncionarioPeloIdInvalidoDeveRetornarErro()
        {
            // Arrange
            var faker = new Faker();

            var funcionario = await CriarFuncionarioValido();

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();
            var repository = new FuncionarioRepository(mockContext.Object);

            // Act

            await repository.GetById(Guid.Empty);

            //Assert
            mockContext.Verify(m => m.Funcionarios.FindAsync(Guid.Empty), Times.Never);
        }

        [Fact(DisplayName = "Retonar Lista de funcionario")]
        [Trait("Funcionario DbContext", " Teste DbContext")]
        public async Task RetornarListaDeFuncionarioComSucesso()
        {
            // Arrange
            var funcionarios = await CriarListaFuncionario(5);

            var mockSet = funcionarios.AsQueryable().BuildMockDbSet();

            var mockContext = _fixture.MockDbContext<FuncionarioDbContext>();

            mockContext.Setup(c => c.Funcionarios).Returns(mockSet.Object);

            // Act
            var repository = new FuncionarioRepository(mockContext.Object);

            var result = await repository.List();

            //Assert
            Assert.NotNull(result);

        }
    }
}
