// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunHoldfastBriefing(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            var session = CoreDemoSession.Create(dataDirectory);
            string dump = HoldfastBriefingView.FormatCatalogDump(session.Catalog);
            GD.Print(dump);
            bool ok = session.LocationCount > 0 && session.QuestCount > 0
                && session.Catalog.GetQuest("quest_holdfast_the_sheet") != null
                && session.Catalog.Items.IsValid
                && session.Catalog.Items.Count == 40;
            return EmitSummary("holdfast_briefing", ok, ok ? 0 : 1, details: $"items={session.Catalog.Items.Count} locations={session.LocationCount} quests={session.QuestCount}");
        }

    }
}
