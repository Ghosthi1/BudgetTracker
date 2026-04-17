using CommunityToolkit.Maui.Views;
using CommunityToolkit.Maui.Extensions;

namespace BudgetTracker.Pages.ShowBudgets;

public partial class Budget : ContentPage
{
    private readonly BudgetViewModel _viewModel;

    public Budget(BudgetViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadAsync();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _viewModel.LogoutAsync();
        SecureStorage.Remove("access_token");
        SecureStorage.Remove("refresh_token");
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void OnAddBillClicked(object sender, EventArgs e)
    {
        var popup = new AddBillPopup(bill => _viewModel.AddBillAsync(bill));
        await this.ShowPopupAsync(popup);
    }

    private async void OnRemoveBillClicked(object sender, EventArgs e)
    {
        if (_viewModel.Bills.Count == 0) return;
        var popup = new RemoveBillPopup(_viewModel.Bills, async bill => await _viewModel.DeleteBillAsync(bill));
        await this.ShowPopupAsync(popup);
    }
}