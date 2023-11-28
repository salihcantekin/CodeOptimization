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

    [HttpGet("GetUsers")]
    public ActionResult<List<UserViewModel>> GetUsers()
    {
        var users = userService.GetUsersAsync().ToList();

        return Ok(users);
    }

    [HttpPost("CreateUser")]
    public void CreateUser([FromQuery] UserViewModel user)
    {
        _ = userService.CreateUser(user);
    }
}
