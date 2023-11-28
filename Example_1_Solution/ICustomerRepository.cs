namespace Example_1_Solution;

public interface ICustomerRepository
{
    void Add(Customer customer);
    IEnumerable<Customer> GetAllActive();
}