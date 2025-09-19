using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZelmasBakeriBackend.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace ZelmasBakeriBackend.DataAccess;

public class SqlServerConnector : IDbAccess
{
    private readonly IConfiguration _config;
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

    public async Task<Cake?> GetCakeById(long id)
    {
        using IDbConnection conn = new SqlConnection(_connectionString);

        var cake = await conn.QueryAsync<Cake>(
            "GetCakeById",
            new { Id = id },
            commandType: CommandType.StoredProcedure);
        
        return cake.First();
    }

    public Task<Customer?> GetCustomerByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task RegisterCustomer(Customer customer)
    {
        throw new NotImplementedException();
    }

    public Task RegisterOrder(Order order)
    {
        throw new NotImplementedException();
    }
}
