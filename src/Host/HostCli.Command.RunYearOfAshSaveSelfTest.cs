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
        /// Year of Ash save gate: build a session, advance the timeline, resolve a
        /// door encounter, capture, write through the codec to a temp path, reload
        /// into a fresh session, restore, and verify the timeline/encounter/faction
        /// state reproduces. Then tamper the file and verify the checksum refuses it.
        /// </summary>
        public static int RunYearOfAshSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_year_of_ash_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

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
                var session = YearOfAshHostSession.Create(dataDirectory);
                // Ownership contract: Dose (Expansion 07) and Verdict (Expansion 08)
                // questlines are NOT registered here — each owns its quest runtime
                // and persists it in its own envelope (DoseLedgerSave v2+ /
                // VerdictSave v3+). YearOfAsh must not remain a second owner.
                foreach (var dqid in new[]
                    {
                        "quest_the_dose_the_first_reading", "quest_the_sick_of_room_seven",
                        "quest_the_childs_number", "quest_the_signed_hour",
                        "quest_verdict_the_warm_range"
                    })
                    Check(session.Quests.FindDefinition(dqid) == null,
                        $"expansion questline not double-owned by YearOfAsh: {dqid}");
                session.TickDay(255);

                // Drive the two phase-scoped systems inside their own windows so the
                // gate covers state the old envelope silently dropped: deep freeze runs
                // to day 240 and de-ices after, radon only wakes at day 300.
                for (int day = 190; day <= 240; day++)
                    session.DeepFreeze.TickDailyThermal(day, -38.0f);
                for (int day = 300; day <= 340; day++)
                    session.Radon.TickDailyRadon(day, -38.0f);
                Check(session.Radon.State.totalAlphaDoseLogged > 0.0f, "radon dose accumulated");
                Check(session.DeepFreeze.State.intakeIceThicknessMm > 0.0f, "intake iced");

                // Resolve a door encounter so the encounters section is non-trivial.
                var enc = session.Encounters.Catalog.Count > 0 ? session.Encounters.Catalog[0] : null;
                if (enc != null)
                {
                    var result = session.Encounters.ResolveChoice(enc, enc.choices[0], session.DemoRoster);
                    Check(result != null, "door encounter resolved");
                }

                var save = session.CaptureSave();
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == YearOfAshSave.CurrentSaveVersion, "saveVersion current");
                Check(save.timeline.currentDay == 255, "envelope carries timeline day");

                Check(YearOfAshSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = YearOfAshHostSession.Create(dataDirectory);
                var loaded = YearOfAshSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded!);
                    Check(fresh.Timeline.CurrentDay == 255, "timeline day restored");
                    Check(fresh.Timeline.CurrentPhase == YearOfAshPhase.Phase5_FactionSiege, "phase restored");
                    Check(fresh.Encounters.State.totalEncountersResolved
                        == session.Encounters.State.totalEncountersResolved,
                        "encounter history restored");
                    Check(fresh.FactionWar.WarTension == session.FactionWar.WarTension,
                        "war tension restored");

                    // v2 sections: the three systems the envelope used to drop.
                    Check(fresh.DeepFreeze.State.intakeIceThicknessMm
                        == session.DeepFreeze.State.intakeIceThicknessMm, "intake ice restored");
                    Check(fresh.DeepFreeze.State.daysFrozenPipelinesExperienced
                        == session.DeepFreeze.State.daysFrozenPipelinesExperienced,
                        "frozen-pipeline days restored");
                    Check(fresh.Radon.State.scrubberFilterHealthPercent
                        == session.Radon.State.scrubberFilterHealthPercent, "scrubber health restored");
                    Check(fresh.Radon.State.totalAlphaDoseLogged
                        == session.Radon.State.totalAlphaDoseLogged, "alpha dose restored");
                    Check(fresh.Quests.State.completedQuestlineIds.Count
                        == session.Quests.State.completedQuestlineIds.Count, "questline progress restored");
                }

                // Tamper: flip the sim day in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"simDay\":255", "\"simDay\":180");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(YearOfAshSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                TryDeleteTempFile(tmpPath);
            }

            return EmitSummary("year_of_ash_save_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
