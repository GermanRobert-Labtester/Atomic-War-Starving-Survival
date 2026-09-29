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
        /// Oral Lore Codex self-test: load both catalog files via the host session,
        /// verify entry count, and exercise query methods (by id, by tag, by genre).
        /// </summary>
        public static int RunOralLoreSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = new OralLoreHostSession();
                session.LoadCatalogs(dataDirectory);

                int count = session.AllSongs.Count;
                GD.Print($"[OralLore] loaded {count} entries from narrative/oral_lore_codex.json + narrative/oral_lore_batch_2.json");
                Check(count > 0, "oral lore catalog is not empty after load");
                // Plan 155: both batches normalize — 16 canonical + 10 batch-2 = 26.
                Check(count == 26, $"oral lore corpus has 26 entries (got {count})");

                // Plan 155: batch-2 schema normalization (entries root, textual tempo).
                var waltz = session.GetSong("oral_b2_the_geiger_counter_waltz");
                Check(waltz != null && waltz.tempo_descriptor == "THREE_FOUR_TIME" && waltz.tempo_bpm == 0,
                    "batch-2 textual tempo preserved verbatim, no fabricated BPM");
                var march = session.GetSong("oral_b2_the_salt_freeholders_march");
                Check(march != null && march.tempo_bpm == 120,
                    "batch-2 explicit N_BPM label normalized to numeric BPM");

                // Query by id: pick the first entry and look it up
                if (count > 0)
                {
                    string firstId = session.AllSongs[0].lore_id;
                    var found = session.GetSong(firstId);
                    Check(found != null && found.lore_id == firstId, "GetSong returns entry by lore_id");
                }

                // Query by tag: exercise the tag filter
                var byTag = session.GetSongsByTag("resistance");
                GD.Print($"[OralLore] GetByTag(\"resistance\"): {byTag.Count} matches");
                Check(byTag != null, "GetSongsByTag returns non-null list");

                // Query by genre: exercise the genre filter
                var byGenre = session.GetSongsByGenre("ballad");
                GD.Print($"[OralLore] GetByGenre(\"ballad\"): {byGenre.Count} matches");
                Check(byGenre != null, "GetSongsByGenre returns non-null list");

                // Null/empty guard
                var nullResult = session.GetSong(null);
                Check(nullResult == null, "GetSong(null) returns null");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[OralLore] selftest exception: {ex.GetType().Name}: {ex.Message}");
                failures++;
            }

            bool passed = failures == 0;
            return EmitSummary("oral_lore_selftest", passed, passed ? 0 : 1,
                passed ? 5 : 0, failures, $"{failures} failures");
        }

    }
}
