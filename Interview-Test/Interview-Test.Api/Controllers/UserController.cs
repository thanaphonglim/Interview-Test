using Interview_Test.Models;
using Interview_Test.Repositories;
using Interview_Test.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Interview_Test.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private IUserRepository _userRepository;
    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public ActionResult GetUsers()
    {
        var res = _userRepository.GetUsers();
        return Ok(res);
    }

    [HttpGet("GetUserById/{id}")]
    public ActionResult GetUserById(string id)
    {
        var res = _userRepository.GetUserById(id);
        return Ok(res);
    }

    [HttpPost("CreateUser")]
    public ActionResult CreateUser()
    {
        var res = _userRepository.CreateUser();
        return Ok();
    }
}