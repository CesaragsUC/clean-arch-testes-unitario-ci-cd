using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace CleanArch.API.Test.Funcionario
{
    [CollectionDefinition(nameof(IntegrationApiFixture))]
    public class FuncionarioIntegrationTest : IClassFixture<SharedFixture<Program>>
    {
        private readonly SharedFixture<Program> _fixture;

        public FuncionarioIntegrationTest(SharedFixture<Program> fixture)
        {
            _fixture = fixture;
        }

    }
}
