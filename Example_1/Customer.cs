namespace Example_1;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }

    public decimal CalculateDiscount()
    {
        return IsActive ? 0.1M : 0;
    }
}