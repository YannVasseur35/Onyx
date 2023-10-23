using AutoMapper;

namespace Onyx.Application.Mappings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<WeatherForecast, WeatherForecastDto>()
                .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Coordinates.Latitude))
                .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Coordinates.Longitude));

            CreateMap<WeatherForecastDto, WeatherForecast>()
                .ForPath(dest => dest.Coordinates.Latitude, opt => opt.MapFrom(src => src.Latitude))
                .ForPath(dest => dest.Coordinates.Longitude, opt => opt.MapFrom(src => src.Longitude));
        }
    }
}