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

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var accessToken  = await SecureStorage.GetAsync("access_token");
        var refreshToken = await SecureStorage.GetAsync("refresh_token");

        if (accessToken is null || refreshToken is null) return;

        try
        {
            await _supabase.Auth.SetSession(accessToken, refreshToken);
            if (_supabase.Auth.CurrentUser is not null)
                await Shell.Current.GoToAsync("bills");
        }
        catch
        {
            SecureStorage.Remove("access_token");
            SecureStorage.Remove("refresh_token");
        }
    }

    private async void OnSignInClicked(object sender, EventArgs e)
    {
        ErrorLabel.IsVisible = false;
        try
        {
            var session = await _supabase.Auth.SignIn(EmailEntry.Text, PasswordEntry.Text);
            await SecureStorage.SetAsync("access_token",  session!.AccessToken!);
            await SecureStorage.SetAsync("refresh_token", session!.RefreshToken!);
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