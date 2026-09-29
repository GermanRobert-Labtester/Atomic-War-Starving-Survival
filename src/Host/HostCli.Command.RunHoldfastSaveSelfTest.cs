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
        /// Sprint 1 save gate: write through HoldfastSaveCodec, reload into a fresh
        /// session, restore, and verify the gate reproduces. Then tamper the file and
        /// verify the checksum refuses it. Uses a temp path so the real user:// save
        /// is never touched by the test.
        /// </summary>
        public static int RunHoldfastSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_holdfast_s1_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

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
                var session = CoreDemoSession.Create(dataDirectory);
                session.UnlockAndClerk();
                for (int i = 0; i < 12; i++)
                    session.TickDay();
                session.HonourCensusLevy();

                var save = session.CaptureSave();
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == HoldfastSave.CurrentSaveVersion, "saveVersion current");
                Check(save.iceRoad.clerkStarted && save.iceRoad.expansionUnlocked,
                    "envelope carries ice road unlock + clerk");

                Check(HoldfastSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = CoreDemoSession.Create(dataDirectory);
                var loaded = HoldfastSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Clock.Day == session.Clock.Day, "sim day restored");
                    Check(fresh.StatusLine() == session.StatusLine(), "status line identical after roundtrip");
                    Check(fresh.CensusLine() == session.CensusLine(), "census line identical after roundtrip");
                }

                // Tamper: flip clerkStarted in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"clerkStarted\":true", "\"clerkStarted\":false");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(HoldfastSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }

                // Stripped checksum: deleting the field must not bypass validation.
                var codecJson = new SystemTextJsonSerializer();
                var stripped = codecJson.Deserialize<HoldfastSave>(raw);
                stripped!.Checksum = "";
                File.WriteAllText(tmpPath, codecJson.Serialize(stripped));
                Check(HoldfastSaveStore.TryLoad(tmpPath) == null, "checksumless save rejected");
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                TryDeleteTempFile(tmpPath);
            }

            return EmitSummary("holdfast_save_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
