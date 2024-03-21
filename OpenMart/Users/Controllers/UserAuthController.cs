using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OpenMart.Data.Context;
using OpenMart.Users.Data.Model.DTOs;
using OpenMart.Users.Data.Model.DTOs.Auth;

namespace OpenMart.Users.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{v:apiVersion}/[controller]")]
public class UserAuthController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly OpenMartDbContext _openMartDbContext;
    private readonly IConfiguration _config;
    
    public UserAuthController(IMapper mapper, OpenMartDbContext openMartDbContext, IConfiguration config)
    {
        _mapper = mapper;
        _openMartDbContext = openMartDbContext;
        _config = config;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegistrationRequest request)
    {
        var user = _mapper.Map<UserRegistrationRequest, Data.Model.User>(request);
        var (hash, salt) = HashPassword(request.Password);
        user.PasswordHash = hash;
        user.PasswordSalt = salt;
        await _openMartDbContext.Users.AddAsync(user);
        await _openMartDbContext.SaveChangesAsync();
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
    {   
        var user = await _openMartDbContext.Users.FirstOrDefaultAsync(
            u => u.Email == request.UsernameOrEmail || u.Username == request.UsernameOrEmail);
        
        if (user is not { CanLogin: true })
        {
            return Unauthorized();
        }
        
        var isPasswordValid = ValidatePassword(request.Password, user.PasswordHash, user.PasswordSalt);

        if (isPasswordValid)
        {
            user.ResetLockTracking();
            await _openMartDbContext.SaveChangesAsync();
            return Ok();
        }
        
        // TODO: Get max tries parameter
        user.UpdateLockTracking(5);
        await _openMartDbContext.SaveChangesAsync();
        return Unauthorized();
    }
    
    private (string Hash, string Salt) HashPassword(string password)
    {
        var iterations = int.Parse(_config["PasswordHasher:Iterations"] ?? "100000"); 
        var saltByteSize = int.Parse(_config["PasswordHasher:saltBytes"] ?? "32");
        var hashByteSize = int.Parse(_config["PasswordHasher:hashBytes"] ?? "64");

        var passwordHasher = new OpenMart.Crypto.PasswordHasher();
        var salt = passwordHasher.CreateSalt(saltByteSize);
        var saltString = Convert.ToBase64String(salt);
        var hash = passwordHasher.PBKDF2_SHA256_GetHash(password, saltString, iterations, hashByteSize);
        return (hash, saltString);
    }

    private bool ValidatePassword(string password, string hash, string salt)
    {
        var iterations = int.Parse(_config["PasswordHasher:Iterations"] ?? "100000");
        var hashByteSize = int.Parse(_config["PasswordHasher:hashBytes"] ?? "64");
        
        var passwordHasher = new OpenMart.Crypto.PasswordHasher();
        return passwordHasher.ValidatePassword(password, salt, iterations, hashByteSize, hash);
    }
}