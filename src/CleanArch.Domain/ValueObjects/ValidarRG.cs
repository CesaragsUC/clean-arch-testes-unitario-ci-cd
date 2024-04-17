namespace CleanArch.Domain.ValueObjects
{
    public class ValidarRG
    {
        private readonly string _numero;
        public ValidarRG()
        {
                
        }
        public ValidarRG(string numero)
        {
            if (!IsValidarRG(numero))
            {
                throw new ArgumentException("RG inválido", nameof(numero));
            }

            _numero = numero;
        }

        public string Numero => _numero;

        public  bool IsValidarRG(string rg)
        {
            // Remova caracteres não numéricos do RG
            rg = new string(rg.Where(char.IsDigit).ToArray());

            // Verifique se o RG tem pelo menos 5 dígitos
            if (rg.Length < 5)
            {
                return false;
            }

            // Aqui você pode adicionar regras específicas de validação para o seu estado, se necessário
            // Por exemplo, alguns estados têm formatos específicos para o RG

            return true;
        }
    }
}
