using Optimization2_Fixed.Exceptions;

namespace Optimization2_Fixed;
public class Product
{
    private int stock;


    public int Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public List<string> Tags { get; private set; }

    public Product(int id, string name, decimal price, int stock, List<string> tags)
    {
        if (id <= 0) throw new InvalidProductException("Product ID must be greater than 0.");
        if (string.IsNullOrWhiteSpace(name)) throw new InvalidProductException("Product name cannot be empty.");
        if (price < 0) throw new InvalidProductException("Product price cannot be negative.");
        if (stock < 0) throw new InvalidProductException("Product stock cannot be negative.");

        Id = id;
        Name = name;
        Price = price;
        this.stock = stock;
        Tags = tags ?? new List<string>();
    }

    public void UpdateStock(int newStock)
    {
        if (newStock < 0) 
            throw new InvalidOperationException("Stock cannot be negative.");

        stock = newStock;
    }

    public int GetStock() => stock;
}
