using DataBase;
namespace Context;

// Класс QuoteRepository
public class QuoteRepository : BaseRepository<Quote>, IQuoteRepository
{
    private readonly TemplateDbContext Db;
    public QuoteRepository(TemplateDbContext db) : base(db)
    {
        Db = db;
    }
}
