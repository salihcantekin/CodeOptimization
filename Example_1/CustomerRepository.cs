namespace Example_1;

public interface IRepository<T>
{
    void Add(T item);
    IEnumerable<T> GetAll();
}


public class CustomerRepository : IRepository<Customer>, ICustomerActions
{
    private List<Customer> customers;

    public CustomerRepository()
    {
        customers = new List<Customer>();
    }

    public void Add(Customer customer)
    {
        // Check if duplicate
        if (customers.Any(c => c.Id == customer.Id))
        {
            throw new Exception("Duplicate ID");
        }

        customers.Add(customer);
    }

    public IEnumerable<Customer> GetAll()
    {
        // Assuming this is a database call
        return customers.ToList();
    }

    public void ArchiveCustomer(Customer customer)
    {
        // Archiving customer process
    }
}