public class Report
{
    public int Id { get; set; }
    public float TotalIn { get; set; }
    public float TotalOut { get; set; }
    public float CurrentBalance { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; } = null!;
}
