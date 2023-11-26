using ChatApp.Core;
using JavaProject___Client.MVVM.Model;
using JavaProject___Client.NET;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaProject___Client.Services
{
    public interface IDataService
    {
        public string Username { get; set; }
        public string UID { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<TweetModel> Tweets { get; set; }
        public Server server { get; set; }
    }
    internal class DataService : IDataService
    {
        public string Username { get; set; }
        public Server server { get; set; }
        public string UID { get; set; }

        public ObservableCollection<UserModel> Users { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<TweetModel> Tweets { get; set; }

        
    }
}
