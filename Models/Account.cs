public class Account
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public float Balance { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<Transaction> Transactions { get; set; } = new();
}
