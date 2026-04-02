namespace BudgetTracker;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute("bills", typeof(Pages.ShowBudgets.Budget));
    }
}