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
using System;
using System.Diagnostics;
using MemoryAPI;
using System.Threading;
using EasyFarm.UserSettings;
using EliteMMO.API;

namespace EasyFarm.Classes
{
    public class Target
    {
        public string Name { get; set; }
        public bool TrackById { get; set; }
        public int Id { get; set; }

        
        public Target(string name, int id, bool trackById)
        {
            Name = name;
            Id = id;
            TrackById = trackById;
        }



    }
}
