namespace ZelmasBakeriBackend.Models;

public class Cake(Int64 id, string name, Int64 price, string image, string description)
{
    public Int64 Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Description { get; set; } = description; 
    public string Image { get; set; } = image; 
    public Int64 Price { get; set; } = price;
}
