using ZelmasBakeriBackend.Models;

namespace ZelmasBakeriBackend.DataAccess;

public interface IDbAccess
{
    /// <summary>
    /// Seeds the database with information about available cakes. This should probably be done even in production.
    /// </summary>
    Task SeedCakes();

    /// <summary>
    /// Seeds the database with dummy customer and order data for development purposes.
    /// </summary>
    Task SeedDbWithDummyData();

    Task<List<Cake>> GetAllCakes();

    Task<Cake?> GetCakeById(long id);

    Task<Customer?> GetCustomerByEmail(string email);

    Task RegisterOrder(Order order);
    Task RegisterCustomer(Customer customer);
    Task<List<Order>> GetAllOrderDetails();

    Task<List<Review>> GetAllReviews();
    Task RegisterReview(Review review);
}
