using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Workstream A: Deterministic Replay Verification for Collectible Effects.
    /// Proves seed equivalence, dispatch equivalence, save/restore continuity,
    /// final hash repeatability across runs, and non-perturbation of RNG.
    /// </summary>
    public class CollectibleReplayTests
    {
        // Stable sequence of 20 locations and their corresponding loot tables (Section 5)
        private static readonly (string LocationId, string TableId)[] ScavengeLocations20 = new[]
        {
            ("loc_apt_01", "table_loot_apartment_block"),
            ("loc_fire_01", "table_loot_fire_station"),
            ("loc_mil_01", "table_loot_military_depot"),
            ("loc_police_01", "table_loot_police_station"),
            ("loc_school_01", "table_loot_school"),
            ("loc_clinic_01", "table_loot_clinic"),
            ("loc_ind_01", "table_loot_industrial_district"),
            ("loc_metro_01", "table_loot_metro_station"),
            ("loc_apt_02", "table_loot_apartment_block"),
            ("loc_fire_02", "table_loot_fire_station"),
            // -- Save checkpoint at location index 10 --
            ("loc_mil_02", "table_loot_military_depot"),
            ("loc_police_02", "table_loot_police_station"),
            ("loc_school_02", "table_loot_school"),
            ("loc_clinic_02", "table_loot_clinic"),
            ("loc_ind_02", "table_loot_industrial_district"),
            ("loc_metro_02", "table_loot_metro_station"),
            ("loc_apt_03", "table_loot_apartment_block"),
            ("loc_fire_03", "table_loot_fire_station"),
            ("loc_mil_03", "table_loot_military_depot"),
            ("loc_police_03", "table_loot_police_station")
        };

        private static (CollectibleReplaySnapshot continuous, CollectibleReplaySnapshot restored) RunCanonicalReplay(int seed = 42)
        {
            // Fixture A: Continuous run of 20 locations without saving
            var fixtureA = new CollectibleReplayFixture(seed);
            for (int i = 0; i < 20; i++)
            {
                fixtureA.ScavengeLocation(ScavengeLocations20[i].LocationId, ScavengeLocations20[i].TableId);
            }
            var snapA = fixtureA.ExtractSnapshot();

            // Fixture B: Pre-save run of first 10 locations
            var fixtureB = new CollectibleReplayFixture(seed);
            for (int i = 0; i < 10; i++)
            {
                fixtureB.ScavengeLocation(ScavengeLocations20[i].LocationId, ScavengeLocations20[i].TableId);
            }

            // Capture save state from B
            var discoverySave = fixtureB.Discovery.CaptureState();
            var uniqueSave = fixtureB.Claims.CaptureState();
            var survivor = fixtureB.Needs.Registered.First();
            var survivorState = new SurvivorNeedsState
            {
                Id = survivor.Id,
                Morale = survivor.Morale,
                Health = survivor.Health,
                Hunger = survivor.Hunger,
                Thirst = survivor.Thirst
            };
            var researchState = fixtureB.Research.CaptureState();
            var journalSave = fixtureB.Journal.CaptureState();
            var mapState = fixtureB.Map.CaptureState();
            ulong rngState = fixtureB.Rng.State;
            var traceCopy = new List<CollectibleReplayTraceEntry>(fixtureB.Trace);

            // Fixture C: Restored run into fresh instance, continuing remaining 10 locations
            var fixtureC = new CollectibleReplayFixture(
                seed, rngState, discoverySave, uniqueSave, survivorState, researchState, journalSave, mapState, traceCopy);

            for (int i = 10; i < 20; i++)
            {
                fixtureC.ScavengeLocation(ScavengeLocations20[i].LocationId, ScavengeLocations20[i].TableId);
            }
            var snapC = fixtureC.ExtractSnapshot();

            return (snapA, snapC);
        }

        [Fact]
        public void CollectibleReplay_ContinuousVsSaveRestore_ProducesIdenticalFinds()
        {
            var (snapA, snapC) = RunCanonicalReplay(42);

            Assert.True(snapA.Trace.Count > 0, "Canonical replay must encounter collectibles");
            Assert.Equal(snapA.Trace.Count, snapC.Trace.Count);

            for (int i = 0; i < snapA.Trace.Count; i++)
            {
                Assert.Equal(snapA.Trace[i].CollectibleItemId, snapC.Trace[i].CollectibleItemId);
                Assert.Equal(snapA.Trace[i].LocationId, snapC.Trace[i].LocationId);
                Assert.Equal(snapA.Trace[i].WasFirstDiscovery, snapC.Trace[i].WasFirstDiscovery);
            }
        }

        [Fact]
        public void CollectibleReplay_ContinuousVsSaveRestore_ProducesIdenticalEffectOrder()
        {
            var (snapA, snapC) = RunCanonicalReplay(42);

            Assert.Equal(snapA.Trace.Count, snapC.Trace.Count);
            for (int i = 0; i < snapA.Trace.Count; i++)
            {
                Assert.Equal(snapA.Trace[i].EffectType, snapC.Trace[i].EffectType);
                Assert.Equal(snapA.Trace[i].EffectTargetOrKey, snapC.Trace[i].EffectTargetOrKey);
                Assert.Equal(snapA.Trace[i].EffectPayloadNormalized, snapC.Trace[i].EffectPayloadNormalized);
            }
        }

        [Fact]
        public void CollectibleReplay_SaveRestore_PreservesMoraleRewards()
        {
            var (snapA, snapC) = RunCanonicalReplay(42);

            Assert.Equal(snapA.SurvivorMorale, snapC.SurvivorMorale, precision: 3);
        }

        [Fact]
        public void CollectibleReplay_SaveRestore_PreservesKnowledgeJournalIntelAndReveals()
        {
            var (snapA, snapC) = RunCanonicalReplay(42);

            Assert.Equal(snapA.UnlockedKnowledge, snapC.UnlockedKnowledge);
            Assert.Equal(snapA.JournalEntryCount, snapC.JournalEntryCount);
            Assert.Equal(snapA.CodexUnlockCount, snapC.CodexUnlockCount);
            Assert.Equal(snapA.RevealedMapNodes, snapC.RevealedMapNodes);
            Assert.Equal(snapA.DiscoveredIds, snapC.DiscoveredIds);
        }

        [Fact]
        public void CollectibleReplay_FinalDiscoveryHash_MatchesAcrossSaveBoundary()
        {
            var (snapA, snapC) = RunCanonicalReplay(42);

            string hashA = CollectibleStateHasher.ComputeHash(snapA);
            string hashC = CollectibleStateHasher.ComputeHash(snapC);

            Assert.False(string.IsNullOrEmpty(hashA));
            Assert.Equal(hashA, hashC);
        }

        [Fact]
        public void CollectibleReplay_Seed42_ThreeFreshRunsProduceSameHash()
        {
            // Run 3 independent fresh executions of the continuous replay with seed 42
            var hashes = new List<string>();
            for (int run = 0; run < 3; run++)
            {
                var fixture = new CollectibleReplayFixture(42);
                for (int i = 0; i < 20; i++)
                {
                    fixture.ScavengeLocation(ScavengeLocations20[i].LocationId, ScavengeLocations20[i].TableId);
                }
                var snap = fixture.ExtractSnapshot();
                hashes.Add(CollectibleStateHasher.ComputeHash(snap));
            }

            Assert.Equal(hashes[0], hashes[1]);
            Assert.Equal(hashes[1], hashes[2]);
        }

        [Fact]
        public void CollectibleReplay_MismatchDiagnostic_ReportsFirstDivergence()
        {
            var traceExpected = new List<CollectibleReplayTraceEntry>
            {
                new CollectibleReplayTraceEntry
                {
                    SequenceIndex = 0,
                    LocationId = "loc_01",
                    CollectibleItemId = "item_collectible_family_portrait",
                    EffectType = "morale",
                    EffectTargetOrKey = "",
                    EffectPayloadNormalized = "2.0",
                    WasFirstDiscovery = true
                },
                new CollectibleReplayTraceEntry
                {
                    SequenceIndex = 1,
                    LocationId = "loc_02",
                    CollectibleItemId = "item_collectible_unit_photograph",
                    EffectType = "faction_info",
                    EffectTargetOrKey = "faction_military_history",
                    EffectPayloadNormalized = "0.0",
                    WasFirstDiscovery = true
                }
            };

            var traceActual = new List<CollectibleReplayTraceEntry>
            {
                new CollectibleReplayTraceEntry
                {
                    SequenceIndex = 0,
                    LocationId = "loc_01",
                    CollectibleItemId = "item_collectible_family_portrait",
                    EffectType = "morale",
                    EffectTargetOrKey = "",
                    EffectPayloadNormalized = "2.0",
                    WasFirstDiscovery = true
                },
                new CollectibleReplayTraceEntry
                {
                    SequenceIndex = 1,
                    LocationId = "loc_02",
                    CollectibleItemId = "item_collectible_road_map", // Deliberate mismatch
                    EffectType = "location_clue",
                    EffectTargetOrKey = "loc_road_junction_cache",
                    EffectPayloadNormalized = "0.0",
                    WasFirstDiscovery = true
                }
            };

            string? divergence = CollectibleReplayDiagnostic.FindFirstDivergence(traceExpected, traceActual, restoreIndex: 1);
            Assert.NotNull(divergence);
            Assert.Contains("Divergence at index 1", divergence);
            Assert.Contains("Expected item: 'item_collectible_unit_photograph'", divergence);
            Assert.Contains("Actual item:   'item_collectible_road_map'", divergence);
            Assert.Contains("AFTER restore", divergence);
        }

        [Fact]
        public void CollectibleReplay_TraceObserver_DoesNotPerturbScavengingRng()
        {
            // Run 1: with RecordTrace enabled
            var fixtureWithTrace = new CollectibleReplayFixture(100);
            fixtureWithTrace.RecordTrace = true;
            var rollsWithTrace = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                var r = fixtureWithTrace.ScavengeLocation(ScavengeLocations20[i].LocationId, ScavengeLocations20[i].TableId);
                rollsWithTrace.Add(r?.ItemId ?? "none");
            }

            // Run 2: with RecordTrace disabled (no-op observer)
            var fixtureNoTrace = new CollectibleReplayFixture(100);
            fixtureNoTrace.RecordTrace = false;
            var rollsNoTrace = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                var r = fixtureNoTrace.ScavengeLocation(ScavengeLocations20[i].LocationId, ScavengeLocations20[i].TableId);
                rollsNoTrace.Add(r?.ItemId ?? "none");
            }

            // Invariant 3: RNG consumption and resulting loot sequence must be strictly identical
            Assert.Equal(rollsWithTrace, rollsNoTrace);
            Assert.Equal(fixtureWithTrace.Rng.State, fixtureNoTrace.Rng.State);
        }
    }
}
