// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : CampaignActionLogSelfTest
// Subsystem          : Deterministic campaign action log
// ============================================================================

using System;
using Ashfall.Core.PlayerCommand;

namespace AtomicWar.GodotApp
{
    public static class HostCliCampaignActionLog
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Campaign Action Log Self-Test ===");
            int passed = 0;
            const int total = 6;

            try
            {
                var session = CampaignActionLogHostSession.Create();

                if (session.EntryCount == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Campaign action log starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Log was not empty.");
                }

                // Sequence numbers must be monotonic and assigned by the Core log.
                long s1 = session.Record(1, "cmd_move", "surv_a", "loc_a", "ok");
                long s2 = session.Record(1, "cmd_take", "surv_a", "item_a", "ok");
                long s3 = session.Record(2, "cmd_move", "surv_b", "loc_b", "ok");
                if (s1 < s2 && s2 < s3 && session.EntryCount == 3)
                {
                    Console.WriteLine($"[PASS] Check 2: Sequence numbers are monotonic ({s1}, {s2}, {s3}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: {s1}, {s2}, {s3}, entries {session.EntryCount}.");
                }

                // Restore must preserve the sequence counter so it never re-issues.
                bool saved = session.TrySave();
                var reloaded = CampaignActionLogHostSession.Create();
                bool loaded = reloaded.TryLoad();
                long s4 = reloaded.Record(3, "cmd_move", "surv_a", "loc_a", "ok");
                if (saved && loaded && s4 == s3 + 1 && reloaded.EntryCount == 4)
                {
                    Console.WriteLine($"[PASS] Check 3: Restored log continues the sequence ({s3} -> {s4}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: saved={saved}, loaded={loaded}, next {s4}.");
                }

                // Day filtering must isolate the requested day's entries.
                var day1 = session.EntriesForDay(1);
                var day2 = reloaded.EntriesForDay(2);
                if (day1.Count == 2 && day2.Count == 1 && day2[0].Day == 2)
                {
                    Console.WriteLine($"[PASS] Check 4: Day filtering isolates entries ({day1.Count}/1, {day2.Count}/2).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: day1={day1.Count}, day2={day2.Count}.");
                }

                // Actor filtering must isolate one survivor's records.
                var actorA = session.EntriesForActor("surv_a");
                var actorB = session.EntriesForActor("surv_b");
                if (actorA.Count == 2 && actorB.Count == 1 && actorB[0].ActorId == "surv_b")
                {
                    Console.WriteLine($"[PASS] Check 5: Actor filtering isolates records ({actorA.Count}/a, {actorB.Count}/b).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: a={actorA.Count}, b={actorB.Count}.");
                }

                // A new campaign must reset the sequence to its initial value.
                reloaded.ClearForNewCampaign();
                var cleared = CampaignActionLogHostSession.Create();
                long fresh = cleared.Record(9, "cmd_scan", "surv_a", "loc_a", "ok");
                if (reloaded.EntryCount == 0 && fresh == 1 && cleared.EntryCount == 1
                    && CampaignActionLogSaveStore.SectionName.Equals("campaign_action_log", StringComparison.Ordinal)
                    && CampaignActionLogSaveStore.FileName.Equals("campaign_action_log_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 6: New-campaign reset and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: cleared={reloaded.EntryCount}, fresh={fresh}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Campaign Action Log Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
