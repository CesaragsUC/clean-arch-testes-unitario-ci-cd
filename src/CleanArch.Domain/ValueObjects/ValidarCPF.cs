using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.ValueObjects
{
    public class ValidarCPF
    {
        private readonly string _numero;
        public ValidarCPF()
        {
                
        }

        public ValidarCPF(string numero)
        {
            if (!IsValidarCPF(numero))
            {
                throw new ArgumentException("CPF inválido", nameof(numero));
            }

            _numero = numero;
        }

        public string Numero => _numero;

        public bool IsValidarCPF(string cpf)
        {
            // Remova caracteres não numéricos do CPF
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // Verifique se o CPF tem 11 dígitos
            if (cpf.Length != 11)
            {
                return false;
            }

            // Verifique se todos os dígitos são iguais (ex: 111.111.111-11)
            if (cpf.All(digit => digit == cpf[0]))
            {
                return false;
            }

            // Cálculo dos dígitos verificadores
            int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
            }

            int resto = soma % 11;
            int digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = 0;

            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
            }

            resto = soma % 11;
            int digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito1.ToString() + digito2.ToString());
        }
    }
}
