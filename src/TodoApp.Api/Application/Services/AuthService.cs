using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TodoApp.Api.Application.Common.Utilities;
using TodoApp.Api.Application.Interfaces;
using TodoApp.Api.Domain.Entities;

namespace TodoApp.Api.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserService _user;
    private readonly IConfiguration _config;
    public AuthService(IUserService user, IConfiguration config)
    {
        _user = user;
        _config = config;
    }
    public async Task<string> Login(string? authorizationString)
    {
        // Login logic here
        // Get Authorazation header via HttpContext
        if (authorizationString == null )
        {
            throw new InvalidCredentialException();
        }

        int stringIndex = authorizationString.IndexOf("Basic ") + "Basic ".Length;
        string? loginStringBase64 = authorizationString.Substring(stringIndex);

        // Build the login string
        string? loginString = null;
        if (loginStringBase64 != null)
        {
            byte[] loginStringBytes = Convert.FromBase64String(loginStringBase64);
            loginString = Encoding.UTF8.GetString(loginStringBytes);
        }

        // Setup the array for username and password
        string[] login = [];
        if (loginString != null)
        {
            login = loginString.Split(':');
        }

        if (login.Length != 2)
        {
            throw new InvalidCredentialException();
        }
        
        Console.WriteLine(loginString);
        // Try to login
        User user = await _user.GetUserByNameAsync(login[0]);

        if (!PasswordHasher.Verify(login[1], user.Password))
        {
            throw new InvalidCredentialException();
        }

        string? key = _config["Jwt:Key"] ?? throw new InvalidCredentialException();

        // Genarate jwt token
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Issuer"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
