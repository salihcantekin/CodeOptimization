namespace Optimization2;
public class ProductManager
{
    private List<Product> products;

    public ProductManager()
    {
        products = new List<Product>();
    }

    public void AddProduct(Product product)
    {
        if (products.Any(p => p.Id == product.Id))
            throw new InvalidOperationException("Product with the same ID already exists.");
        
        products.Add(product);
    }

    public Product GetProductById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            throw new KeyNotFoundException("Product not found.");
        return product;
    }

    public List<Product> SearchProducts(string searchTerm)
    {
        return products.Where(p => p.Name.Contains(searchTerm) || p.Tags.Contains(searchTerm)).ToList();
    }

    public void UpdateStock(int productId, int newStock)
    {
        var product = GetProductById(productId);
        product.Stock = newStock;
    }
}
