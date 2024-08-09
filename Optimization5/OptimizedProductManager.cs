using Microsoft.Data.SqlClient;
using System.Data;

namespace Optimization5;

public class OptimizedProductManager
{
    private const string PRODUCT_BASE_QUERY = "SELECT * FROM Products";
    private readonly IDbConnection dbConnection;

    public OptimizedProductManager(IDbConnection dbConnection)
    {
        this.dbConnection = dbConnection;
    }

    public List<Product> GetAllProducts()
    {
        return GetProducts(PRODUCT_BASE_QUERY);
    }

    public List<Product> GetProductByName(string productName)
    {
        var query = $"{PRODUCT_BASE_QUERY} WHERE Name = @Name";
        var parameters = new Dictionary<string, object> { { "@Name", productName } };

        return GetProducts(query, parameters);
    }

    // why not async
    private List<Product> GetProducts(string query, IDictionary<string, object> parameters = null)
    {
        var products = new List<Product>();
        try
        {
            dbConnection.Open();

            using IDbCommand command = dbConnection.CreateCommand();
            command.CommandText = query;
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    var dbParameter = command.CreateParameter();
                    dbParameter.ParameterName = parameter.Key;
                    dbParameter.Value = parameter.Value;
                    command.Parameters.Add(dbParameter);
                }
            }

            using IDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Price = reader.GetDecimal(reader.GetOrdinal("Price"))
                });
            }
        }
        finally
        {
            dbConnection.Close();
        }
        
        return products;
    }
}

