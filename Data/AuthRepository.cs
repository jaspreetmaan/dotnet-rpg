using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_rpg.Data
{
  public class AuthRepository : IAuthRepository
  {
    private readonly DataContext context;
    private readonly IConfiguration configuration;

    public AuthRepository (DataContext _context, IConfiguration _configuration){
        context = _context;
      configuration = _configuration;
    }
    public async Task<ServiceResponse<string>> Login(string username, string password)
    {
        var response = new ServiceResponse<string>();
        var user = await context.Users
        .FirstOrDefaultAsync(u => u.Uname.ToLower().Equals( username.ToLower()));

        if(user == null)
        {
            response.Success =false;
            response.Message ="User not found";
        }
        else if(!verifyPasswordHash(password, user.PasswordHash,user.PasswordSalt)){
            response.Success =false;
            response.Message = "Wrong Password";

        }
        else
        {
            response.Data = CreateToken(user);
        }
        return response;
    }

    public async Task<ServiceResponse<int>> Register(User user, string password)
    {
                ServiceResponse <int> response = new ServiceResponse<int>();
        CreatePasswordHash(password,out byte[] passwordHash, out byte[] passwordSalt);

            if(await UserExists(user.Uname)){
                response.Success =false;
                response.Message ="User already Exists";
            }
        user.PasswordHash = passwordHash;
        user.PasswordSalt=passwordSalt;
        context.Users.Add(user);
        await context.SaveChangesAsync();

        response.Data = user.id;

        return response;
    }

    public async Task<bool> UserExists(string username)
    {
        if(await context.Users.AnyAsync(u => u.Uname.ToLower() == username.ToLower())){
            return true;
        }
        return false;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordsalt){
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordsalt =hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

        }
    }
    private bool verifyPasswordHash(string password , byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
        {
            var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computeHash.SequenceEqual(passwordHash);        }
    }
    private string CreateToken(User user){
        List<Claim> claims = new List<Claim>{
            new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
            new Claim(ClaimTypes.Name,user.Uname)
        };
        SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
        .GetBytes(configuration.GetSection("AppSettings:Token").Value));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials =creds

        };
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken token =tokenHandler.CreateToken(tokenDescriptor);


        return tokenHandler.WriteToken(token);
    }
  }
}