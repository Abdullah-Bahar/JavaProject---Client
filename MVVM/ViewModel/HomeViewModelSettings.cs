﻿using JavaProject___Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JavaProject___Client.MVVM.ViewModel
{
    internal class HomeViewModelSettings : Core.ViewModel
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
        public HomeViewModelSettings(INavigationService navService, IDataService dataservice)
        {
            DataService = dataservice;
            Navigation = navService;
        }
    }
}