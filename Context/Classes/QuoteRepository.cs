using DataBase;
namespace Context;

// Класс QuoteRepository
public class QuoteRepository(TemplateDbContext db) : BaseRepository<Quote>(db), IQuoteRepository
{
    private readonly TemplateDbContext Db = db;
}
