namespace Onyx.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class WeatherForecastsController : ControllerBase
    {
        public WeatherForecastsController()
        {
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<WeatherForecastDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<WeatherForecastDto> mockList = new List<WeatherForecastDto>()
            {
                new WeatherForecastDto()
                {
                     TemperatureC = 19,
                     City = "Paris"
                },
                new WeatherForecastDto()
                {
                     TemperatureC = 24,
                     City = "Rennes"
                }
            };

            return Ok(mockList);
        }
    }
}