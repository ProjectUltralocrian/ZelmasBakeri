using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using ZelmasBakeriBackend.Models;

namespace ZelmasBakeriBackend.DataAccess;

public class SqliteConnector : IDbAccess
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public SqliteConnector(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DevelopmentDb") ?? "Data Source=ZelmasBakeri.db";
    }

    public async Task<List<Cake>> GetAllCakes()
    {
        using IDbConnection conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cakes = await conn.QueryAsync<Cake>("SELECT * FROM kaker");
        return cakes.ToList();
    }

    public async Task<List<Order>> GetAllOrderDetails()
    {

        using IDbConnection conn = new SqliteConnection(_connectionString);
        conn.Open();
        var sql = @"SELECT
                    o.Id,
                    o.CustomerId,
                    c.name as CustomerName,
                    c.email as CustomerEmail,
                    GROUP_CONCAT(ol.CakeId) AS CakeIdsString,
                    o.Date,
                    o.Comments
                FROM orders o
                INNER JOIN orderlines ol ON o.Id = ol.Id
                INNER JOIN customers c ON c.Id = o.CustomerId
                INNER JOIN kaker k ON k.id = ol.CakeId
                GROUP BY o.Id, o.CustomerId, o.Date, o.Comments
                ORDER BY o.Date;";
        
        var orders = await conn.QueryAsync<Order>(sql);

        foreach (var order in orders) {
            List<long> ids = new();
            foreach (var i in order.CakeIdsString.Split(','))
            {
                ids.Add(Int64.Parse(i));
            }
            foreach (var cakeId in ids) {
                var cake = await GetCakeById(cakeId);
                if (cake is not null) order.Cakes.Add(cake);
            }
        }
        return orders.ToList();
    }

    public async Task<Cake?> GetCakeById(long id)
    {
        using IDbConnection conn = new SqliteConnection(_connectionString);
        conn.Open();
        var cakes = await conn.QueryAsync<Cake>("SELECT * FROM kaker WHERE id=@Id", new { @Id = id });
        try
        {
            return cakes.ToList().First();
        }
        catch (Exception _)
        {
            return null;
        }
    }

    public async Task<Customer?> GetCustomerByEmail(string email)
    {
        using IDbConnection conn = new SqliteConnection(_connectionString);
        conn.Open();
        var customers = await conn.QueryAsync<Customer>("SELECT * FROM customers WHERE email=@Email", new { @Email = email });
        try
        {
            return customers.ToList().First();
        }
        catch (Exception _)
        {
            return null;
        }
    }

    public async Task RegisterCustomer(Customer customer)
    {
        using IDbConnection conn = new SqliteConnection(_connectionString);
        conn.Open();
        var sql = @"INSERT INTO customers (name, email) VALUES (@Name, @Email); SELECT last_insert_rowid();";
        long customerID = await conn.ExecuteScalarAsync<long>(sql, new { Name = customer.Name, Email = customer.Email });
        customer.Id = customerID;
    }

    public async Task RegisterOrder(Order order)
    {
        using IDbConnection conn = new SqliteConnection(_connectionString);
        conn.Open();
        var sql = @"INSERT INTO orders (CustomerId, Date, Comments) VALUES (@CustomerId, @Date, @Comments);
                SELECT last_insert_rowid();";
        long orderId = await conn.ExecuteScalarAsync<long>(sql, new { CustomerId = order.CustomerId, OrderDate = order.Date, Comments = order.Comments });
        order.Id = orderId;
        foreach (var cakeId in order.CakeIds)
        {
            sql = @"INSERT INTO orderlines (OrderID, CakeID, Quantity) VALUES (@Id, @CakeId, @Quantity);";
            conn.Execute(sql, new { OrderId = orderId, CakeId = cakeId, Quantity = 1 });
        }
    }    
}