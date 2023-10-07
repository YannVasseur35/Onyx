using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Onyx.Application.Interfaces;

namespace Onyx.Web.Api.Controllers
{
    public class PlayerJourneysController : ControllerBase
    {
        private readonly ILogger<PlayerJourneysController> _logger;
        private readonly IPlayerJourneyAppServices _playerJourneyAppServices;

        public PlayerJourneysController(
            ILogger<PlayerJourneysController> logger,
            IPlayerJourneyAppServices playerJourneyAppServices)

        {
            _logger = logger;
            _playerJourneyAppServices = playerJourneyAppServices;
        }

        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PlayerJourneyDto>))]
        public async Task<IActionResult> GetAsync()
        {
            var dtos = await _playerJourneyAppServices.GetAllAsync();

            return Ok(dtos);
        }

        [HttpGet]
        [Route("sessionId")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PlayerJourneyDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetBySessionIdAsync(string id)
        {
            if (id is null)
            {
                return BadRequest();
            }

            var dto = await _playerJourneyAppServices.GetBySessionIdAsync(id);

            return Ok(dto);
        }
    }
}