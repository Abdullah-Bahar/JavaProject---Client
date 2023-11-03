using ChatApp.Core;
using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class RegisterViewModel : Core.ViewModel
    {
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

        public RelayCommand NavigateToLoginCommand { get; set; }

        public RegisterViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateToLoginCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<LoginViewModel>();

            }, canExecute => true
            ); 
        }
    }
}
