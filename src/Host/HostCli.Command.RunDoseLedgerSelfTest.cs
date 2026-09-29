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

        public static int RunDoseLedgerSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_dose_ledger_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

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
                var session = DoseLedgerHostSession.Create(dataDirectory);
                Check(session.Registers.npcs.Count == 4, "dose_registers catalog loads the four antagonists");
                Check(session.Registers.bands.Count == 12 && session.Registers.plans.Count == 8,
                    "band and plan vocabulary loaded");
                session.SealDemoSurvivors();
                session.ScribeReading(180f, highEnergy: true);
                session.DiagnoseDemo(DoseLedgerSystem.BandRed);
                session.BookDemoChild();
                session.SignDemoVolunteer();

                // Quest ownership: Dose owns its quest runtime (registered into
                // the session's QuestlineSystem by Create, persisted in the Dose
                // envelope — not the Year of Ash envelope).
                Check(session.Quests.FindDefinition("quest_the_dose_the_first_reading") != null,
                    "first-reading questline registered in the Dose host");
                Check(session.Quests.FindDefinition("quest_the_signed_hour") != null,
                    "signed-hour questline registered in the Dose host");
                Check(session.Quests.StartQuestline("quest_the_dose_the_first_reading", 200),
                    "dose questline starts");

                var save = session.CaptureSave(40);
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == DoseLedgerSave.CurrentSaveVersion, "saveVersion current");
                Check(save.doseLedger.entries.Count > 0, "envelope carries dose ledger entries");
                Check(save.sickList.bands.Count == 1, "envelope carries the sick band");
                Check(save.cohort.children.Count == 1, "envelope carries the cohort child");
                Check(save.voluntaryRegister.entries.Count == 1, "envelope carries the volunteer");
                Check(save.quests.active.Exists(a => a.questlineId == "quest_the_dose_the_first_reading"),
                    "envelope carries dose quest progress");

                Check(DoseLedgerSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = DoseLedgerHostSession.Create(dataDirectory);
                var loaded = DoseLedgerSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Ledger.Entries.Count >= 2, "dose ledger entries restored");
                    Check(fresh.SickList.Bands.Count == 1, "sick list restored");
                    Check(fresh.Cohort.Children.Count == 1, "cohort restored");
                    Check(fresh.Voluntary.Entries.Count == 1, "voluntary register restored");
                    Check(fresh.Ledger.GetCumulative("survivor_gunner_mikhail") > 0f,
                        "cumulative dose restored");
                    Check(fresh.Quests.GetActiveRecord("quest_the_dose_the_first_reading") != null,
                        "dose quest progress restored");
                }

                // Tamper: flip the sim day in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"simDay\":40", "\"simDay\":1");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(DoseLedgerSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
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

            return EmitSummary("dose_ledger_save_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
