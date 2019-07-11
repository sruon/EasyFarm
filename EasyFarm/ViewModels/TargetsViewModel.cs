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
using EasyFarm.Classes;
using EasyFarm.UserSettings;
using MemoryAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EasyFarm.ViewModels
{
    public class TargetsViewModel : ListViewModel<string>, INotifyPropertyChanged
    {
        public TargetsViewModel()
        {
            ViewName = "Targets";
        }

        public override string Value
        {
            get { return Config.Instance.TargetName; }
            set { Set(ref Config.Instance.TargetName, value); OnPropertyChanged("Value"); }
        }

        private IUnit _selectedMob;

        public IUnit SelectedMob {
            get { return _selectedMob; }
            set
            {
                _selectedMob = value;
                OnPropertyChanged("SelectedMob");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public ObservableCollection<Target> SelectedMobs
        {
            get { return Config.Instance.TargetedMobs; }
            set { Set(ref Config.Instance.TargetedMobs, value); }
        }

        public ICollection<IUnit> SurroundingMobs
        {
            get {
                return UnitService.Units?.Where(x => x.NpcType.Equals(NpcType.Mob)).OrderBy(x => x.Name).ToList();
            }
            set {  }
        }

        public bool Aggro
        {
            get { return Config.Instance.AggroFilter; }
            set { Set(ref Config.Instance.AggroFilter, value); }
        }

        public bool Unclaimed
        {
            get { return Config.Instance.UnclaimedFilter; }
            set { Set(ref Config.Instance.UnclaimedFilter, value); }
        }

        public bool PartyClaimed
        {
            get { return Config.Instance.PartyFilter; }
            set { Set(ref Config.Instance.PartyFilter, value); }
        }

        public bool Claimed
        {
            get { return Config.Instance.ClaimedFilter; }
            set { Set(ref Config.Instance.ClaimedFilter, value); }
        }

        public bool TrackByID { get; set; } = false;

        public void AddMob()
        {
            Add();
        }

        protected override void Add()
        {
            if (SelectedMob != null)
            {
                if (TrackByID)
                {
                    var existingMob = SelectedMobs.FirstOrDefault(x => x.Name == SelectedMob.Name && x.Id == SelectedMob.Id);
                    if (existingMob == null)
                    {
                        SelectedMobs.Add(new Target(SelectedMob.Name, SelectedMob.Id, true));
                    }
                }
                else
                {
                    var existingMob = SelectedMobs.FirstOrDefault(x => x.Name == SelectedMob.Name);
                    if (existingMob == null)
                    {
                        SelectedMobs.Add(new Target(SelectedMob.Name, SelectedMob.Id, false));
                    } else
                    {
                        // Found existing track by Id, flip it to generic tracking.
                        existingMob.TrackById = false;
                    }
                }
                SelectedMob = null;
            }
            else
            {
                if (string.IsNullOrWhiteSpace(Value)) return;
                SelectedMobs.Add(new Target(Value, 0, false));
                Value = "";
            }
        }

        protected override void Clear()
        {
            SelectedMobs.Clear();
            Value = "";
        }

        public void DeleteSelected(RoutedEventArgs e)
        {
            try
            {
                SelectedMobs.Remove((Target)((Button)e.Source).DataContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

    }
}