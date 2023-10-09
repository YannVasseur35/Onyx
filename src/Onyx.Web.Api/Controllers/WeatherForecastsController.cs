using Onyx.Application.Interfaces;

namespace Onyx.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class WeatherForecastsController : ControllerBase
    {
        public readonly IWeatherForecastAppServices _weatherForecastAppServices;

        public WeatherForecastsController(IWeatherForecastAppServices weatherForecastAppServices)
        {
            _weatherForecastAppServices = weatherForecastAppServices;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecastDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _weatherForecastAppServices.GetAllWeatherForecasts());
        }
    }
}