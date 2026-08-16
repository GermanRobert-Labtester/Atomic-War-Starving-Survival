using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SomaticFlashbackSystemTests
    {
        // ── Helpers ────────────────────────────────────────────────────

        private static SomaticFlashbackSystem CreateSystem(
            IReadOnlyList<string> aliveSurvivors,
            Func<string, string, bool> companionCheck = null!,
            int rngSeed = 42)
        {
            var sys = new SomaticFlashbackSystem
            {
                GetAliveSurvivorIds = () => aliveSurvivors,
                Rng = new SeededRng(rngSeed),
                IsCompanionInSameRoom = companionCheck
            };
            return sys;
        }

        // ── Tests ──────────────────────────────────────────────────────

        [Fact]
        public void IncreaseSusceptibility_ClampsAt1()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv_1", 0.8f);
            Assert.Equal(0.8f, sys.GetSusceptibility("sv_1"));
            sys.IncreaseSusceptibility("sv_1", 0.5f);
            Assert.Equal(1f, sys.GetSusceptibility("sv_1"));
        }

        [Fact]
        public void IncreaseSusceptibility_RejectsNullOrEmpty()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("", 0.5f);
            sys.IncreaseSusceptibility(null, 0.5f);
            sys.IncreaseSusceptibility("sv_1", -0.1f);
            Assert.Equal(0f, sys.GetSusceptibility("sv_1"));
        }

        [Fact]
        public void IncreaseSusceptibility_FiresStateChanged()
        {
            var sys = new SomaticFlashbackSystem();
            int fired = 0;
            sys.OnStateChanged += () => fired++;
            sys.IncreaseSusceptibility("sv_1", 0.3f);
            Assert.Equal(1, fired);
        }

        [Fact]
        public void OnAudioEvent_NoSusceptibility_NoFlashback()
        {
            var survivors = new List<string> { "sv_1" };
            var sys = CreateSystem(survivors);
            // No susceptibility set → should not trigger
            sys.OnAudioEvent("siren", 1.0f);
            Assert.False(sys.HasActiveFlashback("sv_1"));
        }

        [Fact]
        public void OnAudioEvent_WithSusceptibility_CanTrigger()
        {
            // Use seed that produces a low roll to guarantee trigger
            var survivors = new List<string> { "sv_1" };
            var sys = CreateSystem(survivors, rngSeed: 1);
            sys.IncreaseSusceptibility("sv_1", 1.0f); // max susceptibility

            string triggeredFor = null;
            sys.OnFlashbackTriggered += (id, dur) => triggeredFor = id;
            sys.OnAudioEvent("siren", 1.0f);

            // With susceptibility=1, noiseSeverity=1, base=0.15 → chance=0.15
            // Try multiple seeds to find one that triggers
            bool triggered = false;
            for (int seed = 0; seed < 100; seed++)
            {
                var testSys = CreateSystem(survivors, rngSeed: seed);
                testSys.IncreaseSusceptibility("sv_1", 1.0f);
                testSys.OnAudioEvent("siren", 1.0f);
                if (testSys.HasActiveFlashback("sv_1"))
                {
                    triggered = true;
                    break;
                }
            }
            Assert.True(triggered, "At least one seed should trigger a flashback");
        }

        [Fact]
        public void OnAudioEvent_GroundedByCompanion_ReducesPenalty()
        {
            var survivors = new List<string> { "sv_1", "sv_2" };
            // Find a seed that triggers
            for (int seed = 0; seed < 200; seed++)
            {
                var sys = CreateSystem(survivors,
                    companionCheck: (a, b) => true, // always same room
                    rngSeed: seed);
                sys.IncreaseSusceptibility("sv_1", 1.0f);

                string groundedId = null;
                sys.OnFlashbackGrounded += (id, orig, reduced) => groundedId = id;
                sys.OnAudioEvent("siren", 1.0f);

                if (sys.HasActiveFlashback("sv_1") && sys.IsGroundedByCompanion("sv_1"))
                {
                    Assert.Equal("sv_1", groundedId);
                    Assert.Equal(SomaticFlashbackSystem.GroundedWorkEfficiencyPenalty,
                        sys.GetWorkEfficiencyPenalty("sv_1"));
                    return;
                }
            }
            // If no seed triggered, that's still a valid test outcome but unlikely
            Assert.True(false, "No seed triggered a grounded flashback in 200 tries");
        }

        [Fact]
        public void OnAudioEvent_NotGrounded_FullPenalty()
        {
            var survivors = new List<string> { "sv_1" };
            for (int seed = 0; seed < 200; seed++)
            {
                var sys = CreateSystem(survivors, rngSeed: seed);
                sys.IncreaseSusceptibility("sv_1", 1.0f);
                sys.OnAudioEvent("siren", 1.0f);

                if (sys.HasActiveFlashback("sv_1") && !sys.IsGroundedByCompanion("sv_1"))
                {
                    Assert.Equal(SomaticFlashbackSystem.FlashbackWorkEfficiencyPenalty,
                        sys.GetWorkEfficiencyPenalty("sv_1"));
                    return;
                }
            }
            Assert.True(false, "No seed triggered an ungrounded flashback in 200 tries");
        }

        [Fact]
        public void Tick_DecaysSusceptibility()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv_1", 0.5f);
            float before = sys.GetSusceptibility("sv_1");
            sys.Tick("sv_1", 24f); // 1 day
            float after = sys.GetSusceptibility("sv_1");
            Assert.True(after < before, $"Expected {after} < {before}");
            Assert.Equal(Math.Max(0f, 0.5f - SomaticFlashbackSystem.FlashbackDecayPerDay),
                after, 4);
        }

        [Fact]
        public void Tick_EndsFlashback_WhenTimerExpires()
        {
            // Set up a system with a known active flashback via restore
            var sys = new SomaticFlashbackSystem();
            var save = new SomaticFlashbackSaveState();
            save.survivors.Add(new FlashbackSurvivorState
            {
                survivorId = "sv_1",
                susceptibility = 0.5f,
                activeRemainingHours = 3f,
                workEfficiencyPenalty = 0.6f,
                isGroundedByCompanion = false
            });
            sys.RestoreState(save);

            string endedFor = null;
            sys.OnFlashbackEnded += id => endedFor = id;

            Assert.True(sys.HasActiveFlashback("sv_1"));
            sys.Tick("sv_1", 4f); // more than 3h remaining
            Assert.False(sys.HasActiveFlashback("sv_1"));
            Assert.Equal(0f, sys.GetWorkEfficiencyPenalty("sv_1"));
            Assert.Equal("sv_1", endedFor);
        }

        [Fact]
        public void Tick_PartialDecrement_KeepsFlashbackActive()
        {
            var sys = new SomaticFlashbackSystem();
            var save = new SomaticFlashbackSaveState();
            save.survivors.Add(new FlashbackSurvivorState
            {
                survivorId = "sv_1",
                susceptibility = 0.5f,
                activeRemainingHours = 5f,
                workEfficiencyPenalty = 0.6f,
                isGroundedByCompanion = false
            });
            sys.RestoreState(save);

            sys.Tick("sv_1", 2f);
            Assert.True(sys.HasActiveFlashback("sv_1"));
            Assert.Equal(3f, sys.GetActiveFlashbackRemaining("sv_1"), 4);
        }

        [Fact]
        public void HasActiveFlashback_UnknownSurvivor_ReturnsFalse()
        {
            var sys = new SomaticFlashbackSystem();
            Assert.False(sys.HasActiveFlashback("nonexistent"));
        }

        [Fact]
        public void CaptureRestore_Roundtrip()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv_1", 0.5f);
            sys.IncreaseSusceptibility("sv_2", 0.3f);

            // Manually set up an active flashback via save state
            var save = new SomaticFlashbackSaveState();
            save.survivors.Add(new FlashbackSurvivorState
            {
                survivorId = "sv_1",
                susceptibility = 0.5f,
                activeRemainingHours = 3f,
                workEfficiencyPenalty = 0.6f,
                isGroundedByCompanion = true
            });
            save.survivors.Add(new FlashbackSurvivorState
            {
                survivorId = "sv_2",
                susceptibility = 0.3f,
                activeRemainingHours = 0f,
                workEfficiencyPenalty = 0f,
                isGroundedByCompanion = false
            });
            sys.RestoreState(save);

            var captured = sys.CaptureState();
            Assert.Equal(2, captured.survivors.Count);

            var restored = new SomaticFlashbackSystem();
            restored.RestoreState(captured);

            Assert.Equal(0.5f, restored.GetSusceptibility("sv_1"));
            Assert.Equal(3f, restored.GetActiveFlashbackRemaining("sv_1"), 4);
            Assert.True(restored.HasActiveFlashback("sv_1"));
            Assert.True(restored.IsGroundedByCompanion("sv_1"));
            Assert.Equal(0.3f, restored.GetSusceptibility("sv_2"));
            Assert.False(restored.HasActiveFlashback("sv_2"));
        }

        [Fact]
        public void RestoreState_DeepCopy_MutatingOriginalDoesNotAffectRestored()
        {
            var sys = new SomaticFlashbackSystem();
            var save = new SomaticFlashbackSaveState();
            save.survivors.Add(new FlashbackSurvivorState
            {
                survivorId = "sv_1",
                susceptibility = 0.7f,
                activeRemainingHours = 4f,
                workEfficiencyPenalty = 0.6f,
                isGroundedByCompanion = false
            });
            sys.RestoreState(save);

            // Mutate the original save DTO
            save.survivors[0].susceptibility = 0.0f;
            save.survivors[0].activeRemainingHours = 0f;

            // Restored system should be unaffected
            Assert.Equal(0.7f, sys.GetSusceptibility("sv_1"));
            Assert.Equal(4f, sys.GetActiveFlashbackRemaining("sv_1"), 4);
        }

        [Fact]
        public void RestoreNull_DoesNotCrash()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv_1", 0.5f);
            sys.RestoreState(null);
            // Should have cleared everything
            Assert.Equal(0f, sys.GetSusceptibility("sv_1"));
        }

        [Fact]
        public void TickAll_DecaysAllKnownSurvivors()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv_1", 0.5f);
            sys.IncreaseSusceptibility("sv_2", 0.8f);
            sys.TickAll(24f);
            Assert.True(sys.GetSusceptibility("sv_1") < 0.5f);
            Assert.True(sys.GetSusceptibility("sv_2") < 0.8f);
        }

        [Fact]
        public void OnAudioEvent_NullSurvivorList_DoesNotCrash()
        {
            var sys = new SomaticFlashbackSystem { GetAliveSurvivorIds = () => null };
            sys.OnAudioEvent("siren", 1.0f); // should not throw
        }

        [Fact]
        public void OnAudioEvent_EmptySurvivorList_DoesNotCrash()
        {
            var sys = new SomaticFlashbackSystem
            {
                GetAliveSurvivorIds = () => new List<string>()
            };
            sys.OnAudioEvent("siren", 1.0f); // should not throw
        }

        [Fact]
        public void FlashbackDuration_IsWithinExpectedRange()
        {
            var survivors = new List<string> { "sv_1" };
            for (int seed = 0; seed < 200; seed++)
            {
                var sys = CreateSystem(survivors, rngSeed: seed);
                sys.IncreaseSusceptibility("sv_1", 1.0f);
                sys.OnAudioEvent("siren", 1.0f);

                if (sys.HasActiveFlashback("sv_1"))
                {
                    float remaining = sys.GetActiveFlashbackRemaining("sv_1");
                    Assert.InRange(remaining,
                        SomaticFlashbackSystem.MinFlashbackDurationHours,
                        SomaticFlashbackSystem.MaxFlashbackDurationHours);
                    return;
                }
            }
            Assert.True(false, "No seed triggered in 200 tries");
        }

        [Fact]
        public void Tick_SusceptibilityDecay_FiresStateChanged()
        {
            var sys = new SomaticFlashbackSystem();
            sys.IncreaseSusceptibility("sv_1", 0.5f);
            int stateChanges = 0;
            sys.OnStateChanged += () => stateChanges++;
            sys.Tick("sv_1", 24f);
            Assert.True(stateChanges >= 1, "Tick should fire OnStateChanged when susceptibility decays");
        }
    }
}
