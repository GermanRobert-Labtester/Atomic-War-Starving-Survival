// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 30 (The Press — broadsheet & almanac printing).

using System;
using System.IO;
using Godot;
using Ashfall.Core.Print;

namespace AtomicWar.GodotApp
{
    public static class HostCliBroadsheetPress
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Broadsheet Press Self-Test (Expansion 30) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: a broadsheet run prints copies and reaches the shelter.
                var tray = new TypeTrayState();
                var r1 = PublicBroadsheetPressEngine.ExecutePrintRun(
                    tray, PublicationKind.Broadsheet, 200, 500, 40);
                if (!r1.BlockedByShortage && r1.CopiesPrinted > 0 && r1.AudienceReachPermille > 0)
                {
                    GD.Print($"[PASS] Check 1: Broadsheet printed {r1.CopiesPrinted} copies (reach {r1.AudienceReachPermille}‰).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 1: Broadsheet run failed (blocked={r1.BlockedByShortage}, copies={r1.CopiesPrinted}).");

                // Check 2: the run consumes ink, paper, and accrues type wear.
                if (tray.InkReservoirPermille < 1000 && tray.PaperStockPermille < 1000 && tray.TypeWearPermille > 0)
                {
                    GD.Print($"[PASS] Check 2: Consumables moved (ink {tray.InkReservoirPermille}‰, paper {tray.PaperStockPermille}‰, wear {tray.TypeWearPermille}‰).");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 2: Print run did not consume ink/paper or accrue wear.");

                // Check 3: an almanac is more ink-hungry than a pamphlet.
                var almanacTray = new TypeTrayState();
                var almanac = PublicBroadsheetPressEngine.ExecutePrintRun(almanacTray, PublicationKind.Almanac, 100, 500, 20);
                var pamphletTray = new TypeTrayState();
                var pamphlet = PublicBroadsheetPressEngine.ExecutePrintRun(pamphletTray, PublicationKind.Pamphlet, 100, 500, 20);
                if (almanac.InkConsumedPermille > pamphlet.InkConsumedPermille)
                {
                    GD.Print($"[PASS] Check 3: Almanac ink {almanac.InkConsumedPermille}‰ > pamphlet {pamphlet.InkConsumedPermille}‰.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 3: Almanac did not out-consume a pamphlet.");

                // Check 4: a dry press is refused with a shortage reason.
                var dry = new TypeTrayState { InkReservoirPermille = 10 };
                var r4 = PublicBroadsheetPressEngine.ExecutePrintRun(dry, PublicationKind.Broadsheet, 100, 500, 10);
                if (r4.BlockedByShortage && r4.CopiesPrinted == 0 && !string.IsNullOrEmpty(r4.ShortageReason))
                {
                    GD.Print($"[PASS] Check 4: Dry press refused ('{r4.ShortageReason}').");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 4: Dry press was not refused.");

                // Check 5: worn type degrades reach.
                var wornTray = new TypeTrayState { TypeWearPermille = PublicBroadsheetPressEngine.TypeWearDegradationThreshold };
                var r5 = PublicBroadsheetPressEngine.ExecutePrintRun(wornTray, PublicationKind.Broadsheet, 400, 500, 40);
                if (r5.AudienceReachPermille < 1000)
                {
                    GD.Print($"[PASS] Check 5: Worn type degraded reach to {r5.AudienceReachPermille}‰.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 5: Worn type did not degrade reach.");

                // Check 6: a skilled compositor slows wear.
                var unskilled = new TypeTrayState();
                var skilled = new TypeTrayState();
                PublicBroadsheetPressEngine.ExecutePrintRun(unskilled, PublicationKind.Broadsheet, 50, 0, 10);
                PublicBroadsheetPressEngine.ExecutePrintRun(skilled, PublicationKind.Broadsheet, 50, 1000, 10);
                if (skilled.TypeWearPermille < unskilled.TypeWearPermille)
                {
                    GD.Print($"[PASS] Check 6: Skilled wear {skilled.TypeWearPermille}‰ < unskilled {unskilled.TypeWearPermille}‰.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 6: Compositor skill did not reduce wear.");

                // Check 7: rumor debunk correction scales with reach and evidence.
                int strong = PublicBroadsheetPressEngine.CalculateRumorDebunkingCorrection(1000, 1000, 1000);
                int weak = PublicBroadsheetPressEngine.CalculateRumorDebunkingCorrection(1000, 200, 200);
                if (strong > weak && strong <= 1000)
                {
                    GD.Print($"[PASS] Check 7: Debunk correction {strong}‰ > weak {weak}‰.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 7: Debunk correction incorrect (strong={strong}, weak={weak}).");

                // Check 8: type reset restores wear and adds pieces.
                var resetTray = new TypeTrayState { TypeWearPermille = 500, TypePiecesAvailable = 1000 };
                PublicBroadsheetPressEngine.RestoreTypeTray(resetTray, 400);
                if (resetTray.TypeWearPermille == 300 && resetTray.TypePiecesAvailable == 1400)
                {
                    GD.Print($"[PASS] Check 8: Type reset to wear {resetTray.TypeWearPermille}‰, pieces {resetTray.TypePiecesAvailable}.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 8: Type reset incorrect (wear {resetTray.TypeWearPermille}, pieces {resetTray.TypePiecesAvailable}).");

                // Check 9: the ledger archives runs and reports a census.
                var ledger = new BroadsheetPressLedger();
                ledger.ExecuteRun("pub_1", PublicationKind.Broadsheet, 300, 600, 30, "Shelter bulletin", 4);
                ledger.ExecuteRun("pub_2", PublicationKind.Pamphlet, 100, 600, 30, "Debunk", 5);
                var census = ledger.GetCensus();
                if (census.PublicationCount == 2 && census.CumulativeCopiesPrinted > 0
                    && ledger.FindPublication("pub_1") is { Headline: "Shelter bulletin" })
                {
                    GD.Print($"[PASS] Check 9: Ledger archived {census.PublicationCount} publications, {census.CumulativeCopiesPrinted} copies.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 9: Ledger archive incorrect (count {census.PublicationCount}).");

                // Check 10: a blocked run is not archived and consumes nothing.
                var blockedLedger = new BroadsheetPressLedger(new BroadsheetPressState
                {
                    TypeTray = new TypeTrayState { InkReservoirPermille = 5 }
                });
                int before = blockedLedger.GetCensus().PublicationCount;
                var blocked = blockedLedger.ExecuteRun("pub_x", PublicationKind.Broadsheet, 500, 500, 10);
                if (blocked.BlockedByShortage && blockedLedger.GetCensus().PublicationCount == before)
                {
                    GD.Print("[PASS] Check 10: Blocked run was refused and not archived.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 10: Blocked run was archived or succeeded.");

                // Check 11: capture/restore round-trips the archive and the tray.
                var state = ledger.CaptureState();
                var restored = new BroadsheetPressLedger();
                restored.RestoreState(state);
                var restoredCensus = restored.GetCensus();
                bool roundTrip = restoredCensus.PublicationCount == census.PublicationCount
                    && restored.Tray.TypeWearPermille == ledger.Tray.TypeWearPermille
                    && restored.FindPublication("pub_2") is { Day: 5 };
                if (roundTrip)
                {
                    GD.Print("[PASS] Check 11: Press capture/restore round-trips tray and archive.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 11: Press round-trip lost tray or archive state.");

                // Check 12: host wiring — save section, day heartbeat, canonical routing.
                string main = ReadRepoFile("src", "Main.BroadsheetPress.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                string vocab = ReadRepoFile("Assets", "Ashfall.Core", "Campaign", "DayEventVocabulary.cs");
                if (main.Contains("DebunkRumorWithPamphlet")
                    && main.Contains("_survivors.Needs.Modify")
                    && main.Contains("EnsureSharedSkillProgression")
                    && registry.Contains("broadsheet_press")
                    && registry.Contains("broadsheet_press_save.json")
                    && vocab.Contains("broadsheet_press_ticked"))
                {
                    GD.Print("[PASS] Check 12: Host routes morale/rumor/skill canonically and registers the press section.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 12: Press host wiring or save section missing.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during broadsheet press self-test: {ex}");
            }

            GD.Print($"=== Broadsheet Press Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { /* cleanup: optional probe file is unavailable */ }
            return string.Empty;
        }
    }
}
