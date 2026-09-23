using System.Collections.Generic;
using Ashfall.Core.Voice;
using Xunit;

namespace Ashfall.Core.Tests.Voice
{
    public class VoiceLineSelectionEngineTests
    {
        [Fact]
        public void SelectLine_IncapacitatedSurvivor_ReturnsNone()
        {
            var survivor = new SurvivorVoiceContext(
                survivorId: "surv_01",
                voiceProfileId: "profile_gruff",
                moralePermille: 500,
                stressPermille: 200,
                isIncapacitated: true
            );

            var candidates = new[]
            {
                new VoiceLineCandidate { LineId = "l1", EventKind = "scavenge_success" }
            };

            var result = VoiceLineSelectionEngine.SelectLine(survivor, "scavenge_success", candidates);

            Assert.False(result.HasLine);
        }

        [Fact]
        public void SelectLine_MatchesEventAndStressConditions()
        {
            var survivor = new SurvivorVoiceContext(
                survivorId: "surv_02",
                voiceProfileId: "profile_calm",
                moralePermille: 600,
                stressPermille: 850, // High stress
                traumaTags: new[] { "claustrophobic" }
            );

            var candidates = new[]
            {
                new VoiceLineCandidate
                {
                    LineId = "calm_line",
                    EventKind = "storm_warning",
                    MinStressPermille = 0,
                    MaxStressPermille = 400,
                    PriorityWeight = 50
                },
                new VoiceLineCandidate
                {
                    LineId = "panic_line",
                    VoiceKey = "voice.storm.panic",
                    TextKey = "text.storm.panic",
                    AudioCueKey = "cue_voice_panic",
                    EventKind = "storm_warning",
                    MinStressPermille = 700,
                    MaxStressPermille = 1000,
                    PriorityWeight = 150
                }
            };

            var result = VoiceLineSelectionEngine.SelectLine(survivor, "storm_warning", candidates);

            Assert.True(result.HasLine);
            Assert.Equal("panic_line", result.LineId);
            Assert.Equal("voice.storm.panic", result.VoiceKey);
            Assert.Equal("surv_02", result.SpeakerSurvivorId);
        }

        [Fact]
        public void SelectLine_TraumaTagRequirement_FiltersIneligibleLines()
        {
            var survivorWithoutTag = new SurvivorVoiceContext(
                survivorId: "surv_03",
                voiceProfileId: "profile_young",
                moralePermille: 500,
                stressPermille: 300,
                traumaTags: new[] { "insomniac" }
            );

            var candidates = new[]
            {
                new VoiceLineCandidate
                {
                    LineId = "grief_line",
                    EventKind = "comrade_death",
                    RequiredTraumaTag = "grief_stricken",
                    PriorityWeight = 200
                },
                new VoiceLineCandidate
                {
                    LineId = "standard_grief_line",
                    VoiceKey = "voice.grief.general",
                    EventKind = "comrade_death",
                    PriorityWeight = 100
                }
            };

            var result = VoiceLineSelectionEngine.SelectLine(survivorWithoutTag, "comrade_death", candidates);

            Assert.True(result.HasLine);
            Assert.Equal("standard_grief_line", result.LineId);
        }

        [Fact]
        public void SelectLine_DeterministicSeed_ProvidesConsistentSelection()
        {
            var survivor = new SurvivorVoiceContext(
                survivorId: "surv_04",
                voiceProfileId: "profile_cynic",
                moralePermille: 500,
                stressPermille: 300
            );

            var candidates = new[]
            {
                new VoiceLineCandidate { LineId = "tie_a", EventKind = "idle", PriorityWeight = 100 },
                new VoiceLineCandidate { LineId = "tie_b", EventKind = "idle", PriorityWeight = 100 }
            };

            var result1 = VoiceLineSelectionEngine.SelectLine(survivor, "idle", candidates, deterministicSeed: 42);
            var result2 = VoiceLineSelectionEngine.SelectLine(survivor, "idle", candidates, deterministicSeed: 42);

            Assert.True(result1.HasLine);
            Assert.Equal(result1.LineId, result2.LineId);
        }
    }
}
