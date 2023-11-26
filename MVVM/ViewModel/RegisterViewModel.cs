using ChatApp.Core;
using JavaProject___Client.NET;
using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class RegisterViewModel : Core.ViewModel
    {
        public IDataService DataService { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        private Server _server;

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

        public void RegisterSuccess()
        {
            MessageBox.Show("Kayıt olundu, yönlendiriliyorsunuz...");
            Navigation.NavigateTo<HomeViewModelTweet>();
        }

        public void RegisterFailed()
        {
            MessageBox.Show("Böyle bir hesap var !");
        }

        public RelayCommand RegisterToServer { get; set; }

        public RelayCommand NavigateToLogin { get; set; }
        public RegisterViewModel(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;

            _server = DataService.server;

            Regex regex_email = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            Regex regex_password = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,15}$");   

            

            RegisterToServer = new RelayCommand(o =>
            {
                if (Username != null &&Email != null && Password != null)
                {
                    Match match_email = regex_email.Match(Email);
                    Match match_password = regex_password.Match(Password);
                    if (match_email.Success)
                    {
                        if (match_password.Success)
                        {
                            _server.Register(Username, Email, Password);
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid password");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid email address");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Please enter user information");
                }
                
            }, canExecute => true
            );

            NavigateToLogin = new RelayCommand(o =>
            {
                Navigation.NavigateTo<LoginViewModel>();
            }, canExecute => true
            );

            _server.RegisterSuccessEvent += RegisterSuccess;
            _server.RegisterFailEvent += RegisterFailed;


        }
    }
}
