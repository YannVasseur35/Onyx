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

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<WeatherForecastDto>))]
        public async Task<IActionResult> GetAsync()
        {
            var dtos = await _weatherForecastAppServices.GetAllAsync();

            return Ok(dtos);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherForecastDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByIdAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }
#pragma warning disable S2583
            var operation = await _weatherForecastAppServices.GetByIdAsync(id ?? Guid.Empty);
#pragma warning restore S2583
            if (operation.IsOperationSuccess)
            {
                if (operation.Model != null)
                {
                    return Ok(operation.Model);
                }
                else
                {
                    return NoContent();
                }
            }

            return BadRequest(operation.ErrorMessage);
        }

        [HttpGet]
        [Route("/city/{city}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OpenWeatherMapResponseDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetByCyAsync(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return BadRequest();
            }
#pragma warning disable 86022
            OpenWeatherMapResponseDto? res = await _weatherForecastAppServices.GetWeatherForecast(city);

            return Ok(res);
        }

        [HttpPost]
        [HttpPut]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostWeatherForecastDtoAsync(WeatherForecastDto? weatherForecastDto)
        {
            if (weatherForecastDto == null)
            {
                return BadRequest();
            }

            var operation = await _weatherForecastAppServices.SaveAsync(weatherForecastDto);

            if (operation.IsOperationSuccess)
            {
                return Ok(operation.Model);
            }
            else
            {
                return BadRequest(operation.ErrorMessage);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteWeatherForecastDtoAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest();
            }
#pragma warning disable S2583
            var operation = await _weatherForecastAppServices.DeleteAsync(id ?? Guid.Empty);
#pragma warning restore S2583
            if (operation.IsOperationSuccess)
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
    }
}