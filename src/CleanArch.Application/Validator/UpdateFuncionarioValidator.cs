﻿using CleanArch.Application.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Validator
{
    public class UpdateFuncionarioValidator : AbstractValidator<UpdateFuncionarioDto>
    {
        public UpdateFuncionarioValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().NotNull().WithMessage("Id é obrigatório");

            RuleFor(x => x.Nome)
                .NotEmpty().NotNull().WithMessage("Nome é obrigatório");

            RuleFor(x => x.Email)
                .NotEmpty()
                .NotNull()
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Telefone)
                .NotEmpty()
                .NotNull().WithMessage("Telefone é obrigatório");

            RuleFor(x => x.CPF)
                .NotEmpty()
                .NotNull()
                .Length(14).WithMessage("CPF deve ter 14 caracteres");

            RuleFor(x => x.Tipo)
                .NotEmpty()
                .NotNull().WithMessage("Tipo é obrigatório");

            RuleFor(x => x.DataNascimento)
                .NotEmpty()
                .NotNull().WithMessage("Data de nascimento é obrigatória");
        }   
    }
}
