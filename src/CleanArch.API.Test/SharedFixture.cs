using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace CleanArch.API.Test
{

    [CollectionDefinition(nameof(IntegrationApiFixture))]
    public class IntegrationApiFixture : IClassFixture<SharedFixture<Program>> { }

    public  class SharedFixture<TProgram> : IDisposable where TProgram : class
    {
        private IServiceScope scope;
        private readonly WebApplicationFactory<TProgram> _webApplicationFactory;
        public HttpClient Client;
        public readonly ApplicationFactory<TProgram> Factory;
        protected IServiceProvider ServiceProvider;

        public SharedFixture()
        {
            _webApplicationFactory = new WebApplicationFactory<TProgram>();

            var clientoptions = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = true,
                BaseAddress = new Uri("https://localhost:7191"),
                HandleCookies = true,
                MaxAutomaticRedirections = 7
            };

            Factory = new ApplicationFactory<TProgram>();
            Client = Factory.CreateClient(clientoptions);

        }


        public void Dispose()
        {
     
        }
    }


}
