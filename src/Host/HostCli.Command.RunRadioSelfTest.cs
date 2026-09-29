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
        /// Radio persistence gate: every authoritative mutable receiver value
        /// (intercept history, played-broadcast dedup keys, tuned frequency, day)
        /// survives a checksummed save/load round-trip through RadioSaveStore,
        /// and tampering / missing saves are rejected or degrade to fresh state.
        /// </summary>
        public static int RunRadioSelfTest()
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

            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_radio_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

            try
            {
                var engine = new Ashfall.Core.Radio.FactionRadioEngine();
                engine.RegisterChannel(new Ashfall.Core.Radio.FactionRadioChannel
                {
                    FactionId = "faction_holdfast",
                    Callsign = "HOLDFAST BASE",
                    FrequencyMhz = 97.5f,
                    InterceptChatter = new List<string> { "vo_kind_parley at the hatch" }
                });
                engine.AddSilenceEvent("dead air");

                var session = new RadioHostSession(engine, new SeededRng(2026), day: 10);
                Check(Math.Abs(session.CurrentFrequency - 97.5f) < 0.001f,
                    "receiver tunes to the first faction frequency");

                session.Listen();
                session.Listen();
                session.SetDay(11);
                session.Listen();
                Check(session.History.Count == 3, "intercept history accumulates");
                var lastIntercept = session.History[session.History.Count - 1];
                Check(session.HasPlayed(lastIntercept), "played-dedup key recorded for a voiced broadcast");

                // Capture → store → load → restore.
                var save = session.CaptureSave();
                Check(save != null && save.history.Count == 3, "capture snapshots history");
                Check(RadioSaveStore.TrySave(save!, tmpPath), "radio save written via store");

                var loaded = RadioSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "radio save loads back");
                var fresh = new RadioHostSession(engine, new SeededRng(1), day: 1);
                fresh.RestoreSave(loaded!);
                Check(fresh.History.Count == session.History.Count, "intercept history survives reload");
                Check(Math.Abs(fresh.CurrentFrequency - session.CurrentFrequency) < 0.001f,
                    "tuned frequency survives reload");
                Check(fresh.Day == session.Day, "sim day survives reload");
                Check(fresh.HasPlayed(lastIntercept), "played-broadcast suppression survives reload");

                // Tamper rejection.
                string raw = System.IO.File.ReadAllText(tmpPath);
                string tampered = raw.Replace("at the hatch", "at the gate");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    System.IO.File.WriteAllText(tmpPath, tampered);
                    Check(RadioSaveStore.TryLoad(tmpPath) == null, "tampered radio save rejected (checksum)");
                }

                // No-radio-save fallback → fresh receiver.
                string missing = Path.Combine(
                    Path.GetTempPath(), "ashfall_radio_selftest_missing_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path
                Check(RadioSaveStore.TryLoad(missing) == null, "no radio save falls back to fresh state");
            }
            catch (Exception e)
            {
                Check(false, "radio selftest threw: " + e.Message);
            }
            finally
            {
                TryDeleteTempFile(tmpPath);
            }

            return EmitSummary("radio_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
