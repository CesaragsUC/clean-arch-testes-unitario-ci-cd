using CleanArch.Application.Common;
using CleanArch.Application.Dtos;

namespace CleanArch.Application.Abstractions
{
    public interface IFuncionarioService
    {
        Task<Result<List<FuncionarioDto>>> List();
        Task<Result<FuncionarioDto>> GetById(Guid id);
        Task<Result<FuncionarioDto>> Create(CreateFuncionarioDto funcionario);
        Task<Result<FuncionarioDto>> Update(UpdateFuncionarioDto funcionario);
        Task<Result<FuncionarioDto>> UpdateAsync(UpdateFuncionarioDto funcionarioDto);
        Task<Result<bool>> Remove(Guid id);
    }
}
