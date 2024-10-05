using AutoMapper;
using SalarioWeb.DTOs.Pessoa;
using SalarioWeb.Models;

namespace SalarioWeb.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Pessoa, PessoaDTO>()
                .ForMember(dest => dest.Cargo, opt => opt.MapFrom(src => src.Cargo.Nome)) // Mapeando o nome do Cargo
                .ReverseMap();
            CreateMap<Pessoa, CreatePessoaDTO>().ReverseMap();
            CreateMap<Pessoa, UpdatePessoaDTO>().ReverseMap();
        }
    }
}
