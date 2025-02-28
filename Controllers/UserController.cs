using System.Runtime.InteropServices;
using ExpenseTrackerApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ExpenseTrackerContext _context;

    public UserController(ExpenseTrackerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.Include(u => u.Account).ToListAsync();
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<User>> GetUserById(int userId)
    {
        User? user = await _context.Users.Include(a => a.Account).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            return BadRequest("User does not exist");
        }

        return Ok(user);
    }

    [HttpPost("Register")]
    public async Task<ActionResult<User>> RegisterUser(CreateUserDto dto)
    {
        if (dto.Email == null || dto.Password == null || dto.Name == null)
        {
            return BadRequest("Empty fields");
        }

        User user = new User
        {
            Email = dto.Email,
            Password = dto.Password
        };

        Account acc = new Account
        {
            Name = dto.Name,
            User = user,
        };

        Encryption.CreatePasswordHash(user.Password, out byte[] hash, out byte[] salt);
        user.Password = string.Empty;
        user.PasswordHash = hash;
        user.PasswordSalt = salt;

        user.Account = acc;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<User>> LoginUser(AuthUserDto dto)
    {
        if (dto.Email == null || dto.Password == null)
        {
            return BadRequest("Empty fields");
        }

        User? user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
        {
            return BadRequest("Invalid credentials");
        }
        else if (!Encryption.VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
        {
            return BadRequest("Invalid credentials");
        }

        return Ok("Foi");
    }
}
