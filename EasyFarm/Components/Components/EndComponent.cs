﻿
/*///////////////////////////////////////////////////////////////////
<EasyFarm, general farming utility for FFXI.>
Copyright (C) <2013>  <Zerolimits>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
*/
///////////////////////////////////////////////////////////////////

using EasyFarm.Classes;
using EasyFarm.Logging;
using FFACETools;
using System;
using System.Linq;

namespace EasyFarm.Components
{
    /// <summary>
    /// Handles the end of battle situation. 
    /// Fires off the end list, sets FightStart to true so other 
    /// lists can fire and replaces targets that are dead, null, 
    /// empty or invalid. 
    /// </summary>
    public class EndComponent : BaseComponent
    {
        private FFACE _fface;

        private Executor _executor;

        private UnitService _units;

        private DateTime _lastTargetCheck = DateTime.Now;

        public Unit Target
        {
            get { return AttackContainer.TargetUnit; }
            set { AttackContainer.TargetUnit = value; }
        }

        public EndComponent(FFACE fface)
            : base(fface)
        {
            _fface = fface;
            _executor = new Executor(fface);
            _units = new UnitService(fface);            
        }

        public override bool CheckComponent()
        {
            // Null, dead and empty mob check. 
            if (Target == null || Target.IsDead) return true;

            // Creature is unkillable and does not meets the 
            // user's criteria for valid mobs defined in MobFilters. 
            return !UnitFilters.MobFilter(_fface, Target);
        }

        public override void RunComponent()
        {
            // Execute moves. 
            var Usable = Config.Instance.BattleLists["End"].Actions
                .Where(x => ActionFilters.BuffingFilter(FFACE, x));

            _executor.UseBuffingActions(Usable);

            if (_lastTargetCheck.AddSeconds(Constants.UNIT_ARRAY_CHECK_RATE) < DateTime.Now)
            {
                // First get the first mob by distance. 
                var mobs = _units.MOBArray.Where(x => UnitFilters.MobFilter(_fface, x))
                    .OrderByDescending(x => x.PartyClaim)
                    .ThenByDescending(x => x.HasAggroed)
                    .ThenBy(x => x.Distance)
                    .ToList();

                // Set our new target at the end so that we don't accidentaly cast on a 
                // new target. 
                AttackContainer.TargetUnit = mobs.FirstOrDefault();

                _lastTargetCheck = DateTime.Now;
            }

            if (Target != null)
            {
                // Reset all usage data to begin a new battle. 
                foreach (var action in Config.Instance.BattleLists.Actions)
                {
                    action.Usages = 0;
                }

                Logger.Write.StateRun("Now targeting " + Target.Name + " : " + Target.ID);
            }
        }

        public override void ExitComponent()
        {
            // Set to false in order to use starting moves again in the 
            // attack Component. 
            AttackContainer.FightStarted = false;
        }
    }
}
