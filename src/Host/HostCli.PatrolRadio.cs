// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PatrolRadioSelfTest
// Subsystem          : Patrol encounter → radio broadcast one-shot bridge.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public static class HostCliPatrolRadio
    {
        private const string PatrolEncounter = "enc_patrol_warlord_raid";
        private const string PatrolChoice = "choice_warlord_comply";
        private const string PatrolBroadcast = "radio_patrol_warlord_raid";

        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        /// <summary>
        /// Builds the LIVE travel encounter owner the way the campaign does: the
        /// authored travel_encounters.json catalog, no fabricated rows.
        /// </summary>
        private static TravelEncounterSystem BuildTravelOwner(string dataDir)
        {
            var io = CatalogPath.CreateFileIOForDataDir(dataDir);
            var catalog = TravelEncounterCatalog.LoadFromDirectory(dataDir, io) ?? new TravelEncounterCatalog();
            return new TravelEncounterSystem(catalog);
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Patrol Radio Hooks Self-Test ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string dataRoot = !string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir();

                // The live radio owner and the live travel encounter owner.
                var radio = RadioHostSession.Create(dataRoot);
                var travel = BuildTravelOwner(dataRoot);

                // 1. The authored encounter→broadcast map resolves.
                Check(PatrolRadioHooks.TryGetRadioSignalForEncounter(PatrolEncounter, out string mapped)
                      && mapped == PatrolBroadcast,
                    $"Check 1: authored patrol encounter maps to its broadcast ({PatrolEncounter} -> {mapped}).");
                passed += mapped == PatrolBroadcast ? 1 : 0;

                // 2. Unmapped encounters produce no signal.
                Check(!PatrolRadioHooks.TryGetRadioSignalForEncounter("enc_not_a_patrol", out string unmapped)
                      && string.IsNullOrEmpty(unmapped),
                    "Check 2: an unmapped encounter produces no radio signal.");
                passed += !PatrolRadioHooks.TryGetRadioSignalForEncounter("enc_not_a_patrol", out _) ? 1 : 0;

                // 3. The faction capability table is authored, not invented.
                Check(PatrolRadioHooks.IsFactionRadioCapable("iron_garrison")
                      && !PatrolRadioHooks.IsFactionRadioCapable("faction_scavengers")
                      && !PatrolRadioHooks.IsFactionRadioCapable("faction_not_real")
                      && !PatrolRadioHooks.IsFactionRadioCapable(string.Empty),
                    "Check 3: faction radio-capability table honours authored rows only.");
                passed += PatrolRadioHooks.IsFactionRadioCapable("iron_garrison") ? 1 : 0;

                // 4. Subscribing the live travel owner wires the bridge.
                var session = new PatrolRadioHostSession(() => radio);
                Check(session.Subscribe(travel) && session.PendingCount == 0,
                    "Check 4: bridge subscribes to the live travel encounter owner.");
                passed += 1;

                // 5. Resolving the patrol encounter through the LIVE owner's own
                //    ResolveChoice path queues exactly one broadcast signal.
                bool resolved = travel.ResolveChoice(PatrolEncounter, PatrolChoice, 4, out _, out _, out _);
                Check(resolved && session.PendingCount == 1 && session.PendingSignals[0] == PatrolBroadcast,
                    "Check 5: a resolved patrol choice queues exactly one broadcast signal.");
                passed += resolved && session.PendingCount == 1 ? 1 : 0;

                // 6. Re-resolving the same encounter does not duplicate the signal.
                travel.ResolveChoice(PatrolEncounter, PatrolChoice, 4, out _, out _, out _);
                Check(session.PendingCount == 1,
                    "Check 6: repeating the same encounter choice does not duplicate the signal.");
                passed += session.PendingCount == 1 ? 1 : 0;

                // 7. Draining delivers through the canonical radio intercept log.
                int beforeHistory = radio.History.Count;
                int dispatched = session.DispatchPending().Count;
                var last = radio.History.Count > 0 ? radio.History[radio.History.Count - 1] : default;
                Check(dispatched == 1 && radio.History.Count == beforeHistory + 1
                      && !string.IsNullOrEmpty(last.Message),
                    "Check 7: the queued signal is delivered through the canonical radio intercept log.");
                passed += dispatched == 1 && radio.History.Count == beforeHistory + 1 ? 1 : 0;

                // 8. The queue is one-shot: a second drain delivers nothing.
                int secondDrain = session.DispatchPending().Count;
                Check(secondDrain == 0 && session.PendingCount == 0,
                    "Check 8: the queue is one-shot; a second drain delivers nothing.");
                passed += secondDrain == 0 ? 1 : 0;

                // 9. The queue survives a save/load round-trip.
                var queued = new PatrolRadioHostSession(() => radio);
                queued.Subscribe(travel);
                travel.ResolveChoice(PatrolEncounter, PatrolChoice, 40, out _, out _, out _);
                var captured = queued.CaptureState();
                var persisted = PatrolRadioSaveStore.TryCapturePersisted(captured);
                var restored = PatrolRadioSaveStore.TryRestorePersisted(persisted);
                var reloaded = new PatrolRadioHostSession(() => radio, restored);
                Check(restored != null && reloaded.PendingCount == 1 && reloaded.PendingSignals[0] == PatrolBroadcast,
                    "Check 9: the pending queue survives a save/load round-trip.");
                passed += restored != null && reloaded.PendingCount == 1 ? 1 : 0;

                // 10. Unsubscribing stops the bridge from queueing.
                var travel2 = BuildTravelOwner(dataRoot);
                var detached = new PatrolRadioHostSession(() => radio);
                detached.Subscribe(travel2);
                detached.Unsubscribe(travel2);
                travel2.ResolveChoice(PatrolEncounter, PatrolChoice, 6, out _, out _, out _);
                Check(detached.PendingCount == 0,
                    "Check 10: after unsubscribing, encounter choices no longer queue signals.");
                passed += detached.PendingCount == 0 ? 1 : 0;

                // 11. Unknown broadcast ids are consumed but never become intercepts.
                int historyBefore = radio.History.Count;
                var misfire = new PatrolRadioHostSession(() => radio);
                misfire.QueueSignal("radio_not_a_real_broadcast");
                int drained = misfire.DispatchPending().Count;
                Check(drained == 1 && radio.History.Count == historyBefore,
                    "Check 11: an unknown broadcast id is consumed without fabricating an intercept.");
                passed += drained == 1 && radio.History.Count == historyBefore ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Patrol Radio Hooks Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
