using Microsoft.AspNetCore.Mvc;
using Optimization3.Models;
using Optimization3.Services;

namespace Optimization3.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService userService;

    public UserController(UserService userService)
    {
        this.userService = userService;
    }

    [HttpGet()]
    public ActionResult<IEnumerable<UserViewModel>> GetUsers()
    {
        var users = userService.GetUsersAsync();

        return Ok(users);
    }

    [HttpPost]
    public ActionResult<bool> CreateUser([FromBody] UserViewModel user)
    {
        var isUserCreated = userService.CreateUser(user);

        return Ok(isUserCreated);
    }
}
