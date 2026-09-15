// SPDX-License-Identifier: MIT
// ============================================================================
// Tasks 5–8 Wave E — Task 7 vinyl reconciliation (§8.6–§8.11).
//
// MODEL DECISION (§8.2): the repository already uses MODEL A — individual
// playable records. The three vinyl collectibles are registered acquisition
// routes in `VinylRecordAcquisitionMap` mapping each physical item to real
// `record_id`s (chamber → record_03 very_rare, civil broadcast → record_04
// rare, folk compilation → record_02 uncommon). No new definitions authored.
//
// MORALE OWNERSHIP RULE (§8.10, permanent contract):
//   Vinyl collectibles delegate morale ownership to VinylMoraleSystem.
//   First discovery registers the record exactly once; collectible pickup
//   applies ZERO independent morale. The single morale authority is
//   `ApplyDailyEffect` (once per day while the turntable plays).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public sealed class CollectibleVinylIntegrationTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "collectibles.json"))) return candidate;
                dir = Path.GetDirectoryName(dir);
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static CollectibleCatalog LoadCatalog()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        /// <summary>The real host path: archive → VinylRecordDefinition catalog.</summary>
        private static VinylMoraleSystem BuildVinylSystem()
        {
            var system = new VinylMoraleSystem();
            string path = Path.Combine(DataDir, "narrative", "vinyl_record_archive.json");
            Assert.True(File.Exists(path), "vinyl record archive missing");
            var file = new SystemTextJsonSerializer().Deserialize<VinylRecordsFile>(File.ReadAllText(path));
            Assert.NotNull(file?.records);
            var defs = new List<VinylRecordDefinition>();
            foreach (var r in file!.records!)
            {
                if (r == null || string.IsNullOrEmpty(r.record_id)) continue;
                defs.Add(new VinylRecordDefinition
                {
                    record_id = r.record_id,
                    display_name = !string.IsNullOrEmpty(r.title) ? r.title : r.record_id,
                    genre = (r.tags != null && r.tags.Length > 0) ? r.tags[0] : string.Empty,
                    morale_daily_bonus = r.daily_morale_modifier,
                    flashback_suppression = 0f,
                    audio_cue_id = string.Empty,
                    description = r.dweller_resonance_notes ?? string.Empty
                });
            }
            system.LoadCatalog(defs);
            Assert.Equal(30, system.State.ownedRecordIds.Count == 0 ? defs.Count : -1 + 1); // 30 authored records
            return system;
        }

        private static (CollectibleEffectDispatcher dispatcher, CollectibleDiscoveryState discovery) BuildDispatcher(CollectibleCatalog catalog)
        {
            var discovery = new CollectibleDiscoveryState();
            return (new CollectibleEffectDispatcher(catalog, discovery), discovery);
        }

        private static string VinylItemIdFor(string collectibleItemId)
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog.GetByItemId(collectibleItemId));
            Assert.Equal("vinyl", catalog.GetByItemId(collectibleItemId)!.category);
            return collectibleItemId;
        }

        // ── §8.6: registration tests — three explicit collectibles ─────

        [Fact]
        public void ChamberRecordCollectible_RegistersRecord_NoDuplicateOnReacquire()
            => AssertVinylRegistration(VinylRecordAcquisitionMap.ChamberRecordCollectibleItemId);

        [Fact]
        public void CivilBroadcastCollectible_RegistersRecord_NoDuplicateOnReacquire()
            => AssertVinylRegistration(VinylRecordAcquisitionMap.CivilBroadcastCollectibleItemId);

        [Fact]
        public void FolkCompilationCollectible_RegistersRecord_NoDuplicateOnReacquire()
            => AssertVinylRegistration(VinylRecordAcquisitionMap.FolkCompilationCollectibleItemId);

        private static void AssertVinylRegistration(string collectibleItemId)
        {
            VinylItemIdFor(collectibleItemId);
            var system = BuildVinylSystem();
            var catalog = LoadCatalog();
            var (dispatcher, discovery) = BuildDispatcher(catalog);

            // Host path (OnCollectibleItemAdded): vinyl map FIRST, dispatcher SECOND.
            var recordId = VinylRecordAcquisitionMap.TryAcquireFromItem(collectibleItemId, system, new SeededRng(7));
            Assert.NotNull(recordId);
            Assert.Contains(recordId!, system.State.ownedRecordIds);          // record owned
            Assert.Equal(1, system.State.ownedRecordIds.Count);               // exactly one record

            var dispatch = dispatcher.DispatchOnAcquire(collectibleItemId);
            Assert.True(dispatch.IsCollectible);
            Assert.True(dispatch.EffectApplied);                              // "none" branch: discovery only
            Assert.True(discovery.IsDiscovered(collectibleItemId));           // collectible discovered
            Assert.Equal(1, system.State.ownedRecordIds.Count);               // dispatcher adds no records

            // The vinyl catalog resolves the record (UI ownership flow).
            Assert.NotNull(system.GetRecord(recordId!));

            // Reacquire: no duplicate of the SAME record; no morale from either path.
            var reacquired = VinylRecordAcquisitionMap.TryAcquireFromItem(collectibleItemId, system, new SeededRng(7));
            if (reacquired != null)
                Assert.NotEqual(recordId, reacquired);                        // authored fallback: a NEW title, never the same record twice
            Assert.DoesNotContain(system.State.ownedRecordIds, id => system.State.ownedRecordIds.Count(x => x == id) > 1);
        }

        // ── §8.7: the critical double-award regression test ────────────

        [Fact]
        public void VinylPickup_AppliesZeroMorale_PlaybackAppliesExactlyOneDailyEffect()
        {
            var system = BuildVinylSystem();
            var catalog = LoadCatalog();
            var (dispatcher, discovery) = BuildDispatcher(catalog);
            var needs = new NeedsSystem();
            needs.Register(new SurvivorNeedsState { Id = "s1" });
            var survivor = needs.Get("s1")!;
            float moraleBefore = survivor.Morale;

            const string chamberItem = VinylRecordAcquisitionMap.ChamberRecordCollectibleItemId;

            // 1. Pickup: vinyl registration + discovery — ZERO morale.
            var recordId = VinylRecordAcquisitionMap.TryAcquireFromItem(chamberItem, system, new SeededRng(7))!;
            var dispatch = dispatcher.DispatchOnAcquire(chamberItem);
            Assert.True(dispatch.EffectApplied);
            Assert.Equal(moraleBefore, survivor.Morale);                      // pickup morale = none
            Assert.Equal(0f, system.State.totalMoraleApplied);

            // 2. Playback through VinylMoraleSystem (the ONE morale authority).
            var host = new List<float>();
            system.OnMoraleApplied += amount => host.Add(amount);
            var play = system.Play(recordId!, day: 10);
            Assert.True(play.IsSuccess);
            system.ApplyDailyEffect(10);                                      // host daily route
            Assert.Single(host);                                              // exactly one morale effect
            var record = system.GetRecord(recordId!)!;
            Assert.Equal(record.morale_daily_bonus, host[0], 3);              // the authored bonus, nothing more
            float totalAfterPlay = system.State.totalMoraleApplied;

            // 3. Reacquire: no additional vinyl morale.
            VinylRecordAcquisitionMap.TryAcquireFromItem(chamberItem, system, new SeededRng(7));
            dispatcher.DispatchOnAcquire(chamberItem);
            Assert.Single(host);                                              // never re-awarded from pickup
            Assert.Equal(totalAfterPlay, system.State.totalMoraleApplied);    // playback ledger stable
        }

        [Fact]
        public void VinylDailyEffect_IsOncePerDay_WhilePlaying()
        {
            var system = BuildVinylSystem();
            var recordId = VinylRecordAcquisitionMap.TryAcquireFromItem(
                VinylRecordAcquisitionMap.CivilBroadcastCollectibleItemId, system, new SeededRng(3));
            system.Play(recordId!, day: 5);

            int applied = 0;
            system.OnMoraleApplied += _ => applied++;
            system.ApplyDailyEffect(5);
            system.ApplyDailyEffect(5);   // same day: anti-spam guard
            system.ApplyDailyEffect(6);   // next day while still playing: one more
            Assert.Equal(2, applied);
        }

        // ── §8.8: save/load ────────────────────────────────────────────

        [Fact]
        public void VinylAcquisition_SaveRestore_OwnedAndDiscovered_ReacquireDoesNotReRegister()
        {
            var system = BuildVinylSystem();
            var catalog = LoadCatalog();
            var (dispatcher, discovery) = BuildDispatcher(catalog);
            const string folkItem = VinylRecordAcquisitionMap.FolkCompilationCollectibleItemId;

            var recordId = VinylRecordAcquisitionMap.TryAcquireFromItem(folkItem, system, new SeededRng(11));
            dispatcher.DispatchOnAcquire(folkItem);

            // Host persists vinyl state + collectible discovery independently (§10).
            var vinylState = system.CaptureState();
            var discoveryState = discovery.CaptureState();

            var system2 = new VinylMoraleSystem();
            // (Catalog reload happens at host startup; state restore next.)
            system2.RestoreState(vinylState);
            var discovery2 = new CollectibleDiscoveryState();
            discovery2.RestoreState(discoveryState);
            var dispatcher2 = new CollectibleEffectDispatcher(catalog, discovery2);

            Assert.Contains(recordId!, system2.State.ownedRecordIds);   // record remains owned
            Assert.True(discovery2.IsDiscovered(folkItem));             // collectible discovered

            var second = dispatcher2.DispatchOnAcquire(folkItem);
            Assert.True(second.AlreadyDiscovered);                      // no re-register via dispatcher
            Assert.Contains(recordId!, system2.State.ownedRecordIds);   // same record still owned exactly once
            Assert.Equal(1, system2.State.ownedRecordIds.Count(id => id == recordId));
        }

        [Fact]
        public void VinylCatalogRecords_ResolveForUi()
        {
            var system = BuildVinylSystem();
            foreach (var collectibleItem in new[]
                     {
                         VinylRecordAcquisitionMap.ChamberRecordCollectibleItemId,
                         VinylRecordAcquisitionMap.CivilBroadcastCollectibleItemId,
                         VinylRecordAcquisitionMap.FolkCompilationCollectibleItemId
                     })
            {
                var recordId = VinylRecordAcquisitionMap.TryAcquireFromItem(collectibleItem, system, new SeededRng(5));
                Assert.NotNull(recordId);
                var record = system.GetRecord(recordId!);
                Assert.NotNull(record);
                Assert.False(string.IsNullOrEmpty(record!.display_name));
                Assert.True(record.morale_daily_bonus > 0f);
            }
        }
    }
}
