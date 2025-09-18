namespace ZelmasBakeriBackend.Models;

public class Order
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public DateTime OrderDate { get; set; } = new();
    public string CakeIdsString { get; set; } = "";
    public string CakeNamesString { get; set; } = "";
    public string? Comments { get; set; }

    public HashSet<Cake> Cakes { get; set; } = [];

    
    // Computed property to convert CakeIds to List<int>
    public List<long> CakeIdList => CakeIdsString
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        .Select(id => Int64.Parse(id.Trim()))
        .ToList() ?? [];

    public List<string> CakeNamesList => CakeNamesString
        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //.Select(id => int.Parse(id.Trim()))
        .ToList() ?? [];

}