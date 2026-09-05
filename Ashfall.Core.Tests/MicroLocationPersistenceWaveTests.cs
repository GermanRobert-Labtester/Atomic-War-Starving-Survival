using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F9 flagship wave — persistence evidence beyond NarrativeEncounterDepletionTests.
    /// Exercises the production wire path (SystemTextJsonSerializer, the same payload
    /// the checksummed NarrativeSaveStore envelope carries) for depletion and pending
    /// surfacing, plus: insertion-order-independent serialization, malformed-input
    /// collapse, capture immutability, INV-03 (depletion is independent of reward
    /// effects), and legacy wire saves that predate the depletion field.
    /// </summary>
    public class MicroLocationPersistenceWaveTests
    {
        private const string TruckId = "micro_crashed_truck";
        private const string TruckSearch = "search_truck_cargo";
        private const string MemorialId = "micro_roadside_memorial";
        private const string MemorialLeave = "leave_memorial";
        private const string MemorialTake = "take_offering";

        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static NarrativeEncounterSystem CreateProductionSystem()
        {
            var sys = new NarrativeEncounterSystem();
            string dataDir = DataDir();
            var defs = NarrativeEncounterCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            sys.RegisterRange(defs);
            return sys;
        }

        private static NarrativeEncounterSystem CreateTruckMemorialSystem()
        {
            var sys = new NarrativeEncounterSystem();
            var prod = CreateProductionSystem();
            sys.RegisterEncounter(prod.Find(TruckId)!);
            sys.RegisterEncounter(prod.Find(MemorialId)!);
            return sys;
        }

        private static string SerializeState(NarrativeEncounterState state)
            => new SystemTextJsonSerializer().Serialize(state);

        private static NarrativeEncounterState? DeserializeState(string json)
            => new SystemTextJsonSerializer().Deserialize<NarrativeEncounterState>(json);

        // ── F9.5/F9.14 — wire round-trip + deterministic serialization ──

        [Fact]
        public void ResolveDepletingMicroLocation_WireRoundTrip_RemainsDepleted()
        {
            var sys = CreateTruckMemorialSystem();
            var res = sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 3);
            Assert.NotNull(res);
            Assert.True(res!.DepletesEncounter);

            var restored = CreateTruckMemorialSystem();
            restored.RestoreState(DeserializeState(SerializeState(sys.CaptureState()))!);

            Assert.True(restored.IsDepleted(TruckId));
            Assert.False(restored.IsDepleted(MemorialId));
        }

        [Fact]
        public void CaptureState_RepeatedCaptures_InsertionOrderIndependent_Ordinal()
        {
            var sys = CreateTruckMemorialSystem();
            // Deplete in non-ordinal order.
            sys.TryResolve(MemorialId, MemorialTake, "high_scarp", 1);
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);

            var first = sys.CaptureState();
            var second = sys.CaptureState();

            Assert.NotNull(first.depletedEncounterIds);
            Assert.Equal(first.depletedEncounterIds, second.depletedEncounterIds);
            var ids = new List<string>(first.depletedEncounterIds!);
            ids.Sort(string.CompareOrdinal);
            Assert.Equal(ids, first.depletedEncounterIds);
        }

        [Fact]
        public void CaptureState_PreviouslyCapturedDto_UnaffectedByLaterRuntimeMutation()
        {
            var sys = CreateTruckMemorialSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);
            var captured = sys.CaptureState();

            // More depletion after the capture must not leak into the old DTO.
            sys.TryResolve(MemorialId, MemorialTake, "high_scarp", 3);

            Assert.Single(captured.depletedEncounterIds!);
            Assert.Equal(TruckId, captured.depletedEncounterIds![0]);
        }

        // ── F9.4 rule 5 — malformed input collapses, never throws ──

        [Fact]
        public void RestoreState_DuplicateDepletionIds_CollapsesSafely()
        {
            var sys = CreateTruckMemorialSystem();
            var saved = new NarrativeEncounterState
            {
                depletedEncounterIds = new List<string> { TruckId, TruckId, TruckId }
            };

            var restored = CreateTruckMemorialSystem();
            restored.RestoreState(saved);

            Assert.Equal(1, restored.DepletedCount);
            Assert.True(restored.IsDepleted(TruckId));
        }

        // ── F9.8 — pending surfacing survives the wire exactly ──

        [Fact]
        public void PendingMicroLocation_WireRoundTrip_RestoresExactly_ResolvesOnceAfterReload()
        {
            var sys = CreateProductionSystem();
            sys.EnqueuePending(TruckId, "rural_gas_station", 4, 9);
            Assert.Single(sys.State.pending);

            var restored = CreateProductionSystem();
            int resolvedEvents = 0;
            restored.OnEncounterResolved += _ => resolvedEvents++;
            restored.RestoreState(DeserializeState(SerializeState(sys.CaptureState()))!);

            // Pending identity survives field-for-field.
            var pending = Assert.Single(restored.State.pending);
            Assert.Equal(TruckId, pending.encounterId);
            Assert.Equal("rural_gas_station", pending.locationId);
            Assert.Equal(4, pending.legIndex);
            Assert.Equal(9, pending.day);

            // No premature depletion, no reward committed, and reloading must
            // not have triggered a duplicate selection roll or resolution.
            Assert.False(restored.IsDepleted(TruckId));
            Assert.Empty(restored.State.history);
            Assert.Equal(0, restored.TotalResolved);
            Assert.Equal(0, resolvedEvents);

            // Resolving once after reload commits exactly one record/effect.
            var res = restored.TryResolve(TruckId, TruckSearch, "rural_gas_station", 9);
            Assert.NotNull(res);
            Assert.Single(restored.State.history);
            Assert.Equal(1, restored.TotalResolved);
            Assert.Equal(1, resolvedEvents);
            restored.ClearPending(TruckId);
            Assert.Empty(restored.State.pending);
        }

        // ── INV-03 — depletion never depends on reward state ──

        [Fact]
        public void GrantedItemRemovedAfterResolve_SaveReload_EncounterStaysDepleted()
        {
            var sys = CreateTruckMemorialSystem();
            var res = sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);
            Assert.Equal("canned_food", res!.GrantItemId);

            // The player drops/eats the granted cans: reward state is gone.
            // (Core depletion never reads rewards; this pins that restore does
            // not reconstruct — or clear — depletion from item effects.)

            var restored = CreateTruckMemorialSystem();
            restored.RestoreState(DeserializeState(SerializeState(sys.CaptureState()))!);

            Assert.True(restored.IsDepleted(TruckId));
            // And the production selector can never surface it again.
            for (int seed = 0; seed < 32; seed++)
            {
                var picked = restored.SelectEncounter("Normal", 3f, "rural_gas_station", new SeededRng(seed));
                Assert.NotEqual(TruckId, picked?.id);
            }
        }

        // ── F9.13 (adapted per log D2) — legacy wire save without the field ──

        [Fact]
        public void LegacyWireSave_WithoutDepletionField_RestoresFromHistory_NoRefill()
        {
            // A pre-F1 campaign: resolution history exists, depletion list does
            // not. The shipped contract (see §48 migration) reconstructs from
            // history so already-searched sites stay exhausted — never refill.
            var sys = CreateTruckMemorialSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);
            sys.TryResolve(MemorialId, MemorialLeave, "rural_gas_station", 2);

            var legacy = sys.CaptureState();
            legacy.depletedEncounterIds = null;
            string wire = SerializeState(legacy);
            // The serializer carries the field as explicit null; deserializing
            // yields a null list — the exact legacy condition (missing or null).
            Assert.Contains("\"depletedEncounterIds\":null", wire);

            var restored = CreateTruckMemorialSystem();
            restored.RestoreState(DeserializeState(wire)!);

            Assert.True(restored.IsDepleted(TruckId));   // resolved depleting stays exhausted
            Assert.False(restored.IsDepleted(MemorialId)); // leave-only never depletes
        }

        // ── F9.12 — world-flag authority is idempotent across reload ──

        [Fact]
        public void WorldFlag_SetSaveReload_ReappliedSet_DoesNotDuplicate()
        {
            var flags = new CampaignConsequenceLedger();
            flags.Set("flag_test_micro_pact", NarrativeEncounterSystem.SystemId, "micro_x:choice:3:loc", 3);

            var restored = new CampaignConsequenceLedger();
            restored.RestoreState(flags.CaptureState());

            Assert.True(restored.IsSet("flag_test_micro_pact"));

            // The host guards with IsSet before Set; prove the reapplication
            // path cannot fork state (two ledgers re-converge identically).
            var reapplied = new CampaignConsequenceLedger();
            reapplied.RestoreState(flags.CaptureState());
            if (!reapplied.IsSet("flag_test_micro_pact"))
                reapplied.Set("flag_test_micro_pact", NarrativeEncounterSystem.SystemId, "micro_x:choice:3:loc", 3);

            string a = new SystemTextJsonSerializer().Serialize(restored.CaptureState());
            string b = new SystemTextJsonSerializer().Serialize(reapplied.CaptureState());
            Assert.Equal(a, b);
        }
    }
}
