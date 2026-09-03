using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F1 — persistent encounter exhaustion. A depleting choice marks the
    /// whole encounter depleted; a non-depleting choice does not; depleted
    /// encounters are excluded from weighted selection; the set round-trips
    /// through save state and reconstructs from history on legacy restores.
    /// </summary>
    public class NarrativeEncounterDepletionTests
    {
        private const string TruckId = "micro_crashed_truck";
        private const string TruckSearch = "search_truck_cargo";
        private const string TruckIgnore = "ignore_truck";
        private const string MemorialId = "micro_roadside_memorial";
        private const string MemorialLeave = "leave_memorial";

        private static NarrativeEncounterSystem NewMicroSystem()
        {
            var sys = new NarrativeEncounterSystem();
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = TruckId,
                title = "Crashed Supply Truck",
                category = "Discovery",
                baseWeight = 1f,
                minDangerLevel = 0f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition
                    {
                        choiceId = TruckSearch, text = "Search", moraleDelta = 1, guiltDelta = 0,
                        grantItemId = "canned_food", grantItemQuantity = 2, depletesOnResolve = true
                    },
                    new EncounterChoiceDefinition { choiceId = TruckIgnore, text = "Move on", moraleDelta = 0, guiltDelta = 0 }
                }
            });
            sys.RegisterEncounter(new EncounterDefinition
            {
                id = MemorialId,
                title = "Roadside Memorial",
                category = "Discovery",
                baseWeight = 1f,
                minDangerLevel = 0f,
                choices = new List<EncounterChoiceDefinition>
                {
                    new EncounterChoiceDefinition { choiceId = MemorialLeave, text = "Leave it", moraleDelta = 1, guiltDelta = 0 }
                }
            });
            return sys;
        }

        [Fact]
        public void DepletingChoice_MarksEncounterDepleted()
        {
            var sys = NewMicroSystem();
            Assert.False(sys.IsDepleted(TruckId));

            var result = sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 3);

            Assert.NotNull(result);
            Assert.True(result!.DepletesEncounter);
            Assert.True(sys.IsDepleted(TruckId));
        }

        [Fact]
        public void NonDepletingChoice_DoesNotMarkEncounter()
        {
            var sys = NewMicroSystem();

            var result = sys.TryResolve(MemorialId, MemorialLeave, "rural_gas_station", 2);

            Assert.NotNull(result);
            Assert.False(result!.DepletesEncounter);
            Assert.False(sys.IsDepleted(MemorialId));
        }

        [Fact]
        public void IgnoreChoice_NonDepletingEncounterStaysEligible()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(TruckId, TruckIgnore, "rural_gas_station", 1);
            Assert.False(sys.IsDepleted(TruckId));
        }

        [Fact]
        public void DepletingEncounter_IsExcludedFromSelection()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 1);

            // Only the memorial remains eligible in this context.
            var picked = sys.SelectEncounter("Stealth", 0f, "anywhere", new SeededRng(1234));
            Assert.NotNull(picked);
            Assert.Equal(MemorialId, picked!.id);
        }

        [Fact]
        public void DepletedEncounter_NeverSelectedAcrossSeeds()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 1);

            // Weighted selection must exclude the depleted truck for any seed.
            for (int seed = 0; seed < 64; seed++)
            {
                var picked = sys.SelectEncounter("Stealth", 0f, "anywhere", new SeededRng(seed));
                if (picked != null) Assert.Equal(MemorialId, picked.id);
            }
        }

        [Fact]
        public void RepeatedResolve_SameEncounter_DoesNotGrowDepletedSet()
        {
            var sys = NewMicroSystem();
            // Direct re-resolution of the same encounter id (backlog/defense):
            // history grows, the depleted set stays unique-bounded.
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 1);
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 1);
            sys.TryResolve(TruckId, TruckIgnore, "rural_gas_station", 1);
            Assert.Equal(1, sys.DepletedCount);
            Assert.Equal(3, sys.TotalResolved);
        }

        [Fact]
        public void CaptureState_PersistsDepletion_OrdinalSorted()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(MemorialId, MemorialLeave, "rural_gas_station", 1);
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);

            var state = sys.CaptureState();

            Assert.NotNull(state.depletedEncounterIds);
            Assert.Equal(new[] { TruckId }, state.depletedEncounterIds);
        }

        [Fact]
        public void SaveLoad_RoundTripsDepletionAndBlocksReselection()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);

            var saved = sys.CaptureState();
            var restored = new NarrativeEncounterSystem();
            restored.RegisterEncounter(sys.Find(TruckId)!);
            restored.RegisterEncounter(sys.Find(MemorialId)!);
            restored.RestoreState(saved);

            Assert.True(restored.IsDepleted(TruckId));
            var picked = restored.SelectEncounter("Stealth", 0f, "anywhere", new SeededRng(7));
            Assert.NotNull(picked);
            Assert.Equal(MemorialId, picked!.id);
        }

        [Fact]
        public void Restore_NewFormatEmptyList_IsAuthoritative_NoReconstruction()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2);

            // Simulate a new-format save whose explicit depletion list is
            // empty (e.g. a future policy revision): it must win over history.
            var saved = sys.CaptureState();
            saved.depletedEncounterIds = new List<string>();

            var restored = new NarrativeEncounterSystem();
            restored.RegisterEncounter(sys.Find(TruckId)!);
            restored.RestoreState(saved);

            Assert.False(restored.IsDepleted(TruckId));
        }

        [Fact]
        public void Restore_LegacyStateWithoutDepletion_ReconstructsFromHistory()
        {
            var sys = NewMicroSystem();
            sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 2); // depleting
            sys.TryResolve(MemorialId, MemorialLeave, "rural_gas_station", 2); // non-depleting

            var legacy = sys.CaptureState();
            legacy.depletedEncounterIds = null; // pre-F1 save shape

            var restored = new NarrativeEncounterSystem();
            restored.RegisterEncounter(sys.Find(TruckId)!);
            restored.RegisterEncounter(sys.Find(MemorialId)!);
            restored.RestoreState(legacy);

            // The truck was searched before the feature shipped: it stays
            // exhausted. The memorial was only ever left alone: eligible.
            Assert.True(restored.IsDepleted(TruckId));
            Assert.False(restored.IsDepleted(MemorialId));
        }

        [Fact]
        public void Restore_LegacyState_UnknownHistoricalIdsSkippedNotGuessed()
        {
            var legacy = new NarrativeEncounterState
            {
                totalResolved = 1,
                depletedEncounterIds = null,
                history = new List<EncounterResolutionRecord>
                {
                    new EncounterResolutionRecord
                    { encounterId = "micro_removed_from_catalog", choiceId = "some_choice", day = 4 }
                }
            };

            var restored = new NarrativeEncounterSystem();
            restored.RestoreState(legacy);

            Assert.Equal(0, restored.DepletedCount);
        }

        [Fact]
        public void Resolve_MoraleAndGuiltUnchangedByRefactor()
        {
            // F2–F4 must not change existing morale/guilt behavior (plan §57).
            var sys = NewMicroSystem();

            var result = sys.TryResolve(TruckId, TruckSearch, "rural_gas_station", 3);

            Assert.NotNull(result);
            Assert.Equal(1, result!.MoraleDelta);
            Assert.Equal(0, result.GuiltDelta);
            Assert.Equal(1, sys.State.cumulativeMorale);
            Assert.Equal(0, sys.State.cumulativeGuilt);
        }

        [Fact]
        public void Resolve_UnknownEncounterOrChoice_NoStateMutation()
        {
            var sys = NewMicroSystem();

            Assert.Null(sys.TryResolve("micro_missing", TruckSearch, "x", 1));
            Assert.Null(sys.TryResolve(TruckId, "no_such_choice", "x", 1));

            Assert.Equal(0, sys.TotalResolved);
            Assert.Empty(sys.State.history);
            Assert.Equal(0, sys.DepletedCount);
        }
    }
}
