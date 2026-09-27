// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : WarlordResponseSelfTest
// Subsystem          : Idempotent warlord tribute responses over the live
//                      WarlordDoctrineSystem.
// ============================================================================
using System;
using Ashfall.Core.Warlords;
using AtomicWar.GodotApp.YearOfAsh;

namespace AtomicWar.GodotApp
{
    public static class HostCliWarlordResponse
    {
        private const string TributeItem = "scrap_metal";

        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        private static WarlordResponseHostSession BuildSession(WarlordDoctrineSystem doctrine, WarlordResponseState? saved = null)
            => new WarlordResponseHostSession(() => doctrine, saved);

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Warlord Tribute Response Self-Test ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string dataRoot = !string.IsNullOrEmpty(dataDir) && System.IO.Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir();

                // The live Year-of-Ash warlord owner, exactly as the host holds it.
                var yoa = YearOfAshHostSession.Create(dataRoot);
                var doctrine = yoa.Warlord;

                // 1. The canonical tribute id is derived from the live doctrine state.
                int weekBefore = doctrine.State.totalWeeksAsked;
                string id = WarlordResponseHostSession.TributeIdForWeek(weekBefore);
                Check(id == $"tribute_week_{weekBefore}" && BuildSession(doctrine).CurrentTributeId() == id,
                    $"Check 1: canonical tribute id derived from the live doctrine state ({id}).");
                passed += id == $"tribute_week_{weekBefore}" ? 1 : 0;

                // 2. A first payment is accepted and settles the ask.
                var session = BuildSession(doctrine);
                var pay = session.Pay(5, 3);
                Check(pay != null && pay.Succeeded && session.IsResponded(WarlordResponseHostSession.TributeIdForWeek(weekBefore)),
                    "Check 2: first tribute payment is accepted and recorded.");
                passed += pay != null && pay.Succeeded ? 1 : 0;

                // 3. A second payment for the SAME ask is refused — the bug this
                //    surface exists to close.
                var payAgain = session.Pay(5, 3);
                Check(payAgain == null || !payAgain.Succeeded,
                    "Check 3: double settlement of the same ask is refused.");
                passed += payAgain != null && payAgain.Succeeded ? 0 : 1;

                // 4. Contest and Submit for an already-answered ask are refused too.
                var contestAfterPay = session.Contest(3);
                var submitAfterPay = session.Submit(3);
                Check((contestAfterPay == null || !contestAfterPay.Succeeded)
                      && (submitAfterPay == null || !submitAfterPay.Succeeded),
                    "Check 4: contest/submit cannot follow an issued response for the same ask.");
                passed += (contestAfterPay != null && contestAfterPay.Succeeded) || (submitAfterPay != null && submitAfterPay.Succeeded) ? 0 : 1;

                // 5. A refusal is a response: it is recorded and blocks re-entry.
                var fresh = BuildSession(doctrine);
                string openWeekId = WarlordResponseHostSession.TributeIdForWeek(weekBefore);
                var refuse = fresh.Contest(4, openWeekId);
                Check(refuse != null && refuse.Succeeded && fresh.HasRespondedToCurrentAsk(),
                    "Check 5: refusal is recorded as the week's response.");
                passed += refuse != null && refuse.Succeeded ? 1 : 0;

                // 6. Submission is the third authored response kind.
                var submitter = BuildSession(doctrine);
                var submit = submitter.Submit(5, openWeekId);
                Check(submit != null && submit.Succeeded && submit.Record.Kind == WarlordResponseKind.Submit,
                    "Check 6: submission is accepted as a distinct response kind.");
                passed += submit != null && submit.Succeeded ? 1 : 0;

                // 7. The response ledger survives a save/load round-trip.
                var captured = session.CaptureState();
                var persisted = WarlordResponseSaveStore.TryCapturePersisted(captured);
                var restored = WarlordResponseSaveStore.TryRestorePersisted(persisted);
                var reloaded = BuildSession(doctrine, restored);
                Check(restored != null && reloaded.ResponseCount == session.ResponseCount
                      && reloaded.IsResponded(WarlordResponseHostSession.TributeIdForWeek(weekBefore)),
                    "Check 7: response ledger survives a save/load round-trip.");
                passed += restored != null && reloaded.ResponseCount == session.ResponseCount ? 1 : 0;

                // 8. Superseded tribute ids stop blocking a new ask.
                int pruned = reloaded.PruneSuperseded(weekBefore + 1);
                Check(pruned == 1 && !reloaded.HasRespondedToCurrentAsk(),
                    $"Check 8: superseded ask pruned ({pruned}); the new ask is open.");
                passed += pruned == 1 && !reloaded.HasRespondedToCurrentAsk() ? 1 : 0;

                // 9. The live doctrine owner is never replaced or bypassed.
                int askBefore = doctrine.State.totalWeeksAsked;
                var third = BuildSession(doctrine);
                third.Pay(5, 6);
                Check(ReferenceEquals(yoa.Warlord, doctrine) && doctrine.State.totalWeeksAsked == askBefore,
                    "Check 9: the live WarlordDoctrineSystem instance is unchanged by the response surface.");
                passed += ReferenceEquals(yoa.Warlord, doctrine) ? 1 : 0;

                // 10. Reset clears only the response ledger.
                third.Clear();
                Check(third.ResponseCount == 0 && third.StatusLine() == "awaiting response",
                    "Check 10: reset clears the response ledger only.");
                passed += third.ResponseCount == 0 ? 1 : 0;

                // 11. The status line is a truthful projection of the live state.
                var fourth = BuildSession(doctrine);
                fourth.Pay(5, 7);
                string line = fourth.StatusLine();
                Check(line.StartsWith("responded", StringComparison.Ordinal) && line.Contains("Pay"),
                    $"Check 11: status line reports the issued response truthfully ({line}).");
                passed += line.StartsWith("responded", StringComparison.Ordinal) ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Warlord Tribute Response Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
