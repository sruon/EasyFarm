// ///////////////////////////////////////////////////////////////////
// This file is a part of EasyFarm for Final Fantasy XI
// Copyright (C) 2013 Mykezero
//  
// EasyFarm is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// EasyFarm is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// If not, see <http://www.gnu.org/licenses/>.
// ///////////////////////////////////////////////////////////////////
using System.Collections.ObjectModel;
using EasyFarm.Classes;

using EasyFarm.Infrastructure;
using EasyFarm.UserSettings;
using System.ComponentModel;
using System;

namespace EasyFarm.ViewModels
{
    public class ReportViewModel : ViewModelBase, IViewModel, INotifyPropertyChanged
    {
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        private static int _defeated = 0;
        private static int _exp = 0;
        private static int _lp = 0;
        private static int _cp = 0;
        private static int _levelsGained = 0;
        private static int _meritsGained = 0;
        private static int _jobPointsGained = 0;
        private static int _sparks = 0;
        private static int _unity = 0;
        private static int _gil = 0;

        public ReportViewModel()
        {
            ViewName = "Report";
        }

        private static void NotifyPropertyChanged(string propertyName)
        {
           StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
        }

        public static int Defeated
        {
            get { return _defeated; }
            set
            {
                _defeated += value;
                NotifyPropertyChanged("Defeated");
            }
        }

        public static int EXP
        {
            get { return _exp; }
            set
            {
                _exp += value;
                NotifyPropertyChanged("EXP");
            }
        }
        public static int LP
        {
            get { return _lp; }
            set
            {
                _lp += value;
                NotifyPropertyChanged("LP");
            }
        }


        public static int CP
        {
            get { return _cp; }
            set
            {
                _cp += value;
                NotifyPropertyChanged("CP");
            }
        }

        public static int LevelsGained
        {
            get { return _levelsGained; }
            set
            {
                _levelsGained += value;
                NotifyPropertyChanged("LevelsGained");

            }
        }
        public static int MeritsGained
        {
            get { return _meritsGained; }
            set
            {
                _meritsGained += value;
                NotifyPropertyChanged("MeritsGained");
            }
        }

        public static int JobPointsGained
        {
            get { return _jobPointsGained; }
            set
            {
                _jobPointsGained += value;
                NotifyPropertyChanged("JobPointsGained");
            }
        }
        public static int Sparks
        {
            get { return _sparks; }
            set
            {
                _sparks += value;
                NotifyPropertyChanged("Sparks");
            }
        }

        public static int Unity
        {
            get { return _unity; }
            set
            {
                _unity += value;
                NotifyPropertyChanged("Unity");
            }
        }
        public static int Gil
        {
            get { return _gil; }
            set
            {
                _gil += value;
                NotifyPropertyChanged("Gil");
            }
        }

    }
}