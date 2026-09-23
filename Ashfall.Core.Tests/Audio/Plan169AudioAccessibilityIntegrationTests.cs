// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests.Plan169AudioAccessibility
{
    public sealed class Plan169AudioAccessibilityIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/audio_accessibility_cues.json");
            if (File.Exists(path))
                return File.ReadAllText(path);

            string altPath = "Assets/StreamingAssets/Data/audio_accessibility_cues.json";
            if (File.Exists(altPath))
                return File.ReadAllText(altPath);

            return string.Empty;
        }

        [Fact]
        public void Catalog_LoadsSuccessfully_CuesAndPresetsPopulated()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                coordinator.LoadCatalog(json);

            Assert.True(coordinator.Cues.Count >= 7, "Expected at least 7 accessibility cues");
            Assert.True(coordinator.Presets.Count >= 3, "Expected 3 acoustic presets");
            Assert.Contains("cue_alarm_general", coordinator.Cues.Keys);
            Assert.Contains("cue_raid_incoming", coordinator.Cues.Keys);
            Assert.Contains("preset_reduced_stimulation", coordinator.Presets.Keys);
        }

        [Fact]
        public void TriggerCue_EmitsVisualNotification_And_AppliesDucking()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                coordinator.LoadCatalog(json);

            VisualAudioNotification? emitted = null;
            float duckReported = 0f;
            coordinator.OnVisualNotificationEmittedSeam = n => emitted = n;
            coordinator.OnDuckingChangedSeam = d => duckReported = d;

            bool triggered = coordinator.TriggerCue("cue_raid_incoming", 10.0, out var notif);

            Assert.True(triggered);
            Assert.NotNull(emitted);
            Assert.NotNull(notif);
            Assert.Equal("cue_raid_incoming", notif!.CueId);
            Assert.Equal("Critical", notif.Severity);
            Assert.Contains("Armed raiders", notif.VisualLabel);
            Assert.Equal(-14.0f, coordinator.ActiveDuckingDb);
            Assert.Equal(-14.0f, duckReported);
        }

        [Fact]
        public void AlertCoalescing_SuppressesRapidDuplicateAlerts()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                coordinator.LoadCatalog(json);

            // cue_alarm_general has 3.0s coalesce window
            bool first = coordinator.TriggerCue("cue_alarm_general", 100.0, out var notif1);
            bool second = coordinator.TriggerCue("cue_alarm_general", 101.0, out var notif2); // within window -> coalesced
            bool third = coordinator.TriggerCue("cue_alarm_general", 104.0, out var notif3); // after window -> permitted

            Assert.True(first);
            Assert.False(second);
            Assert.Null(notif2);
            Assert.True(third);
            Assert.NotNull(notif3);
            Assert.Equal(1, coordinator.CoalescedAlertCount);
        }

        [Fact]
        public void DuckingRelease_RestoresAttenuationToZero()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            coordinator.TriggerCue("cue_alarm_general", 10.0, out _);
            Assert.True(coordinator.ActiveDuckingDb < 0f);

            float releasedDb = -999f;
            coordinator.OnDuckingChangedSeam = d => releasedDb = d;
            coordinator.ReleaseDucking();

            Assert.Equal(0.0f, coordinator.ActiveDuckingDb);
            Assert.Equal(0.0f, releasedDb);
        }

        [Fact]
        public void MixPresets_CanBeAppliedAndRetrieved()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            string json = GetCatalogJson();
            if (!string.IsNullOrEmpty(json))
                coordinator.LoadCatalog(json);

            string presetEmitted = string.Empty;
            coordinator.OnMixPresetChangedSeam = p => presetEmitted = p;

            bool applied = coordinator.ApplyPreset("preset_reduced_stimulation");
            Assert.True(applied);
            Assert.Equal("preset_reduced_stimulation", coordinator.ActivePresetId);
            Assert.Equal("preset_reduced_stimulation", presetEmitted);

            var readout = coordinator.GetDiagnosticReadout();
            Assert.Equal("preset_reduced_stimulation", readout.CurrentPresetId);
            Assert.Contains("Alert", readout.ActiveBuses);
        }

        [Fact]
        public void DiagnosticReadout_AccuratelySummarizesState()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            coordinator.TriggerCue("cue_low_power_warning", 50.0, out _);
            var readout = coordinator.GetDiagnosticReadout();

            Assert.NotNull(readout.LastNotification);
            Assert.Equal("cue_low_power_warning", readout.LastNotification!.CueId);
            Assert.Equal("Warning", readout.LastNotification.Severity);
            Assert.NotEmpty(readout.ActiveBuses);
        }
    }
}
