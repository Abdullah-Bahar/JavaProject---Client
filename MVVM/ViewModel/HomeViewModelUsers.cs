using ChatApp.Core;
using JavaProject___Client.MVVM.Model;
using JavaProject___Client.NET;
using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class HomeViewModelUsers : Core.ViewModel
    {

        public IDataService DataService { get; set; }

        private INavigationService _navigation;
        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                OnPropertyChanged();
            }
        }
        //****************************************************************
        public String Username { get; set; }

        public string UID { get; set; }

        private UserModel _selectedUser;

        private Server _server;

        public UserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        private void UserConnected()
        {
            string username = _server.PacketReader.ReadMessage();
            string uid = _server.PacketReader.ReadMessage();
            string messageCount = _server.PacketReader.ReadMessage();
            var user = new UserModel
            {
                Username = username,
                ImageSource = "CornflowerBlue",
                UID = uid,
                Messages = new ObservableCollection<MessageModel>()
            };
            string dataUsername = "";
            string dataUID = "";
            string dataImageSource = "";
            string dataMessage = "";
            DateTime dataTime = DateTime.Now;
            bool dataFirstMessage = true;
            for (int i = 0; i < int.Parse(messageCount); i++)
            {
                try
                {
                    dataUsername = _server.PacketReader.ReadMessage();
                    dataUID = _server.PacketReader.ReadMessage();
                    dataImageSource = _server.PacketReader.ReadMessage();
                    dataMessage = _server.PacketReader.ReadMessage();
                    dataTime = DateTime.Parse(_server.PacketReader.ReadMessage());
                    dataFirstMessage = bool.Parse(_server.PacketReader.ReadMessage());
                    if (dataUsername != null)
                    {
                        user.Messages.Add(new MessageModel()
                        {
                            Username = dataUsername,
                            ImageSource = "",
                            UsernameColor = "CornflowerBlue",
                            Message = dataMessage,
                            Time = dataTime,
                            FirstMessage = dataFirstMessage
                        });
                    }

                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
            if (dataUsername == "")
            {
                user.Messages.Add(new MessageModel()
                {
                    Username = "ChatApp",
                    ImageSource = "",
                    UsernameColor = "CornflowerBlue",
                    Message = username + " Çevrimiçi oldu",
                    Time = DateTime.Now,
                    FirstMessage = true
                });
            }
            if (!DataService.Users.Any(x => x.UID == user.UID))
            {
                Application.Current.Dispatcher.Invoke(() => DataService.Users.Add(user));
            }
        }

        private void MessageReceived()
        {
            var msg = _server.PacketReader.ReadMessage();
            var username = _server.PacketReader.ReadMessage();
            var sendedUserUID = _server.PacketReader.ReadMessage();
            var user = DataService.Users.Where(x => x.UID == sendedUserUID).FirstOrDefault();
            if (user != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    bool FirstMessage = false;
                    if (user.LastMessage.Username != username)
                    {
                        FirstMessage = true;
                    }
                    user.Messages.Add(new MessageModel
                    {
                        Username = username,
                        ImageSource = "",
                        UsernameColor = "CornflowerBlue",
                        Message = msg,
                        Time = DateTime.Now,
                        FirstMessage = FirstMessage
                    });
                });
            }
        }

        private void createGroup()
        {
            var groupName = _server.PacketReader.ReadMessage();
            var userUIDS = _server.PacketReader.ReadMessage();
            List<string> uids = userUIDS.Split(' ').ToList();
            List<string> usernames = new List<string>();
            foreach (string uid in uids)
            {
                UserModel? userModel = DataService.Users.Where(x => x.UID == uid).FirstOrDefault();
                if (userModel != null)
                {
                    usernames.Add(userModel.Username);
                }
                else
                {
                    //kullanıcı açık olmadığı için ekleme yapılmadı
                }
            }
            var user = new UserModel
            {
                Username = groupName,
                ImageSource = "CornflowerBlue",
                UID = userUIDS,
                Messages = new ObservableCollection<MessageModel>()
            };
            user.Messages.Add(new MessageModel()
            {
                Username = groupName,
                ImageSource = "",
                UsernameColor = "CornflowerBlue",
                Message = "Uye Listesi: " + string.Join(" ", usernames),
                Time = DateTime.Now,
                FirstMessage = true
            });
            Application.Current.Dispatcher.Invoke(() => DataService.Users.Add(user));
        }

        private void UserDisconnected()
        {
            var uid = _server.PacketReader.ReadMessage();
            var user = DataService.Users.FirstOrDefault(x => x.UID == uid);
            //if (user != null)
            //{
            //    Application.Current.Dispatcher.Invoke(() => DataService.Users.Remove(user));
            //}
        }
        public RelayCommand SendMessageCommand { get; set; }

        public RelayCommand NavigateToHome { get; set; }

        public HomeViewModelUsers(INavigationService navService, IDataService dataservice)
        {
            
            DataService = dataservice;
            Navigation = navService;
            _server = dataservice.server;
            Username = dataservice.Username;
            UID = dataservice.UID;

            NavigateToHome = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModelTweet>();
            });

            SendMessageCommand = new RelayCommand(o =>
            {
                if (!string.IsNullOrEmpty(Message))
                {
                    if (_selectedUser != null)
                    {
                        bool FirstMessage = false;
                        if (_selectedUser.LastMessage.Username != dataservice.Username)
                        {
                            FirstMessage = true;
                        }
                        SelectedUser.Messages.Add(new MessageModel
                        {
                            Username = dataservice.Username,
                            ImageSource = "",
                            UsernameColor = "CornflowerBlue",
                            Message = Message,
                            Time = DateTime.Now,
                            FirstMessage = FirstMessage
                        });
                        _server.SendMessage(Message, SelectedUser.UID, FirstMessage.ToString());
                    }
                    Message = "";
                }
            });

            _server.UserConnectedEvent += UserConnected;
            _server.MessageReceivedEvent += MessageReceived;
            _server.UserDisconnectedEvent += UserDisconnected;
            _server.GroupCreatedEvent += createGroup;

        }

    }
}
