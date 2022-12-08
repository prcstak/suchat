using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.Api.Models;
using Chat.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Api.Controllers;

public class AuthController : BaseController
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(       
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    
    [HttpPost]
    [Route("Registration")]
    public async Task<IActionResult> Register(
        [FromForm] RegisterUserRequest addUserDto,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = addUserDto.Username,
            Role = addUserDto.Role
        };
        
        var result = await _userManager.CreateAsync(user, addUserDto.Password);
        if (!result.Succeeded)
            return BadRequest(new { Error = string.Join(" ", result.Errors.Select(s => s.Description).ToList()) });

        return NoContent();
    }
    
    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login(
        [FromForm] UserInfoRequest userInfoRequest,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userInfoRequest.Username);

        if (userInfoRequest.Password.Length > 0)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                userInfoRequest.Username, 
                userInfoRequest.Password,
                false,
                false);

            if (signInResult.Succeeded)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                    new Claim("Role", user.Role.ToString()),
                };
                
                var secretBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);

                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretBytes),
                    SecurityAlgorithms.HmacSha256);

                var from = DateTime.Now;
                var till = DateTime.Now.AddDays(9999);
                
                var token = new JwtSecurityToken(
                    _configuration["JWT:Issuer"],
                    _configuration["JWT:Audience"],
                    claims,
                    from,
                    till,
                    signingCredentials);

                var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new
                {
                    access_token = tokenJson,
                    starts_at = from,
                    expires = till
                });
            }
        }

        return BadRequest(new { Error = "Unable to login with such credentials"});
    }
}