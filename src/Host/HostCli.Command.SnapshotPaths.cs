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

        /// <summary>
        /// Phase-2 visual-evidence harness. The capture/diff/regenerate run is
        /// driven by Main.BeginSnapshotRun, which mounts SnapshotOrchestrator
        /// into the scene tree (it needs process frames to render panels and
        /// quits the app when done). Kept as the path resolver so callers can
        /// discover the golden root.
        /// </summary>
        public static string SnapshotGoldenRoot()
        {
            return Path.Combine(CatalogPath.ResolveRepoRoot(), "snapshots");
        }

        /// <summary>Capture-side scratch root (outside snapshots/ so the Godot
        /// importer never sees diff captures; the dir ships a .gdignore).</summary>
        public static string SnapshotCaptureRoot()
        {
            return Path.Combine(CatalogPath.ResolveRepoRoot(), "snapshot-capture");
        }

    }
}
