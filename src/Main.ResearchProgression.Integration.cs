// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Factions;
using Ashfall.Core.Narrative;
using Ashfall.Core.Quests;
using Ashfall.Core.Random;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {

        private void TickPlan166Research(int day)
        {
            SetupPlans166To169();
            _sharedResearch?.Tick(day);
        }

        private sealed class Plan166ResearchDayOwner : Ashfall.Core.Campaign.IDayAdvanceOwner
        {
            private readonly Main _main;
            public Plan166ResearchDayOwner(Main main) => _main = main;
            public void CapturePreDaySnapshot(int day) { }
            public void TickDay(int day, List<DayStateChangeEvent> events)
            {
                _main.TickPlan166Research(day);
                events.Add(new DayStateChangeEvent("research_ticked", "plan_166", null, null, day));
            }
        }

    }
}
