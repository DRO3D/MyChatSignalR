using System.Net.Http;
using System.Text.Json;
using Azure;

namespace MyChat.View;

public partial class RegisterPage : ContentPage
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _serializerOptions;
    public RegisterPage()
	{
		InitializeComponent();
        _httpClient = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }
    public async Task<bool> RegisterNewUser()
    {
        bool auth = false;

        string v = $"http://mychatforunionddt.somee.com/api/Values/Regist/{Username.Text}/{Password.Text}/{Name.Text}";
        Uri uri = new Uri(v);
        HttpResponseMessage response = new HttpResponseMessage();
        
        try
        {
            response = await _httpClient.GetAsync(uri);
        }
        catch (Exception ex)
        {
            DisplayAlert("Error!", $"Error message: {ex.Message}", "Ok");
        }
        if (response.IsSuccessStatusCode)
        {
            string text = await response.Content.ReadAsStringAsync();
            auth = JsonSerializer.Deserialize<bool>(text, _serializerOptions);
        }
        
        return auth;
    }
    private async void Button_OnClicked(object sender, EventArgs e)
    {
        bool result = await RegisterNewUser();
        if (result)
        {
            DisplayAlert("Registration", "You have registered!", "OK");
            Shell.Current.SendBackButtonPressed();
        }
        else
        {
            DisplayAlert("Registration", "Your account already registered!", "OK");
        }
    }
}