using CommunityToolkit.Maui.Views;
using BudgetTracker.Models;

namespace BudgetTracker.Pages.ShowBudgets;

public partial class RemoveBillPopup : Popup
{
    private readonly Action<Bill> _onBillRemoved;
    private Bill? _selected;

    public RemoveBillPopup(IEnumerable<Bill> bills, Action<Bill> onBillRemoved)
    {
        InitializeComponent();
        _onBillRemoved = onBillRemoved;
        BillsList.ItemsSource = bills;
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _selected = e.CurrentSelection.FirstOrDefault() as Bill;
    }

    private async void OnCancelClicked(object sender, EventArgs e) => await CloseAsync();

    private async void OnRemoveClicked(object sender, EventArgs e)
    {
        if (_selected is null) return;
        _onBillRemoved(_selected);
        await CloseAsync();
    }
}