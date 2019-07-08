﻿// ///////////////////////////////////////////////////////////////////
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

using System.Threading.Tasks;
using EasyFarm.Classes;
using MahApps.Metro.Controls.Dialogs;
using EasyFarm.ViewModels;

namespace EasyFarm.Handlers
{
    public class AutoAttachCharacter : EventHandlerBase<LoadedEvent>
    {
        private readonly SelectCharacter _characterSelecter;
        private readonly SelectProcessViewModel _processSelecter;

        public AutoAttachCharacter(
            SelectCharacter characterSelecter,
            SelectProcessViewModel processSelecter,
            IDialogCoordinator dialogCoordinator,
            EventMessenger events) : base(events)
        {
            _characterSelecter = characterSelecter;
            _processSelecter = processSelecter;
            
        }

        protected override async Task Execute()
        {
            if (_processSelecter.Processes.Count == 1)
            {
                _characterSelecter.ChangeCharacter(new SelectCharacterResult
                 {
                     Process = _processSelecter.Processes[0],
                     IsSelected = true
                 });
            }
        }
    }
}