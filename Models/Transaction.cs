using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public float Amount { get; set; }
    public string Type { get; set; } = "expense"; // "income" ou "expense"
    
    [Required]
    public int AccountId { get; set; }
    public Account ?Account { get; set; }
}
