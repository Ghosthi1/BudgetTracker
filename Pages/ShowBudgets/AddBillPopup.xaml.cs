using CommunityToolkit.Maui.Views;
using BudgetTracker.Models;

namespace BudgetTracker.Pages.ShowBudgets;

public partial class AddBillPopup : Popup
{
    private readonly Func<Bill, Task> _onBillAdded;

    public AddBillPopup(Func<Bill, Task> onBillAdded)
    {
        InitializeComponent();
        FrequencyPicker.SelectedIndex = 0;
        _onBillAdded = onBillAdded;
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();

    private async void OnAddClicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NameEntry.Text))
        {
            ErrorLabel.Text = "Name is required.";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (!decimal.TryParse(AmountEntry.Text, out var amount))
        {
            ErrorLabel.Text = "Enter a valid amount.";
            ErrorLabel.IsVisible = true;
            return;
        }

        var frequency = FrequencyPicker.SelectedIndex == 1 ? Frequency.Yearly : Frequency.Monthly;
        var date = DueDayPicker.Date.GetValueOrDefault(DateTime.Today);
        var dueDay = frequency == Frequency.Yearly ? date.DayOfYear : date.Day;

        var bill = new Bill
        {
            Name = NameEntry.Text.Trim(),
            Amount = amount,
            DueDay = dueDay,
            Frequency = frequency,
        };

        try
        {
            await _onBillAdded(bill);
            await CloseAsync();
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
    }
}