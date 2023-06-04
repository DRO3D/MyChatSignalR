using System.Text.Json;
using Microsoft.Data.SqlClient;
using MyChat.ViewModels;

namespace MyChat.View;

public partial class LoginPage : ContentPage
{
 //   SqlConnection conn;

 private HttpClient _httpClient;
 private JsonSerializerOptions _serializerOptions;

 

    public LoginPage()
	{
		InitializeComponent();


        _httpClient = new HttpClient();
        _serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        if (SecureStorage.GetAsync("hasAuth").Result == "true")
        {
            Shell.Current.GoToAsync("///home");
        }

    }
    
    

    public async Task<bool> LoginInSys()
    {
        bool auth = false;

        string v = $"http://mychatforunionddt.somee.com/api/Values/{Username.Text}/{Password.Text}";
        Uri uri = new Uri(v);
        
        HttpResponseMessage response = await _httpClient.GetAsync(uri);
        if (response.IsSuccessStatusCode)
        {
            string text = await response.Content.ReadAsStringAsync();
            auth = JsonSerializer.Deserialize<bool>(text, _serializerOptions);
        }
        
        return auth;
    }

    /*
        public async Task<bool> IsCredentialCorrectAsync(string username, string password)
        {
            string connectionString = "workstation id=MyDBForChat.mssql.somee.com;packet size=4096;user id=dro3d_rus;pwd=vbif4fvbif4f;data source=MyDBForChat.mssql.somee.com;persist security info=False;initial catalog=MyDBForChat;Trust Server Certificate=true;Encrypt=False";
            string sqlQuery;

            conn = new SqlConnection(connectionString);
            try
            {
               await conn.OpenAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Login failed", ex.Message, "Try again");
                return false;
            }
            sqlQuery = "SELECT Email, Pass FROM [dbo].[Accounts]";
            SqlCommand cmd = new SqlCommand(sqlQuery, conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (await reader.ReadAsync())
            {
                if (reader.GetValue(0).ToString() == username && reader.GetValue(1).ToString() == password)
                {
                    return true;
                }

            }
            await conn.CloseAsync();

            return false;
        }*/
    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (await LoginInSys())
        {
            await SecureStorage.SetAsync("usrAuth", Username.Text);
            await SecureStorage.SetAsync("hasAuth", "true");
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync("///home");
        }
        else
        {
            await DisplayAlert("Login failed", "Username or password is invalid", "Try again");
            //await Shell.Current.GoToAsync("///home");
        }
    }
    

    private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
    {
         Shell.Current.GoToAsync("/RegisterPage");
    }
}