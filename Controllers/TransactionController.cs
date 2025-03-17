using ExpenseTrackerApi.Controllers;
using ExpenseTrackerApi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TransactionController : BaseController
{
    private readonly ExpenseTrackerContext _context;

    public TransactionController(ExpenseTrackerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
    {
        int userId = GetUserIdToken();

        return await _context.Transactions.Include(a => a.Account).Where(t => t.AccountId == userId).ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Transaction>> CreateTransaction(Transaction transaction)
    {
        int userId = GetUserIdToken();

        Account? account = await _context.Accounts.FirstOrDefaultAsync(a => a.UserId == userId);
        if (account == null)
        {
            return BadRequest("Account not found");
        }

        transaction.AccountId = account.Id;
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
        int userId = GetUserIdToken();

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

        if (account.UserId != userId) {
            return BadRequest("Operation invalid");
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
