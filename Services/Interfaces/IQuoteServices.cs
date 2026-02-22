using DataBase;

namespace Services.Interfaces;

// Interface IQuoteServices
public interface IQuoteServices
{
    Task<IBaseResponse<IEnumerable<Quotes>>> Get(int offset, int pageLimit);
    Task<IBaseResponse<Quotes>> GetRand();
    Task<IBaseResponse> Create(string quoteText);
}
