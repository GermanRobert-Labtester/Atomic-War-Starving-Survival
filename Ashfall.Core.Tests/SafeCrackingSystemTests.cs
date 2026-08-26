using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SafeCrackingSystemTests
    {
        private static SafeDefinition DemoDef(string id = "safe_demo", int difficulty = 3, int maxAttempts = 10)
        {
            return new SafeDefinition
            {
                id = id,
                displayName = "Demo Safe",
                roomId = "room_demo",
                difficulty = difficulty,
                maxAttempts = maxAttempts,
                noisePerAttempt = 0.2f,
                alarmThreshold = 0.8f,
                loot = new List<SafeLootEntry>
                {
                    new SafeLootEntry { itemId = "scrap_metal", minQuantity = 2, maxQuantity = 5, weightKg = 1f },
                    new SafeLootEntry { itemId = "clean_water", minQuantity = 1, maxQuantity = 3, weightKg = 1.5f }
                }
            };
        }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        // ── Registration ─────────────────────────────────────────────

        [Fact]
        public void RegisterSafe_CreatesInstance()
        {
            var sys = new SafeCrackingSystem(42);
            Assert.True(sys.RegisterSafe(DemoDef(), "loc_demo"));
            Assert.NotNull(sys.GetSafe("safe_demo"));
        }

        [Fact]
        public void RegisterSafe_RejectsDuplicate()
        {
            var sys = new SafeCrackingSystem(42);
            Assert.True(sys.RegisterSafe(DemoDef(), "loc_demo"));
            Assert.False(sys.RegisterSafe(DemoDef(), "loc_demo"));
        }

        [Fact]
        public void RegisterSafe_RejectsNull()
        {
            var sys = new SafeCrackingSystem(42);
            Assert.False(sys.RegisterSafe(null, "loc_demo"));
            Assert.False(sys.RegisterSafe(new SafeDefinition(), "loc_demo")); // empty id
        }

        // ── Inspection ───────────────────────────────────────────────

        [Fact]
        public void InspectSafe_ReturnsState()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(), "loc_demo");
            var safe = sys.InspectSafe("safe_demo");
            Assert.NotNull(safe);
            Assert.Equal(3, safe!.difficulty);
        }

        [Fact]
        public void InspectSafe_ReturnsNullForUnknown()
        {
            var sys = new SafeCrackingSystem(42);
            Assert.Null(sys.InspectSafe("safe_unknown"));
        }

        // ── Attempt ──────────────────────────────────────────────────

        [Fact]
        public void Attempt_CorrectCombinationOpensSafe()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 1), "loc_demo");
            // Get the combination (we know it's deterministic)
            var safe = sys.GetSafe("safe_demo")!;
            int[] combo = (int[])safe.combination.Clone();
            var feedback = sys.Attempt("safe_demo", combo, 1.0f, Rng(1));
            Assert.Equal(SafeAttemptResult.Success, feedback.Result);
            Assert.True(sys.IsOpened("safe_demo"));
        }

        [Fact]
        public void Attempt_WrongCombinationFails()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var safe = sys.GetSafe("safe_demo")!;
            int[] wrong = new int[] { 9, 9, 9 };
            // Make sure it's actually wrong
            bool allMatch = true;
            for (int i = 0; i < safe.difficulty; i++)
                if (wrong[i] != safe.combination[i]) allMatch = false;
            if (allMatch) wrong[0] = (wrong[0] + 1) % 10;

            var feedback = sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.NotEqual(SafeAttemptResult.Success, feedback.Result);
            Assert.False(sys.IsOpened("safe_demo"));
        }

        [Fact]
        public void Attempt_IncrementsAttemptsUsed()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var safe = sys.GetSafe("safe_demo")!;
            int[] wrong = new int[] { 9, 9, 9 };
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.Equal(1, safe.attemptsUsed);
        }

        [Fact]
        public void Attempt_GeneratesNoise()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var safe = sys.GetSafe("safe_demo")!;
            int[] wrong = new int[] { 9, 9, 9 };
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.True(safe.cumulativeNoise > 0f);
        }

        [Fact]
        public void Attempt_DamagesTool()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            int[] wrong = new int[] { 9, 9, 9 };
            var feedback = sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.True(feedback.ToolCondition < 1.0f);
        }

        [Fact]
        public void Attempt_RejectsWhenOpened()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 1), "loc_demo");
            var safe = sys.GetSafe("safe_demo")!;
            sys.Attempt("safe_demo", safe.combination, 1.0f, Rng(1));
            var feedback = sys.Attempt("safe_demo", safe.combination, 1.0f, Rng(1));
            Assert.Equal(SafeAttemptResult.AlreadyOpened, feedback.Result);
        }

        [Fact]
        public void Attempt_RejectsWhenJammed()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3, maxAttempts: 1), "loc_demo");
            int[] wrong = new int[] { 9, 9, 9 };
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            var feedback = sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.Equal(SafeAttemptResult.Jammed, feedback.Result);
        }

        [Fact]
        public void Attempt_RejectsWrongGuessLength()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var feedback = sys.Attempt("safe_demo", new int[] { 1, 2 }, 1.0f, Rng(1));
            Assert.Equal(SafeAttemptResult.InvalidInput, feedback.Result);
        }

        [Fact]
        public void Attempt_RejectsLowToolCondition()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var feedback = sys.Attempt("safe_demo", new int[] { 1, 2, 3 }, 0.01f, Rng(1));
            Assert.Equal(SafeAttemptResult.ToolDamaged, feedback.Result);
        }

        // ── Jamming ──────────────────────────────────────────────────

        [Fact]
        public void Attempt_JamsAfterMaxAttempts()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3, maxAttempts: 2), "loc_demo");
            int[] wrong = new int[] { 9, 9, 9 };
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.True(sys.IsJammed("safe_demo"));
        }

        // ── Alarm ────────────────────────────────────────────────────

        [Fact]
        public void Attempt_TriggersAlarmWhenNoiseExceedsThreshold()
        {
            var sys = new SafeCrackingSystem(42);
            var def = DemoDef(difficulty: 3, maxAttempts: 20);
            def.alarmThreshold = 0.1f; // very low threshold
            sys.RegisterSafe(def, "loc_demo");
            int[] wrong = new int[] { 9, 9, 9 };
            bool alarmFired = false;
            sys.OnAlarmTriggered += _ => alarmFired = true;
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));
            Assert.True(alarmFired);
        }

        // ── Accessible mode ──────────────────────────────────────────

        [Fact]
        public void AttemptAccessible_CanOpenSafe()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 1), "loc_demo");
            // High confidence + high skill = good chance
            bool opened = false;
            for (int i = 0; i < 20; i++)
            {
                var feedback = sys.AttemptAccessible("safe_demo", 0.9f, 1.0f, 0.9f, Rng(i));
                if (feedback.Result == SafeAttemptResult.Success)
                {
                    opened = true;
                    break;
                }
            }
            Assert.True(opened, "Accessible mode should eventually open with high confidence/skill");
        }

        [Fact]
        public void AttemptAccessible_RejectsWhenOpened()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 1), "loc_demo");
            // Force open
            var safe = sys.GetSafe("safe_demo")!;
            safe.isOpened = true;
            var feedback = sys.AttemptAccessible("safe_demo", 0.9f, 1.0f, 0.9f, Rng(1));
            Assert.Equal(SafeAttemptResult.AlreadyOpened, feedback.Result);
        }

        // ── Loot transfer ────────────────────────────────────────────

        [Fact]
        public void TransferLoot_ReturnsLootFromOpenedSafe()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 1), "loc_demo");
            var safe = sys.GetSafe("safe_demo")!;
            // Open the safe
            sys.Attempt("safe_demo", safe.combination, 1.0f, Rng(1));
            var loot = sys.TransferLoot("safe_demo", Rng(1));
            Assert.NotNull(loot);
            Assert.True(loot!.Count > 0);
        }

        [Fact]
        public void TransferLoot_ReturnsNullWhenNotOpened()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            Assert.Null(sys.TransferLoot("safe_demo", Rng(1)));
        }

        [Fact]
        public void TransferLoot_CannotTransferTwice()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 1), "loc_demo");
            var safe = sys.GetSafe("safe_demo")!;
            sys.Attempt("safe_demo", safe.combination, 1.0f, Rng(1));
            sys.TransferLoot("safe_demo", Rng(1));
            Assert.Null(sys.TransferLoot("safe_demo", Rng(1)));
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void SameSeed_SameCombination()
        {
            var a = new SafeCrackingSystem(42);
            a.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var b = new SafeCrackingSystem(42);
            b.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            Assert.Equal(a.GetSafe("safe_demo")!.combination, b.GetSafe("safe_demo")!.combination);
        }

        [Fact]
        public void DifferentSeed_DifferentCombination()
        {
            var a = new SafeCrackingSystem(42);
            a.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var b = new SafeCrackingSystem(99);
            b.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            Assert.NotEqual(a.GetSafe("safe_demo")!.combination, b.GetSafe("safe_demo")!.combination);
        }

        [Fact]
        public void SameSafe_SameAttemptOutcome()
        {
            var sysA = new SafeCrackingSystem(42);
            sysA.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            var sysB = new SafeCrackingSystem(42);
            sysB.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");

            int[] guess = new int[] { 1, 2, 3 };
            var fA = sysA.Attempt("safe_demo", guess, 1.0f, Rng(7));
            var fB = sysB.Attempt("safe_demo", guess, 1.0f, Rng(7));

            Assert.Equal(fA.Result, fB.Result);
            Assert.Equal(fA.CorrectTumblers, fB.CorrectTumblers);
            Assert.Equal(fA.NoiseLevel, fB.NoiseLevel);
        }

        // ── Save/Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            int[] wrong = new int[] { 9, 9, 9 };
            sys.Attempt("safe_demo", wrong, 1.0f, Rng(1));

            var state = sys.CaptureState();
            var sys2 = new SafeCrackingSystem(42);
            sys2.RestoreState(state);

            var safe = sys2.GetSafe("safe_demo");
            Assert.NotNull(safe);
            Assert.Equal(1, safe!.attemptsUsed);
            Assert.True(safe.cumulativeNoise > 0f);
        }

        [Fact]
        public void CaptureState_OrdinalOrdered()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef("safe_z", 3), "loc_demo");
            sys.RegisterSafe(DemoDef("safe_a", 3), "loc_demo");
            var state = sys.CaptureState();
            Assert.Equal("safe_a", state.safes[0].safeId);
            Assert.Equal("safe_z", state.safes[1].safeId);
        }

        [Fact]
        public void Checksum_Stable()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            string before = SaveChecksum.Compute(sys.CaptureState());

            var sys2 = new SafeCrackingSystem(42);
            sys2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(sys2.CaptureState());

            Assert.Equal(before, after);
        }

        // ── Abandon ──────────────────────────────────────────────────

        [Fact]
        public void Abandon_ReturnsTrueForKnownSafe()
        {
            var sys = new SafeCrackingSystem(42);
            sys.RegisterSafe(DemoDef(difficulty: 3), "loc_demo");
            Assert.True(sys.Abandon("safe_demo"));
        }

        [Fact]
        public void Abandon_ReturnsFalseForUnknown()
        {
            var sys = new SafeCrackingSystem(42);
            Assert.False(sys.Abandon("safe_unknown"));
        }
    }
}
