using ChatApp.Core;
using JavaProject___Client.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {
        private Server _server;
        public RelayCommand ConnectToServer { get; set; }
        public MainViewModel()
        {
            ConnectToServer = new RelayCommand(o =>
            {
                _server = new Server();
                _server.ConnectToServer("PnterNN");
            });
        }
    }
}
