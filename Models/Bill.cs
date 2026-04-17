using Newtonsoft.Json;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace BudgetTracker.Models;

[Table("bills")]
public class Bill : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("due_day")]
    public int DueDay { get; set; }

    [Column("frequency")]
    public Frequency Frequency { get; set; }

    [Column("category")]
    public string Category { get; set; } = string.Empty;

    [Column("user_id")]
    public string UserId { get; set; } = string.Empty;

    [JsonIgnore]
    public decimal MonthlyEquivalent => Frequency == Frequency.Monthly ? Amount : Amount / 12;

    [JsonIgnore]
    public decimal YearlyEquivalent  => Frequency == Frequency.Yearly  ? Amount : Amount * 12;
}

public enum Frequency
{
    Monthly,
    Yearly
}