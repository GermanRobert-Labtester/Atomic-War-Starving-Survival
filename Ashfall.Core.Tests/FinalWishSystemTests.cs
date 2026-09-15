using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class FinalWishSystemTests
    {
        private static FinalWishSystem CreateSystem(int? seed = 42)
        {
            var sys = new FinalWishSystem();
            sys.Rng = seed.HasValue ? new SeededRng(seed.Value) : null;
            return sys;
        }

        [Fact]
        public void DeclareTerminalPrognosis_ActivatesWish()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.True(sys.HasTerminalPrognosis("sv_1"));
            Assert.True(sys.HasActiveWish("sv_1"));
            Assert.True(sys.GetDaysRemaining("sv_1") > 0f);
        }

        [Fact]
        public void DeclareTerminalPrognosis_RejectsDeadSurvivor()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: false);
            Assert.False(sys.HasTerminalPrognosis("sv_1"));
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void DeclareTerminalPrognosis_RejectsDuplicate()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            float firstDays = sys.GetDaysRemaining("sv_1");
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Equal(firstDays, sys.GetDaysRemaining("sv_1"));
        }

        [Fact]
        public void DeclareTerminalPrognosis_FiresEvent()
        {
            var sys = CreateSystem();
            string firedId = null;
            string firedWish = null;
            float firedDays = 0f;
            sys.OnTerminalPrognosisDeclared += (id, wish, days) =>
            {
                firedId = id;
                firedWish = wish;
                firedDays = days;
            };
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Equal("sv_1", firedId);
            Assert.NotEmpty(firedWish);
            Assert.True(firedDays > 0f);
        }

        [Fact]
        public void AdvanceWishStep_CompletesDeliverLetter_InTwoSteps()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            // Default wish is deliver_letter (2 steps)
            Assert.False(sys.AdvanceWishStep("sv_1", "step_1"));
            Assert.True(sys.AdvanceWishStep("sv_1", "step_2"));
            Assert.True(sys.HasCompletedWish("sv_1"));
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void AdvanceWishStep_BuildMemorial_RequiresThreeSteps()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "soldier_archetype", isAlive: true);
            // soldier → build_memorial (3 steps)
            Assert.False(sys.AdvanceWishStep("sv_1", "step_1"));
            Assert.False(sys.AdvanceWishStep("sv_1", "step_2"));
            Assert.True(sys.AdvanceWishStep("sv_1", "step_3"));
            Assert.True(sys.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void AdvanceWishStep_SeeTheSky_CompletesInOneStep()
        {
            var sys = CreateSystem();
            sys.RegisterWish("skywatcher", FinalWishSystem.WishSeeTheSky);
            sys.DeclareTerminalPrognosis("sv_1", "skywatcher", isAlive: true);
            Assert.True(sys.AdvanceWishStep("sv_1", "step_1"));
            Assert.True(sys.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void CompleteWish_AppliesMoraleBuff_AndFiresEvent()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            string completedId = null;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.OnFinalWishCompleted += (id) => completedId = id;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.AdvanceWishStep("sv_1", "step_2");
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, appliedBuff);
            Assert.Equal("sv_1", completedId);
        }

        [Fact]
        public void OnPrognosisExpired_AppliesPenalty_AndFiresEvent()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            string failedId = null;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.OnFinalWishFailed += (id) => failedId = id;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.OnPrognosisExpired("sv_1");
            Assert.Equal(FinalWishSystem.WishFailedMoralePenalty, appliedBuff);
            Assert.Equal("sv_1", failedId);
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void OnPrognosisExpired_DoesNothingIfWishAlreadyCompleted()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.AdvanceWishStep("sv_1", "step_2");
            sys.OnPrognosisExpired("sv_1");
            // Should not apply penalty since wish was completed
            Assert.Equal(FinalWishSystem.WishCompletedMoraleBuff, appliedBuff);
        }

        [Fact]
        public void Tick_DecrementsDaysRemaining()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            float initialDays = sys.GetDaysRemaining("sv_1");
            sys.Tick("sv_1", 24f, isAlive: true); // 1 day
            Assert.Equal(initialDays - 1f, sys.GetDaysRemaining("sv_1"), 4);
        }

        [Fact]
        public void Tick_ExpiresPrognosis_WhenDaysReachZero()
        {
            var sys = CreateSystem();
            float appliedBuff = 0f;
            sys.ApplyPermanentShelterMoraleBuff = (buff) => appliedBuff = buff;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            // Tick enough to expire (max 7 days)
            sys.Tick("sv_1", 24f * 10f, isAlive: true);
            Assert.Equal(FinalWishSystem.WishFailedMoralePenalty, appliedBuff);
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void Tick_DoesNothingForDeadSurvivor()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            float initialDays = sys.GetDaysRemaining("sv_1");
            sys.Tick("sv_1", 24f, isAlive: false);
            Assert.Equal(initialDays, sys.GetDaysRemaining("sv_1"));
        }

        [Fact]
        public void RegisterWish_OverridesArchetypeMapping()
        {
            var sys = CreateSystem();
            sys.RegisterWish("custom_arch", FinalWishSystem.WishRetrieveHeirloom);
            sys.DeclareTerminalPrognosis("sv_1", "custom_arch", isAlive: true);
            Assert.Equal(FinalWishSystem.WishRetrieveHeirloom, sys.GetWishType("sv_1"));
        }

        [Fact]
        public void ArchetypePrefix_Surgeon_MapsToTeachLesson()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "field_surgeon", isAlive: true);
            Assert.Equal(FinalWishSystem.WishTeachLesson, sys.GetWishType("sv_1"));
        }

        [Fact]
        public void ArchetypePrefix_Parent_MapsToReconcile()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "single_parent", isAlive: true);
            Assert.Equal(FinalWishSystem.WishReconcile, sys.GetWishType("sv_1"));
        }

        [Fact]
        public void SaveLoad_RoundTrips()
        {
            var sys = CreateSystem();
            sys.RegisterWish("custom_arch", FinalWishSystem.WishBuildMemorial);
            sys.DeclareTerminalPrognosis("sv_1", "custom_arch", isAlive: true);
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.DeclareTerminalPrognosis("sv_2", "unknown_archetype", isAlive: true);

            var saved = sys.CaptureState();

            var sys2 = CreateSystem();
            sys2.RestoreState(saved);

            Assert.True(sys2.HasActiveWish("sv_1"));
            Assert.Equal(1, sys2.GetStepsCompleted("sv_1"));
            Assert.Equal(FinalWishSystem.WishBuildMemorial, sys2.GetWishType("sv_1"));
            Assert.True(sys2.HasActiveWish("sv_2"));
            Assert.Equal(FinalWishSystem.WishDeliverLetter, sys2.GetWishType("sv_2"));
        }

        [Fact]
        public void SaveLoad_DeepCopy_NoSharedReferences()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            var saved = sys.CaptureState();

            // Mutate original
            sys.AdvanceWishStep("sv_1", "step_1");
            sys.AdvanceWishStep("sv_1", "step_2");

            // Restore into new system — should have original state, not mutated
            var sys2 = CreateSystem();
            sys2.RestoreState(saved);
            Assert.Equal(0, sys2.GetStepsCompleted("sv_1"));
            Assert.True(sys2.HasActiveWish("sv_1"));
            Assert.False(sys2.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void RestoreState_Null_ClearsAll()
        {
            var sys = CreateSystem();
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            sys.RestoreState(null);
            Assert.False(sys.HasTerminalPrognosis("sv_1"));
            Assert.False(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void AdvanceWishStep_RejectsUnknownSurvivor()
        {
            var sys = CreateSystem();
            Assert.False(sys.AdvanceWishStep("nonexistent", "step_1"));
        }

        [Fact]
        public void OnStateChanged_FiresOnDeclare()
        {
            var sys = CreateSystem();
            int fireCount = 0;
            sys.OnStateChanged += () => fireCount++;
            sys.DeclareTerminalPrognosis("sv_1", "unknown_archetype", isAlive: true);
            Assert.True(fireCount > 0);
        }

        // ── Catalog pool model ──────────────────────────────────────────

        /// <summary>A minimal in-memory catalog for deterministic system tests.</summary>
        private sealed class TestCatalog : IFinalWishCatalog
        {
            private readonly Dictionary<string, List<string>> _pools = new(StringComparer.Ordinal);
            private readonly Dictionary<string, FinalWishEntry> _entries = new(StringComparer.Ordinal);
            public void Add(string archetype, FinalWishEntry entry)
            {
                _entries[entry.id] = entry;
                if (!_pools.TryGetValue(archetype, out var pool)) { pool = new List<string>(); _pools[archetype] = pool; }
                pool.Add(entry.id);
            }
            public IReadOnlyList<string> GetWishIdsForArchetype(string archetypeId) =>
                _pools.TryGetValue(archetypeId, out var p) ? p : System.Array.Empty<string>();
            public FinalWishEntry? GetEntry(string wishId) =>
                _entries.TryGetValue(wishId, out var e) ? e : null;
        }

        private static TestCatalog TwoStepPool(string archetype, string wishIdA, string wishIdB)
        {
            var cat = new TestCatalog();
            cat.Add(archetype, Entry(wishIdA, archetype, 2));
            cat.Add(archetype, Entry(wishIdB, archetype, 2));
            return cat;
        }

        private static FinalWishEntry Entry(string id, string archetype, int stepCount)
        {
            var e = new FinalWishEntry { id = id, archetype_id = archetype, wish_type = FinalWishSystem.WishDeliverLetter };
            for (int i = 0; i < stepCount; i++) e.steps.Add(new FinalWishStep { step_id = $"{id}_s{i}" });
            return e;
        }

        [Fact]
        public void Catalog_PoolSelection_IsDeterministicForSameSeed()
        {
            var cat = TwoStepPool("medic_archetype", "wish_a", "wish_b");
            var sys1 = CreateSystem(seed: 7); sys1.Catalog = cat;
            var sys2 = CreateSystem(seed: 7); sys2.Catalog = cat;
            sys1.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            sys2.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Equal(sys1.GetWishId("sv_1"), sys2.GetWishId("sv_1"));
            Assert.NotEmpty(sys1.GetWishId("sv_1"));
        }

        [Fact]
        public void Catalog_DifferentSeeds_CanSelectDifferentWishes()
        {
            var cat = TwoStepPool("medic_archetype", "wish_a", "wish_b");
            var picks = new System.Collections.Generic.HashSet<string>();
            for (int seed = 0; seed < 40; seed++)
            {
                var sys = CreateSystem(seed); sys.Catalog = cat;
                sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
                picks.Add(sys.GetWishId("sv_1"));
            }
            // With 2 pool entries over 40 seeds, we expect both to appear eventually.
            Assert.True(picks.Count >= 2, $"expected both pool entries drawn, got {picks.Count}");
        }

        [Fact]
        public void Catalog_NullCatalog_LeavesWishIdEmpty()
        {
            var sys = CreateSystem();
            // No Catalog assigned — must behave exactly as before (no wishId).
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Empty(sys.GetWishId("sv_1"));
            Assert.True(sys.HasActiveWish("sv_1"));
        }

        [Fact]
        public void Catalog_EmptyPool_LeavesWishIdEmpty()
        {
            var sys = CreateSystem(); sys.Catalog = new TestCatalog(); // no pools
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.Empty(sys.GetWishId("sv_1"));
        }

        [Fact]
        public void Catalog_StepCountHonorsEntry()
        {
            var cat = new TestCatalog();
            cat.Add("medic_archetype", Entry("wish_three", "medic_archetype", 3));
            var sys = CreateSystem(); sys.Catalog = cat;
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            Assert.False(sys.AdvanceWishStep("sv_1", "s1"));
            Assert.False(sys.AdvanceWishStep("sv_1", "s2"));
            Assert.True(sys.AdvanceWishStep("sv_1", "s3"));
            Assert.True(sys.HasCompletedWish("sv_1"));
        }

        [Fact]
        public void Catalog_WishId_RoundTripsThroughSave()
        {
            var cat = TwoStepPool("medic_archetype", "wish_a", "wish_b");
            var sys = CreateSystem(seed: 11); sys.Catalog = cat;
            sys.DeclareTerminalPrognosis("sv_1", "medic_archetype", isAlive: true);
            string drawnId = sys.GetWishId("sv_1");
            Assert.NotEmpty(drawnId);

            var saved = sys.CaptureState();
            var sys2 = CreateSystem(); sys2.Catalog = cat;
            sys2.RestoreState(saved);

            Assert.Equal(drawnId, sys2.GetWishId("sv_1"));
        }

        [Fact]
        public void Catalog_LegacySave_MissingWishId_LoadsGracefully()
        {
            // A save authored without wishId (legacy format) must restore without throwing
            // and surface an empty wishId (degrades to wishType-only behavior).
            var legacy = new FinalWishSaveState
            {
                survivors = new System.Collections.Generic.List<FinalWishSurvivorState>
                {
                    new FinalWishSurvivorState
                    {
                        survivorId = "sv_legacy",
                        wishType = FinalWishSystem.WishDeliverLetter,
                        wishId = null!, // simulate a pre-wishId save
                        daysRemaining = 5f,
                        stepsCompleted = 0,
                        isActive = true,
                        hasTerminalPrognosis = true,
                        wishCompleted = false
                    }
                }
            };
            var sys = CreateSystem(); sys.Catalog = new TestCatalog();
            sys.RestoreState(legacy);
            Assert.True(sys.HasActiveWish("sv_legacy"));
            Assert.True(string.IsNullOrEmpty(sys.GetWishId("sv_legacy")));
            // Still completable via the wishType fallback (deliver_letter = 2 steps).
            Assert.False(sys.AdvanceWishStep("sv_legacy", "s1"));
            Assert.True(sys.AdvanceWishStep("sv_legacy", "s2"));
        }
    }
}
