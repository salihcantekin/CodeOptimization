using Optimization3.Models;

namespace Optimization3.Services;

public class UserService
{
    public bool CreateUser(CreateUserRequestModel user)
    {
        // Saving to Storage

        var created = true; // or false, depending on the result
        return created;
    }

    public IEnumerable<UserViewModel> GetUsersAsync()
    {
        // Getting from Storage (Relational Db, Cache, NoSql etc.)
        return new List<UserViewModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Salih",
                LastName = "Cantekin",
                EmailAddress = "salihcantekin@gmail.com"
            },
            new()
            {
                Id = Guid.NewGuid(),
                FirstName = "Salih Can",
                LastName = "tekin",
                EmailAddress = "salihcan_tekin@gmail.com"
            },
        };
    }   
}