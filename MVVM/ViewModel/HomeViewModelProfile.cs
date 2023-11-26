using JavaProject___Client.MVVM.Model;
using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class HomeViewModelProfile : Core.ViewModel
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
        public HomeViewModelProfile(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;
            dataservice.Username = dataservice.server.Username;
            dataservice.UID = dataservice.server.UID;
            dataservice.Users = new ObservableCollection<UserModel>();
            dataservice.Messages = new ObservableCollection<MessageModel>();
            dataservice.Tweets = new ObservableCollection<TweetModel>();
        }
    }
}
