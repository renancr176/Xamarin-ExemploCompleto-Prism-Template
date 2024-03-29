﻿using System;
using System.Collections.ObjectModel;
using Prism.Mvvm;

namespace TmsCollectorAndroid.Models
{
    public class MaintenanceSealsInputModel : BindableBase
    {
        public MaintenanceSealsInputModel()
        {
            Seals = new ObservableCollection<string>();
            LvSeals = new ObservableCollection<string>();
        }

        #region Controls

        public Action SealFocus;

        public Action BtnConfirmationFocus;

        #endregion

        private int _packingListAccessoryId;
        public int PackingListAccessoryId
        {
            get { return _packingListAccessoryId; }
            set { SetProperty(ref _packingListAccessoryId, value); }
        }

        private int _transportAccessoryDoors;
        public int TransportAccessoryDoors
        {
            get { return _transportAccessoryDoors; }
            set { SetProperty(ref _transportAccessoryDoors, value); }
        }

        private bool _onlyConference;
        public bool OnlyConference
        {
            get { return _onlyConference; }
            set { SetProperty(ref _onlyConference, value); }
        }

        private ObservableCollection<string> _seals;
        public ObservableCollection<string> Seals
        {
            get { return _seals; }
            set { SetProperty(ref _seals, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _seal;
        public string Seal
        {
            get { return _seal; }
            set { SetProperty(ref _seal, value?.Trim()); }
        }

        private ObservableCollection<string> _lvSeals;
        public ObservableCollection<string> LvSeals
        {
            get { return _lvSeals; }
            set { SetProperty(ref _lvSeals, value); }
        }
    }
}