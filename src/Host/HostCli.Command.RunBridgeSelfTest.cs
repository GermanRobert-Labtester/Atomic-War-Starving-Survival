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

        /// <summary>The UnityEngine.* compatibility shim (src/Bridge/) has been fully removed.
        /// This selftest is retained as a stable CLI verb so CI/documentation references do not
        /// break; it reports the removal and exits 0 instead of silently hanging in the app loop.</summary>
        public static int RunBridgeSelfTest()
        {
            GD.Print("[BridgeSelfTest] UnityEngine.* shim removed — src/Bridge/ is empty. Migration to Godot is complete; nothing to shim.");
            return EmitSummary("bridge_selftest", true, 0, details: "UnityEngine.* shim removed");
        }

    }
}
