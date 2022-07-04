using AutoMapper;


namespace FinStarAPI.DTO
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<FinStarEntity.Models.Items, Items>().ReverseMap();
            CreateMap<InputItems, FinStarEntity.Models.Items>()
                .ForMember(dest => dest.ID, from => from.MapFrom(src => src.Code))
                .ForMember(dest => dest.Value, from => from.MapFrom(src => src.Value));
        }
    }
}
