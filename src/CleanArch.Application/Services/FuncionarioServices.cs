using AutoMapper;
using CleanArch.Application.Abstractions;
using CleanArch.Application.Common;
using CleanArch.Application.Dtos;
using CleanArch.Application.Validator;
using CleanArch.Domain.Entities;
using CleanArch.Domain.Interfaces;

namespace CleanArch.Application.Services
{
    public class FuncionarioServices : IFuncionarioService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public FuncionarioServices(IFuncionarioRepository funcionarioRepository, IMapper mapper, IEmailService emailService)
        {
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<Result<List<FuncionarioDto>>> List()
        {
            try
            {
                var funcionarios = await _funcionarioRepository.List();

                var funcionarioDtos = _mapper.Map<List<FuncionarioDto>>(funcionarios);

                if (funcionarioDtos is null)
                    return Result<List<FuncionarioDto>>.Failure("Nenhum funcionário encontrado");


                return await Result<List<FuncionarioDto>>.SuccessAsync(funcionarioDtos);
            }
            catch (Exception ex)
            {

                return await Result<List<FuncionarioDto>>.FailureAsync(500, "Falha no serviço.");
            }

        }

        public async Task<Result<FuncionarioDto>> GetById(Guid id)
        {
            try
            {
                var entity = await _funcionarioRepository.GetById(id);

                if (entity == null)
                    return Result<FuncionarioDto>.Failure(400, "Funcionário não encontrado");

                var dto = _mapper.Map<FuncionarioDto>(entity);

                return await Result<FuncionarioDto>.SuccessAsync(dto);
            }
            catch (Exception ex)
            {
                return await Result<FuncionarioDto>.FailureAsync(500, "falha no serviço.");

            }


        }

        public async Task<Result<FuncionarioDto>> Create(CreateFuncionarioDto funcionarioDto)
        {
            try
            {
                var validor = new CreateFuncionarioValidator();
                var result = validor.Validate(funcionarioDto);

                if (!result.IsValid)
                    return Result<FuncionarioDto>.Failure(400, result.Errors.Select(x=> x.ErrorMessage).ToList());

                var funcionario = _mapper.Map<Funcionario>(funcionarioDto);
                await _funcionarioRepository.Create(funcionario);

                var dto = _mapper.Map<FuncionarioDto>(funcionario);

                await EnviarEmail(funcionarioDto);

                return await Result<FuncionarioDto>.SuccessAsync(dto, "Cadastrado com sucesso.");
            }
            catch (Exception ex)
            {
                return await Result<FuncionarioDto>.FailureAsync(500, "Falha ao cadastrar, falha no serviço.");
            }

        }

        public async Task<Result<FuncionarioDto>> Update(UpdateFuncionarioDto funcionarioDto)
        {
            try
            {
                var validor = new UpdateFuncionarioValidator();
                var result = validor.Validate(funcionarioDto);

                if (!result.IsValid)
                    return Result<FuncionarioDto>.Failure(400, result.Errors.Select(x => x.ErrorMessage).ToList());

                var funcionario = _mapper.Map<Funcionario>(funcionarioDto);
                await _funcionarioRepository.Update(funcionario);

                var dto = _mapper.Map<FuncionarioDto>(funcionario);
                return await Result<FuncionarioDto>.SuccessAsync(dto, "Atualizado com sucesso.");
            }
            catch (Exception ex)
            {

                return await Result<FuncionarioDto>.FailureAsync(500, "Falha ao atualizar, falha no serviço.");
            }

        }

        public async Task<Result<FuncionarioDto>> UpdateAsync(UpdateFuncionarioDto funcionarioDto)
        {
            try
            {
                var validor = new UpdateFuncionarioValidator();
                var result = validor.Validate(funcionarioDto);

                if (!result.IsValid)
                    return Result<FuncionarioDto>.Failure(400, result.Errors.Select(x => x.ErrorMessage).ToList());

                var funcionario = _mapper.Map<Funcionario>(funcionarioDto);
                await _funcionarioRepository.UpdateAsync(funcionario);

                var dto = _mapper.Map<FuncionarioDto>(funcionario);
                return await Result<FuncionarioDto>.SuccessAsync(dto, "Atualizado com sucesso.");
            }
            catch (Exception ex)
            {

                return await Result<FuncionarioDto>.FailureAsync(500, "Falha ao atualizar, falha no serviço.");
            }

        }

        public async Task<Result<bool>> Remove(Guid id)
        {
            try
            {
                var funcionario = await _funcionarioRepository.GetById(id);
                if (funcionario == null)
                    return Result<bool>.Failure(400,"Funcionário não encontrado");


                await _funcionarioRepository.Remove(funcionario);

                return await Result<bool>.SuccessAsync("Removido com sucesso.");
            }
            catch (Exception ex)
            {
                return await Result<bool>.FailureAsync(500, "Falha no serviço.");
            }

        }
        private async Task EnviarEmail(CreateFuncionarioDto funcionario)
        {
            await _emailService.SendAsync(new EmailRequestDto
            {
                To = funcionario.Email,
                Subject = "Bem vindo",
                Body = "Seja bem vindo ao sistema"
            });
        }

        private async Task<bool> Novometodo01(int numero)
        {
            return numero > 0;
        }

        private async Task<bool> Novometodo02(int numero)
        {
            return numero < 0;
        }

        private async Task<int> Novometodo03(int numero)
        {
            return numero ;
        }
    }
}
