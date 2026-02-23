using Context;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services.Implementations;

// Class QuoteServices
public class QuoteServices : IQuoteServices
{
    private readonly IQuoteRepository _QuoteRepository;
    private readonly IUserServices _UserServices;

    public QuoteServices(IQuoteRepository quoteRepository, IUserServices userServices)
    {
        _QuoteRepository = quoteRepository;
        _UserServices = userServices;
    }

    public async Task<IBaseResponse<IEnumerable<Quotes>>> Get(int offset, int pageLimit)
    {
        IBaseResponse<IEnumerable<Quotes>> baseResponse;

        var quotes = await _QuoteRepository.GetQueryable().Skip(offset).Take(pageLimit).ToListAsync();

        if (!quotes.Any())
        {
            baseResponse = BaseResponse<IEnumerable<Quotes>>.NoContent();
            return baseResponse;
        }

        var result = new List<Quotes>();

        foreach (var q in quotes)
        {
            var user = (await _UserServices.GetUser(q.UserId)).Data;

            var quote = new Quotes
            {
                QuoteText = q.QuoteText,
                CreationDate = q.CreationDate,
                Username = user.SecondName + " " + user.FirstName,
            };

            result.Add(quote);
        }

        baseResponse = BaseResponse<IEnumerable<Quotes>>.Ok(result);
        return baseResponse;
    }

    public async Task<IBaseResponse<Quotes>> GetRand()
    {
        IBaseResponse<Quotes> baseResponse;
        var randQuote = await _QuoteRepository.GetQueryable().OrderBy(e =>
                Guid.NewGuid()).FirstOrDefaultAsync();

        if (randQuote == null) {
            baseResponse = BaseResponse<Quotes>.NotFound("User not found");
            return baseResponse;
        }

        var user = (await _UserServices.GetUser(randQuote.UserId)).Data;
        var quote = new Quotes
        {
            QuoteText = randQuote.QuoteText,
            CreationDate = randQuote.CreationDate,
            Username = user.SecondName + " " + user.FirstName,
        };

        baseResponse = BaseResponse<Quotes>.Ok(quote);
        return baseResponse;
    }

    public async Task<IBaseResponse> Create(string quoteText)
    {
        IBaseResponse baseResponse;

        var userId = _UserServices.GetMyId();

        var quote = new Quote
        {
            QuoteText = quoteText,
            UserId = userId,
            CreationDate = DateTime.Today.ToString("dd.MM.yyyy")
        };

        await _QuoteRepository.Create(quote);
        baseResponse = BaseResponse.Created();

        return baseResponse;

    }

}
