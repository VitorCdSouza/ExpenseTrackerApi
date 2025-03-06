using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
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
        return await _context.Transactions.Include(a => a.Account).Where(t => t.AccountId == accountId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == transaction.AccountId);
        if (account == null)
        {
            return BadRequest("Account not found");
        }

        transaction.Account = account;

        if (transaction.Type == "income")
        {
            account.Balance += transaction.Amount;
        }
        else
        {
            account.Balance -= transaction.Amount;
        }

        _context.Transactions.Add(transaction);
        _context.Accounts.Update(account);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTransactions), new { accountId = transaction.AccountId }, transaction);
    }

    [HttpDelete("{transactionId}")]
    public async Task<ActionResult<Transaction>> DeleteTransaction(int transactionId)
    {
        Transaction? transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == transactionId);
        if (transaction == null)
        {
            return BadRequest("Transaction does not exist");
        }

        Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == transaction.AccountId);
        if (account == null)
        {
            return BadRequest("Transaction without account");
        }

        if (transaction.Type == "income")
        {
            account.Balance -= transaction.Amount;
        }
        else
        {
            account.Balance += transaction.Amount;
        }

        _context.Accounts.Update(account);
        _context.Transactions.Remove(transaction);
        await _context.SaveChangesAsync();
        return Ok(transaction);
    }
}
