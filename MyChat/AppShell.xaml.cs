using MyChat.View;

namespace MyChat;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("main", typeof(MainPage));
        Routing.RegisterRoute("home", typeof(MainPage));
        Routing.RegisterRoute("message", typeof(MessagePage));
        Routing.RegisterRoute("settings", typeof(Settings));
        Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
    }
    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
