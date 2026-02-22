using Microsoft.EntityFrameworkCore;
using DataBase;
using System.Security.Cryptography.X509Certificates;
namespace Context;

// Класс Context
public class TemplateDbContext : DbContext
{
    public TemplateDbContext(DbContextOptions<TemplateDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Quote> Quotes { get; set; }
}
