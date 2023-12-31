﻿using ChatApp.Core;
using JavaProject___Client.Core;
using JavaProject___Client.MVVM.Model;
using JavaProject___Client.NET;
using JavaProject___Client.Services;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class HomeViewModelTweet : Core.ViewModel
    {
        public string Username { get; set; }
        private Server _server;
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

        public RelayCommand NavigateToHomeUser {get; set;}

        public HomeViewModelTweet(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;
            _server = dataservice.server;
            Username = dataservice.Username;
            UID = dataservice.UID;


            NavigateToHomeUser = new RelayCommand(o =>
            {
                Navigation.NavigateTo<HomeViewModelUsers>();
            });


        }
    }
}
