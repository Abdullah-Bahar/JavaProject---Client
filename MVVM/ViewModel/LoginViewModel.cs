using ChatApp.Core;
using JavaProject___Client.Core;
using JavaProject___Client.NET;
using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JavaProject___Client.MVVM.ViewModel
{
    public class LoginViewModel : Core.ViewModel
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
        public RelayCommand NavigateToCommunityCommand { get; set; }

        public LoginViewModel(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;
            NavigateToCommunityCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<CommunityViewModel>();
            }, canExecute => true
            );
        }
    }
}
