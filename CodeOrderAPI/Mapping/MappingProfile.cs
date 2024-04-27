using AutoMapper;
using CodeOrderAPI.Model;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Personagem, PersonagemDTO>();
        CreateMap<PersonagemDTO, Personagem>();
    }
}
