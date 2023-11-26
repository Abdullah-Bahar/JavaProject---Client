using ChatApp.Core;
using JavaProject___Client.Core;
using JavaProject___Client.NET;
using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JavaProject___Client.MVVM.ViewModel
{
    public class LoginViewModel : Core.ViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        private Server _server;
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
        public void LoginCorrect()
        {
            Navigation.NavigateTo<HomeViewModelTweet>();
        }
        public void LoginFailed()
        {
            MessageBox.Show("Login failed");
        }
        public RelayCommand Login { get; set; }

        public RelayCommand NavigatoToRegister { get; set; }

        public LoginViewModel(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;

            _server = new Server();
            DataService.SetServer(_server);

            Login = new RelayCommand(o =>
            {
                if (Email != null && Password != null)
                {
                    DataService.server.Login(Email, Password);
                }
                else
                {
                    MessageBox.Show("Please fill in all fields");
                }
                
            }, canExecute => true
            );

            NavigatoToRegister = new RelayCommand(o =>
            {
                Navigation.NavigateTo<RegisterViewModel>();
            }, canExecute => true
            );

            DataService.server.LoginCorrectEvent += LoginCorrect;
            DataService.server.LoginFailEvent += LoginFailed;
        }
    }
}
