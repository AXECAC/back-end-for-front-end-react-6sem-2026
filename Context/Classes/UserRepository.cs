using DataBase;
namespace Context;

// Класс UserRepository
public class UserRepository(TemplateDbContext db) : BaseRepository<User>(db), IUserRepository
{
    private readonly TemplateDbContext Db = db;
}
