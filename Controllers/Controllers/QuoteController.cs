using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Controllers.QuoteController
{
    [Route("api/[controller]/")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class QuoteController : Controller
    {
        private readonly IQuoteServices _QuoteServices;

        public QuoteController(IQuoteServices quoteServices)
        {
            _QuoteServices = quoteServices;
        }

        [HttpGet("{offset}/{pageLimit}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Get(int offset, int pageLimit)
        {
            var response = await _QuoteServices.Get(offset, pageLimit);

            // Найдены некоторые Users
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Вернуть response 200
                return Ok(response.Data.ToList());
            }

            // 0 найдено
            // Вернуть response 204
            return NoContent();
        }

        [HttpGet("/GetRand")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetRand()
        {
            var response = await _QuoteServices.GetRand();

            // Найдены некоторые Users
            if (response.StatusCode == DataBase.StatusCodes.Ok)
            {
                // Вернуть response 200
                return Ok(response.Data);
            }

            // Цитат нет
            // Вернуть response 404
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Create(string quoteText)
        {
            await _QuoteServices.Create(quoteText);
            return Created();
        }
    }
}
