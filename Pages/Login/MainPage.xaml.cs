namespace BudgetTracker;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    private async void OnNextPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("bills");
    }
    
}