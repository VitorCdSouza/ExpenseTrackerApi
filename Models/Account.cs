using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Account
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public float Balance { get; set; }
    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; } = null!;

    [JsonIgnore]
    public List<Transaction> Transactions { get; set; } = new();
}
