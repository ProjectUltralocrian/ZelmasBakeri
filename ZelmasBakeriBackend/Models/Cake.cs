namespace ZelmasBakeriBackend.Models;

public class Cake(Int64 id, string name, Int64 price, string image, string description)
{
    public Cake() : this(0, "", 0, "", "") { }
    public long Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description; 
    public string Image { get; set; } = image; 
    public long Price { get; set; } = price;
}
