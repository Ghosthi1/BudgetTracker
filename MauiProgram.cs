using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Supabase;
using BudgetTracker.Pages.ShowBudgets;

namespace BudgetTracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(_ => new Client(
            Constants.SupabaseUrl,
            Constants.SupabaseKey,
            new SupabaseOptions { AutoConnectRealtime = false }
        ));

        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Budget>();
        builder.Services.AddSingleton<BudgetViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}