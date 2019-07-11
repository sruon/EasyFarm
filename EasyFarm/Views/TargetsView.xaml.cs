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
using EasyFarm.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace EasyFarm.Views
{
    /// <summary>
    ///     Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TargetsView
    {
        public TargetsView()
        {
            InitializeComponent();
            DataContext = new TargetsViewModel();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // This is probably a terrible way of doing it.
            ((TargetsViewModel)DataContext).DeleteSelected(e);
        }

        private void cmbNearby_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBox)sender).IsDropDownOpen)
            {
                if (e.AddedItems.Count > 0)
                {
                    ((TargetsViewModel)DataContext).SelectedMob = (IUnit)e.AddedItems[0];
                    ((TargetsViewModel)DataContext).AddMob();
                    ((ComboBox)sender).SelectedIndex = -1;
                }
            }
        }
        private void cmbNearby_DropDownOpened(object sender, EventArgs e)
        {
            // Refresh the list when user opens it.
            ((ComboBox)sender).ItemsSource = ((TargetsViewModel)DataContext).SurroundingMobs;
            ((ComboBox)sender).SelectedIndex = -1;
        }
    }
}