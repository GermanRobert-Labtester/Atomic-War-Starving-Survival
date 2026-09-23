// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Voice;
using Xunit;

namespace Ashfall.Core.Tests.Voice
{
    public class VoiceLineDispatchCoordinatorTests
    {
        [Fact]
        public void Dispatch_ValidSelection_DispatchesSuccessfully()
        {
            var coordinator = new VoiceLineDispatchCoordinator();
            var candidates = new List<VoiceLineCandidate>
            {
                new VoiceLineCandidate
                {
                    LineId = "line_work_01",
                    VoiceKey = "voice_gruff",
                    TextKey = "txt_work_done",
                    AudioCueKey = "cue_work_done",
                    EventKind = "work_completed",
                    PriorityWeight = 100
                }
            };

            var survivor = new SurvivorVoiceContext("survivor_1", "voice_gruff", 500, 200);

            VoicePlaybackPayload? dispatchedPayload = null;
            coordinator.OnVoiceDispatched += p => dispatchedPayload = p;

            var result = coordinator.TrySelectAndDispatch(
                survivor,
                "work_completed",
                candidates,
                VoiceLinePriority.TaskWork,
                currentTick: 100,
                durationTicks: 10);

            Assert.True(result.Dispatched);
            Assert.NotNull(result.Payload);
            Assert.Equal("survivor_1", result.Payload.SpeakerSurvivorId);
            Assert.Equal("line_work_01", result.Payload.LineId);
            Assert.Equal(VoiceLinePriority.TaskWork, result.Payload.Priority);
            Assert.True(coordinator.IsPlaying(105));
            Assert.False(coordinator.IsPlaying(111));
            Assert.NotNull(dispatchedPayload);
        }

        [Fact]
        public void PerCharacterCooldown_RejectsRapidSuccessiveCalls()
        {
            var coordinator = new VoiceLineDispatchCoordinator
            {
                PerCharacterCooldownTicks = 20,
                GlobalCooldownTicks = 5
            };

            var selection = new VoiceLineSelectionResult(true, "line_1", "voice_1", "txt_1", "cue_1", "survivor_1");

            // First dispatch at tick 10 with duration 5
            var res1 = coordinator.TryDispatch(selection, VoiceLinePriority.AmbientIdle, currentTick: 10, durationTicks: 5);
            Assert.True(res1.Dispatched);

            // Tick 16: Active playback ended at tick 15, but per-character cooldown is 20 (ends at tick 30)
            var res2 = coordinator.TryDispatch(selection, VoiceLinePriority.AmbientIdle, currentTick: 16, durationTicks: 5);
            Assert.False(res2.Dispatched);
            Assert.Equal("PerCharacterCooldownActive", res2.RejectionReason);

            // Tick 31: Cooldown elapsed
            var res3 = coordinator.TryDispatch(selection, VoiceLinePriority.AmbientIdle, currentTick: 31, durationTicks: 5);
            Assert.True(res3.Dispatched);
        }

        [Fact]
        public void GlobalCooldown_EnforcesSpacingBetweenDifferentSurvivors()
        {
            var coordinator = new VoiceLineDispatchCoordinator
            {
                PerCharacterCooldownTicks = 30,
                GlobalCooldownTicks = 5
            };

            var selA = new VoiceLineSelectionResult(true, "line_a", "voice_a", "txt_a", "cue_a", "survivor_A");
            var selB = new VoiceLineSelectionResult(true, "line_b", "voice_b", "txt_b", "cue_b", "survivor_B");

            // Survivor A speaks at tick 10 with duration 2 (ends at 12)
            var resA = coordinator.TryDispatch(selA, VoiceLinePriority.AmbientIdle, currentTick: 10, durationTicks: 2);
            Assert.True(resA.Dispatched);

            // Survivor B tries to speak at tick 13 (line A has finished, but global cooldown 5 ticks requires tick >= 15)
            var resB = coordinator.TryDispatch(selB, VoiceLinePriority.AmbientIdle, currentTick: 13, durationTicks: 2);
            Assert.False(resB.Dispatched);
            Assert.Equal("GlobalCooldownActive", resB.RejectionReason);

            // Tick 15: Global cooldown elapsed
            var resB2 = coordinator.TryDispatch(selB, VoiceLinePriority.AmbientIdle, currentTick: 15, durationTicks: 2);
            Assert.True(resB2.Dispatched);
        }

        [Fact]
        public void PriorityPreemption_CrisisBarkInterruptsAmbientLine()
        {
            var coordinator = new VoiceLineDispatchCoordinator();

            var ambientSel = new VoiceLineSelectionResult(true, "ambient_line", "v1", "t1", "c1", "survivor_1");
            var crisisSel = new VoiceLineSelectionResult(true, "crisis_line", "v2", "t2", "c2", "survivor_2");

            // Start ambient line at tick 50 with long duration (20 ticks, ends at 70)
            var res1 = coordinator.TryDispatch(ambientSel, VoiceLinePriority.AmbientIdle, currentTick: 50, durationTicks: 20);
            Assert.True(res1.Dispatched);

            VoicePlaybackPayload? preemptedPayload = null;
            string? preemptReason = null;
            coordinator.OnVoicePreempted += (p, r) =>
            {
                preemptedPayload = p;
                preemptReason = r;
            };

            // At tick 55, a crisis occurs -> CrisisBark
            var res2 = coordinator.TryDispatch(crisisSel, VoiceLinePriority.CrisisBark, currentTick: 55, durationTicks: 8);
            Assert.True(res2.Dispatched);
            Assert.NotNull(preemptedPayload);
            Assert.Equal("ambient_line", preemptedPayload.LineId);
            Assert.Equal("PreemptedBy_CrisisBark", preemptReason);

            // Active playback is now the crisis bark
            Assert.Equal("crisis_line", coordinator.ActivePlayback?.LineId);
            Assert.Equal("survivor_2", coordinator.ActivePlayback?.SpeakerSurvivorId);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesCooldownsAndActivePlayback()
        {
            var coordinator1 = new VoiceLineDispatchCoordinator();
            var sel = new VoiceLineSelectionResult(true, "line_alpha", "v", "t", "c", "survivor_x");
            coordinator1.TryDispatch(sel, VoiceLinePriority.TaskWork, currentTick: 200, durationTicks: 15);

            var saveState = coordinator1.CaptureState();
            Assert.NotNull(saveState);
            Assert.Equal("survivor_x", saveState.ActiveSpeakerId);
            Assert.Equal(215, saveState.ActiveLineEndTick);

            var coordinator2 = new VoiceLineDispatchCoordinator();
            coordinator2.RestoreState(saveState);

            Assert.True(coordinator2.IsPlaying(205));
            Assert.Equal("survivor_x", coordinator2.ActivePlayback?.SpeakerSurvivorId);
            Assert.Equal("line_alpha", coordinator2.ActivePlayback?.LineId);
        }
    }
}
