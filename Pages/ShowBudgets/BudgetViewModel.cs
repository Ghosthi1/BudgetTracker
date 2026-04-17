using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using BudgetTracker.Models;
using Supabase;

namespace BudgetTracker.Pages.ShowBudgets;

public partial class BudgetViewModel : ObservableObject
{
    private readonly Client _supabase;

    public ObservableCollection<Bill> Bills { get; } = new();

    public decimal TotalMonthly => Bills.Where(b => b.Frequency == Frequency.Monthly).Sum(b => b.Amount);
    public decimal TotalYearly  => Bills.Where(b => b.Frequency == Frequency.Yearly).Sum(b => b.Amount)
                                   + TotalMonthly * 12;

    public BudgetViewModel(Client supabase)
    {
        _supabase = supabase;
        Bills.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(TotalMonthly));
            OnPropertyChanged(nameof(TotalYearly));
        };
    }

    public string CurrentUserEmail => _supabase.Auth.CurrentUser?.Email ?? string.Empty;

    public async Task LoadAsync()
    {
        var result = await _supabase.From<Bill>().Get();
        Bills.Clear();
        foreach (var bill in result.Models)
            Bills.Add(bill);
    }

    public async Task AddBillAsync(Bill bill)
    {
        bill.UserId = _supabase.Auth.CurrentUser?.Id ?? string.Empty;
        await _supabase.From<Bill>().Insert(bill);
        Bills.Add(bill);
    }

    public async Task LogoutAsync() => await _supabase.Auth.SignOut();

    public async Task DeleteBillAsync(Bill bill)
    {
        await _supabase.From<Bill>().Delete(bill);
        Bills.Remove(bill);
    }
}