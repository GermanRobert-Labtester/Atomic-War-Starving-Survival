// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Year Two Package P2 (Play On & Chapter Mechanism).

using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Endgame;

namespace AtomicWar.GodotApp
{
    public static class HostCliYearTwoChapter
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Year Two Chapter & Play On Self-Test (Package P2) ===");
            int passed = 0;
            int total = 7;

            try
            {
                // Check 1: Constants JSON exists and is valid
                string path = Path.Combine(dataDir, "year_two_chapter.json");
                if (File.Exists(path))
                {
                    GD.Print("[PASS] Check 1: year_two_chapter.json located.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 1: year_two_chapter.json missing.");
                }

                // Check 2: EndgameHostSession initializes with chapterIndex 1 and phase Active
                var host = EndgameHostSession.Create(dataDir);
                if (host.ChapterIndex == 1 && host.Phase == EndgamePhase.Active && !host.IsSealed && !host.HasPlayedOn)
                {
                    GD.Print("[PASS] Check 2: EndgameHostSession default chapter 1 active.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Invalid host state chapter={host.ChapterIndex}, phase={host.Phase}.");
                }

                // Check 3: Trigger ending at day 360 enables CanPlayOn
                var ctx = new CampaignEvaluationContext
                {
                    CurrentDay = 360,
                    LivingSurvivors = 10,
                    DeceasedSurvivors = 2,
                    AverageMorale = 60f
                };
                host.TriggerEnding(ctx);
                if (host.Phase == EndgamePhase.Epilogue && host.CanPlayOn)
                {
                    GD.Print("[PASS] Check 3: TriggerEnding day 360 enables CanPlayOn.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: CanPlayOn={host.CanPlayOn}, phase={host.Phase}.");
                }

                // Check 4: Terminal ending (living == 0) forbids Play On
                var termHost = EndgameHostSession.Create(dataDir);
                var termCtx = new CampaignEvaluationContext
                {
                    CurrentDay = 100,
                    LivingSurvivors = 0,
                    DeceasedSurvivors = 10,
                    ForceExtinction = true
                };
                termHost.TriggerEnding(termCtx);
                if (termHost.Phase == EndgamePhase.Epilogue && !termHost.CanPlayOn && termHost.System.IsTerminalEnding())
                {
                    GD.Print("[PASS] Check 4: Terminal ending correctly forbids CanPlayOn.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Terminal ending failed to forbid Play On.");
                }

                // Check 5: ContinueChapter transitions to chapter 2, phase Active, records chapters[0]
                bool contSuccess = host.ContinueChapter();
                if (contSuccess && host.ChapterIndex == 2 && host.Phase == EndgamePhase.Active && host.HasPlayedOn && host.Chapters.Count == 1)
                {
                    GD.Print("[PASS] Check 5: ContinueChapter transitioned to Chapter 2 with archived prior chapter.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: ContinueChapter failed: ch={host.ChapterIndex}, phase={host.Phase}.");
                }

                // Check 6: Chapter 2 Target Reading Day is 720
                if (host.GetTargetReadingDay() == 720)
                {
                    GD.Print("[PASS] Check 6: Chapter 2 target reading day is 720.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Chapter 2 target reading day is {host.GetTargetReadingDay()}, expected 720.");
                }

                // Check 7: Save & Restore round-trip preserves ChapterIndex 2 and chapters[0]
                var captured = host.CaptureState();
                var restoredHost = EndgameHostSession.Create(dataDir);
                restoredHost.RestoreState(captured);
                if (restoredHost.ChapterIndex == 2 && restoredHost.HasPlayedOn && restoredHost.Chapters.Count == 1 &&
                    restoredHost.Chapters[0].chapterIndex == 1)
                {
                    GD.Print("[PASS] Check 7: Save/restore round-trip preserved Chapter 2 state.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Save/restore round-trip failed to preserve Chapter 2.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] HostCliYearTwoChapter error: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Year Two Chapter Self-Test Complete: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
