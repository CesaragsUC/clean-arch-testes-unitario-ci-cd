using System.Security.Cryptography;
using CleanArch.Domain.ValueObjects;

namespace CleanArch.Domain.Test
{
    [Collection(nameof(DocumentoCollection))]
    public class ValidarDocumentoTest
    {
        private readonly DocumentoFixture _fixture;
        public ValidarDocumentoTest(DocumentoFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("000.000.000.00")]
        [InlineData("000.000.000-00")]
        [InlineData("111.111.111-11")]
        [Trait("Documento", "Validacao")]
        public void Validar_CPFs_Devem_Ser_InValidos(string cpf)
        {
            // Act
            var result = new ValidarCPF();

            // Assert
            Assert.False(result.IsValidarCPF(cpf));
        }

        [Fact(DisplayName = "Cadastrar CPF Valido")]
        [Trait("Documento", "Validacao")]
        public void Validar_CPF_Deve_Ser_Valido()
        {
            // Arrange
            var cpfValido = _fixture.CriarCPFValido();

            // Act
            var result = new ValidarCPF();

            // Assert
            Assert.True(result.IsValidarCPF(cpfValido));
        }

        [Fact(DisplayName = "Cadastrar CPF Inalido")]
        [Trait("Documento", "Validacao")]
        public void Validar_CPF_Deve_Ser_Invalido()
        {
            // Arrange
            var cpfInValido = _fixture.CriarCPFInvalido();

            // Act
            var result = new ValidarCPF();

            // Assert
            Assert.False(result.IsValidarCPF(cpfInValido));
        }


        [Fact(DisplayName = "Cadastrar RG Valido")]
        [Trait("Documento", "Validacao")]
        public void Validar_RG_Deve_Ser_Valido()
        {
            // Arrange
            var rgValido = _fixture.CriarRGValido();

            // Act
            var result = new ValidarRG();

            // Assert
            Assert.True(result.IsValidarRG(rgValido));
        }


        [Fact(DisplayName = "Cadastrar RG Inalido")]
        [Trait("Documento", "Validacao")]
        public void Validar_RG_Deve_Ser_Invalido()
        {
            // Arrange
            var rgInValido = _fixture.CriarRGInvalido();

            // Act
            var result = new ValidarRG();

            // Assert
            Assert.False(result.IsValidarRG(rgInValido));
        }

        [Fact(DisplayName = "CI CD Test")]
        [Trait("Documento", "Validacao")]
        public void CI_CD_Teste()
        {

            Assert.True(1==0);
        }
    }
}