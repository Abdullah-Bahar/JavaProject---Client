using JavaProject___Client.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaProject___Client.Services
{

    public interface IDataService
    {
        public string Username { get; set; }

        public Server server { get; set; }

        void SetUsername(string username);
        void SetServer(Server server);
    }
    internal class DataService : IDataService
    {
        public string Username { get; set; }
        public Server server { get; set; }

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
