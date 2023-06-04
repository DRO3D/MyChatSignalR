using MyChat.ViewModels;
namespace MyChat.View;

public partial class MessagePage : ContentPage
{
    

    ChatViewModel viewModel;
    public MessagePage()
    {
        InitializeComponent();
        viewModel = new ChatViewModel();
        BindingContext = viewModel;
        

    }


    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
#if ANDROID
            var edittext = entry.Handler.PlatformView as Android.Widget.EditText;
            edittext.Background = null;
#endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.Connect();
    }
 
    protected override async void OnDisappearing()
    {
        base.OnDisappearing();
        await viewModel.Disconnect();
    }
}