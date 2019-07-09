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
using System.Linq;
using EasyFarm.Classes;
using MemoryAPI;
using System.Reflection;
using System;
using EasyFarm.ViewModels;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using EasyFarm.Context;
using EasyFarm.UserSettings;
using Player = EasyFarm.Classes.Player;

namespace EasyFarm.States
{
    public class ReportState : BaseState
    {
        private Dictionary<Regex, string> chatPatterns = null;

        public override bool Check(IGameContext context)
        {
            return true;
        }

        public override void Run(IGameContext context)
        {
            ParseChatForReports(context);
        }

        private void OnExp(Match match) { ReportViewModel.EXP = Int32.Parse(match.Groups[1].Value); }
        private void OnLevel(Match match) { ReportViewModel.LevelsGained = 1; }
        private void OnLimit(Match match) { ReportViewModel.LP = Int32.Parse(match.Groups[1].Value); ; }
        private void OnCapacity(Match match) { ReportViewModel.CP = Int32.Parse(match.Groups[1].Value); ; }
        private void OnMerit(Match match) { ReportViewModel.MeritsGained = 1; }
        private void OnJobPoint(Match match) { ReportViewModel.JobPointsGained = 1; }
        private void OnSpark(Match match) { ReportViewModel.Sparks = Int32.Parse(match.Groups[1].Value); }
        private void OnUnity(Match match) { ReportViewModel.Unity = Int32.Parse(match.Groups[1].Value); }
        private void OnDrop(Match match) { ReportViewModel.AddDrop(match.Groups[1].Value); }
        private void OnGil(Match match) { ReportViewModel.Gil = Int32.Parse(match.Groups[1].Value.Replace(",", "")); }
        private void ParseChatForReports(IGameContext context)
        {
            if (chatPatterns == null)
            {
                chatPatterns = new Dictionary<Regex, string>()
                {
                    {
                        new Regex(context.Player.Name + @" gains (\d+) experience points."),
                        "OnExp"
                    },
                    {
                        new Regex(context.Player.Name + @" attains level (\d+)!"),
                        "OnLevel"
                    },
                    {
                        new Regex(context.Player.Name + @" gains (\d+) limit points."),
                        "OnLimit"
                    },
                    {
                        new Regex(context.Player.Name + @" gains (\d+) capacity points."),
                        "OnCapacity"
                    },
                    {
                        new Regex(context.Player.Name + @" earns a merit point.*"),
                        "OnMerit"
                    },
                    {
                        new Regex(context.Player.Name + @" earns a job point.*"),
                        "OnJobPoint"
                    },
                    {
                        new Regex(@"You receive (\d+) sparks of eminence.*"),
                        "OnSpark"
                    },
                    {
                        new Regex(@"You receive (\d+) Unity accolades.*"),
                        "OnUnity"
                    },
                    {
                        new Regex(@"You find (.*) on .*"),
                        "OnDrop"
                    },
                    {
                        new Regex(context.Player.Name + @" obtains (\d{1,3}(,\d{3})*(\.\d+)?) gil."),
                        "OnGil"
                    }
        };
            }
            var chatEntries = context.API.Chat.ChatEntries.ToList();

            DateTime now = DateTime.Now;

            List<EliteMMO.API.EliteAPI.ChatEntry> matches = chatEntries
                .Where(x => x.Timestamp > now.AddSeconds(-5)).ToList();

            foreach (var line in matches)
            {
                foreach (var pattern in chatPatterns)
                {
                    if (pattern.Key.IsMatch(line.Text))
                    {
                        MethodInfo mi = this.GetType().GetTypeInfo().GetDeclaredMethod(pattern.Value);
                        mi.Invoke(this, new[] { pattern.Key.Match(line.Text) });
                        line.Text = String.Empty;
                    }
                }
            }
        }
    }
}