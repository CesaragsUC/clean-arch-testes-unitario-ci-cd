using AutoMapper;
using CleanArch.Application.Dtos;
using CleanArch.Domain.Entities;

namespace CleanArch.Application.AutomapperConfig
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Funcionario, FuncionarioDto>().ReverseMap();
            CreateMap<Funcionario, UpdateFuncionarioDto>().ReverseMap();
            CreateMap<Funcionario, CreateFuncionarioDto>().ReverseMap();

            CreateMap<CreateFuncionarioDto, UpdateFuncionarioDto>().ReverseMap();
            CreateMap<CreateFuncionarioDto, FuncionarioDto>().ReverseMap();
            CreateMap<CreateFuncionarioDto, Funcionario>().ReverseMap();

            CreateMap<UpdateFuncionarioDto, FuncionarioDto>().ReverseMap();
            CreateMap<UpdateFuncionarioDto, CreateFuncionarioDto>().ReverseMap();
            CreateMap<UpdateFuncionarioDto, Funcionario>().ReverseMap();

        }
    }
}
