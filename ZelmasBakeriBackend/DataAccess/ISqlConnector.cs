using ZelmasBakeriBackend.Models;

namespace ZelmasBakeriBackend.DataAccess;

public interface IDbAccess
{
    Task<List<Cake>> GetAllCakes();

    Task<Cake?> GetCakeById(long id);

    Task<Customer?> GetCustomerByEmail(string email);

    Task RegisterOrder(Order order);
    Task RegisterCustomer(Customer customer);
    Task<List<Order>> GetAllOrderDetails();
}