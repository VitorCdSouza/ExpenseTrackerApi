using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly ExpenseTrackerContext _context;

    public TransactionController(ExpenseTrackerContext context)
    {
        _context = context;
    }

    [HttpGet("{accountId}")]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions(int accountId)
    {
        return await _context.Transactions.Where(t => t.AccountId == accountId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTransactions), new { accountId = transaction.AccountId }, transaction);
    }
}
