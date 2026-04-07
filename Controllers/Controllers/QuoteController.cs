using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Controllers.QuoteController
{
    [Route("api/[controller]/")]
    [Authorize]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class QuoteController(IQuoteServices quoteServices) : Controller
    {
        private readonly IQuoteServices _QuoteServices = quoteServices;

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

        [HttpGet("GetRand")]
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

        [HttpGet("TotalQuotes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> TotalQuotes()
        {
            var response = await _QuoteServices.HowMuchQuotes();
            return Ok(response.Data);
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
