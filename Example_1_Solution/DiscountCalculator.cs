namespace Example_1_Solution;

public static class DiscountCalculator
{
    public static decimal Calculate(Customer customer)
    {
        return customer.IsActive ? 0.1M : 0;
    }
}