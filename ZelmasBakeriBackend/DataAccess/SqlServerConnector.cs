using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using ZelmasBakeriBackend.Models;

namespace ZelmasBakeriBackend.DataAccess;

public class SqlServerConnector : IDbAccess
{
    private readonly IConfiguration? _config;
    private readonly string _connectionString;
    private readonly ILogger<SqlServerConnector> _logger;

    public SqlServerConnector(IConfiguration config, ILogger<SqlServerConnector> logger)
    {
        _config = config;
        _connectionString = _config?.GetConnectionString("SqlServerDb") ?? "";
        _logger = logger;
    }

    public async Task<List<Cake>> GetAllCakes()
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        conn.Open();
        Console.WriteLine(conn.State);
        //await Task.Delay(1);
        var cakes = await conn.QueryAsync<Cake>("SELECT * FROM Cakes");

        return cakes.ToList();
    }

    public async Task<List<Order>> GetAllOrderDetails()
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var orders = (
            await conn.QueryAsync<Order>(
                "GetAllOrderDetails",
                commandType: CommandType.StoredProcedure
            )
        ).ToList();

        foreach (var order in orders)
        {
            var ids = order
                .CakeIdsString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(id => Convert.ToInt64(id))
                .ToList();

            foreach (var cakeId in ids)
            {
                var cake = await GetCakeById(cakeId);
                if (cake is not null)
                    order.Cakes.Add(cake);
            }
        }

        return orders;
    }

    public async Task<List<Review>> GetAllReviews()
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var reviews = await conn.QueryAsync<Review>(
            "GetAllReviews",
            commandType: CommandType.StoredProcedure
        );
        return reviews.ToList();
    }

    public async Task<Cake?> GetCakeById(long id)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);

        var cake = await conn.QueryAsync<Cake>(
            "GetCakeById",
            new { Id = id },
            commandType: CommandType.StoredProcedure
        );

        return cake.FirstOrDefault();
    }

    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var customer = await conn.QueryAsync<Customer>(
            "GetCustomerByEmail",
            new { @Email = email },
            commandType: CommandType.StoredProcedure
        );
        return customer.FirstOrDefault();
    }

    public async Task RegisterCustomer(Customer customer)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var id = await conn.ExecuteScalarAsync<long>(
            "RegisterCustomer",
            new { customer.Name, customer.Email },
            commandType: CommandType.StoredProcedure
        );
        customer.Id = id;
    }

    public async Task RegisterOrder(Order order)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var id = await conn.ExecuteScalarAsync<long>(
            "RegisterOrder",
            new
            {
                order.CustomerId,
                order.Date,
                order.Comments,
            },
            commandType: CommandType.StoredProcedure
        );
        order.Id = id;

        foreach (var cakeId in order.CakeIds)
        {
            var sql =
                @"INSERT INTO Orderlines (OrderID, CakeID, Quantity) VALUES (@OrderId, @CakeId, @Quantity);";
            conn.Execute(
                sql,
                new
                {
                    OrderId = order.Id,
                    CakeId = cakeId,
                    Quantity = 1,
                }
            );
        }
    }

    public async Task RegisterReview(Review review)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var id = await conn.ExecuteScalarAsync<long>(
            "RegisterReview",
            new
            {
                review.Name,
                review.Message,
                review.CreatedAt,
            },
            commandType: CommandType.StoredProcedure
        );
        review.Id = id;
    }

    public async Task SeedCakes()
    {
        var cakes = await GetAllCakes();
        if (cakes.Count == 0)
        {
            _logger.LogWarning("Seeding cakes");
            cakes = [
                new(1, "Sitraturkake", 400, "Sitratur1.jpeg", "Min signaturkake med sitronkrem og makroner"),
                new(2, "Gulrot kake", 250, "GulrotKake.jpeg", "Min spesialitet"),
                new(3, "Makroner", 100, "Makroner.jpeg", "Makroner i forskjellige farger og smak (mango, karamell, mørk sjokolade, jordbær osv.). 10 stk boks."),
                new(4, "Vanlijekake med karamellkrem", 350, "OnePieceVaniljekake.jpeg", "Til bursdagsfest eller One Piece theme party"),
                new(5, "Swiftie kake", 300, "Swiftie.jpeg", "Når nytt Taylor Swift album slippes <3 - med mørk sjokolade og appelsinkrem"),
                new(6, "Jordbærkake med hvit sjokolade", 250, "JordbærOgHvitsjokolade.jpeg", "Perfekt til 17. mai"),
                new(7, "Snøhvit", 300, "HvitsjokoladeOgBringebær.jpeg", "Hvit sjokolade og bringebær"),
                new(8, "Sjokoladekake med jordbær og lemon curd", 350, "JordbærkakeMedLemonCurdOgMørksjokolade.jpeg", "Deilig blanding av fyldig mørk sjokolade og smakfull tropisk frukt"),
                new(9, "Marsipankake med hvit sjokolade og bringebær", 350, "MarsipankakeMedHvitsjokoladeOgBringebær.jpeg", "Har du kanskje bryllup i helgen?"),
                new(10, "Gulrotkake - stor", 300, "GulrotKakeStor.jpeg", "Kaninenes favoritt"),
                new(11, "Tiramisu", 300, "Tiramisu.jpeg", "Akkurat som italienerne lager den"),
                new(12, "Duolingo kake", 250, "Duolingo.jpeg", "Når du oppnår 1000-dagers streak"),
                new(13, "Ostekake", 250, "Cheesecake1.jpeg", "Klassisk ostekake med bringebærsyltetøy")
            ];
            using IDbConnection conn = new SqlConnection(_connectionString);
            foreach (var cake in cakes)
            {
                var sql = @"INSERT INTO dbo.Cakes (Name, Description, Price, Image) VALUES (@Name, @Description, @Price, @Image)";
                await conn.ExecuteAsync(sql, new { cake.Name, cake.Description, cake.Price, cake.Image });
            }
        }
    }

    /// <inheritdoc />
    public Task SeedDbWithDummyData()
    {
        // TODO implement seeding with dummy data if needed
        return Task.CompletedTask;
    }
}
