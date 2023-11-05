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

        void SetUsername(string username);
    }
    internal class DataService : IDataService
    {
        public string Username { get; set; }

        public void SetUsername(string username)
        {
            Username = username;
        }
    }
}
