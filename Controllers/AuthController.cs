using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.User;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
    private readonly IAuthRepository authrepo;

    public  AuthController (IAuthRepository _authrepo){
      authrepo = _authrepo;
    }
    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>>Register(UserRegisterDto userRegisterDto)
    {
        var response = await authrepo.Register(
            new User {Uname = userRegisterDto.Uname}, userRegisterDto.Password
        );
        if(!response.Success){
            return BadRequest(response);

        }
        return Ok(response);
    }

     [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<String>>>Login(UserLoginDto userLoginDto)
    {
        var response = await authrepo.Login(userLoginDto.Uname, userLoginDto.Password );
        if(!response.Success){
            return BadRequest(response);

        }
        return Ok(response);
    }

    }
}