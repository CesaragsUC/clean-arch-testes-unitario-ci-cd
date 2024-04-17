using AutoMapper;
using CleanArch.Application.AutomapperConfig;
using CleanArchIntegrantion.Test.Factory;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using System.Text.Json;

namespace CleanArchIntegrantion.Test.Fixture
{
    [CollectionDefinition(nameof(IntegrationSharedFixture))]
    public class IntegrationSharedFixture : ICollectionFixture<SharedFixture<Program>> { }

    public class SharedFixture<TProgram> : IDisposable where TProgram : class
    {
        private IServiceScope scope;
        public readonly ApplicationFactory<TProgram> Factory;
        public IServiceProvider ServiceProvider;
        public HttpClient Client;
        public AutoMocker Mocker;
        public IMapper mapper;

        public SharedFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost:7267"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

           // Mocker = new AutoMocker();

            Factory = new ApplicationFactory<TProgram>();
            Client = Factory.CreateClient(clientOptions);

            scope = Factory.Services.CreateScope();
            ServiceProvider = scope.ServiceProvider;

            mapper = ServiceProvider.GetService<IMapper>();

        }

        public async Task<T> DeseralizaObjetoResponse<T>(HttpResponseMessage responseMessage)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<T>(await responseMessage.Content.ReadAsStringAsync(), options);
        }


        public Mock<TEntity> MockDbContext<TEntity>() where TEntity : DbContext
        {
            var options = new DbContextOptionsBuilder<TEntity>()
                           .UseSqlite(ApplicationFactory<Program>.ConnectionString).Options;

            var mockContext = new Mock<TEntity>(options);

            return mockContext;
        }

        /// <summary>
        /// Configura um DbSet de um objeto do banco de dados, ex: DbSet<Funcionario>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public Mock<DbSet<T>> MockDbSet<T>(List<T> data) where T : class
        {
            var queryableData = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();

            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());

            return mockDbSet;
        }


        /// <summary>
        /// MockerCreateInstance<T>:
        /// Este método é responsável por criar uma instância de uma classe usando um AutoMocker.
        /// Um AutoMocker é uma ferramenta que facilita a criação de objetos de teste injetando automaticamente mocks para todas as dependências.
        /// Ele é útil quando você quer criar uma instância de uma classe para testes e deseja que todas as dependências dessa classe sejam automaticamente substituídas por mocks. 
        /// Retorna uma instância da classe especificada com todas as suas dependências injetadas como mocks.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T MockerCreateInstance<T>() where T : class
        {
            Mocker = new AutoMocker();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Substitua por seu perfil de mapeamento real
                cfg.AddProfile<AutoMapperConfig>();
            });

            var mapper = mapperConfig.CreateMapper();

            Mocker.Use<IMapper>(mapper);

            return Mocker.CreateInstance<T>();
        }

        /// <summary>
        /// CreateService<T>:
        /// Este método é responsável por criar uma instância de um serviço real que está registrado no contêiner de injeção de dependência.
        /// Ele configura um ambiente semelhante ao que a aplicação real usaria, permitindo que você teste o serviço em um contexto mais próximo do ambiente de produção.
        /// Retorna uma instância do serviço real especificado, que pode ser usada para chamar métodos reais e testar o comportamento real do serviço.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T CreateService<T>() where T : class
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost:7267"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Mocker = new AutoMocker();

            Mocker.Use<IMapper>(mapper);

            Client = Factory.CreateClient(clientOptions);

            scope = Factory.Services.CreateScope();
            ServiceProvider = scope.ServiceProvider;

            mapper = ServiceProvider.GetService<IMapper>();

            // obtem o ServiceProvider das interfaces para uso nos testes
            var service = ServiceProvider.GetService<T>();
            return service;
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
            scope?.Dispose();
        }
    }
}
