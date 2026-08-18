using System;
using System.Collections.Generic;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for <see cref="StandingRecordEngine"/> and the unified
    /// StandingRecordState envelope. Mirrors the Phase-18
    /// SkillProgressionSystem tests (12) with 8 cases.
    /// </summary>
    public class StandingRecordEngineTests
    {
        // -- Test fixture helpers ------------------------------------------

        private static (IFileIO files, IJsonSerializer json, ISeededRng rng, NullLog log)
            MakeWiring()
        {
            return (
                files: new FileSystemIO(),
                json: new SystemTextJsonSerializer(),
                rng: new SeededRng(1401),
                log: new NullLog()
            );
        }

        private static StandingRecordEngine BuildEngine(StandingRecordState state = null)
        {
            var (files, json, rng, log) = MakeWiring();
            return new StandingRecordEngine(files, json, rng, log, state);
        }

        // -- Tests ---------------------------------------------------------

        [Fact]
        public void Ctor_StartsLocked_WithOverlayAccess()
        {
            var engine = BuildEngine();
            Assert.False(engine.IsUnlocked);
            Assert.True(engine.HasOverlayAccess);
            Assert.Equal(0, engine.CurrentDay);
            Assert.NotNull(engine.Layouts);
            Assert.NotNull(engine.Memory);
            Assert.NotNull(engine.Encounters);
        }

        [Fact]
        public void UnlockExpansion_SetsFlagAndDay()
        {
            var engine = BuildEngine();
            engine.UnlockExpansion(currentDay: 12);

            Assert.True(engine.IsUnlocked);
            Assert.Equal(12, engine.CurrentDay);
            Assert.True(engine.Layouts.IsUnlocked);
            Assert.True(engine.Memory.IsUnlocked);
            Assert.True(engine.Encounters.IsUnlocked);
            Assert.True(engine.Memory.HasMutation(StandingRecordEngine.FlagExpUnlocked));
        }

        [Fact]
        public void UnlockExpansion_IsIdempotent()
        {
            var engine = BuildEngine();
            engine.UnlockExpansion(currentDay: 5);
            engine.UnlockExpansion(currentDay: 99);
            // Second call must NOT advance the day.
            Assert.Equal(5, engine.CurrentDay);
        }

        [Fact]
        public void Tick_UpdatesDayAndOverlayAccess()
        {
            var engine = BuildEngine();
            engine.UnlockExpansion(currentDay: 1);

            engine.Tick(newDay: 14);
            Assert.Equal(14, engine.CurrentDay);

            // Tick before unlock is a no-op.
            var engine2 = BuildEngine();
            engine2.Tick(newDay: 14);
            Assert.Equal(0, engine2.CurrentDay);
            Assert.False(engine2.IsUnlocked);
        }

        [Fact]
        public void ApplySiteMutation_SetsMemoryFlag_AndLayoutFlag()
        {
            var engine = BuildEngine();
            engine.UnlockExpansion(currentDay: 5);

            Assert.True(engine.ApplySiteMutation(
                "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
            Assert.True(engine.Memory.HasMutation(LocationMemorySystem.MutationKm19Plated));
            Assert.True(engine.Layouts.HasFlag(
                "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
        }

        [Fact]
        public void ApplySiteMutation_GatedByUnlock()
        {
            var engine = BuildEngine();
            // Locked engine — mutation rejected.
            Assert.False(engine.ApplySiteMutation(
                "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
            Assert.False(engine.Memory.HasMutation(LocationMemorySystem.MutationKm19Plated));
        }

        [Fact]
        public void ApplySiteMutation_EmptyMutation_Rejected()
        {
            var engine = BuildEngine();
            engine.UnlockExpansion(currentDay: 5);
            Assert.False(engine.ApplySiteMutation("loc_cut_kilometre_19", ""));
        }

        [Fact]
        public void CaptureState_RoundTrip_PreservesState()
        {
            var engine = BuildEngine();
            engine.UnlockExpansion(currentDay: 7);
            engine.ApplySiteMutation(
                "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated);
            engine.Tick(newDay: 18);

            var saved = engine.CaptureState();
            Assert.True(saved.expansionUnlocked);
            Assert.Equal(18, saved.currentDay);
            Assert.Contains(
                LocationMemorySystem.MutationKm19Plated, saved.memory.activeFlags);

            // Round-trip into a fresh engine.
            var (files, json, rng, log) = MakeWiring();
            var engine2 = new StandingRecordEngine(files, json, rng, log, saved);
            Assert.True(engine2.IsUnlocked);
            Assert.Equal(18, engine2.CurrentDay);
            Assert.True(engine2.Memory.HasMutation(LocationMemorySystem.MutationKm19Plated));
            Assert.True(engine2.Layouts.HasFlag(
                "loc_cut_kilometre_19", LocationMemorySystem.MutationKm19Plated));
        }

        [Fact]
        public void Tick_IsDeterministicUnderSeedControl()
        {
            var engineA = BuildEngine();
            engineA.UnlockExpansion(currentDay: 1);
            engineA.Tick(newDay: 42);

            var engineB = BuildEngine();
            engineB.UnlockExpansion(currentDay: 1);
            engineB.Tick(newDay: 42);

            // Two engines with the same starting state and same day-step
            // tick should produce identical Day and overlayAccess state.
            Assert.Equal(engineA.CurrentDay, engineB.CurrentDay);
            Assert.Equal(engineA.HasOverlayAccess, engineB.HasOverlayAccess);
            Assert.Equal(engineA.State.platesScrapedForExpeditionStep(
                engineA.State.currentDay), engineB.State.platesScrapedForExpeditionStep(
                engineB.State.currentDay));
        }
    }

    /// <summary>
    /// Internal state-derived helper for the deterministic test surface;
    /// kept in the tests namespace so it does not enlarge the engine
    /// surface. The expected contract is: same seed + same tick input
    /// ⇒ same overlay-access page.
    /// </summary>
    internal static class StandingRecordStateTestExtensions
    {
        public static int platesScrapedForExpeditionStep(this StandingRecordState state, int day)
        {
            // Read-only projection used by the deterministic-tick test
            // to compare fixed-resolution state across engines.
            return state.encounters != null ? state.encounters.platesScraped : 0;
        }
    }
}
