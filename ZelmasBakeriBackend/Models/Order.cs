namespace ZelmasBakeriBackend.Models;

using System.Linq;

public class Order
{
    public long OrderId { get; set; }
    public long CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public DateTime OrderDate { get; set; } = new();
    public string? Comments { get; set; }
    public string CakeIdsString { get; set; } = "";

    public HashSet<Cake> Cakes { get; set; } = [];

    public List<long> CakeIds => Cakes.Select(c => c.Id).ToList();

    public List<string> CakeNamesList => Cakes.Select(c => c.Name).ToList();

}