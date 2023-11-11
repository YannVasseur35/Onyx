using AutoMapper;
using Onyx.Infrastructure.Datas;
using Onyx.Infrastructure.Models.Entities;

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
            return await Task.FromResult(_context.WeatherForecasts.Select(x => _mapper.Map<WeatherForecast>(x)));
        }

        public async Task<WeatherForecast?> GetAsync(Guid id)
        {
            return _mapper.Map<WeatherForecast>(await _context.WeatherForecasts.FindAsync(id));
        }

        public async Task<Guid> AddOrUpdateAsync(WeatherForecast entity)
        {
            WeatherForecastEntity? dbEntity = entity.Id == Guid.Empty ? null : await _context.WeatherForecasts.FindAsync(entity.Id);

            if (dbEntity == null)
            {
                //ADD
                dbEntity = new WeatherForecastEntity();
                _mapper.Map<WeatherForecast, WeatherForecastEntity>(entity, dbEntity);

                dbEntity.ModifiedAt = dbEntity.CreatedAt;

                _context.WeatherForecasts.Add(dbEntity);
                await _context.SaveChangesAsync();
            }
            else
            {
                //UPDATE

                _mapper.Map<WeatherForecast, WeatherForecastEntity>(entity, dbEntity);

                dbEntity.ModifiedAt = DateTime.UtcNow;

                _context.WeatherForecasts.Update(dbEntity);
                _context.SaveChanges();
            }

            return dbEntity.Id;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.WeatherForecasts.FindAsync(id);

            if (entity != null)
            {
                _context.WeatherForecasts.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}