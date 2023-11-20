using ChatApp.Core;
using JavaProject___Client.Services;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class HomeViewModelGames : Core.ViewModel
    {
        public string Username { get; set; }


        public string UID { get; set; }

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

        public RelayCommand SendTestMessage {get; set;}

        public HomeViewModelGames(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;
            dataservice.SetUsername(dataservice.server.Username);
            dataservice.SetUID(dataservice.server.UID);
            Username = dataservice.Username;
            UID = dataservice.UID;
            dataservice.server.MessageReceivedEvent += MessageReceivedEvent;

            SendTestMessage = new RelayCommand(o =>
            {
                dataservice.server.SendMessageToUserTest();
            });
        }

        private void MessageReceivedEvent()
        {
            var message = DataService.server.PacketReader.ReadMessage();
            var sender = DataService.server.PacketReader.ReadMessage();
            var senderUID = DataService.server.PacketReader.ReadMessage();
            MessageBox.Show(message + " " + sender + " " + senderUID);

        }
    }
}
