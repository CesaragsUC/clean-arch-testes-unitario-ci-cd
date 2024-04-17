using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchIntegrantion.Test.Fixture
{
    [CollectionDefinition(nameof(ControllerTestFixture))]
    public class ControllerTestFixture : ICollectionFixture<FuncionarioControllerFixture> { }
    public class FuncionarioControllerFixture
    {
    }
}
