
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChat.ViewModels
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        HubConnection hubConnection;

        private int count = 0;
        public string UserName { get; set; }
        public string Message { get; set; }
        // список всех полученных сообщений
        public ObservableCollection<MessageData> Messages { get; } = new();

        // идет ли отправка сообщений
        bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }
        // осуществлено ли подключение
        bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }
        // команда отправки сообщений
        public Command SendMessageCommand { get; }

        private async Task saveMessages(string user, string message)
        {
            string povstring = user + "|" + message;
            await SecureStorage.SetAsync($"{UserName}{count}", povstring);
        }

        private async Task loadMessages()
        {

            string localString = SecureStorage.GetAsync($"{UserName}{count}").Result;
            
            while (localString!= null)
            {/*
                bool newUser = true;
                for (int j = 0; j < userLoad.Length; j++)
                {
                    if (userLoad[j] == localString[j])
                    {
                        newUser = false;
                    }
                    else
                    {
                        newUser = true;
                        break;
                    }
                }

                if (newUser)
                {
                    for (int i = 0; i < localString.Length; i++)
                    {
                        if (localString[i] != '|')
                        {
                            userLoad += localString[i];
                        }
                        else
                        {
                            break;
                        }
                    }
                }*/
                string[] data = localString.Split('|');


                SendLocalMessage(data[0], data[1]);
                count++;
                localString = SecureStorage.GetAsync($"{UserName}{count}").Result;
            }


        }
        public ChatViewModel()
        {
            if(SecureStorage.GetAsync("usrAuth").Result != "") { 
            
                UserName = SecureStorage.GetAsync("usrAuth").Result;

            } else
            {
                UserName = "Test";
            }

            // создание подключения
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://MyChatForUnionDDT.somee.com/chatHub")
                .Build();
            IsConnected = false;    // по умолчанию не подключены
            IsBusy = false;         // отправка сообщения не идет
            

            SendMessageCommand = new Command(async () => await SendMessage(), () => IsConnected);

            hubConnection.Closed += async (error) =>
            {
                SendLocalMessage(string.Empty, "Подключение закрыто...");
                IsConnected = false;
                await Task.Delay(5000);
                await Connect();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                SendLocalMessage(user, message);
                ++count;
            });
        }
        // подключение к чату
        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await hubConnection.StartAsync();
                SendLocalMessage(string.Empty, "Вы вошли в чат...");
                await loadMessages();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                SendLocalMessage(string.Empty, $"Ошибка подключения: {ex.Message}");
            }
        }

        // Отключение от чата
        public async Task Disconnect()
        {
            if (!IsConnected) return;

            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage(string.Empty, "Вы покинули чат...");
        }

        // Отправка сообщения
        async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                
                await hubConnection.InvokeAsync("SendMessage", UserName, Message);
                
            }
            catch (Exception ex)
            {
                SendLocalMessage(string.Empty, $"Ошибка отправки: {ex.Message}");
            }

            IsBusy = false;
        }
        // Добавление сообщения
        private async void SendLocalMessage(string user, string message)
        {
            if (user != "")
            {
                await saveMessages(user, message);
            }
            MainThread.BeginInvokeOnMainThread(() =>
            {
                user += ":";
                Messages.Insert(0, new MessageData(user, message));
            });
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    public record MessageData(string User, string Message);
}