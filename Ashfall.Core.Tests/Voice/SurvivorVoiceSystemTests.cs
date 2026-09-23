// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Voice;
using Xunit;

namespace Ashfall.Core.Tests.Voice
{
    public sealed class SurvivorVoiceSystemTests
    {
        [Fact]
        public void CatalogLoading_LoadsFromAuthoredJson()
        {
            var system = new SurvivorVoiceSystem();
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/survivor_voice_lines.json");
            if (!File.Exists(path))
            {
                path = "Assets/StreamingAssets/Data/survivor_voice_lines.json";
            }

            Assert.True(File.Exists(path), $"File should exist at {path}");
            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            Assert.True(system.Catalog.Count >= 8);
        }

        [Fact]
        public void VoiceLineSelection_SelectsAppropriateLineByTrigger()
        {
            var system = new SurvivorVoiceSystem();
            system.RegisterLine(new VoiceLineDefinition
            {
                Id = "line_perished_01",
                Speaker = "fitter",
                Trigger = "survivor_perished",
                Register = "clipped",
                TextKey = "voice.perished.01",
                TextEnglish = "Take their wrench and tag the locker.",
                MinMorale = 0f,
                MaxMorale = 100f,
                CooldownDays = 2
            });

            var context = new SurvivorSpeechContext
            {
                SurvivorId = "surv_fitter_01",
                Profession = "fitter",
                Morale = 40f,
                Fatigue = 20f
            };

            bool ok = system.TrySelectVoiceLine(context, "survivor_perished", currentDay: 1, rng: null, out var payload);

            Assert.True(ok);
            Assert.Equal("line_perished_01", payload.LineId);
            Assert.Equal("surv_fitter_01", payload.SpeakerSurvivorId);
            Assert.Equal("fitter", payload.Profession);
            Assert.Equal("voice.perished.01", payload.TextKey);
            Assert.Single(system.History);
        }

        [Fact]
        public void VoiceLineCooldown_PreventsSameSurvivorRapidSpeech()
        {
            var system = new SurvivorVoiceSystem();
            system.RegisterLine(new VoiceLineDefinition
            {
                Id = "line_ration_01",
                Speaker = "any",
                Trigger = "ration_cut",
                Register = "weary",
                TextKey = "voice.ration.01",
                TextEnglish = "Belt notched tighter again.",
                CooldownDays = 2
            });

            var context = new SurvivorSpeechContext
            {
                SurvivorId = "surv_scout_01",
                Profession = "scavenger",
                Morale = 30f
            };

            // First speech on day 1 succeeds
            bool ok1 = system.TrySelectVoiceLine(context, "ration_cut", currentDay: 1, rng: null, out _);
            Assert.True(ok1);

            // Second speech on same day 1 is rejected by survivor-level cooldown
            bool ok2 = system.TrySelectVoiceLine(context, "ration_cut", currentDay: 1, rng: null, out _);
            Assert.False(ok2);

            // Speech on day 2 is permitted
            bool ok3 = system.TrySelectVoiceLine(context, "ration_cut", currentDay: 2, rng: null, out _);
            // But line cooldown is 2 days (1 + 2 = 3), so line itself is in cooldown
            Assert.False(ok3);

            // Speech on day 3 succeeds (cooldown satisfied)
            bool ok4 = system.TrySelectVoiceLine(context, "ration_cut", currentDay: 3, rng: null, out _);
            Assert.True(ok4);
        }

        [Fact]
        public void SeamInvocation_FiresDeliveredSinkAndAudioBark()
        {
            var system = new SurvivorVoiceSystem();
            system.RegisterLine(new VoiceLineDefinition
            {
                Id = "line_rad_01",
                Speaker = "medic",
                Trigger = "radiation_spike",
                Register = "clipped",
                TextKey = "voice.rad.01",
                TextEnglish = "Dosimeters screaming in corridor six.",
                CooldownDays = 1
            });

            VoiceLinePayload? deliveredPayload = null;
            string? audioSurvivor = null;
            string? audioLine = null;

            system.VoiceLineDeliveredSink = p => deliveredPayload = p;
            system.VoiceBarkAudioSeam = (survId, lineId) =>
            {
                audioSurvivor = survId;
                audioLine = lineId;
            };

            var context = new SurvivorSpeechContext
            {
                SurvivorId = "surv_doc",
                Profession = "medic"
            };

            bool ok = system.TrySelectVoiceLine(context, "radiation_spike", currentDay: 5, rng: null, out var payload);

            Assert.True(ok);
            Assert.NotNull(deliveredPayload);
            Assert.Equal("line_rad_01", deliveredPayload.LineId);
            Assert.Equal("surv_doc", audioSurvivor);
            Assert.Equal("line_rad_01", audioLine);
        }

        [Fact]
        public void StateCaptureAndRestore_RoundTripsAccurately()
        {
            var sys1 = new SurvivorVoiceSystem();
            sys1.RegisterLine(new VoiceLineDefinition
            {
                Id = "line_test",
                Speaker = "any",
                Trigger = "test_event",
                TextKey = "voice.test",
                TextEnglish = "Test speech line.",
                CooldownDays = 1
            });

            sys1.TrySelectVoiceLine(new SurvivorSpeechContext { SurvivorId = "surv_alpha", Profession = "guard" }, "test_event", 1, null, out _);

            var state = sys1.CaptureState();
            string json = JsonSerializer.Serialize(state);
            var deserialized = JsonSerializer.Deserialize<SurvivorVoiceState>(json);
            Assert.NotNull(deserialized);

            var sys2 = new SurvivorVoiceSystem();
            sys2.RestoreState(deserialized);

            Assert.Single(sys2.History);
            Assert.Equal("line_test", sys2.History[0].LineId);
            Assert.Equal("surv_alpha", sys2.History[0].SpeakerSurvivorId);
        }
    }
}
