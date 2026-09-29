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
        /// Duty Roster save gate: build a session, unlock, tick a morning, write a
        /// pencil row, queue a visitor, capture, write through the codec to a temp
        /// path, reload into a fresh session, restore, and verify the wall/encounter
        /// state reproduces. Then tamper the file and verify the checksum refuses it.
        /// </summary>
        public static int RunDutyRosterSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_duty_roster_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

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
                var session = DutyRosterHostSession.Create(dataDirectory);
                session.Unlock(5);
                session.ResolveChart(DutyRosterIds.ChoiceWritePencil);
                session.TickDay();
                session.Marks.SetMark("mark_ration_protocol", "selftest", session.Clock.Day);
                session.SyncDay(60);
                session.StartRosterQuest(DutyRosterIds.QuestTheChart);
                session.GrantOverflowAccess();
                session.RegisterOverflowVisit(DutyRosterIds.LocOverflowAlloc11);
                var assignResult = session.AssignDuty(DutyRosterIds.RoleNightWatch, "npc_kess_adler");
                Check(assignResult.IsSuccess, "duty assigned to night watch");

                var save = session.CaptureSave();
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == DutyRosterSave.CurrentSaveVersion, "saveVersion current");
                Check(save.roster.expansionUnlocked, "envelope carries roster unlock");
                Check(save.roster.rows != null && save.roster.rows.Count > 0, "envelope carries chart rows");

                // Direct in-memory RestoreSave probe on a fresh host session
                var memoryFresh = DutyRosterHostSession.Create(dataDirectory);
                memoryFresh.RestoreSave(save);
                Check(memoryFresh.Clock.Day == session.Clock.Day, "direct RestoreSave: sim day restored");
                Check(memoryFresh.WallLine() == session.WallLine(), "direct RestoreSave: wall line restored");
                Check(memoryFresh.EncountersLine() == session.EncountersLine(), "direct RestoreSave: encounters line restored");
                Check(memoryFresh.Marks.HasMark("mark_ration_protocol"), "direct RestoreSave: mark restored");
                Check(memoryFresh.Quests.IsStarted(DutyRosterIds.QuestTheChart), "direct RestoreSave: quest progress restored");
                Check(memoryFresh.Roster.HasVisitedOverflow(DutyRosterIds.LocOverflowAlloc11), "direct RestoreSave: overflow visited restored");
                Check(memoryFresh.Roster.GetAssignment(DutyRosterIds.RoleNightWatch) == "npc_kess_adler", "direct RestoreSave: role assignment restored");

                // Idempotence probe: calling RestoreSave repeatedly produces identical state
                memoryFresh.RestoreSave(save);
                Check(memoryFresh.Marks.Count == session.Marks.Count, "RestoreSave idempotent: mark count unchanged");
                Check(memoryFresh.Roster.OccupiedRowCount == session.Roster.OccupiedRowCount, "RestoreSave idempotent: row count unchanged");

                // Null safety check: RestoreSave(null) throws ArgumentNullException
                bool nullThrew = false;
                try
                {
                    memoryFresh.RestoreSave(null!);
                }
                catch (ArgumentNullException)
                {
                    nullThrew = true;
                }
                Check(nullThrew, "RestoreSave(null) throws ArgumentNullException");

                Check(DutyRosterSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = DutyRosterHostSession.Create(dataDirectory);
                var loaded = DutyRosterSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded!);
                    Check(fresh.Clock.Day == session.Clock.Day, "disk RestoreSave: sim day restored");
                    Check(fresh.WallLine() == session.WallLine(), "disk RestoreSave: wall line identical after roundtrip");
                    Check(fresh.EncountersLine() == session.EncountersLine(),
                        "disk RestoreSave: encounters line identical after roundtrip");
                    Check(fresh.Marks.HasMark("mark_ration_protocol"), "disk RestoreSave: mark restored");
                    Check(fresh.Quests.IsStarted(DutyRosterIds.QuestTheChart), "disk RestoreSave: quest progress restored");
                    Check(fresh.Roster.HasVisitedOverflow(DutyRosterIds.LocOverflowAlloc11), "disk RestoreSave: overflow visited restored");
                    Check(fresh.Roster.GetAssignment(DutyRosterIds.RoleNightWatch) == "npc_kess_adler", "disk RestoreSave: role assignment restored");
                }

                // Tamper: flip the roster unlock flag in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"expansionUnlocked\":true", "\"expansionUnlocked\":false");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }

                // Error handling / try-catch resilience:
                // 1. Missing file returns null without throwing
                Check(DutyRosterSaveStore.TryLoad(tmpPath + ".non_existent") == null, "missing save file returns null safely");

                // 2. Empty/whitespace file returns null without throwing
                File.WriteAllText(tmpPath, "   \n\t ");
                Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "empty save file returns null safely");

                // 3. Corrupt/malformed JSON returns null without throwing
                File.WriteAllText(tmpPath, "{ \"saveVersion\": 3, corrupt_syntax: [ }");
                Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "corrupt JSON caught and returns null safely");

                // 4. Truncated JSON returns null without throwing
                File.WriteAllText(tmpPath, "{\"saveVersion\":3,\"simDay\":");
                Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "truncated JSON caught and returns null safely");

                // 5. Missing checksum returns null without throwing
                File.WriteAllText(tmpPath, "{\"saveVersion\":3,\"simDay\":5,\"Checksum\":\"\"}");
                Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "missing checksum caught and returns null safely");

                // 6. Unsupported future version returns null without throwing
                File.WriteAllText(tmpPath, "{\"saveVersion\":999,\"simDay\":5,\"Checksum\":\"dummy\"}");
                Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "future saveVersion caught and returns null safely");

                // 7. Invalid negative version returns null without throwing
                File.WriteAllText(tmpPath, "{\"saveVersion\":-1,\"simDay\":5,\"Checksum\":\"dummy\"}");
                Check(DutyRosterSaveStore.TryLoad(tmpPath) == null, "invalid negative saveVersion caught and returns null safely");

                // 8. Null arguments safety
                Check(!DutyRosterSaveStore.TrySave(null!, tmpPath), "TrySave(null) returns false safely");
                Check(DutyRosterSaveStore.TryRestore(null!) == null, "TryRestore(null) returns null safely");
                Check(DutyRosterSaveStore.TryRestore("{ invalid json }") == null, "TryRestore(corrupt) returns null safely");
                Check(DutyRosterSaveStore.TryCapture(null!) == string.Empty, "TryCapture(null) returns empty string safely");
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                TryDeleteTempFile(tmpPath);
            }

            return EmitSummary("duty_roster_save_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
