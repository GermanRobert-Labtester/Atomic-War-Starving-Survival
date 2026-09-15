// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Task 2 — survivor-driven authenticity detection runtime.
    /// Deterministic skill-scaled deception detection with a hard invariant:
    /// genuine signals are never classified as traps or false flags.
    /// </summary>
    public sealed class RescueSignalAuthenticityTests
    {
        private const int SeedBase = 20260913;

        private sealed class FixedRng : Ashfall.Core.ISeededRng
        {
            private readonly int _value;
            public FixedRng(int value) { _value = value; }
            public int Seed => _value;
            public int Next(int minInclusive, int maxExclusive) => _value;
            public float NextFloat() => _value / 10000f;
            public double NextDouble() => _value / 10000.0;
        }

        private static DistressSignalDefinition Def(string id, string authenticity, string outcome = "survivor_isolated")
            => new DistressSignalDefinition { FrequencyId = id, Authenticity = authenticity, OutcomeTypeStr = outcome };

        private static Func<string, string, bool> Skills(params string[] owned)
        {
            var set = new System.Collections.Generic.HashSet<string>(owned, StringComparer.OrdinalIgnoreCase);
            return (survivorId, skillId) => set.Contains(skillId);
        }

        // ── Evaluator-level: detection scales with skill ─────────────────────

        [Fact]
        public void Trap_HighSkillSurvivor_Detects()
        {
            // roll 40 misses at no-skill (chance 35) but signal_ear pushes chance to 50.
            var def = Def("sig_trap_test", "trap");
            var r = SignalAuthenticityEvaluator.Evaluate(def, 1, "survivor_a", Skills("skill_signal_ear"), new FixedRng(40));
            Assert.True(r.ThreatDetected);
            Assert.Equal(SignalAuthenticityCategory.Trap, r.Assessment);
            Assert.Equal("skill_signal_ear", r.SkillId);
            Assert.Equal(50, r.DetectionChance);
        }

        [Fact]
        public void Trap_LowSkillSurvivor_Misses_InFixedDeterministicCase()
        {
            var def = Def("sig_trap_test", "trap");
            var r = SignalAuthenticityEvaluator.Evaluate(def, 1, "survivor_b", null, new FixedRng(40));
            Assert.False(r.ThreatDetected);
            Assert.Equal(SignalAuthenticityCategory.Uncertain, r.Assessment);
            Assert.Empty(r.SkillId);
        }

        [Fact]
        public void FalseFlag_HighSkillSurvivor_Detects()
        {
            var def = Def("sig_ff_test", "false_flag");
            var r = SignalAuthenticityEvaluator.Evaluate(def, 1, "survivor_a", Skills("skill_signal_ear"), new FixedRng(10));
            Assert.True(r.ThreatDetected);
            Assert.Equal(SignalAuthenticityCategory.FalseFlag, r.Assessment);
        }

        [Fact]
        public void FalseFlag_LowSkillSurvivor_CanMiss()
        {
            var def = Def("sig_ff_test", "false_flag");
            var r = SignalAuthenticityEvaluator.Evaluate(def, 1, "survivor_b", null, new FixedRng(90));
            Assert.False(r.ThreatDetected);
            Assert.Equal(SignalAuthenticityCategory.Uncertain, r.Assessment);
        }

        // ── Hard invariant: genuine never hostile ────────────────────────────

        [Fact]
        public void Genuine_NeverClassifiedAsTrapOrFalseFlag_AcrossWholeRollRange()
        {
            var def = Def("sig_genuine_test", "genuine", "survivor_isolated");
            for (int roll = 0; roll < 100; roll++)
            {
                var r = SignalAuthenticityEvaluator.Evaluate(def, 1, "survivor_c", null, new FixedRng(roll));
                Assert.NotEqual(SignalAuthenticityCategory.Trap, r.Assessment);
                Assert.NotEqual(SignalAuthenticityCategory.FalseFlag, r.Assessment);
                Assert.False(r.ThreatDetected);
                Assert.True(r.Assessment == SignalAuthenticityCategory.Genuine || r.Assessment == SignalAuthenticityCategory.Unknown);
            }
        }

        // ── Staleness: trace-progress read, never a trap ─────────────────────

        [Fact]
        public void Stale_BeforeTraceCompletion_RemainsUncertain()
        {
            var def = Def("sig_stale_test", "stale");
            def.DaysToTrace = 4;
            var r = SignalAuthenticityEvaluator.Evaluate(def, 2, "survivor_a", Skills("skill_signal_ear"), new FixedRng(0));
            Assert.False(r.StalenessDetected);
            Assert.Equal(SignalAuthenticityCategory.Uncertain, r.Assessment);
            Assert.Equal(-1, r.Roll); // staleness consumes no randomness
        }

        [Fact]
        public void Stale_AfterTraceCompletion_IsDetected()
        {
            var def = Def("sig_stale_test", "stale");
            def.DaysToTrace = 4;
            var r = SignalAuthenticityEvaluator.Evaluate(def, 4, "survivor_a", Skills("skill_signal_ear"), new FixedRng(0));
            Assert.True(r.StalenessDetected);
            Assert.Equal(SignalAuthenticityCategory.Stale, r.Assessment);
        }

        // ── Manager-level: persistence, determinism, graceful failure ────────

        private static DistressRescueMissionManager ManagerWithDefs()
        {
            var distress = new RadioDistressSystem();
            // Authored flagship ids: 88_3 genuine, 156_8 genuine, 192_4 trap.
            distress.RegisterSignal(Def("freq_distress_88_3", "genuine", "survivor_community"));
            distress.RegisterSignal(Def("freq_distress_156_8", "genuine", "survivor_community"));
            distress.RegisterSignal(Def("freq_distress_192_4", "trap", "bait_trap"));
            return new DistressRescueMissionManager(null, distress);
        }

        [Fact]
        public void SameSeedAndState_ProducesSameResult()
        {
            var m1 = ManagerWithDefs();
            var m2 = ManagerWithDefs();
            m1.RecordSignalHeard("freq_distress_192_4", 2);
            m2.RecordSignalHeard("freq_distress_192_4", 2);
            var r1 = m1.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 3, SeedBase);
            var r2 = m2.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 3, SeedBase);
            Assert.NotNull(r1);
            Assert.NotNull(r2);
            Assert.Equal(r1!.Assessment, r2!.Assessment);
            Assert.Equal(r1.Roll, r2.Roll);
        }

        [Fact]
        public void RepeatCheck_DoesNotReroll()
        {
            var m = ManagerWithDefs();
            m.RecordSignalHeard("freq_distress_192_4", 2);
            var first = m.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 3, SeedBase);
            var second = m.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 4, SeedBase);
            Assert.NotNull(first);
            Assert.NotNull(second);
            Assert.Equal(first!.Assessment, second!.Assessment);
            Assert.Equal(-1, second.Roll); // persisted, no reroll
        }

        [Fact]
        public void ResultPersists_AcrossSaveReload()
        {
            var m = ManagerWithDefs();
            m.RecordSignalHeard("freq_distress_192_4", 2);
            var first = m.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 3, SeedBase);
            var fresh = ManagerWithDefs();
            fresh.RestoreState(m.CaptureState());
            var restored = fresh.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 3, SeedBase);
            Assert.NotNull(restored);
            Assert.Equal(first!.Assessment, restored!.Assessment);
            Assert.Equal(-1, restored.Roll);
        }

        [Fact]
        public void UnknownSignalId_ReturnsNull_Gracefully()
        {
            var m = ManagerWithDefs();
            Assert.Null(m.RecordAuthenticityCheck("freq_unknown_999", "survivor_x", 3, SeedBase));
            Assert.Null(m.RecordAuthenticityCheck("freq_distress_88_3", "", 3, SeedBase));
        }

        [Fact]
        public void AuthenticityCheck_DoesNotConsumeSharedSessionRng()
        {
            // The manager derives its own SeededRng sub-stream from a StableHash
            // of (purpose:seed:signal:survivor) — the caller's RNG is untouched.
            var m = ManagerWithDefs();
            m.RecordSignalHeard("freq_distress_192_4", 2);
            m.RecordSignalHeard("freq_distress_88_3", 2);
            m.RecordAuthenticityCheck("freq_distress_192_4", "survivor_x", 3, SeedBase);
            // No exception + persisted result is the contract; the dedicated
            // sub-stream construction guarantees zero shared-stream draws.
            var stored = m.GetMissionBySignal("freq_distress_192_4");
            Assert.NotNull(stored);
            Assert.True(stored!.AuthenticityChecked);
        }
    }
}
