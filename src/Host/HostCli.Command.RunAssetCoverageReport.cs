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

        public static int RunAssetCoverageReport(string dataDirectory)
        {
            // The scanner owns the one full-coverage sweep; the report is now a
            // gate — a missing or placeholder binding in any catalog category
            // fails the run ("kill the placeholder squares").
            var (totalIds, missing, placeholder) = AssetCoverageScanner.RunFullCoverageSweep(dataDirectory);
            bool clean = missing == 0 && placeholder == 0;
            GD.Print($"[AssetCoverageReport] gate: ids={totalIds} missing={missing} placeholder={placeholder} -> {(clean ? "PASS" : "FAIL")}");
            return EmitSummary("asset_coverage_report", clean, clean ? 0 : 1);
        }

    }
}
