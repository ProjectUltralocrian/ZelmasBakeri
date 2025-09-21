namespace ZelmasBakeriBackend.Models;

public class Review(long id, string? name, string message, DateTime date)
{
    public Review() : this(0, null, String.Empty, DateTime.Now) { }
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Message { get; set; } = message;
    public DateTime CreatedAt { get; set; } = date;
    public bool Visible { get; set; } = true;
}
