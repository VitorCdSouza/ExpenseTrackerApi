public class Transaction
{
    public int Id { get; set; }
    public float Amount { get; set; }
    public string Type { get; set; } = "expense"; // "income" ou "expense"
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}
