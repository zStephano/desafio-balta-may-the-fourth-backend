using AutoMapper;
using CodeOrderAPI.Model;
using CodeOrderAPI.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Personagem, PersonagemDTO>()
            .ForMember(dto => dto.PlanetId, conf => conf.MapFrom(p => p.Planet.Id))
            .ForMember(dto => dto.MoviesIds, conf => conf.MapFrom(p => p.Movies.Select(m => m.Id).ToList()));
        CreateMap<PersonagemDTO, Personagem>();


        // mapeamentos para ViewModels
        CreateMap<PersonagemToAddViewModel, Personagem>();
        CreateMap<PersonagemToUpdateViewModel, Personagem>();
    }
}
