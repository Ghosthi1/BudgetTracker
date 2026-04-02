namespace BudgetTracker.Models;


public class Bill
{
    public Guid Id { get; set; }
    public string Name { get; set; }        // "Netflix"
    public decimal Amount { get; set; }     // 10.99
    public int DueDay { get; set; }         // 15 (day of month)
    public Frequency Frequency { get; set; } // Monthly or Yearly
    public bool IsPaid { get; set; }        // paid this cycle?
    public string Category { get; set; }   // "Subscriptions", "Utilities" etc
}

public enum Frequency
{
    Monthly,
    Yearly
}