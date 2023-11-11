namespace Onyx.Application.Services
{
    public class WeatherForecastAppServices : IWeatherForecastAppServices
    {
        private readonly INotificationsAppServices _notificationsService;
        private readonly IWeatherForecastDataServices _weatherForecastDataServices;
        private readonly IMapper _mapper;

        public WeatherForecastAppServices(
            INotificationsAppServices notificationsService,
            IWeatherForecastDataServices weatherForecastDataServices,
            IMapper mapper)
        {
            _notificationsService = notificationsService;
            _weatherForecastDataServices = weatherForecastDataServices;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WeatherForecastDto>?> GetAllAsync()
        {
            try
            {
                var weatherForecastList = await _weatherForecastDataServices.GetAllAsync();

                return weatherForecastList == null ? (IEnumerable<WeatherForecastDto>?)null : weatherForecastList.Select(x => _mapper.Map<WeatherForecastDto>(x));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<OperationResult<WeatherForecastDto?>> GetByIdAsync(Guid guid)
        {
            var operation = new OperationResult<WeatherForecastDto?>();
            try
            {
                if (guid == Guid.Empty)
                {
                    return operation.Build(null, "id should not be null or empty", false, OperationErrorType.Functional);
                }

                var feedback = await _weatherForecastDataServices.GetAsync(guid);

                var feedbackDto = _mapper.Map<WeatherForecastDto>(feedback);

                return operation.Build(feedbackDto);
            }
            catch (Exception ex)
            {
                return operation.Build(null, ex.Message, false, OperationErrorType.Technical);
            }
        }

        public async Task<OperationResult<Guid>> SaveAsync(WeatherForecastDto weatherForecastDto)
        {
            var operation = new OperationResult<Guid>();
            try
            {
                if (weatherForecastDto != null && weatherForecastDto.TemperatureC <= 0)
                {
                    await _notificationsService.WeatherAlertAsync("Attention risque de gel", 0, DateTime.UtcNow);
                }

                Guid guid = await _weatherForecastDataServices.AddOrUpdateAsync(_mapper.Map<WeatherForecast>(weatherForecastDto));

                return operation.Build(guid);
            }
            catch (Exception ex)
            {
                return operation.Build(Guid.Empty, ex.Message, false, OperationErrorType.Technical);
            }
        }

        public async Task<Operation> DeleteAsync(Guid guid)
        {
            var operation = new Operation();
            try
            {
                if (guid == Guid.Empty)
                {
                    return operation.Build("Guid should not be null or empty", false, OperationErrorType.Functional);
                }

                await _weatherForecastDataServices.DeleteAsync(guid);

                return operation.Build();
            }
            catch (Exception ex)
            {
                return operation.Build(ex.Message, false, OperationErrorType.Technical);
            }
        }
    }
}