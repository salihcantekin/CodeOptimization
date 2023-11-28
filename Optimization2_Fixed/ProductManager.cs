using Optimization2_Fixed.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization2_Fixed;
public class ProductManager
{
    private readonly List<Product> products = new List<Product>();

    public void AddProduct(Product product)
    {
        if (products.Any(p => p.Id == product.Id))
        {
            throw new InvalidOperationException("Product with the same ID already exists.");
        }

        products.Add(product);
    }

    public void AddProduct(int id, string name, decimal price, int stock, List<string> tags)
    {
        var product = new Product(id, name, price, stock, tags);
        AddProduct(product);
    }

    public Product GetProductById(int id)
    {
        var product = products.FirstOrDefault(p => p.Id == id);
        if (product == null)
        {
            throw new ProductNotFoundException(id);
        }

        return product;
    }

    public List<Product> SearchProducts(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return new List<Product>();

        return products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                                //|| p.Tags.Contains(searchTerm, StringComparer.OrdinalIgnoreCase))
                                || p.Tags.Any(t => t.Equals(searchTerm, StringComparison.OrdinalIgnoreCase)))
                       .ToList();
    }

    public void UpdateStock(int productId, int newStock)
    {
        var product = GetProductById(productId);
        product.UpdateStock(newStock);
    }
}
