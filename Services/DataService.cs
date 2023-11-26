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

        void SetUsername(string username);
        void SetServer(Server server);
        void SetUID(string uid);
    }
    internal class DataService : IDataService
    {
        public string Username { get; set; }
        public Server server { get; set; }
        public string UID { get; set; }

        public ObservableCollection<UserModel> Users { get; set; }

        public ObservableCollection<MessageModel> Messages { get; set; }
        public ObservableCollection<TweetModel> Tweets { get; set; }

        public void SetUID(string uid)
        {
            UID = uid;
        }

        public void SetUsername(string username)
        {
            Username = username;
        }
        public void SetServer(Server server)
        {
              this.server = server;
        }
    }
}
