// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Save;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    /// <summary>
    /// Save-fuzz battery (ashfall-save-fuzz skill, Plan 85 follow-up) —
    /// stresses the wasteland_map save section's RegisteredMapFragments
    /// list: the single persisted fragment-progress authority for the
    /// damaged-map treasure-hunt layer.
    ///
    /// Battery coverage:
    ///   1. clean round-trip through the live DamagedMapSystem (partial +
    ///      complete zones; no completion/reveal re-fire after restore);
    ///   2. checksummed-envelope round-trip via SaveEnvelopeHelper (the
    ///      exact contract WastelandMapSaveStore uses through SaveStoreHub);
    ///   3. checksum mutation → rejected with the documented error;
    ///   4. null/empty checksum on the new-format envelope → rejected
    ///      (no silent legacy fallback);
    ///   5. bare-state payload → rejected when the legacy fallback is
    ///      disabled (the wasteland_map section's strictness);
    ///   6. byte-for-byte serialization determinism across captures and
    ///      restore generations;
    ///   7. seeded fuzz sweep over random fragment subsets — registration
    ///      set is stable across round-trips and duplicates never double-count;
    ///   8. unknown (rolled-back-catalog) fragment ids in an old save are
    ///      inert, do not corrupt zone progress, and survive round-trips.
    /// </summary>
    public sealed class WastelandMapFragmentPersistenceFuzzTests
    {
        private readonly string _dataDir;
        private readonly IFileIO _files;
        private readonly IJsonSerializer _json;

        public WastelandMapFragmentPersistenceFuzzTests()
        {
            _dataDir = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (!Directory.Exists(_dataDir))
                _dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            _files = new FileSystemIO();
            _json = new SystemTextJsonSerializer();
        }

        private (DamagedMapSystem system, WastelandMapSystem map, List<DamagedMapZone> zones) CreateLive(
            WastelandMapState? state = null)
        {
            var (nodes, routes) = WastelandMapCatalogLoader.Load(_dataDir, _files, _json);
            var map = new WastelandMapSystem(state ?? new WastelandMapState(), nodes, routes);
            var (zones, errors) = DamagedMapCatalogLoader.LoadWithValidation(_dataDir, _files, _json);
            Assert.Empty(errors);
            return (new DamagedMapSystem(zones, map), map, zones);
        }

        // ── 1. Clean round-trip through the live system ───────────────

        [Fact]
        public void RegisteredFragments_CleanRoundTrip_PreservesProgressAndNeverRefires()
        {
            var (system, map, zones) = CreateLive();
            var partial = zones.First(z => z.Fragments.Count >= 3);
            var complete = zones.Last(z => z.Fragments.Count >= 2 && z.ZoneId != partial.ZoneId);

            // Partial progress on one zone; full completion + reveal on another.
            system.RegisterFragment(partial.Fragments[0].fragment_id);
            foreach (var f in complete.Fragments)
                system.RegisterFragment(f.fragment_id);

            string completeNode = DamagedMapSystem.ResolveRevealNodeId(complete.InstallationId)!;
            Assert.True(map.IsDiscovered(completeNode));

            // Save boundary: capture → serialize → deserialize → restore.
            string json = _json.Serialize(map.CaptureState());
            var restoredState = _json.Deserialize<WastelandMapState>(json);
            Assert.NotNull(restoredState);

            var (restoredSystem, restoredMap, restoredZones) = CreateLive(restoredState);

            Assert.Equal(1, restoredSystem.RegisteredCount(partial.ZoneId));
            Assert.True(restoredSystem.IsZoneComplete(complete.ZoneId));
            Assert.True(restoredSystem.IsInstallationRevealed(complete.ZoneId));
            Assert.True(restoredMap.IsDiscovered(completeNode));
            Assert.False(restoredSystem.IsInstallationRevealed(partial.ZoneId));

            // Idempotence across the save boundary: replaying the fragments
            // that were actually registered must not re-fire completion or
            // double-register; completing the partial zone fires exactly once.
            int completions = 0;
            restoredSystem.OnZoneCompleted += _ => completions++;
            Assert.False(restoredSystem.RegisterFragment(partial.Fragments[0].fragment_id));
            foreach (var f in complete.Fragments)
                Assert.False(restoredSystem.RegisterFragment(f.fragment_id));
            Assert.Equal(0, completions);
            Assert.Equal(1, restoredSystem.RegisteredCount(partial.ZoneId));
        }

        // ── 2. Checksummed-envelope round-trip (host store contract) ──

        [Fact]
        public void RegisteredFragments_ChecksummedEnvelope_RoundTripsViaSaveEnvelopeHelper()
        {
            var (system, map, zones) = CreateLive();
            var zone = zones[0];
            foreach (var f in zone.Fragments)
                system.RegisterFragment(f.fragment_id);

            WastelandMapState captured = map.CaptureState();
            Assert.NotEmpty(captured.RegisteredMapFragments);

            string envelopeJson = SaveEnvelopeHelper.CaptureEnvelope(captured, _json);
            Assert.Contains("Checksum", envelopeJson);

            var (success, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
                envelopeJson, _json, legacyFallback: null, allowBareFallback: false);
            Assert.True(success, error);
            Assert.NotNull(restored);

            var (restoredSystem, _, _) = CreateLive(restored);
            Assert.True(restoredSystem.IsZoneComplete(zone.ZoneId));
            foreach (var f in zone.Fragments)
                Assert.Contains(f.fragment_id, restored!.RegisteredMapFragments);
        }

        // ── 3. Checksum mutation is rejected ──────────────────────────

        [Fact]
        public void RegisteredFragments_ChecksumMutation_IsRejectedWithDocumentedError()
        {
            var (system, map, zones) = CreateLive();
            foreach (var f in zones[0].Fragments)
                system.RegisterFragment(f.fragment_id);

            string envelopeJson = SaveEnvelopeHelper.CaptureEnvelope(map.CaptureState(), _json);

            // Tamper with the persisted fragment list inside the envelope.
            string fragmentId = zones[0].Fragments[0].fragment_id;
            Assert.Contains(fragmentId, envelopeJson);
            string tampered = envelopeJson.Replace(fragmentId, "damaged_map_tampered_x", StringComparison.Ordinal);
            Assert.NotEqual(envelopeJson, tampered);

            var (success, _, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
                tampered, _json, legacyFallback: null, allowBareFallback: false);
            Assert.False(success);
            Assert.Contains("Checksum mismatch", error);
        }

        // ── 4. Null/empty checksum on new-format envelope is rejected ──

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RegisteredFragments_MissingChecksum_IsRejectedWithoutLegacyFallback(string? checksum)
        {
            var (system, map, zones) = CreateLive();
            foreach (var f in zones[0].Fragments)
                system.RegisterFragment(f.fragment_id);

            var envelope = new SaveEnvelope<WastelandMapState> { State = map.CaptureState(), Checksum = checksum };
            string json = _json.Serialize(envelope);

            var (success, _, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
                json, _json, legacyFallback: null, allowBareFallback: false);
            Assert.False(success);
            Assert.Contains("Checksum field missing", error);
        }

        // ── 5. Bare-state payload is rejected (wasteland_map strictness) ──

        [Fact]
        public void RegisteredFragments_BareStatePayload_IsRejectedWhenLegacyFallbackDisabled()
        {
            var (system, map, zones) = CreateLive();
            foreach (var f in zones[0].Fragments)
                system.RegisterFragment(f.fragment_id);

            // Pre-envelope bare state, exactly as old sections stored it.
            string bareJson = _json.Serialize(map.CaptureState());
            Assert.DoesNotContain("Checksum", bareJson);

            var (success, _, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
                bareJson, _json, legacyFallback: null, allowBareFallback: false);
            Assert.False(success);
            Assert.NotNull(error);
        }

        // ── 6. Serialization determinism ──────────────────────────────

        [Fact]
        public void RegisteredFragments_Serialization_IsByteForByteDeterministic()
        {
            var (system, map, _) = CreateLive();
            var rng = new SeededRng(20260907);
            var allFragments = system.Zones.SelectMany(z => z.Fragments).Select(f => f.fragment_id).ToList();

            // Register a deterministic subset.
            foreach (var f in allFragments)
                if (rng.Next(0, 100) < 60)
                    system.RegisterFragment(f);

            string jsonA = _json.Serialize(map.CaptureState());
            string jsonB = _json.Serialize(map.CaptureState());
            Assert.Equal(jsonA, jsonB);

            // Restore into a fresh system and re-capture: second generation
            // must serialize identically (no drift across save generations).
            var restoredState = _json.Deserialize<WastelandMapState>(jsonA);
            Assert.NotNull(restoredState);
            var (restoredSystem, restoredMap, _) = CreateLive(restoredState);
            string jsonC = _json.Serialize(restoredMap.CaptureState());
            Assert.Equal(jsonA, jsonC);
        }

        // ── 7. Seeded fuzz sweep over fragment subsets ─────────────────

        [Fact]
        public void RegisteredFragments_SeededFuzzSweep_RoundTripStable_NoDoubleCount()
        {
            var (system, map, zones) = CreateLive();
            var allFragments = zones.SelectMany(z => z.Fragments).Select(f => f.fragment_id).ToList();
            var rng = new SeededRng(8512);

            for (int iteration = 0; iteration < 25; iteration++)
            {
                var fresh = CreateLive();
                var live = fresh.system;

                // Random subset for a random zone; include deliberate duplicates.
                var target = zones[rng.Next(0, zones.Count)];
                int take = rng.Next(1, target.Fragments.Count + 1);
                for (int i = 0; i < take; i++)
                    live.RegisterFragment(target.Fragments[i].fragment_id);
                for (int i = 0; i < take; i++)
                    Assert.False(live.RegisterFragment(target.Fragments[i].fragment_id)); // duplicate no-op

                int expectedCount = take;
                Assert.Equal(expectedCount, live.RegisteredCount(target.ZoneId));

                // Round-trip through the checksummed envelope.
                string envelopeJson = SaveEnvelopeHelper.CaptureEnvelope(fresh.map.CaptureState(), _json);
                var (success, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<WastelandMapState>(
                    envelopeJson, _json, legacyFallback: null, allowBareFallback: false);
                Assert.True(success, error);

                var (restoredSystem, _, _) = CreateLive(restored);
                Assert.Equal(expectedCount, restoredSystem.RegisteredCount(target.ZoneId));
                Assert.Equal(
                    live.IsZoneComplete(target.ZoneId),
                    restoredSystem.IsZoneComplete(target.ZoneId));

                // Invariant: registered fragments are always a subset of the catalog.
                foreach (var fragment in restored!.RegisteredMapFragments)
                    Assert.Contains(fragment, allFragments);
            }
        }

        // ── 8. Unknown fragment ids in old saves are inert ─────────────

        [Fact]
        public void RegisteredFragments_UnknownIdsInOldSave_AreInertAndRoundTrip()
        {
            // Simulate an old save whose catalog has since rolled back or
            // renamed a fragment: the persisted list references a fragment
            // the current catalog does not define.
            var oldState = new WastelandMapState();
            oldState.RegisteredMapFragments.Add("damaged_map_industrial_1");
            oldState.RegisteredMapFragments.Add("damaged_map_industrial_2");
            oldState.RegisteredMapFragments.Add("damaged_map_retired_zone_9"); // unknown to current catalog

            var (system, map, zones) = CreateLive(oldState);

            // Known progress survives; the unknown id does not corrupt zone
            // progress or completion (progress is catalog-derived).
            Assert.Equal(2, system.RegisteredCount("industrial_district"));
            Assert.False(system.IsZoneComplete("industrial_district"));

            // Registering the final original fragment still completes and reveals.
            Assert.True(system.RegisterFragment("damaged_map_industrial_3"));
            Assert.True(system.IsZoneComplete("industrial_district"));
            Assert.True(system.IsInstallationRevealed("industrial_district"));

            // The unknown id round-trips untouched (never silently rewritten —
            // save bytes remain a faithful record of what was persisted).
            string json = _json.Serialize(map.CaptureState());
            var restored = _json.Deserialize<WastelandMapState>(json);
            Assert.NotNull(restored);
            Assert.Contains("damaged_map_retired_zone_9", restored!.RegisteredMapFragments);
            Assert.Equal(4, restored.RegisteredMapFragments.Count);
        }
    }
}
