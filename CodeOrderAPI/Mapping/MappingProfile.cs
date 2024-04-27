using AutoMapper;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Personagem, PersonagemDTO>();
        CreateMap<PersonagemDTO, Personagem>();


        // mapeamentos para ViewModels
        CreateMap<PersonagemToAddViewModel, Personagem>();
        CreateMap<PersonagemToUpdateViewModel, Personagem>();
    }
}
