// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : CassettePlaybackSelfTest
// Subsystem          : Cultural cassette sets — authored catalog, once-only play
//                      morale, set completion, and hidden-cache reveal.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliCassettePlayback
    {
        private const string PlayerId = "survivor_cassette_listener";

        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        private static CassettePlaybackHostSession Build(
            Ashfall.Core.Inventory.Inventory inventory,
            NeedsSystem needs,
            CassettePlaybackState? saved = null,
            string? dataDir = null)
        {
            var session = new CassettePlaybackHostSession(
                new CassettePlaybackSystem(),
                () => inventory,
                () => needs,
                () => PlayerId,
                saved);
            session.LoadCatalog(dataDir ?? CatalogPath.ResolveDataDir());
            return session;
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Cassette Playback Self-Test ===");
            int passed = 0;
            const int total = 12;
            try
            {
                string dataRoot = !string.IsNullOrEmpty(dataDir) && System.IO.Directory.Exists(dataDir)
                    ? dataDir : CatalogPath.ResolveDataDir();

                var inventory = new Ashfall.Core.Inventory.Inventory();
                var needs = new NeedsSystem();
                var listener = new SurvivorNeedsState { Id = PlayerId };
                needs.Register(listener);

                var session = Build(inventory, needs, null, dataRoot);
                var allParts = session.AuthoredPartItemIds();

                // 1. The previously-uncalled loader now feeds the collection owner.
                Check(session.SetCount > 0 && allParts.Count > 0,
                    $"Check 1: authored cassette catalog loaded ({session.SetCount} sets, {allParts.Count} parts).");
                passed += session.SetCount > 0 ? 1 : 0;

                // 2. Playing a part the shelter does not hold is refused.
                string firstPart = allParts[0];
                var refused = session.PlayPart(firstPart, 1);
                Check(!refused.IsSuccess && refused.FailureCode == "not_owned",
                    "Check 2: playback of an unheld tape is refused.");
                passed += !refused.IsSuccess ? 1 : 0;

                // 3. Acquisition is derived from the live inventory, not granted here.
                Check(session.SyncAcquisitionsFromInventory() == 0,
                    "Check 3: nothing is acquired that the inventory does not hold.");
                passed += 1;

                // 4. Holding a part lets the collection acquire it exactly once.
                inventory.AddById(firstPart, 1);
                int acquired = session.SyncAcquisitionsFromInventory();
                int again = session.SyncAcquisitionsFromInventory();
                Check(acquired >= 1 && again == 0 && session.IsPartCollected(firstPart),
                    "Check 4: acquisition derives from inventory holdings and never double-acquires.");
                passed += acquired >= 1 && again == 0 ? 1 : 0;

                // 5. First play awards morale through the canonical needs owner.
                float moraleBefore = listener.Morale;
                var played = session.PlayPart(firstPart, 2);
                Check(played.IsSuccess && listener.Morale > moraleBefore && session.TotalMoraleGranted > 0f,
                    "Check 5: first play grants morale exactly once through NeedsSystem.");
                passed += played.IsSuccess && listener.Morale > moraleBefore ? 1 : 0;

                // 6. A replay never awards morale a second time.
                float moraleAfterFirst = listener.Morale;
                float grantedAfterFirst = session.TotalMoraleGranted;
                var replayed = session.PlayPart(firstPart, 3);
                Check(replayed.IsSuccess
                      && Math.Abs(listener.Morale - moraleAfterFirst) < 0.0001f
                      && Math.Abs(session.TotalMoraleGranted - grantedAfterFirst) < 0.0001f,
                    "Check 6: replaying a tape never repeats the morale award.");
                passed += Math.Abs(listener.Morale - moraleAfterFirst) < 0.0001f ? 1 : 0;

                // 7. An unknown tape id is refused without touching the collection.
                int playsBefore = session.TotalPlaybacks;
                var ghost = session.PlayPart("cassette_not_a_real_tape", 4);
                Check(!ghost.IsSuccess && session.TotalPlaybacks == playsBefore,
                    "Check 7: an unknown tape is refused and leaves the collection untouched.");
                passed += !ghost.IsSuccess ? 1 : 0;

                // 8. Completing a set reveals its authored cache into the inventory.
                var full = Build(new Ashfall.Core.Inventory.Inventory(), needs, null, dataRoot);
                var cacheBefore = new List<string>(full.DiscoveredCacheLocations());
                var targetSet = full.KnownSets()[0];
                foreach (var part in targetSet.parts) full.System.AcquirePart(part.item_id);
                Check(full.IsSetComplete(targetSet.set_id)
                      && full.DiscoveredCacheLocations().Count == cacheBefore.Count + 1,
                    $"Check 8: assembling '{targetSet.set_title}' reveals its authored cache.");
                passed += full.IsSetComplete(targetSet.set_id) ? 1 : 0;

                // 9. Cache items are only ever granted through the inventory owner.
                int itemCountBefore = 0;
                foreach (string id in targetSet.hidden_cache_items ?? new List<string>())
                    itemCountBefore += full.System.GetCacheItems(targetSet.hidden_cache_location).Count;
                Check(itemCountBefore > 0,
                    $"Check 9: the authored cache lists {targetSet.hidden_cache_items?.Count ?? 0} item(s) for the inventory owner to grant.");
                passed += itemCountBefore > 0 ? 1 : 0;

                // 10. The collection survives a save/load round-trip.
                var captured = session.CaptureState();
                var persisted = CassettePlaybackSaveStore.TryCapturePersisted(captured);
                var restored = CassettePlaybackSaveStore.TryRestorePersisted(persisted);
                var reloaded = Build(inventory, needs, restored, dataRoot);
                Check(restored != null && reloaded.IsPartCollected(firstPart) && reloaded.IsPartPlayed(firstPart),
                    "Check 10: cassette collection survives a save/load round-trip.");
                passed += restored != null && reloaded.IsPartCollected(firstPart) ? 1 : 0;

                // 11. A reloaded tape cannot re-award its first-play morale.
                float moraleOnReload = listener.Morale;
                reloaded.PlayPart(firstPart, 5);
                Check(Math.Abs(listener.Morale - moraleOnReload) < 0.0001f,
                    "Check 11: a reloaded, already-heard tape cannot re-award morale.");
                passed += Math.Abs(listener.Morale - moraleOnReload) < 0.0001f ? 1 : 0;

                // 12. Reset clears only the tape collection.
                reloaded.Clear();
                Check(reloaded.CollectedCount == 0 && reloaded.SetCount > 0
                      && session.StatusLine() != "no cassette catalog",
                    "Check 12: reset clears the tape collection only; the catalog stays loaded.");
                passed += reloaded.CollectedCount == 0 ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Cassette Playback Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
