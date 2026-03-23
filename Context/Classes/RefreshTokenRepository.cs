using DataBase;
namespace Context;

// Класс RefreshTokenRepository
public class RefreshTokenRepository(TemplateDbContext db) : BaseRepository<RefreshToken>(db), IRefreshTokenRepository
{
    private readonly TemplateDbContext Db = db;
}
