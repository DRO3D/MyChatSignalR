namespace MyChat.View;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Are you sure?", "You will be logged out.", "Yes", "No"))
        {
            SecureStorage.Remove("usrAuth");
            SecureStorage.Remove("hasAuth");
            await Shell.Current.GoToAsync("///login");
        }
    }
}