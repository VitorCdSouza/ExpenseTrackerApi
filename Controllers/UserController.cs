using System.Runtime.InteropServices;
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

    [HttpPost]
    public async Task<ActionResult<User>> CreateUser(CreateUserDto dto)
    {
        if (dto.Email == null || dto.PasswordHash == null || dto.Name == null) {
            return BadRequest("Campos não podem ser nulos.");
        }
        User user = new User
        {
            Email = dto.Email,
            PasswordHash = dto.PasswordHash
        };
        Account acc = new Account
        {
            Name = dto.Name,
            User = user,
        };

        user.Account = acc;
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }
}
