namespace DataBase;

// Entity Class Quote
public class Quote
{
    public int? Id { get; set; }
    public string QuoteText { get; set; }
    public int UserId { get; set; }
    public string CreationDate { get; set; }
}
