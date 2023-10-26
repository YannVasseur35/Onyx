using AutoMapper;
using Onyx.Infrastructure.Datas;

namespace Onyx.Infrastructure.Services
{
    public class WeatherForecastDataServices : IWeatherForecastDataServices
    {
        private readonly OnyxDbContext _context;
        private readonly IMapper _mapper;

        public WeatherForecastDataServices(OnyxDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WeatherForecast>?> GetAllAsync()
        {
            return _context.WeatherForecasts.Select(x => _mapper.Map<WeatherForecast>(x));
        }

        public Task<WeatherForecast?> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateAsync(WeatherForecast entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }
    }
}