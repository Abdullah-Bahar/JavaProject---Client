using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public HomeViewModelGames(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;
            dataservice.SetUsername(dataservice.server.Username);
            dataservice.SetUID(dataservice.server.UID);
            Username = dataservice.Username;
            UID = dataservice.UID;
            dataservice.server.MessageEvent += MessageEvent;
        }

        private void MessageEvent()
        {
            var result = DataService.server.PacketReader.ReadMessage();
            MessageBox.Show(result);
        }
    }
}
