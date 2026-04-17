using Supabase;

namespace BudgetTracker;

public partial class MainPage : ContentPage
{
    private readonly Client _supabase;

    public MainPage(Client supabase)
    {
        InitializeComponent();
        _supabase = supabase;
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        try
        {
            await _supabase.Auth.SignIn(EmailEntry.Text, PasswordEntry.Text);
            await Shell.Current.GoToAsync("bills");
        }
        catch (Exception ex)
        {
            ErrorLabel.Text = ex.Message;
            ErrorLabel.IsVisible = true;
        }
    }

    private async void OnNextPageClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("bills");
    }
}