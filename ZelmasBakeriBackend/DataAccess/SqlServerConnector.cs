using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using ZelmasBakeriBackend.Models;

namespace ZelmasBakeriBackend.DataAccess;

public class SqlServerConnector : IDbAccess
{
    private readonly IConfiguration? _config;
    private readonly string _connectionString;

    public SqlServerConnector(IConfiguration config)
    {
        _config = config;
        _connectionString = _config?.GetConnectionString("SqlServerDb") ?? "";
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
        var orders = (await conn.QueryAsync<Order>(
            "GetAllOrderDetails",
            commandType: CommandType.StoredProcedure)).ToList();

        foreach (var order in orders)
        {
            var ids = order.CakeIdsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
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
            commandType: CommandType.StoredProcedure);
        return reviews.ToList();
    }

    public async Task<Cake?> GetCakeById(long id)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);

        var cake = await conn.QueryAsync<Cake>(
            "GetCakeById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);

        return cake.FirstOrDefault();
    }

    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var customer = await conn.QueryAsync<Customer>(
            "GetCustomerByEmail", new { @Email = email }, commandType: CommandType.StoredProcedure);
        return customer.FirstOrDefault();
    }

    public async Task RegisterCustomer(Customer customer)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var id = await conn.ExecuteScalarAsync<long>("RegisterCustomer", new { @Name = customer.Name, @Email = customer.Email }, commandType: CommandType.StoredProcedure);
        customer.Id = id;
    }

    public async Task RegisterOrder(Order order)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var id = await conn.ExecuteScalarAsync<long>("RegisterOrder", new { @CustomerId = order.CustomerId, @Date = order.Date, @Comments = order.Comments }, commandType: CommandType.StoredProcedure);
        order.Id = id;

        foreach (var cakeId in order.CakeIds)
        {
            var sql = @"INSERT INTO Orderlines (OrderID, CakeID, Quantity) VALUES (@OrderId, @CakeId, @Quantity);";
            conn.Execute(sql, new { OrderId = order.Id, CakeId = cakeId, Quantity = 1 });
        }
    }

    public async Task RegisterReview(Review review)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);
        var id = await conn.ExecuteScalarAsync<long>("RegisterReview", new { Name = review.Name, Message = review.Message, CreatedAt = review.CreatedAt }, commandType: CommandType.StoredProcedure);
        review.Id = id;
    }
}
